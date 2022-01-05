using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Polyline : MathObject
    {
        private List<Segment> segments;
        public bool IsClosed { get; }

        public Polyline(List<Segment> segments, bool IsClosed = false)
        {
            this.segments = segments;
            this.IsClosed = IsClosed;
        }

        public Polyline(List<Point> points, bool isClosed = false)
        {
            if(points.Count < 2)
            {
                throw new ArgumentException("Невозможно построить полилинию из одной точки.", "points");
            }
            segments = new List<Segment>();
            for(int i = 0; i < points.Count - 1; i++)
            {
                this.segments.Add(new Segment(points[i], points[i + 1]));
            }
            if (isClosed)
            {
                this.segments.Add(new Segment(points[points.Count - 1], points[0]));
            }
            this.IsClosed = isClosed;
        }

        public bool IsIntersecting(Segment segment)
        {
            foreach(Segment seg in segments)
            {
                if (seg.GetIntersection2D(segment) != null)
                {
                    return true;
                }
            }
            return false;
        }
        public Point GetIntersection2D(Segment segment)
        {
            foreach(Segment seg in segments)
            {
                Point result = seg.GetIntersection2D(segment);
                if(result != null)
                {
                    return result;
                }
            }
            return null;
        }
        public List<Point> GetIntersections2D(Segment segment)
        {
            List<Point> result = new List<Point>();
            foreach (Segment seg in segments)
            {
                Point inter = seg.GetIntersection2D(segment);
                if (result != null)
                {
                    result.Add(inter);
                }
            }
            return result;
        }
        public List<Point> GetIntersections2D(Polygon poly)
        {
            List<Point> result = new List<Point>();
            foreach (Segment segment in poly.edges)
            {
                foreach (Segment seg in segments)
                {
                    Point inter = seg.GetIntersection2D(segment);
                    if (inter != null)
                    {
                        result.Add(inter);
                    }
                }
            }
            return result;
        }
        public List<Point> GetIntersections2D(Polyline poly)
        {
            List<Point> result = new List<Point>();
            foreach (Segment segment in poly.segments)
            {
                foreach (Segment seg in segments)
                {
                    Point inter = seg.GetIntersection2D(segment);
                    if (inter != null)
                    {
                        result.Add(inter);
                    }
                }
            }
            return result;
        }
        public List<Point> GetPoints()
        {
            List<Point> result = new List<Point>();
            for(int i = 0; i < this.segments.Count; i ++)
            {
                result.Add(segments[i].points[0]);
            }
            if(!IsClosed)
            result.Add(segments[segments.Count - 1].points[1]);
            return result;
        }

        public List<Segment> GetSegmentsCopy()
        {
            return new List<Segment>(segments);
        }

        public void AddPoint(Point point)
        {
            this.segments.Add(new Segment(this.segments[this.segments.Count - 1].points[1], point));
        }
        public void AddPointAtStart(Point point)
        {
            this.segments.Insert(0, new Segment(point, this.segments[0].points[0]));
        }
        public double GetArea()
        {
            double result = 0d;
            for (int i = 0; i < GetPoints().Count - 1; i++)
            {
                result += MxF.Det(new double[2, 2] { { GetPoints()[i].X, GetPoints()[i].Y }, { GetPoints()[i + 1].X, GetPoints()[i + 1].Y } });
            }
            result += MxF.Det(new double[2, 2] { { GetPoints()[GetPoints().Count - 1].X, GetPoints()[GetPoints().Count - 1].Y }, { GetPoints()[0].X, GetPoints()[0].Y } });
            return Math.Abs(result / 2);
        }
        public Point GetMin()
        {
            double xMin = double.MaxValue;
            double yMin = double.MaxValue;
            double zMin = double.MaxValue;

            foreach (Point point in this.GetPoints())
            {
                if (point.X < xMin)
                {
                    xMin = point.X;
                }
                if (point.Y < yMin)
                {
                    yMin = point.Y;
                }
                if (point.Z < xMin)
                {
                    zMin = point.Z;
                }
            }
            return new Point(xMin, yMin, zMin);
        }
        public Point GetMax()
        {
            double xMax = double.MinValue;
            double yMax = double.MinValue;
            double zMax = double.MinValue;

            foreach (Point point in this.GetPoints())
            {
                if (point.X > xMax)
                {
                    xMax = point.X;
                }
                if (point.Y > yMax)
                {
                    yMax = point.Y;
                }
                if (point.Z > zMax)
                {
                    zMax = point.Z;
                }
            }
            return new Point(xMax, yMax, zMax);
        }
        public Point GetCenter()
        {
            List<Point> points = this.GetPoints();
            double X = 0;
            double Y = 0;
            double Z = 0;
            foreach (Point point in points)
            {
                X += point.X;
                Y += point.Y;
                Z += point.Z;
            }
            return new Point(X / points.Count, Y / points.Count, Z / points.Count);
        }

        public bool IsInside(Point point)
        {
            List<Point> pointsT = GetPoints();


            if (point.IsEqual2D(pointsT[0]))
            {
                return true;
            }
            int winding = 0;
            for (int i = 0; i < pointsT.Count - 1; i++)
            {
                if (BMF.Deq(pointsT[i + 1].Y, point.Y))
                {
                    if (BMF.Deq(pointsT[i + 1].Y, point.X))
                    {
                        return true;
                    }
                    else if (BMF.Deq(pointsT[i].Y, point.Y) && (pointsT[i + 1].X > point.X == pointsT[i].X < point.X))
                    {
                        return true;
                    }
                }
                if (pointsT[i].Y < point.Y != pointsT[i + 1].Y < point.Y)
                {
                    if (pointsT[i].X >= point.X)
                    {
                        if (pointsT[i + 1].X > point.X)
                        {
                            winding = winding + 2 * (pointsT[i + 1].Y > pointsT[i].Y ? 1 : 0) - 1;
                        }
                        else if ((pointsT[i].X - point.X) * (pointsT[i + 1].Y - point.Y) - (pointsT[i + 1].X - point.X) * (pointsT[i].Y - point.Y) > 0 == (pointsT[i + 1].Y > pointsT[i].Y))
                        {
                            winding = winding + 2 * (pointsT[i + 1].Y > pointsT[i].Y ? 1 : 0) - 1;
                        }
                    }
                    else
                    {

                        if ((pointsT[i].X - point.X) * (pointsT[i + 1].Y - point.Y) - (pointsT[i + 1].X - point.X) * (pointsT[i].Y - point.Y) > 0 == (pointsT[i + 1].Y > pointsT[i].Y))
                        {
                            winding = winding + 2 * (pointsT[i + 1].Y > pointsT[i].Y ? 1 : 0) - 1;
                        }

                    }
                }
            }
            return winding != 0;
        }



        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Segment segment in segments)
            {
                builder.Append(segment.ToString() + "\n");
            }
            builder.Append("\n");
            foreach (Point point in GetPoints())
            {
                builder.Append(point.ToString() + "\n");
            }
            return builder.ToString();
        }
    }
}
