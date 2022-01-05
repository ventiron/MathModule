using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Volume
    {
        public static double TetraederVolume(Triangle basis, Point top)
        {
            Point[] points = basis.GetPoints();
            Vector3D v1 = new Vector3D(top, points[0]);
            Vector3D v2 = new Vector3D(top, points[1]);
            Vector3D v3 = new Vector3D(top, points[2]);
            return Math.Abs(MxF.Det(new double[3, 3] { { v1.X, v1.Y, v1.Z }, { v2.X, v2.Y, v2.Z }, { v3.X, v3.Y, v3.Z } }))/6;
        }
        public static double TruncatedTrianglePiramidVolumeForRegularModel(Triangle top, Triangle bottom)
        {
            double area1 = TetraederVolume(top, bottom.GetPoints()[0]);
            double area2 = TetraederVolume(bottom, top.GetPoints()[1]);
            double area3 = TetraederVolume(new Triangle(
                new TriangulationEdge(top.GetPoints()[1], bottom.GetPoints()[0]),
                new TriangulationEdge(bottom.GetPoints()[0], bottom.GetPoints()[2]),
                new TriangulationEdge(bottom.GetPoints()[2], top.GetPoints()[1])),
                top.GetPoints()[2]);

            return area1 + area2 + area3;
        }

        public static double[] RectangleVolumeForRegularModel(Parallelogram rec1, Parallelogram rec2)
        {
            double[] result = new double[3];
            double topMoreBottom = 0;
            double bottomMoreTop = 0;
            for (int j = 0; j < 2; j++)
            {
                Triangle trTop = null;
                Triangle trBottom = null;

                if (j == 0)
                {
                    trTop = new Triangle(new TriangulationEdge(rec1.GetPoints()[0], rec1.GetPoints()[1]), new TriangulationEdge(rec1.GetPoints()[1], rec1.GetPoints()[2]), new TriangulationEdge(rec1.GetPoints()[2], rec1.GetPoints()[0]));
                    trBottom = new Triangle(new TriangulationEdge(rec2.GetPoints()[0], rec2.GetPoints()[1]), new TriangulationEdge(rec2.GetPoints()[1], rec2.GetPoints()[2]), new TriangulationEdge(rec2.GetPoints()[2], rec2.GetPoints()[0]));
                }
                else
                {
                    trTop = new Triangle(new TriangulationEdge(rec1.GetPoints()[0], rec1.GetPoints()[2]), new TriangulationEdge(rec1.GetPoints()[2], rec1.GetPoints()[3]), new TriangulationEdge(rec1.GetPoints()[3], rec1.GetPoints()[0]));
                    trBottom = new Triangle(new TriangulationEdge(rec2.GetPoints()[0], rec2.GetPoints()[2]), new TriangulationEdge(rec2.GetPoints()[2], rec2.GetPoints()[3]), new TriangulationEdge(rec2.GetPoints()[3], rec2.GetPoints()[0]));
                }

                bool[] isPointsEqual = new bool[3] { false, false, false };
                byte countOfEquality = 0;


                for (int i = 0; i < 3; i++)
                {
                    if (BMF.Deq(trTop.GetPoints()[i].Z, trBottom.GetPoints()[i].Z))
                    {
                        isPointsEqual[i] = true;
                        countOfEquality++;
                    }
                }

                switch (countOfEquality)
                {
                    case 3:
                        break;
                    case 2:
                        Point top = null;
                        Point bottom = null;
                        for (int i = 0; i < 3; i++)
                        {
                            if (!isPointsEqual[i])
                            {
                                top = trTop.GetPoints()[i];
                                bottom = trBottom.GetPoints()[i];
                                break;
                            }
                        }
                        double res = TetraederVolume(trBottom, top);
                        if (top.Z > bottom.Z)
                        {
                            topMoreBottom += res;
                        }
                        else
                        {
                            bottomMoreTop += res;
                        }
                        break;
                    case 1:
                        int intersect = 0;
                        List<int> notInter = new List<int>();
                        for (int i = 0; i < 3; i++)
                        {
                            if (isPointsEqual[i])
                            {
                                intersect = i;
                            }
                            else
                            {
                                notInter.Add(i);
                            }
                        }


                        if (trTop.GetPoints()[notInter[0]].Z > trBottom.GetPoints()[notInter[0]].Z && trTop.GetPoints()[notInter[1]].Z > trBottom.GetPoints()[notInter[1]].Z)
                        {
                            topMoreBottom += TetraederVolume(new Triangle(
                                new TriangulationEdge(trTop.GetPoints()[notInter[0]], trTop.GetPoints()[notInter[1]]),
                                new TriangulationEdge(trTop.GetPoints()[notInter[1]], trBottom.GetPoints()[notInter[0]]),
                                new TriangulationEdge(trBottom.GetPoints()[notInter[0]], trTop.GetPoints()[notInter[0]])),
                                trTop.GetPoints()[intersect]);
                            topMoreBottom += TetraederVolume(new Triangle(
                                new TriangulationEdge(trBottom.GetPoints()[notInter[0]], trBottom.GetPoints()[notInter[1]]),
                                new TriangulationEdge(trBottom.GetPoints()[notInter[1]], trTop.GetPoints()[notInter[1]]),
                                new TriangulationEdge(trTop.GetPoints()[notInter[1]], trBottom.GetPoints()[notInter[0]])),
                                trTop.GetPoints()[intersect]);
                            break;
                        }
                        else if (trTop.GetPoints()[notInter[0]].Z < trBottom.GetPoints()[notInter[0]].Z && trTop.GetPoints()[notInter[1]].Z < trBottom.GetPoints()[notInter[1]].Z)
                        {
                            bottomMoreTop += TetraederVolume(new Triangle(
                                new TriangulationEdge(trTop.GetPoints()[notInter[0]], trTop.GetPoints()[notInter[1]]),
                                new TriangulationEdge(trTop.GetPoints()[notInter[1]], trBottom.GetPoints()[notInter[0]]),
                                new TriangulationEdge(trBottom.GetPoints()[notInter[0]], trTop.GetPoints()[notInter[0]])),
                                trTop.GetPoints()[intersect]);
                            bottomMoreTop += TetraederVolume(new Triangle(
                                new TriangulationEdge(trBottom.GetPoints()[notInter[0]], trBottom.GetPoints()[notInter[1]]),
                                new TriangulationEdge(trBottom.GetPoints()[notInter[1]], trTop.GetPoints()[notInter[1]]),
                                new TriangulationEdge(trTop.GetPoints()[notInter[1]], trBottom.GetPoints()[notInter[0]])),
                                trTop.GetPoints()[intersect]);
                            break;
                        }

                        TriangulationEdge edge1 = trTop.GetEdgeByPoints(trTop.GetPoints()[notInter[0]], trTop.GetPoints()[notInter[1]]);
                        TriangulationEdge edge2 = trBottom.GetEdgeByPoints(trBottom.GetPoints()[notInter[0]], trBottom.GetPoints()[notInter[1]]);
                        Point inter = edge2.GetClosestPoint3D(edge1);
                        inter.id = "inter";

                        double res1 = TetraederVolume(new Triangle(
                            new TriangulationEdge(trTop.GetPoints()[intersect], inter),
                            new TriangulationEdge(inter, trBottom.GetPoints()[notInter[0]]),
                            new TriangulationEdge(trBottom.GetPoints()[notInter[0]], trTop.GetPoints()[intersect])),
                            trTop.GetPoints()[notInter[1]]);
                        //Console.WriteLine(inter);
                        if (trTop.GetPoints()[notInter[0]].Z > trBottom.GetPoints()[notInter[0]].Z)
                        {
                            topMoreBottom += res1;
                        }
                        else
                        {
                            bottomMoreTop += res1;
                        }

                        double res2 = TetraederVolume(new Triangle(
                            new TriangulationEdge(trTop.GetPoints()[intersect], inter),
                            new TriangulationEdge(inter, trBottom.GetPoints()[notInter[1]]),
                            new TriangulationEdge(trBottom.GetPoints()[notInter[1]], trTop.GetPoints()[intersect])),
                            trTop.GetPoints()[notInter[1]]);
                        if (trTop.GetPoints()[notInter[1]].Z > trBottom.GetPoints()[notInter[1]].Z)
                        {
                            topMoreBottom += res2;
                        }
                        else
                        {
                            bottomMoreTop += res2;
                        }
                        break;
                    case 0:
                        if (trBottom.GetPoints()[0].Z > trTop.GetPoints()[0].Z && trBottom.GetPoints()[1].Z > trTop.GetPoints()[1].Z && trBottom.GetPoints()[2].Z > trTop.GetPoints()[2].Z)
                        {
                            bottomMoreTop += TruncatedTrianglePiramidVolumeForRegularModel(trTop, trBottom);
                            break;
                        }
                        else if (trBottom.GetPoints()[0].Z < trTop.GetPoints()[0].Z && trBottom.GetPoints()[1].Z < trTop.GetPoints()[1].Z && trBottom.GetPoints()[2].Z < trTop.GetPoints()[2].Z)
                        {
                            topMoreBottom += TruncatedTrianglePiramidVolumeForRegularModel(trTop, trBottom);
                            break;
                        }

                        bool[] bottomHigher = new bool[3] { false, false, false };
                        int bottomHigherCount = 0;
                        for (int i = 0; i < 3; i++)
                        {
                            if (trBottom.GetPoints()[i].Z > trTop.GetPoints()[i].Z)
                            {
                                bottomHigher[i] = true;
                                bottomHigherCount++;
                            }
                        }
                        int differ = 0;
                        List<int> common = new List<int>();

                        if (bottomHigherCount == 1)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (bottomHigher[i])
                                {
                                    differ = i;
                                }
                                else
                                {
                                    common.Add(i);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (!bottomHigher[i])
                                {
                                    differ = i;
                                }
                                else
                                {
                                    common.Add(i);
                                }
                            }
                        }

                        Point inter1 = new TriangulationEdge(trTop.GetPoints()[common[0]], trTop.GetPoints()[differ]).GetIntersection3D(new TriangulationEdge(trBottom.GetPoints()[common[0]], trBottom.GetPoints()[differ]));
                        Point inter2 = new TriangulationEdge(trTop.GetPoints()[common[1]], trTop.GetPoints()[differ]).GetIntersection3D(new TriangulationEdge(trBottom.GetPoints()[common[1]], trBottom.GetPoints()[differ]));

                        Triangle tr1 = new Triangle(
                            new TriangulationEdge(trTop.GetPoints()[common[0]], inter1),
                            new TriangulationEdge(inter1, trBottom.GetPoints()[common[0]]),
                            new TriangulationEdge(trBottom.GetPoints()[common[0]], trTop.GetPoints()[common[0]]));
                        Triangle tr2 = new Triangle(
                            new TriangulationEdge(trTop.GetPoints()[common[1]], inter2),
                            new TriangulationEdge(inter2, trBottom.GetPoints()[common[1]]),
                            new TriangulationEdge(trBottom.GetPoints()[common[1]], trTop.GetPoints()[common[1]]));

                        double bigRes = TruncatedTrianglePiramidVolumeForRegularModel(tr1, tr2);
                        double smalRes = TetraederVolume(new Triangle(
                            new TriangulationEdge(inter1, inter2),
                            new TriangulationEdge(inter2, trBottom.GetPoints()[differ]),
                            new TriangulationEdge(trBottom.GetPoints()[differ], inter1)), trTop.GetPoints()[differ]);
                        if(bottomHigherCount == 1)
                        {
                            topMoreBottom += bigRes;
                            bottomMoreTop += smalRes;
                        }
                        else
                        {
                            topMoreBottom += smalRes;
                            bottomMoreTop += bigRes;
                        }

                        break;
                }
                result[2] += TruncatedTrianglePiramidVolumeForRegularModel(trTop, trBottom);
            }



            result[0] = topMoreBottom;
            result[1] = bottomMoreTop;
            return result;
        }
    }
}
