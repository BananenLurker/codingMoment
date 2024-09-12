module Main where

{- This is a framework in which all functions to be written are "undefined".  -
 - Note that in most cases parameters, pattern-matching and guards have been  -
 - omitted! You will have to add those yourself.                              -}

import Data.Char
import Data.List
import Data.Maybe

-- | Model
type Field = String

type Row = [Field]

type Table = [Row]

-- | Main
main :: IO ()
main = interact (unlines . exercise . lines)

exercise :: [String] -> [String]
exercise =
  printTable
    . project ["last", "first", "salary"]
    . select "gender" "male"
    . parseTable

-- | Parsing

-- * Exercise 1

-- Split each string into words at the space character. This used to be a function with two helper functions,
-- then it used guards, then it was a recursive call. hlint didn't like any of that, so now it uses this weird
-- syntax, which totally implies that the function is being called on each string in [String].
parseTable :: [String] -> Table
parseTable = map words

-- | Printing

-- * Exercise 2

-- Print a line of + and - based on the column width.
printLine :: [Int] -> String
printLine [] = "+"
printLine (x : xs) = "+" ++ replicate x '-' ++ printLine xs

-- * Exercise 3

-- Format the data correctly spaced, based on the length of the column and whether or not the entire string is a digit.
-- isDigit only checks a single character, which is why the keyword 'all' is used.
printField :: Int -> String -> String
printField n s
  | length s >= n = s
  | all isDigit s = replicate (n - length s) ' ' ++ s
  | otherwise = s ++ replicate (n - length s) ' '

-- * Exercise 4

-- Use recursive.. ness(?) to print each row correctly formatted with '|' seperating the data.
-- An empty list = '|', as otherwise the table will not be closed.
printRow :: [(Int, String)] -> String
printRow [] = "|"
printRow ((n, s) : xs) = "|" ++ printField n s ++ printRow xs

-- * Exercise 5

-- Get the maximum width found in a column.
-- I don't remember why the table had to be transposed, but it was in the hints and it didn't work otherwise.
columnWidths :: Table -> [Int]
columnWidths table = map (maximum . map length) (transpose table)

-- * Exercise 6

-- Where everything ties together:
-- cWidth returns a list of integers, which are the sizes of the columns.
-- printLine and printRow are used intermitently, where printLine prints the borders and printRow prints nicely formatted data.
-- All the different lists are concatenated into one using (++).
printTable :: Table -> [String]
printTable table@(header : rows) =
  [printLine cWidth, map toUpper (printRow (zip cWidth header))]
    ++ [printLine cWidth]
    ++ map (printRow . zip cWidth) rows
    ++ [printLine cWidth]
  where
    cWidth = columnWidths table

-- | Querying

-- * Exercise 7

-- Filter checks for each row whether checkRow returns True.
-- checkRow returns True when the Field in row at the index position (!!) == the value that we are filtering (in this case, the item at the index of "gender" should equal "mae").
-- Then, header is added as the head to this filtered list.
-- 'maybe' is used to handle the cases where colIndex doesn't have a value.
select :: Field -> Field -> Table -> Table
select column value table@(header : rows) =
  maybe table (\colIndex -> header : filter (checkRow colIndex value) rows) colIndex
  where
    colIndex = elemIndex column header

checkRow :: Int -> String -> Row -> Bool
checkRow index value row = row !! index == value

-- * Exercise 8

-- This implementation still uses the values of the columns,
-- while the don't care pattern implies that this is not desired.
{-
project :: [Field] -> Table -> Table
project columns table@(header : _) =
  map filterRow table
  where
    filterRow :: Row -> Row
    filterRow row = mapMaybe (\(header, col) -> if header `elem` columns then Just col else Nothing) (zip header row)
-}

-- Transpose the table, find the headers that are in 'columns', transpose back.
-- Possibly the most straightforward implementation.
-- EDIT: except it isn't what is meant in the exercise.

-- project :: [Field] -> Table -> Table
-- project columns table@(header : _) =
--   transpose (filter (\(header : _) -> header `elem` columns) (transpose table))

-- orderTable :: [Field] -> Table -> Table
-- orderTable (x : xs) table@(header : _)
--   | [] _ = []
--   | (c : cs) =
--   where
--     tTable = transpose table

-- mapMaybe removes the Nothing entries from the Maybe Int returned by elemIndex.
-- So, we find the indices of each header requested in columns.
-- We map these indices to a transposed table, which gives us rows instead of columns, which allows us to just get a string instead of a column.
-- Use the (!!) operator to find the first occurence of each index in the transposed table.
-- Transpose the table back to its original form and return the filteres, sorted, transposed table.
project :: [Field] -> Table -> Table
project columns table@(header : _) =
  transpose (map (transpose table !!) colIndex)
  where
    colIndex = mapMaybe (`elemIndex` header) columns