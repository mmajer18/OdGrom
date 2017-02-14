using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;


namespace OdGrom
{
    public class Iglica
    {
        public double wysokosc;
        public double X;
        public double Y;
        public Iglica(double x, double y, double wys)
        {
            X = x;
            Y = y;
            wysokosc = wys;
        }
    }
    public class dPoint
    { 

        public double x;
        public double y;
        
        public dPoint CircleCenter(Vector3 p1, Vector3 p2, Vector3 p3)
        { // Funkcja licząca położenie środka okręgu
            dPoint point = new dPoint();
            point.x = 0.5 * ((p2.X * p2.X * p3.Y + p2.Y * p2.Y * p3.Y - p1.X * p1.X * p3.Y + p1.X * p1.X * p2.Y - p1.Y * p1.Y * p3.Y + p1.Y * p1.Y * p2.Y + p1.Y * p3.X * p3.X + p1.Y * p3.Y * p3.Y - p1.Y * p2.X * p2.X - p1.Y * p2.Y * p2.Y - p2.Y * p3.X * p3.X - p2.Y * p3.Y * p3.Y) / (p1.Y * p3.X - p1.Y * p2.X - p2.Y * p3.X - p3.Y * p1.X + p3.Y * p2.X + p2.Y * p1.X));
            point.y = 0.5 * ((-p1.X * p3.X * p3.X - p1.X * p3.Y * p3.Y + p1.X * p2.X * p2.X + p1.X * p2.Y * p2.Y + p2.X * p3.X * p3.X + p2.X * p3.Y * p3.Y - p2.X * p2.X * p3.X - p2.Y * p2.Y * p3.X + p1.X * p1.X * p3.X - p1.X * p1.X * p2.X + p1.Y * p1.Y * p3.X - p1.Y * p1.Y * p2.X) / (p1.Y * p3.X - p1.Y * p2.X - p2.Y * p3.X - p3.Y * p1.X + p3.Y * p2.X + p2.Y * p1.X));
            return point;
        }
        public double Length(dPoint pkt_1, dPoint pkt_2)
        {
            return Math.Sqrt(Math.Pow(pkt_2.x - pkt_1.x,2)+Math.Pow(pkt_2.y-pkt_1.y,2));
        }

    }

}
    