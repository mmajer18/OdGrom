using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;

namespace OdGrom
{
    class str_ochronna
    {
        /// <summary>
        /// Wyznacza środek kuli o promieniu R opartej na punktach pk1, pk2, pk3.
        /// </summary>
        /// <param name="pk1">Punkt nr 1</param>
        /// <param name="pk2">Punkt nr 2</param>
        /// <param name="pk3">Punkt nr 3</param>
        /// <param name="pk4">Srodek okregu opsanego na punktach</param>
        /// <param name="radius">Zadany promień kuli</param>
        /// <returns>Zwraca współrzędne środka kuli </returns>
        public static Vector3 Srodek_kuli(Vector3 pk1, Vector3 pk2, Vector3 pk3, Vector3 pk4, double radius)
        {
            Vector3 sphere_center;
            var dir = Vector3.CrossProduct(pk2 - pk1, pk3 - pk1);
            if (dir.Z<0 )
            {
                dir = Vector3.CrossProduct(pk3 - pk1, pk2 - pk1);
            }
            var norm = Vector3.Normalize(dir);
            double przesuniecie;
            przesuniecie = Math.Sqrt(Math.Pow(radius,2) - Math.Pow(Vector3.Distance(pk4, pk1),2));
            sphere_center = Vector3.Add(Vector3.Multiply(przesuniecie, norm), pk4);

            return sphere_center;
        }
        /// <summary>
        /// Wyznacza wzór prostej przechodzącej przez dwa punkty.
        /// </summary>
        /// <param name="pk1">Punkt nr 1</param>
        /// <param name="pk2">Punkt nr 2</param>
        /// <returns>Zwraca współczynniki a i b prostej</returns>
        public static double[] Prosta(Vector3 pk1, Vector3 pk2)
        {
            double a, b;
            double[] parametry = new double[3];
            if (pk1.X == pk2.X)
            {
                parametry[0] = -1;
                parametry[1] = pk1.X;
                parametry[2] = 0;
            }
            else
            {
                a = (pk1.Y - pk2.Y) / (pk1.X - pk2.X);
                b = (pk1.X*pk2.Y-pk2.X*pk1.Y)/(pk1.X - pk2.X);
                parametry[0] = a;
                parametry[1] = b;
                parametry[2] = 1;
            }
            return parametry;
        }

    }
}
