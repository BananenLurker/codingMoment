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

Color regenboogRood = Color.FromArgb(247, 2, 2);
Color regenboogOranje = Color.FromArgb(247, 125, 2);
Color regenboogGeel = Color.FromArgb(247, 235, 2);
Color regenboogGroen = Color.FromArgb(2, 247, 2);
Color regenboogBlauw = Color.FromArgb(2, 2, 247);

Color kobaltBlauw = Color.FromArgb(60, 50, 149);
Color royalPaars = Color.FromArgb(99, 28, 153);
Color lichtLila = Color.FromArgb(177, 147, 196);
Color donkerPaars = Color.FromArgb(59, 0, 101);
Color blauwGroen = Color.FromArgb(52, 180, 174);

Color donkerRood = Color.FromArgb(181, 0, 12);
Color warmOranje = Color.FromArgb(247, 125, 2);
Color helderGeel = Color.FromArgb(252, 220, 75);
Color vuurRood = Color.FromArgb(251, 49, 4);
Color lichtOranje = Color.FromArgb(252, 126, 20);

Color zwart = Color.FromArgb(0, 0, 0);
Color donkerGrijs = Color.FromArgb(69, 69, 69);
Color middenGrijs = Color.FromArgb(102, 102, 102);
Color lichtGrijs = Color.FromArgb(153, 153, 153);
Color wit = Color.FromArgb(255, 255, 255);

Color boterGeel = Color.FromArgb(255, 229, 74);
Color lichtGroen = Color.FromArgb(152, 255, 110);
Color luchtBlauw = Color.FromArgb(161, 248, 255);
Color bosGroen = Color.FromArgb(0, 174, 125);
Color grasGroen = Color.FromArgb(128, 216, 25);

Color darkestGreen = Color.FromArgb(52, 78, 65);
Color green1 = Color.FromArgb(58, 90, 64);
Color green2 = Color.FromArgb(88, 129, 87);
Color green3 = Color.FromArgb(163, 177, 138);
Color lightestGreen = Color.FromArgb(233, 237, 201);

Color lighterGreen = ColorTranslator.FromHtml("#3a7415");
Color darkMagenta = ColorTranslator.FromHtml("#230513");
Color forestGreen = ColorTranslator.FromHtml("#092f0d");

Color[] zwartWitteLijst = new Color[] { Color.Black, Color.White };
Color[] regenboogLijst = new Color[] { regenboogRood, regenboogOranje, regenboogGeel, regenboogGroen, regenboogBlauw };
Color[] koeleLijst = new Color[] { kobaltBlauw, royalPaars, lichtLila, donkerPaars, blauwGroen };
Color[] vurigeLijst = new Color[] { donkerRood, warmOranje, helderGeel, vuurRood, lichtOranje };
Color[] grijzeLijst = new Color[] { zwart, donkerGrijs, middenGrijs, lichtGrijs, wit };
Color[] groeneLijst = new Color[] { boterGeel, lichtGroen, luchtBlauw, bosGroen, grasGroen };
Color[] groeneLijst2 = new Color[] { darkestGreen, green1, green2, green3, lightestGreen };
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

Label roodL = new Label
{
    Size = new Size(60, 15),
    Location = new Point(320, 15),
    Text = "R: 0",
    BackColor = Color.FromArgb(255, 190, 190)
};

Label groenL = new Label
{
    Size = new Size(60, 15),
    Location = new Point(320, 65),
    Text = "G: 0",
    BackColor = Color.FromArgb(190, 255, 190)
};

Label blauwL = new Label
{
    Size = new Size(60, 15),
    Location = new Point(320, 115),
    Text = "B: 0",
    BackColor = Color.FromArgb(190, 190, 255)
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
    Size = new Size(60, 25),
    Location = new Point(110, 120),
    Text = "Go!"
};

Button presetButton = new Button
{
    Size = new Size(125, 25),
    Location = new Point(185, 95),
    Text = "Presets"
};

