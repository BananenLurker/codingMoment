using System;
using System.Collections.Generic;
using System.Drawing;

public class TekenElementMaster
{
    // Deze klasse beheert alle TekenElementen en houdt ook een lijst met weggehaalde elementen bij voor de undo en redo methods
    public List<TekenElement> TekenElementLijst;
    public List<TekenElement> WeggehaaldLijst;

    public TekenElementMaster()
    {
        TekenElementLijst = new List<TekenElement>();
        WeggehaaldLijst = new List<TekenElement>();
    }
    // Verwijderd een element waanneer erop geklikt wordt
    public void VerwijderElement(Point p)
    {
        for (int i = TekenElementLijst.Count - 1; i >= 0; i--)
        {
            if (Geraakt(TekenElementLijst[i], p))
            {
                Program.se.Gewijzigd();
                WeggehaaldLijst.Add(TekenElementLijst[i]);
                TekenElementLijst.RemoveAt(i);
                return;
            }
        }
    }
    // Gaat van nieuw naar oud door de lijst heen en geeft het nieuwste
    // TekenElement dat geraakt is terug aan de vorige methode
    public TekenElement ElementSelectie(Point p)
    {
        for (int i = TekenElementLijst.Count - 1; i >= 0; i--)
        {
            if (Geraakt(TekenElementLijst[i], p))
            {
                TekenElement temp = TekenElementLijst[i];
                TekenElementLijst.RemoveAt(i);
                Program.se.Gewijzigd();
                return temp;
            }
        }
        return null;
    }
    // Zoekt het bovenste element dat gesleept wordt door de move tool
    public TekenElement ZoekDragElement(Point p)
    {
        for (int i = TekenElementLijst.Count - 1; i >= 0; i--)
        {
            if (Geraakt(TekenElementLijst[i], p))
            {
                Program.se.Gewijzigd();
                return TekenElementLijst[i];
            }
        }
        return null;
    }
    // Roteert ieder element
    public void Roteer(int width, int height)
    {
        foreach (TekenElement te in TekenElementLijst)
        {
            for (int i = 0; i < te.Punten.Count; i++)
            {
                // Roteer ieder element adhv het kwadrant waar zij in leven: beweeg met de klok mee
                int newX = -(te.Punten[i].Y - height / 2) + width / 2;
                int newY = te.Punten[i].X - width / 2 + height / 2;

                te.Punten[i] = new Point(newX, newY);
            }

            // Als het element tekst is, wordt deze alsnog scheef getekend. Geef daarom een hoek mee die later gebruikt wordt
            // om de tekst juist te tekenen
            if (te.Tool == "tekst")
            {
                if (te.Hoek == 270)
                {
                    te.Hoek = 0;
                }
                else
                {
                    te.Hoek += 90;
                }

            }
        }
    }

    // Check welk type element geraakt is
    private bool Geraakt(TekenElement te, Point p)
    {
        switch (te.Tool)
        {
            case "tekst":
                return TekstGeraakt(te, p);
            case "kader":
                return KaderGeraakt(te, p);
            case "vlak":
                return VlakGeraakt(te, p);
            case "ovaal":
                return OvaalGeraakt(te, p);
            case "disc":
                return DiscGeraakt(te, p);
            case "lijn":
                return LijnGeraakt(te, p);
            case "pen":
                return PenGeraakt(te, p);
        }
        return false;
    }


    private bool TekstGeraakt(TekenElement te, Point p)
    {
        Graphics g = Graphics.FromImage(new Bitmap(1, 1));

        // Meet de string op
        SizeF sz = g.MeasureString(te.Letters, new Font("Tahoma", 40));
        Point startpunt = te.Punten[0];

        // Als het geklikte punt binnen de gemeette string ligt, is er op geklikt
        if (p.X > startpunt.X && p.X < startpunt.X + sz.Width && p.Y > startpunt.Y && p.Y < startpunt.Y + sz.Height)
        {
           return true;
        }
        return false;
    }

