using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class SchetsControl : UserControl
{
    public Schets schets;
    private Color penkleur = Color.Black;

    // Een property die de TEM ophaalt bij de Schets die bij deze SchetsControl hoort
    public TekenElementMaster Ophalen
    {
        get { return schets.Ophalen; }
    }
    // Weinig spannende methoden die vanuit de source code beschikbaar zijn
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
        // Undo: haal de huidige TekenElementLijst op
        List<TekenElement> tel = Ophalen.TekenElementLijst;
        if (tel.Count > 0)
        {
            // Als deze niet leeg is, voeg het laatste element toe aan een lijst met
            // andere weggehaalde elementen en haal het uit de lijst met op dit moment
            // zichtbare elementen: teken daarna opnieuw.
            Ophalen.WeggehaaldLijst.Add(tel[tel.Count - 1]);
            tel.RemoveAt(tel.Count - 1);
            OpnieuwTekenen(tel);
            Program.se.Gewijzigd();
        }
    }
    public void Redo(object o, EventArgs ea)
    {
        // Haal de huidige lijst en de weggehaalde lijst op
        List<TekenElement> tel = Ophalen.TekenElementLijst;
        List<TekenElement> weg = Ophalen.WeggehaaldLijst;
        if (weg.Count > 0)
        {
            // Als er iets in de weggehaalde lijst zit, voeg deze dan weer toe aan de
            // huidige lijst en haal hem weg uit de weggehaalde lijst.
            // Hierdoor kunnen elementen nooit twee keer over elkaar heen toegevoegd worden
            tel.Add(weg[weg.Count - 1]);
            weg.RemoveAt(weg.Count - 1);
            OpnieuwTekenen(tel);
            Program.se.Gewijzigd();
        }
    }
    public void OpnieuwTekenen(List<TekenElement> tel)
    {
        // Geef de huidige lijst met getekende elementen aan de Teken functie,
        // die alles opnieuw mooi gaat tekenen
        this.Invalidate();
        Schets.Teken(MaakBitmapGraphics(), tel);
    }
    public void VeranderKleur(Object o, EventArgs ea)
    {
        // Verander de kleur aan de hand van een kleurdialoog:
        // dit geeft een ontzettend grote keuze aan verschillende kleuren
        ColorDialog colorDlg = new ColorDialog();
        colorDlg.AllowFullOpen = true;
        colorDlg.AnyColor = true;
        colorDlg.SolidColorOnly = false;
        colorDlg.Color = Color.Red;

        if (colorDlg.ShowDialog() == DialogResult.OK)
        {
            penkleur = colorDlg.Color;
        }
    }
}