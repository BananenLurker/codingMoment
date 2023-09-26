using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

// Global variables

int mandelGrootte = 800; // For better looking image, use 800. For ~50% faster computing, use 500.
int clientBreedte = mandelGrootte + 100;
int clientHoogte = mandelGrootte + 250;
Stopwatch timer = new Stopwatch();
string kleurenSelectie = "Kleur presets";
bool resetButtonClicked = false;

// Colors

Color regenboogRood1 = Color.FromArgb(247, 2, 2);
Color regenboogRood2 = Color.FromArgb(242, 24, 33);
Color regenboogOranje1 = Color.FromArgb(247, 125, 2);
Color regenboogOranje2 = Color.FromArgb(248, 99, 31);
Color regenboogOranje3 = Color.FromArgb(250, 147, 26);
Color regenboogGeel1 = Color.FromArgb(255, 195, 9);
Color regenboogGeel2 = Color.FromArgb(247, 235, 2);
Color regenboogGroen1 = Color.FromArgb(205, 222, 37);
Color regenboogGroen2 = Color.FromArgb(139, 200, 59);
Color regenboogGroen3 = Color.FromArgb(2, 247, 2);
Color regenboogBlauw1 = Color.FromArgb(4, 185, 158);
Color regenboogBlauw2 = Color.FromArgb(1, 174, 243);
Color regenboogBlauw3 = Color.FromArgb(2, 2, 247);
Color regenboogPaars1 = Color.FromArgb(89, 84, 168);
Color regenboogPaars2 = Color.FromArgb(143, 89, 167);
Color regenboogPaars3 = Color.FromArgb(191, 22, 141);

Color kobaltBlauw1 = Color.FromArgb(60, 50, 149);
Color kobaltBlauw2 = Color.FromArgb(36, 51, 99);
Color kobaltBlauw3 = Color.FromArgb(97, 38, 125);
Color kobaltBlauw4 = Color.FromArgb(85, 95, 163);
Color kobaltBlauw5 = Color.FromArgb(205, 205, 224);
Color royalPaars1 = Color.FromArgb(99, 28, 153);
Color royalPaars2 = Color.FromArgb(36, 0, 84);
Color royalPaars3 = Color.FromArgb(70, 0, 139);
Color royalPaars4 = Color.FromArgb(117, 85, 163);
Color royalPaars5 = Color.FromArgb(178, 164, 201);
Color lichtLila1 = Color.FromArgb(177, 147, 196);
Color lichtLila2 = Color.FromArgb(71, 0, 128);
Color lichtLila3 = Color.FromArgb(99, 28, 153);
Color lichtLila4 = Color.FromArgb(220, 204, 228);
Color donkerPaars1 = Color.FromArgb(59, 0, 101);
Color donkerPaars2 = Color.FromArgb(48, 0, 84);
Color donkerPaars3 = Color.FromArgb(131, 72, 167);
Color blauwGroen1 = Color.FromArgb(52, 180, 174);
Color blauwGroen2 = Color.FromArgb(30, 97, 115);
Color blauwGroen3 = Color.FromArgb(46, 114, 148);
Color blauwGroen4 = Color.FromArgb(52, 132, 173);
Color blauwGroen5 = Color.FromArgb(52, 143, 182);
Color blauwGroen6 = Color.FromArgb(169, 218, 228);

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
Color boterGeel2 = Color.FromArgb(255, 244, 97);
Color lichtGroen = Color.FromArgb(152, 255, 110);
Color lichtGroen2 = Color.FromArgb(198, 249, 143);
Color luchtBlauw = Color.FromArgb(161, 248, 255);
Color luchtBlauw2 = Color.FromArgb(137, 222, 255);
Color bosGroen = Color.FromArgb(0, 174, 125);
Color bosGroen2 = Color.FromArgb(10, 187, 120);
Color grasGroen = Color.FromArgb(128, 216, 25);
Color grasGroen2 = Color.FromArgb(74, 135, 6);

Color darkestGreen = Color.FromArgb(52, 78, 65);
Color green1 = Color.FromArgb(58, 90, 64);
Color green2 = Color.FromArgb(88, 129, 87);
Color green3 = Color.FromArgb(163, 177, 138);
Color lightestGreen = Color.FromArgb(233, 237, 201);

Color lighterGreen = Color.FromArgb(28, 116, 21);
Color darkMagenta = Color.FromArgb(25, 5, 19);
Color forestGreen = Color.FromArgb(9, 47, 13);

Color emerald = Color.FromArgb(0, 152, 116);
Color tangerineTango = Color.FromArgb(221, 65, 36);
Color honeySuckle = Color.FromArgb(214, 80, 118);
Color turquoise = Color.FromArgb(68, 184, 172);
Color mimosa = Color.FromArgb(239, 192, 80);
Color blueIzis = Color.FromArgb(91, 94, 166);
Color chiliPeper = Color.FromArgb(155, 35, 53);
Color sandDollar = Color.FromArgb(223, 207, 190);
Color blueTurquoise = Color.FromArgb(85, 180, 176);
Color tigerLily = Color.FromArgb(225, 93, 68);
Color aquaSky = Color.FromArgb(127, 205, 205);
Color trueRed = Color.FromArgb(188, 36, 60);
Color fuchsiaRose = Color.FromArgb(195, 68, 122);
Color ceruleanBlue = Color.FromArgb(152, 180, 212);

