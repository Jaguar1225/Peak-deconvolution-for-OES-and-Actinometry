using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    public class Clustering
    {

        public Clustering() { }

        public static (int[], double) KMC(double[] Score, int numCluster)
        {
            var VectorScore = Vector<double>.Build.Dense(Score);
            var VectorCenter = Vector<double>.Build.Dense(numCluster);
            var label = Vector<double>.Build.Dense(Score.Length, 0); // 초기 레이블 설정

            var Distance = Matrix<double>.Build.Dense(Score.Length, numCluster);
            for (int i = 0; i < VectorCenter.Count; i++)
            {
                VectorCenter[i] = Score[(Score.Length/numCluster)*i]; // 초기 클러스터 중심을 점수의 처음 몇 개로 설정
            }

            while (true)
            {
                for (int i = 0; i < numCluster; i++)
                {
                    Distance.SetColumn(i, CalculateDistances(VectorScore, VectorCenter[i]));
                }
                Vector<double> tempLabel = Distance.MapRows(dist_row => dist_row.MinimumIndex());
                if (label.Equals(tempLabel)) break;
                label = tempLabel; // 레이블 업데이트
                VectorCenter = MeanOfCluster(VectorScore, label); // 클러스터 중심 업데이트
            }
            int[] intlabel = label.ToArray().Select(x => (int)x).ToArray();
            double[] doubleCener = VectorCenter.ToArray<double>();
            return (intlabel, ValidityScore(VectorScore, label, VectorCenter));
        }
        public static Vector<double> CalculateDistances(Vector<double> score, double center)
        {
            return (score - center).Map(Math.Abs);
        }
        public static Vector<double> MeanOfCluster(Vector<double> score, Vector<double> label )
        {

            var uniqueLabels = label.ToList().Distinct().ToList();
            var clusterMeans = Vector<double>.Build.Dense(uniqueLabels.Count);
            for (int i = 0; i < uniqueLabels.Count; i++)
            {
                var clusterIndices = label.Where(x => x == uniqueLabels[i]).ToList();
                var clusterScores = score.Rows(clusterIndices);
                clusterMeans[i] = clusterScores.Mean();
            }
            return clusterMeans;
        }
        public static double ValidityScore(
            Vector<double> VectorScore, 
            Vector<double> label, 
            Vector<double> VectorCenter
            )
        {
            var overallCenter = VectorScore.Mean();
            double overallSum = (VectorScore-overallCenter).Map(Math.Abs).Sum();

            var ClusterDistance = Vector<double>.Build.Dense(VectorCenter.Count);
            var Distance = Matrix<double>.Build.Dense(VectorScore.Count, VectorCenter.Count);
            for (int i = 0; i < VectorCenter.Count; i++)
            {
                Distance.SetColumn(i, CalculateDistances(VectorScore, VectorCenter[i]));
                ClusterDistance[i] = label.Where(x => x == i).Sum();
            }
            double clusterSum = ClusterDistance.Sum();

            return overallSum / clusterSum;
        }
    }
}
