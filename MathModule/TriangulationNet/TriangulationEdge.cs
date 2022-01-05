using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MathModule
{
    public class TriangulationEdge : Segment
    {

        public Triangle[] triangles = new Triangle[2];
        public List<Point> interpolationPoints = new List<Point>();


        public TriangulationEdge(Point p1, Point p2, Triangle tr1, Triangle tr2) : base(p1,p2)
        {
            if (p1 == p2)
            {
                throw new AggregateException();
            }
            triangles[0] = tr1;
            triangles[1] = tr2;
        }
        public TriangulationEdge(Point p1, Point p2) : base(p1,p2)
        {
        }
        public TriangulationEdge(List<Point> points) : base(points)
        {
        }
        public void PseudoConstruct(Point p1, Point p2)
        {
            points[0] = p1;
            points[1] = p2;
        }

        public void RemoveTriangle(Triangle tr)
        {
            if (triangles[0] == tr)
            {
                triangles[0] = null;
            }
            if (triangles[1] == tr)
            {
                triangles[1] = null;
            }
        }
        public Triangle GetOtherTriangle(Triangle tr)
        {
            return triangles[0] == tr ? triangles[1] : triangles[0];
        }

        // Возвращает является ли ребро крайним
        public bool isBorderEdge()
        {
            if (triangles[0] == null || triangles[1] == null)
            {
                return true;
            }
            return false;
        }



        public decimal[] GetVector()
        {
            return new decimal[3] { this.points[0].X - this.points[1].X,     this.points[0].Y - this.points[1].Y,     this.points[0].Z - this.points[1].Z};
        }
    }
}