Color donkerBlauw = Color.FromArgb(35, 31, 71);
Color ijsBlauw = Color.FromArgb(137, 164, 195);
Color beige = Color.FromArgb(237, 232, 225);
Color gebrokenWit = Color.FromArgb(237, 251, 255);
Color felBlauw = Color.FromArgb(160, 230, 253);
Color viezeSneeuw = Color.FromArgb(172, 167, 155);

Color pastelOranje1 = Color.FromArgb(254, 235, 201);
Color pastelOranje2 = Color.FromArgb(253, 202, 162);
Color pastelOranje3 = Color.FromArgb(252, 169, 133);
Color pastelRoze1 = Color.FromArgb(253, 222, 238);
Color pastelRoze2 = Color.FromArgb(251, 182, 209);
Color pastelRoze3 = Color.FromArgb(249, 140, 182);
Color pastelPaars1 = Color.FromArgb(221, 212, 232);
Color pastelPaars2 = Color.FromArgb(193, 179, 215);
Color pastelGroen1 = Color.FromArgb(207, 236, 207);
Color pastelGroen2 = Color.FromArgb(181, 225, 174);
Color pastelGroen3 = Color.FromArgb(145, 210, 144);
Color pastelGeel1 = Color.FromArgb(125, 255, 176);
Color pastelGeel2 = Color.FromArgb(255, 250, 129);


// Color lists

Color[] zwartWitteLijst = new Color[] { Color.Black, Color.White };
Color[] regenboogLijst = new Color[] { regenboogRood1, regenboogRood2, regenboogOranje1, regenboogOranje2, regenboogOranje3, regenboogGeel1, regenboogGeel2, regenboogGroen1, regenboogGroen2, regenboogGroen3, regenboogBlauw1, regenboogBlauw2, regenboogBlauw3, regenboogPaars1, regenboogPaars2, regenboogPaars3 };
Color[] koeleLijst = new Color[] { kobaltBlauw1, kobaltBlauw2, kobaltBlauw3, kobaltBlauw4, kobaltBlauw5, royalPaars1, royalPaars2, royalPaars3, royalPaars4, royalPaars5, lichtLila1, lichtLila2, lichtLila3, lichtLila4, donkerPaars1, donkerPaars2, donkerPaars3, blauwGroen1, blauwGroen2, blauwGroen3, blauwGroen4, blauwGroen5, blauwGroen6 };
Color[] vurigeLijst = new Color[] { donkerRood, warmOranje, helderGeel, vuurRood, lichtOranje };
Color[] grijzeLijst = new Color[] { zwart, donkerGrijs, middenGrijs, lichtGrijs, wit };
Color[] groeneLijst = new Color[] { boterGeel, lichtGroen, luchtBlauw, bosGroen, grasGroen, boterGeel2, lichtGroen2, luchtBlauw2, bosGroen2, grasGroen2 };
Color[] groeneLijst2 = new Color[] { darkestGreen, green1, green2, green3, lightestGreen };
Color[] funkyLijst = new Color[] { Color.Black, Color.Orange, Color.Purple, Color.Yellow };
Color[] pantoneLijst = new Color[] { emerald, tangerineTango, honeySuckle, turquoise, mimosa, blueIzis, chiliPeper, sandDollar, blueTurquoise, tigerLily, aquaSky, trueRed, fuchsiaRose, ceruleanBlue };
Color[] winterLijst = new Color[] { donkerBlauw, ijsBlauw, beige, gebrokenWit, felBlauw, viezeSneeuw };
Color[] lenteLijst = new Color[] { pastelGeel1, pastelGeel2, pastelGroen1, pastelGroen2, pastelGroen3, pastelOranje1, pastelOranje2, pastelOranje3, pastelPaars1, pastelPaars2, pastelRoze1, pastelRoze2, pastelRoze3 };


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
    Location = new Point((clientBreedte - mandelGrootte) / 2, 150),
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
    Location = new Point(mandelGrootte - 125, mandelGrootte + 155),
    Text = "",
    BackColor = Color.White
};

Label uitleg = new Label
{
    Size = new Size(125, 50),
    Location = new Point(50, mandelGrootte + 155),
    Text = "LMB = zoom in\nRMB = zoom out\nMMB = reset position",
    BackColor = Color.White
};

TextBox lichtDonkerBox = new TextBox
{
    Size = new Size(50, 15),
    Location = new Point(320, 65),
    Text = "1",
    BackColor = Color.LightGray
};

TextBox roodBox = new TextBox
{
    Size = new Size(50, 15),
    Location = new Point(320, 15),
    Text = "0",
    BackColor = Color.FromArgb(255, 190, 190)
};

TextBox groenBox = new TextBox
{
    Size = new Size(50, 15),
    Location = new Point(320, 65),
    Text = "0",
    BackColor = Color.FromArgb(190, 255, 190)
};

