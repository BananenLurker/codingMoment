using System;
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
    public int aantal = 0;

    protected Point startpunt;
    protected Brush kwast;
    protected TekenElement te;
    protected TekenElementMaster tem;

    public virtual void MuisVast(SchetsControl s, Point p)
    {
        te = new TekenElement();
    }
    public virtual void MuisLos(SchetsControl s, Point p)
    {   kwast = new SolidBrush(s.PenKleur);
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
        this.Bezig(s.CreateGraphics(), this.startpunt, p);
    }
    public override void MuisLos(SchetsControl s, Point p)
    {   base.MuisLos(s, p);
        this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
        Debug.WriteLine($"Wijziging {aantal}");
        Program.se.Gewijzigd();
        aantal++;
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
    public override string ToString() { return "gum"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {   g.DrawLine(MaakPen(Brushes.White, 7), p1, p2);
    }
}