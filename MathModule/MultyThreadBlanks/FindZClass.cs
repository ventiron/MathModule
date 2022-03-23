using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule.MultyThreadBlanks
{
    public class FindZClass
    {
        Point p;
        Polygon border;
        Triangulation triangulation;
        double step;
        public FindZClass(Point p, Polygon border, Triangulation triangulation, double step)
        {
            this.p = p;
            this.border = border;
            this.triangulation = triangulation;
            this.step = step;
        }


        public void FindPointZ()
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
                    Triangle tr = GetClosestSegment(p, triangulation.outerEdges).GetOtherTriangle(null);
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

    }
}
