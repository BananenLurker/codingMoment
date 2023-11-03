using System;
using System.Collections.Generic;
using System.Drawing;

public class Schets
{
    private Bitmap bitmap;
    private TekenElementMaster tem = new TekenElementMaster();

    public Schets()
    {
        bitmap = new Bitmap(1, 1);
    }
    public Schets(Bitmap bitmap)
    {
        this.bitmap = bitmap;
    }
    public TekenElementMaster Ophalen
    {
        get { return tem; }
    }
    public Graphics BitmapGraphics
    {
        get { return Graphics.FromImage(bitmap); }
    }
    public void VeranderAfmeting(Size sz)
    {
        if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
        {
            int newWidth = Math.Max(sz.Width, bitmap.Size.Width);
            int newHeight = Math.Max(sz.Height, bitmap.Size.Height);
            Bitmap nieuw = new Bitmap(newWidth, newHeight);
            afmeting = new Point(newWidth, newHeight);
            Graphics gr = Graphics.FromImage(nieuw);
            gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
            gr.DrawImage(bitmap, 0, 0);
            bitmap = nieuw;
        }
    }
    private Point afmeting
    {
        get;
        set;
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
    public void Roteer()
    {
        tem.Roteer(afmeting.X, afmeting.Y);
        Teken(BitmapGraphics, tem.TekenElementLijst);
    }

    public static void Teken(Graphics gr, List<TekenElement> tekenObjecten)
    {
        gr.FillRectangle(Brushes.White, 0, 0, 2560, 1440);
        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        foreach (TekenElement te in tekenObjecten)
        {
            Color color = te.Kleur;
            SolidBrush brush = new SolidBrush(te.Kleur);

            switch (te.Tool)
            {
                case "tekst":
                    Font font = new Font("Tahoma", 40);
                    gr.MeasureString(te.Letters, font, te.Punten[0], StringFormat.GenericTypographic);
                    gr.DrawString(te.Letters, font, brush, te.Punten[0], StringFormat.GenericTypographic);

                    //if (te.Hoek == 90)
                    //    tekstBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    //else if (te.Hoek == 180)
                    //    tekstBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    //else if (te.Hoek == 270)
                    //    tekstBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);

                    //gr.DrawImage(tekstBmp, te.Punten[0]);
                    break;
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