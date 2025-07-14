using System.Linq;
using System;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Extensions;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    internal class PCA
    {
        public static (double[], double) Transform(Matrix<double> matrix)
        {
            var Mean = matrix.MapCols(col => col.Mean());
            var Std = matrix.MapCols(col => col.StandardDeviation(2.0));
            Matrix<double> LAMatrix = Matrix<double>.Build.Dense(matrix.RowCount, matrix.ColumnCount);
            
            for (int i = 0; i < LAMatrix.RowCount; i++)
                LAMatrix.SetRow(i, (matrix.Row(i) - Mean)/Std);

            var n = LAMatrix.ColumnCount;
            var eigenVector = Vector<double>.Build.Random(n).Normalize(2.0);

            for (int iter = 0; iter < 100; iter++)
            {
                var Xv = LAMatrix * eigenVector;
                eigenVector = LAMatrix.Transpose() * Xv / (LAMatrix.RowCount - 1);
                eigenVector = eigenVector.Normalize(2.0);
            }


            var Score = (LAMatrix * eigenVector).ToArray();
            if (Score.Average() < 0)
            {
                Score = Score.Select(x => -x).ToArray(); // Ensure positive scores
                eigenVector = -eigenVector; // Adjust eigenvector accordingly
            }
            double variance = eigenVector* LAMatrix.Transpose() * (LAMatrix * eigenVector)/ (LAMatrix.RowCount - 1);

            return (Score, variance);
        }
    }
}
