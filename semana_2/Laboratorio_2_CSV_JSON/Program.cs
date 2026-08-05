using System;
using System.IO;

string[] lineas = File.ReadAllLines("estudiantes.csv");

foreach (string linea in lineas)
{
    Console.WriteLine(linea);
}

