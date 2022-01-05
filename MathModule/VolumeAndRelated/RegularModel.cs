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
        public RegularModel(Triangulation triangulation, double step)
        {
            MessageBox.Show("Изменения 2 приняты");
            points = new List<Point>();
            CreateFromTriangulation(triangulation, step);
        }
        public RegularModel(Triangulation triangulation, Polygon border, double step)
        {
            points = new List<Point>();
            CreateFromTriangulation(triangulation, border, step);
        }
        public void CreateFromTriangulation(Triangulation triangulation, double step)
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

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (double i = sMinX; i < sMaxX; i += step)
            {
                for(double j = sMinY; j < sMaxY; j += step)
                {
                    Point nPoint = new Point(i, j, 0);
                    if (outerPolygon.IsInside(nPoint))
                    {
                        //foreach (Triangle tr in triangulation.triangles)
                        //{
                        //    if (tr.IsInside(nPoint))
                        //    {
                        Triangle tr = triangulation.FindTriangle(nPoint);
                        double[] factors = tr.GetPlanarFactors();
                        nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
                        points.Add(nPoint);
                        //        break;
                        //    }
                        //}
                        continue;
                    }
                    double dist = BMF.GetDistance2D(triangulation.outerEdges[0].points[0],nPoint);
                    Point zPoint = triangulation.outerEdges[0].points[0];
                    foreach(TriangulationEdge edge in triangulation.outerEdges)
                    {
                        double tDist = edge.GetLengthTo2D(nPoint);
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

            double sMinXB = minXB + (step - (minXB % step));
            double sMinYB = minYB + (step - (minYB % step));
            double sMinZB = minZB + (step - (minZB % step));
            double sMaxXB = maxXB - (maxXB % step);
            double sMaxYB = maxYB - (maxYB % step);
            double sMaxZB = maxZB - (maxZB % step);

            Polygon outerPolygon = new Polygon(new List<Segment>(triangulation.outerEdges));
            for (double i = sMinXB; i < sMaxXB; i += step)
            {
                for (double j = sMinYB; j < sMaxYB; j += step)
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
                            double[] factors = tr.GetPlanarFactors();
                            nPoint.Z = ((-nPoint.X * factors[0]) + (-nPoint.Y * factors[1]) - factors[3]) / factors[2];
                            points.Add(nPoint);
                            //break;
                            //    }
                            //}
                        }
                        continue;
                    }
                    double dist = border.GetDistance2DToPoint(nPoint);
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
                        double[] factors = tr.GetPlanarFactors();
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
