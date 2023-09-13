import math

def restBijDeling():
    x = int(input("X: "))
    y = int(input("Y: "))
    getalVD = x // y
    getalND = x - getalVD * y
    print("Het antwoor is " + str(getalVD) + " rest " + str(getalND))

def omtrek():
    lengte = int(input("Lengte: "))
    breedte = int(input("Breedte: "))
    print("De omtrek is " + str(lengte * 2 + breedte * 2))

def diagonaal():
    l = int(input("Lengte: "))
    b = int(input("Breedte: "))
    diag = math.sqrt(l ** 2 + b ** 2)
    print("De diagonale lengte is " + str(diag))

def driewerf(woord):
    print(woord * 3)

def laatste():
    cijfers = input("Geef een paar cijfers: ")
    print(cijfers[len(cijfers) - 1])

def naLaatste():
    cijfertjes = input("Geef een paar cijfers: ")
    t = int(input("Hoeveelste cijfer? "))
    print(cijfertjes[t - 1])

def studentNummer():
    student = input("Wat is je studentnummer? ")
    checksum = student[0] * 7 + student[1] * 6 + student[2] * 5 + student[3] * 4 + student[4] * 3 + student[5] * 2 + student[6] * 1
    print(str(checksum % 11))

studentNummer()