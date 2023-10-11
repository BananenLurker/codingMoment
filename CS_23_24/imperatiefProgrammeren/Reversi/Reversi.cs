using System;
using System.Drawing;
using System.Windows.Forms;

int bordGrootte = 6;

// Instantiating globally used variables
// Arrays
int[,] bord = new int[bordGrootte, bordGrootte];
Label[,] vakjes = new Label[bordGrootte, bordGrootte];
int[] turfStenen = new int[4];
int[] xLijst = { 1, 1, 1, 0, -1, -1, -1, 0 };
int[] yLijst = { 1, 0, -1, -1, -1, 0, 1, 1 };

// GUI variables
int halfBord = bordGrootte / 2;

Font arial = new Font("Arial", 20, FontStyle.Bold);

// Mousepointer
Point hier = new Point(0, 0);

// Boolean variables
bool roodAanZet = true;
bool blauwKanNiet = false;
bool roodKanNiet = false;
bool hulpAan = true;

// Creating GUI elements
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.ClientSize = new Size(770, 75 * bordGrootte + 210);

Bitmap roodcirkelbit = new Bitmap(40, 40);
Bitmap blauwcirkelbit = new Bitmap(40, 40);
Bitmap validecirkelbit = new Bitmap(40, 40);
Graphics roodteken = Graphics.FromImage(roodcirkelbit);
Graphics blauwteken = Graphics.FromImage(blauwcirkelbit);
Graphics valideteken = Graphics.FromImage(validecirkelbit);

Button nieuwspelknop = new Button
{
    Location = new Point(150, 10),
    Size = new Size(100, 20),
    Text = "Nieuw spel"
};

Button helpKnop = new Button
{
    Location = new Point(270, 10),
    Size = new Size(100, 20),
    Text = "Help"
};

Label roodstatus = new Label
{
    Location = new Point(120, 55),
    Size = new Size(150, 40),
    Font = arial,
    ForeColor = Color.Red
};

Label blauwstatus = new Label
{
    Location = new Point(120, 105),
    Size = new Size(150, 40),
    Font = arial,
    ForeColor = Color.Blue
};

Label roodcirkellabel = new Label
{
    Location = new Point(60, 50),
    Size = new Size(40, 40),
    Image = roodcirkelbit
};

Label blauwcirkellabel = new Label
{
    Location = new Point(60, 100),
    Size = new Size(40, 40),
    Image = blauwcirkelbit
};

roodteken.FillEllipse(Brushes.Red, 0, 0, 40, 40);
blauwteken.FillEllipse(Brushes.Blue, 0, 0, 40, 40);
valideteken.DrawEllipse(Pens.Black, 10, 10, 20, 20);

Label blauwKanLab = new Label
{
    Size = new Size(100, 20),
    Location = new Point(100, 175),
    BackColor = Color.LightBlue
};

Label roodKanLab = new Label
{
    Size = new Size(100, 20),
    Location = new Point(250, 175),
    BackColor = Color.LightBlue
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
        vakje.Location = new Point(20 + i * 75, 210 + n * 75);
        vakje.Size = new Size(55, 55);
        vakje.MouseClick += bord_Click;
        scherm.Controls.Add(vakje);
        vakjes[i, n] = vakje;
    }
}

scherm.Controls.Add(nieuwspelknop);
scherm.Controls.Add(helpKnop);
scherm.Controls.Add(roodstatus);
scherm.Controls.Add(blauwstatus);
scherm.Controls.Add(roodcirkellabel);
scherm.Controls.Add(blauwcirkellabel);
scherm.Controls.Add(roodKanLab);
scherm.Controls.Add(blauwKanLab);
scherm.Controls.Add(winnaarLabel);
scherm.Controls.Add(bordGrootteComboBox);

void nieuwSpel_Click(object o, EventArgs e)
{
    roodKanLab.Text = "";
    blauwKanLab.Text = "";
    roodKanNiet = false;
    blauwKanNiet = false;
    roodAanZet = true;
    aanDeBeurt();
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
            valideCheck(h, k);
        }
    }
    tellen();
    scherm.Invalidate();
}

void bord_Click(object o, MouseEventArgs mea)
{
    hier = scherm.PointToClient(Cursor.Position);

    if (hier.Y > 200)
    {
        roodKanNiet = false;
        blauwKanNiet = false;

        int x = hier.X / 75;
        int y = (hier.Y - 200) / 75;
        if (x > bordGrootte - 1 || y > bordGrootte - 1 || x < 0 || y < 0)
        {
            // Buiten het bord geklikt
            return;
        }
        if (bord[x, y] == 2)
        {
            if (roodAanZet)
            {
                bord[x, y] = 1;
                overnemen(x, y);
            }
            else
            {
                bord[x, y] = -1;
                overnemen(x, y);
            }
            roodAanZet = !roodAanZet;
            eindeBeurt();
        }
    }
}

