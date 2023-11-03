using System;
using System.Collections.Generic;

public class TekenElementMaster
{
    public List<TekenElement> TekenElementLijst;

    public TekenElementMaster()
    {
        TekenElementLijst = new List<TekenElement>();
    }
    public void Toevoegen(TekenElement TekenElement)
    {
        TekenElementLijst.Add(TekenElement);
    }

    public List<TekenElement> Opvragen
    {
        get { return TekenElementLijst; }
    }
}