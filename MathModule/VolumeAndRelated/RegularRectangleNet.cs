using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class RegularRectangleNet
    {
        public List<Parallelogram> rectangles;
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
            minX -= step;
            minY -= step;
            minZ -= step;
            maxX += step;
            maxY += step;
            maxZ += step;

            double sMinX = minX + (step - (minX % step));
            double sMinY = minY + (step - (minY % step));
            double sMinZ = minZ + (step - (minZ % step));
            double sMaxX = maxX - (maxX % step);
            double sMaxY = maxY - (maxY % step);
            double sMaxZ = maxZ - (maxZ % step);


            double minXB = border.GetPoints()[0].X;
            double minYB = border.GetPoints()[0].Y;
            double maxXB = border.GetPoints()[0].X;
            double maxYB = border.GetPoints()[0].Y;
            double minZB = border.GetPoints()[0].Z;
            double maxZB = border.GetPoints()[0].Z;

            foreach (Point p in border.GetPoints())
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

            double sMinXB = minX + (step - (minX % step));
            double sMinYB = minY + (step - (minY % step));
            double sMinZB = minZ + (step - (minZ % step));
            double sMaxXB = maxX - (maxX % step);
            double sMaxYB = maxY - (maxY % step);
            double sMaxZB = maxZ - (maxZ % step);

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (double i = sMinXB; i < sMaxXB; i += step)
            {
                List<Point> tPoints = new List<Point>();
                for (double j = sMinYB; j < sMaxYB; j += step)
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
                        continue;
                    }

                    rectangles.Add(new Parallelogram(nPoint1, nPoint2, nPoint3, nPoint4));
                }
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
                    Triangle tr = triangulation.FindTriangle(p);
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
                    Triangle tr = triangulation.FindTriangle(p);
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
            double[] result = new double[3];

            for(int i = 0; i < this.rectangles.Count; i++)
            {
                if (this.rectangles[i].IsEqual2D(net.rectangles[i]))
                {
                    double[] sRes = Volume.RectangleVolumeForRegularModel(this.rectangles[i], net.rectangles[i]);
                    result[0] += sRes[0];
                    result[1] += sRes[1];
                    result[2] = sRes[1];
                }
            }
            return result;
        }

    }
}
