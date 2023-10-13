using System;
using System.Drawing;
using System.Windows.Forms;

// -- Instantiating globally used variables --
// GUI variables
int bordGrootte = 6;
int halfBord = bordGrootte / 2;
int afstandBord = (770 - bordGrootte * 75) / 2;

Font arial = new Font("Arial", 20, FontStyle.Bold);

Pen bordPen = new Pen(Brushes.Black, 3);

// Arrays
int[,] bord = new int[bordGrootte, bordGrootte];
Label[,] vakjes = new Label[bordGrootte, bordGrootte];
int[] turfStenen = new int[4];
int[] xLijst = { 1, 1, 1, 0, -1, -1, -1, 0 };
int[] yLijst = { 1, 0, -1, -1, -1, 0, 1, 1 };

// Boolean variables
bool WitAanZet = true;
bool WitBegint = true;
bool ZwartKanNiet = false;
bool WitKanNiet = false;
bool hulpAan = true;

// -- End of globally used variables --

// -- Creating GUI elements --
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.DarkGreen;
scherm.ClientSize = new Size(770, 75 * bordGrootte + 210);

Bitmap witCirkelBit = new Bitmap(40, 40);
Bitmap zwartCirkelBit = new Bitmap(40, 40);
Bitmap valideCirkelBit = new Bitmap(40, 40);
Graphics witTeken = Graphics.FromImage(witCirkelBit);
Graphics zwartTeken = Graphics.FromImage(zwartCirkelBit);
Graphics valideTeken = Graphics.FromImage(valideCirkelBit);

witTeken.FillEllipse(Brushes.White, 0, 0, 40, 40);
zwartTeken.FillEllipse(Brushes.Black, 0, 0, 40, 40);
valideTeken.DrawEllipse(bordPen, 10, 10, 20, 20);

Button wieBegintKnop = new Button
{
    Location = new Point(30, 10),
    Size = new Size(100, 25),
    Text = "Wit begint",
    BackColor = Color.Green
};

Button nieuwSpelKnop = new Button
{
    Location = new Point(150, 10),
    Size = new Size(100, 25),
    Text = "Nieuw spel",
    BackColor = Color.Green
};

Button helpKnop = new Button
{
    Location = new Point(270, 10),
    Size = new Size(100, 25),
    Text = "Help",
    BackColor = Color.Green
};

Label witStatus = new Label
{
    Location = new Point(120, 55),
    Size = new Size(150, 40),
    Font = arial,
    ForeColor = Color.White
};

Label zwartStatus = new Label
{
    Location = new Point(120, 105),
    Size = new Size(150, 40),
    Font = arial,
    ForeColor = Color.Black
};

Label witCirkelLabel = new Label
{
    Location = new Point(60, 50),
    Size = new Size(40, 40),
    Image = witCirkelBit
};

Label zwartCirkelLabel = new Label
{
    Location = new Point(60, 100),
    Size = new Size(40, 40),
    Image = zwartCirkelBit
};

Label zwartKanLab = new Label
{
    Size = new Size(100, 20),
    Location = new Point(100, 175),
    BackColor = Color.Green
};

Label witKanLab = new Label
{
    Size = new Size(100, 20),
    Location = new Point(250, 175),
    BackColor = Color.Green
};

Label winnaarLabel = new Label
{
    Size = new Size(300, 40),
    Location = new Point(300, 75),
    Font = arial
};

ComboBox bordGrootteComboBox = new ComboBox
{
    Location = new Point(390, 10),
    Size = new Size(100, 20),
};

bordGrootteComboBox.Items.AddRange(new string[] { "4x4", "6x6", "8x8", "10x10" });
bordGrootteComboBox.SelectedIndex = 1;

for (int i = 0; i < bordGrootte; i++)
{
    for (int n = 0; n < bordGrootte; n++)
    {
        Label vakje = new Label();
        vakje.Location = new Point(afstandBord + 10 + i * 75, 210 + n * 75);
        vakje.Size = new Size(55, 55);
        vakje.MouseClick += Bord_Click;
        scherm.Controls.Add(vakje);
        vakjes[i, n] = vakje;
    }
}

scherm.Controls.Add(nieuwSpelKnop);
scherm.Controls.Add(helpKnop);
scherm.Controls.Add(witStatus);
scherm.Controls.Add(zwartStatus);
scherm.Controls.Add(witCirkelLabel);
scherm.Controls.Add(zwartCirkelLabel);
scherm.Controls.Add(witKanLab);
scherm.Controls.Add(zwartKanLab);
scherm.Controls.Add(winnaarLabel);
scherm.Controls.Add(bordGrootteComboBox);
scherm.Controls.Add(wieBegintKnop);
// -- End of GUI elements --

