using System;
using System.Drawing;
using System.Windows.Forms;

Form scherm = new Form();
scherm.Text = "knoppenklikkerteller";
scherm.BackColor = Color.LightYellow;
scherm.ClientSize = new Size(800, 500);

Label label = new Label();
scherm.Controls.Add(label);
label.Location = new Point(400, 100);
label.Size = new Size(200, 100);
label.Text = "Je hebt 0 keer geklikt";

Button teller = new Button();
scherm.Controls.Add(teller);
teller.Location = new Point(50, 50);
teller.Size = new Size(300, 100);
teller.Text = "p-p-pwease pwess me o-onii-chan UwU";

int aantal = 0;
void tellenmaar(object o, EventArgs e)
{
    aantal++;
    label.Text = $"Je hebt {aantal} keer geklikt";
}

teller.Click += tellenmaar;

Application.Run(scherm);