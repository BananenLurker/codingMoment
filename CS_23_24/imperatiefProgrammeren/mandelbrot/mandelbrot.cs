using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

// Global variables

int mandelGrootte = 500; // For better looking image, use 800. For ~60% faster computing, use 500.
int clientBreedte = mandelGrootte + 100;
int clientHoogte = mandelGrootte + 200;
Stopwatch timer = new Stopwatch();

// Colors

Color darkestGreen = Color.FromArgb(52, 78, 65);
Color green1 = Color.FromArgb(58, 90, 64);
Color green2 = Color.FromArgb(88, 129, 87);
Color green3 = Color.FromArgb(163, 177, 138);
Color lightestGreen = Color.FromArgb(233, 237, 201);

Color lighterGreen = ColorTranslator.FromHtml("#3a7415");
Color darkMagenta = ColorTranslator.FromHtml("#230513");
Color forestGreen = ColorTranslator.FromHtml("#092f0d");

Color[] zwartWitteLijst = new Color[] { Color.Black, Color.White };
Color[] groeneLijst = new Color[] { darkestGreen, green1, green2, green3, lightestGreen };
Color[] funkyLijst = new Color[] { Color.Black, Color.Orange, Color.Purple, Color.Yellow };

Color[] kleurenLijst = zwartWitteLijst;

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
    Location = new Point(mandelGrootte - 125, mandelGrootte + 150),
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

Button kleurenGenButton = new Button
{
    Size = new Size(125, 25),
    Location = new Point(350, 120),
    Text = "Kleuren generatie"
};

Button kleurenFunkyButton = new Button
{
    Size = new Size(60, 20),
    Location = new Point(180, 125),
    Text = "Kleuren1"
};

Button kleurenGroenButton = new Button
{
    Size = new Size(60, 20),
    Location = new Point(250, 125),
    Text = "Kleuren2"
};

ContextMenuStrip presetStrip = new ContextMenuStrip();
ToolStripMenuItem toolStrip1 = new ToolStripMenuItem();
ToolStripMenuItem toolStrip2 = new ToolStripMenuItem();
ToolStripMenuItem toolStrip3 = new ToolStripMenuItem();
ToolStripMenuItem toolStrip4 = new ToolStripMenuItem();

ToolStripMenuItem lastToolStrip = toolStrip1;

toolStrip1.Text = "Preset 1";
toolStrip2.Text = "Preset 2";
toolStrip3.Text = "Preset 3";
toolStrip4.Text = "Preset 4";

presetStrip.Items.AddRange(new ToolStripItem[] {toolStrip1, toolStrip2, toolStrip3, toolStrip4});

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
scherm.Controls.Add(kleurenFunkyButton);
scherm.Controls.Add(kleurenGroenButton);
scherm.Controls.Add(kleurenGenButton);

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
            string kleurenSelectie = kleurenGenButton.Text;
            if (kleurenSelectie == "Kleuren generatie")
            {
                mandelMap.SetPixel(column, row, zwartWitteLijst[mandelgetal % zwartWitteLijst.Length]);
            }
            else if (kleurenSelectie == "Kleur presets")
            {
                mandelMap.SetPixel(column, row, kleurenLijst[mandelgetal % kleurenLijst.Length]);
            }
            mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal % 100, mandelgetal % 50, mandelgetal % 10));
            //mandelMap.SetPixel(column, row, kleurenLijst[mandelgetal % kleurenLijst.Length]);
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

    lastToolStrip.Checked = false;
    toolStrip1.Checked = true;
    lastToolStrip = toolStrip1;
    mandelRekenen_Click(null, null);
};

void toolStrip2_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,108625";
    middenyBox.Text = "0,9014428";
    schaalBox.Text = "3,8147E-08";
    aantalBox.Text = "400";

    lastToolStrip.Checked = false;
    toolStrip2.Checked = true;
    lastToolStrip = toolStrip2;
    mandelRekenen_Click(null, null);
};
void toolStrip3_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,3346749973297119";
    middenyBox.Text = "0,3832778453826904";
    schaalBox.Text = "3,814697265625E-08";
    aantalBox.Text = "400";

    lastToolStrip.Checked = false;
    toolStrip3.Checked = true;
    lastToolStrip = toolStrip3;
    mandelRekenen_Click(null, null);
};

void toolStrip4_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,328020";
    middenyBox.Text = "-0,528115";
    schaalBox.Text = "2,80031E-07";
    aantalBox.Text = "200";

    lastToolStrip.Checked = false;
    toolStrip4.Checked = true;
    lastToolStrip = toolStrip4;
    mandelRekenen_Click(null, null);
};

// Color preset functions

void kleurenGroen_Click(object o, EventArgs e)
{
    kleurenLijst = groeneLijst;
    mandelRekenen_Click(null, null);
};

void kleurenFunky_Click(object o, EventArgs e)
{
    kleurenLijst = funkyLijst;
    mandelRekenen_Click(null, null);
};

// Toolstrip controls

toolStrip1.Click += toolStrip1_Click;
toolStrip2.Click += toolStrip2_Click;
toolStrip3.Click += toolStrip3_Click;
toolStrip4.Click += toolStrip4_Click;

// Other (mouse-)button controls + run on startup command

mandelButton.Click += mandelRekenen_Click;
mandelLabel.MouseClick += mandelRekenen_MouseClick;
presetButton.Click += showPresets;

kleurenGroenButton.Click += kleurenGroen_Click;
kleurenFunkyButton.Click += kleurenFunky_Click;

mandelRekenen_Click(null, null);

Application.Run(scherm);