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
        //public RegularRectangleNet(Triangulation triangulation, decimal step)
        //{
        //    GetPoints() = new List<List<Point>>();
        //    CreateFromTriangulation(triangulation, step);
        //}
        public RegularRectangleNet(Triangulation triangulation, Polygon border, decimal step)
        {
            rectangles = new List<Parallelogram>();
            CreateFromTriangulation(triangulation, border, step);
        }
        //public void CreateFromTriangulation(Triangulation triangulation, decimal step)
        //{
        //    decimal minX = triangulation.GetPoints()[0].X;
        //    decimal minY = triangulation.GetPoints()[0].Y;
        //    decimal maxX = triangulation.GetPoints()[0].X;
        //    decimal maxY = triangulation.GetPoints()[0].Y;
        //    decimal minZ = triangulation.GetPoints()[0].Z;
        //    decimal maxZ = triangulation.GetPoints()[0].Z;
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

        //    decimal sMinX = minX + (step - (minX % step));
        //    decimal sMinY = minY + (step - (minY % step));
        //    decimal sMinZ = minZ + (step - (minZ % step));
        //    decimal sMaxX = maxX - (maxX % step);
        //    decimal sMaxY = maxY - (maxY % step);
        //    decimal sMaxZ = maxZ - (maxZ % step);

        //    Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
        //    for (decimal i = sMinX; i < sMaxX; i += step)
        //    {
        //        List<Point> tPoints = new List<Point>();
        //        for (decimal j = sMinY; j < sMaxY; j += step)
        //        {
        //            Point nPoint = new Point(i, j, 0);
        //            if (outerPolygon.IsInside(nPoint))
        //            {
        //                //foreach (Triangle tr in triangulation.triangles)
        //                //{
        //                //    if (tr.IsPointInsideTriangle(nPoint1))
        //                //    {
        //                Triangle tr = triangulation.FindTriangle_Borders(nPoint);
        //                decimal[] factors = tr.GetPlanarFactors();
        //                nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
        //                tPoints.Add(nPoint);
        //                //break;
        //                //    }
        //                //}
        //                continue;
        //            }
        //            decimal dist = BMF.GetDistance2D(triangulation.outerEdges[0].GetPoints()[0], nPoint);
        //            Point zPoint = triangulation.outerEdges[0].GetPoints()[0];
        //            foreach (TriangulationEdge edge in triangulation.outerEdges)
        //            {
        //                decimal tDist = edge.GetLengthTo2D(nPoint);
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
        public void CreateFromTriangulation(Triangulation triangulation, Polygon border, decimal step)
        {
            decimal minX = triangulation.points[0].X;
            decimal minY = triangulation.points[0].Y;
            decimal maxX = triangulation.points[0].X;
            decimal maxY = triangulation.points[0].Y;
            decimal minZ = triangulation.points[0].Z;
            decimal maxZ = triangulation.points[0].Z;

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

            decimal sMinX = minX + (step - (minX % step));
            decimal sMinY = minY + (step - (minY % step));
            decimal sMinZ = minZ + (step - (minZ % step));
            decimal sMaxX = maxX - (maxX % step);
            decimal sMaxY = maxY - (maxY % step);
            decimal sMaxZ = maxZ - (maxZ % step);


            decimal minXB = border.GetPoints()[0].X;
            decimal minYB = border.GetPoints()[0].Y;
            decimal maxXB = border.GetPoints()[0].X;
            decimal maxYB = border.GetPoints()[0].Y;
            decimal minZB = border.GetPoints()[0].Z;
            decimal maxZB = border.GetPoints()[0].Z;

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

            decimal sMinXB = minX + (step - (minX % step));
            decimal sMinYB = minY + (step - (minY % step));
            decimal sMinZB = minZ + (step - (minZ % step));
            decimal sMaxXB = maxX - (maxX % step);
            decimal sMaxYB = maxY - (maxY % step);
            decimal sMaxZB = maxZ - (maxZ % step);

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (decimal i = sMinXB; i < sMaxXB; i += step)
            {
                List<Point> tPoints = new List<Point>();
                for (decimal j = sMinYB; j < sMaxYB; j += step)
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



        public void FindPointZ(Point p, Polygon border, Triangulation triangulation, decimal step)
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
                    decimal[] factors = tr.GetPlanarFactors();
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
                    decimal[] factors = tr.GetPlanarFactors();
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
                    decimal[] factors = tr.GetPlanarFactors();
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
                    decimal[] factors = tr.GetPlanarFactors();
                    p.Z = ((-p.X * factors[0]) + (-p.Y * factors[1]) - factors[3]) / factors[2];
                    //break;
                    //    }
                    //}
                }
            }
        }

        public TriangulationEdge GetClosestSegment(Point point, List<TriangulationEdge> outerEdges)
        {
            decimal count = outerEdges[0].GetLengthTo2D(point);
            TriangulationEdge result = outerEdges[0];
            foreach (TriangulationEdge segment in outerEdges)
            {
                decimal dist = segment.GetLengthTo2D(point);
                if (dist < count)
                {
                    result = segment;
                }
            }
            return result;
        }



        public decimal[] GetVolume(RegularRectangleNet net)
        {
            decimal[] result = new decimal[3];

            for(int i = 0; i < this.rectangles.Count; i++)
            {
                if (this.rectangles[i].IsEqual2D(net.rectangles[i]))
                {
                    decimal[] sRes = Volume.RectangleVolumeForRegularModel(this.rectangles[i], net.rectangles[i]);
                    result[0] += sRes[0];
                    result[1] += sRes[1];
                    result[2] = sRes[1];
                }
            }
            return result;
        }

    }
}
