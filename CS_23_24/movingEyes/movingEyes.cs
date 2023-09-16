using System;
using System.Drawing;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;

Size size = new Size(800, 400);
Point muis = new Point(0, 0);
int penDikte = 3;
Pen penZwart = new Pen(Color.Black, penDikte);
System.Drawing.Brush brushZwart = Brushes.Blue;
int oogGrootte = 150;
int grootte = 20;
int xcoord = 100;
int ycoord = 100;
int middenX = xcoord + oogGrootte/2;
int middenY = ycoord + oogGrootte/2;

Form scherm = new Form();
scherm.Text = "Bewegende oogjes";
scherm.ClientSize = size;

void bewegen(object sender, MouseEventArgs mea)
{
    muis = mea.Location;
    scherm.Invalidate();
}
void ogenBewegen(object sender, PaintEventArgs pea)
{
    int e = oogGrootte / 2 + grootte / 2;
    int dx = muis.X - middenX;
    int dey = muis.Y - middenY;
    int de = (int)Math.Sqrt(dx * dx + dey * dey);
    int d = de - e;
    int ex = (dx / d) * e;
    int ey = (int)Math.Sqrt(e * e - ex * ex);
    int dy = dey - ey;
    int xoog = muis.X - grootte / 2;
    int yoog = muis.Y - grootte / 2;
    Graphics gr = pea.Graphics;
    gr.DrawEllipse(penZwart, xcoord, ycoord, oogGrootte, oogGrootte);
    gr.DrawEllipse(penZwart, xcoord + oogGrootte + 50, ycoord, oogGrootte, oogGrootte);
    gr.DrawEllipse(penZwart, ex, ey, 10, 10);
    //gr.FillEllipse(brushZwart, ex, dy, grootte, grootte);
}

scherm.MouseMove += bewegen;
scherm.Paint += ogenBewegen;

Application.Run(scherm);