using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class MxF
    {

        /// <summary>
        /// Функция определяющая сумму массивовв в виде многомерного массива
        /// </summary>
        /// <param name="mat1">Первая матрица, в виде многомерного массива</param>
        /// <param name="mat2">Вторая матрица, в виде многомерного массива</param>
        /// <returns></returns>
        public static decimal[,] Sum(decimal[,] mat1, decimal[,] mat2)
        {
            if (!IsSameSize(mat1, mat2))
            {
                return null;
            }
            decimal[,] result = new decimal[mat1.GetLength(0), mat1.GetLength(1)];
            for (int i = 0; i < mat1.GetLength(0); i++)
            {
                for (int j = 0; j < mat1.GetLength(1); j++)
                {
                    result[i, j] = mat1[i, j] + mat2[i, j];
                }
            }
            return result;
        }


        /// <summary>
        /// Умножает матрицы друг на друга в виде многомерного массива
        /// </summary>
        /// <param name="mat1">Первая матрица, в виде многомерного массива</param>
        /// <param name="mat2">Вторая матрица, в виде многомерного массива</param>
        /// <returns></returns>
        public static decimal[,] Mult(decimal[,] mat1, decimal[,] mat2)
        {
            if (!IsMultPossible(mat1, mat2))
            {
                throw new ArgumentOutOfRangeException("mat1, mat2", "Неверный формат матриц, число столбцов первой матрицы должно совпадать с числом строк второй матрицы");
            }
            decimal[,] result = new decimal[mat1.GetLength(0), mat2.GetLength(1)];
            for (int i = 0; i < mat2.GetLength(1); i++)
            {
                for (int j = 0; j < mat1.GetLength(0); j++)
                {
                    for (int k = 0; k < mat2.GetLength(0); k++)
                    {
                        result[j, i] += mat1[j, k] * mat2[k, i];
                    }
                }

            }
            return result;
        }


        /// <summary>
        /// Вычисляет детерминант матрицы
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static decimal Det(decimal[,] mat)
        {
            decimal result = 0;
            if (mat.GetLength(0) != mat.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("mat", "Детерминант возможно найти только у квадратной матрицы");
            }
            if (mat.GetLength(0) == 1)
            {
                return mat[0, 0];
            }
            if (mat.GetLength(0) == 2)
            {
                return mat[0, 0] * mat[1, 1] - mat[1, 0] * mat[0, 1];
            }
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                result = i%2 == 0? result + mat[0, i] * Det(GetSubMatrix(mat, 0, i)) : result - mat[0, i] * Det(GetSubMatrix(mat, 0, i));
            }
            return result;
        }

        /// <summary>
        /// Находит субматрицу из изначальной, исключая заданные строку и столбец
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static decimal[,] GetSubMatrix(decimal[,] mat, int row, int column)
        {
            if (mat.GetLength(0) != mat.GetLength(1) || mat.GetLength(0) == 1 || mat.GetLength(1) == 1)
            {
                throw new ArgumentOutOfRangeException("mat", "Неверный формат матрицы");
            }
            if (row >= mat.GetLength(0) || column >= mat.GetLength(1) || row < 0 || column < 0)
            {
                throw new IndexOutOfRangeException("Выбранная строка или столбец выходят за пределы матрицы");
            }
            decimal[,] result = new decimal[mat.GetLength(0) - 1, mat.GetLength(1) - 1];

            int rowCount = 0;
            int columnCount = 0;
            for(int i = 0; i < mat.GetLength(0); i++)
            {
                if (i == row)
                {
                    rowCount = 1;
                    continue;
                }
                for(int j = 0; j < mat.GetLength(1); j++)
                {
                    if (j == column) 
                    {
                        columnCount = 1;
                        continue; 
                    }
                    result[i - rowCount, j - columnCount] = mat[i, j];
                }
                columnCount = 0;
            }
            return result;
        }
        /// <summary>
        /// Проверяет матрицы на соответсвие по размеру сторок и столбцов
        /// </summary>
        /// <param name="mat1">Первая матрица, в виде многомерного массива</param>
        /// <param name="mat2">Вторая матрица, в виде многомерного массива</param>
        /// <returns></returns>
        public static bool IsSameSize(decimal[,] mat1, decimal[,] mat2)
        {
            return mat1.GetLength(0) == mat2.GetLength(0) && mat1.GetLength(1) == mat2.GetLength(1);
        }



        /// <summary>
        /// Проверяет возможно ли умножение матриц
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="mat2"></param>
        /// <returns></returns>
        public static bool IsMultPossible(decimal[,] mat1, decimal[,] mat2)
        {

            return mat1.GetLength(1) == mat2.GetLength(0);
        }
        /// <summary>
        /// Только для квадратных матриц размерностью 3 и выше
        /// </summary>
        /// <param name="row_colummn"></param>
        /// <param name="isRow"></param>
        /// <returns></returns>
        public static decimal[] GetFactors(decimal[,] mat)
        {
            decimal[] result = new decimal[mat.GetLength(1)];
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = Det(GetSubMatrix(mat, 0, i));
            }

            return result;
        }

        public static decimal GetMinod(decimal[,] mat, int row, int column)
        {
            return Det(GetSubMatrix(mat, row, column));
        }

        public static decimal[,] InverseMat(decimal[,] mat)
        {
            decimal[,] result = new decimal[mat.GetLength(1), mat.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    result[j, i] = mat[i, j];
                }

            }
            return mat;
        }
    }
}
