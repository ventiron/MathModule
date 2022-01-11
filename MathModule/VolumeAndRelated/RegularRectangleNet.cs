using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathModule
{
    public class RegularRectangleNet
    {
        public List<Parallelogram> rectangles;
        //public List<List<Parallelogram>> rectanglesArray;
        public Parallelogram[,] rectanglesArray;
        private double step;
        private double minX;
        private double minY;


        public RegularRectangleNet()
        {
            rectangles = new List<Parallelogram>();
        }
        //public RegularRectangleNet(Triangulation triangulation, double step)
        //{
        //    GetPoints() = new List<List<Point>>();
        //    CreateFromTriangulation(triangulation, step);
        //}
        public RegularRectangleNet(Triangulation triangulation, Polygon border, double step)
        {
            rectangles = new List<Parallelogram>();
            CreateFromTriangulation(triangulation, border, step);
        }
        //public void CreateFromTriangulation(Triangulation triangulation, double step)
        //{
        //    double minX = triangulation.GetPoints()[0].X;
        //    double minY = triangulation.GetPoints()[0].Y;
        //    double maxX = triangulation.GetPoints()[0].X;
        //    double maxY = triangulation.GetPoints()[0].Y;
        //    double minZ = triangulation.GetPoints()[0].Z;
        //    double maxZ = triangulation.GetPoints()[0].Z;
        //    foreach (Point p in triangulation.GetPoints())
        //    {
        //        if (p.X > maxX)
        //        {
        //            maxX = p.X;
        //        }
        //        if (p.X < minX)
        //        {
        //            minX = p.X;
        //        }
        //        if (p.Y > maxY)
        //        {
        //            maxY = p.Y;
        //        }
        //        if (p.Y < minY)
        //        {
        //            minY = p.Y;
        //        }
        //        if (p.Z > maxZ)
        //        {
        //            maxZ = p.Z;
        //        }
        //        if (p.Z < minZ)
        //        {
        //            minZ = p.Z;
        //        }
        //    }
        //    minX -= step;
        //    minY -= step;
        //    minZ -= step;
        //    maxX += step;
        //    maxY += step;
        //    maxZ += step;

        //    double sMinX = minX + (step - (minX % step));
        //    double sMinY = minY + (step - (minY % step));
        //    double sMinZ = minZ + (step - (minZ % step));
        //    double sMaxX = maxX - (maxX % step);
        //    double sMaxY = maxY - (maxY % step);
        //    double sMaxZ = maxZ - (maxZ % step);

        //    Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
        //    for (double i = sMinX; i < sMaxX; i += step)
        //    {
        //        List<Point> tPoints = new List<Point>();
        //        for (double j = sMinY; j < sMaxY; j += step)
        //        {
        //            Point nPoint = new Point(i, j, 0);
        //            if (outerPolygon.IsInside(nPoint))
        //            {
        //                //foreach (Triangle tr in triangulation.triangles)
        //                //{
        //                //    if (tr.IsPointInsideTriangle(nPoint1))
        //                //    {
        //                Triangle tr = triangulation.FindTriangle_Borders(nPoint);
        //                double[] factors = tr.GetPlanarFactors();
        //                nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
        //                tPoints.Add(nPoint);
        //                //break;
        //                //    }
        //                //}
        //                continue;
        //            }
        //            double dist = BMF.GetDistance2D(triangulation.outerEdges[0].GetPoints()[0], nPoint);
        //            Point zPoint = triangulation.outerEdges[0].GetPoints()[0];
        //            foreach (TriangulationEdge edge in triangulation.outerEdges)
        //            {
        //                double tDist = edge.GetLengthTo2D(nPoint);
        //                if (dist > tDist)
        //                {
        //                    dist = tDist;
        //                    zPoint = edge.GetPoints()[0];
        //                }
        //            }
        //            if (dist > step) continue;
        //            nPoint.Z = zPoint.Z;
        //            tPoints.Add(nPoint);
        //        }
        //        GetPoints().Add(tPoints);
        //    }
        //}
        public void CreateFromTriangulation(Triangulation triangulation, Polygon border, double step)
        {
            double minX = triangulation.points[0].X;
            double minY = triangulation.points[0].Y;
            double maxX = triangulation.points[0].X;
            double maxY = triangulation.points[0].Y;
            double minZ = triangulation.points[0].Z;
            double maxZ = triangulation.points[0].Z;


            List<Point> BorderPoints = border.GetPoints();
            double minXB = BorderPoints[0].X;
            double minYB = BorderPoints[0].Y;
            double maxXB = BorderPoints[0].X;
            double maxYB = BorderPoints[0].Y;
            double minZB = BorderPoints[0].Z;
            double maxZB = BorderPoints[0].Z;


            foreach (Point p in BorderPoints)
            {
                if (p.X > maxXB)
                {
                    maxXB = p.X;
                }
                if (p.X < minXB)
                {
                    minXB = p.X;
                }
                if (p.Y > maxYB)
                {
                    maxYB = p.Y;
                }
                if (p.Y < minYB)
                {
                    minYB = p.Y;
                }
                if (p.Z > maxZB)
                {
                    maxZB = p.Z;
                }
                if (p.Z < minZB)
                {
                    minZB = p.Z;
                }
            }
            minXB -= step;
            minYB -= step;
            minZB -= step;
            maxXB += step;
            maxYB += step;
            maxZB += step;
            foreach (Point p in triangulation.points)
            {
                if (p.X > maxX)
                {
                    maxX = p.X;
                }
                if (p.X < minX)
                {
                    minX = p.X;
                }
                if (p.Y > maxY)
                {
                    maxY = p.Y;
                }
                if (p.Y < minY)
                {
                    minY = p.Y;
                }
                if (p.Z > maxZ)
                {
                    maxZ = p.Z;
                }
                if (p.Z < minZ)
                {
                    minZ = p.Z;
                }
            }
            //minX -= step;
            //minY -= step;
            //minZ -= step;
            //maxX += step;
            //maxY += step;
            //maxZ += step;

            //double sMinXB = minX + (step - (minX % step));
            //double sMinYB = minY + (step - (minY % step));
            //double sMinZB = minZ + (step - (minZ % step));
            //double sMaxXB = maxX - (maxX % step);
            //double sMaxYB = maxY - (maxY % step);
            //double sMaxZB = maxZ - (maxZ % step);

            double sMinXBB = minXB + (step - (minXB % step));
            double sMinYBB = minYB + (step - (minYB % step));
            double sMinZBB = minZB + (step - (minZB % step));
            double sMaxXBB = maxXB - (maxXB % step);
            double sMaxYBB = maxYB - (maxYB % step);
            double sMaxZBB = maxZB - (maxZB % step);

            this.step = step;
            this.minX = sMinXBB;
            this.minY = sMinYBB;

            int XCount = 0;
            int YCount = 0;

            rectanglesArray = new Parallelogram[(int)((sMaxXBB - sMinXBB) / step), (int)((sMaxYBB - sMinYBB) / step)];

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (double i = sMinXBB; i < sMaxXBB - step * 1.001; i += step)
            {
                YCount = 0;
                List<Point> tPoints = new List<Point>();
                for (double j = sMinYBB; j < sMaxYBB - step * 1.001; j += step)
                {
                    Point nPoint1 = new Point(i, j, 0);
                    Point nPoint2 = new Point(i + step, j, 0);
                    Point nPoint3 = new Point(i + step, j + step, 0);
                    Point nPoint4 = new Point(i, j + step, 0);

                    FindPointZ(nPoint1, border, triangulation, step);
                    FindPointZ(nPoint2, border, triangulation, step);
                    FindPointZ(nPoint3, border, triangulation, step);
                    FindPointZ(nPoint4, border, triangulation, step);

                    if(BMF.Deq(nPoint1.Z,0) || BMF.Deq(nPoint2.Z, 0) || BMF.Deq(nPoint3.Z, 0) || BMF.Deq(nPoint4.Z, 0))
                    {
                        YCount++;
                        continue;
                    }

                    Parallelogram par = new Parallelogram(nPoint1, nPoint2, nPoint3, nPoint4);
                    rectanglesArray[XCount, YCount] = par;
                    rectangles.Add(par);
                    YCount++;
                }
                XCount++;
            }
        }



        public void FindPointZ(Point p, Polygon border, Triangulation triangulation, double step)
        {
            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            if (border.IsInside(p))
            {
                if (outerPolygon.IsInside(p))
                {
                    //foreach (Triangle tr in triangulation.triangles)
                    //{
                    //    if (tr.IsPointInsideTriangle(nPoint1))
                    //    {
                    Triangle tr = triangulation.FindTriangle_Borders(p);
                    double[] factors = tr.GetPlanarFactors();
                    p.Z = ((-p.X * factors[0]) + (-p.Y * factors[1]) - factors[3]) / factors[2];
                    //break;
                    //    }
                    //}
                }
                else if (outerPolygon.GetDistance2DToPoint(p) < step / 100)
                {
                    //foreach (Triangle tr in triangulation.triangles)
                    //{
                    //    if (tr.IsPointInsideTriangle(nPoint1))
                    //    {
                    TriangulationEdge egads = GetClosestSegment(p, triangulation.outerEdges);
                    Triangle tr = egads.GetOtherTriangle(null);
                    double[] factors = tr.GetPlanarFactors();
                    p.Z = ((-p.X * factors[0]) + (-p.Y * factors[1]) - factors[3]) / factors[2];
                    //break;
                    //    }
                    //}
                }
                return;
            }
            if (!(border.GetDistance2DToPoint(p) > step / 1.6666))
            {
                if (outerPolygon.IsInside(p))
                {
                    //foreach (Triangle tr in triangulation.triangles)
                    //{
                    //    if (tr.IsPointInsideTriangle(nPoint1))
                    //    {
                    Triangle tr = triangulation.FindTriangle_Borders(p);
                    double[] factors = tr.GetPlanarFactors();
                    p.Z = ((-p.X * factors[0]) + (-p.Y * factors[1]) - factors[3]) / factors[2];
                    //break;
                    //    }
                    //}
                }
                else if (outerPolygon.GetDistance2DToPoint(p) < step / 100)
                {
                    //foreach (Triangle tr in triangulation.triangles)
                    //{
                    //    if (tr.IsPointInsideTriangle(nPoint1))
                    //    {
                    Triangle tr = GetClosestSegment(p, triangulation.outerEdges).GetOtherTriangle(null);
                    double[] factors = tr.GetPlanarFactors();
                    p.Z = ((-p.X * factors[0]) + (-p.Y * factors[1]) - factors[3]) / factors[2];
                    //break;
                    //    }
                    //}
                }
            }
        }

        public TriangulationEdge GetClosestSegment(Point point, List<TriangulationEdge> outerEdges)
        {
            double count = outerEdges[0].GetLengthTo2D(point);
            TriangulationEdge result = outerEdges[0];
            foreach (TriangulationEdge segment in outerEdges)
            {
                double dist = segment.GetLengthTo2D(point);
                if (dist < count)
                {
                    result = segment;
                }
            }
            return result;
        }



        public double[] GetVolume(RegularRectangleNet net)
        {
            //try
            //{
            //    double[] result = new double[3];
            //    List<Parallelogram> par = new List<Parallelogram>(net.rectangles);
            //    for (int i = 0; i < this.rectangles.Count; i++)
            //    {
            //        for (int j = 0; j < par.Count; j++)
            //        {
            //            if (this.rectangles[i].IsEqual2D(par[j]))
            //            {
            //                double[] sRes = Volume.RectangleVolumeForRegularModel(this.rectangles[i], par[j]);
            //                result[0] += sRes[0];
            //                result[1] += sRes[1];
            //                result[2] = sRes[1];
            //                par.Remove(par[j]);
            //                break;
            //            }
            //        }
            //    }
            //    return result;
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //    return null;
            //}
            double[] result = new double[3];
            double maxX = this.minX + step * this.rectanglesArray.GetLength(0);
            double maxY = this.minY + step * this.rectanglesArray.GetLength(1);

            double maxX1 = net.minX + step * net.rectanglesArray.GetLength(0);
            double maxY1 = net.minY + step * net.rectanglesArray.GetLength(1);

            double dMaxX = maxX - maxX1;
            double dMaxY = maxY - maxY1;


            int startX = 0;
            int startY = 0;
            int endX = this.rectanglesArray.GetLength(0);
            int endY = this.rectanglesArray.GetLength(1);

            if (dMaxX > 0)
            {
                endX = endX - (int)(dMaxX / step);
            }
            if (dMaxY > 0)
            {
                endY = endY - (int)(dMaxY / step);
            }



            int x1 = (int)((net.minX - this.minX) / step);
            int y1 = (int)((net.minY - this.minY) / step);
            startX = x1 < 0 ? 0 : x1;
            startY = y1 < 0 ? 0 : y1;

            if (BMF.Deq(this.step, net.step) && minX < maxX1 && minY < maxY1)
            {
                for (; startX < endX; startX++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        if (rectanglesArray[startX, y] != null && net.rectanglesArray[startX - x1, y - y1] != null)
                        {
                            double[] sRes = Volume.RectangleVolumeForRegularModel(rectanglesArray[startX, y], net.rectanglesArray[startX - x1, y - y1]);
                            result[0] += sRes[0];
                            result[1] += sRes[1];
                            result[2] = sRes[1];
                        }
                    }
                }
            }
            return result;

        }

    }
}
