using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "Smiley op muis";
scherm.BackColor = Color.White;
scherm.ClientSize = new Size(1000, 500);

System.Drawing.Brush brushKleurOgen = Brushes.Black;
System.Drawing.Brush brushKleurGezicht = Brushes.Yellow;
int grootte = 100;
int blijdschap = 10;

int ogenSize = grootte / 8;
int penDikte = grootte / 30;
Pen penZwart = new Pen(Color.Black, penDikte);

Point hier = new Point(0, 0);

void bewegen(object sender, MouseEventArgs mea)
{
    hier = mea.Location;
    scherm.Invalidate();
}
void smileyTekenen(object sender, PaintEventArgs pea)
{ 
    PointF startPoint = new PointF(hier.X + grootte / 5 - grootte / 2, hier.Y + 3 * grootte / 5 - grootte / 2);
    PointF controlPoint1 = new PointF(hier.X + 2 * grootte / 5 - grootte / 2, hier.Y + 4 * grootte * (blijdschap + 10) / 100 - grootte / 2);
    PointF controlPoint2 = new PointF(hier.X + 3 * grootte / 5 - grootte / 2, hier.Y + 4 * grootte * (blijdschap + 10) / 100 - grootte / 2);
    PointF endPoint = new PointF(hier.X + 4 * grootte / 5 - grootte / 2, hier.Y + 3 * grootte / 5 - grootte / 2);

    Graphics gr = pea.Graphics;
    gr.FillEllipse(brushKleurGezicht, hier.X - grootte/2, hier.Y - grootte / 2, grootte, grootte);
    gr.DrawEllipse(penZwart, hier.X - grootte / 2, hier.Y - grootte / 2, grootte, grootte);
    gr.FillEllipse(brushKleurOgen, hier.X + grootte / 4 - ogenSize / 2 - grootte / 2, hier.Y + grootte / 4 - grootte / 2, ogenSize, ogenSize);
    gr.FillEllipse(brushKleurOgen, hier.X + grootte - grootte / 4 - ogenSize / 2 - grootte / 2, hier.Y + grootte / 4 - grootte / 2, ogenSize, ogenSize);
    gr.DrawBezier(penZwart, startPoint, controlPoint1, controlPoint2, endPoint);
}

scherm.MouseMove += bewegen;
scherm.Paint += smileyTekenen;

Application.Run(scherm);