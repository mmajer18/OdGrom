using System;
using netDxf;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OdGrom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Triangulator.Geometry.Point> Vertices;
        private double[] kontury;


        public MainWindow()
        {
            InitializeComponent();
            Vertices = new List<Triangulator.Geometry.Point>();
            listBox1.Items.Add("h=0,5m");
            listBox1.Items.Add("h=1,5m");
            listBox1.Items.Add("h=2,0m");
            listBox1.Items.Add("h=2,5m");
            listBox1.Items.Add("h=3,0m");
            listBox1.Items.Add("h=3,5m");
            listBox1.Items.Add("h=4,0m");
            listBox1.Items.Add("h=4,5m");
            listBox1.Items.Add("h=5,0m");

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //test
            int liczba_iglic;
            int punkty_x, punkty_y;
            punkty_x = Convert.ToInt16(tb_dlugosc.Text);
            punkty_y = Convert.ToInt16(tb_szerokosc.Text);
            liczba_iglic = listBox.Items.Count;
            kontury = new double[listBox1.Items.Count];
            Iglica[] iglice = new Iglica[liczba_iglic];
            DxfDocument dxf = new DxfDocument();
            Triangulator.Geometry.Point pNew;
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                iglice[i] = new Iglica();
                            
                    string linia = listBox.Items.GetItemAt(i).ToString();
                    int index0 = linia.IndexOf("x=");
                    int index1 = linia.IndexOf(";");
                    iglice[i].X = Convert.ToDouble(linia.Substring(index0 + 2, index1 - index0 - 2));
                    linia = linia.Substring(index1 + 1, linia.Length - index1 - 1);
                    index0 = linia.IndexOf("y=");
                    index1 = linia.IndexOf(";");
                    iglice[i].Y = Convert.ToDouble(linia.Substring(index0 + 2, index1 - index0 - 2));
                    linia = linia.Substring(index1 + 1, linia.Length - index1 - 1);
                    index0 = linia.IndexOf("h=");
                    index1 = linia.IndexOf(";");
                    iglice[i].wysokosc = Convert.ToDouble(linia.Substring(index0 + 2, index1 - index0 - 2));
            
                pNew = new Triangulator.Geometry.Point(iglice[i].X, iglice[i].Y);
                if (!Vertices.Exists(delegate (Triangulator.Geometry.Point p) { return pNew.Equals2D(p); }))
                    Vertices.Add(pNew);

            }
            /*
            for (int i=0; i<punkty_x;i++)
            {
                pNew = new Triangulator.Geometry.Point(0, i);
                Vertices.Add(pNew);
                pNew = new Triangulator.Geometry.Point(Convert.ToInt16(tb_szerokosc.Text), i);
                Vertices.Add(pNew);

            }
            for (int i = 0; i < punkty_y; i++)
            {
                pNew = new Triangulator.Geometry.Point(i, 0);
                Vertices.Add(pNew);
                pNew = new Triangulator.Geometry.Point(i,Convert.ToInt16(tb_dlugosc.Text));
                Vertices.Add(pNew);

            }
            */
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                kontury[i] = new double();
                string linia = listBox1.Items.GetItemAt(i).ToString();
                int index0 = linia.IndexOf("=");
                int index1 = linia.IndexOf("m");
                kontury[i] = Convert.ToDouble(linia.Substring(index0 + 1, index1 - index0 - 1));

            }

            string pathF = @"aCS_2.dxf";

            //wylicz_punkty(dxf, iglice, 60, 0.1, 0.005, kontury);
            rysuj_troj(dxf, iglice);

            try
            {
                dxf.Save(pathF);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Błąd zapisu pliku");
            }


        }

        public double[,] wylicz_punkty(DxfDocument dxf, Iglica[] iglice, double promien, double dok_pkt, double dok_wys, double[] kontury)
        {
            int punkty_x, punkty_y;

            double dist;
            double[] dist_rob = new double[iglice.Length];
            punkty_x = Convert.ToInt16(tb_dlugosc.Text) * Convert.ToInt16(1 / dok_pkt);
            punkty_y = Convert.ToInt16(tb_szerokosc.Text) * Convert.ToInt16(1 / dok_pkt);
            double[] x = new double[punkty_x];
            double[] y = new double[punkty_y];
            double[,] siatka = new double[punkty_x, punkty_y];
            Progresbar1.Maximum = punkty_x * punkty_y;

            for (int i = 0; i < Convert.ToInt16(tb_dlugosc.Text) * Convert.ToInt16(1 / dok_pkt); i++)
            {
                x[i] = i * dok_pkt;
                for (int j = 0; j < Convert.ToInt16(tb_szerokosc.Text) * Convert.ToInt16(1 / (dok_pkt)); j++)
                {
                    y[j] = j * dok_pkt;
                    for (int k = 0; k < (5 / dok_wys); k++)
                    {
                        siatka[i, j] = k * dok_wys;
                        Progresbar1.Value++;
                        for (int m = 0; m < iglice.Length; m++)
                        {
                            dist_rob[m] = Math.Sqrt(Math.Pow(i * dok_pkt - iglice[m].X, 2)
                                                    + Math.Pow(j * dok_pkt - iglice[m].Y, 2)
                                                    + Math.Pow(siatka[i, j] + promien - iglice[m].wysokosc, 2));
                        }
                        dist = dist_rob[0];
                        for (int n = 0; n < dist_rob.Length; n++)
                        {
                            if (dist_rob[n] < dist)
                            {
                                dist = dist_rob[n];
                            }
                        }
                        if (dist > promien)
                        {
                            break;
                        }
                    }

                }
            }
            Contour(dxf, siatka, x, y, kontury);
            rysuj_troj(dxf, iglice);
            return siatka;

        }
        public void wylicz_punkty_2(DxfDocument dxf, Vector3 pk_1, Vector3 pk_2,Vector3 pk_3, Vector3 center, ref double[,] siatka, ref double[] x, ref double[] y )
        {
            
            double dok_pkt = 0.01;
            double promien_tocz_kuli = 60;            
            double radius = Math.Sqrt(Math.Pow(pk_1.X - center.X, 2) + Math.Pow(pk_1.Y - center.Y, 2) + Math.Pow(pk_1.Z - center.Z, 2));
            double z_kuli = center.Z+Math.Sqrt(Math.Pow(promien_tocz_kuli, 2)- Math.Pow(radius, 2));
            double min_x, min_y, max_x, max_y;
            int k,l;
            min_x = Math.Min(Math.Min(pk_1.X, pk_2.X),pk_3.X);
            min_y = Math.Min(Math.Min(pk_1.Y, pk_2.Y), pk_3.Y);
            max_x = Math.Max(Math.Max(pk_1.X, pk_2.X), pk_3.X);
            max_y = Math.Max(Math.Max(pk_1.Y, pk_2.Y), pk_3.Y);
            k = Convert.ToInt16(min_x * (1 / dok_pkt));
            
            for (double i = min_x; i<max_x; i += 0.01)
            {
                x[k] = i;
                k = k + 1;
                l = Convert.ToInt16(min_y * (1 / dok_pkt));
                for (double j = min_y; j < max_y; j += 0.01)
                {
                    y[l] = j;
                    siatka[k, l] = z_kuli - Math.Sqrt(Math.Pow(promien_tocz_kuli,2)-Math.Pow(i,2)+2*i*center.X-Math.Pow(center.X,2)-Math.Pow(j,2)+2*j*center.Y-Math.Pow(center.Y,2));
                    l = l + 1;
                }
            }      
                           
            
               
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Add("Iglica " + listBox.Items.Count + " x=" + tb_X.Text + "; y=" + tb_Y.Text + "; h=" + tb_H.Text + ";");
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void bt_usun_iglice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listBox.Items.RemoveAt(listBox.SelectedIndex);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Zaznacz iglicę do usunięcia");
            }

        }


        public void Contour(DxfDocument dxf, double[,] d, double[] x, double[] y, double[] z)
        {
            double x1 = 0.0;
            double x2 = 0.0;
            double y1 = 0.0;
            double y2 = 0.0;

            var h = new double[5];
            var sh = new int[5];
            var xh = new double[5];
            var yh = new double[5];

            int ilb = d.GetLowerBound(0);
            int iub = d.GetUpperBound(0);
            int jlb = d.GetLowerBound(1);
            int jub = d.GetUpperBound(1);
            int nc = z.Length;

            // The indexing of im and jm should be noted as it has to start from zero
            // unlike the fortran counter part
            int[] im = { 0, 1, 1, 0 };
            int[] jm = { 0, 0, 1, 1 };

            // Note that castab is arranged differently from the FORTRAN code because
            // Fortran and C/C++ arrays are transposed of each other, in this case
            // it is more tricky as castab is in 3 dimension
            int[,,] castab = {
                                 { { 0, 0, 8 }, { 0, 2, 5 }, { 7, 6, 9 } }, { { 0, 3, 4 }, { 1, 3, 1 }, { 4, 3, 0 } },
                                 { { 9, 6, 7 }, { 5, 2, 0 }, { 8, 0, 0 } }
                             };

            Func<int, int, double> xsect = (p1, p2) => (h[p2] * xh[p1] - h[p1] * xh[p2]) / (h[p2] - h[p1]);
            Func<int, int, double> ysect = (p1, p2) => (h[p2] * yh[p1] - h[p1] * yh[p2]) / (h[p2] - h[p1]);

            for (int j = jub - 1; j >= jlb; j--)
            {
                int i;
                for (i = ilb; i <= iub - 1; i++)
                {
                    double temp1 = Math.Min(d[i, j], d[i, j + 1]);
                    double temp2 = Math.Min(d[i + 1, j], d[i + 1, j + 1]);
                    double dmin = Math.Min(temp1, temp2);
                    temp1 = Math.Max(d[i, j], d[i, j + 1]);
                    temp2 = Math.Max(d[i + 1, j], d[i + 1, j + 1]);
                    double dmax = Math.Max(temp1, temp2);

                    if (dmax >= z[0] && dmin <= z[nc - 1])
                    {
                        short k;
                        for (k = 0; k < nc; k++)
                        {
                            if (z[k] >= dmin && z[k] <= dmax)
                            {
                                int m;
                                for (m = 4; m >= 0; m--)
                                {
                                    if (m > 0)
                                    {
                                        // The indexing of im and jm should be noted as it has to
                                        // start from zero
                                        h[m] = d[i + im[m - 1], j + jm[m - 1]] - z[k];
                                        xh[m] = x[i + im[m - 1]];
                                        yh[m] = y[j + jm[m - 1]];
                                    }
                                    else
                                    {
                                        h[0] = 0.25 * (h[1] + h[2] + h[3] + h[4]);
                                        xh[0] = 0.5 * (x[i] + x[i + 1]);
                                        yh[0] = 0.5 * (y[j] + y[j + 1]);
                                    }

                                    if (h[m] > 0.0)
                                    {
                                        sh[m] = 1;
                                    }
                                    else if (h[m] < 0.0)
                                    {
                                        sh[m] = -1;
                                    }
                                    else
                                    {
                                        sh[m] = 0;
                                    }
                                }

                                // Note: at this stage the relative heights of the corners and the
                                // centre are in the h array, and the corresponding coordinates are
                                // in the xh and yh arrays. The centre of the box is indexed by 0
                                // and the 4 corners by 1 to 4 as shown below.
                                // Each triangle is then indexed by the parameter m, and the 3
                                // vertices of each triangle are indexed by parameters m1,m2,and
                                // m3.
                                // It is assumed that the centre of the box is always vertex 2
                                // though this isimportant only when all 3 vertices lie exactly on
                                // the same contour level, in which case only the side of the box
                                // is drawn.
                                // vertex 4 +-------------------+ vertex 3
                                // | \               / |
                                // |   \    m-3    /   |
                                // |     \       /     |
                                // |       \   /       |
                                // |  m=2    X   m=2   |       the centre is vertex 0
                                // |       /   \       |
                                // |     /       \     |
                                // |   /    m=1    \   |
                                // | /               \ |
                                // vertex 1 +-------------------+ vertex 2
                                // Scan each triangle in the box
                                for (m = 1; m <= 4; m++)
                                {
                                    int m1 = m;
                                    int m2 = 0;
                                    int m3;
                                    if (m != 4)
                                    {
                                        m3 = m + 1;
                                    }
                                    else
                                    {
                                        m3 = 1;
                                    }

                                    int caseValue = castab[sh[m1] + 1, sh[m2] + 1, sh[m3] + 1];
                                    if (caseValue != 0)
                                    {
                                        switch (caseValue)
                                        {
                                            case 1: // Line between vertices 1 and 2
                                                x1 = xh[m1];
                                                y1 = yh[m1];
                                                x2 = xh[m2];
                                                y2 = yh[m2];
                                                break;
                                            case 2: // Line between vertices 2 and 3
                                                x1 = xh[m2];
                                                y1 = yh[m2];
                                                x2 = xh[m3];
                                                y2 = yh[m3];
                                                break;
                                            case 3: // Line between vertices 3 and 1
                                                x1 = xh[m3];
                                                y1 = yh[m3];
                                                x2 = xh[m1];
                                                y2 = yh[m1];
                                                break;
                                            case 4: // Line between vertex 1 and side 2-3
                                                x1 = xh[m1];
                                                y1 = yh[m1];
                                                x2 = xsect(m2, m3);
                                                y2 = ysect(m2, m3);
                                                break;
                                            case 5: // Line between vertex 2 and side 3-1
                                                x1 = xh[m2];
                                                y1 = yh[m2];
                                                x2 = xsect(m3, m1);
                                                y2 = ysect(m3, m1);
                                                break;
                                            case 6: // Line between vertex 3 and side 1-2
                                                x1 = xh[m3];
                                                y1 = yh[m3];
                                                x2 = xsect(m1, m2);
                                                y2 = ysect(m1, m2);
                                                break;
                                            case 7: // Line between sides 1-2 and 2-3
                                                x1 = xsect(m1, m2);
                                                y1 = ysect(m1, m2);
                                                x2 = xsect(m2, m3);
                                                y2 = ysect(m2, m3);
                                                break;
                                            case 8: // Line between sides 2-3 and 3-1
                                                x1 = xsect(m2, m3);
                                                y1 = ysect(m2, m3);
                                                x2 = xsect(m3, m1);
                                                y2 = ysect(m3, m1);
                                                break;
                                            case 9: // Line between sides 3-1 and 1-2
                                                x1 = xsect(m3, m1);
                                                y1 = ysect(m3, m1);
                                                x2 = xsect(m1, m2);
                                                y2 = ysect(m1, m2);
                                                break;
                                            default:
                                                break;
                                        }
                                        Vector3 pk_1 = new Vector3(x1, y1, 0);
                                        Vector3 pk_2 = new Vector3(x2, y2, 0);
                                        netDxf.Entities.Line ln_1 = new netDxf.Entities.Line(pk_1, pk_2);
                                        string nazwa_warsty = z[k].ToString();
                                        int index0;
                                        if (nazwa_warsty.IndexOf(",") != -1)
                                        {
                                            index0 = nazwa_warsty.IndexOf(",");
                                            nazwa_warsty = nazwa_warsty.Substring(0, index0) + "_" + nazwa_warsty.Substring(index0 + 1, nazwa_warsty.Length - index0 - 1);

                                        }

                                        netDxf.Tables.Layer warstwa = new netDxf.Tables.Layer("h-" + nazwa_warsty + "m");
                                        dxf.Layers.Add(warstwa);
                                        ln_1.Layer = warstwa;
                                        dxf.AddEntity(ln_1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            if (Vertices.Count > 2)
            {
                //Do triangulation
                List<Triangulator.Geometry.Triangle> tris = Triangulator.Delauney.Triangulate(Vertices);
                // Draw the created triangles
                foreach (Triangulator.Geometry.Triangle t in tris)
                {
                    // g.DrawLine(myPen, (float)Vertices[t.p1].X, (float)Vertices[t.p1].Y, (float)Vertices[t.p2].X, (float)Vertices[t.p2].Y);
                    //  g.DrawLine(myPen, (float)Vertices[t.p2].X, (float)Vertices[t.p2].Y, (float)Vertices[t.p3].X, (float)Vertices[t.p3].Y);
                    // g.DrawLine(myPen, (float)Vertices[t.p1].X, (float)Vertices[t.p1].Y, (float)Vertices[t.p3].X, (float)Vertices[t.p3].Y);
                    //dxf.AddEntity(ln_1)
                }
            }
        }
        public void rysuj_troj(DxfDocument dxf, Iglica[] iglice)
        {
            int punkty_x, punkty_y;
            punkty_x = Convert.ToInt16(tb_dlugosc.Text) * Convert.ToInt16(1 / 0.01);
            punkty_y = Convert.ToInt16(tb_szerokosc.Text) * Convert.ToInt16(1 /0.01);
            double[] x = new double[punkty_x+1];
            double[] y = new double[punkty_y+1];
            double[,] siatka = new double[punkty_x+1, punkty_y+1];
            if (Vertices.Count > 2)
            {
                //Do triangulation
                List<Triangulator.Geometry.Triangle> tris = Triangulator.Delauney.Triangulate(Vertices);
                // Draw the created triangles
                
                foreach (Triangulator.Geometry.Triangle t in tris)
                {
                    Vector3 pk_1= new Vector3();
                    Vector3 pk_2 = new Vector3();
                    Vector3 pk_3 = new Vector3();
                    double epsilon = 0.01;
                    // g.DrawLine(myPen, (float)Vertices[t.p1].X, (float)Vertices[t.p1].Y, (float)Vertices[t.p2].X, (float)Vertices[t.p2].Y);
                    //  g.DrawLine(myPen, (float)Vertices[t.p2].X, (float)Vertices[t.p2].Y, (float)Vertices[t.p3].X, (float)Vertices[t.p3].Y);
                    // g.DrawLine(myPen, (float)Vertices[t.p1].X, (float)Vertices[t.p1].Y, (float)Vertices[t.p3].X, (float)Vertices[t.p3].Y);
                    pk_1 = new Vector3((float)Vertices[t.p1].X, (float)Vertices[t.p1].Y, 0);
                    pk_2 = new Vector3((float)Vertices[t.p2].X, (float)Vertices[t.p2].Y, 0);
                    pk_3 = new Vector3((float)Vertices[t.p3].X, (float)Vertices[t.p3].Y, 0);
                    for (int i = 0; i < listBox.Items.Count; i++)
                    {
                        if ((Math.Abs(Vertices[t.p1].X - iglice[i].X)<epsilon)  & (Math.Abs(Vertices[t.p1].Y - iglice[i].Y)<epsilon))
                        {
                            pk_1 = new Vector3((float)Vertices[t.p1].X, (float)Vertices[t.p1].Y, iglice[i].wysokosc);
                        }
                        if ((Math.Abs(Vertices[t.p2].X - iglice[i].X) < epsilon) & (Math.Abs(Vertices[t.p2].Y - iglice[i].Y) < epsilon))
                        {
                            pk_2 = new Vector3((float)Vertices[t.p2].X, (float)Vertices[t.p2].Y, iglice[i].wysokosc);
                        }
                        if ((Math.Abs(Vertices[t.p3].X - iglice[i].X) < epsilon) & (Math.Abs(Vertices[t.p3].Y - iglice[i].Y) < epsilon))
                        {
                            pk_3 = new Vector3((float)Vertices[t.p3].X, (float)Vertices[t.p3].Y, iglice[i].wysokosc);
                        }
                        
                    }
                    dPoint pk_4 = new dPoint();
                    dPoint pk_5 = new dPoint();
                    pk_5.x = pk_1.X;
                    pk_5.y = pk_1.Y;
                    pk_4 = pk_4.CircleCenter(pk_1, pk_2, pk_3);
                    Vector3 pk_6 = new Vector3();                    
                    pk_6 = GetCen(pk_1, pk_2, pk_3);
                    Vector2 pk_7 = new Vector2(pk_6.X, pk_6.Y);
                    netDxf.Entities.Line ln_1 = new netDxf.Entities.Line(pk_1, pk_2);
                    netDxf.Entities.Circle circle_1 = new netDxf.Entities.Circle(pk_6,Math.Sqrt(Math.Pow(pk_1.X-pk_6.X,2)+ Math.Pow(pk_1.Y - pk_6.Y, 2)+ Math.Pow(pk_1.Z - pk_6.Z, 2)));                    
                   // dxf.AddEntity(circle_1);
                    //dxf.AddEntity(ln_1);                   
                    ln_1 = new netDxf.Entities.Line(pk_2, pk_3);
                    //dxf.AddEntity(ln_1);
                    ln_1 = new netDxf.Entities.Line(pk_1, pk_3);
                    //dxf.AddEntity(ln_1);
                    wylicz_punkty_2(dxf, pk_1, pk_2, pk_3, pk_6, ref siatka, ref x, ref y);                

                }

                Contour(dxf, siatka, x, y, kontury);

            }
        }
        static Vector3 GetCen(Vector3 pk_1, Vector3 pk_2, Vector3 pk_3)
        {
            double r1 = pk_1.X - pk_3.X;
            double r2 = pk_1.Y - pk_3.Y;
            double r3 = pk_1.Z - pk_3.Z;
            double q1 = pk_2.X - pk_3.X;
            double q2 = pk_2.Y - pk_3.Y;
            double q3 = pk_2.Z - pk_3.Z;
            double t1 = r2 * q1 * Math.Pow(r3, 2.0) * q2;   //NOTE:  the negative is NOT included
            double t2 = q1 * Math.Pow(r3, 2.0) * Math.Pow(q2, 2.0);
            double t3 = r3 * q3 * r1 * Math.Pow(q2, 2.0);
            double t4 = r3 * q3 * r1 * Math.Pow(q1, 2.0);
            double t5 = r3 * q3 * Math.Pow(r1, 2.0) * q1;
            double t6 = r3 * Math.Pow(r2, 2.0) * q3 * q1;
            double t7 = Math.Pow(q3, 2.0) * r1 * Math.Pow(r3, 2.0);
            double t8 = q1 * Math.Pow(r2, 3.0) * q2;
            double t9 = q1 * Math.Pow(r2, 2.0) * Math.Pow(q2, 2.0);
            double t10 = q1 * Math.Pow(r2, 2.0) * Math.Pow(q3, 2.0);
            double t11 = r2 * Math.Pow(q1, 2.0) * r1 * q2;
            double t12 = r2 * q1 * Math.Pow(r1, 2.0) * q2;
            double t13 = q1 * Math.Pow(r3, 2.0) * Math.Pow(q3, 2.0);
            double t14 = Math.Pow(r2, 2.0) * Math.Pow(q1, 3.0);
            double t15 = Math.Pow(q1, 3.0) * Math.Pow(r3, 2.0);
            double t16 = r3 * Math.Pow(q3, 3.0) * r1;
            double t17 = Math.Pow(r1, 3.0) * Math.Pow(q3, 2.0);
            double t18 = q3 * q1 * Math.Pow(r3, 3.0);
            double t19 = Math.Pow(q2, 3.0) * r1 * r2;
            double t20 = Math.Pow(q3, 2.0) * r1 * r2 * q2;
            double t21 = r1 * Math.Pow(r2, 2.0) * Math.Pow(q3, 2.0);
            double t22 = Math.Pow(r3, 2.0) * r1 * Math.Pow(q2, 2.0);
            double t23 = Math.Pow(q2, 2.0) * r1 * Math.Pow(r2, 2.0);
            double t24 = Math.Pow(r1, 3.0) * Math.Pow(q2, 2.0);
            double numx = (-1 * t1) + t2 - t3 - t4 - t5 - t6 + t7 - t8 + t9 + t10 - t11 - t12 + t13 + t14 + t15 - t16 + t17 - t18 - t19 - t20 + t21 + t22 + t23 + t24;
            double s1 = r2 * r1 * Math.Pow(q1, 3.0);
            double s2 = Math.Pow(q1, 2.0) * Math.Pow(r2, 3.0);
            double s3 = Math.Pow(q1, 2.0) * r2 * Math.Pow(r1, 2.0);
            double s4 = Math.Pow(q1, 2.0) * r2 * q3 * r3;
            double s5 = Math.Pow(q1, 2.0) * r2 * Math.Pow(r3, 2.0);
            double s6 = Math.Pow(q1, 2.0) * Math.Pow(r3, 2.0) * q2;
            double s7 = Math.Pow(q1, 2.0) * Math.Pow(r1, 2.0) * q2;
            double s8 = q1 * Math.Pow(r2, 2.0) * q2 * r1;
            double s9 = q1 * r2 * Math.Pow(q2, 2.0) * r1;
            double s10 = q1 * r2 * Math.Pow(q3, 2.0) * r1;
            double s11 = q1 * Math.Pow(r1, 3.0) * q2;
            double s12 = q1 * Math.Pow(r3, 2.0) * r1 * q2;
            double s13 = Math.Pow(q3, 2.0) * Math.Pow(r2, 3.0);
            double s14 = r3 * q3 * q2 * Math.Pow(r2, 2.0);
            double s15 = Math.Pow(r3, 2.0) * r2 * Math.Pow(q3, 2.0);
            double s16 = r2 * Math.Pow(q3, 2.0) * Math.Pow(r1, 2.0);
            double s17 = r2 * r3 * q3 * Math.Pow(q2, 2.0);
            double s18 = r2 * r3 * Math.Pow(q3, 3.0);
            double s19 = Math.Pow(q2, 3.0) * Math.Pow(r1, 2.0);
            double s20 = q3 * Math.Pow(r3, 3.0) * q2;
            double s21 = r3 * q3 * Math.Pow(r1, 2.0) * q2;
            double s22 = Math.Pow(q3, 2.0) * Math.Pow(r1, 2.0) * q2;
            double s23 = Math.Pow(r3, 2.0) * Math.Pow(q2, 3.0);
            double s24 = Math.Pow(r3, 2.0) * q2 * Math.Pow(q3, 2.0);
            double numy = s1 - s2 - s3 + s4 - s5 - s6 - s7 + s8 + s9 + s10 + s11 + s12 - s13 + s14 - s15 - s16 + s17 + s18 - s19 + s20 + s21 - s22 - s23 - s24;
            double v1 = q3 * Math.Pow(r1, 2.0) * Math.Pow(q2, 2.0);      //negative not put on this variable
            double v2 = q3 * Math.Pow(r1, 2.0) * Math.Pow(q1, 2.0);
            double v3 = Math.Pow(r3, 3.0) * Math.Pow(q2, 2.0);
            double v4 = Math.Pow(r2, 2.0) * Math.Pow(q3, 3.0);
            double v5 = Math.Pow(q1, 2.0) * Math.Pow(r3, 3.0);
            double v6 = q3 * Math.Pow(r1, 3.0) * q1;
            double v7 = Math.Pow(q1, 2.0) * r3 * Math.Pow(r1, 2.0);
            double v8 = r3 * Math.Pow(q2, 2.0) * Math.Pow(r1, 2.0);
            double v9 = Math.Pow(q1, 3.0) * r3 * r1;
            double v10 = r3 * q2 * r2 * Math.Pow(q3, 2.0);
            double v11 = r2 * q3 * Math.Pow(r3, 2.0) * q2;
            double v12 = q1 * r3 * r1 * Math.Pow(q3, 2.0);
            double v13 = q3 * r1 * q1 * Math.Pow(r3, 2.0);
            double v14 = Math.Pow(q3, 3.0) * Math.Pow(r1, 2.0);
            double v15 = r3 * Math.Pow(q2, 3.0) * r2;
            double v16 = Math.Pow(r2, 2.0) * q3 * q1 * r1;
            double v17 = Math.Pow(r2, 2.0) * q3 * Math.Pow(q1, 2.0);
            double v18 = Math.Pow(r2, 2.0) * q3 * Math.Pow(q2, 2.0);
            double v19 = Math.Pow(q1, 2.0) * r3 * Math.Pow(r2, 2.0);
            double v20 = q1 * r3 * r1 * Math.Pow(q2, 2.0);
            double v21 = r3 * Math.Pow(q2, 2.0) * Math.Pow(r2, 2.0);
            double v22 = r2 * q3 * Math.Pow(r1, 2.0) * q2;
            double v23 = Math.Pow(r2, 3.0) * q3 * q2;
            double v24 = r3 * q2 * r2 * Math.Pow(q1, 2.0);
            double numz = (-1 * v1) - v2 - v3 - v4 - v5 + v6 - v7 - v8 + v9 + v10 + v11 + v12 + v13 - v14 + v15 + v16 - v17 - v18 - v19 + v20 - v21 + v22 + v23 + v24;
            double n1 = Math.Pow(q2, 2.0) * Math.Pow(r1, 2.0);
            double n2 = Math.Pow(q3, 2.0) * Math.Pow(r1, 2.0);
            double n3 = Math.Pow(r2, 2.0) * Math.Pow(q3, 2.0);
            double n4 = Math.Pow(q1, 2.0) * Math.Pow(r3, 2.0);
            double n5 = Math.Pow(r3, 2.0) * Math.Pow(q2, 2.0);
            double n6 = Math.Pow(q1, 2.0) * Math.Pow(r2, 2.0);
            double n7 = 2 * r3 * q2 * r2 * q3;
            double n8 = 2 * q3 * r1 * q1 * r3;
            double n9 = 2 * q1 * r2 * r1 * q2;
            double den = n1 + n2 + n3 + n4 + n5 + n6 - n7 - n8 - n9;
            if (den != 0.0)
            {
                double x = (0.5 * (numx / den)) + pk_3.X;
                double y = ((-0.5) * (numy / den)) + pk_3.Y;
                double z = ((-0.5) * (numz / den)) + pk_3.Z;
                Vector3 xyz = new Vector3( x, y, z );
                return xyz;
            }
            Vector3 xyz_2 = new Vector3(0, 0, 0);
            return xyz_2;
        }
    }
}


