using System;
using System.Collections.Generic;
using System.Drawing;

public class TekenElementMaster
{
    public List<TekenElement> TekenElementLijst;
    public List<TekenElement> WeggehaaldLijst;

    public TekenElementMaster()
    {
        TekenElementLijst = new List<TekenElement>();
        WeggehaaldLijst = new List<TekenElement>();
    }
    public void VerwijderElement(Point p)
    {
        for (int i = TekenElementLijst.Count - 1; i >= 0; i--)
        {
            if (geraakt(TekenElementLijst[i], p))
            {
                Program.se.Gewijzigd();
                WeggehaaldLijst.Add(TekenElementLijst[i]);
                TekenElementLijst.RemoveAt(i);
                return;
            }
        }
    }
    public void Roteer(int width, int height)
    {
        foreach (TekenElement te in TekenElementLijst)
        {
            for (int i = 0; i < te.Punten.Count; i++)
            {
                int newX = -(te.Punten[i].Y - height / 2) + width / 2;
                int newY = te.Punten[i].X - width / 2 + height / 2;

                te.Punten[i] = new Point(newX, newY);
            }

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

    private bool geraakt(TekenElement te, Point p)
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

        SizeF sz = g.MeasureString(te.Letters, new Font("Tahoma", 40));
        Point startpunt = te.Punten[0];

        if (p.X > startpunt.X && p.X < startpunt.X + sz.Width && p.Y > startpunt.Y && p.Y < startpunt.Y + sz.Height)
        {
           return true;
        }
        return false;
    }

    private bool KaderGeraakt(TekenElement te, Point p)
    {
        Point startpunt = new Point(Math.Min(te.Punten[0].X, te.Punten[1].X), Math.Min(te.Punten[0].Y, te.Punten[1].Y));
        Point einde = new Point(Math.Max(te.Punten[0].X, te.Punten[1].X), Math.Max(te.Punten[0].Y, te.Punten[1].Y));

        bool opOmtrekX = Math.Abs(p.X - startpunt.X) <= 5 || Math.Abs(p.X - einde.X) <= 5;
        bool opOmtrekY = Math.Abs(p.Y - startpunt.Y) <= 5 || Math.Abs(p.Y - einde.Y) <= 5;

        return p.X >= startpunt.X - 5 && p.Y >= startpunt.Y - 5 && p.X <= einde.X + 5 && p.Y <= einde.Y + 5 && (opOmtrekX || opOmtrekY);
    }

    private bool VlakGeraakt(TekenElement te, Point p)
    {
        Point startpunt = new Point(Math.Min(te.Punten[0].X, te.Punten[1].X), Math.Min(te.Punten[0].Y, te.Punten[1].Y));
        Point einde = new Point(Math.Max(te.Punten[0].X, te.Punten[1].X), Math.Max(te.Punten[0].Y, te.Punten[1].Y));
        return p.X > startpunt.X && p.Y > startpunt.Y && p.X < einde.X && p.Y < einde.Y;
    }

    private bool OvaalGeraakt(TekenElement te, Point p)
    {
        int centerX = (te.Punten[0].X + te.Punten[1].X) / 2;
        int centerY = (te.Punten[0].Y + te.Punten[1].Y) / 2;
        int halfWidth = Math.Abs(te.Punten[0].X - te.Punten[1].X) / 2;
        int halfHeight = Math.Abs(te.Punten[0].Y - te.Punten[1].Y) / 2;

        for (int angle = 0; angle < 360; angle++)
        {
            double radians = angle * Math.PI / 180;
            int x = centerX + (int)(halfWidth * Math.Cos(radians));
            int y = centerY + (int)(halfHeight * Math.Sin(radians));

            int deltaX = p.X - x;
            int deltaY = p.Y - y;
            double afstand = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (afstand <= 5)
            {
                return true;
            }
        }

        return false;
    }

    private bool DiscGeraakt(TekenElement te, Point p)
    {
        int centerX = (te.Punten[0].X + te.Punten[1].X) / 2;
        int centerY = (te.Punten[0].Y + te.Punten[1].Y) / 2;
        int radius = Math.Abs(te.Punten[0].X - te.Punten[1].X) / 2;

        int deltaX = p.X - centerX;
        int deltaY = p.Y - centerY;

        return deltaX * deltaX + deltaY * deltaY <= radius * radius;
    }

    private bool LijnGeraakt(TekenElement te, Point p)
    {
        int d = Math.Abs((te.Punten[0].X - te.Punten[1].X) * (te.Punten[0].Y - p.Y) -
                         (te.Punten[0].Y - te.Punten[1].Y) * (te.Punten[0].X - p.X)) /
                 (int)Math.Sqrt(Math.Pow(te.Punten[0].X - te.Punten[1].X, 2) + Math.Pow(te.Punten[0].Y - te.Punten[1].Y, 2));

        return d <= 5;
    }

    private bool PenGeraakt(TekenElement te, Point p)
    {
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