TextBox blauwBox = new TextBox
{
    Size = new Size(50, 15),
    Location = new Point(320, 115),
    Text = "0",
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

Button kleurenButton1 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(320, 0),
    Text = "Winter",
    Visible = false
};

Button kleurenButton2 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(377, 0),
    Text = "Lente",
    Visible = false
};

Button kleurenButton3 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(434, 0),
    Text = "Zomer",
    Visible = false
};

Button kleurenButton4 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(491, 0),
    Text = "Herfst",
    Visible = false
};

Button kleurenButton5 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(320, 25),
    Text = "Rboog",
    Visible = false
};

Button kleurenButton6 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(377, 25),
    Text = "Koel",
    Visible = false
};

Button kleurenButton7 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(434, 25),
    Text = "Vurig",
    Visible = false
};

Button kleurenButton8 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(491, 25),
    Text = "Grijs",
    Visible = false
};

Button kleurenButton9 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(320, 50),
    Text = "Funky",
    Visible = false
};

Button kleurenButton10 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(377, 50),
    Text = "Lgroen",
    Visible = false
};

Button kleurenButton11 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(434, 50),
    Text = "Zw/Wit",
    Visible = false
};

Button kleurenButton12 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(491, 50),
    Text = "Kantoor",
    Visible = false
};

Button kleurenButton13 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(320, 75),
    Text = "Rood",
    Visible = false
};

Button kleurenButton14 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(377, 75),
    Text = "Gerbil",
    Visible = false
};

Button kleurenButton15 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(434, 75),
    Text = "Blurple",
    Visible = false
};

Button kleurenButton16 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(491, 75),
    Text = "Jurk",
    Visible = false
};

Button kleurenButton17 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(320, 100),
    Text = "Groen",
    Visible = false
};

Button kleurenButton18 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(377, 100),
    Text = "Barbie",
    Visible = false
};

Button kleurenButton19 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(434, 100),
    Text = "Plant",
    Visible = false
};

Button kleurenButton20 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(491, 100),
    Text = "Papier",
    Visible = false
};

Button kleurenButton21 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(320, 125),
    Text = "Blauw",
    Visible = false
};

Button kleurenButton22 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(377, 125),
    Text = "Error",
    Visible = false
};

Button kleurenButton23 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(434, 125),
    Text = "Groente",
    Visible = false
};

Button kleurenButton24 = new Button
{
    Size = new Size(57, 25),
    Location = new Point(491, 125),
    Text = "Koffie",
    Visible = false
};

Button mandelResetButton = new Button
{
    Size = new Size(100, 25),
    Location = new Point(10, 120),
    Text = "Reset app",
};

Button up = new Button
{
    Size = new Size(25, 25),
    Location = new Point(clientBreedte / 2 - 25, clientHoogte - 100),
    Text = "U"
};

Button down = new Button
{
    Size = new Size(25, 25),
    Location = new Point(clientBreedte / 2 - 25, clientHoogte - 50),
    Text = "D"
};

Button left = new Button
{
    Size = new Size(25, 25),
    Location = new Point(clientBreedte / 2 - 50, clientHoogte - 75),
    Text = "L"
};

