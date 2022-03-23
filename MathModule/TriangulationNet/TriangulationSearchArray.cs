using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class TriangulationSearchArray
    {
        //LinkCell[,] linkCells;
        Triangle[,] linkCells;
        private int planarMod;
        long planarBorder;
        int count;
        private  double section;
        Triangulation triangulation;

        private Point min;
        private Point max;

        int codk;
        public TriangulationSearchArray(Triangulation triangulation)
        {
            codk = 0;
            planarBorder = 1;
            planarMod = 5;
            section = 0;
            this.triangulation = triangulation;

            Point min = BMF.GetMin(triangulation.points);
            Point max = BMF.GetMax(triangulation.points);

            this.min = min;
            this.max = max;


            double dX = max.X - min.X;
            double dY = max.Y - min.Y;

            double dd = dX - dY;

            if (dd>0)
            {
                min.Y -= dd + 1;
                max.Y += dd + 1;
                min.X--;
                max.X++;

                //min.Y -= dd;
                //max.Y += dd;
            }
            else
            {
                min.X += dd - 1;
                max.X -= dd - 1;
                min.Y--;
                max.Y++;

                //min.X += dd;
                //max.X -= dd;
            }

            section = max.X - min.X;

            linkCells = new Triangle[1, 1];

            count = 0;
            foreach (Triangle triangle in triangulation.triangles)
            {
                this.AddTriangle(triangle);
            }
        }


        public void AddTriangle(Triangle tr, bool CheckIfInMinMax = false)
        {
            if (CheckIfInMinMax)
            {
                Point tMin = BMF.GetMin(new List<Point>(tr.GetPoints()));
                Point tMax = BMF.GetMax(new List<Point>(tr.GetPoints()));

                if(min.X > tMin.X)
                {
                    double del = min.X - tMin.X;
                    min.X -= del;
                    min.Y -= del;
                    section = max.X - min.X;
                }
                if (min.Y > tMin.Y)
                {
                    double del = min.Y - tMin.Y;
                    min.X -= del;
                    min.Y -= del;
                    section = max.X - min.X;
                }
                if (min.Z > tMin.Z)
                {
                    min.Z = min.Z - (min.Z - tMin.Z);
                }
                if (max.X < tMax.X)
                {
                    double del = max.X - tMax.X;
                    max.X -= del;
                    max.Y -= del;
                    section = max.X - min.X;
                }
                if (max.Y < tMax.Y)
                {
                    double del = max.Y - tMax.Y;
                    max.X -= del;
                    max.Y -= del;
                    section = max.X - min.X;
                }
                if (max.Z < tMax.Z)
                {
                    double del = max.Z + tMax.Z;
                    min.Z = min.Z - (max.Z - tMin.Z);
                }
            }



            count++;
            linkCells[Convert.ToInt32(Math.Truncate((tr.GetCentroid().X - min.X) / section)), Convert.ToInt32(Math.Truncate((tr.GetCentroid().Y - min.Y) / section))] = tr;
            if (count == planarBorder)
            {
                planarBorder = (linkCells.Length ^ 2) * planarMod;
                this.UpgradePlanarRectangles();
            }
        }
        public void DeleteTriangle(Triangle tr)
        {
            if(linkCells.GetLength(0) == 1)
            {
                throw new AggregateException("Невозможно удалить единственный треугольник в массиве");
            }

        }
        public Triangle GetTriangleByPoint(Point p)
        {
           return linkCells[Convert.ToInt32(Math.Truncate((p.X - min.X) / section)), Convert.ToInt32(Math.Truncate((p.Y - min.Y) / section))];
        }
        private bool IsInside(Triangle tr)
        {
            return false;
        }



        private void UpgradePlanarRectangles()
        {
            codk++;
            //LinkCell[,] linkCellsTemp = new LinkCell[linkCells.GetLength(0) * 2, linkCells.GetLength(0) * 2];
            Triangle[,] linkCellsTemp = new Triangle[linkCells.GetLength(0) * 2, linkCells.GetLength(0) * 2];
            for (int i = 0; i < linkCells.GetLength(0); i++)
            {
                for (int j = 0; j < linkCells.GetLength(0); j++)
                {
                    //linkCellsTemp[i * 2, j * 2] = new LinkCell(linkCells[i, j].MinX, linkCells[i, j].MaxY / 2d, linkCells[i, j].MaxX / 2d, linkCells[i, j].MaxY, linkCells[i, j].GetTriangle());
                    //linkCellsTemp[i * 2 + 1, j * 2] = new LinkCell(linkCells[i, j].MinX, linkCells[i, j].MinY, linkCells[i, j].MaxX / 2d, linkCells[i, j].MaxY / 2d, linkCells[i, j].GetTriangle());
                    //linkCellsTemp[i * 2, j * 2 + 1] = new LinkCell(linkCells[i, j].MaxX / 2d, linkCells[i, j].MaxY / 2d, linkCells[i, j].MaxX, linkCells[i, j].MaxY, linkCells[i, j].GetTriangle());
                    //linkCellsTemp[i * 2 + 1, j * 2 + 1] = new LinkCell(linkCells[i, j].MaxX / 2d, linkCells[i, j].MinY, linkCells[i, j].MaxX, linkCells[i, j].MaxY / 2d, linkCells[i, j].GetTriangle());

                    linkCellsTemp[i * 2, j * 2] = linkCells[i, j];
                    linkCellsTemp[i * 2 + 1, j * 2] = linkCells[i, j];
                    linkCellsTemp[i * 2, j * 2 + 1] = linkCells[i, j];
                    linkCellsTemp[i * 2 + 1, j * 2 + 1] = linkCells[i, j];
                }
            }
            section /= 2d;
            linkCells = linkCellsTemp;
        }



        public double GetSection()
        {
            return section;
        }
        public Point GetMin()
        {
            return min;
        }

        public Point GetMax()
        {
            return max;
        }
    }
}
