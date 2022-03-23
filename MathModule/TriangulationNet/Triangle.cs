using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathModule
{
    public class Triangle : MathObject
    {
        public new TriangulationEdge[] edges = new TriangulationEdge[3];
        private Point[] points;

        // Конструкторы класса
        public Triangle(TriangulationEdge edge1, TriangulationEdge edge2, TriangulationEdge edge3)
        {
            if (edge1 == null || edge2 == null || edge3 == null)
            {
                throw new ArgumentNullException(edge1 == null ? "edge1": edge2 == null ? "edge2" : "edge3", "One of the edges was null");
            }
            edges[0] = edge1;
            this.InsetrTriangleInEdge(edge1);
            edges[1] = edge2;
            this.InsetrTriangleInEdge(edge2);
            edges[2] = edge3;
            this.InsetrTriangleInEdge(edge3);
        }
        public Triangle(TriangulationEdge edge1, TriangulationEdge edge2, TriangulationEdge edge3, Triangle tr, bool isOnlyFirstAreOld = false)
        {
            if (edge1 == null || edge2 == null || edge3 == null)
            {
                throw new ArgumentNullException(edge1 == null ? "edge1" : edge2 == null ? "edge2" : "edge3", "One of the edges was null");
            }
            if (!isOnlyFirstAreOld)
            {
                edges[0] = edge1;
                this.InsetrTriangleInEdge(edge1, tr);
                edges[1] = edge2;
                this.InsetrTriangleInEdge(edge2, tr);
                edges[2] = edge3;
                this.InsetrTriangleInEdge(edge3, tr);
            }
            else
            {
                edges[0] = edge1;
                this.InsetrTriangleInEdge(edge1, tr);
                edges[1] = edge2;
                this.InsetrTriangleInEdge(edge2);
                edges[2] = edge3;
                this.InsetrTriangleInEdge(edge3);
            }
        }
        public Triangle(TriangulationEdge edge1, TriangulationEdge edge2, TriangulationEdge edge3, Triangle tr1, Triangle tr2, bool isFirstFullNew = false)
        {
            if (edge1 == null || edge2 == null || edge3 == null)
            {
                throw new ArgumentNullException(edge1 == null ? "edge1" : edge2 == null ? "edge2" : "edge3", "One of the edges was null");
            }
            edges[0] = edge1;
            if (!isFirstFullNew)
            {
                this.InsetrTriangleInEdge(edge1, tr1);
                this.InsetrTriangleInEdge(edge1, tr2);
            }
            else
            {
                this.InsetrTriangleInEdge(edge1);
            }
            edges[1] = edge2;
            this.InsetrTriangleInEdge(edge2, tr1);
            this.InsetrTriangleInEdge(edge2, tr2);
            edges[2] = edge3;
            this.InsetrTriangleInEdge(edge3, tr1);
            this.InsetrTriangleInEdge(edge3, tr2);
        }
        public void PseudoConstruct(TriangulationEdge edge1, TriangulationEdge edge2, TriangulationEdge edge3)
        {
            edges[0] = edge1;
            this.InsetrTriangleInEdge(edge1);
            edges[1] = edge2;
            this.InsetrTriangleInEdge(edge2);
            edges[2] = edge3;
            this.InsetrTriangleInEdge(edge3);
        }
        public void PseudoConstruct(TriangulationEdge edge1, TriangulationEdge edge2, TriangulationEdge edge3, Triangle tr, bool isOnlyFirstAreOld = false)
        {
            if (edge1 == null || edge2 == null || edge3 == null)
            {
                throw new ArgumentNullException(edge1 == null ? "edge1" : edge2 == null ? "edge2" : "edge3", "One of the edges was null");
            }
            this.edges = new TriangulationEdge[3];
            if (!isOnlyFirstAreOld)
            {
                edges[0] = edge1;
                this.InsetrTriangleInEdge(edge1, tr);
                edges[1] = edge2;
                this.InsetrTriangleInEdge(edge2, tr);
                edges[2] = edge3;
                this.InsetrTriangleInEdge(edge3, tr);
            }
            else
            {
                edges[0] = edge1;
                this.InsetrTriangleInEdge(edge1, tr);
                edges[1] = edge2;
                this.InsetrTriangleInEdge(edge2);
                edges[2] = edge3;
                this.InsetrTriangleInEdge(edge3);
            }
        }
        public void PseudoConstruct(TriangulationEdge edge1, TriangulationEdge edge2, TriangulationEdge edge3, Triangle tr1, Triangle tr2, bool isFirstFullNew = false)
        {
            if (edge1 == null || edge2 == null || edge3 == null)
            {
                MessageBox.Show("Какой-то Пиздец");
            }
            edges[0] = edge1;
            if (!isFirstFullNew)
            {
                this.InsetrTriangleInEdge(edge1, tr1);
                this.InsetrTriangleInEdge(edge1, tr2);
            }
            else
            {
                this.InsetrTriangleInEdge(edge1);
            }
            edges[1] = edge2;
            this.InsetrTriangleInEdge(edge2, tr1);
            this.InsetrTriangleInEdge(edge2, tr2);
            edges[2] = edge3;
            this.InsetrTriangleInEdge(edge3, tr1);
            this.InsetrTriangleInEdge(edge3, tr2);
        }
        public override string ToString()
        {
            return $"Triangle ID: {this.id} \nTriangulationEdge №1: {edges[0]}TriangulationEdge №2: {edges[1]}TriangulationEdge №3: {edges[2]}Centroid: {GetCentroid()}";
        }
        public TriangulationEdge GetOtherEdge(TriangulationEdge otherEdge1, TriangulationEdge otherEdge2)
        {
            foreach(TriangulationEdge edge in edges)
            {
                if(edge != otherEdge1 && edge != otherEdge2)
                {
                    return edge;
                }
            }
            return edges[0];
        }
        // Возвращает массив размерности 2 из рёбер имеющих заданную точку
        public TriangulationEdge[] GetEdgesByPoint(Point point)
        {
            TriangulationEdge[] result = new TriangulationEdge[2];
            foreach(TriangulationEdge edge in edges)
            {
                if (edge.IsEqual2D(point))
                {
                    if(result[0] == null)
                    {
                        result[0] = edge;
                    }
                    else
                    {
                        result[1] = edge;
                    }
                }
            }
            return result;
        }
        // Возвращает массив размерности 3 из точек составляющих треугольник
        public new Point[] GetPoints()
        {
            Point[] result = new Point[3];
            result[0] = edges[0].points[0];
            result[1] = edges[0].points[1];
            result[2] = (result[0] == edges[1].points[0] || result[1] == edges[1].points[0]) ? edges[1].points[1] : edges[1].points[0];
            return result;
        }


        // Возвращает массив размерности 4 из точек составляющих 2 соседних треугольника
        public Point[] GetPoints(Triangle tr)
        {
            Point[] result = new Point[4];
            TriangulationEdge commonEdge = this.GetCommonEdge(tr);
            Point[] points1 = this.GetPoints();
            Point[] points2 = tr.GetPoints();
            Point p1 = null;
            Point p2 = null;
            for (int i = 0; i < 3; i++)
            {
                if (!commonEdge.IsContainingPoint(points1[i]))
                {
                    p1 = points1[i];
                }
                if (!commonEdge.IsContainingPoint(points2[i]))
                {
                    p2 = points2[i];
                }
            }
            result[0] = p1;
            result[1] = commonEdge.points[0];
            result[2] = p2;
            result[3] = commonEdge.points[1];
            //List<Point> r1 = new List<Point>(result);
            //BMF.SortPointsClockwise(r1);
            //return r1.ToArray();
            return result;
        }
        public Point GetOtherPoint(Point p1, Point p2)
        {
            Point[] points = this.GetPoints();
            foreach(Point p in points)
            {
                if(p != p1 && p != p2)
                {
                    return p;
                }
            }
            return null;
        }

        // Определяет имеется ли у треугольника заданная точка
        public bool IsHavePoint(Point point)
        {
            if (point == this.edges[0].points[0] || point == this.edges[0].points[1] || point == this.edges[1].points[0] || point == this.edges[1].points[1])
            {
                return true;
            }
            return false;
        }


        public bool IsPointIntersect2D(Point point)
        {
            Point[] points = this.GetPoints();
            foreach(Point p in points)
            {
                if (p.IsEqual2D(point))
                {
                    return true;
                }
            }
            return false;
        }

        public Point PointIntersect2D(Point point)
        {
            Point[] points = this.GetPoints();
            foreach (Point p in points)
            {
                if (p.IsEqual2D(point))
                {
                    return point;
                }
            }
            return null;
        }


        // Определяет имеется ли у треугольников общая грань
        public bool IsHaveCommonEdge(Triangle tr)
        {
            for(int i = 0; i < 3; i++)
            {
                if(this.edges[i] == tr.edges[0] || this.edges[i] == tr.edges[1] || this.edges[i] == tr.edges[2])
                {
                    return true;
                }
            }
            return false;
        }

        // Возвращает общее ребро двух треугольников
        public TriangulationEdge GetCommonEdge(Triangle tr)
        {
            if (this == tr) return null;
            for (int i = 0; i < 3; i++)
            {
                if (this.edges[i] == tr.edges[0] || this.edges[i] == tr.edges[1] || this.edges[i] == tr.edges[2])
                {
                    return this.edges[i];
                }
            }
            return null;
        }


        // Возвращает ребро треугольника по точам
        public TriangulationEdge GetEdgeByPoints(Point p1, Point p2)
        {
            for(int i = 0; i < 3 ; i++)
            {
                if((this.edges[i].IsContainingPoint(p1) && this.edges[i].IsContainingPoint(p2)))
                {
                    return this.edges[i];
                }
            }
            return null;
        }

        // Возвращает соседние с текущим треугольником треугольники
        public Triangle[] GetBorderTriangles()
        {
            Triangle[] result = new Triangle[3];
            for(int i = 0; i < 3; i++)
            {
                result[i] = this.edges[i].GetOtherTriangle(this);
            }
            return result;
        }


        // Заносит треугольник во внутренний массив ребра, если есть место
        private void InsetrTriangleInEdge(TriangulationEdge edge)
        {
            if(edge.triangles[0] == null)
            {
                edge.triangles[0] = this;
                return;
            }
            else if(edge.triangles[1] == null)
            {
                edge.triangles[1] = this;
                return;
            }
            return;
            //
            //
            // Кинь здесь ошибку на случай полноты обоих полей
            //
            //
        }
        // Заносит треугольник во внутрениий массив ребра, если есть место, или заменяет передаваемый треугольник
        private void InsetrTriangleInEdge(TriangulationEdge edge, Triangle tr)
        {
            if (tr == edge.triangles[0] && tr != null)
            {
                edge.triangles[0] = this;
                return;
            }
            else if (tr == edge.triangles[1] && tr != null)
            {
                edge.triangles[1] = this;
                return;
            }
            return;
            //
            //
            // Кинь здесь ошибку на случай полноты обоих полей
            //
            //
        }
        // Возвращает центр треугольника
        public Point GetCentroid(bool isThreeDimensions = false)
        {
            Point[] points = this.GetPoints();
            if(!isThreeDimensions){
                return new Point((points[0].X + points[1].X + points[2].X) / 3d, (points[0].Y + points[1].Y + points[2].Y) / 3d, 0);
            }
            else
            {
                return new Point((points[0].X + points[1].X + points[2].X) / 3d, (points[0].Y + points[1].Y + points[2].Y) / 3d, (points[0].Z + points[1].Z + points[2].Z) / 3d);
            }
        }

        public bool IsEdgeOnHeightPlane(double height)
        {
            foreach(TriangulationEdge edge in edges)
            {
                if (edge.IsPointsHeightEqual(height)) return true;
            }
            return false;
        }
        public bool IsPointOnEdge(Point p)
        {
            foreach (TriangulationEdge edge in this.edges)
            {
                if (edge.isPointBelongToSegment(p))
                {
                    return true;
                }
            }
            return false;
        }
        public TriangulationEdge GetEdgeWithPoint(Point point)
        {
            foreach(TriangulationEdge edge in this.edges)
            {
                if (edge.isPointBelongToSegment(point))
                {
                    return edge;
                }
            }
            return null;
        }
        public double[] GetPlanarFactors()
        {
            double[] result = new double[4]; // Массив для хранения результата
            Point[] points = this.GetPoints();  // Получаем точки текущего треугольника
            Vector3D vec1 = new Vector3D(new TriangulationEdge(points[0], points[1]));  // Создаём вектор из точек 0 и 1
            Vector3D vec2 = new Vector3D(new TriangulationEdge(points[0], points[2]));  // Создаём вестор из точек 0 и 2
            result[0] = MxF.Det(new double[2, 2] { { vec1.Y, vec2.Y }, { vec1.Z, vec2.Z } }); // Через детерминант находим все коэффициенты
            result[1] = - MxF.Det(new double[2, 2] { { vec1.X, vec2.X }, { vec1.Z, vec2.Z } });
            result[2] = MxF.Det(new double[2, 2] { { vec1.X, vec2.X }, { vec1.Y, vec2.Y } });
            result[3] = (-points[0].X * result[0]) + (-points[0].Y * result[1]) + (-points[0].Z * result[2]); // Через полученные коэффициенты находим последний
            return result;
        }
        public new double GetArea()
        {
            Point[] points = this.GetPoints();
            Vector3D vec3d1 = new Vector3D(points[0], points[1]);
            Vector3D vec3d2 = new Vector3D(points[0], points[2]);

            return vec3d1.VectorMult(vec3d2).Length() / 2;
        }
        public new bool IsInside(Point point)
        {
            Point[] points = this.GetPoints();
            double k1 = (points[0].X - point.X) * (points[1].Y - point.Y) - (points[1].X - point.X) * (points[0].Y - point.Y);
            double k2 = (points[1].X - point.X) * (points[2].Y - point.Y) - (points[2].X - point.X) * (points[1].Y - point.Y);
            double k3 = (points[2].X - point.X) * (points[0].Y - point.Y) - (points[0].X - point.X) * (points[2].Y - point.Y);

            bool result = (k1 > 0 && k2 > 0 && k3 > 0) || (k1 < 0 && k2 < 0 && k3 < 0) || BMF.Deq(k1, 0) || BMF.Deq(k2, 0) || BMF.Deq(k3, 0);

            return result;
        }
        public bool IsEqual2D(Triangle triangle)
        {
            Point[] points = this.GetPoints();
            bool b1 = triangle.IsPointIntersect2D(points[0]);
            bool b2 = triangle.IsPointIntersect2D(points[1]);
            bool b3 = triangle.IsPointIntersect2D(points[2]);

            return (b1 && b2 && b3);
        }


        public Point GetPointBelongingToSegment(Segment seg)
        {
            Point[] points = this.GetPoints();
            foreach(Point point in points)
            {
                if (seg.isPointBelongToSegment(point)) return point;
            }
            return null;
        }
    }
}
