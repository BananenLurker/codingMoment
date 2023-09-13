using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "knoppenklikkerteller";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(800, 500);

Button teller = new Button();
scherm.Controls.Add(teller);
teller.Location = new Point(50, 50);
teller.Size = new Size(300, 100);
teller.Text = "Je hebt 0 keer geklikt";

int aantal = 0;
void tellenmaar(object o, EventArgs e)
{
    aantal++;
    teller.Text = $"Je hebt {aantal} keer geklikt";

}

teller.Click += tellenmaar;

Application.Run(scherm);