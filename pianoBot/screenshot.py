import pyautogui
# import os

i = 0
while(i<1):
    x, y = pyautogui.locateCenterOnScreen("./shinyHuntBot/tile.jpg", confidence = 0.99)

    pyautogui.moveTo(x, y)
    print(x, y)
    # pyautogui.leftClick

    i+=1

# print("Working dir:", os.getcwd())
# print("Files in here:", os.listdir("."))