Button right = new Button
{
    Size = new Size(25, 25),
    Location = new Point(clientBreedte / 2, clientHoogte - 75),
    Text = "R"
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

TrackBar lichtDonkerBar = new TrackBar
{
    Size = new Size(200, 10),
    Location = new Point(360, 60),
    Maximum = 255,
    Minimum = 1
};

// Context menus and toolstrip menu items: instantiating and settings

ContextMenuStrip kleurenStrip = new ContextMenuStrip();
ToolStripMenuItem kleurenToolStrip0 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip1 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip2 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip3 = new ToolStripMenuItem();
ToolStripMenuItem kleurenToolStrip4 = new ToolStripMenuItem();

ToolStripMenuItem lastKleurenToolStrip = kleurenToolStrip0;

kleurenToolStrip1.Text = "Kleur presets";
kleurenToolStrip2.Text = "Donker - Licht";
kleurenToolStrip3.Text = "RGB sliders";

kleurenStrip.Items.AddRange(new ToolStripItem[] { kleurenToolStrip1, kleurenToolStrip2, kleurenToolStrip3, kleurenToolStrip4 });

ContextMenuStrip presetStrip = new ContextMenuStrip();
ToolStripMenuItem presetToolStrip0 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip1 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip2 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip3 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip4 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip5 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip6 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip7 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip8 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip9 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip10 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip11 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip12 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip13 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip14 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip15 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip16 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip17 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip18 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip19 = new ToolStripMenuItem();
ToolStripMenuItem presetToolStrip20 = new ToolStripMenuItem();

ToolStripMenuItem lastPresetToolStrip = presetToolStrip0;

presetToolStrip0.Text = $"{presetButton.Text}";
presetToolStrip1.Text = "Spiraal";
presetToolStrip2.Text = "Scheur";
presetToolStrip3.Text = "Aderen";
presetToolStrip4.Text = "Pupil";
presetToolStrip5.Text = "Zeepaard";
presetToolStrip6.Text = "Orkaan";
presetToolStrip7.Text = "Droste";
presetToolStrip8.Text = "Achtbaan";
presetToolStrip9.Text = "Tentakel";
presetToolStrip10.Text = "Wervelkolom";
presetToolStrip11.Text = "Stelsel";
presetToolStrip12.Text = "Kust";
presetToolStrip13.Text = "Baai";
presetToolStrip14.Text = "Zenuwen";
presetToolStrip15.Text = "Zoom";
presetToolStrip16.Text = "Straaljager";
presetToolStrip17.Text = "Barst";
presetToolStrip18.Text = "Ster";
presetToolStrip19.Text = "Infectie";
presetToolStrip20.Text = "Jelly Bean";

presetStrip.Items.AddRange(new ToolStripItem[] { presetToolStrip1, presetToolStrip2, presetToolStrip3, presetToolStrip4, presetToolStrip5, presetToolStrip6, presetToolStrip7, presetToolStrip8, presetToolStrip9, presetToolStrip10, presetToolStrip11, presetToolStrip12, presetToolStrip13, presetToolStrip14, presetToolStrip15, presetToolStrip16, presetToolStrip17, presetToolStrip18, presetToolStrip19, presetToolStrip20 });

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
scherm.Controls.Add(roodBar);
scherm.Controls.Add(groenBar);
scherm.Controls.Add(blauwBar);
scherm.Controls.Add(roodBox);
scherm.Controls.Add(groenBox);
scherm.Controls.Add(blauwBox);
scherm.Controls.Add(lichtDonkerBar);
scherm.Controls.Add(lichtDonkerBox);
scherm.Controls.Add(kleurenButton1);
scherm.Controls.Add(kleurenButton2);
scherm.Controls.Add(kleurenButton3);
scherm.Controls.Add(kleurenButton4);
scherm.Controls.Add(kleurenButton5);
scherm.Controls.Add(kleurenButton6);
scherm.Controls.Add(kleurenButton7);
scherm.Controls.Add(kleurenButton8);
scherm.Controls.Add(kleurenButton9);
scherm.Controls.Add(kleurenButton10);
scherm.Controls.Add(kleurenButton11);
scherm.Controls.Add(kleurenButton12);
scherm.Controls.Add(kleurenButton13);
scherm.Controls.Add(kleurenButton14);
scherm.Controls.Add(kleurenButton15);
scherm.Controls.Add(kleurenButton16);
scherm.Controls.Add(kleurenButton17);
scherm.Controls.Add(kleurenButton18);
scherm.Controls.Add(kleurenButton19);
scherm.Controls.Add(kleurenButton20);
scherm.Controls.Add(kleurenButton21);
scherm.Controls.Add(kleurenButton22);
scherm.Controls.Add(kleurenButton23);
scherm.Controls.Add(kleurenButton24);
scherm.Controls.Add(kleurenGenButton);
scherm.Controls.Add(mandelResetButton);
scherm.Controls.Add(uitleg);
scherm.Controls.Add(up);
scherm.Controls.Add(down);
scherm.Controls.Add(left);
scherm.Controls.Add(right);

// Declaring calculation variables

double xmidden = double.Parse(middenxBox.Text);
double ymidden = double.Parse(middenyBox.Text);
double maxIteraties;
double sch;
int mandelgetal;

int rood = 0;
int groen = 0;
int blauw = 0;

int ld = 1;

// Setting up variables for the mandelbrot calculation

void mandelRekenen_MouseClick(object sender, MouseEventArgs mea)
{
    timer.Start();
    maxIteraties = double.Parse(aantalBox.Text);
    sch = double.Parse(schaalBox.Text);
    if (mea.Button == MouseButtons.Left)
    {
        sch *= 0.5;
        xmidden = (mea.X - mandelGrootte / 2.0) * sch + xmidden;
        ymidden = (mea.Y - mandelGrootte / 2.0) * sch + ymidden;
        schaalBox.Text = $"{sch}";
        middenxBox.Text = $"{xmidden}";
        middenyBox.Text = $"{ymidden}";
    }
    else if (mea.Button == MouseButtons.Right)
    {
        sch *= 2.0;
        schaalBox.Text = $"{sch}";
    }
    else
    {
        resetButtonClicked = true;
        presetButton.Text = "Presets";
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

// Mandelbrot calculation

void mandelRekenen(double maxIt, double s, double xm, double ym)
{
    if(resetButtonClicked == false)
    {
        presetButton.Text = $"{lastPresetToolStrip}";
    }
    resetButtonClicked = false;
    try
    {
        Int32.Parse(lichtDonkerBox.Text);
        Int32.Parse(roodBox.Text);
        Int32.Parse(groenBox.Text);
        Int32.Parse(blauwBox.Text);

    }
    catch(Exception)
    {
        return;
    }
    ld = Int32.Parse(lichtDonkerBox.Text);
    rood = Int32.Parse(roodBox.Text);
    groen = Int32.Parse(groenBox.Text);
    blauw = Int32.Parse(blauwBox.Text);
    if (ld > 255 || ld < 0 || rood > 255 || rood < 0 || groen > 255 || groen < 0 || blauw > 255 || blauw < 0)
    {
        return;
    }
    lichtDonkerBar.Value = ld;
    roodBar.Value = rood;
    groenBar.Value = groen;
    blauwBar.Value = blauw;
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

            // Color generation selection
            if (kleurenSelectie == "Kleur presets")
            {
                mandelMap.SetPixel(column, row, kleurenLijst[mandelgetal % kleurenLijst.Length]);
            }
            else if (kleurenSelectie == "Herfst")
            {
                mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal % 100, mandelgetal % 50, mandelgetal % 10));
            }
            else if (kleurenSelectie == "RGB sliders")
            {
                mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal * rood % 256, mandelgetal * groen % 256, mandelgetal * blauw % 256));
            }
            else if (kleurenSelectie == "Donker - Licht")
            {
                mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal % ld, mandelgetal % ld, mandelgetal % ld));
            }
            else if (kleurenSelectie == "Vloeiend")
            {
                if (mandelgetal * rood < 255 && mandelgetal * groen < 255 && mandelgetal * blauw < 255)
                {
                    mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal, mandelgetal, mandelgetal));
                }
                else
                {
                    mandelMap.SetPixel(column, row, Color.FromArgb(255 - ((mandelgetal * rood) % 256), 255 - ((mandelgetal * groen) % 256), 255 - ((mandelgetal * blauw)) % 256));
                }
                mandelMap.SetPixel(column, row, Color.FromArgb(mandelgetal % ld, mandelgetal % ld, mandelgetal % ld));
            }
        }
        
    }
    mandelLabel.Image = mandelMap;
    scherm.Refresh();
    timer.Stop();
    loading.Text = $"Loading time: {timer.ElapsedMilliseconds} ms.";
    timer.Reset();
}

