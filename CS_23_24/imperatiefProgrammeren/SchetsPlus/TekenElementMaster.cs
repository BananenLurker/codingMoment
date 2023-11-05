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
            for(int i = 0; i < te.Punten.Count; i++)
            {
                int newX = -(te.Punten[i].Y - height / 2) + width / 2;
                int newY = te.Punten[i].X - width / 2 + height / 2;

                te.Punten[i] = new Point(newX, newY);
            }

            if(te.Tool == "tekst")
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
    //private bool geraakt(TekenElement te, Point p)
    //{
    //    switch (te.Tool)
    //    {
    //        case "tekst":
    //            return TekstGeraakt(te, p);
    //        case "kader":
    //            return KaderGeraakt(te, p);
    //        case "vlak":
    //            return VlakGeraakt(te, p);
    //        case "ovaal":
    //            return OvaalGeraakt(te, p);
    //        case "disc":
    //            return DiscGeraakt(te, p);
    //        case "lijn":
    //            return LijnGeraakt(te, p);
    //        case "pen":
    //            return PenGeraakt(te, p);
    //    }
    //    return false;
    //}
    private bool geraakt(TekenElement te, Point p)
    {
        switch (te.Tool)
        {
            case "tekst":
                return VlakGeraakt(te, p);
            case "kader":
                return VlakGeraakt(te, p);
            case "vlak":
                return VlakGeraakt(te, p);
            case "ovaal":
                return VlakGeraakt(te, p);
            case "disc":
                return VlakGeraakt(te, p);
            case "lijn":
                return VlakGeraakt(te, p);
            case "pen":
                return VlakGeraakt(te, p);
        }
        return false;
    }
    private bool VlakGeraakt(TekenElement te, Point p)
    {
        Point begin = new Point(Math.Min(te.Punten[0].X, te.Punten[1].X), Math.Min(te.Punten[0].Y, te.Punten[1].Y));
        Point einde = new Point(Math.Max(te.Punten[0].X, te.Punten[1].X), Math.Max(te.Punten[0].Y, te.Punten[1].Y));
        if (p.X > begin.X && p.Y > begin.Y && p.X < einde.X && p.Y < einde.Y)
        {
            return true;
        }
        return false;
    }
}