using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Polygon : MathObject
    {
        public List<Segment> edges { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="onlyCopy"></param>
        public Polygon(List<Segment> edges, bool onlyCopy = true)
        {
            this.edges = new List<Segment>();
            if (onlyCopy)
            {
                this.edges.AddRange(edges);
            }
            else
            {
                this.edges = edges;
            }
            if (!IsClosed())
            {
                throw new ArgumentOutOfRangeException("edges", "Unapropriate edges");
            }
        }
        public Polygon(List<Point> points)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("Невозможно построить полилинию из одной точки.", "points");
            }
            edges = new List<Segment>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                this.edges.Add(new Segment(points[i], points[i + 1]));
            }
            edges.Add(new Segment(points[points.Count - 1], points[0]));
        }

        public Polygon()
        {

        }

        /// <summary>
        /// Проверяет находится ли точка внутри полигона. На момент написания ты довольно слабо понимаешь как это работает (особенно часть с якобы детерминантом)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsInside(Point point)
        {
            List<Point> pointsT = new List<Point>();
            pointsT.AddRange(GetPointsByOrder());
            pointsT.Add(GetPoints()[0]);


            if (point.IsEqual2D(pointsT[0]))
            {
                return true;
            }
            int winding = 0;
            for(int i = 0; i < pointsT.Count - 1; i++)
            {
                if(BMF.Deq(pointsT[i + 1].Y, point.Y))
                {
                    if(BMF.Deq(pointsT[i + 1].Y, point.X))
                    {
                        return true;
                    }
                    else if(BMF.Deq(pointsT[i].Y,point.Y) && (pointsT[i + 1].X > point.X == pointsT[i].X < point.X))
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

                        if ((pointsT[i].X - point.X) * (pointsT[i+1].Y - point.Y) - (pointsT[i + 1].X - point.X) * (pointsT[i].Y - point.Y) > 0 == (pointsT[i + 1].Y > pointsT[i].Y))
                        {
                            winding = winding + 2 * (pointsT[i + 1].Y > pointsT[i].Y ? 1 : 0) - 1;
                        }

                    }
                }
            }
            return winding != 0;
        }

        /// <summary>
        /// Вовращает расстояние от точки до полигона
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public decimal GetDistance2DToPoint(Point point)
        {
            decimal result = edges[0].GetLengthTo2D(point);
            foreach (Segment segment in edges)
            {
                decimal dist = segment.GetLengthTo2D(point);
                if (dist < result)
                {
                    result = dist;
                }
            }
            return result;
        }

        /// <summary>
        /// Вовращает расстояние от точки до полигона
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Segment GetClosestSegment(Point point)
        {
            decimal count = edges[0].GetLengthTo2D(point);
            Segment result = null;
            foreach (Segment segment in edges)
            {
                decimal dist = segment.GetLengthTo2D(point);
                if (dist < count)
                {
                    result = segment;
                }
            }
            return result;
        }


        /// <summary>
        /// Возвращает площадь полигона
        /// </summary>
        /// <returns></returns>
        public decimal GetArea()
        {
            decimal result = 0m;
            for(int i = 0; i < GetPoints().Count - 1; i++)
            {
                result += MxF.Det(new decimal[2, 2] { { GetPoints()[i].X, GetPoints()[i].Y }, { GetPoints()[i+1].X, GetPoints()[i+1].Y } });
            }
            result += MxF.Det(new decimal[2, 2] { { GetPoints()[GetPoints().Count - 1].X, GetPoints()[GetPoints().Count - 1].Y }, { GetPoints()[0].X, GetPoints()[0].Y } });
            return result / 2;
        }


        /// <summary>
        /// Возвращает периметр полигона
        /// </summary>
        /// <returns></returns>
        public decimal GetPerimeter()
        {
            decimal result = 0;
            foreach (Segment edge in edges)
            {
                result += edge.GetLength();
            }
            return result;
        }


        /// <summary>
        /// Возвращает барицентр полигона
        /// </summary>
        /// <returns></returns>
        public Point GetCenter()
        {
            List<Point> points = this.GetPoints();
            decimal X = 0;
            decimal Y = 0;
            decimal Z = 0;
            foreach (Point point in points)
            {
                X += point.X;
                Y += point.Y;
                Z += point.Z;
            }
            return new Point(X / points.Count, Y / points.Count, Z / points.Count);
        }



        /// <summary>
        /// Возвращает список точек в порядке соединения
        /// </summary>
        /// <returns></returns>
        public List<Point> GetPointsByOrder()
        {
            List<Point> result = new List<Point>();
            Point StartPoint = edges[0].points[0];
            Point CurrentPoint = edges[0].points[1];
            List<Segment> usedEdges = new List<Segment>();
            usedEdges.Add(edges[0]);
            result.Add(edges[0].points[0]);
            result.Add(edges[0].points[1]);

            while (usedEdges.Count < edges.Count)
            {
                bool end = true;
                foreach (Segment segment in edges)
                {
                    if (usedEdges.Contains(segment))
                    {
                        continue;
                    }
                    if (segment.IsContainingPoint(CurrentPoint))
                    {
                        CurrentPoint = segment.GetOtherPoint(CurrentPoint);
                        if(StartPoint == CurrentPoint)
                        {
                            return result;
                        }
                        result.Add(CurrentPoint);
                        usedEdges.Add(segment);
                        end = false;
                        break;
                    }
                }
                if (end) throw new AggregateException("polygon are unapropriante");
            }

            return result;
        }
        public List<Point> GetPoints()
        {
            List<Point> result = new List<Point>();
            foreach(Segment seg in edges)
            {
                if (!result.Contains(seg.points[0]))
                {
                    result.Add(seg.points[0]);
                }
                if (!result.Contains(seg.points[1]))
                {
                    result.Add(seg.points[1]);
                }
            }
            return result;
        }




        /// <summary>
        /// Проверяет закрыт ли полигон
        /// </summary>
        /// <returns></returns>
        public bool IsClosed()
        {
            int count = 0;
            foreach(Segment segment in edges)
            {
                if (segment.IsContainingPoint(edges[0].points[0]))
                {
                    count++;
                }
            }
            if(count != 2)
            {
                return false;
            }
            Point StartPoint = edges[0].points[0];
            Point CurrentPoint = edges[0].points[1];
            List<Segment> usedEdges = new List<Segment>();
            usedEdges.Add(edges[0]);

            while(usedEdges.Count < edges.Count)
            {
                bool end = true;
                foreach(Segment segment in edges)
                {
                    if (usedEdges.Contains(segment))
                    {
                        continue;
                    }
                    if (segment.IsContainingPoint(CurrentPoint))
                    {
                        CurrentPoint = segment.GetOtherPoint(CurrentPoint);
                        usedEdges.Add(segment);
                        end = false;
                        break;
                    }
                }
                if (end) return false;
            }
            return true;
        }
        public void UpdatePoints()
        {
            GetPoints().Clear();
            GetPoints().AddRange(this.GetPointsByOrder());
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(Segment segment in edges)
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
        public Point GetMin()
        {
            decimal xMin = decimal.MaxValue;
            decimal yMin = decimal.MaxValue;
            decimal zMin = decimal.MaxValue;

            foreach (Point point in this.GetPoints())
            {
                if(point.X < xMin)
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
            decimal xMax = decimal.MinValue;
            decimal yMax = decimal.MinValue;
            decimal zMax = decimal.MinValue;

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
    }
}
