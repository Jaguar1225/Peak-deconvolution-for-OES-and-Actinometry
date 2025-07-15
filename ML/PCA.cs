using System.Linq;
using System;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Extensions;
using Peak_deconvolution_for_OES_and_Actinometry.Utills;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    internal class PCA
    {
        public static (double[,], double[,]) Transform(Matrix<double> matrix)
        {
            var Mean = matrix.MapCols(col => col.Mean());
            var Std = matrix.MapCols(col => col.StandardDeviation(2.0));
            Matrix<double> LAMatrix = Matrix<double>.Build.Dense(matrix.RowCount, matrix.ColumnCount);
            
            for (int i = 0; i < LAMatrix.RowCount; i++)
                LAMatrix.SetRow(i, (matrix.Row(i) - Mean)/Std);

            var n = LAMatrix.ColumnCount;
            int numComponents = Math.Min(3, n); // Ensure we don't exceed the number of columns
            var eigenVector = Matrix<double>.Build.Random(n, numComponents).NormalizeColumns(2.0);

            for (int iter = 0; iter < 100; iter++)
            {
                var Xv = LAMatrix * eigenVector;
                eigenVector = LAMatrix.Transpose() * Xv / (LAMatrix.RowCount - 1);
                eigenVector = eigenVector.NormalizeColumns(2.0);
            }
            var varianceMatrix = eigenVector.Transpose() * LAMatrix.Transpose() * (LAMatrix * eigenVector) / (LAMatrix.RowCount - 1);
            double[] varianceValues = new double[numComponents];
            for (int i = 0; i < numComponents; i++)
            {
                varianceValues[i] = varianceMatrix[i, i];
            }
            // 분산 크기에 따른 내림차순 정렬을 위한 인덱스 배열
            int[] indices = Enumerable.Range(0, numComponents)
                                     .OrderByDescending(i => varianceValues[i])
                                     .ToArray();

            // 고유벡터 재정렬
            Matrix<double> sortedEigenVector = Matrix<double>.Build.Dense(n, numComponents);
            for (int i = 0; i < numComponents; i++)
            {
                sortedEigenVector.SetColumn(i, eigenVector.Column(indices[i]));
            }

            // 정렬된 고유벡터로 Score 계산
            var score = (LAMatrix * sortedEigenVector).ToArray();

            // 정렬된 분산 계산
            double[,] sortedVariance = Matrix<double>.Build.Dense(numComponents, numComponents).ToArray();
            for (int i = 0; i < numComponents; i++)
            {
                sortedVariance[i, i] = varianceValues[indices[i]];
            }
            return (score, sortedVariance);
        }

        public static (double[,], double[,]) TransformNonNegative(Matrix<double> matrix)
        {
            // 1. 기본 PCA 수행
            var Mean = matrix.MapCols(col => col.Mean());
            var Std = matrix.MapCols(col => col.StandardDeviation(2.0));
            
            Matrix<double> LAMatrix = Matrix<double>.Build.Dense(matrix.RowCount, matrix.ColumnCount);
            for (int i = 0; i < LAMatrix.RowCount; i++)
                LAMatrix.SetRow(i, (matrix.Row(i) - Mean) / Std);

            var n = LAMatrix.ColumnCount;
            int numComponents = Math.Min(3, n);
            var eigenVector = Matrix<double>.Build.Random(n, numComponents).NormalizeColumns(2.0);

            for (int iter = 0; iter < 100; iter++)
            {
                var Xv = LAMatrix * eigenVector;
                eigenVector = LAMatrix.Transpose() * Xv / (LAMatrix.RowCount - 1);
                eigenVector = eigenVector.NormalizeColumns(2.0);
            }
            
            // 2. 비음수 제약 조건 적용 (각 열에서 음수 값을 0으로 대체)
            for (int i = 0; i < eigenVector.RowCount; i++)
                for (int j = 0; j < eigenVector.ColumnCount; j++)
                    if (eigenVector[i, j] < 0) eigenVector[i, j] = 0;
            
            // 3. 정규화 (각 열의 L2 노름을 1로)
            eigenVector = eigenVector.NormalizeColumns(2.0);
            
            // 4. 분산 계산 및 정렬 (이하 코드는 이전과 동일)
            var varianceMatrix = eigenVector.Transpose() * LAMatrix.Transpose() * (LAMatrix * eigenVector) / (LAMatrix.RowCount - 1);
            
            double[] varianceValues = new double[numComponents];
            for (int i = 0; i < numComponents; i++)
                varianceValues[i] = varianceMatrix[i, i];
            
            int[] indices = Enumerable.Range(0, numComponents)
                                     .OrderByDescending(i => varianceValues[i])
                                     .ToArray();
            
            Matrix<double> sortedEigenVector = Matrix<double>.Build.Dense(n, numComponents);
            for (int i = 0; i < numComponents; i++)
                sortedEigenVector.SetColumn(i, eigenVector.Column(indices[i]));
            
            var score = (LAMatrix * sortedEigenVector).ToArray();
            
            double[,] sortedVariance = Matrix<double>.Build.Dense(numComponents, numComponents).ToArray();
            for (int i = 0; i < numComponents; i++)
                sortedVariance[i, i] = varianceValues[indices[i]];
            
            return (score, sortedVariance);
        }
        public static async Task<(double[,], double[,])> TransformAsync(Matrix<double> matrix)
        {
            return await LoadingManager.RunWithLoadingAsync(async () =>
            {
                LoadingManager.UpdateMessage("Performing PCA transformation...");

                var Mean = matrix.MapCols(col => col.Mean());
                var Std = matrix.MapCols(col => col.StandardDeviation(2.0));

                Matrix<double> LAMatrix = Matrix<double>.Build.Dense(matrix.RowCount, matrix.ColumnCount);
                
                for (int i = 0; i < LAMatrix.RowCount; i++)
                    LAMatrix.SetRow(i, (matrix.Row(i) - Mean) / Std);

                var n = LAMatrix.ColumnCount;
                int numComponents = Math.Min(3, n); // Ensure we don't exceed the number of columns
                var eigenVector = Matrix<double>.Build.Random(n, numComponents).NormalizeColumns(2.0);

                for (int iter = 0; iter < 100; iter++)
                {
                    var Xv = LAMatrix * eigenVector;
                    eigenVector = LAMatrix.Transpose() * Xv / (LAMatrix.RowCount - 1);
                    eigenVector = eigenVector.NormalizeColumns(2.0);
                    LoadingManager.UpdateProgress(iter * 100 / 100); // Update progress bar
                }
                LoadingManager.UpdateProgress(100); // Ensure progress bar reaches 100%

                var varianceMatrix = eigenVector.Transpose() * LAMatrix.Transpose() * (LAMatrix * eigenVector) / (LAMatrix.RowCount - 1);
                double[] varianceValues = new double[numComponents];
                LoadingManager.UpdateMessage("Calculating variance...");
                LoadingManager.ResetProgress();
                for (int i = 0; i < numComponents; i++)
                {
                    varianceValues[i] = varianceMatrix[i, i];
                    LoadingManager.UpdateProgress(i * 100 / numComponents); // Update progress bar
                }
                LoadingManager.UpdateProgress(100); // Ensure progress bar reaches 100%

                // Sort eigenvectors by variance in descending order
                int[] indices = Enumerable.Range(0, numComponents)
                                         .OrderByDescending(i => varianceValues[i])
                                         .ToArray();
                LoadingManager.UpdateMessage("Sorting eigenvectors by variance...");
                LoadingManager.ResetProgress();
                Matrix<double> sortedEigenVector = Matrix<double>.Build.Dense(n, numComponents);
                for (int i = 0; i < numComponents; i++)
                {
                    sortedEigenVector.SetColumn(i, eigenVector.Column(indices[i]));
                    LoadingManager.UpdateProgress(i * 100 / numComponents); // Update progress bar
                }

                // Calculate scores using sorted eigenvectors
                var score = (LAMatrix * sortedEigenVector).ToArray();

                return (score, sortedEigenVector.ToArray());
            });
        }
        public static async Task<(double[,], double[,])> TransformNonNegativeAsync(Matrix<double> matrix)
        {
            return await Task.Run(() =>
            {
                var n = matrix.RowCount;
                var m = matrix.ColumnCount;
                int numComponents = Math.Min(3, m); // Ensure we don't exceed the number of columns
                var Mean = matrix.MapCols(col => col.Mean());
                var Std = matrix.MapCols(col => col.StandardDeviation(2.0));
                Matrix<double> LAMatrix = Matrix<double>.Build.Dense(n, m);
                for (int i = 0; i < n; i++)
                    LAMatrix.SetRow(i, (matrix.Row(i) - Mean) / Std);
                
                var eigenVector = Matrix<double>.Build.Random(m, numComponents).NormalizeColumns(2.0);
                for (int iter = 0; iter < 100; iter++)
                {
                    var Xv = LAMatrix * eigenVector;
                    eigenVector = LAMatrix.Transpose() * Xv / (LAMatrix.RowCount - 1);
                    eigenVector = eigenVector.NormalizeColumns(2.0);
                }
                // Apply non-negativity constraint
                for (int i = 0; i < eigenVector.RowCount; i++)
                {
                    for (int j = 0; j < eigenVector.ColumnCount; j++)
                    {
                        if (eigenVector[i, j] < 0) eigenVector[i, j] = 0;
                    }
                }
                // Normalize columns
                eigenVector = eigenVector.NormalizeColumns(2.0);
                // Calculate variance matrix
                var varianceMatrix = eigenVector.Transpose() * LAMatrix.Transpose() * (LAMatrix * eigenVector) / (LAMatrix.RowCount - 1);
                double[] varianceValues = new double[numComponents];
                for (int i = 0; i < numComponents; i++)
                {
                    varianceValues[i] = varianceMatrix[i, i];
                }
                // Sort eigenvectors by variance in descending order
                
                int[] indices = Enumerable.Range(0, numComponents)
                                         .OrderByDescending(i => varianceValues[i])
                                         .ToArray();
                Matrix<double> sortedEigenVector = Matrix<double>.Build.Dense(m, numComponents);
                for (int i = 0; i < numComponents; i++)
                {
                    sortedEigenVector.SetColumn(i, eigenVector.Column(indices[i]));
                }
                // Calculate scores using sorted eigenvectors
                var score = (LAMatrix * sortedEigenVector).ToArray();
                double[,] sortedVariance = Matrix<double>.Build.Dense(numComponents, numComponents).ToArray();
                for (int i = 0; i < numComponents; i++)
                {
                    sortedVariance[i, i] = varianceValues[indices[i]];
                }
                return (score, sortedVariance);
            });
        }
    }
}
