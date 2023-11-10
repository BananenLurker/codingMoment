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
        // Als de muis ingedruk wordt, is er al een hoop duidelijk over het element
        // dat daardoor ontstaat. Bijvoorbeeld het startpunt, de tool en de kleur.
        // Deze worden direct toegevoegd aan het TekenElement
        startpunt = p;
        te = new TekenElement();
        te.Punten.Add(p);
        te.Tool = ToString();
        te.Kleur = s.PenKleur;
        if(te.Tool != "gum")
        {
            // Voeg het net gemaakte element alleen toe als het geen gum is:
            // die hoeven we namelijk niet in onze lijst te hebben
            s.schets.Ophalen.TekenElementLijst.Add(te);
        }
    }
    public virtual void MuisLos(SchetsControl s, Point p)
    {
        // Als de muis losgelaten wordt, wordt de daadwerkelijke kleur zichtbaar
        // in plaats van het grijs dat getekend wordt terwijl de gebruiker het element
        // nog tekent. Daarna is ook het eindpunt bekend, dus wordt deze toegevoegd
        // aan de lijst met punten van het TekenElement
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
            // Voeg alle letters als string toe aan het TekenElement
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
        // Voeg de laatste gegevens toe als de muis losgelaten wordt
        te.Punten.Add(p);
        this.Teken(s.MaakBitmapGraphics(), this.startpunt, p);
        // Teken het hele zooitje opnieuw via de nieuw Teken functie in Schets
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
        // Het voltooien van het getekende element
        g.DrawRectangle(MaakPen(kwast,3), TweepuntTool.Punten2Rechthoek(p1, p2));
    }
    public override void Teken(Graphics g, Point p1, Point p2)
    {
        // Als een tekenfunctie wordt aangeroepen zonder kwast, wordt de member kwast gebruikt,
        // waardoor de grijze kleur ontstaat wanneer er nog niet losgelaten is.
        // Dit geldt voor iedere tool, behalve de pentool, die op een andere manier tekent en
        // geen preview heeft. Bij overige tools zullen dus geen comments geplaatst worden,
        // gezien deze allemaal hetzelfde werken met minimale verschillen
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
        // Terwijl de muis dragt, worden punten toegevoegd aan het TekenElement.
        te.Punten.Add(p);
        kwast = new SolidBrush(s.PenKleur);
        TekenLijn(s.CreateGraphics(), te.Punten, kwast);
    }

    // Tussen deze punten worden vervolgens lijnen getrokken, waardoor
    // de illusie van een vloeiende lijn ontstaat
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

    public virtual void MuisVast(SchetsControl s, Point p)
    {
        // Haal de huidige TEM op en call VerwijderElement.
        // Verdere uitleg van deze methode staat in TekenElementMaster.cs
        tem = s.Ophalen;
        tem.VerwijderElement(p);

        s.Invalidate();
        Schets.Teken(s.MaakBitmapGraphics(), tem.TekenElementLijst);
    }
}
public class BovenopTool : GumTool
{
    public override string ToString() { return "hoog"; }

    public override void MuisVast(SchetsControl s, Point p)
    {
        // Wederom dingen die zich in TekenElementMaster afspelen, zie
        // die file voor meer details
        tem = s.Ophalen;
        TekenElement te = tem.ElementSelectie(p);
        if(te != null)
        {
            tem.TekenElementLijst.Add(te);
        }
        s.Invalidate();
        Schets.Teken(s.MaakBitmapGraphics(), tem.TekenElementLijst);
    }
}
public class OnderopTool : BovenopTool
{
    public override string ToString() { return "laag"; }

    public override void MuisVast(SchetsControl s, Point p)
    {
        // en nog een keer
        tem = s.Ophalen;
        TekenElement te = tem.ElementSelectie(p);
        if(te != null)
        {
            tem.TekenElementLijst.Insert(0, te);
        }
        s.Invalidate();
        Schets.Teken(s.MaakBitmapGraphics(), tem.TekenElementLijst);
    }
}
public class MoveTool : ISchetsTool
{
    protected TekenElementMaster tem;
    private TekenElement tempte;
    private Point hier;

    public void Letter(SchetsControl s, char c) { }
    public virtual void MuisVast(SchetsControl s, Point p)
    {
        // Als er geklikt wordt met de MoveTool, wordt eerst gekeken of op deze plek
        // überhaupt wel een TekenElement is. Wel wordt alvast de membervariabelen geüpdate,
        // voor als er een TekenElement gevonden wordt.
        hier = p;
        tem = s.Ophalen;
        tempte = tem.ZoekDragElement(p);
    }

    public void MuisDrag(SchetsControl s, Point p)
    {
        // Check of er wel daadwerkelijk een TekenElement aangeklikt is
        if (tempte != null)
        {
            // Reken het verschil uit tussen het beginpunt en de plek waar de muis nu is.
            // Point p is hier mea.Location().
            int offsetX = p.X - hier.X;
            int offsetY = p.Y - hier.Y;

            // Update ieder punt met de offset
            for (int i = 0; i < tempte.Punten.Count; i++)
            {
                tempte.Punten[i] = new Point(tempte.Punten[i].X + offsetX, tempte.Punten[i].Y + offsetY);
            }

            // Teken alle elementen opnieuw
            s.Invalidate();
            Schets.Teken(s.MaakBitmapGraphics(), tem.TekenElementLijst);
            // Update het 'beginpunt', wat nu dus niet meer het beginpunt is.
            // Doordat de berekening van de offset voortdurend wordt uitgevoerd,
            // zal het lijken alsof de gesleepte elementen soepel meebewegen.
            hier = p;
        }
    }
    public void MuisLos(SchetsControl s, Point p) { }

    public override string ToString() { return "move"; }
}