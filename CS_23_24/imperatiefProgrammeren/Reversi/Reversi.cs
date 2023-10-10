using System;
using System.Drawing;
using System.Windows.Forms;

int aantal = 1;
int RofB;
int bordGrootte = 8;
int aanZet;
int loop;

int[,] bord = new int[bordGrootte, bordGrootte];
int halfBord = bordGrootte / 2;
bord[halfBord - 1, halfBord - 1] = 1;
bord[halfBord, halfBord] = 1;
bord[halfBord, halfBord - 1] = -1;
bord[halfBord - 1, halfBord] = -1;
Label[,] vakjes = new Label[8, 8];

Point hier = new Point(0, 0);

bool roodAanZet = true;
bool blauwKanNiet = false;
bool roodKanNiet = false;
bool detectie = false;

int[] xLijst = { 1, 1, 1, 0, -1, -1, -1, 0 };
int[] yLijst = { 1, 0, -1, -1, -1, 0, 1, 1 };

Form scherm = new Form();
scherm.Text = "Reversi";
scherm.ClientSize = new Size(bordGrootte * 75 + 20, 810);

Button nieuwspelknop = new Button();
scherm.Controls.Add(nieuwspelknop);
nieuwspelknop.Location = new Point(150, 10);
nieuwspelknop.Size = new Size(100, 20);
nieuwspelknop.Text = "Nieuw spel";

Button helpknop = new Button();
scherm.Controls.Add(helpknop);
helpknop.Location = new Point(270, 10);
helpknop.Size = new Size(100, 20);
helpknop.Text = "Help";

Label roodstatus = new Label();
scherm.Controls.Add(roodstatus);
roodstatus.Location = new Point(120, 55);
roodstatus.Size = new Size(150, 40);
roodstatus.Text = "2 Stenen";
roodstatus.Font = new Font("Arial", 20, FontStyle.Bold);
roodstatus.ForeColor = Color.Red;

Label blauwstatus = new Label();
scherm.Controls.Add(blauwstatus);
blauwstatus.Location = new Point(120, 105);
blauwstatus.Size = new Size(150, 40);
blauwstatus.Text = "2 Stenen";
blauwstatus.Font = new Font("Arial", 20, FontStyle.Bold);
blauwstatus.ForeColor = Color.Blue;

Bitmap roodcirkelbit = new Bitmap(40, 40);
Graphics roodteken = Graphics.FromImage(roodcirkelbit);
Label roodcikellabel = new Label();
scherm.Controls.Add(roodcikellabel);
roodcikellabel.Location = new Point(60, 50);
roodcikellabel.Size = new Size(40, 40);
roodcikellabel.Image = roodcirkelbit;
roodteken.FillEllipse(Brushes.Red, 0, 0, 40, 40);

Bitmap blauwcirkelbit = new Bitmap(40, 40);
Graphics blauwteken = Graphics.FromImage(blauwcirkelbit);
Label blauwcikellabel = new Label();
scherm.Controls.Add(blauwcikellabel);
blauwcikellabel.Location = new Point(60, 100);
blauwcikellabel.Size = new Size(40, 40);
blauwcikellabel.Image = blauwcirkelbit;
blauwteken.FillEllipse(Brushes.Blue, 0, 0, 40, 40);

Bitmap validecirkelbit = new Bitmap(40, 40);
Graphics valideteken = Graphics.FromImage(validecirkelbit);
valideteken.DrawEllipse(Pens.Black, 0, 0, 20, 20);

for (int i = 0; i < 8; i++)
{
    for (int n = 0; n < 8; n++)
    {
        Label vakje = new Label();
        vakje.Location = new Point(20 + i * 75, 210 + n * 75);
        vakje.Size = new Size(55, 55);
        vakje.MouseClick += bord_Click;
        scherm.Controls.Add(vakje);
        vakjes[i, n] = vakje;
    }
}

