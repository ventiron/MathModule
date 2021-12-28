using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Segment : MathObject
    {
        //int precision = 7;

        /// <summary>
        /// Список точек
        /// </summary>
        public List<Point> points;
        int precision = 9;
        double A, B, C;//коэффициенты уравнения прямой вида: Ax+By+C=0
        public Segment(Point p1, Point p2, string id = "0")
        {
            if(p1 == p2)
            {
                throw new AggregateException();
            }
            this.points = new List<Point>(2);
            this.points.Add(p1);
            this.points.Add(p2);
            this.id = id;
        }
        public Segment(List<Point> points, string id = "0")
        {
            this.points = new List<Point>(points);
            this.id = id;
        }
        public Segment(Point point, Vector2D vector, string id = "0")
        {
            this.points = new List<Point>(2);
            this.points.Add(point);
            this.points.Add(new Point(point.X + vector.X, point.Y + vector.Y, point.Z));
            this.id = id;
        }
        public Segment(Point point, Vector3D vector, string id = "0")
        {
            this.points = new List<Point>(2);
            this.points.Add(point);
            this.points.Add(new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z));
            this.id = id;
        }

        /// <summary>
        /// Возвращяет длину отрезка через формулу пифагора
        /// </summary>
        /// <returns></returns>
        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(points[0].X - points[1].X, 2) + Math.Pow(points[0].Y - points[1].Y,2));
        }


        /// <summary>
        /// Находит кратчайшее 2D расстояние от отрезка до заданной точки
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double GetLengthTo2D(Point p)
        {
            Vector2D P1P2 = new Vector2D(this);
            Vector2D P1P = new Vector2D(this.points[0], p);
            Vector2D P2P = new Vector2D(this.points[1], p);

            double p1 = P1P.ScalarMultOrtho(P1P2);
            double p2 = P2P.ScalarMultOrtho(P1P2);

            if(p1 < 0)
            {
                return Math.Sqrt(Math.Pow(P1P.X, 2) + Math.Pow(P1P.Y, 2));
            }
            else if(p2 > 0)
            {
                return Math.Sqrt(Math.Pow(P2P.X, 2) + Math.Pow(P2P.Y, 2));
            }
            else
            {
                double mod = Math.Sqrt(P1P2.ScalarMultOrtho(P1P2));
                return Math.Abs(P1P2.X * P1P.Y - P1P2.Y * P1P.X) / mod;
            }
        }

        /// <summary>
        /// Проверяет содержит ли отрезок заданный Point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsContainingPoint(Point point)
        {
            if (points[0] == point || points[1] == point) return true;
            return false;

        }


        // Определяет точку пересечения двух отрезков
        public Point GetIntersection2D(Point pt1, Point pt2, bool CheckForBorderPoints = true)
        {

            //if (pt1 == null || pt2 == null)
            //{
            //    return null;
            //}
            //// Пункты, чтобы не менять оригиналы.
            //Point p1 = pt1;
            //Point p2 = pt2;
            //Point p3 = this.points[0];
            //Point p4 = this.points[1];
            //double A1;
            //double A2;
            //double b1;
            //double b2;
            //double Xa;
            //double Ya;
            //// Переброска точек, чтобы первые были минимальны
            //if (p1.X > p2.X)
            //{
            //    Point temp = p1;
            //    p1 = p2;
            //    p2 = temp;
            //}
            //if (p3.X > p4.X)
            //{
            //    Point temp = p3;
            //    p3 = p4;
            //    p4 = temp;
            //}
            //// Проверка могут ли отрезки пересекаться в принципе
            //if (p2.X < p3.X || Math.Max(p1.Y, p2.Y) < Math.Min(p3.Y, p4.Y))
            //{
            //    return null;
            //}
            //// Проверка, вертикальны ли оба отрезка
            //if (BMF.Deq(p1.X, p2.X) && BMF.Deq(p3.X, p4.X))
            //{
            //    if (p2.X == p4.X)
            //    {
            //        if (!((Math.Max(p1.Y, p2.Y) < Math.Min(p3.Y, p4.Y)) || (Math.Min(p1.Y, p2.Y) > Math.Max(p3.Y, p4.Y))))
            //        {
            //            return new Point(p1.X, p1.Y, 0);
            //        }
            //    }
            //    return null;
            //}
            //// Проверка, вертикален ли первый отрезок
            //if (BMF.Deq(p1.X, p2.X))
            //{
            //    Xa = p1.X;
            //    A2 = (p3.Y - p4.Y) / (p3.X - p4.X);
            //    b2 = p3.Y - A2 * p3.X;
            //    Math.Round(Ya = A2 * Xa + b2, precision);

            //    if (p3.X <= Xa && p4.X >= Xa && Math.Min(p1.Y, p2.Y) <= Ya && Math.Max(p1.Y, p2.Y) >= Ya)
            //    {
            //        return new Point(Xa, Ya, 0);
            //    }
            //    return null;
            //}
            //// Проверка, вертикален ли второй отрезок
            //if (BMF.Deq(p3.X, p4.X))
            //{
            //    Xa = p3.X;
            //    A1 = (p1.Y - p2.Y) / (p1.X - p2.X);
            //    b1 = p1.Y - A1 * p1.X;
            //    Math.Round(Ya = A1 * Xa + b1, precision);
            //    if (p1.X <= Xa && p2.X >= Xa && Math.Min(p3.Y, p4.Y) <= Ya && Math.Max(p3.Y, p4.Y) >= Ya)
            //    {
            //        return new Point(Xa, Ya, 0);
            //    }
            //    return null;
            //}
            //A1 = (p1.Y - p2.Y) / (p1.X - p2.X);
            //A2 = (p3.Y - p4.Y) / (p3.X - p4.X);
            //b1 = p1.Y - A1 * p1.X;
            //b2 = p3.Y - A2 * p3.X;
            //// Проверка на параллельность
            //if (BMF.Deq(A1, A2))
            //{
            //    double A3 = (p1.Y - p3.Y) / (p1.X - p3.X);
            //    if (BMF.Deq(A1, A3))
            //    {
            //        return new Point(p3.X, p3.Y, p3.Z);
            //    }
            //    return null;
            //}

            //Xa = Math.Round((b2 - b1) / (A1 - A2), precision);
            //Ya = Math.Round(A1 * Xa + b1, precision);
            //if ((Xa < Math.Max(p1.X, p3.X)) || (Xa > Math.Min(p2.X, p4.X)))
            //{
            //    return null;
            //}
            //if (!CheckForBorderPoints && this.IsEqual2D(new Point(Xa, Ya, 0)))
            //{
            //    return null;
            //}

            //return new Point(Xa, Ya, 0);








            Segment seg = new Segment(pt1, pt2);
            double dist1 = this.GetLengthTo2D(pt1);
            double dist2 = this.GetLengthTo2D(pt2);
            double dist3 = seg.GetLengthTo2D(this.points[0]);
            double dist4 = seg.GetLengthTo2D(this.points[1]);
            double dist = Math.Min(Math.Min(dist1, dist2), Math.Min(dist3, dist4));
            if (dist < BMF.epsilon)
            {
                if (BMF.Deq(dist1, dist))
                {
                    return pt1;
                }
                if (BMF.Deq(dist2, dist))
                {
                    return pt2;
                }
                if (BMF.Deq(dist3, dist))
                {
                    return this.points[0];
                }
                if (BMF.Deq(dist4, dist))
                {
                    return this.points[1];
                }
            }
            if (pt1 == null || pt2 == null)
            {
                return null;
            }
            // Пункты, чтобы не менять оригиналы.
            Point p1 = pt1;
            Point p2 = pt2;
            Point p3 = this.points[0];
            Point p4 = this.points[1];
            double A1;
            double A2;
            double b1;
            double b2;
            double Xa;
            double Ya;


            // Переброска точек, чтобы первые были минимальны
            if (p1.X > p2.X)
            {
                Point temp = p1;
                p1 = p2;
                p2 = temp;
            }
            if (p3.X > p4.X)
            {
                Point temp = p3;
                p3 = p4;
                p4 = temp;
            }
            // Проверка могут ли отрезки пересекаться в принципе
            if (p2.X < p3.X || Math.Max(p1.Y, p2.Y) < Math.Min(p3.Y, p4.Y))
            {
                return null;
            }
            // Проверка, вертикальны ли оба отрезка
            if (BMF.Deq(p1.X, p2.X) && BMF.Deq(p3.X, p4.X))
            {
                if (p2.X == p4.X)
                {
                    if (!((Math.Max(p1.Y, p2.Y) < Math.Min(p3.Y, p4.Y)) || (Math.Min(p1.Y, p2.Y) > Math.Max(p3.Y, p4.Y))))
                    {
                        return new Point(p1.X, p1.Y, 0);
                    }
                }
                return null;
            }
            // Проверка, вертикален ли первый отрезок
            if (BMF.Deq(p1.X, p2.X))
            {
                Xa = p1.X;
                A2 = (p3.Y - p4.Y) / (p3.X - p4.X);
                b2 = p3.Y - A2 * p3.X;
                Ya = A2 * Xa + b2;

                if (p3.X <= Xa && p4.X >= Xa && Math.Min(p1.Y, p2.Y) <= Ya && Math.Max(p1.Y, p2.Y) >= Ya)
                {
                    return new Point(Xa, Ya, 0);
                }
                return null;
            }
            // Проверка, вертикален ли второй отрезок
            if (BMF.Deq(p3.X, p4.X))
            {
                Xa = p3.X;
                A1 = (p1.Y - p2.Y) / (p1.X - p2.X);
                b1 = p1.Y - A1 * p1.X;
                Ya = A1 * Xa + b1;
                if (p1.X <= Xa && p2.X >= Xa && Math.Min(p3.Y, p4.Y) <= Ya && Math.Max(p3.Y, p4.Y) >= Ya)
                {
                    return new Point(Xa, Ya, 0);
                }
                return null;
            }
            A1 = (p1.Y - p2.Y) / (p1.X - p2.X);
            A2 = (p3.Y - p4.Y) / (p3.X - p4.X);
            b1 = p1.Y - A1 * p1.X;
            b2 = p3.Y - A2 * p3.X;
            // Проверка на параллельность
            if (BMF.Deq(A1, A2))
            {
                double A3 = (p1.Y - p3.Y) / (p1.X - p3.X);
                if (BMF.Deq(A1, A3))
                {
                    return new Point(p3.X, p3.Y, p3.Z);
                }
                return null;
            }

            Xa = (b2 - b1) / (A1 - A2);
            Ya = A1 * Xa + b1;
            if ((Xa < Math.Max(p1.X, p3.X)) || (Xa > Math.Min(p2.X, p4.X)))
            {
                return null;
            }
            if (this.IsEqual2D(new Point(Xa, Ya, 0)) && !CheckForBorderPoints)
            {
                return null;
            }

            return new Point(Xa, Ya, 0);
        }
        public Point GetIntersection2D(Segment edge, bool CheckForBorderPoints = true)
        {
            Point inter = GetIntersection2D(edge.points[0], edge.points[1]);
            if (inter != null && !CheckForBorderPoints)
            {
                if (this.IsEqual2D(inter) || edge.IsEqual2D(inter))
                {
                    return null;
                }
            }
            return inter;
        }

        public Point GetProjection2D(Point point, bool limited = false)
        {

            Vector2D vec2D1 = new Vector2D(this);
            Vector2D vec2D2 = new Vector2D(this.points[0], point);

            double mult = vec2D1.ScalarMultOrtho(vec2D2);

            double selfMult = vec2D1.ScalarMultOrtho(vec2D1);

            double factor = mult / selfMult;
            Point result = new Point(this.points[0].X + vec2D1.X * factor, this.points[0].Y + vec2D1.Y * factor, 0);
            if (limited)
            {
                if (!this.isPointBelongToSegment(result))
                {
                    return null;
                }
            }
            return result;
        }




        /// <summary>
        /// Находит точку пересечения двух отрезков в пространстве. Работает на магии аналитической геометрии.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="firstLine"></param>
        /// <returns></returns>
        public Point GetClosestPoint3D(Segment edge, bool firstLine = true)
        {
            Point p1 = this.points[0];
            Point p2 = this.points[1];
            Point p3 = edge.points[0];
            Point p4 = edge.points[1];

            Vector3D v12 = new Vector3D(p1, p2);
            Vector3D v34 = new Vector3D(p3, p4);


            Vector3D v21 = new Vector3D(p2, p1);
            Vector3D v13 = new Vector3D(p1, p3);
            Vector3D v43 = new Vector3D(p4, p3);

            double d1343 = v13.ScalarMultOrtho(v43);
            double d4321 = v43.ScalarMultOrtho(v21);
            double d1321 = v13.ScalarMultOrtho(v21);
            double d4343 = v43.ScalarMultOrtho(v43);
            double d2121 = v21.ScalarMultOrtho(v21);

            double modA = (d1343 * d4321 - d1321 * d4343) / (d2121 * d4343 - d4321 * d4321);
            double modB = (d1343 + modA * d4321) / d4343;


            if (firstLine)
            {
                return new Point(p1.X + v12.X * modA, p1.Y + v12.Y * modA, p1.Z + v12.Z * modA);
            }
            else
            {
                return new Point(p3.X + v34.X * modB, p3.Y + v34.Y * modB, p3.Z + v34.Z * modB);
            }
        }


        /// <summary>
        /// Находит точку пересечения двух отрезков в пространстве. Работает на магии аналитической геометрии.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="firstLine"></param>
        /// <returns></returns>
        public Point GetIntersection3D(Segment edge, bool firstLine = true)
        {
            Point p1 = this.points[0];
            Point p2 = this.points[1];
            Point p3 = edge.points[0];
            Point p4 = edge.points[1];

            Vector3D v12 = new Vector3D(p1, p2);
            Vector3D v34 = new Vector3D(p3, p4);


            Vector3D v21 = new Vector3D(p2, p1);
            Vector3D v13 = new Vector3D(p1, p3);
            Vector3D v43 = new Vector3D(p4, p3);

            double d1343 = v13.ScalarMultOrtho(v43);
            double d4321 = v43.ScalarMultOrtho(v21);
            double d1321 = v13.ScalarMultOrtho(v21);
            double d4343 = v43.ScalarMultOrtho(v43);
            double d2121 = v21.ScalarMultOrtho(v21);

            double modA = (d1343 * d4321 - d1321 * d4343) / (d2121 * d4343 - d4321 * d4321);
            double modB = (d1343 + modA * d4321) / d4343;


            if (firstLine)
            {
                return new Point(p1.X + v12.X * modA, p1.Y + v12.Y * modA, p1.Z + v12.Z * modA);
            }
            else
            {
                return new Point(p3.X + v34.X * modA, p2.Y + v34.Y * modA, p3.Z + v34.Z * modB);
            }
        }
        /// <summary>
        /// Проверяет эквивалентены ли планарно отрезки хотя бы по 1 точке
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool IsEqual2D(Segment edge)
        {
            if ((this.points[0].IsEqual2D(edge.points[0]) && this.points[1].IsEqual2D(edge.points[1])) || (this.points[1].IsEqual2D(edge.points[0]) && this.points[0].IsEqual2D(edge.points[1]))) return true;
            return false;
        }
        /// <summary>
        /// Проверяет эквивалентна ли планарно заданной точке отдна из точек отрезка
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool IsEqual2D(Point point)
        {
            if (this.points[0].IsEqual2D(point) || this.points[1].IsEqual2D(point)) return true;
            return false;
        }

        /// <summary>
        /// Проверяет эквивалентены ли планарно заданные точки хотя бы 1 точке отрезка
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool IsEqual2D(Point point1, Point point2)
        {
            return (this.points[0].IsEqual2D(point1) && this.points[1].IsEqual2D(point2)) || (this.points[0].IsEqual2D(point2) && this.points[1].IsEqual2D(point1));
        }


        /// <summary>
        /// Проверяет равны ли высоты точек отрезка заданной высоте
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool IsPointsHeightEqual(double height)
        {
            if (BMF.Deq(this.points[0].Z, height) && BMF.Deq(this.points[1].Z, height))
            {
                return true;
            }
            return false;
        }

        public Point GetOtherPoint(Point point)
        {
            return this.points[0] == point ? this.points[1] : this.points[0];
        }

        public bool isPointBelongToSegment(Point point)
        {
            //return !(point.X < Math.Min(this.points[0].X, this.points[1].X) || point.Y < Math.Min(this.points[0].Y, this.points[1].Y) || point.X > Math.Max(this.points[0].X, this.points[1].X) || point.Y > Math.Max(this.points[0].Y, this.points[1].Y))
            //    && IsCollinear(point);
            return BMF.Deq(this.GetLengthTo2D(point), 0d);
        }

        /// <summary>
        /// Returns string equivalent of Object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"TriangulationEdge ID: {this.id}\n    p1: {points[0]}    p2: {points[1]}";
        }

        /// <summary>
        /// Определяют коллинеарны ли точки отрезка с передаваемой точкой.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsCollinear(Point p)
        {
            return BMF.Deq((p.X - points[0].X) * (points[1].Y - points[0].Y) - (points[1].X - points[0].X) * (p.Y - points[0].Y), 0);
        }
        /// <summary>
        /// Возвращает середину отрезка.
        /// </summary>
        /// <returns></returns>
        public Point GetMiddlePoint()
        {
            return new Point(points[0].X + (points[1].X - points[0].X) / 2, points[0].Y + (points[1].Y - points[0].Y) / 2, points[0].Z + (points[1].Z - points[0].Z) / 2);
        }

        public static Segment GetLineOnY2D(double x, double dist = double.MaxValue - 1E200)
        {
            return new Segment(new Point(x, dist, 0), new Point(x, -dist, 0));
        }
        public static Segment GetLineOnX2D(double y, double dist = double.MaxValue - 1E200)
        {
            return new Segment(new Point(dist, y, 0), new Point(-dist, y, 0));
        }
        public static Segment GetLineByAngle2D(double x, double y, double ang, double dist = double.MaxValue - 1E200, bool mirror = false)
        {
            if (!mirror)
                return new Segment(new Point(x + dist * Math.Cos(ang), y + dist * Math.Sin(ang), 0), new Point(x - dist * Math.Cos(ang), y - dist * Math.Sin(ang), 0));
            else
                return new Segment(new Point(x + dist * Math.Cos(ang), y - dist * Math.Sin(ang), 0), new Point(x - dist * Math.Cos(ang), y + dist * Math.Sin(ang), 0));
        }






















    }
}
