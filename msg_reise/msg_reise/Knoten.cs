using System;
using System.Collections.Generic;

namespace msg_reise
{
    internal class Knoten
    {
        private int Nummer;
        private double Breitengrad;
        private double Längengrad;
        private Tuple<Knoten, double> next = Tuple.Create<Knoten, double>(null, double.MaxValue);

        public List<Tuple<Knoten, double>> Neighbors = new List<Tuple<Knoten, double>>();

        public Knoten(int Nummer, double Breitengrad, double Längengrad)
        {
            this.Nummer = Nummer;
            this.Breitengrad = Breitengrad;
            this.Längengrad = Längengrad;
        }

        public int getNummer()
        {
            return this.Nummer;
        }

        public double getBreitengrad()
        {
            return this.Breitengrad;
        }

        public double getLängengrad()
        {
            return this.Längengrad;
        }

        public void addNeighbor(Knoten k, double dist)
        {
            Tuple<Knoten, double> t = Tuple.Create(k, dist);
            Neighbors.Add(t);
            if (t.Item2 < next.Item2)
                next = t;
        }

        public Tuple<Knoten, double> getClosest(int pos)
        {
            return Neighbors[pos];
        }

        public void sortNeighbors()
        {
            Tuple<Knoten, double> temp;
            int j;

            for (int i = 1; i < Neighbors.Count; i++)
            {
                j = i;
                while ((j > 0) && (Neighbors[j - 1].Item2 > Neighbors[j].Item2))
                {
                    temp = Neighbors[j - 1];
                    Neighbors[j - 1] = Neighbors[j];
                    Neighbors[j] = temp;
                    j--;
                }
            }
        }
    }
}