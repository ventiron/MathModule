using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class LinkCell
    {
        public decimal MinX { get; set; }
        public decimal MinY { get; set; }
        public decimal MaxX { get; set; }
        public decimal MaxY { get; set; }
        Triangle triangle;

        public LinkCell(decimal minX, decimal minY, decimal maxX, decimal maxY, Triangle triangle)
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
