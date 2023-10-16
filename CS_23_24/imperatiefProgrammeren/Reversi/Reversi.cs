using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

// -- Instantiating globally used variables --
// GUI variables
int bordGrootte = 6;
int halfBord = bordGrootte / 2;
int afstandBord = (770 - bordGrootte * 75) / 2;

Font arial = new Font("Arial", 20, FontStyle.Bold);
Font arialSubHeader = new Font("Arial", 15, FontStyle.Bold);

Pen bordPen = new Pen(Brushes.Black, 3);

// Arrays
int[,] bord = new int[bordGrootte, bordGrootte];
Label[,] vakjes = new Label[bordGrootte, bordGrootte];
int[] turfStenen = new int[4];
int[] xLijst = { 1, 1, 1, 0, -1, -1, -1, 0 };
int[] yLijst = { 1, 0, -1, -1, -1, 0, 1, 1 };

List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

// Boolean variables
bool ZwartAanZet = true;
bool ZwartBegint = true;
bool zwartKanNiet = false;
bool witKanNiet = false;
bool hulpAan = true;
bool botActive = false;
bool AIbeurt = false;
bool botSpeeltZwart = true;

// -- End of globally used variables --

// -- Creating GUI elements --
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.DarkGreen;
scherm.ClientSize = new Size(770, 75 * bordGrootte + 210);

Bitmap zwartCirkelBit = new Bitmap(40, 40);
Bitmap witCirkelBit = new Bitmap(40, 40);
Bitmap valideCirkelBit = new Bitmap(40, 40);
Graphics zwartTeken = Graphics.FromImage(zwartCirkelBit);
Graphics witTeken = Graphics.FromImage(witCirkelBit);
Graphics valideTeken = Graphics.FromImage(valideCirkelBit);

zwartTeken.FillEllipse(Brushes.Black, 0, 0, 40, 40);
witTeken.FillEllipse(Brushes.White, 0, 0, 40, 40);
valideTeken.DrawEllipse(Pens.Black, 10, 10, 20, 20);

Button wieBegintKnop = new Button
{
    Location = new Point(30, 10),
    Size = new Size(100, 25),
    Text = "Zwart begint",
    BackColor = Color.Green
};

Button activateAIButton = new Button
{
    Location = new Point(390, 40),
    Size = new Size(100, 25),
    Text = "Activeer AI",
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

Label zwartStatus = new Label
{
    Location = new Point(120, 55),
    Size = new Size(150, 40),
    Font = arial,
    ForeColor = Color.Black
};

Label witStatus = new Label
{
    Location = new Point(120, 105),
    Size = new Size(150, 40),
    Font = arial,
    ForeColor = Color.White
};

Label zwartCirkelLabel = new Label
{
    Location = new Point(60, 50),
    Size = new Size(40, 40),
    Image = zwartCirkelBit
};

Label witCirkelLabel = new Label
{
    Location = new Point(60, 100),
    Size = new Size(40, 40),
    Image = witCirkelBit
};

Label witKanLab = new Label
{
    Size = new Size(100, 20),
    Location = new Point(100, 175),
    BackColor = Color.Green
};

Label zwartKanLab = new Label
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

Label botLabel = new Label
{
    Size = new Size(220, 25),
    Location = new Point(500, 40),
    Font = arialSubHeader,
    ForeColor = Color.Gray,
    Text = "bot speelt niet"
};

ComboBox bordGrootteComboBox = new ComboBox
{
    Location = new Point(390, 10),
    Size = new Size(100, 20),
};

bordGrootteComboBox.Items.AddRange(new string[] { "4x4", "6x6", "8x8", "10x10" });
bordGrootteComboBox.SelectedIndex = 1;

Random random = new Random();

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
scherm.Controls.Add(zwartStatus);
scherm.Controls.Add(witStatus);
scherm.Controls.Add(zwartCirkelLabel);
scherm.Controls.Add(witCirkelLabel);
scherm.Controls.Add(zwartKanLab);
scherm.Controls.Add(witKanLab);
scherm.Controls.Add(winnaarLabel);
scherm.Controls.Add(bordGrootteComboBox);
scherm.Controls.Add(wieBegintKnop);
scherm.Controls.Add(activateAIButton);
scherm.Controls.Add(botLabel);
// -- End of GUI elements --

void NieuwSpel_Click(object o, EventArgs e)
{
    Debug.WriteLine("huts");

    zwartKanLab.Text = "";
    witKanLab.Text = "";
    zwartKanNiet = false;
    witKanNiet = false;
    if (ZwartBegint)
    {
        ZwartAanZet = true;
    }
    else
    {
        ZwartAanZet = false;
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
    UpdateBotText();
    scherm.Invalidate();
}

void Bord_Click(object o, MouseEventArgs mea)
{
    zwartKanLab.Text = "";
    witKanLab.Text = "";

    Point hier = scherm.PointToClient(Cursor.Position);

    if (hier.Y > 200)
    {
        zwartKanNiet = false;
        witKanNiet = false;

        int x = (hier.X - afstandBord) / 75;
        int y = (hier.Y - 200) / 75;

        if (x > bordGrootte - 1 || y > bordGrootte - 1 || x < 0 || y < 0)
        {
            // Buiten het bord geklikt
            return;
        }
        if (bord[x, y] == 2)
        {
            Debug.WriteLine("Speler neemt een tegel over");

            Overnemen(x, y);
            ZwartAanZet = !ZwartAanZet;
            EindeBeurt();
            scherm.Invalidate();
            if (botActive)
            {
                AI_Move();
            }
        }
    }
}

void AI_Move()
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

    validMoves.Clear();

    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            if (bord[i, n] == 2)
            {
                validMoves.Add(Tuple.Create(i, n));
            }
        }
    }

    if (validMoves.Count == 0)
    {
        Debug.WriteLine("Er zijn geen valide moves voor de AI");
        ZwartAanZet = !ZwartAanZet;
        EindeBeurt();
    }
    else
    {
        Tuple<int, int> randomMove = validMoves[random.Next(validMoves.Count)];

        Bord_ClickAI(randomMove.Item1, randomMove.Item2);
    }
}

