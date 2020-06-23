using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace msg_reise
{
    internal class Program
    {
        private static double GetDistance(double lat1, double lat2, double lon1, double lon2)
        {
            //Formel zur Berechnung der entfernung zwischen 2 Punkten aus gegebenen Längen und Breitengraden
            double lat = (lat1 + lat2) / 2 * 0.01745;
            double dist = Math.Sqrt((111.3 * Math.Cos(lat) * (lon1 - lon2)) * (111.3 * Math.Cos(lat) * (lon1 - lon2)) + (111.3 * (lat1 - lat2)) * (111.3 * (lat1 - lat2)));
            return dist;
        }

        private static void Main(string[] args)
        {
            //importieren der csv
            var reader = new StreamReader("A:\\Programmieren\\C#\\msg_reise\\msg_standorte_deutschland.csv");
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<msg>();

            //variablen für Algorithmus
            double dist = 0;
            Knoten k;
            List<Knoten> knotenliste = new List<Knoten>();
            List<double[]> kantenliste = new List<double[]>();
            List<Knoten> path = new List<Knoten>();

            //Knotenlist erstellen
            foreach (msg i in records)
            {
                k = new Knoten(i.Nummer, i.Breitengrad, i.Längengrad);
                knotenliste.Add(k);
            }

            //Nachbarn der knoten setzen
            foreach (Knoten i in knotenliste)
            {
                foreach (Knoten j in knotenliste)
                {
                    if (i == j)
                        continue;
                    dist = GetDistance(i.getBreitengrad(), j.getBreitengrad(), i.getLängengrad(), j.getLängengrad());
                    i.addNeighbor(j, dist);
                }
                i.sortNeighbors();
            }

            //Kantenliste erstellen
            int help1 = 0, help2;
            while (help1 < knotenliste.Count)
            {
                help2 = help1 + 1;
                while (help2 < knotenliste.Count)
                {
                    double[] kante = new double[3];
                    kante[0] = help1 + 1;
                    kante[1] = help2 + 1;
                    kante[2] = GetDistance(knotenliste[help1].getBreitengrad(), knotenliste[help2].getBreitengrad(), knotenliste[help1].getLängengrad(), knotenliste[help2].getLängengrad());
                    kantenliste.Add(kante);
                    help2++;
                }
                help1++;
            }
                        
            //temp Variablen
            help1 = help2 = 0;
            Knoten helper;
            double[] distances = new double[knotenliste.Count + 1];

            //Algorithmus:
            path.Add(knotenliste[0]);
            while (true)
            {
                helper = path[help1].getClosest(help2).Item1;                       //set helper to closest Neighbor
                distances[help1 + 1] = path[help1].getClosest(help2).Item2;         //add Distance to Array
                while (path.Contains(helper))                                       //If this Neighbor is already in the pathlist try the next one
                {
                    help2++;
                    helper = path[help1].getClosest(help2).Item1;
                    distances[help1 + 1] = path[help1].getClosest(help2).Item2;
                }
                path.Add(helper);                                                   //Adding helper to pathlist
                help2 = 0;                                                          //Resetting Helpers
                help1++;

                if (path.Count == knotenliste.Count)                                //Break Loop if Length of Path = Length of Locationslist
                    break;
            }
            path.Add(knotenliste[0]);                                               //Add Destination to path
            distances[help1 + 1] = GetDistance(path[help1 + 1].getBreitengrad(), path[help1].getBreitengrad(), path[help1 + 1].getLängengrad(), path[help1].getLängengrad());

            //Ausgabe des Ergebisses
            help1 = 0;
            foreach (Knoten n in path)
                Console.WriteLine(n.getNummer() + ", " + distances[help1++]);

            for (int i = 0; i < distances.Length; i++)
                dist += distances[i];

            Console.WriteLine("Insgesamt: " + dist + "km");

            for (int i = 0; i < kantenliste.Count; i++)
                dist += kantenliste[i][2];
            dist = dist / kantenliste.Count * distances.Length;
            Console.WriteLine("Durchschnitt: " + dist + "km");
        }
    }
}