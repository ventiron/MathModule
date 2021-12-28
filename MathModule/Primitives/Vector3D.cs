using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Vector3D : MathObject
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3D(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Vector3D(Segment segment, bool backwards = false)
        {
            if (backwards)
            {
                this.X = segment.points[0].X - segment.points[1].X;
                this.Y = segment.points[0].Y - segment.points[1].Y;
                this.Z = segment.points[0].Z - segment.points[1].Z;
            }
            else
            {
                this.X = segment.points[1].X - segment.points[0].X;
                this.Y = segment.points[1].Y - segment.points[0].Y;
                this.Z = segment.points[1].Z - segment.points[0].Z;
            }
        }
        public Vector3D(Point p1, Point p2, bool backwards = false)
        {
            if (backwards)
            {
                this.X = p1.X - p2.X;
                this.Y = p1.Y - p2.Y;
                this.Z = p1.Z - p2.Z;
            }
            else
            {
                this.X = p2.X - p1.X;
                this.Y = p2.Y - p1.Y;
                this.Z = p2.Z - p1.Z;
            }
        }

        public Vector3D Diff(Vector3D vector)
        {
            return new Vector3D(this.X - vector.X, this.Y - vector.Y, this.Z - vector.Z);
        }
        public Vector3D Sum(Vector3D vector)
        {
            return new Vector3D(this.X + vector.X, this.Y + vector.Y, this.Z + vector.Z);
        }
        public double ScalarMultOrtho(Vector3D vector)
        {
            return this.X * vector.X + this.Y * vector.Y + this.Z * vector.Z;
        }
        public double ScalarMult(Vector2D vector)
        {
            return 0d;
        }
        public Vector3D VectorMult(Vector3D vector)
        {
            double[] factors = MxF.GetFactors(new double[3, 3] { { 1, 1, 1 }, { this.X, this.Y, this.Z }, { vector.X, vector.Y, vector.Z } });
            return new Vector3D(factors[0], factors[1], factors[2]);
        }
        public double Length()
        {
            return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2));
        }
        public double PowLength()
        {
            return Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2);
        }
    }
}
