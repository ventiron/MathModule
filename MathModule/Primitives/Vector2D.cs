using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Vector2D : MathObject
    {
        public decimal X;
        public decimal Y;

        public Vector2D(decimal X, decimal Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector2D(Segment segment, bool backwards = false)
        {
            if (backwards)
            {
                this.X = segment.points[0].X - segment.points[1].X;
                this.Y = segment.points[0].Y - segment.points[1].Y;
            }
            else
            {
                this.X = segment.points[1].X - segment.points[0].X;
                this.Y = segment.points[1].Y - segment.points[0].Y;
            }
        }
        public Vector2D(Point p1, Point p2, bool backwards = false)
        {
            if (backwards)
            {
                this.X = p1.X - p2.X;
                this.Y = p1.Y - p2.Y;
            }
            else
            {
                this.X = p2.X - p1.X;
                this.Y = p2.Y - p1.Y;
            }
        }

        public Vector2D Diff(Vector2D vector)
        {
            return new Vector2D(this.X - vector.X, this.Y - vector.Y);
        }
        public Vector2D Sum(Vector2D vector)
        {
            return new Vector2D(this.X + vector.X, this.Y + vector.Y);
        }
        public decimal ScalarMultOrtho(Vector2D vector)
        {
            return this.X * vector.X + this.Y * vector.Y;
        }
        public decimal ScalarMult(Vector2D vector)
        {
            return 0m;
        }
        public decimal Length()
        {
            return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
        }
    }
}
