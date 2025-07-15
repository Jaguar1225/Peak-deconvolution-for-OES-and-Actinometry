using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    public class Clustering
    {

        public Clustering() { }

        public static (int[], double) KMC(double[,] Score, int numCluster)
        {

            var MatrixScore = Matrix<double>.Build.DenseOfArray(Score);
            var MatrixCenter = Matrix<double>.Build.Dense(numCluster, MatrixScore.ColumnCount);
            var label = Vector<double>.Build.Dense(Score.GetLength(0), 0); // 초기 레이블 설정

            var Distance = Matrix<double>.Build.Dense(Score.GetLength(0), numCluster);
            for (int i = 0; i < MatrixCenter.RowCount; i++)
            {
                for (int j = 0; j < MatrixCenter.ColumnCount; j++)
                {
                    MatrixCenter[i, j] = MatrixScore[ (Score.GetLength(0)/numCluster)*i,j]; // 초기 클러스터 중심을 점수의 처음 몇 개로 설정
                }
            }

            while (true)
            {
                for (int i = 0; i < numCluster; i++)
                {
                    Distance.SetColumn(i, CalculateDistances(MatrixScore, MatrixCenter.Row(i))); // 각 클러스터 중심과의 거리 계산
                }
                Vector<double> tempLabel = Distance.MapRows(dist_row => dist_row.MinimumIndex());
                if (label.Equals(tempLabel)) break;
                label = tempLabel; // 레이블 업데이트
                MatrixCenter = MeanOfCluster(MatrixScore, label); // 클러스터 중심 업데이트
            }

            int[] intlabel = label.ToArray().Select(x => (int)x).ToArray();
            var overallCenter = MatrixScore.Mean().ToArray<double>();
            var power_on_label = intlabel[FindOnRow(Score, overallCenter)[0]];

            intlabel = intlabel.Select(x => x == power_on_label ? 1 : 0).ToArray(); // 전원 켜짐 상태의 레이블을 1으로 설정

            double[,] doubleCener = MatrixCenter.ToArray();
            return (intlabel, ValidityScore(MatrixScore, label, MatrixCenter));
        }

        public static Vector<double> CalculateDistances(Matrix<double> score, Vector<double> center)
        {
            return score.MapRows(row => 
            {
                double distance = 0.0;
                for (int i = 0; i < row.Count; i++)
                {
                    distance += Math.Pow(row[i] - center[i], 2);
                }
                return Math.Sqrt(distance);
            });
        }

        public static Matrix<double> MeanOfCluster(Matrix<double> score, Vector<double> label )
        {

            var uniqueLabels = label.ToList().Distinct().ToList();
            var clusterMeans = Matrix<double>.Build.Dense(uniqueLabels.Count, score.ColumnCount);
            for (int i = 0; i < uniqueLabels.Count; i++)
            {
                var clusterIndices = label.Where(x => x == uniqueLabels[i]).ToList();
                var clusterScores = score.Rows(clusterIndices);
                clusterMeans.SetRow(i, clusterScores.Mean()); // 각 클러스터의 평균 계산
            }
            return clusterMeans;
        }
        public static double[] GetRow(double[,] matrix, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= matrix.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index is out of range.");
            return Enumerable.Range(0, matrix.GetLength(1)).Select(col => matrix[rowIndex, col]).ToArray();
        }
        public static List<int> FindOnRow(double[,] matrix, double[] doubles)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = GetRow(matrix, i);
                bool found = true;
                for (int j = 0; j < row.Length; j++)
                {
                    if (row[j] < doubles[j])
                    {
                        found = false;
                        break; // 중복을 피하기 위해 한 번 찾으면 다음 행으로 넘어감
                    }
                }
                if (found)
                    {
                    result.Add(i); // 조건을 만족하는 행의 인덱스를 추가
                }
            }
            return result;
        }

        public static (int[], double) DBSCAN(double[,] Score, double eps, int minPts)
        {
            var MatrixScore = Matrix<double>.Build.DenseOfArray(Score);
            var label = Vector<double>.Build.Dense(Score.GetLength(0), -1); // 초기 레이블 설정
            int clusterId = 0;
            for (int i = 0; i < MatrixScore.RowCount; i++)
            {
                if (label[i] != -1) continue; // 이미 처리된 점은 건너뜀
                var neighbors = FindNeighbors(MatrixScore, i, eps);
                if (neighbors.Count < minPts) 
                {
                    label[i] = -1; // 노이즈로 레이블링
                    continue;
                }
                clusterId++;
                label[i] = clusterId; // 새로운 클러스터 시작
                ExpandCluster(MatrixScore, label, neighbors, clusterId, eps, minPts);
            }
            int[] intlabel = label.ToArray().Select(x => (int)x).ToArray();
            ReassignLabels(ref intlabel); // 레이블을 1부터 시작하도록 재배치
            return (intlabel, ValidityScore(MatrixScore, label, MeanOfCluster(MatrixScore, label)));
        }

        public static List<int> FindNeighbors(Matrix<double> score, int index, double eps)
        {
            var neighbors = new List<int>();
            for (int i = 0; i < score.RowCount; i++)
            {
                if (i != index && (score.Row(i) - score.Row(index)).Norm(2) <= eps)
                {
                    neighbors.Add(i);
                }
            }
            return neighbors;
        }

        public static void ExpandCluster(
            Matrix<double> score, 
            Vector<double> label, 
            List<int> neighbors, 
            int clusterId, 
            double eps, 
            int minPts
            )
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                int neighborIndex = neighbors[i];
                if (label[neighborIndex] == -1) // 노이즈로 레이블링된 경우
                {
                    label[neighborIndex] = clusterId; // 클러스터로 변경
                }
                if (label[neighborIndex] != 0) continue; // 이미 클러스터에 속한 경우 건너뜀
                
                label[neighborIndex] = clusterId; // 클러스터 레이블 설정
                var newNeighbors = FindNeighbors(score, neighborIndex, eps);
                if (newNeighbors.Count >= minPts)
                {
                    neighbors.AddRange(newNeighbors); // 새로운 이웃 추가
                }
            }
        }
        public static void ReassignLabels(ref int[] labels)
        {
            Dictionary<int, int> labelMap = new Dictionary<int, int>();
            int newLabel = 1; // 레이블을 1부터 시작하도록 설정
            int[] newLabels = new int[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                int currentLabel = labels[i];
                if ( !labelMap.ContainsKey(currentLabel))
                {
                    labelMap[currentLabel] = newLabel++;
                }
                newLabels[i] = labelMap[currentLabel]; // 새로운 레이블로 매핑
            }
            labels = newLabels; // 기존 레이블을 새로운 레이블로 업데이트
        }

        public static double ValidityScore(
            Matrix<double> MatrixScore, 
            Vector<double> label, 
            Matrix<double> MatrixCenter
            )
        {
            var overallCenter = MatrixScore.Mean();
            double overallSum = MatrixScore.MapRows(row => 
            {
                double distance = 0.0;
                for (int i = 0; i < row.Count; i++)
                {
                    distance += Math.Pow(row[i] - overallCenter[i], 2);
                }
                return Math.Sqrt(distance);
            }).Sum();

            var ClusterDistance = Vector<double>.Build.Dense(MatrixCenter.RowCount);
            var Distance = Vector<double>.Build.Dense(MatrixScore.RowCount);
            for (int i = 0; i < MatrixCenter.RowCount; i++)
            {
                Distance = CalculateDistances(MatrixScore, MatrixCenter.Row(i));
                ClusterDistance[i] = label.Where(x => x == i).Select(x => Distance[x]).Sum(); // 각 클러스터의 거리 합계
            }
            double clusterSum = ClusterDistance.Sum();
            return overallSum / clusterSum;
        }
    }
}
