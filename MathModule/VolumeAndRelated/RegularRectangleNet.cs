using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
        public RegularRectangleNet(Triangulation triangulation, Polygon border, double step)
        {
            rectangles = new List<Parallelogram>();
            CreateFromTriangulation(triangulation, border, step);
        }
        public void CreateFromTriangulation(Triangulation triangulation, Polygon border, double step)
        {

            List<Point> BorderPoints = border.GetPoints();

            Point minPoint = BMF.GetMin(triangulation.points);
            Point maxPoint = BMF.GetMax(triangulation.points);
            Point minPointB = BMF.GetMin(BorderPoints);
            Point maxPointB = BMF.GetMax(BorderPoints);

            double sMinXB = minPoint.X + (step - (minPoint.X % step));
            double sMinYB = minPoint.Y + (step - (minPoint.Y % step));
            double sMinZB = minPoint.Z + (step - (minPoint.Z % step));
            double sMaxXB = maxPoint.X - (maxPoint.X % step);
            double sMaxYB = maxPoint.Y - (maxPoint.Y % step);
            double sMaxZB = maxPoint.Z - (maxPoint.Z % step);

            double sMinXBB = minPointB.X + (step - (minPointB.X % step));
            double sMinYBB = minPointB.Y + (step - (minPointB.Y % step));
            double sMinZBB = minPointB.Z + (step - (minPointB.Z % step));
            double sMaxXBB = maxPointB.X - (maxPointB.X % step);
            double sMaxYBB = maxPointB.Y - (maxPointB.Y % step);
            double sMaxZBB = maxPointB.Z - (maxPointB.Z % step);

            if (sMinXBB < sMinXB) sMinXBB = sMinXB;
            if (sMinYBB < sMinYB) sMinYBB = sMinYB;
            if (sMinZBB < sMinZB) sMinZBB = sMinZB;
            if (sMaxXBB > sMaxXB) sMaxXBB = sMaxXB;
            if (sMaxYBB > sMaxYB) sMaxYBB = sMaxYB;
            if (sMaxZBB > sMaxZB) sMaxZBB = sMaxZB;

            this.step = step;
            this.minX = sMinXBB;
            this.minY = sMinYBB;

            int XCount = 0;
            int YCount = 0;

            rectanglesArray = new Parallelogram[(int)((sMaxXBB - sMinXBB) / step), (int)((sMaxYBB - sMinYBB) / step)];
            Point[,] points = new Point[(int)((sMaxXBB - sMinXBB) / step) + 1, (int)((sMaxYBB - sMinYBB) / step) + 1];


            List<Task> tasks = new List<Task>();
            for(int i = 0; sMinXBB < sMaxXBB - step * 0.001 + step - step * i; i++)
            {
                for (int j = 0; sMinYBB < sMaxYBB - step * 0.001 + step - step * j; j++)
                {
                    Point point = new Point(sMinXBB + step * i, sMinYBB + step * j, 0);
                    Task task = new Task(() => FindPointZ(point, border, triangulation, step));
                    task.Start();
                    tasks.Add(task);
                    points[i, j] = point;
                }
            }
            foreach (Task task in tasks)
            {
                task.Wait();
            }
            for (int i = 0; i < points.GetLength(0)-1; i++)
            {
                for(int j = 0; j <points.GetLength(1)-1; j++)
                {
                    if (BMF.Deq(points[i, j].Z, 0) || BMF.Deq(points[i + 1, j].Z, 0) || BMF.Deq(points[i + 1, j + 1].Z, 0) || BMF.Deq(points[i, j + 1].Z, 0)) 
                    {
                        continue;
                    }

                    Parallelogram par = new Parallelogram(points[i, j], points[i + 1, j], points[i + 1, j + 1], points[i, j + 1]);
                    rectanglesArray[i, j] = par;
                    rectangles.Add(par);
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
                else if (outerPolygon.GetDistance2DToPoint(p) < step / 1000)
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
                else if (outerPolygon.GetDistance2DToPoint(p) < step / 1000)
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
            int i = 1;
            List<Task<double[]>> tasks = new List<Task<double[]>>();
            if (BMF.Deq(this.step, net.step) && minX < maxX1 && minY < maxY1)
            {
                for (; startX < endX; startX++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        if (rectanglesArray[startX, y] != null && net.rectanglesArray[startX - x1, y - y1] != null)
                        {
                            int a = startX;
                            int b = y;
                            Task <double[]> task = new Task<double[]>(() => Volume.RectangleVolumeForRegularModel(rectanglesArray[a, b], net.rectanglesArray[a - x1, b - y1]));
                            task.Start();
                            tasks.Add(task);
                        }
                    }
                }
            }
            foreach(Task<double[]> task in tasks)
            {
                task.Wait();
                result[0] += task.Result[0];
                result[1] += task.Result[1];
                result[2] += task.Result[1];
            }
            return result;
        }

    }
}
