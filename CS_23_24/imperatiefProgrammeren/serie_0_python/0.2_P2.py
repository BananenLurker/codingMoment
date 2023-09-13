import math
from tkinter import Frame, Label, Button, Entry

scherm = Frame()
scherm.master.title("Studentnummer calculator")
scherm.configure(background="lightblue")
scherm.configure(width=800, height=500)
scherm.pack()

sn = Entry (scherm)
bereken = Button (scherm)
uitkomst = Label (scherm)

sn.place(x=60, y=60)
bereken.place(x=150, y=60)
uitkomst.place(x=60, y=120)

sn.configure(width=10)

bereken.configure(text="de knop")
uitkomst.configure(text="Vul je studentnummer in en klik op de knop!")

def studentnummerbereken():
    student = sn.get()
    if(len(student) == 7):
        checksum = (int(student[0]) * 7 + int(student[1]) * 6 + int(student[2]) * 5 + int(student[3]) * 4 + int(student[4]) * 3 + int(student[5]) * 2 + int(student[6])) % 11
    else:
        uitkomst.configure(text="Het ingevulde nummer bevat geen 7 cijfers!")
        scherm.configure(background="#FA5B5B")
        return
    if(checksum == 0):
        uitkomst.configure(text="Je studentnummer is geldig!")
        scherm.configure(background="green")
    else:
        uitkomst.configure(text="Je studentnummer is niet geldig! Rest: " + f"{checksum}")
        scherm.configure(background="red")
    

bereken.configure(command=studentnummerbereken)

scherm.mainloop()