using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

public interface ISchetsTool
{
    void MuisVast(SchetsControl s, Point p);
    void MuisDrag(SchetsControl s, Point p);
    void MuisLos(SchetsControl s, Point p);
    void Letter(SchetsControl s, char c);
}

public abstract class StartpuntTool : ISchetsTool
{
    public SchetsControl SchetsC;

    protected Point startpunt;
    protected Brush kwast;
    protected TekenElement te;
    protected TekenElementMaster tem;

    public virtual void MuisVast(SchetsControl s, Point p)
    {
        SchetsC = s;
        startpunt = p;
        te = new TekenElement();
        te.Punten.Add(p);
        te.Tool = ToString();
        te.Kleur = s.PenKleur;

        s.schets.tem.TekenElementLijst.Add(te);
    }
    public virtual void MuisLos(SchetsControl s, Point p)
    {   
        kwast = new SolidBrush(s.PenKleur);
        te.Punten.Add(p);
    }
    public abstract void MuisDrag(SchetsControl s, Point p);
    public abstract void Letter(SchetsControl s, char c);
}

public class TekstTool : StartpuntTool
{
    public override string ToString() { return "tekst"; }

    public override void MuisDrag(SchetsControl s, Point p) { }

    public override void Letter(SchetsControl s, char c)
    {
        if (c >= 32)
        {
            Graphics gr = s.MaakBitmapGraphics();
            Font font = new Font("Tahoma", 40);
            string tekst = c.ToString();
            SizeF sz = 
            gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
            gr.DrawString   (tekst, font, kwast, 
                                            this.startpunt, StringFormat.GenericTypographic);
            // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
            startpunt.X += (int)sz.Width;
            te.Letters.Add(c);
            s.Invalidate();
        }
    }
}

public abstract class TweepuntTool : StartpuntTool
{
    public static Ellipse Punten2Ovaal(Point p1, Point p2)
    {
        return new Ellipse(p1, p2);
    }
    public static Rectangle Punten2Rechthoek(Point p1, Point p2)
    {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                            , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                            );
    }
    public static Pen MaakPen(Brush b, int dikte)
    {   Pen pen = new Pen(Brushes.Black, 3);
        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;
        return pen;
    }
    public override void MuisVast(SchetsControl s, Point p)
    {   base.MuisVast(s, p);
        kwast = Brushes.Gray;
    }
    public override void MuisDrag(SchetsControl s, Point p)
    {   s.Refresh();
        this.Bezig(s.CreateGraphics(), this.startpunt, p);
    }
    public override void MuisLos(SchetsControl s, Point p)
    {   base.MuisLos(s, p);
        this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
        Program.se.Gewijzigd();
        s.Invalidate();
    }
    public override void Letter(SchetsControl s, char c)
    {
    }
    public abstract void Bezig(Graphics g, Point p1, Point p2);
        
    public virtual void Compleet(Graphics g, Point p1, Point p2)
    {   this.Bezig(g, p1, p2);
    }
}

public struct Ellipse
{
    private Point StartPunt;
    private int Breedte;
    private int Hoogte;

    public Ellipse(Point p1, Point p2)
    {
        StartPunt = p1;

        Breedte = p2.X - p1.X;
        Hoogte = p2.Y - p1.Y;
    }
    public Rectangle OvaalOmtrek()
    {
        return new Rectangle(StartPunt.X, StartPunt.Y, Breedte, Hoogte);
    }
}


public class RechthoekTool : TweepuntTool
{
    public override string ToString() { return "kader"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {   g.DrawRectangle(MaakPen(kwast,3), TweepuntTool.Punten2Rechthoek(p1, p2));
    }
}
public class VolRechthoekTool : RechthoekTool
{
    public override string ToString() { return "vlak"; }

    public override void Compleet(Graphics g, Point p1, Point p2)
    {   g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
    }
}

public class OvaalTool : TweepuntTool
{
    public override string ToString() { return "ovaal"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        Ellipse ellipse = TweepuntTool.Punten2Ovaal(p1, p2);
        g.DrawEllipse(MaakPen(kwast, 3), ellipse.OvaalOmtrek());
    }
}

public class VolOvaalTool : TweepuntTool
{
    public override string ToString() { return "disc"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        Ellipse ellipse = TweepuntTool.Punten2Ovaal(p1, p2);
        g.FillEllipse(kwast, ellipse.OvaalOmtrek());
    }
}


public class LijnTool : TweepuntTool
{
    public override string ToString() { return "lijn"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {   g.DrawLine(MaakPen(this.kwast,3), p1, p2);
    }
}

public class PenTool : LijnTool
{
    public override string ToString() { return "pen"; }

