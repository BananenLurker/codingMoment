﻿using System.Collections.Generic;

public class TekenElementMaster
{

    private List<TekenElement> TekenElementLijst;

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

    public void Reset()
    {
        TekenElementLijst.Clear();
    }

}