void mandelReset_Click(object o, EventArgs e)
{
    hideLichtDonker();
    hideRGB();
    hideColorPreset();
    presetButton.Text = "Presets";
    kleurenGenButton.Text = "Kleuren generatie";
    xmidden = 0;
    ymidden = 0;
    sch = 0.008;
    maxIteraties = 100;
    schaalBox.Text = $"{sch}";
    middenxBox.Text = $"{xmidden}";
    middenyBox.Text = $"{ymidden}";
    aantalBox.Text = $"{maxIteraties}";
    lastKleurenToolStrip.Checked = false;
    lastPresetToolStrip.Checked = false;
    resetButtonClicked = true;
    kleurenButton11_Click(null, null);
}

// UI visibility methods

void hideLichtDonker()
{
    lichtDonkerBar.Visible = false;
    lichtDonkerBox.Visible = false;
}

void hideRGB()
{
    roodBar.Visible = false;
    groenBar.Visible = false;
    blauwBar.Visible = false;
    roodBox.Visible = false;
    groenBox.Visible = false;
    blauwBox.Visible = false;
}

void hideColorPreset()
{
    kleurenButton1.Visible = false;
    kleurenButton2.Visible = false;
    kleurenButton3.Visible = false;
    kleurenButton4.Visible = false;
    kleurenButton5.Visible = false;
    kleurenButton6.Visible = false;
    kleurenButton7.Visible = false;
    kleurenButton8.Visible = false;
    kleurenButton9.Visible = false;
    kleurenButton10.Visible = false;
    kleurenButton11.Visible = false;
    kleurenButton12.Visible = false;
    kleurenButton13.Visible = false;
    kleurenButton14.Visible = false;
    kleurenButton15.Visible = false;
    kleurenButton16.Visible = false;
    kleurenButton17.Visible = false;
    kleurenButton18.Visible = false;
    kleurenButton19.Visible = false;
    kleurenButton20.Visible = false;
    kleurenButton21.Visible = false;
    kleurenButton22.Visible = false;
    kleurenButton23.Visible = false;
    kleurenButton24.Visible = false;
}

void showLichtDonker()
{
    lichtDonkerBar.Visible = true;
    lichtDonkerBox.Visible = true;
}

void showRGB()
{
    roodBar.Visible = true;
    groenBar.Visible = true;
    blauwBar.Visible = true;
    roodBox.Visible = true;
    groenBox.Visible = true;
    blauwBox.Visible = true;
}

void showColorPreset()
{
    kleurenButton1.Visible = true;
    kleurenButton2.Visible = true;
    kleurenButton3.Visible = true;
    kleurenButton4.Visible = true;
    kleurenButton5.Visible = true;
    kleurenButton6.Visible = true;
    kleurenButton7.Visible = true;
    kleurenButton8.Visible = true;
    kleurenButton9.Visible = true;
    kleurenButton10.Visible = true;
    kleurenButton11.Visible = true;
    kleurenButton12.Visible = true;
    kleurenButton13.Visible = true;
    kleurenButton14.Visible = true;
    kleurenButton15.Visible = true;
    kleurenButton16.Visible = true;
    kleurenButton17.Visible = true;
    kleurenButton18.Visible = true;
    kleurenButton19.Visible = true;
    kleurenButton20.Visible = true;
    kleurenButton21.Visible = true;
    kleurenButton22.Visible = true;
    kleurenButton23.Visible = true;
    kleurenButton24.Visible = true;
}