void nieuwSpel_Click(object o, EventArgs e)
{
    for (int i = 0; i < 8; i++)
    {
        for (int n = 0; n < 8; n++)
        {
            bord[i, n] = 0;
        }
    }
    bord[3, 3] = 1;
    bord[4, 4] = 1;
    bord[4, 3] = -1;
    bord[3, 4] = -1;
    aantal++;
    roodAanZet = true;
    scherm.Invalidate();
}

void tekenen(object o, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;

    for (int i = 0; i < 9; i++)
    {
        gr.DrawLine(Pens.Black, 10 + 75 * i, 200, 10 + 75 * i, 800);
        gr.DrawLine(Pens.Black, 10, 200 + 75 * i, 610, 200 + 75 * i);
    }
    for (int i = 0; i < 8; i++)
    {
        for (int n = 0; n < 8; n++)
        {
            if (bord[i, n] == 1)
            {
                vakjes[i, n].Image = roodcirkelbit;
            }
            else if (bord[i, n] == -1)
            {
                vakjes[i, n].Image = blauwcirkelbit;
            }
            else if (bord[i, n] == 2)
            {
                vakjes[i, n].Image = validecirkelbit;
            }
            else if (bord[i, n] == 0)
            {
                vakjes[i, n].Image = null;
            }
        }
    }
}
void bord_Click(object o, MouseEventArgs mea)
{
    hier = scherm.PointToClient(Cursor.Position);

    if (roodKanNiet && blauwKanNiet)
    {
        // einde van het spel
        return;
    }
    if (hier.Y > 200)
    {
        roodKanNiet = false;
        blauwKanNiet = false;

        int x = hier.X / 75;
        int y = (hier.Y - 200) / 75;
        roodstatus.Text = x.ToString();
        blauwstatus.Text = y.ToString();
        if (x > 7 || y > 7 || x < 0 || y < 0)
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
            helpknop.Text = roodAanZet.ToString();
            resetNaTurn();
            for (int h = 0; h < bordGrootte; h++)
            {
                for (int k = 0; k < bordGrootte; k++)
                {
                    valideCheck(h, k);
                }
            }
        }
    }
}

void overnemen(int x, int y)
{
    if (roodAanZet)
    {
        RofB = 1;
    }
    else
    {
        RofB = -1;
    }

    for (int i = x + 1; i <= 7; i++)
    {
        if (bord[i, y] == 0)
        {
            break;
        }
        else if (bord[i, y] == RofB)
        {
            for (int n = x; n < i; n++)
            {
                bord[n, y] = RofB;
            }
        }
    }

    for (int i = x; i >= 0; i--)
    {
        if (bord[i, y] == 0)
        {
            break;
        }
        else if (bord[i, y] == RofB)
        {
            for (int n = x; n > i; n--)
            {
                bord[n, y] = RofB;
            }
        }
    }

    for (int i = y + 1; i <= 7; i++)
    {
        if (bord[x, i] == 0)
        {
            break;
        }
        else if (bord[x, i] == RofB)
        {
            for (int n = y; n < i; n++)
            {
                bord[x, n] = RofB;
            }
        }
    }

    for (int i = y; i >= 0; i--)
    {
        if (bord[x, i] == 0)
        {
            break;
        }
        else if (bord[x, i] == RofB)
        {
            for (int n = y; n > i; n--)
            {
                bord[x, n] = RofB;
            }
        }
    }
    scherm.Invalidate();
}

void resetNaTurn()
{
    for (int i = 0; i < 8; i++)
    {
        for (int n = 0; n < 8; n++)
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
        loop = 1;
        detectie = false;

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

nieuwspelknop.Click += nieuwSpel_Click;
scherm.Paint += tekenen;
scherm.MouseClick += bord_Click;

for (int h = 0; h < bordGrootte; h++)
{
    for (int k = 0; k < bordGrootte; k++)
    {
        valideCheck(h, k);
    }
}

Application.Run(scherm);