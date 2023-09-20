using System;
using System.Drawing;
using System.Windows.Forms;

// Variabelen

int maxIteraties = 1000;
int mandelgetal = 0;

double a = 0.0;
double b = 0.0;

int mandelBreedte = 400;
int mandelHoogte = 400;

// Controls en GUI

Form scherm = new Form
{
    Text = "mandelbrot",
    BackColor = Color.White,
    ClientSize = new Size(3/2 * mandelBreedte, 3/2 * mandelHoogte)
};

Bitmap mandel = new Bitmap((int)mandelBreedte, (int)mandelHoogte);

Button mandelbrot = new Button
{
    Size = new Size(60, 20),
    Location = new Point(170, 96),
    Text= "Maak!",
};
scherm.Controls.Add(mandelbrot);

Label label = new Label
{
    Size = new Size(100, 100),
    Location = new Point(100, 200)
};
scherm.Controls.Add(label);
label.Text = "label1";

Label lbeginx = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 6)
};

scherm.Controls.Add(lbeginx);
lbeginx.Text = "Midden x";

TextBox beginx = new TextBox
{
    Location = new Point(110, 6),
    Size = new Size(60, 16)
};
scherm.Controls.Add(beginx);

Label lbeginy = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 36)
};
scherm.Controls.Add(lbeginy);
lbeginy.Text = "Midden y";

TextBox beginy = new TextBox
{
    Location = new Point(110, 36),
    Size = new Size(60, 16)
};
scherm.Controls.Add(beginy);

Label lschaal = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 66)
};
scherm.Controls.Add(lschaal);
lschaal.Text = "Schaal";

TextBox schaal = new TextBox
{
    Location = new Point(110, 66),
    Size = new Size(60, 16)
};
scherm.Controls.Add(schaal);

Label laantal = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 96)
};
scherm.Controls.Add(laantal);
laantal.Text = "Max aantal: ";

TextBox aantal = new TextBox
{
    Location = new Point(110, 96),
    Size = new Size(60, 16)
};
scherm.Controls.Add(aantal);


// Berekeningen

//void mandelRekenen(object o, EventArgs e){
//    double x = Double.Parse(beginx.Text);
//    double y = Double.Parse(beginy.Text);
//    //a += 0.01;
//    //b += 0.1;
//    while (Math.Sqrt(a * a + b * b) < 2)
//    {
//    a = a * a - b * b + x;
//    b = 2 * a * b + b + y;
//    mandelgetal++;
//    label.Text = $"{mandelgetal}";
//        if (mandelgetal > maxIteraties)
//        {
//            label.Text = $"{mandelgetal}";
//            return;
//        }
//    }
//}

void mandelRekenen(object o, EventArgs e)
{
    for (int x = 0; x < maxIteraties; x++)
    {

    }
}

mandelbrot.Click += mandelRekenen;
Application.Run(scherm);