void NieuwSpel_Click(object o, EventArgs e)
{
    witKanLab.Text = "";
    zwartKanLab.Text = "";
    WitKanNiet = false;
    ZwartKanNiet = false;
    if (WitBegint)
    {
        WitAanZet = true;
    }
    else
    {
        WitAanZet = false;
    }
    AanDeBeurt();
    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            bord[i, n] = 0;
        }
    }
    bord[halfBord - 1, halfBord - 1] = 1;
    bord[halfBord, halfBord] = 1;
    bord[halfBord, halfBord - 1] = -1;
    bord[halfBord - 1, halfBord] = -1;
    for (int h = 0; h < bordGrootte; h++)
    {
        for (int k = 0; k < bordGrootte; k++)
        {
            ValideCheck(h, k);
        }
    }
    Tellen();
    scherm.Invalidate();
}

void Bord_Click(object o, MouseEventArgs mea)
{
    Point hier = scherm.PointToClient(Cursor.Position);

    if (hier.Y > 200)
    {
        WitKanNiet = false;
        ZwartKanNiet = false;

        int x = (hier.X - afstandBord) / 75;
        int y = (hier.Y - 200) / 75;

        if (x > bordGrootte - 1 || y > bordGrootte - 1 || x < 0 || y < 0)
        {
            // Buiten het bord geklikt
            return;
        }
        if (bord[x, y] == 2)
        {
            if (WitAanZet)
            {
                bord[x, y] = 1;
                Overnemen(x, y);
            }
            else
            {
                bord[x, y] = -1;
                Overnemen(x, y);
            }
            WitAanZet = !WitAanZet;
            EindeBeurt();
        }
    }
}

void Overnemen(int x, int y)
{
    int RofB;
    if (WitAanZet)
    {
        RofB = 1;
    }
    else
    {
        RofB = -1;
    }

    for (int d = 0; d < 8; d++)
    {
        for (int i = 1; i <= 7; i++)
        {
            int nx = x + i * xLijst[d];
            int ny = y + i * yLijst[d];

            if (nx < 0 || nx >= bordGrootte || ny < 0 || ny >= bordGrootte || bord[nx, ny] == 0 || bord[nx, ny] == 2)
            {
                break;
            }
            else if (bord[nx, ny] == RofB)
            {
                for (int n = 0; n < i; n++)
                {
                    bord[x + n * xLijst[d], y + n * yLijst[d]] = RofB;
                }
                break;
            }
        }
    }
    scherm.Invalidate();
}

void EindeBeurt()
{
    ResetHulp();
    for (int h = 0; h < bordGrootte; h++)
    {
        for (int k = 0; k < bordGrootte; k++)
        {
            ValideCheck(h, k);
        }
    }
    Tellen();
    CheckSoftlock();
}

void ResetHulp()
{
    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            if (bord[i, n] == 2)
            {
                bord[i, n] = 0;
            }
        }
    }
    scherm.Invalidate();
}

void ValideCheck(int x, int y)
{
    int aanZet;
    if (WitAanZet)
    {
        aanZet = 1;
    }
    else
    {
        aanZet = -1;
    }

    if (bord[x, y] != 0)
    {
        return;
    }
    for (int i = 0; i < 8; i++)
    {
        int loop = 1;
        bool detectie = false;

        int dx = x + xLijst[i] * loop;
        int dy = y + yLijst[i] * loop;

        while (dx >= 0 && dy >= 0 && dx < bordGrootte && dy < bordGrootte)
        {
            if (bord[dx, dy] == aanZet && detectie)
            {
                bord[x, y] = 2;
            }
            else if (bord[dx, dy] != aanZet * -1)
            {
                break;
            }
            else
            {
                detectie = true;
            }

            dx = x + loop * xLijst[i];
            dy = y + loop * yLijst[i];
            loop++;
        }
    }
}

void Tellen()
{
    for (int i = 0; i < 4; i++)
    {
        turfStenen[i] = 0;
    }
    foreach (int l in bord)
    {
        turfStenen[l + 1]++;
    }

    if (turfStenen[2] == 1)
    {
        witStatus.Text = "1 Steen";
    }
    else
    {
        witStatus.Text = turfStenen[2].ToString() + " Stenen";
    }

    if (turfStenen[0] == 1)
    {
        zwartStatus.Text = "1 Steen";
    }
    else
    {
        zwartStatus.Text = turfStenen[0].ToString() + " Stenen";
    }
}

