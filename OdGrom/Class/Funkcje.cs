using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;

namespace OdGrom
{
    class Funkcje
    {

        public bool nalezy_do_trojkata(Vector3 pk1, Vector3 pk2, Vector3 pk3, Vector3 pk4)
        {

            return true;
        }
        public static bool pointInPolygon(Vector3[] punkty, Vector3 pk)
        {
            int i, j = 2;
            bool oddNodes = false;
            for (i = 0; i < 3; i++)
            {
                if (punkty[i].X < pk.Y && punkty[j].Y >= pk.Y || punkty[j].Y < pk.Y && punkty[i].Y >= pk.Y)
                {
                    if (punkty[i].X + (pk.Y - punkty[i].Y) / (punkty[j].Y - punkty[i].Y) * (punkty[j].X - punkty[i].X) < pk.X)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }


            return oddNodes;
        }
        public static bool IsInPolygon(Vector3[] poly, Vector3 point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
        public static bool polyCheck(Vector3 v, Vector3[] p)
        {
            int j = p.Length - 1;
            bool c = false;
            for (int i = 0; i < p.Length; j = i++) c ^= p[i].Y > v.Y ^ p[j].Y > v.Y && v.X < (p[j].X - p[i].X) * (v.Y - p[i].Y) / (p[j].Y - p[i].Y) + p[i].X;
            return c;
        }

    }
}