// Toolstrip methods
// Presets

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

void presetToolStrip5_Click(object o, EventArgs e)
{
    middenxBox.Text = "-7,44839761E-1";
    middenyBox.Text = "12,4224001E-2";
    schaalBox.Text = "6,25E-6";
    aantalBox.Text = "1000";

    lastPresetToolStrip.Checked = false;
    presetToolStrip5.Checked = true;
    lastPresetToolStrip = presetToolStrip5;
    mandelRekenen_Click(null, null);
};

void presetToolStrip6_Click(object o, EventArgs e)
{
    middenxBox.Text = "-4,601222E-1";
    middenyBox.Text = "5,702860E-1";
    schaalBox.Text = "2,009600E-5";
    aantalBox.Text = "384";

    lastPresetToolStrip.Checked = false;
    presetToolStrip6.Checked = true;
    lastPresetToolStrip = presetToolStrip6;
    mandelRekenen_Click(null, null);
};

void presetToolStrip7_Click(object o, EventArgs e)
{
    middenxBox.Text = "-1,373547";
    middenyBox.Text = "-1,234831E-2";
    schaalBox.Text = "7,054904E-4";
    aantalBox.Text = "256";

    lastPresetToolStrip.Checked = false;
    presetToolStrip7.Checked = true;
    lastPresetToolStrip = presetToolStrip7;
    mandelRekenen_Click(null, null);
};

void presetToolStrip8_Click(object o, EventArgs e)
{
    middenxBox.Text = "-7,445366E-1";
    middenyBox.Text = "12,17208E-2";
    schaalBox.Text = "5E-4";
    aantalBox.Text = "900";

    lastPresetToolStrip.Checked = false;
    presetToolStrip8.Checked = true;
    lastPresetToolStrip = presetToolStrip8;
    mandelRekenen_Click(null, null);
};

void presetToolStrip9_Click(object o, EventArgs e)
{
    middenxBox.Text = "-7,44839761E-1";
    middenyBox.Text = "12,1724001E-2";
    schaalBox.Text = "6,25E-6";
    aantalBox.Text = "1000";

    lastPresetToolStrip.Checked = false;
    presetToolStrip9.Checked = true;
    lastPresetToolStrip = presetToolStrip9;
    mandelRekenen_Click(null, null);
};

void presetToolStrip10_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,6";
    middenyBox.Text = "0,658249";
    schaalBox.Text = "9,2583E-5";
    aantalBox.Text = "512";

    lastPresetToolStrip.Checked = false;
    presetToolStrip10.Checked = true;
    lastPresetToolStrip = presetToolStrip10;
    mandelRekenen_Click(null, null);
};

void presetToolStrip11_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,461701";
    middenyBox.Text = "-0,583460248";
    schaalBox.Text = "1,607365E-9";
    aantalBox.Text = "256";

    lastPresetToolStrip.Checked = false;
    presetToolStrip11.Checked = true;
    lastPresetToolStrip = presetToolStrip11;
    mandelRekenen_Click(null, null);
};

void presetToolStrip12_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,46318684";
    middenyBox.Text = "-0,56710204";
    schaalBox.Text = "6,28E-6";
    aantalBox.Text = "384";

    lastPresetToolStrip.Checked = false;
    presetToolStrip12.Checked = true;
    lastPresetToolStrip = presetToolStrip12;
    mandelRekenen_Click(null, null);
};

void presetToolStrip13_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,7501222";
    middenyBox.Text = "0,914059E-2";
    schaalBox.Text = "3,0517578125E-8";
    aantalBox.Text = "512";

    lastPresetToolStrip.Checked = false;
    presetToolStrip13.Checked = true;
    lastPresetToolStrip = presetToolStrip13;
    mandelRekenen_Click(null, null);
};

void presetToolStrip14_Click(object o, EventArgs e)
{
    middenxBox.Text = "-0,1039375";
    middenyBox.Text = "-0,9194375";
    schaalBox.Text = "-6,25E-5";
    aantalBox.Text = "256";

    lastPresetToolStrip.Checked = false;
    presetToolStrip14.Checked = true;
    lastPresetToolStrip = presetToolStrip14;
    mandelRekenen_Click(null, null);
};

void presetToolStrip15_Click(object o, EventArgs e)
{
    middenxBox.Text = "2,5884375E-1";
    middenyBox.Text = "1,875E-4";
    schaalBox.Text = "1,5625E-5";
    aantalBox.Text = "512";

    lastPresetToolStrip.Checked = false;
    presetToolStrip15.Checked = true;
    lastPresetToolStrip = presetToolStrip15;
    mandelRekenen_Click(null, null);
};

void presetToolStrip16_Click(object o, EventArgs e)
{
    middenxBox.Text = "-1,679";
    middenyBox.Text = "-5,0E-4";
    schaalBox.Text = "5E-4";
    aantalBox.Text = "256";

    lastPresetToolStrip.Checked = false;
    presetToolStrip16.Checked = true;
    lastPresetToolStrip = presetToolStrip16;
    mandelRekenen_Click(null, null);
};

