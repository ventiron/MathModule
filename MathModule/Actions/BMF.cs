using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class BMF
    {
        public static double epsilon = 1E-7;

        public static bool Deq(double d1, double d2)
        {
            if (d1 == d2)
            {
                return true;
            }
            return Math.Abs(d1 - d2) <= epsilon;
        }
        public static bool Deq(double d1, double d2, double epsilon)
        {
            if (d1 == d2)
            {
                return true;
            }
            return Math.Abs(d1 - d2) <= epsilon;
        }
        public static List<Point> Interpol(TriangulationEdge edge, double dH)
        {
            List<Point> result = new List<Point>();
            Point p1 = edge.points[0];
            Point p2 = edge.points[1];
            double height1 = p1.Z;
            double height2 = p2.Z;
            double dheight = Math.Abs(height1 - height2);
            double diff = height1 % dH;
            double length = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            double ang = AngleByCoordinates(p1, p2);
            double tan = (p1.Z - p2.Z) / length;
            double x;
            double y;
            double z;
            double dist = dH / dheight * length;
            // Придумать название переменной, или использовать усложненные формулы.
            // Проверка на присутствие горизнтали между точками
            if (dheight < dH)
            {
                if (height2 - (height1 - diff) < dH)
                {
                    return null;
                }
            }
            if (Deq(diff, 0d))
            {
                diff = dH;
            }
            else
            {

                diff = height2 < height1 ? diff : dH - diff;
            }
            // Основной цикл отрисовки. Требуется улучшить воспринимаемость.
            for (double i = dist * diff / dH; Math.Round(i, 4) < length; i += dist)
            {
                // "Вверх"
                if (Deq(p1.X, p2.X) && p2.Y - p1.Y > 0)
                {
                    x = p1.X;
                    y = p1.Y + i;
                    z = p1.Z + i * tan;
                }
                // "Вниз"
                else if (Deq(p1.X, p2.X) && p2.Y - p1.Y < 0)
                {
                    x = p1.X;
                    y = p1.Y - i;
                    z = p1.Z + i * tan;
                }
                // "Вправо"
                else if (Deq(p1.Y, p2.Y) && p2.X - p1.X > 0)
                {
                    x = p1.X + i;
                    y = p1.Y;
                    z = p1.Z + i * tan;
                }
                // "Влево"
                else if (Deq(p1.Y , p2.Y) && p2.X - p1.X < 0)
                {
                    x = p1.X - i;
                    y = p1.Y;
                    z = p1.Z + i * tan;
                }
                else
                {
                    x = p1.X + Math.Cos(ang) * i;
                    y = p1.Y + Math.Sin(ang) * i;
                    z = p1.Z + i * tan;
                }
                result.Add(new Point(x, y, z));
            }
            return result;
        }

        // При интерполировании считается, что Х - вертикально направленная ось
        public static Point InterpolToPoint(TriangulationEdge edge, double PointHeight)
        {
            // Точки.
            Point p1 = edge.points[0];
            Point p2 = edge.points[1];
            // Высоты точек.
            double height1 = p1.Z;
            double height2 = p2.Z;
            // Превышение между точками.
            double dheight = Math.Abs(height1 - height2);
            // Превышение между первой и искомой точкой.
            double diff = height1 > height2 ? Math.Abs(height1 - (PointHeight % height1))   :  PointHeight - (height1 % PointHeight);
            double length = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            double ang = AngleByCoordinates_Clockwise(p1, p2);
            double tan = (height2 - height1) / length;
            double x;
            double y;
            double z;
            double dist = diff / dheight * length;
            // Придумать название переменной, или использовать усложненные формулы.
            // Проверка на присутствие горизнтали между точками
            if(edge.points[0].Z > PointHeight && edge.points[1].Z > PointHeight || edge.points[0].Z < PointHeight && edge.points[1].Z < PointHeight)
            {
                return null;
            }
            // "Вверх"
            if (Deq(p1.X, p2.X) && p2.Y - p1.Y > 0d)
            {
                x = p1.X;
                y = p1.Y + dist;
                z = p1.Z + dist * tan;
            }
            // "Вниз"
            else if (Deq(p1.X, p2.X) && p2.Y - p1.Y < 0d)
            {
                x = p1.X;
                y = p1.Y - dist;
                z = p1.Z + dist * tan;
            }
            // "Вправо"
            else if (Deq(p1.Y, p2.Y) && p2.X - p1.X > 0d)
            {
                x = p1.X + dist;
                y = p1.Y;
                z = p1.Z + dist * tan;
            }
            // "Влево"
            else if (Deq(p1.Y, p2.Y) && p2.X - p1.X < 0d)
            {
                x = p1.X - dist;
                y = p1.Y;
                z = p1.Z + dist * tan;
            }
            else
            {
                x = p1.X + Math.Cos(ang) * dist;
                y = p1.Y + Math.Sin(ang) * dist;
                z = p1.Z + dist * tan;
            }
            return new Point(x, y, z);
        }


        // Служебно-математические функции, нужны для вычислений.
        public static double[] CoordinatesByAngle(Point origin, double angle, double distance)
        {
            double[] resultXY = new double[2];

            switch (angle)
            {
                case 0d:
                    resultXY[0] = origin.X;
                    resultXY[1] = origin.Y + distance;
                    return resultXY;
                case 90d / 180d * Math.PI:
                    resultXY[0] = origin.X + distance;
                    resultXY[1] = origin.Y;
                    return resultXY;
                case 180d / 180d * Math.PI:
                    resultXY[0] = origin.X;
                    resultXY[1] = origin.Y - distance;
                    return resultXY;
                case 270d / 180d * Math.PI:
                    resultXY[0] = origin.X - distance;
                    resultXY[1] = origin.Y;
                    return resultXY;
            }
            resultXY[0] = origin.X + distance * Math.Cos(angle);
            resultXY[1] = origin.Y + distance * Math.Sin(angle);
            return resultXY;
        }

        /// <summary>
        ///  Крайне полезная и, в будущем, скорее всего, основная функция расчета углов из координат. Работает, не трогай. На вход требует
        /// 2 Point и выдаёт дирекционный угол в виде double вторая и четвёртая четверть перевёрнуты из-за того, что в автокаде Y идёт в обратную сторону от прямоугольной 
        /// системы координат.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double AngleByCoordinates_Clockwise(Point p1, Point p2)
        {
            double dX = p2.X - p1.X;
            double dY = p2.Y - p1.Y;
            if (Deq(dX, 0d) && Deq(dY, 0d))
            {
                return 0;
            }
            if (Deq(dX, 0d) && dY > 0d)
            {
                return Math.PI / 2d;
            }
            if (Deq(dX, 0d) && dY < 0d)
            {
                return Math.PI * 1.5d;
            }
            if (dX > 0 && Deq(dY, 0d))
            {
                return 0d;
            }
            if (dX < 0 && Deq(dY, 0d))
            {
                return Math.PI;
            }
            double rum = Math.Atan(Math.Abs(dY / dX));
            double ang = 0d;
            // Первая четверть
            if (dX > 0d && dY > 0d)
            {
                ang = rum;
            }
            // Вторая четверть
            else if (dX < 0d && dY > 0d)
            {
                ang =  Math.PI - rum;
            }
            //Третья четверть
            else if (dX < 0d && dY < 0d)
            {
                ang = Math.PI + rum;
            }
            //Четвертая четверть
            else if (dX > 0d && dY < 0d)
            {

                ang = 2d * Math.PI - rum;
            }
            return ang;
        }



        /// <summary>
        ///  Крайне полезная и, в будущем, скорее всего, основная функция расчета углов из координат. Работает, не трогай. На вход требует
        /// 2 Point и выдаёт дирекционный угол в виде double вторая и четвёртая четверть перевёрнуты из-за того, что в автокаде Y идёт в обратную сторону от прямоугольной 
        /// системы координат.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double AngleByCoordinates(Point p1, Point p2)
        {
            double dX = p2.X - p1.X;
            double dY = p2.Y - p1.Y;
            if(Deq(dX,0d) && Deq(dY, 0d))
            {
                return 0d;
            }
            if (Deq(dX, 0d) && dY > 0d)
            {
                return Math.PI + Math.PI / 2d;
            }
            if (Deq(dX, 0d) && dY < 0d)
            {
                return Math.PI / 2d;
            }
            if(dX > 0 && Deq(dY, 0d))
            {
                return 0d;
            }
            if (dX < 0 && Deq(dY, 0d))
            {
                return Math.PI;
            }
            double rum = Math.Atan(Math.Abs(dY / dX));
            double ang = 0d;
            // Первая четверть (4)
            if (dX > 0d && dY > 0d)
            {
                ang = 2d * Math.PI - rum;
            }
            // Вторая четверть (3)
            else if (dX < 0d && dY > 0d)
            {
                ang = Math.PI + rum;

            }
            //Третья четверть (2)
            else if (dX < 0d && dY < 0d)
            {
                ang = Math.PI - rum;
            }
            //Четвертая четверть (1)
            else if (dX > 0d && dY < 0d)
            {
                ang = rum;
            }
            return ang;
        }


        /// <summary>
        /// Рассчитывает расстояние между двумя точками в 2D
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistance2D(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p1.X - p2.X),2) + Math.Pow((p1.Y - p2.Y),2));
        }


        /// <summary>
        /// Находит по какую сторону от отрезка находится точка
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static double SideRelativelyOfSegment(Segment segment, Point point)
        {
            return (segment.points[0].X - point.X) * (segment.points[1].Y - point.Y) - (segment.points[1].X - point.X) * (segment.points[0].Y - point.Y);
        }
        public static double DistanceBetweenPointAndPlane(Point point, double[] planeFactors)
        {
            return (point.X * planeFactors[0] + point.Y * planeFactors[1] + point.Z * planeFactors[2] + planeFactors[3]) /
                Math.Sqrt(Math.Pow(planeFactors[0], 2) + Math.Pow(planeFactors[1], 2) + Math.Pow(planeFactors[2], 2));
        }

        public static Point GetCenter(List<Point> points)
        {
            double x = 0;
            double y = 0;
            double z = 0;
            foreach(Point point in points)
            {
                x += point.X;
                y += point.Y;
                z += point.Z;
            }
            return new Point(x / points.Count, y / points.Count, z / points.Count);
        }


        /// <summary>
        /// Возвращает точку, с координатами наименьшими из всего набора точек
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetMin(List<Point> points)
        {
            double xMin = double.MaxValue;
            double yMin = double.MaxValue;
            double zMin = double.MaxValue;

            foreach (Point point in points)
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
        /// <summary>
        /// Возвращает точку, с координатами наибольшими из всего набора точек
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetMax(List<Point> points)
        {
            double xMax = double.MinValue;
            double yMax = double.MinValue;
            double zMax = double.MinValue;

            foreach (Point point in points)
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
        public static void SortPointsClockwise(List<Point> points)
        {
            Point center = BMF.GetCenter(points);
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (BMF.CompatePointsByAngle(center, points[i], points[j]))
                    {
                        Point temp = points[i];
                        points[i] = points[j];
                        points[j] = temp;
                    }
                }
            }
        }
        public static bool CompatePointsByAngle(Point basis, Point p1, Point p2)
        {
            double ang1 = BMF.AngleByCoordinates_Clockwise(basis, p1);
            double ang2 = BMF.AngleByCoordinates_Clockwise(basis, p2);
            if (ang1 > ang2)
            {
                return true;
            }
            else if (ang1 < ang2)
            {
                return false;
            }
            else
            {
                return basis.GetDistance2D(p1) < basis.GetDistance2D(p2);
            }
        }
    }
}
