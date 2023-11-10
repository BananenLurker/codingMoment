using System;
using System.Collections.Generic;
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
    protected Point startpunt;
    protected Brush kwast;
    protected TekenElement te;
    protected TekenElementMaster tem;

    public virtual void MuisVast(SchetsControl s, Point p)
    {
        startpunt = p;
        te = new TekenElement();
        te.Punten.Add(p);
        te.Tool = ToString();
        te.Kleur = s.PenKleur;
        if(te.Tool != "gum")
        {
            s.schets.Ophalen.TekenElementLijst.Add(te);
        }
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
            gr.DrawString   (tekst, font, kwast, this.startpunt, StringFormat.GenericTypographic);
            startpunt.X += (int)sz.Width;
            te.Letters += c;
            s.Invalidate();
        }
    }
}

public abstract class TweepuntTool : StartpuntTool
{
    public static Rectangle Punten2Rechthoek(Point p1, Point p2)
    {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                            , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                            );
    }
    public static Pen MaakPen(Brush b, int dikte)
    {   Pen pen = new Pen(b, dikte);
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
        this.Teken(s.CreateGraphics(), this.startpunt, p);
    }
    public override void MuisLos(SchetsControl s, Point p)
    {
        te.Punten.Add(p);
        this.Teken(s.MaakBitmapGraphics(), this.startpunt, p);
        Schets.Teken(s.MaakBitmapGraphics(), s.Ophalen.TekenElementLijst);
        s.Invalidate();
        Program.se.Gewijzigd();
    }
    public override void Letter(SchetsControl s, char c)
    {
    }
    public abstract void Teken(Graphics g, Point p1, Point p2);

    public abstract void Teken(Graphics g, Point p1, Point p2, Brush kwast);
}

public class RechthoekTool : TweepuntTool
{
    public override string ToString() { return "kader"; }

    public override void Teken(Graphics g, Point p1, Point p2, Brush kwast)
    {   
        g.DrawRectangle(MaakPen(kwast,3), TweepuntTool.Punten2Rechthoek(p1, p2));
    }
    public override void Teken(Graphics g, Point p1, Point p2)
    {
        Teken(g, p1, p2, kwast);
    }
}
public class VolRechthoekTool : RechthoekTool
{
    public override string ToString() { return "vlak"; }
    public override void Teken(Graphics g, Point p1, Point p2)
    {
        Teken(g, p1, p2, kwast);
    }
    public override void Teken(Graphics g, Point p1, Point p2, Brush kwast)
    {   
        g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
    }
}

public class OvaalTool : TweepuntTool
{
    public override string ToString() { return "ovaal"; }
    public override void Teken(Graphics g, Point p1, Point p2)
    {
        Teken(g, p1, p2, kwast);
    }
    public override void Teken(Graphics g, Point p1, Point p2, Brush kwast)
    {
        g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
    }
}

public class VolOvaalTool : OvaalTool
{
    public override string ToString() { return "disc"; }
    public override void Teken(Graphics g, Point p1, Point p2)
    {
        Teken(g, p1, p2, kwast);
    }
    public override void Teken(Graphics g, Point p1, Point p2, Brush kwast)
    {
        g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
    }
}

public class LijnTool : TweepuntTool
{
    public override string ToString() { return "lijn"; }
    public override void Teken(Graphics g, Point p1, Point p2)
    {
        Teken(g, p1, p2, kwast);
    }
    public override void Teken(Graphics g, Point p1, Point p2, Brush kwast)
    {   g.DrawLine(MaakPen(kwast, 3), p1, p2);
    }
}

public class PenTool : LijnTool
{
    public override string ToString() { return "pen"; }

    public override void MuisDrag(SchetsControl s, Point p)
    {
        te.Punten.Add(p);
        kwast = new SolidBrush(s.PenKleur);
        TekenLijn(s.CreateGraphics(), te.Punten, kwast);
    }

    public void TekenLijn(Graphics g, List<Point> points, Brush kwast)
    {
        for (int i = 1; i < points.Count; i++)
            g.DrawLine(MaakPen(kwast, 3), points[i - 1], points[i]);
    }
}
    public class GumTool : ISchetsTool
{
    protected TekenElementMaster tem;

    public void Letter(SchetsControl s, char c) { }
    public void MuisDrag(SchetsControl s, Point p) { }
    public void MuisLos(SchetsControl s, Point p) { }

    public override string ToString() { return "gum"; }

    public void MuisVast(SchetsControl s, Point p)
    {
        tem = s.Ophalen;
        tem.VerwijderElement(p);

        s.Invalidate();
        Schets.Teken(s.MaakBitmapGraphics(), tem.TekenElementLijst);
    }
}