void presetToolStrip17_Click(object o, EventArgs e)
{
    middenxBox.Text = "-1,26621875";
    middenyBox.Text = "0,4138828125";
    schaalBox.Text = "9,765625E-7";
    aantalBox.Text = "125";

    lastPresetToolStrip.Checked = false;
    presetToolStrip17.Checked = true;
    lastPresetToolStrip = presetToolStrip17;
    mandelRekenen_Click(null, null);
};

void presetToolStrip18_Click(object o, EventArgs e)
{
    middenxBox.Text = "-1,26589552937448";
    middenyBox.Text = "0,4140791503190994";
    schaalBox.Text = "1,4901101193847657E-11";
    aantalBox.Text = "125";

    lastPresetToolStrip.Checked = false;
    presetToolStrip18.Checked = true;
    lastPresetToolStrip = presetToolStrip18;
    mandelRekenen_Click(null, null);
};

void presetToolStrip19_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,3510585937499999";
    middenyBox.Text = "-0,5814804687500001";
    schaalBox.Text = "1,5625E-05";
    aantalBox.Text = "175";

    lastPresetToolStrip.Checked = false;
    presetToolStrip19.Checked = true;
    lastPresetToolStrip = presetToolStrip19;
    mandelRekenen_Click(null, null);
};

void presetToolStrip20_Click(object o, EventArgs e)
{
    middenxBox.Text = "0,352017038345337";
    middenyBox.Text = "-0,5804430749714374";
    schaalBox.Text = "2,980232238769531E-11";
    aantalBox.Text = "175";

    lastPresetToolStrip.Checked = false;
    presetToolStrip20.Checked = true;
    lastPresetToolStrip = presetToolStrip20;
    mandelRekenen_Click(null, null);
};

// Toolstrip methods
// Colour gen selection

void kleurenToolStrip1_Click(object o, EventArgs e)
{
    if (lastKleurenToolStrip != kleurenToolStrip1)
    {
        hideLichtDonker();
        hideRGB();
        showColorPreset();
    }
    hideLichtDonker();
    kleurenGenButton.Text = "Kleur presets";
    kleurenSelectie = "Kleur presets";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip1.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip1;
};

void kleurenToolStrip2_Click(object o, EventArgs e)
{
    if (lastKleurenToolStrip != kleurenToolStrip2)
    {
        hideRGB();
        hideColorPreset();
        showLichtDonker();
    }
    kleurenGenButton.Text = "Donker - Licht";
    kleurenSelectie = "Donker - Licht";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip2.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip2;
};

void kleurenToolStrip3_Click(object o, EventArgs e)
{
    if (lastKleurenToolStrip != kleurenToolStrip3)
    {
        hideColorPreset();
        hideLichtDonker();
        showRGB();
    }
    kleurenGenButton.Text = "RGB sliders";
    kleurenSelectie = "RGB sliders";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip3.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip3;
};

void kleurenToolStrip4_Click(object o, EventArgs e)
{
    if (!(lastKleurenToolStrip == kleurenToolStrip3 || lastKleurenToolStrip == kleurenToolStrip4))
    {
        hideColorPreset();
        hideLichtDonker();
        showRGB();
    }
    kleurenGenButton.Text = "Vloeiend";
    kleurenSelectie = "Vloeiend";
    lastKleurenToolStrip.Checked = false;
    kleurenToolStrip4.Checked = true;
    lastKleurenToolStrip = kleurenToolStrip4;
}

// Color preset buttons

void kleurenButton1_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = winterLijst;
    mandelRekenen_Click(null, null);
};

void kleurenButton2_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = lenteLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton3_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = groeneLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton4_Click(object o, EventArgs e)
{
    kleurenSelectie = "Herfst";
    mandelRekenen_Click(null, null);
};
void kleurenButton5_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = regenboogLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton6_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = koeleLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton7_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = vurigeLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton8_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = grijzeLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton9_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = funkyLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton10_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = groeneLijst2;
    mandelRekenen_Click(null, null);
};
void kleurenButton11_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = zwartWitteLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton12_Click(object o, EventArgs e)
{
    kleurenSelectie = "Kleur presets";
    kleurenLijst = pantoneLijst;
    mandelRekenen_Click(null, null);
};
void kleurenButton13_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "251";
    groenBox.Text = "0";
    blauwBox.Text = "0";
    mandelRekenen_Click(null, null);
};
void kleurenButton14_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "0";
    groenBox.Text = "10";
    blauwBox.Text = "251";
    mandelRekenen_Click(null, null);
};
void kleurenButton15_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "3";
    groenBox.Text = "0";
    blauwBox.Text = "161";
    mandelRekenen_Click(null, null);
};
void kleurenButton16_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "255";
    groenBox.Text = "255";
    blauwBox.Text = "66";
    mandelRekenen_Click(null, null);
};
void kleurenButton17_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "0";
    groenBox.Text = "251";
    blauwBox.Text = "0";


    mandelRekenen_Click(null, null);
};
void kleurenButton18_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "255";
    groenBox.Text = "0";
    blauwBox.Text = "255";
    mandelRekenen_Click(null, null);
};
void kleurenButton19_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "106";
    groenBox.Text = "62";
    blauwBox.Text = "255";
    mandelRekenen_Click(null, null);
};
void kleurenButton20_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "91";
    groenBox.Text = "0";
    blauwBox.Text = "25";
    mandelRekenen_Click(null, null);
};
void kleurenButton21_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "0";
    groenBox.Text = "0";
    blauwBox.Text = "251";
    mandelRekenen_Click(null, null);
};
void kleurenButton22_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "171";
    groenBox.Text = "113";
    blauwBox.Text = "75";
    mandelRekenen_Click(null, null);
};
void kleurenButton23_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "240";
    groenBox.Text = "231";
    blauwBox.Text = "205";
    mandelRekenen_Click(null, null);
};
void kleurenButton24_Click(object o, EventArgs e)
{
    kleurenSelectie = "RGB sliders";
    roodBox.Text = "252";
    groenBox.Text = "168";
    blauwBox.Text = "133";
    mandelRekenen_Click(null, null);
};

