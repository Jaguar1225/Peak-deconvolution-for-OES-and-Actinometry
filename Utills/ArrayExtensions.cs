using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_deconvolution_for_OES_and_Actinometry.Utills
{
    public static class ArrayExtensions
    {
        public static T[] GetColumn<T>(this T[,] matrix, int columnIndex)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                .Select(rowIndex => matrix[rowIndex, columnIndex])
                .ToArray();
        }
        public static T[,] GetColumns<T>(this T[,] matrix, List<int> columnIndices)
        {
            int rowCount = matrix.GetLength(0);
            int colCount = columnIndices.Count;
            T[,] result = new T[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    result[i, j] = matrix[i, columnIndices[j]];
                }
            }
            return result;
        }
        public static T[] GetRow<T>(this T[,] matrix, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= matrix.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index is out of range.");
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(col => matrix[rowIndex, col])
                .ToArray();
        }
        public static T[,] GetRows<T>(this T[,] matrix, List<int> rowIndices)
        {
            int rowCount = rowIndices.Count; int colCount = matrix.GetLength(1);
            T[,] result = new T[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    result[i, j] = matrix[rowIndices[i], j];
                }
            }
            return result;
        }
        public static double GetStd<T>(this T[,] matrix, int columnIndex)
        {
            var column = matrix.GetColumn(columnIndex);
            double mean = matrix.GetMean(columnIndex);
            double sumOfSquares = column.Sum(value => Math.Pow(Convert.ToDouble(value) - mean, 2));
            double stdDev = Math.Sqrt(sumOfSquares / (column.Length - 1)); // Sample standard deviation
            return stdDev;
        }
        public static double GetMean<T>(this T[,] matrix, int columnIndex)
        {
            var column = matrix.GetColumn(columnIndex);
            return column.Average(value => Convert.ToDouble(value));
        }
    }
}
