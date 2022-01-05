﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class Point : MathObject
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public Point(decimal X, decimal Y, decimal Z, string id = "0")
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.id = id;
        }
        /// <summary>
        /// Создаёт Point из строки, которую возвращает Point.ToString()
        /// </summary>
        /// <param name="pointString">Строка-источник</param>
        public Point(string pointString)
        {
            //pointString = pointString.Replace("Point ID: ", "");
            //pointString = pointString.Replace("X: ", "");
            //pointString = pointString.Replace("Y: ", "");
            //pointString = pointString.Replace("Z: ", "");

            pointString = pointString.Replace("ID:", "");
            pointString = pointString.Replace("X:", " ");
            pointString = pointString.Replace("Y:", " ");
            pointString = pointString.Replace("Z:", " ");

            string[] parts = pointString.Split(' ');
            this.id = parts[0].Trim();
            this.X = Convert.Todecimal(parts[1].Trim());
            this.Y = Convert.Todecimal(parts[2].Trim());
            this.Z = Convert.Todecimal(parts[3].Trim());
        }
        public bool IsEqual2D(Point point)
        {
            return (BMF.Deq(point.X, this.X) && BMF.Deq(point.Y, this.Y));
        }
        public decimal GetDistance(Point point)
        {
            return Math.Sqrt(Math.Pow(this.X - point.X, 2) + Math.Pow(this.Y - point.Y, 2) + Math.Pow(this.Z - point.Z, 2));
        }
        public decimal GetDistance2D(Point point)
        {
            return Math.Sqrt(Math.Pow(this.X - point.X, 2) + Math.Pow(this.Y - point.Y, 2));
        }
        public override string ToString()
        {
            //return $"Point ID: {this.id} X: {X} Y: {Y} Z: {Z}\n";
            return $"ID:{this.id}X:{X}Y:{Y}Z:{Z}\n";
        }
    }
}
