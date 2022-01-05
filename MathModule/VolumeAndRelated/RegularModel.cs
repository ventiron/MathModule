using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathModule
{
    public class RegularModel
    {
        public List<Point> points;
        public RegularModel()
        {
            points = new List<Point>();
        }
        public RegularModel(Triangulation triangulation, decimal step)
        {
            MessageBox.Show("Изменения 2 приняты");
            points = new List<Point>();
            CreateFromTriangulation(triangulation, step);
        }
        public RegularModel(Triangulation triangulation, Polygon border, decimal step)
        {
            points = new List<Point>();
            CreateFromTriangulation(triangulation, border, step);
        }
        public void CreateFromTriangulation(Triangulation triangulation, decimal step)
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

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (decimal i = sMinX; i < sMaxX; i += step)
            {
                for(decimal j = sMinY; j < sMaxY; j += step)
                {
                    Point nPoint = new Point(i, j, 0);
                    if (outerPolygon.IsInside(nPoint))
                    {
                        //foreach (Triangle tr in triangulation.triangles)
                        //{
                        //    if (tr.IsInside(nPoint))
                        //    {
                        Triangle tr = triangulation.FindTriangle(nPoint);
                        decimal[] factors = tr.GetPlanarFactors();
                        nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
                        points.Add(nPoint);
                        //        break;
                        //    }
                        //}
                        continue;
                    }
                    decimal dist = BMF.GetDistance2D(triangulation.outerEdges[0].points[0],nPoint);
                    Point zPoint = triangulation.outerEdges[0].points[0];
                    foreach(TriangulationEdge edge in triangulation.outerEdges)
                    {
                        decimal tDist = edge.GetLengthTo2D(nPoint);
                        if(dist > tDist)
                        {
                            dist = tDist;
                            zPoint = edge.points[0];
                        }
                    }
                    if (dist > step) continue;
                    nPoint.Z = zPoint.Z;
                    points.Add(nPoint);
                }
            }
        }
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

            foreach(Point p in border.GetPoints())
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

            decimal sMinXB = minXB + (step - (minXB % step));
            decimal sMinYB = minYB + (step - (minYB % step));
            decimal sMinZB = minZB + (step - (minZB % step));
            decimal sMaxXB = maxXB - (maxXB % step);
            decimal sMaxYB = maxYB - (maxYB % step);
            decimal sMaxZB = maxZB - (maxZB % step);

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (decimal i = sMinXB; i < sMaxXB; i += step)
            {
                for (decimal j = sMinYB; j < sMaxYB; j += step)
                {
                    Point nPoint = new Point(i, j, 0);
                    if (border.IsInside(nPoint))
                    {
                        if (outerPolygon.IsInside(nPoint))
                        {
                            //foreach (Triangle tr in triangulation.triangles)
                            //{
                            //    if (tr.IsPointInsideTriangle(nPoint))
                            //    {
                            Triangle tr = triangulation.FindTriangle_Borders(nPoint);
                            decimal[] factors = tr.GetPlanarFactors();
                            nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
                            points.Add(nPoint);
                            //break;
                            //    }
                            //}
                        }
                        continue;
                    }
                    decimal dist = border.GetDistance2DToPoint(nPoint);
                    if (dist > step)
                    {
                        continue;
                    }
                    if (outerPolygon.IsInside(nPoint))
                    {
                        //foreach (Triangle tr in triangulation.triangles)
                        //{
                        //    if (tr.IsPointInsideTriangle(nPoint))
                        //    {
                        Triangle tr = triangulation.FindTriangle_Borders(nPoint);
                        decimal[] factors = tr.GetPlanarFactors();
                        nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
                        points.Add(nPoint);
                        //break;
                        //    }
                        //}
                    }
                }
            }
        }
    }
}