    private bool KaderGeraakt(TekenElement te, Point p)
    {
        // Bereken het begin en eindpunt waar het kader tussen leeft
        Point startpunt = new Point(Math.Min(te.Punten[0].X, te.Punten[1].X), Math.Min(te.Punten[0].Y, te.Punten[1].Y));
        Point einde = new Point(Math.Max(te.Punten[0].X, te.Punten[1].X), Math.Max(te.Punten[0].Y, te.Punten[1].Y));

        // Bereken de omtrek, zodat 'door het kader heen' geklikt kan worden
        bool opOmtrekX = Math.Abs(p.X - startpunt.X) <= 5 || Math.Abs(p.X - einde.X) <= 5;
        bool opOmtrekY = Math.Abs(p.Y - startpunt.Y) <= 5 || Math.Abs(p.Y - einde.Y) <= 5;

        // Geef 5 pixels ruimte, zodat er niet precies op het kader geklikt hoeft te worden.
        return p.X >= startpunt.X - 5 && p.Y >= startpunt.Y - 5 && p.X <= einde.X + 5 && p.Y <= einde.Y + 5 && (opOmtrekX || opOmtrekY);
    }

    private bool VlakGeraakt(TekenElement te, Point p)
    {
        // Kijk of er geklikt is tussen de coördinaten waar het vlak leeft, er kan immers niet door een vak heen geklikt worden.
        Point startpunt = new Point(Math.Min(te.Punten[0].X, te.Punten[1].X), Math.Min(te.Punten[0].Y, te.Punten[1].Y));
        Point einde = new Point(Math.Max(te.Punten[0].X, te.Punten[1].X), Math.Max(te.Punten[0].Y, te.Punten[1].Y));
        return p.X > startpunt.X && p.Y > startpunt.Y && p.X < einde.X && p.Y < einde.Y;
    }

    private bool OvaalGeraakt(TekenElement te, Point p)
    {
        // Bereken het midden en de straal van de ovaal
        int centerX = (te.Punten[0].X + te.Punten[1].X) / 2;
        int centerY = (te.Punten[0].Y + te.Punten[1].Y) / 2;
        int halfWidth = Math.Abs(te.Punten[0].X - te.Punten[1].X) / 2;
        int halfHeight = Math.Abs(te.Punten[0].Y - te.Punten[1].Y) / 2;

        for (int angle = 0; angle < 360; angle++)
        {
            // Bereken hoe ver de getekende ovaal van het midden af ligt
            double radians = angle * Math.PI / 180;
            int x = centerX + (int)(halfWidth * Math.Cos(radians));
            int y = centerY + (int)(halfHeight * Math.Sin(radians));

            int deltaX = p.X - x;
            int deltaY = p.Y - y;

            if (deltaX * deltaX + deltaY * deltaY <= 25)
            {
                return true;
            }
        }

        return false;
    }

    private bool DiscGeraakt(TekenElement te, Point p)
    {
        // Bereken het midden van de ovaal
        int centerX = (te.Punten[0].X + te.Punten[1].X) / 2;
        int centerY = (te.Punten[0].Y + te.Punten[1].Y) / 2;
        int radius = Math.Abs(te.Punten[0].X - te.Punten[1].X) / 2;

        int deltaX = p.X - centerX;
        int deltaY = p.Y - centerY;
        // Hoe ver de getekende ovaal van het midden af ligt is nu niet relevant: het gaat enkel om de oppervlakte
        return deltaX * deltaX + deltaY * deltaY <= radius * radius;
    }

    private bool LijnGeraakt(TekenElement te, Point p)
    {
        // Formule voor afstand punt tot lijn
        int d = Math.Abs((te.Punten[0].X - te.Punten[1].X) * (te.Punten[0].Y - p.Y) -
                         (te.Punten[0].Y - te.Punten[1].Y) * (te.Punten[0].X - p.X)) /
                 (int)Math.Sqrt(Math.Pow(te.Punten[0].X - te.Punten[1].X, 2) + Math.Pow(te.Punten[0].Y - te.Punten[1].Y, 2));

        return d <= 5;
    }

    private bool PenGeraakt(TekenElement te, Point p)
    {
        // Voor ieder punt dat opgeslagen zit in TekenElement: formule voor afstand punt tot punt
        foreach (Point punt in te.Punten)
        {
            if (Math.Pow(punt.X - p.X, 2) + Math.Pow(punt.Y - p.Y, 2) <= 25)
            {
                return true;
            }
        }
        return false;
    }
}