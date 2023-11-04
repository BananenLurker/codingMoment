using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class SchetsControl : UserControl
{
    public Schets schets;
    private Color penkleur;

    public TekenElementMaster Ophalen
    {
        get { return schets.Ophalen; }
    }
    public Color PenKleur
    { get { return penkleur; }
    }
    public Schets Schets
    { get { return schets;   }
    }
    public SchetsControl()
    {   this.BorderStyle = BorderStyle.Fixed3D;
        this.schets = new Schets();
        this.Paint += this.teken;
        this.Resize += this.veranderAfmeting;
        this.veranderAfmeting(null, null);
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }
    private void teken(object o, PaintEventArgs pea)
    {   schets.Teken(pea.Graphics);
    }
    private void veranderAfmeting(object o, EventArgs ea)
    {   schets.VeranderAfmeting(this.ClientSize);
        this.Invalidate();
    }
    public Graphics MaakBitmapGraphics()
    {   Graphics g = schets.BitmapGraphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        return g;
    }
    public void Schoon(object o, EventArgs ea)
    {   schets.Schoon();
        this.Invalidate();
    }
    public void Roteer(object o, EventArgs ea)
    {   schets.VeranderAfmeting(new Size(this.ClientSize.Height, this.ClientSize.Width));
        schets.Roteer();
        this.Invalidate();
    }
    public void Undo(object o, EventArgs ea)
    {
        List<TekenElement> tel = Ophalen.LijstOphalen;
        if (tel.Count > 0)
        {
            tel.RemoveAt(tel.Count - 1);
            this.Invalidate();
            Schets.Teken(MaakBitmapGraphics(), tel);
        }
    }
    public void Opslaan(object o, EventArgs ea)
    {
        foreach (TekenElement te in schets.Ophalen.TekenElementLijst)
        {
            Debug.WriteLine($"{te.Tool}");
            Debug.WriteLine($"{te.Kleur}");
            Debug.WriteLine($"{te.Punten[0]}");
            Debug.WriteLine($"{te.Punten[1]}");
            Debug.WriteLine($"----------------");
        }
    }
    public void VeranderKleur(object obj, EventArgs ea)
    {   string kleurNaam = ((ComboBox)obj).Text;
        penkleur = Color.FromName(kleurNaam);
    }
    public void VeranderKleurViaMenu(object obj, EventArgs ea)
    {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
        penkleur = Color.FromName(kleurNaam);
    }
}