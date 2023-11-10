using System;
using System.Collections.Generic;
using System.Drawing;

public class Schets
{
    private int newWidth;
    private int newHeight;
    private Bitmap bitmap;
    private TekenElementMaster tem = new TekenElementMaster();

    public Schets()
    {
        bitmap = new Bitmap(1, 1);
    }
    // Een property waarmee de TEM opgehaald kan worden, zodat deze door andere klasses gebruikt kunnen worden
    // zonder dat daar drie of vier punten aan te pas hoeven te komen
    public TekenElementMaster Ophalen
    {
        get { return tem; }
    }
    // Een mogelijkheid om de TEM te overschrijven, wanneer er een nieuwe is gemaakt door het openen
    // van een SchetsPlus XML bestand
    public void TemSchrijven(TekenElementMaster temaster)
    {
        tem = temaster;
    }
    public Graphics BitmapGraphics
    {
        get { return Graphics.FromImage(bitmap); }
    }
    public void VeranderAfmeting(Size sz)
    {
        if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
        {
            newWidth = Math.Max(sz.Width, bitmap.Size.Width);
            newHeight = Math.Max(sz.Height, bitmap.Size.Height);

            Bitmap nieuw = new Bitmap(newWidth, newHeight);
            Graphics gr = Graphics.FromImage(nieuw);
            gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
            gr.DrawImage(bitmap, 0, 0);
            bitmap = nieuw;
        }
    }
    public void Teken(Graphics gr)
    {
        gr.DrawImage(bitmap, 0, 0);
    }
    public void Schoon()
    {
        Graphics gr = Graphics.FromImage(bitmap);
        gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        Ophalen.TekenElementLijst.Clear();
    }
    // Roteren gebeurd in TEM
    public void Roteer()
    {
        tem.Roteer(newWidth, newHeight);
        Teken(BitmapGraphics, tem.TekenElementLijst);
    }
    // Teken alles opnieuw
    public static void Teken(Graphics gr, List<TekenElement> TekenElement)
    {
        // Maak eerst alles wit
        gr.FillRectangle(Brushes.White, 0, 0, 2560, 1440);
        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Teken ieder element opnieuw
        foreach (TekenElement te in TekenElement)
        {
            SolidBrush brush = new SolidBrush(te.Kleur);

            switch (te.Tool)
            {
                case "tekst":
                    // Zet de tekst op een bitmap
                    Font font = new Font("Tahoma", 40);
                    SizeF sz = gr.MeasureString(te.Letters, font);
                    Bitmap tekstMap = new Bitmap((int)sz.Width + 1, (int)sz.Height + 1);
                    Graphics g = Graphics.FromImage(tekstMap);
                    g.DrawString(te.Letters, font, brush, new Point(0, 0), StringFormat.GenericTypographic);

                    // Draai deze bitmap zo ver als nodig is, adhv de opgeslagen hoek
                    if (te.Hoek == 90)
                        tekstMap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    else if (te.Hoek == 180)
                        tekstMap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    else if (te.Hoek == 270)
                        tekstMap.RotateFlip(RotateFlipType.Rotate270FlipNone);

                    gr.DrawImage(tekstMap, te.Punten[0]);
                    break;
                    // Voor iedere andere tool wordt een nieuwe tool aangemaakt
                    // met alle benodigde arguments die het TekenElement vervolgens zelf maakt
                case "kader":
                    new RechthoekTool().Teken(gr, te.Punten[0], te.Punten[1], brush);
                    break;
                case "vlak":
                    new VolRechthoekTool().Teken(gr, te.Punten[0], te.Punten[1], brush);
                    break;
                case "ovaal":
                    new OvaalTool().Teken(gr, te.Punten[0], te.Punten[1], brush);
                    break;
                case "disc":
                    new VolOvaalTool().Teken(gr, te.Punten[0], te.Punten[1], brush);
                    break;
                case "lijn":
                    new LijnTool().Teken(gr, te.Punten[0], te.Punten[1], brush);
                    break;
                case "pen":
                    new PenTool().TekenLijn(gr, te.Punten, brush);
                    break;
            }
        }
    }
}