    public override void MuisDrag(SchetsControl s, Point p)
    {   this.MuisLos(s, p);
        this.MuisVast(s, p);
    }
}

public class GumTool : PenTool
{
    public List<TekenElement> tel;
    public override string ToString() { return "gum"; }
    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        tel = SchetsC.schets.tem.TekenElementLijst;
        foreach (TekenElement te in tel)
        {
            switch (te.Tool)
            {
                case "kader":
                    if (p1.X > te.Punten[0].X && p1.Y > te.Punten[0].Y && p1.X < te.Punten[1].X && p1.Y < te.Punten[1].Y)
                    {
                        VerwijderElement(te, g, p1, p2);
                        Debug.WriteLine("kader yup " + $"{te.Kleur}");
                        return;
                    }
                    break;
                case "vlak":
                    if (p1.X > te.Punten[0].X && p1.Y > te.Punten[0].Y && p1.X < te.Punten[1].X && p1.Y < te.Punten[1].Y)
                    {
                        Debug.WriteLine("vlak yup " + $"{te.Kleur}");
                        return;
                    }
                    break;
                case "ovaal":
                    if (p1.X > te.Punten[0].X && p1.Y > te.Punten[0].Y && p1.X < te.Punten[1].X && p1.Y < te.Punten[1].Y)
                    {
                        Debug.WriteLine("ovaal yup " + $"{te.Kleur}");
                        return;
                    }
                    break;
                case "disc":
                    if (p1.X > te.Punten[0].X && p1.Y > te.Punten[0].Y && p1.X < te.Punten[1].X && p1.Y < te.Punten[1].Y)
                    {
                        Debug.WriteLine("disc yup " + $"{te.Kleur}");
                        return;
                    }
                    break;
                case "lijn":
                    if (p1.X > te.Punten[0].X && p1.Y > te.Punten[0].Y && p1.X < te.Punten[1].X && p1.Y < te.Punten[1].Y)
                    {
                        Debug.WriteLine("lijn yup " + $"{te.Kleur}");
                        return;
                    }
                    break;
                case "pen":
                    if (p1.X > te.Punten[0].X && p1.Y > te.Punten[0].Y && p1.X < te.Punten[1].X && p1.Y < te.Punten[1].Y)
                    {
                        Debug.WriteLine("pen yup " + $"{te.Kleur}");
                        return;
                    }
                    break;
            }
        }
    }
    private void VerwijderElement(TekenElement te, Graphics g, Point p1, Point p2)
    {
        tel.Remove(te);
        SchetsC.Schoon(this, null);
        OpnieuwTekenen(g, p1, p2);
        SchetsC.Invalidate();
    }
    // Moet op basis van MuisVast en MuisLos gemaakt worden, niet op basis van Bezig!
    private void OpnieuwTekenen(Graphics g, Point p1, Point p2)
    {
        foreach (TekenElement te in tel)
        {
            kwast = new SolidBrush(te.Kleur);
            Debug.WriteLine(((SolidBrush)kwast).Color);
            switch (te.Tool)
            {
                case "kader":
                    RechthoekTool rht = new RechthoekTool();
                    rht.Compleet(g, p1, p2);
                    break;
                case "vlak":
                    VolRechthoekTool vrt = new VolRechthoekTool();
                    vrt.Compleet(g, p1, p2);
                    break;
                case "ovaal":
                    OvaalTool ot = new OvaalTool();
                    ot.Compleet(g, p1, p2);
                    break;
                case "disc":
                    VolOvaalTool vot = new VolOvaalTool();
                    vot.Compleet(g, p1, p2);
                    break;
                case "lijn":
                    LijnTool lt = new LijnTool();
                    lt.Compleet(g, p1, p2);
                    break;
                case "pen":
                    PenTool pt = new PenTool();
                    pt.Compleet(g, p1, p2);
                    break;
            }
        }
    }
}