void overnemen(int x, int y)
{
    int RofB;
    if (roodAanZet)
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

void eindeBeurt()
{
    resetHulp();
    for (int h = 0; h < bordGrootte; h++)
    {
        for (int k = 0; k < bordGrootte; k++)
        {
            valideCheck(h, k);
        }
    }
    tellen();
    checkSoftlock();
}

void resetHulp()
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

void valideCheck(int x, int y)
{
    int aanZet;
    if (roodAanZet)
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

void tellen()
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
        roodstatus.Text = "1 Steen";
    }
    else
    {
        roodstatus.Text = turfStenen[2].ToString() + " Stenen";
    }

    if (turfStenen[0] == 1)
    {
        blauwstatus.Text = "1 Steen";
    }
    else
    {
        blauwstatus.Text = turfStenen[0].ToString() + " Stenen";
    }
}

void checkSoftlock()
{
    //cijfer();
    roodKanLab.Text = "";
    blauwKanLab.Text = "";
    if (roodAanZet && turfStenen[3] == 0 && !roodKanNiet)
    {
        roodKanNiet = true;
        roodAanZet = !roodAanZet;
        eindeBeurt();
        roodKanLab.Text = "rood kan niet";
    }
    if (!roodAanZet && turfStenen[3] == 0 && !blauwKanNiet)
    {
        blauwKanNiet = true;
        roodAanZet = !roodAanZet;
        eindeBeurt();
        blauwKanLab.Text = "blauw kan niet";
    }

    aanDeBeurt();
    if (blauwKanNiet && roodKanNiet || turfStenen[0] == 0 || turfStenen[2] == 0)
    {
        eindeSpel();
    }
}

void aanDeBeurt()
{
    if (roodAanZet)
    {
        winnaarLabel.Text = "Rood is aan zet";
        winnaarLabel.ForeColor = Color.Red;
    }
    else
    {
        winnaarLabel.Text = "Blauw is aan zet";
        winnaarLabel.ForeColor = Color.Blue;
    }
}

void eindeSpel()
{
    tellen();
    if (turfStenen[0] > turfStenen[2])
    {
        winnaarLabel.Text = "Blauw is de winnaar!";
        winnaarLabel.ForeColor = Color.Blue;
    }
    else if (turfStenen[2] > turfStenen[0])
    {
        winnaarLabel.Text = "Rood is de winnaar!";
        winnaarLabel.ForeColor = Color.Red;
    }
    else
    {
        winnaarLabel.Text = "Gelijkspel!";
        winnaarLabel.ForeColor = Color.Black;
    }
}

void tekenen(object o, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;

    for (int i = 0; i < bordGrootte + 1; i++)
    {
        gr.DrawLine(Pens.Black, 10 + 75 * i, 200, 10 + 75 * i, 75 * bordGrootte + 200);
        gr.DrawLine(Pens.Black, 10, 200 + 75 * i, 75 * bordGrootte + 10, 200 + 75 * i);
    }
    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            if (bord[i, n] == -1)
            {
                vakjes[i, n].Image = blauwcirkelbit;
            }
            else if (bord[i, n] == 0 || !hulpAan && bord[i, n] == 2)
            {
                vakjes[i, n].Image = null;
            }
            else if (bord[i, n] == 1)
            {
                vakjes[i, n].Image = roodcirkelbit;
            }
            else if (bord[i, n] == 2 && hulpAan)
            {
                vakjes[i, n].Image = validecirkelbit;
            }
        }
    }
}

void helpKnop_Click(Object o, EventArgs ea)
{
    hulpAan = !hulpAan;
    scherm.Invalidate();
}

void bordGrootteComboBoxverander(object sender, EventArgs e)
{
    string selectedSize = (string)bordGrootteComboBox.SelectedItem;
    int newSize = int.Parse(selectedSize.Split('x')[0]);

    foreach (Label vakje in vakjes)
    {
        scherm.Controls.Remove(vakje);
    }

    bordGrootte = newSize;
    halfBord = bordGrootte / 2;
    bord = new int[bordGrootte, bordGrootte];
    vakjes = new Label[bordGrootte, bordGrootte];

    scherm.ClientSize = new Size(770, 75 * bordGrootte + 210);

    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            Label vakje = new Label();
            vakje.Location = new Point(20 + i * 75, 210 + n * 75);
            vakje.Size = new Size(55, 55);
            vakje.MouseClick += bord_Click;
            scherm.Controls.Add(vakje);
            vakjes[i, n] = vakje;
        }
    }
    nieuwSpel_Click(null, null);
}

void start()
{
    bord[halfBord - 1, halfBord - 1] = 1;
    bord[halfBord, halfBord] = 1;
    bord[halfBord, halfBord - 1] = -1;
    bord[halfBord - 1, halfBord] = -1;

    nieuwspelknop.Click += nieuwSpel_Click;
    scherm.Paint += tekenen;
    scherm.MouseClick += bord_Click;
    helpKnop.Click += helpKnop_Click;
    bordGrootteComboBox.SelectedIndexChanged += bordGrootteComboBoxverander;
    eindeBeurt();
}

// ----- WORDT GEBRUIKT VOOR DEBUGGING - NIET WEGHALEN -----
//void cijfer()
//{
//    for (int i = 0; i < bordGrootte; i++)
//    {
//        for (int n = 0; n < bordGrootte; n++)
//        {
//            vakjes[i, n].Text = bord[i, n].ToString();
//        }
//    }
//}

start();
Application.Run(scherm);