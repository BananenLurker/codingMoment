using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;

// Instantiating screen
Form scherm = new Form();
scherm.Text = "T2";
scherm.BackColor = Color.Green;
scherm.ClientSize = new Size(400, 600);

//Bitmap map = new Bitmap(200, 200);

//Label afbeelding = new Label()
//{
//    Location = new Point(200, 400),
//    Size = new Size(200, 200),
//    Image = map
//};

//Graphics gr = Graphics.FromImage(map);

// GUI elements
Label label1 = new Label();
label1.Location = new Point(25, 10);
label1.Size = new Size(100, 25);
label1.BackColor = Color.LightYellow;

Label label2 = new Label();
label2.Location = new Point(150, 10);
label2.Size = new Size(100, 25);
label2.BackColor = Color.LightYellow;

Label label3 = new Label();
label3.Location = new Point(275, 10);
label3.Size = new Size(100, 25);
label3.BackColor = Color.LightYellow;

Button sizeLabelButton = new Button
{
    Size = new Size(100, 25),
    Location = new Point(150, 275),
    Text = "Click me"
};

Rectangle r1, r2, r3;
r1 = new Rectangle(50, 40, 60, 20);
r3 = r2 = r1;
r2.Offset(0, 20);
r3.Offset(0, 40);

scherm.Controls.Add(label1);
scherm.Controls.Add(label2);
scherm.Controls.Add(label3);
scherm.Controls.Add(sizeLabelButton);

// Lists
List<Label> labels = new List<Label>();
labels.Add(label1);
labels.Add(label2);
labels.Add(label3);

List<Rectangle> vierkant = new List<Rectangle>();
vierkant.Add(r1);
vierkant.Add(r2);
vierkant.Add(r3);

int[] teller = new int[50];

void klik(object o, EventArgs ea)
{
    if (o == sizeLabelButton)
    {
        vierkant.Clear();
        r1.Offset(100, 0);
        vierkant.Add(r1);
        scherm.Invalidate();
    }
    label1.Text = Macht(2, 2).ToString();
    teller[2]++;
}

int Macht(int x, int n)
{
    int res = x;
    for(int t = 1; t < n; t++)
    {
        res *= x;
    }
    return res;
}

void tekenen(object o, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;
    foreach(Rectangle r in vierkant)
    {
        gr.DrawRectangle(Pens.Black, r);
    }
    //gr.DrawRectangle(Pens.Blue, r1);
}

scherm.Paint += tekenen;
sizeLabelButton.Click += klik;

Application.Run(scherm);