Button kleurenGenButton = new Button
{
    Size = new Size(125, 25),
    Location = new Point(185, 120),
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

TrackBar roodBar = new TrackBar
{
    Size = new Size(200, 10),
    Location = new Point(360, 10),
    Maximum = 255,
    Minimum = 0
};

TrackBar groenBar = new TrackBar
{
    Size = new Size(200, 10),
    Location = new Point(360, 60),
    Maximum = 255,
    Minimum = 0
};

TrackBar blauwBar = new TrackBar
{
    Size = new Size(200, 10),
    Location = new Point(360, 110),
    Maximum = 255,
    Minimum = 0
};

ContextMenuStrip kleurenStrip = new ContextMenuStrip();
ToolStripMenuItem kleurenToolStrip1 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip2 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip3 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip4 = new ToolStripMenuItem();

ToolStripMenuItem lastKleurenToolStrip = kleurenToolStrip1;

kleurenToolStrip1.Text = "Zwart-Wit";
kleurenToolStrip2.Text = "Kleur presets";
kleurenToolStrip3.Text = "Vloeiende kleur";
kleurenToolStrip4.Text = "RGB sliders";

kleurenStrip.Items.AddRange(new ToolStripItem[] {kleurenToolStrip1, kleurenToolStrip2, kleurenToolStrip3, kleurenToolStrip4});

ContextMenuStrip presetStrip = new ContextMenuStrip();
ToolStripMenuItem presetToolStrip1 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip2 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip3 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip4 = new ToolStripMenuItem();

ToolStripMenuItem lastPresetToolStrip = presetToolStrip1;

presetToolStrip1.Text = "Preset 1";
presetToolStrip2.Text = "Preset 2";
presetToolStrip3.Text = "Preset 3";
presetToolStrip4.Text = "Preset 4";

presetStrip.Items.AddRange(new ToolStripItem[] {presetToolStrip1, presetToolStrip2, presetToolStrip3, presetToolStrip4});

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
scherm.Controls.Add(roodBar);
scherm.Controls.Add(groenBar);
scherm.Controls.Add(blauwBar);
scherm.Controls.Add(roodL);
scherm.Controls.Add(groenL);
scherm.Controls.Add(blauwL);

// Calculation variables

double xmidden = double.Parse(middenxBox.Text);
double ymidden = double.Parse(middenyBox.Text);
double maxIteraties;
double sch;
int mandelgetal;

int rood = 0;
int groen = 0;
int blauw = 0;

// Calculations

void mandelRekenen_MouseClick(object sender, MouseEventArgs mea)
{
    timer.Start();
    lastPresetToolStrip.Checked = false;
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
            else if(kleurenSelectie == "Zwart-Wit")
            {

                mandelMap.SetPixel(column, row, zwartWitteLijst[mandelgetal % zwartWitteLijst.Length]);
            }
            else if (kleurenSelectie == "Kleur presets")
            {
                mandelMap.SetPixel(column, row, kleurenLijst[mandelgetal % kleurenLijst.Length]);
            }
            else if (kleurenSelectie == "Vloeiende kleur")
            {
                mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal % 100, mandelgetal % 50, mandelgetal % 10));
            }
            else if(kleurenSelectie == "RGB sliders")
            {
                mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal * rood % 256, mandelgetal * groen % 256, mandelgetal * blauw % 256));
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

void showKleuren(object sender, EventArgs e)
{
    kleurenStrip.Show(kleurenGenButton, 0, 20);
}

void presetToolStrip1_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,3233934";
    middenyBox.Text = "0,41776";
    schaalBox.Text = "6,62939453125E-08";
    aantalBox.Text = "600";

    lastPresetToolStrip.Checked = false;
    presetToolStrip1.Checked = true;
    lastPresetToolStrip = presetToolStrip1;
    mandelRekenen_Click(null, null);
};

