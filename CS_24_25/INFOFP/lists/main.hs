module Main where

main :: IO ()
main = print(remSuccDup [1,1,2,3,4,3,3,32,2,1,1,1,2,2,1,3,4,4,4,4,4,4,1])

newProduct :: Num a => [a] -> a
newProduct [] = 1
newProduct (x:xs) = x * product xs

newConcat :: [[a]] -> [a]
newConcat [] = []
newConcat (x:xs) = x ++ newConcat xs

nAnd :: [Bool] -> Bool
nAnd [] = True
nAnd (b:bs)
  | b == True = nAnd bs
  | otherwise = False

nOr :: [Bool] -> Bool
nOr [] = False
nOr (b:bs)
  | b == True = True
  | otherwise = nOr bs

nAll :: (a -> Bool) -> [a] -> Bool
nAll _ [] = True
nAll p (x:xs)
  | p x == True = nAll p xs
  | otherwise = False

nMap :: (a -> b) -> [a] -> [b]
nMap _ [] = []
nMap f (x:xs) = [f x] ++ nMap f xs

nIntersperse :: a -> [a] -> [a]
nIntersperse c (x:xs)
  | null xs = [x]
  | otherwise = [x, c] ++ nIntersperse c xs

nConcatMap :: (a -> [b]) -> [a] -> [b]
nConcatMap _ [] = []
nConcatMap f (x:xs) = f x ++ nConcatMap f xs

nUnlines :: [String] -> String
nUnlines [] = []
nUnlines (x:xs) = x ++ nUnlines xs

nFilter :: (a -> Bool) -> [a] -> [a]
nFilter _ [] = []
nFilter p (x:xs) 
  | p x = [x] ++ nFilter p xs
  | otherwise = nFilter p xs

nPartition :: (a -> Bool) -> [a] -> ([a], [a])
nPartition _ [] = ([], [])
nPartition p (x:xs)
  | p x = (x:ts, fs)
  | otherwise = (ts, x:fs)
  where
    (ts, fs) = nPartition p xs

nUnzip :: [(a, b)] -> ([a], [b])
nUnzip [] = ([], [])
nUnzip ((x, y):xs) = (x:os, y:ss)
  where
    (os, ss) = nUnzip xs

nInsert :: Ord a => a -> [a] -> [a]
nInsert n [] = [n]
nInsert n (x:xs)
  | n <= x = n:x:xs
  | otherwise = x : nInsert n xs

sort :: Ord a => [a] -> [a]
sort [x] = [x]
sort (x:xs) = nInsert x (sort xs)

nTake :: Int -> [a] -> [a]
nTake n (x:xs)
  | n < 1 = []
  | length (x:xs) <= n = (x:xs)
  | otherwise = x : take (n-1) xs

nTakeWhile :: (a -> Bool) -> [a] -> [a]
nTakeWhile _ [] = []
nTakeWhile p (x:xs)
  | p x = x : nTakeWhile p xs
  | otherwise = []

nGroup :: Eq a => [a] -> [[a]]
nGroup [] = []
nGroup [x] = [[x]]
nGroup (x:y:xs)
  | x == y = (x:g) : gs
  | otherwise = [x] : nGroup (y:xs)
  where
    (g:gs) = nGroup (y:xs)

remSuccDup :: Eq a => [a] -> [a]
remSuccDup [] = []
remSuccDup [x] = [x]
remSuccDup (x:y:xs)
  | x == y = remSuccDup (y:xs)
  | otherwise = x : remSuccDup (y:xs)

nub :: Eq a => [a] -> [a]
nub 