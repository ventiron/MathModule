using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Circle : MathObject
    {
        public double radius;
        public Point center;

        public Circle(Point center, double radius)
        {
            this.radius = radius;
            this.center = center;
        }

        public Point[] GetIntersection(Segment seg)
        {
            Point[] result = new Point[2];
            Vector2D segV = new Vector2D(seg);
            double segVK = segV.ScalarMultOrtho(segV);

            double k1 = 2 * (segV.X * (seg.points[0].X - center.X) + segV.Y * (seg.points[0].Y - center.Y));
            Vector2D tempVect = new Vector2D(center, seg.points[0]);
            double k2 = tempVect.ScalarMultOrtho(tempVect) - (radius * radius);

            double det = k1 * k1 - 4 * segVK * k2;
            if (segVK <= 1E-12 || det < 0)
            {
                return null;
            }
            else if (BMF.Deq(det, 0))
            {
                double mod = -k1 / (segVK * 2);
                Point newPoint = new Point(seg.points[0].X + segV.X * mod, seg.points[0].Y + segV.Y * mod, 0);
                if (seg.isPointBelongToSegment(newPoint))
                {
                    result[0] = new Point(seg.points[0].X + segV.X * mod, seg.points[0].Y + segV.Y * mod, 0);
                }
            }
            else
            {
                double mod = (-k1 + Math.Sqrt(det)) / (segVK * 2);
                Point newPoint = new Point(seg.points[0].X + segV.X * mod, seg.points[0].Y + segV.Y * mod, 0);
                if (seg.isPointBelongToSegment(newPoint))
                {
                    result[0] = new Point(seg.points[0].X + segV.X * mod, seg.points[0].Y + segV.Y * mod, 0);
                }
                mod = (-k1 - Math.Sqrt(det)) / (segVK * 2);
                newPoint = new Point(seg.points[0].X + segV.X * mod, seg.points[0].Y + segV.Y * mod, 0);
                if (seg.isPointBelongToSegment(newPoint))
                {
                    result[1] = new Point(seg.points[0].X + segV.X * mod, seg.points[0].Y + segV.Y * mod, 0);
                }
            }
            return result;
        }
        public List<Point> GetIntersection(Polyline poly)
        {
            List<Point> result = new List<Point>();
            foreach(Segment seg in poly.GetSegmentsCopy())
            {
                Point[] inter = this.GetIntersection(seg);
                if(inter[0] != null)
                {
                    result.Add(inter[0]);
                }
                if (inter[1] != null)
                {
                    result.Add(inter[1]);
                }
            }
            return result;
        }

        public double GetArea()
        {
            return Math.PI * 2 * radius;
        }
    }
}