void Bord_ClickAI(int x, int y)
{
    Debug.WriteLine($"Bot neemt een tegel over op {x}, {y}");
    Overnemen(x, y);
    ZwartAanZet = !ZwartAanZet;
    EindeBeurt();
}


void Overnemen(int x, int y)
{
    zwartKanLab.Text = "";
    witKanLab.Text = "";

    int RofB;
    if (ZwartAanZet)
    {
        WofZ = 1;
    }
    else
    {
        WofZ = -1;
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
            else if (bord[nx, ny] == WofZ)
            {
                for (int n = 0; n < i; n++)
                {
                    bord[x + n * xLijst[d], y + n * yLijst[d]] = WofZ;
                }
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
    if (ZwartAanZet)
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
        zwartStatus.Text = "1 Steen";
    }
    else
    {
        zwartStatus.Text = turfStenen[2].ToString() + " Stenen";
    }

    if (turfStenen[0] == 1)
    {
        witStatus.Text = "1 Steen";
    }
    else
    {
        witStatus.Text = turfStenen[0].ToString() + " Stenen";
    }
}

void CheckSoftlock()
{
    //Cijfer();
    if (ZwartAanZet && turfStenen[3] == 0 && !zwartKanNiet)
    {
        Debug.WriteLine("Zwart kan niet");
        zwartKanNiet = true;
        ZwartAanZet = false;
        if (botActive && !botSpeeltZwart)
        {
            Debug.WriteLine("--- Bot actief en bot speelt wit ---");
            AI_Move();
            return;
        }
        EindeBeurt();
        zwartKanLab.Text = "Zwart kan niet";
        return;
    }
    //Debug.WriteLine($"{!ZwartAanZet}, {turfStenen[3] == 0}, {!witKanNiet}");
    if (!ZwartAanZet && turfStenen[3] == 0 && !witKanNiet)
    {
        Debug.WriteLine("Wit kan niet");
        witKanNiet = true;
        ZwartAanZet = true;
        if (botActive && botSpeeltZwart)
        {
            Debug.WriteLine("--- Bot actief en bot speelt zwart ---");
            AI_Move();
            return;
        }
        EindeBeurt();
        witKanLab.Text = "Wit kan niet";
        return;
    }

    AanDeBeurt();
    Tellen();
    if (zwartKanNiet && witKanNiet || turfStenen[0] == 0 || turfStenen[2] == 0 || turfStenen[1] + turfStenen[3] == 0)
    {
        CheckSpelEinde();
    }

    zwartKanNiet = false;
    witKanNiet = false;
}

bool CheckSpelEinde()
{
    if (zwartKanNiet && witKanNiet)
    {
        Debug.WriteLine("GAME END: Wit kan niet en zwart kan niet");
        EindeSpel();
        return true;
    }
    if(turfStenen[1] + turfStenen[3] == 0)
    {
        Debug.WriteLine("GAME END: Geen lege of valide stenen meer over");
        EindeSpel();
        return true;
    }
    if(turfStenen[0] == 0)
    {
        Debug.WriteLine("GAME END: Geen witte stenen meer over");
        EindeSpel();
        return true;
    }
    if(turfStenen[2] == 0)
    {
        Debug.WriteLine("GAME END: Geen zwarte stenen meer over");
        EindeSpel();
        return true;
    }
    return false;
}

void AanDeBeurt()
{
    if (ZwartAanZet)
    {
        winnaarLabel.Text = "Zwart is aan zet";
        winnaarLabel.ForeColor = Color.Black;
    }
    else
    {
        winnaarLabel.Text = "Wit is aan zet";
        winnaarLabel.ForeColor = Color.White;
    }
}

void EindeSpel()
{
    Tellen();
    if (turfStenen[0] > turfStenen[2])
    {
        winnaarLabel.Text = "Wit is de winnaar!";
        winnaarLabel.ForeColor = Color.White;
    }
    else if (turfStenen[2] > turfStenen[0])
    {
        winnaarLabel.Text = "Zwart is de winnaar!";
        winnaarLabel.ForeColor = Color.Black;
    }
    else
    {
        winnaarLabel.Text = "Gelijkspel!";
        winnaarLabel.ForeColor = Color.Black;
    }
    return;
}

void Tekenen(object o, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;

    for (int i = 0; i < bordGrootte + 1; i++)
    {
        gr.DrawLine(Pens.Black, afstandBord + 75 * i, 200, afstandBord + 75 * i, 75 * bordGrootte + 200);
        gr.DrawLine(Pens.Black, afstandBord, 200 + 75 * i, afstandBord + 75 * bordGrootte, 200 + 75 * i);
    }
    for (int i = 0; i < bordGrootte; i++)
    {
        for (int n = 0; n < bordGrootte; n++)
        {
            if (bord[i, n] == -1)
            {
                vakjes[i, n].Image = witCirkelBit;
            }
            else if (bord[i, n] == 0 || !hulpAan && bord[i, n] == 2)
            {
                vakjes[i, n].Image = null;
            }
            else if (bord[i, n] == 1)
            {
                vakjes[i, n].Image = zwartCirkelBit;
            }
            else if (bord[i, n] == 2 && hulpAan)
            {
                vakjes[i, n].Image = valideCirkelBit;
            }
        }
    }
}

void ActivateAI_Click(object o, EventArgs ea)
{
    botActive = !botActive;
    UpdateBotText();
}

void UpdateBotText()
{
    if (botActive)
    {
        activateAIButton.Text = "Deactiveer AI";
        if (ZwartAanZet)
        {
            botLabel.Text = "Bot speelt als: wit";
            botLabel.ForeColor = Color.White;
            botSpeeltZwart = false;
        }
        else
        {
            botLabel.Text = "Bot speelt als: zwart";
            botLabel.ForeColor = Color.Black;
            botSpeeltZwart = true;
        }
    }
    else
    {
        activateAIButton.Text = "Activeer AI";
        botLabel.Text = "Bot speelt niet";
        botLabel.ForeColor = Color.Gray;
    }
}

void HelpKnop_Click(Object o, EventArgs ea)
{
    hulpAan = !hulpAan;
    scherm.Invalidate();
}

void WieBegint_Click(object o, EventArgs ea)
{
    ZwartBegint = !ZwartBegint;
    if (ZwartBegint)
    {
        wieBegintKnop.Text = "Zwart begint";
    }
    else
    {
        wieBegintKnop.Text = "Wit begint";
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
    activateAIButton.Click += ActivateAI_Click;
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