using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

// Global variables

Stopwatch timer = new Stopwatch();

int mandelGrootte = 500; // For better looking image, use 800. For ~50% faster computing, use 500.
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

Label middenyL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 36),
    Text = "Midden Y:"
};

Label schaalL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 66),
    Text = "Schaal:"
};

Label aantalL = new Label
{
    Size = new Size(60, 16),
    Location = new Point(10, 96),
    Text = "Max aantal iteraties:"
};

Label loading = new Label
{
    Size = new Size(175, 16),
    Location = new Point(mandelGrootte - 125, 125),
    Text = "",
    BackColor = Color.White
};

TextBox middenxBox = new TextBox
{
    Location = new Point(110, 6),
    Size = new Size(200, 16),
    Text = "0"
};

TextBox middenyBox = new TextBox
{
    Location = new Point(110, 36),
    Size = new Size(200, 16),
    Text = "0"
};

TextBox schaalBox = new TextBox
{
    Location = new Point(110, 66),
    Size = new Size(200, 16),
    Text = "0,008"
};

TextBox aantalBox = new TextBox
{
    Location = new Point(110, 96),
    Size = new Size(60, 16),
    Text = "100"
};

Button mandelButton = new Button
{
    Size = new Size(60, 20),
    Location = new Point(170, 96),
    Text = "Go!"
};

Button presetButton = new Button
{
    Size = new Size(60, 20),
    Location = new Point(110, 125),
    Text = "Presets"
};

ContextMenuStrip presetStrip = new ContextMenuStrip();
ToolStripMenuItem toolStrip1 = new ToolStripMenuItem();
ToolStripMenuItem toolStrip2 = new ToolStripMenuItem();
ToolStripMenuItem toolStrip3 = new ToolStripMenuItem();

toolStrip1.Text = "Preset 1";
toolStrip2.Text = "Preset 2";
toolStrip3.Text = "Preset 3";

presetStrip.Items.AddRange(new ToolStripItem[] {toolStrip1, toolStrip2, toolStrip3});

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
scherm.Controls.Add(presetButton);

// Calculation variables

double xmidden = double.Parse(middenxBox.Text);
double ymidden = double.Parse(middenyBox.Text);
double maxIteraties;
double sch;
int mandelgetal;

// Calculations

void mandelRekenen_MouseClick(object sender, MouseEventArgs mea)
{
    timer.Start();
    maxIteraties = double.Parse(aantalBox.Text);
    sch = double.Parse(schaalBox.Text);
    if(mea.Button == MouseButtons.Left)
    {
        sch *= 0.5;
        xmidden = (mea.X - mandelGrootte / 2.0) * sch + xmidden;
        ymidden = (mea.Y - mandelGrootte / 2.0) * sch + ymidden;
        schaalBox.Text = $"{sch}";
        middenxBox.Text = $"{xmidden}";
        middenyBox.Text = $"{ymidden}";
    }
    else if(mea.Button == MouseButtons.Right)
    {
        sch *= 2.0;
        schaalBox.Text = $"{sch}";
    }
    else
    {
        xmidden = 0;
        ymidden = 0;
        sch = 0.008;
        maxIteraties = 100;
        schaalBox.Text = $"{sch}";
        middenxBox.Text = $"{xmidden}";
        middenyBox.Text = $"{ymidden}";
        aantalBox.Text = $"{maxIteraties}";
    }
    mandelRekenen(maxIteraties, sch, xmidden, ymidden);
}

void mandelRekenen_Click(object o, EventArgs e)
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

            double x = (column - mandelGrootte / 2.0) * s + xm;
            double y = (row - mandelGrootte / 2.0) * s + ym;

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


// Preset functions

void showPresets(object sender, EventArgs e)
{
    presetStrip.Show(presetButton, 0, 20);
}

void toolStrip1_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,3233934";
    middenyBox.Text = "0,41776";
    schaalBox.Text = "6,62939453125E-08";
    aantalBox.Text = "600";
    mandelRekenen_Click(null, null);
};

void toolStrip2_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,108625";
    middenyBox.Text = "0,9014428";
    schaalBox.Text = "3,8147E-08";
    aantalBox.Text = "400";
    mandelRekenen_Click(null, null);
};
void toolStrip3_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,3346749973297119";
    middenyBox.Text = "0,3832778453826904";
    schaalBox.Text = "3,814697265625E-08";
    aantalBox.Text = "400";
    mandelRekenen_Click(null, null);
};

// Toolstrip controls

toolStrip1.Click += toolStrip1_Click;
toolStrip2.Click += toolStrip2_Click;
toolStrip3.Click += toolStrip3_Click;

// Other (mouse-)button controls + run on startup command

mandelButton.Click += mandelRekenen_Click;
mandelLabel.MouseClick += mandelRekenen_MouseClick;
presetButton.Click += showPresets;

mandelRekenen_Click(null, null);

Application.Run(scherm);