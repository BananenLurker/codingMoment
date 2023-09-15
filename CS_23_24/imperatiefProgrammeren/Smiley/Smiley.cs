using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "Smiley";
scherm.BackColor = Color.White;
scherm.ClientSize = new Size(1000, 500);

Bitmap smiley = new Bitmap(800, 400);
Label afbeelding = new Label();

scherm.Controls.Add(afbeelding);
afbeelding.Location = new Point(0, 0);
afbeelding.Size = new Size(800, 400);
afbeelding.BackColor = Color.White;
afbeelding.Image = smiley;
Graphics gr = Graphics.FromImage(smiley);

Pen penZwart = new Pen(Color.Black, 2);
int xcoord = 100;
int ycoord = 100;
int grootte = 200;

gr.FillEllipse(Brushes.Yellow, xcoord, ycoord, grootte, grootte);
gr.DrawEllipse(penZwart, xcoord, ycoord, grootte, grootte);

Application.Run(scherm);