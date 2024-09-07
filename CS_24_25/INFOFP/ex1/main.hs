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

parseTable :: [String] -> Table
parseTable [] = []
parseTable (x : xs) = words x : parseTable xs

-- | Printing

-- * Exercise 2

printLine :: [Int] -> String
printLine [] = "+"
printLine (x : xs) = "+" ++ replicate x '-' ++ printLine xs

-- * Exercise 3

printField :: Int -> String -> String
printField n s
  | length s >= n = s
  | all isDigit s = replicate (length s - n) ' ' ++ s
  | otherwise = s ++ replicate (length s - n) ' '

-- * Exercise 4

printRow :: [(Int, String)] -> String
printRow [] = "|"
printRow ((x, str) : xs) = "|" ++ printField x str ++ printRow xs

-- * Exercise 5

columnWidths :: Table -> [Int]
columnWidths table = map (maximum . map length) (transpose table)

-- * Exercise 6

printTable :: Table -> [String]
printTable table@(header : rows) =
  [printLine cWidth, printRow (zip cWidth header)]
    ++ [printLine cWidth]
    ++ map (printRow . zip cWidth) rows
    ++ [printLine cWidth]
  where
    cWidth = columnWidths table

-- | Querying

-- * Exercise 7

select :: Field -> Field -> Table -> Table
select column value table@(header : rows) =
  undefined

-- * Exercise 8

project :: [Field] -> Table -> Table
project columns table@(header : _) =
  undefined
