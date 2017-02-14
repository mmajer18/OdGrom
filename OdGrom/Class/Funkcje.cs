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

    }

}
