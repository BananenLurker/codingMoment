import datetime
from datetime import date, datetime, timedelta

print("Wat is je geboortejaar?")
y = input()
print("Hoeveelste maand?")
m = input()
print("Welke dag?")
d = input()

geboorteDatum = datetime(int(y), int(m), int(d)).date()
datumNu = date.today()

verschil = datumNu - geboorteDatum
verKDag = verschil.days % 1000

vorigeVerKdag = datumNu + timedelta(days=-verKDag)
volgendeVerKdag = datumNu + timedelta(days=1000-verKDag)
volgendeKroonVerKDag = datumNu + timedelta(days=8000-verschil.days % 8000)

if(verKDag == 0):
    print("Gefeliciteerd, het is je verKdag!")
else:
    print(f"Je volgende verKdag is op {volgendeVerKdag}.")

print(f"Je vorige verKdag was op {vorigeVerKdag}.")
print(f"Je volgende kroon-verKdag is op {volgendeKroonVerKDag}.")

if(volgendeVerKdag == volgendeKroonVerKDag):
    print("Je volgende verKdag is ook je kroon-verKdag!")