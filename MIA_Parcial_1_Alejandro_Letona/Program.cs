using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Ingrese su nombre completo: ");
        string nombre = Console.ReadLine();

        Console.Write("Ingrese la ruta del archivo de texto: ");
        string ruta = Console.ReadLine();

        if (!File.Exists(ruta))
        {
            Console.WriteLine("El archivo no existe.");
            return;
        }

        int lineas = 0;
        int vocales = 0;
        int caracteres = 0;

        using (StreamReader sr = new StreamReader(ruta))
        {
            string linea;

            while ((linea = sr.ReadLine()) != null)
            {
                lineas++;

                foreach (char c in linea)
                {
                    caracteres++;

                    char letra = Char.ToLower(c);

                    if (letra == 'a' ||
                        letra == 'e' ||
                        letra == 'i' ||
                        letra == 'o' ||
                        letra == 'u')
                    {
                        vocales++;
                    }
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine("RESULTADOS");
        Console.WriteLine("Usuario: " + nombre);
        Console.WriteLine("Lineas: " + lineas);
        Console.WriteLine("Vocales: " + vocales);
        Console.WriteLine("Caracteres: " + caracteres);

        string nombreArchivo = nombre.Replace(" ", "_");
        string carpeta = Path.GetDirectoryName(ruta);
        string rutaCSV = Path.Combine(carpeta, "Resultados_" + nombreArchivo + ".csv");

        using (StreamWriter sw = new StreamWriter(rutaCSV))
        {
            sw.WriteLine("Nombre,Lineas,Vocales,Caracteres");
            sw.WriteLine($"{nombre},{lineas},{vocales},{caracteres}");
        }

        Console.WriteLine();
        Console.WriteLine("Archivo CSV creado correctamente.");
        Console.WriteLine(rutaCSV);
    }
}