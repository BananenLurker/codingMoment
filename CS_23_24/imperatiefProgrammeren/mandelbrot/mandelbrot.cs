using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

// Global variables

Stopwatch timer = new Stopwatch();

int mandelGrootte = 500;
int clientBreedte = mandelGrootte + 100;
int clientHoogte = mandelGrootte + 200;

// GUI: instantiating and settings

Form scherm = new Form
{
    Text = "mandelbrot",
    BackColor = Color.LightGray,
    ClientSize = new Size(clientBreedte, clientHoogte)
};

Bitmap mandelMap = new Bitmap(mandelGrootte, mandelGrootte);

Label mandelLabel = new Label()
{
    Location = new Point(50, 150),
    Size = new Size(mandelGrootte, mandelGrootte),
    BackColor = Color.White,
    Image = mandelMap
};

Label middenxL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 6),
    Text = "Midden X:"
};

TextBox middenxBox = new TextBox
{
    Location = new Point(110, 6),
    Size = new Size(200, 16),
    Text = "-0,108625"
};

Label middenyL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 36),
    Text = "Midden Y:"
};

TextBox middenyBox = new TextBox
{
    Location = new Point(110, 36),
    Size = new Size(200, 16),
    Text = "0,9014428"
};

Label schaalL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 66),
    Text = "Schaal:"
};

TextBox schaalBox = new TextBox
{
    Location = new Point(110, 66),
    Size = new Size(200, 16),
    Text = "3,8147E-8"
};

Label aantalL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 96),
    Text = "Max aantal iteraties:"
};

TextBox aantalBox = new TextBox
{
    Location = new Point(110, 96),
    Size = new Size(60, 16),
    Text = "400"
};

Button mandelButton = new Button
{
    Size = new Size(60, 20),
    Location = new Point(170, 96),
    Text = "Go!"
};

Label loading = new Label
{
    Size = new Size(175, 16),
    Location = new Point(mandelGrootte - 125, 125),
    Text = "",
    BackColor = Color.White
};

// Adding controls

scherm.Controls.Add(mandelLabel);
scherm.Controls.Add(mandelButton);
scherm.Controls.Add(middenxL);
scherm.Controls.Add(middenxBox);
scherm.Controls.Add(middenyL);
scherm.Controls.Add(middenyBox);
scherm.Controls.Add(schaalL);
scherm.Controls.Add(schaalBox);
scherm.Controls.Add(aantalL);
scherm.Controls.Add(aantalBox);
scherm.Controls.Add(loading);

// Calculation variables

double xmidden = double.Parse(middenxBox.Text);
double ymidden = double.Parse(middenyBox.Text);
double maxIteraties;
double sch;
int mandelgetal;

// Calculations

void muisMandelRekenen(object sender, MouseEventArgs mea)
{
    timer.Start();
    maxIteraties = double.Parse(aantalBox.Text);
    sch = double.Parse(schaalBox.Text);
    if(mea.Button == MouseButtons.Left)
    {
        sch *= 0.5;
    }
    else if(mea.Button == MouseButtons.Right)
    {
        sch *= 2.0;
    }
    else
    {
        sch = 0.01;
        xmidden = 0;
        ymidden = 0;
        schaalBox.Text = $"{(decimal)sch}";
        middenxBox.Text = $"{xmidden}";
        middenyBox.Text = $"{ymidden}";
        mandelRekenen(maxIteraties, sch, xmidden, ymidden);
        return;
    }
    xmidden = (mea.X - mandelGrootte / 2.0) * sch + xmidden;
    ymidden = (mea.Y - mandelGrootte / 2.0) * sch + ymidden;
    schaalBox.Text = $"{sch}";
    middenxBox.Text = $"{xmidden}";
    middenyBox.Text = $"{ymidden}";
    mandelRekenen(maxIteraties, sch, xmidden, ymidden);
}

void preMandelRekenen(object o, EventArgs e)
{
    timer.Start();
    maxIteraties = double.Parse(aantalBox.Text);
    sch = double.Parse(schaalBox.Text);
    xmidden = double.Parse(middenxBox.Text);
    ymidden = double.Parse(middenyBox.Text);
    mandelRekenen(maxIteraties, sch, xmidden, ymidden);
}

void mandelRekenen(double maxIt, double s, double xm, double ym)
{
    for (int row = 0; row < mandelGrootte; row++)
    {
        mandelgetal = 0;
        for (int column = 0; column < mandelGrootte; column++)
        {
            double a = 0;
            double b = 0;

            double x = ((column - mandelGrootte / 2.0) * s + xm);
            double y = ((row - mandelGrootte / 2.0) * s + ym);

            mandelgetal = 0;

            while (mandelgetal < maxIt && Math.Sqrt(a * a + b * b) < 2)
            {
                double atijdelijk = a * a - b * b + x;
                double btijdelijk = 2 * a * b + y;
                a = atijdelijk;
                b = btijdelijk;
                mandelgetal++;
            }
            if (mandelgetal % 2 == 0)
            {
                mandelMap.SetPixel(column, row, Color.Black);
            }
            else if (mandelgetal % 3 == 0)
            {
                mandelMap.SetPixel(column, row, Color.Blue);
            }
            else
            {
                mandelMap.SetPixel(column, row, Color.DarkBlue);
            }
        }
    }

    mandelLabel.Image = mandelMap;
    scherm.Refresh();
    timer.Stop();
    loading.Text = $"Loading time: {timer.ElapsedMilliseconds} ms.";
    timer.Reset();
}

mandelButton.Click += preMandelRekenen;
mandelLabel.MouseClick += muisMandelRekenen;

preMandelRekenen(null, null);
Application.Run(scherm);