// Scrollbar Methods

void roodBar_Scroll(object o, EventArgs e)
{
    roodBox.Text = $"{roodBar.Value}";
}

void groenBar_Scroll(object o, EventArgs e)
{
    groenBox.Text = $"{groenBar.Value}";
}

void blauwBar_Scroll(object o, EventArgs e)
{
    blauwBox.Text = $"{blauwBar.Value}";
}

void lichtDonkerBar_Scroll(object o, EventArgs e)
{
    lichtDonkerBox.Text = $"{lichtDonkerBar.Value}";
}

// Toolstrip and button controls for presets and colors

presetButton.Click += showPresets;
kleurenGenButton.Click += showKleuren;

presetToolStrip1.Click += presetToolStrip1_Click;
presetToolStrip2.Click += presetToolStrip2_Click;
presetToolStrip3.Click += presetToolStrip3_Click;
presetToolStrip4.Click += presetToolStrip4_Click;
presetToolStrip5.Click += presetToolStrip5_Click;
presetToolStrip6.Click += presetToolStrip6_Click;
presetToolStrip7.Click += presetToolStrip7_Click;
presetToolStrip8.Click += presetToolStrip8_Click;
presetToolStrip9.Click += presetToolStrip9_Click;
presetToolStrip10.Click += presetToolStrip10_Click;
presetToolStrip11.Click += presetToolStrip11_Click;
presetToolStrip12.Click += presetToolStrip12_Click;
presetToolStrip13.Click += presetToolStrip13_Click;
presetToolStrip14.Click += presetToolStrip14_Click;
presetToolStrip15.Click += presetToolStrip15_Click;
presetToolStrip16.Click += presetToolStrip16_Click;
presetToolStrip17.Click += presetToolStrip17_Click;
presetToolStrip18.Click += presetToolStrip18_Click;
presetToolStrip19.Click += presetToolStrip19_Click;
presetToolStrip20.Click += presetToolStrip20_Click;

kleurenToolStrip1.Click += kleurenToolStrip1_Click;
kleurenToolStrip2.Click += kleurenToolStrip2_Click;
kleurenToolStrip3.Click += kleurenToolStrip3_Click;
kleurenToolStrip4.Click += kleurenToolStrip4_Click;

kleurenButton1.Click += kleurenButton1_Click;
kleurenButton2.Click += kleurenButton2_Click;
kleurenButton3.Click += kleurenButton3_Click;
kleurenButton4.Click += kleurenButton4_Click;
kleurenButton5.Click += kleurenButton5_Click;
kleurenButton6.Click += kleurenButton6_Click;
kleurenButton7.Click += kleurenButton7_Click;
kleurenButton8.Click += kleurenButton8_Click;
kleurenButton9.Click += kleurenButton9_Click;
kleurenButton10.Click += kleurenButton10_Click;
kleurenButton11.Click += kleurenButton11_Click;
kleurenButton12.Click += kleurenButton12_Click;
kleurenButton13.Click += kleurenButton13_Click;
kleurenButton14.Click += kleurenButton14_Click;
kleurenButton15.Click += kleurenButton15_Click;
kleurenButton16.Click += kleurenButton16_Click;
kleurenButton17.Click += kleurenButton17_Click;
kleurenButton18.Click += kleurenButton18_Click;
kleurenButton19.Click += kleurenButton19_Click;
kleurenButton20.Click += kleurenButton20_Click;
kleurenButton21.Click += kleurenButton21_Click;
kleurenButton22.Click += kleurenButton22_Click;
kleurenButton23.Click += kleurenButton23_Click;
kleurenButton24.Click += kleurenButton24_Click;

roodBar.Scroll += roodBar_Scroll;
groenBar.Scroll += groenBar_Scroll;
blauwBar.Scroll += blauwBar_Scroll;

lichtDonkerBar.Scroll += lichtDonkerBar_Scroll;

// Mousebutton controls + run on startup command

mandelButton.Click += mandelRekenen_Click;
mandelLabel.MouseClick += mandelRekenen_MouseClick;
mandelResetButton.Click += mandelReset_Click;

hideLichtDonker();
hideRGB();
hideColorPreset();

mandelRekenen_Click(null, null);

Application.Run(scherm);