void CheckSoftlock()
{
    //Cijfer();
    witKanLab.Text = "";
    zwartKanLab.Text = "";
    if (WitAanZet && turfStenen[3] == 0 && !WitKanNiet)
    {
        WitKanNiet = true;
        WitAanZet = !WitAanZet;
        EindeBeurt();
        witKanLab.Text = "Wit kan niet";
    }
    if (!WitAanZet && turfStenen[3] == 0 && !ZwartKanNiet)
    {
        ZwartKanNiet = true;
        WitAanZet = !WitAanZet;
        EindeBeurt();
        zwartKanLab.Text = "Zwart kan niet";
    }

    AanDeBeurt();
    if (ZwartKanNiet && WitKanNiet || turfStenen[0] == 0 || turfStenen[2] == 0)
    {
        EindeSpel();
    }
}

void AanDeBeurt()
{
    if (WitAanZet)
    {
        winnaarLabel.Text = "Wit is aan zet";
        winnaarLabel.ForeColor = Color.White;
    }
    else
    {
        winnaarLabel.Text = "Zwart is aan zet";
        winnaarLabel.ForeColor = Color.Black;
    }
}

void EindeSpel()
{
    Tellen();
    if (turfStenen[0] > turfStenen[2])
    {
        winnaarLabel.Text = "Zwart is de winnaar!";
        winnaarLabel.ForeColor = Color.Black;
    }
    else if (turfStenen[2] > turfStenen[0])
    {
        winnaarLabel.Text = "Wit is de winnaar!";
        winnaarLabel.ForeColor = Color.White;
    }
    else
    {
        winnaarLabel.Text = "Gelijkspel!";
        winnaarLabel.ForeColor = Color.Black;
    }
}

void Tekenen(object o, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;

    for (int i = 0; i < bordGrootte + 1; i++)
    {
        gr.DrawLine(bordPen, afstandBord + 75 * i, 200, afstandBord + 75 * i, 75 * bordGrootte + 200);
        gr.DrawLine(bordPen, afstandBord, 200 + 75 * i, afstandBord + 75 * bordGrootte, 200 + 75 * i);
    }
    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            if (bord[i, n] == -1)
            {
                vakjes[i, n].Image = zwartCirkelBit;
            }
            else if (bord[i, n] == 0 || !hulpAan && bord[i, n] == 2)
            {
                vakjes[i, n].Image = null;
            }
            else if (bord[i, n] == 1)
            {
                vakjes[i, n].Image = witCirkelBit;
            }
            else if (bord[i, n] == 2 && hulpAan)
            {
                vakjes[i, n].Image = valideCirkelBit;
            }
        }
    }
}

void HelpKnop_Click(Object o, EventArgs ea)
{
    hulpAan = !hulpAan;
    scherm.Invalidate();
}

void WieBegint_Click(object o, EventArgs ea)
{
    WitBegint = !WitBegint;
    if (WitBegint)
    {
        wieBegintKnop.Text = "Wit begint";
    }
    else
    {
        wieBegintKnop.Text = "Zwart begint";
    }
}

void BordGrotte_Verander(object sender, EventArgs e)
{
    string selectedSize = (string)bordGrootteComboBox.SelectedItem;
    int newSize = int.Parse(selectedSize.Split('x')[0]);

    foreach (Label vakje in vakjes)
    {
        scherm.Controls.Remove(vakje);
    }

    bordGrootte = newSize;
    halfBord = bordGrootte / 2;
    afstandBord = (770 - bordGrootte * 75) / 2;
    bord = new int[bordGrootte, bordGrootte];
    vakjes = new Label[bordGrootte, bordGrootte];

    scherm.ClientSize = new Size(770, 75 * bordGrootte + 210);

    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            Label vakje = new Label();
            vakje.Location = new Point(afstandBord + 10 + i * 75, 210 + n * 75);
            vakje.Size = new Size(55, 55);
            vakje.MouseClick += Bord_Click;
            scherm.Controls.Add(vakje);
            vakjes[i, n] = vakje;
        }
    }
    NieuwSpel_Click(null, null);
}
void Start()
{
    bord[halfBord - 1, halfBord - 1] = 1;
    bord[halfBord, halfBord] = 1;
    bord[halfBord, halfBord - 1] = -1;
    bord[halfBord - 1, halfBord] = -1;

    nieuwSpelKnop.Click += NieuwSpel_Click;
    scherm.Paint += Tekenen;
    scherm.MouseClick += Bord_Click;
    helpKnop.Click += HelpKnop_Click;
    wieBegintKnop.Click += WieBegint_Click;
    bordGrootteComboBox.SelectedIndexChanged += BordGrotte_Verander;
    EindeBeurt();
}

// ----- WORDT GEBRUIKT VOOR DEBUGGING - NIET WEGHALEN -----
//void Cijfer()
//{
//    for (int i = 0; i < bordGrootte; i++)
//    {
//        for (int n = 0; n < bordGrootte; n++)
//        {
//            vakjes[i, n].Text = bord[i, n].ToString();
//        }
//    }
//}

Start();
Application.Run(scherm);