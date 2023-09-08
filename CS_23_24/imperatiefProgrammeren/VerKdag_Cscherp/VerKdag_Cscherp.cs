using System;

Console.WriteLine("Wat is je geboortejaar?");
string y = Console.ReadLine();
Console.WriteLine("Hoeveelste maand?");
string m = Console.ReadLine();
Console.WriteLine("Welke dag?");
string d = Console.ReadLine();

DateTime geboorteDatumJoin = new DateTime(Int32.Parse(y), Int32.Parse(m), Int32.Parse(d));

var datumNu = DateTime.Now.Date;
var datumVerschil = datumNu - geboorteDatumJoin;

var verKDag = datumVerschil.Days % 1000;
var vorigeVerKDag = datumNu.AddDays(-verKDag);
var volgendeVerKDag = datumNu.AddDays(1000 - verKDag);

var kroonVerKDag = datumVerschil.Days % 8000;
var volgendeKroonVerKDag = datumNu.AddDays(8000 - kroonVerKDag);

if (verKDag == 0)
{
    Console.WriteLine("Gefeliciteerd, het is je verKdag!");
}
else
{
    Console.WriteLine($"Je volgende verKdag is op {volgendeVerKDag}.");
}

Console.WriteLine($"Je vorige verKdag was op {vorigeVerKDag}.");
Console.WriteLine($"Je volgende kroon-verKdag is op {volgendeKroonVerKDag}.");

if (volgendeVerKDag == volgendeKroonVerKDag)
{
    Console.WriteLine("Je volgende verKdag is ook je kroon-verKdag!");
}