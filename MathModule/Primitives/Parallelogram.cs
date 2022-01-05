using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Parallelogram : Polygon
    {
        new Point[] points = new Point[4];

        public Parallelogram(Point p1, Point p2, Point p3, Point p4) : base(new List<Segment>() { new TriangulationEdge(p1, p2), new TriangulationEdge(p2, p3), new TriangulationEdge(p3, p4), new TriangulationEdge(p4,p1)})
        {
            this.points[0] = p1;
            this.points[1] = p2;
            this.points[2] = p3;
            this.points[3] = p4;
        }
        public Parallelogram(decimal minX, decimal minY, decimal maxX, decimal maxY)
        {
            Point p1 = new Point(minX, minY, 0m);
            Point p2 = new Point(minX, maxY, 0m);
            Point p3 = new Point(maxX, maxY, 0m);
            Point p4 = new Point(maxX, minY, 0m);

            List<Segment> segments = new List<Segment>() {
            new TriangulationEdge(p1, p2),
            new TriangulationEdge(p2, p3),
            new TriangulationEdge(p3, p4),
            new TriangulationEdge(p4, p1) };


            this.points[0] = p1;
            this.points[1] = p2;
            this.points[2] = p3;
            this.points[3] = p4;
        }
        public decimal GetMinX()
        {
            decimal result = points[0].X;
            foreach(Point p in points)
            {
                if(p.X < result)
                {
                    result = p.X;
                }
            }
            return result;
        }
        public decimal GetMaxX()
        {
            decimal result = points[0].X;
            foreach (Point p in points)
            {
                if (p.X > result)
                {
                    result = p.X;
                }
            }
            return result;
        }
        public decimal GetMinY()
        {
            decimal result = points[0].Y;
            foreach (Point p in points)
            {
                if (p.Y < result)
                {
                    result = p.Y;
                }
            }
            return result;
        }
        public decimal GetMaxY()
        {
            decimal result = points[0].Y;
            foreach (Point p in points)
            {
                if (p.Y > result)
                {
                    result = p.Y;
                }
            }
            return result;
        }

        public bool IsEqual2D(Parallelogram par)
        {
            bool b1 = this.points[0].IsEqual2D(par.points[0]) || this.points[0].IsEqual2D(par.points[1]) || this.points[0].IsEqual2D(par.points[2]) || this.points[0].IsEqual2D(par.points[3]);
            bool b2 = this.points[1].IsEqual2D(par.points[0]) || this.points[1].IsEqual2D(par.points[1]) || this.points[1].IsEqual2D(par.points[2]) || this.points[1].IsEqual2D(par.points[3]);
            bool b3 = this.points[2].IsEqual2D(par.points[0]) || this.points[2].IsEqual2D(par.points[1]) || this.points[2].IsEqual2D(par.points[2]) || this.points[2].IsEqual2D(par.points[3]);
            bool b4 = this.points[3].IsEqual2D(par.points[0]) || this.points[3].IsEqual2D(par.points[1]) || this.points[3].IsEqual2D(par.points[2]) || this.points[3].IsEqual2D(par.points[3]);

            return b1 && b2 && b3 && b4;
        }
    }
}
