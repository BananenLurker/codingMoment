using System;
using System.Collections.Generic;
using System.Drawing;

// Het TekenElement is het object dat ieder getekend 'ding' toegewezen krijgt.
// De properties hiervan worden ingevuld als ze bekend zijn, dus de tool en kleur
// als de muis voor het eerst wordt vastgehouden en het tweede (of meerdere) punt(en)
// wanneer deze bekend zijn
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