void presetToolStrip2_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,108625";
    middenyBox.Text = "0,9014428";
    schaalBox.Text = "3,8147E-08";
    aantalBox.Text = "400";

    lastPresetToolStrip.Checked = false;
    presetToolStrip2.Checked = true;
    lastPresetToolStrip = presetToolStrip2;
    mandelRekenen_Click(null, null);
};
void presetToolStrip3_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,3346749973297119";
    middenyBox.Text = "0,3832778453826904";
    schaalBox.Text = "3,814697265625E-08";
    aantalBox.Text = "400";

    lastPresetToolStrip.Checked = false;
    presetToolStrip3.Checked = true;
    lastPresetToolStrip = presetToolStrip3;
    mandelRekenen_Click(null, null);
};

void presetToolStrip4_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,328020";
    middenyBox.Text = "-0,528115";
    schaalBox.Text = "2,80031E-07";
    aantalBox.Text = "200";

    lastPresetToolStrip.Checked = false;
    presetToolStrip4.Checked = true;
    lastPresetToolStrip = presetToolStrip4;
    mandelRekenen_Click(null, null);
};

// Color preset functions

void kleurenGroen_Click(object o, EventArgs e)
{
    kleurenLijst = groeneLijst2;
    mandelRekenen_Click(null, null);
};

void kleurenFunky_Click(object o, EventArgs e)
{
    kleurenLijst = funkyLijst;
    mandelRekenen_Click(null, null);
};

void kleurenToolStrip1_Click(object o, EventArgs e)
{
    kleurenGenButton.Text = "Zwart-Wit";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip1.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip1;
    mandelRekenen_Click(null, null);
};

void kleurenToolStrip2_Click(object o, EventArgs e)
{
    kleurenGenButton.Text = "Kleur presets";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip2.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip2;
    mandelRekenen_Click(null, null);
};

void kleurenToolStrip3_Click(object o, EventArgs e)
{
    kleurenGenButton.Text = "Vloeiende kleur";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip3.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip3;
    mandelRekenen_Click(null, null);
};

void kleurenToolStrip4_Click(object o, EventArgs e){
    kleurenGenButton.Text = "RGB sliders";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip4.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip4;
    mandelRekenen_Click(null, null);
};

void roodBar_Scroll(object o, EventArgs e)
{
    rood = roodBar.Value;
    roodL.Text = $"R: {rood}";
}

void groenBar_Scroll(object o, EventArgs e)
{
    groen = groenBar.Value;
    groenL.Text = $"G: {groen}";
}

void blauwBar_Scroll(object o, EventArgs e)
{
    blauw = blauwBar.Value;
    blauwL.Text = $"B: {blauw}";
}

// Toolstrip and button controls for presets and colors

presetToolStrip1.Click += presetToolStrip1_Click;
presetToolStrip2.Click += presetToolStrip2_Click;
presetToolStrip3.Click += presetToolStrip3_Click;
presetToolStrip4.Click += presetToolStrip4_Click;

kleurenToolStrip1.Click += kleurenToolStrip1_Click;
kleurenToolStrip2.Click += kleurenToolStrip2_Click;
kleurenToolStrip3.Click += kleurenToolStrip3_Click;
kleurenToolStrip4.Click += kleurenToolStrip4_Click;

kleurenGroenButton.Click += kleurenGroen_Click;
kleurenFunkyButton.Click += kleurenFunky_Click;

roodBar.Scroll += roodBar_Scroll;
groenBar.Scroll += groenBar_Scroll;
blauwBar.Scroll += blauwBar_Scroll;

// Other (mouse-)button controls + run on startup command

mandelButton.Click += mandelRekenen_Click;
mandelLabel.MouseClick += mandelRekenen_MouseClick;
presetButton.Click += showPresets;
kleurenGenButton.Click += showKleuren;

mandelRekenen_Click(null, null);

Application.Run(scherm);