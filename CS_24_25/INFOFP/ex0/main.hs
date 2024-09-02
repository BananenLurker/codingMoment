module Main where
import Data.List
main :: IO ()
main = interact (intercalate " / " . map reverse . lines)