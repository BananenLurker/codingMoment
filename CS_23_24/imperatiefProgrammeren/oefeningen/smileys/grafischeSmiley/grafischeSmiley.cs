using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "Smiley";
scherm.BackColor = Color.White;
scherm.ClientSize = new Size(1200, 500);

Bitmap smiley = new Bitmap(800, 400);
Label afbeelding = new Label();

scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(0, 0);
afbeelding.Size = new Size(800, 400);
afbeelding.BackColor = Color.LightBlue;
afbeelding.Image = smiley;
Graphics gr = Graphics.FromImage(smiley);

Label labelX = new Label();
labelX.Location = new Point(825, 50);
labelX.Size = new Size(250, 25);
labelX.Text = "X-coordinaat van de smiley (< ~800):";

Label labelY = new Label();
labelY.Location = new Point(825, 125);
labelY.Size = new Size(250, 25);
labelY.Text = "Y-coordinaat van de smiley(< ~400):";

Label grootteLabel = new Label();
grootteLabel.Location = new Point(825, 200);
grootteLabel.Size = new Size(175, 25);
grootteLabel.Text = "Grootte van de smiley(> 0)";

Label blijLabel = new Label();
blijLabel.Location = new Point(825, 275);
blijLabel.Size = new Size(175, 25);
blijLabel.Text = "Blijdschap (1 - 10)";

scherm.Controls.Add(labelX);
scherm.Controls.Add(labelY);
scherm.Controls.Add(grootteLabel);
scherm.Controls.Add(blijLabel);

TextBox entryX = new TextBox();
entryX.Location = new Point(825, 75);
entryX.Size = new Size(100, 50);
entryX.Text = "100";
TextBox entryY = new TextBox();
entryY.Location = new Point(825, 150);
entryY.Size = new Size(100, 50);
entryY.Text = "100";
TextBox grootteText = new TextBox();
grootteText.Location = new Point(825, 225);
grootteText.Size = new Size(100, 50);
grootteText.Text = "100";
TextBox blijText = new TextBox();
blijText.Location = new Point(825, 300);
blijText.Size = new Size(100, 50);
blijText.Text = "5";

scherm.Controls.Add(entryX);
scherm.Controls.Add(entryY);
scherm.Controls.Add(grootteText);
scherm.Controls.Add(blijText);

Button tekenSmiley = new Button();
tekenSmiley.Location = new Point(875, 375);
tekenSmiley.Size = new Size(100, 50);
tekenSmiley.Text = "Teken smiley 1!";
int aantal = 1;

scherm.Controls.Add(tekenSmiley);

System.Drawing.Brush brushKleurOgen = Brushes.Black;
System.Drawing.Brush brushKleurGezicht = Brushes.Yellow;

void smileyTekenen(object o, EventArgs e)
{
    try
    {
        Int32.Parse(grootteText.Text);
        Int32.Parse(entryX.Text);
        Int32.Parse(entryY.Text);
        Int32.Parse(blijText.Text);
    }
    catch (Exception)
    {
        grootteLabel.Text = "Er gaat iets fout. Sad smiley.";
        return;
    }
    if(Int32.Parse(grootteText.Text) == 0)
    {
        grootteLabel.Text = "Grappenmaker";
        return;
    }

    int grootte = Int32.Parse(grootteText.Text);
    int xcoord = Int32.Parse(entryX.Text);
    int ycoord = Int32.Parse(entryY.Text);
    int blijdschap = Int32.Parse(blijText.Text);

    int ogenSize = grootte / 8;
    int penDikte = grootte / 30;
    Pen penZwart = new Pen(Color.Black, penDikte);

    PointF startPoint = new PointF(xcoord + grootte / 5, ycoord + 3 * grootte / 5);
    PointF controlPoint1 = new PointF(xcoord + 2 * grootte / 5, ycoord + 4 * grootte * (blijdschap + 10) / 100);
    PointF controlPoint2 = new PointF(xcoord + 3 * grootte / 5, ycoord + 4 * grootte * (blijdschap + 10) / 100);
    PointF endPoint = new PointF(xcoord + 4 * grootte / 5, ycoord + 3 * grootte / 5);

    gr.FillEllipse(brushKleurGezicht, xcoord, ycoord, grootte, grootte);
    gr.DrawEllipse(penZwart, xcoord, ycoord, grootte, grootte);
    gr.FillEllipse(brushKleurOgen, xcoord + grootte / 4 - ogenSize / 2, ycoord + grootte / 4, ogenSize, ogenSize);
    gr.FillEllipse(brushKleurOgen, xcoord + grootte - grootte / 4 - ogenSize / 2, ycoord + grootte / 4, ogenSize, ogenSize);
    gr.DrawBezier(penZwart, startPoint, controlPoint1, controlPoint2, endPoint);

    afbeelding.Invalidate();

    aantal++;
    tekenSmiley.Text = $"Teken smiley {aantal}!";
    grootteLabel.Text = "Grootte van de smiley (>0):";
}

tekenSmiley.Click += smileyTekenen;

Application.Run(scherm);