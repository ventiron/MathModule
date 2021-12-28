using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class LinkCell
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        Triangle triangle;

        public LinkCell(double minX, double minY, double maxX, double maxY, Triangle triangle)
        {
            this.MinX = minX;
            this.MinY = minY;
            this.MaxX = maxX;
            this.MaxY = maxY;
            this.triangle = triangle;
        }



        public void SetTriangle (Triangle triangle)
        {
            this.triangle = triangle;
        }

        public Triangle GetTriangle()
        {
            return triangle;
        }

    }
}
