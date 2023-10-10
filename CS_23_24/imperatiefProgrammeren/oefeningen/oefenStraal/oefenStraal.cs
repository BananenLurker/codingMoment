//using System;
//using System.Drawing;
//using System.Windows.Forms;

//Form scherm = new Form();

//scherm.ClientSize = new Size(400, 400);
//scherm.Text = "kaders";

//int aantal = 3;

//Point locatie = new Point(0, 0);

//void bewegen(object o, MouseEventArgs mea)
//{
//    locatie = mea.Location;
//    scherm.Invalidate();
//}

//void tekenen(object sender, PaintEventArgs pea)
//{
//    Graphics gr = pea.Graphics;
//    for (int i = 0; i <= aantal; i++)
//    {
//        int r = (aantal + 1 - i) * 10;
//        Brush brush = new SolidBrush(Color.FromArgb(255 * i / aantal, 255 * i / aantal, 255 * i / aantal));
//        gr.FillRectangle(brush, locatie.X - r, locatie.Y - r, 2 * r, 2 * r);
//    }
//}

//void klikken(object o, EventArgs ea)
//{
//    aantal++;
//    scherm.Invalidate();
//}

//scherm.MouseMove += bewegen;
//scherm.Paint += tekenen;
//scherm.MouseClick += klikken;

//Application.Run(scherm);

using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "T1";
scherm.ClientSize = new Size(200, 200);

int aantal = 1;
Point hier = new Point();

scherm.MouseClick += klikken;
scherm.Paint += tekenen;
scherm.MouseMove += bewegen;

void klikken(object o, EventArgs e)
{
    aantal++;
    scherm.Invalidate();
}

void bewegen(object sender, MouseEventArgs mea)
{
    hier = mea.Location;
    scherm.Invalidate();
}

void tekenen(object sender, PaintEventArgs pea)
{
    Graphics gr = pea.Graphics;
    for(int i = 0; i <= aantal; i++)
    {
        Brush brush = new SolidBrush(Color.FromArgb(0, 255 * i / aantal, 255));
        gr.FillRectangle(brush, 200 * i / aantal, hier.Y, 200 / aantal, 200 - hier.Y);
    }
}

Application.Run(scherm);