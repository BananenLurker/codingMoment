using System;
using System.Collections.Generic;
using System.Drawing;

public class TekenElement
{
    public TekenElement()
    {
        Punten = new List<Point>();
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

    public Color Kleur
    {
        get;
        set;
    }

    public string Letters
    {
        get;
        set;
    }

    public int Hoek
    {
        get;
        set;
    }
}
