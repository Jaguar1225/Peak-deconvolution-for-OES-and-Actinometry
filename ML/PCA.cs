using System.Linq;
using System;

using MathNet.Numerics.LinearAlgebra;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    internal class PCA
    {
        public static (double[], double) Transform(double[,] Matrix)
        {
            var LAMatrix = Matrix<double>.Build.DenseOfArray(Matrix);

            var svd = LAMatrix.Svd(true);
            var eigenVector = svd.VT.Row(0);

            var Score = (LAMatrix*eigenVector).ToArray<double>();
            double variance = Math.Pow(svd.S[0],2)/(LAMatrix.RowCount-1);

            return (Score, variance);
        }
    }
}
