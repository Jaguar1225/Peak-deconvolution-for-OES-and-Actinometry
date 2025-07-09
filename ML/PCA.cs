using System.Linq;
using System;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Extensions;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    internal class PCA
    {
        public static (double[], double) Transform(double[,] Matrix)
        {
            var LAMatrix = Matrix<double>.Build.DenseOfArray(Matrix);

            var Mean = LAMatrix.Mean();
            for (int i = 0; i < LAMatrix.RowCount; i++)
                LAMatrix.SetRow(i, LAMatrix.Row(i) - Mean);

            var n = LAMatrix.ColumnCount;
            var eigenVector = Vector<double>.Build.Random(n).Normalize(2.0);

            for (int iter = 0; iter < 100; iter++)
            {
                var Xv = LAMatrix * eigenVector;
                eigenVector = LAMatrix.Transpose() * Xv / (LAMatrix.RowCount - 1);
                eigenVector = eigenVector.Normalize(2.0);
            }

            var Score = (LAMatrix*eigenVector).ToArray();
            double variance = eigenVector*LAMatrix.Transpose() * (LAMatrix * eigenVector)/ (LAMatrix.RowCount - 1);

            return (Score, variance);
        }
    }
}
