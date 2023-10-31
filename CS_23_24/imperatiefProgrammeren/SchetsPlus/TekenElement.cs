using System;
using System.Collections.Generic;
using System.Drawing;

public class TekenElement
{
    public TekenElement()
    {
        Punten = new List<Point>();
        Letters = new List<char>();
    }
    public String Tool
    {
        get;
        set;
    }

    public List<Point> Punten
    {
        get;
        set;
    }

    public String Kleur
    {
        get;
        set;
    }

    public void PuntToevoegen(Point p)
    {
        Punten.Add(p);
    }

    public List<char> Letters
    {
        get;
        set;
    }

    public void LetterToevoegen(char c)
    {
        Letters.Add(c);
    }

    public int Rotatie
    {
        get;
        set;
    }
}
