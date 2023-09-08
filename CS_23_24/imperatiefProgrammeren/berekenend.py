import time
from datetime import datetime

print("Wat is je naam?")

name = input()
print(f"{name.upper()}!!!")

print("Wat is je geboortedag?")
dag = int(input())
print("In welke maand?")
maand = int(input())
print("In welk jaar?")
jaar = int(input())

gebdat = datetime(jaar, maand, dag)
print(gebdat.isoweekday())