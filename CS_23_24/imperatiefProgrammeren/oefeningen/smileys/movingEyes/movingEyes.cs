using System;
using System.Drawing;
using System.Windows.Forms;

Size size = new Size(800, 400);
Point muis = new Point(0, 0);
int penDikte = 3;
Pen penZwart = new Pen(Color.Black, penDikte);
System.Drawing.Brush brushBlauw = Brushes.Blue;
double oogGrootte = 150;
int grootte = 20;
int xcoord = 100;
int ycoord = 100;
double middenX = xcoord + oogGrootte/2;
double middenY = ycoord + oogGrootte/2;
double middenX2 = xcoord + 2 * oogGrootte;

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
    double e = oogGrootte / 2 - grootte / 2;
    double dx = muis.X - middenX;
    double dy = muis.Y - middenY;
    double d = Math.Sqrt(dx * dx + dy * dy);

    double ex = dx * (e / d);
    double ey = dy * (e / d);

    double dx2 = muis.X - middenX2;
    double d2 = Math.Sqrt(dx2 * dx2 + dy * dy);
    double ex2 = dx2 * (e / d2);
    double ey2 = dy * (e / d2);

    Graphics gr = pea.Graphics;
    gr.DrawEllipse(penZwart, xcoord, ycoord, (int)oogGrootte, (int)oogGrootte);
    gr.DrawEllipse(penZwart, xcoord + (int)(oogGrootte + oogGrootte / 2), ycoord, (int)oogGrootte, (int)oogGrootte);
    gr.FillEllipse(brushBlauw, (int)ex + (int)middenX - grootte / 2, (int)ey + (int)middenY - grootte / 2, grootte, grootte);
    gr.FillEllipse(brushBlauw, (int)ex2 + (int)middenX2 - grootte / 2, (int)ey2 + (int)middenY - grootte / 2, grootte, grootte);
}

scherm.MouseMove += bewegen;
scherm.Paint += ogenBewegen;

Application.Run(scherm);