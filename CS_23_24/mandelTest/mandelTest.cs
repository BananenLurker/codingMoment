using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "mandelbrot";
scherm.BackColor = Color.LightGray;
scherm.ClientSize = new Size(550, 700);

Bitmap bit = new Bitmap(500, 500);
Graphics draw = Graphics.FromImage(bit);

//interface
Label mandelbrot = new Label();
scherm.Controls.Add(mandelbrot);
mandelbrot.Location = new Point(25, 175);
mandelbrot.Size = new Size(500, 500);
mandelbrot.BackColor = Color.White;
mandelbrot.Image = bit;

Label tekst1 = new Label
{
    Location = new Point(25, 30),
    Text = "midden x:",
    Size = new Size(75, 20)
};
Label tekst2 = new Label
{
    Location = new Point(25, 60),
    Text = "midden y:",
    Size = new Size(75, 20)
};
Label tekst3 = new Label
{
    Location = new Point(25, 90),
    Text = "schaal:",
    Size = new Size(75, 20)
};
Label tekst4 = new Label
{
    Location = new Point(25, 120),
    Text = "max aantal:",
    Size = new Size(75, 20)
};

scherm.Controls.Add(tekst1);
scherm.Controls.Add(tekst2);
scherm.Controls.Add(tekst3);
scherm.Controls.Add(tekst4);

TextBox invoerx = new TextBox
{
    Location = new Point(100, 30),
    Size = new Size(300, 20),
    Text = "0"
};
TextBox invoery = new TextBox
{
    Location = new Point(100, 60),
    Size = new Size(300, 20),
    Text = "0"
};
TextBox invoerschaal = new TextBox
{
    Location = new Point(100, 90),
    Size = new Size(300, 20),
    Text = "0,1"
};
TextBox invoerperforaties = new TextBox
{
    Location = new Point(100, 120),
    Size = new Size(100, 20),
    Text = "100"
};

scherm.Controls.Add(invoerx);
scherm.Controls.Add(invoery);
scherm.Controls.Add(invoerschaal);
scherm.Controls.Add(invoerperforaties);

Button knop = new Button
{
    Location = new Point(260, 120),
    Size = new Size(100, 20),
    Text = "Go!"
};

scherm.Controls.Add(knop);

// variabelen
double middenx = double.Parse(invoerx.Text);
double middeny = double.Parse(invoery.Text);
double schaal = double.Parse(invoerschaal.Text);
int maxAantal = int.Parse(invoerperforaties.Text);
double mandelgetal(double x, double y)
{
    double a = 0;
    double b = 0;
    int f = 0;

    while ((a * a + b * b) < 4 && f < maxAantal)
    {
        double da = a;
        double db = b;
        a = da * da - db * db + x;
        b = 2 * da * db + y;
        f++;
    }
    return f;
}

void knopp(object o, EventArgs e)
{
    schaal = double.Parse(invoerschaal.Text);
    middenx = double.Parse(invoerx.Text);
    middeny = double.Parse(invoery.Text);
    draws(middenx, middeny, schaal);
}

void muis(object o, MouseEventArgs mea)
{

    middenx = (mea.X - 250) * schaal + middenx;
    middeny = (mea.Y - 250) * schaal + middeny;
    if (mea.Button == MouseButtons.Left)
    {
        schaal *= 0.5;
    }
    else
    {
        schaal *= 2;
    }

    invoerx.Text = $"{middenx}";
    invoery.Text = $"{middeny}";
    invoerschaal.Text = $"{schaal}";
    draws(middenx, middeny, schaal);

}

void draws(double a, double b, double c)
{
    maxAantal = int.Parse(invoerperforaties.Text);
    int rij = 0;
    while (rij < 500)
    {
        int kolom = 0;
        while (kolom < 500)
        {
            if (mandelgetal((kolom - 250) * c + a, (rij - 250) * c + b) % 2 == 0)
            {
                bit.SetPixel(kolom, rij, Color.Black);
            }
            else
            {
                bit.SetPixel(kolom, rij, Color.White);
            }
            kolom++;
        }
        rij++;
    }
    mandelbrot.Invalidate();
}

void test(object o, MouseEventArgs mea)
{
    tekst1.Text = mandelgetal(((mea.X - 250) * schaal + middenx), ((mea.Y - 250) * schaal + middeny)).ToString();
    //tekst1.Text = ((mea.X - 250) * schaal + middenx).ToString();
    tekst2.Text = ((mea.Y - 250) * schaal + middeny).ToString();
}



mandelbrot.MouseMove += test;
mandelbrot.MouseClick += muis;
knop.Click += knopp;

Application.Run(scherm);