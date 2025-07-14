using System;
using System.Collections.Generic;
using System.Linq;

namespace MathNet.Numerics.LinearAlgebra.Extensions
{
    public static class MatrixExtensions
    {
        public static Matrix<double> Rows(this Matrix<double> matrix, List<int> indicies)
        {
            var selectedRows = indicies.Select(index => matrix.Row(index));
            return Matrix<double>.Build.DenseOfRows(selectedRows);
        }
        public static Vector<double> Mean(this Matrix<double> matrix)
        {
            return matrix.ColumnSums() / matrix.RowCount;
        }
        public static Vector<double> MapRows(this Matrix<double> matrix, Func<Vector<double>, double> func)
        {
            return Vector<double>.Build.Dense(matrix.EnumerateRows().Select(func).ToArray());
        }
        public static Vector<double> MapCols(this Matrix<double> matrix, Func<Vector<double>, double> func)
        {
            return Vector<double>.Build.Dense(matrix.EnumerateColumns().Select(func).ToArray());
        }
    }
    public static class VectorExtensions
    {
        public static Vector<double> Rows(this Vector<double> vector, List<int> indices)
        {
            var selectedRows = indices.Select(index => vector[index]);
            return Vector<double>.Build.Dense(selectedRows.ToArray());
        }
        public static double Mean(this Vector<double> vector)
        {
            return vector.Sum() / vector.Count;
        }
        public static double StandardDeviation(this Vector<double> vector, double mean)
        {
            return Math.Sqrt(vector.Select(x => Math.Pow(x - mean, 2)).Sum() / (vector.Count - 1));
        }
        public static List<int> Where(this Vector<double> vector, Func<double, bool> predicate)
        {
            List<int> indices = new List<int>();
            var temp_vector = vector.Clone();

            for (int i = 0; i < vector.Count; i++)
            {
                if (predicate(vector[i]))
                {
                    indices.Add(i);
                }
            }
            return indices;
        }
    }
}
