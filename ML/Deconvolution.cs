using System;
using System.Linq;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using Accord.MachineLearning;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    internal class Deconvolution
    {
        public static double PseudoVoigt(
            double x, double amplitude, double center, double sigma, double gamma, double baseline = 0)
        {   
            // Calculate the Gaussian and Lorentzian components
            double f = 0.5346 * gamma + Math.Sqrt(0.2166 * gamma * gamma + sigma * sigma);
            double eta = 1.36603 * (gamma/f) - 0.47719 * (gamma / f) * (gamma / f) + 0.11116 * (gamma / f) * (gamma / f) * (gamma / f);
            double gaussian = Math.Exp(-0.5 * Math.Pow((x - center) / sigma, 2));
            double lorentzian = gamma * gamma / (Math.Pow(x - center, 2) + gamma * gamma);

            // Combine them using eta
            return amplitude * (eta * lorentzian + (1 - eta) * gaussian) + baseline;
        }
        public static (double,double[]) VoigtOptimization(double[] x, double[] y, int peak_idx,int numClusters, double threshold = 0.5)
        {
            // Create and fit the Voigt model
            // Initialize the model parameters
            double[] initialAmplitudes = new double[numClusters];
            double[] initialCenters = new double[numClusters];
            double[] initialSigmas = new double[numClusters];
            double[] initialGammas = new double[numClusters];
            double[] initialBaseline = new double[1] { 300 }; // Assuming the baseline is the minimum intensity

            for (int i = 0; i < numClusters; i++)
            {
                int idx = x.Length / numClusters * i;
                initialAmplitudes[i] = y[idx]; // Assuming a small amplitude relative to the peak intensity
                initialCenters[i] = x[idx]; // Center at the peak position
                initialSigmas[i] = 50; // Assuming a small sigma relative to the intensity
                initialGammas[i] = 50; // Assuming a small gamma relative to the intensity
            }
            initialAmplitudes[numClusters/2] = y[peak_idx]; // Set the last amplitude to the peak intensity
            initialCenters[numClusters/2] = x[peak_idx]; // Set the last center to the peak position
            initialSigmas[numClusters/2] = (x[peak_idx+1] - x[peak_idx-1])/2.355; // Set the last sigma to a small value relative to the peak intensity
            initialGammas[numClusters/2] = (x[peak_idx+1] - x[peak_idx-1])/2; // Set the last gamma to a small value relative to the peak intensity

            var initialParameters = Vector<double>.Build.Dense(
                initialAmplitudes.Concat(initialCenters)
                .Concat(initialSigmas)
                .Concat(initialGammas)
                .Concat(initialBaseline)
                .ToArray());
            // Define the objective function to minimize
            Func<Vector<double>, double> objectiveFunction = parameters =>
            {
                for (int i = 0; i < numClusters; i++)
                {
                    if (parameters[i] < 0 || parameters[numClusters + i] < x.Min() || parameters[numClusters + i] > x.Max() ||
                        parameters[2 * numClusters + i] <= 0 || parameters[3 * numClusters + i] <= 0)
                    {
                        return double.MaxValue; // Return a large value if parameters are out of bounds
                    }
                }
                if (parameters[4 * numClusters] < 0 || parameters[4 * numClusters] > 500) // Baseline must be non-negative
                {
                    return double.MaxValue;
                }
                if ((Math.Abs(parameters[numClusters+numClusters/2] - x[peak_idx])>0.1)||
                    parameters[2 * numClusters + numClusters / 2] >= 20 || parameters[3 * numClusters + numClusters / 2] >= 20)
                {
                    return double.MaxValue; // Center must be within the range of x
                }
                double[] model = new double[x.Length];
                for (int i = 0; i < numClusters; i++)
                {
                    double amplitude = parameters[i];
                    double center = parameters[numClusters + i];
                    double sigma = parameters[2 * numClusters + i];
                    double gamma = parameters[3 * numClusters + i];
                    double baseline = parameters[4 * numClusters]; // Baseline is the last parameter
                    for (int j = 0; j < x.Length; j++)
                    {
                        model[j] += PseudoVoigt(x[j], amplitude, center, sigma, gamma);
                    }
                }
                model = model.Select(m => m + parameters[4 * numClusters]).ToArray(); // Add baseline to the model
                return Math.Sqrt(model.Zip(y, (m, t) => Math.Pow(m - t, 2)).Sum()); // RMSE
            };

            // Use a numerical optimizer to minimize the objective function
            var optimizer = new NelderMeadSimplex(1e-6, 30000); // Tolerance and maximum iterations
            var result = optimizer.FindMinimum(ObjectiveFunction.Value(objectiveFunction), initialParameters);
            
            return (result.FunctionInfoAtMinimum.Value, result.MinimizingPoint.ToArray()); // Return the optimized parameters and the minimum value of the objective function
        }
    }
}
