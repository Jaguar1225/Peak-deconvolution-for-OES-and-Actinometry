using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Extensions;
using ScottPlot;
using ScottPlot.Colormaps;
using ScottPlot.Finance;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Peak_deconvolution_for_OES_and_Actinometry
{
    public partial class Form1 : Form
    {   
        private string _folderPath, _filePath, _dataPath, _graphPath;

        private List<Utills.Csvhandler.OESIntensityRecord> _records;
        private double _variance;
        private int[] _label;
        private int _idx_wavelength_1, _idx_wavelength_2;
        private double[] _timestamp, _scores, _wavelengths, _w_profile_1, _profile_1,_w_profile_2, _profile_2;
        private double[] _conditions, _ratios;
        private Matrix<double> _recordsMatrix;

        private DataLogger _rangeSignal;
        public Form1()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(FolderPath.Text))
            {
                FileList.Items.Clear();
                try
                {
                    var csvFiles = System.IO.Directory.GetFiles(FolderPath.Text, "*.csv");
                    foreach (var file in csvFiles)
                    {
                        FileList.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                FileList.Sorted = true;
                FileList.SelectedIndex = 0;
            }
        }
        
        private async void FolderPath_Click(object sender, EventArgs e)
        {
            if (FolderPathDialog.ShowDialog() == DialogResult.OK)
            {
                FolderPath.Text = FolderPathDialog.SelectedPath;
                FileList.Items.Clear();
                try
                {
                    var csvFiles = System.IO.Directory.GetFiles(FolderPath.Text, "*.csv");
                    foreach (var file in csvFiles)
                    {
                        FileList.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                FileList.Sorted = true;
                FileList.SelectedIndex = 0;
                await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
            else
            {
                MessageBox.Show("No folder selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(FolderPath.Text))
            {
                MessageBox.Show("Please select a folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void DataPath_Click(object sender, EventArgs e)
        {
            if (DataPathDialog.ShowDialog() == DialogResult.OK)
            {
                DataPath.Text = DataPathDialog.SelectedPath;
                await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
            else
            {
                MessageBox.Show("No folder selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(FolderPath.Text))
            {
                MessageBox.Show("Please select a folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private async void GraphPath_Click(object sender, EventArgs e)
        {
            if (GraphPathDialog.ShowDialog() == DialogResult.OK)
            {
                GraphPath.Text = GraphPathDialog.SelectedPath;
                await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
            else
            {
                MessageBox.Show("No folder selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(GraphPath.Text))
            {
                MessageBox.Show("Please select a folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderPath.Text))
            {
                MessageBox.Show("Please fill in all fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (FileList.SelectedItem == null)
            {
                MessageBox.Show("Please select a file from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _folderPath = FolderPath.Text;
            _filePath = System.IO.Path.Combine(_folderPath, FileList.SelectedItem.ToString() + ".csv");
            // Here you would add the logic to load the data based on the provided paths.
            (_wavelengths, _records) = Utills.Csvhandler.ReadCsv(_filePath, ','); // Example usage of the Csvhandler to read data
            (_timestamp, _recordsMatrix) = Utills.Csvhandler.RecordsToMatrix(_records, _wavelengths.Length); // Convert records to matrix format
            _records?.Clear(); // Clear the records list to free up memory
            (_scores, _variance) = ML.PCA.Transform(_recordsMatrix); // Perform PCA transformation on the records matrix
            var init_label = new int[_scores.Length]; // Initialize the label array for clustering
            await RangePlot_Update(_timestamp, _scores, init_label); // Initialize the range plot with the timestamp and scores
        }
        private async void SaveData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DataPath.Text))
            {
                MessageBox.Show("Please fill in all fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _dataPath = DataPath.Text;
            string fileName = FileList.SelectedItem.ToString() + "_data.csv"; // Create a file name for the data
            string fullPath = System.IO.Path.Combine(_dataPath, fileName);
            try
            {
                List<string> data = new List<string>();
                string xlabel, ylabel;
                if (XLabel.Text != string.Empty)
                {
                    xlabel = XLabel.Text; // Add the X-axis label to the CSV header
                }
                else
                {
                    xlabel = "Condition"; // Default X-axis label
                };

                if (YLabel.Text != string.Empty)
                {
                    ylabel = YLabel.Text; // Add the Y-axis label to the CSV header
                }
                else
                {
                    ylabel = "Ratio"; // Default Y-axis label
                };

                data.Add($"{xlabel},{ylabel}"); // Add the header to the CSV file

                for (int i = 0; i < _conditions.Length; i++)
                {
                    data.Add($"{_conditions[i]},{_ratios[i]}"); // Add each record with timestamp
                }
                Utills.Csvhandler.WriteCsv(fullPath, data); // Write the data to a CSV file
                MessageBox.Show($"Data saved successfully at {fullPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.CompletedTask; // Placeholder for any save logic you might want to implement
            }
        }
        private async void SaveGraph_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GraphPath.Text) || string.IsNullOrEmpty(FolderPath.Text) || FileList.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _graphPath = GraphPath.Text;
            string fileName = FileList.SelectedItem.ToString() + "_graph.png"; // Create a file name for the graph
            string fullPath = System.IO.Path.Combine(_graphPath, fileName);
            try 
            {
                Result.Plot.SavePng(fullPath, width: 600, height:600); // Save the Result plot as an image
                await Task.CompletedTask; // Placeholder for any save logic you might want to implement
                MessageBox.Show($"Graph saved successfully at {fullPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving graph: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void RangeFind_Click(object sender, EventArgs e)
        {
            int numCond;
            if (int.TryParse(NoCond.Text, out numCond))
            {
                double validity;
                (_label, validity) = ML.Clustering.KMC(_scores, int.Parse(NoCond.Text));
                await RangePlot_Update(_timestamp, _scores, _label); // Update the range plot with the new labels
                NoCluster.Text = 1.ToString(); // Update the number of clusters found
                double[] p1 = new double[5]{
                    1,
                    double.Parse(Wavelength1.Text),
                    1,
                    1,
                    0
                };
                double[] p2 = new double[5]{
                    1,
                    double.Parse(Wavelength2.Text),
                    1,
                    1,
                    0
                };  
                await Task.WhenAll(OesPlot_Update(), Profile1_Update(p1), Profile2_Update(p2)); // Update the profiles based on the new labels
            }
        }
        private async Task RangePlot_Update(double[] timestamp, double[] score, int[] label)
        {
            try
            {
                if (RangePlot.InvokeRequired)
                    {
                        RangePlot.Invoke(new Action(async () => await RangePlot_Update(timestamp, score, label)));
                        return;
                    }
                RangePlot.Reset();
                    var plt = RangePlot.Plot;
                    plt.YLabel("Score");
                    plt.XLabel("Time (s)");
                    System.Drawing.Color[] clusterColors = new System.Drawing.Color[]
                    {
                   System.Drawing.Color.Red, System.Drawing.Color.Green, System.Drawing.Color.Blue,
                     System.Drawing.Color.Yellow, System.Drawing.Color.Cyan, System.Drawing.Color.Magenta
                    };

                    var NumOfCluster = label.Distinct().Count();

                    for (int cluster = 0; cluster < NumOfCluster; cluster++)
                    {
                        int start = -2;
                        int End = -1; // Initialize End to -1
                        bool SegDetect = false;
                        for (int i = 0; i < label.Length; i++)
                        {
                            if (start < End)
                            {
                                if (i == 0)
                                {
                                    if ((label[i] == cluster))
                                    {
                                        start = i;
                                    }
                                    continue;
                                }
                                if ((label[i] == cluster) && (label[i - 1] != cluster))
                                {
                                    start = i; // Start of a new segment
                                    continue; // Skip to the next iteration
                                }
                            }
                            if (i == label.Length - 1 && label[i] == cluster)
                            {
                                End = i; // End of the last segment
                                SegDetect = true;
                            }
                            else if ((label[i] != cluster) && (label[i - 1] == cluster))
                            {
                                End = i - 1; // End of the current segment
                                SegDetect = true;
                            }

                            int length = End - start;

                            if ((length > 0) && SegDetect)
                            {
                                double[] x = new double[length];
                                double[] y = new double[length];
                                Array.Copy(timestamp, start, x, 0, length);
                                Array.Copy(score, start, y, 0, length);
                                var sig = plt.Add.Scatter(
                                    x, y,
                                    color: ScottPlot.Color.FromColor(clusterColors[cluster % clusterColors.Length])
                                    );
                                sig.LineWidth = 2; // Set line width for better visibility
                                sig.MarkerSize = 3; // Set marker size for better visibility
                                SegDetect = false;
                            }
                        }

                    }
                    RangePlot.Refresh(); // Refresh the plot to show the updated data
                    await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating range plot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
        }
        private async void NoCluster_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(NoCluster.Text))
                {
                    MessageBox.Show("Please enter a value for No. Cluster.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrEmpty(Wavelength1.Text) || string.IsNullOrEmpty(Wavelength2.Text))
                {
                    MessageBox.Show("Please enter values for Wavelength 1 and Wavelength 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double[] p1 = new double[5]{
                1,
                double.Parse(Wavelength1.Text),
                1,
                1,
                0
                };
                double[] p2 = new double[5]{
                1,
                double.Parse(Wavelength2.Text),
                1,
                1,
                0
                };
                await Task.WhenAll(OesPlot_Update(), Profile1_Update(p1), Profile2_Update(p2));       
            }
        }
        private async Task OesPlot_Update()
        {

                try
                {
                    if (OesPlot.InvokeRequired)
                    {
                        OesPlot.Invoke(new Action(async () => await OesPlot_Update()));
                        return;
                    }
                    OesPlot.Reset();
                    var plt = OesPlot.Plot;
                    plt.Clear(); // Clear the previous plot
                    plt.XLabel("Wavelength (nm)");
                    plt.YLabel("Intensity");
                    plt.Title("OES Intensity Plot");
                    var clusterIndicies = _label
                        .Select((value, index) => new { value, index })
                        .Where(x => x.value == int.Parse(NoCluster.Text) - 1)
                        .Select(x => x.index)
                        .ToList();
                    if (clusterIndicies.Count == 0)
                    {
                        MessageBox.Show("No data available for the selected cluster.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var clusterCenter = _recordsMatrix.Rows(clusterIndicies).Mean().ToArray(); // Filter the records matrix based on the selected cluster
                    var sig = plt.Add.ScatterLine(_wavelengths, clusterCenter, color: ScottPlot.Color.FromColor(System.Drawing.Color.Blue));
                    sig.MarkerSize = 3; // Set marker size for better visibility
                    sig.LineWidth = 2; // Set line width for better visibility

                    OesPlot.Refresh(); // Refresh the plot to show the updated data
                    await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating OES plot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        private async void Wavelength1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(Wavelength1.Text))
                {
                    MessageBox.Show("Please enter a value for Wavelength 1.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double[] p = new double[3]{
                    1,
                    double.Parse(Wavelength1.Text),
                    1
                };
                await Profile1_Update(p);
            }
        }
        private async Task Profile1_Update(double[] p)
        {
            try
            {
                if (Profile1.InvokeRequired)
                {
                    Profile1.Invoke(new Action(async () => await Profile1_Update(p)));
                    return;
                }
                Profile1.Reset();
                var plt = Profile1.Plot;
                plt.Clear(); // Clear the previous plot
                plt.XLabel("Wavelength (nm)");
                plt.YLabel("Intensity");
                var indicies = _label
                    .Select((value, index) => new { value, index })
                    .Where(x => x.value == int.Parse(NoCluster.Text) - 1)
                    .Select(x => x.index)
                    .ToList();
                _label.Where(l => l == (int.Parse(NoCluster.Text) - 1)).ToList();
                var clusterCenter = _recordsMatrix.Rows(indicies).Mean().ToArray(); // Filter the records matrix based on the selected cluster

                _w_profile_1 = new double[20];
                _profile_1 = new double[20];
                _idx_wavelength_1 = ML.PeakFinder.FindPeak(_wavelengths, clusterCenter, double.Parse(Wavelength1.Text));

                Array.Copy(_wavelengths, _idx_wavelength_1 - 10, _w_profile_1, 0, 20); // Copy the first 30 wavelengths
                Array.Copy(clusterCenter, _idx_wavelength_1 - 10, _profile_1, 0, 20); // Copy the first 30 intensities from the cluster center
                var sig = plt.Add.Scatter(
                    _w_profile_1, _profile_1,
                    color: ScottPlot.Color.FromColor(System.Drawing.Color.Blue) // Set the color for the plot line
                );
                var sig_baseLine = plt.Add.HorizontalLine(
                    p[4],
                    color: ScottPlot.Color.FromColor(System.Drawing.Color.Green)
                ); // Add a horizontal line for the baseline
                double[] _w_profile_range = new double[_w_profile_1.Length*3];
                double start = _w_profile_1.Min();
                double end = _w_profile_1.Max();
                for (int i = 0; i < _w_profile_range.Length; i++)
                {
                    _w_profile_range[i] = start + (end - start) * i / (_w_profile_range.Length - 1); // Create a range of wavelengths for the profile
                }
                var decomposedProfile = _w_profile_range
                    .Select(x => ML.Deconvolution.PseudoVoigt(
                        x, p[0], p[1], p[2], p[3], p[4])) // Decompose the profile using the PseudoVoigt function
                    .ToArray(); // Decompose the profile by subtracting the baseline
                var sig_decomposed = plt.Add.Scatter(
                    _w_profile_range, decomposedProfile, // Decompose the profile by subtracting the baseline
                    color: ScottPlot.Color.FromColor(System.Drawing.Color.Red) // Set the color for the decomposed profile
                );
                sig.MarkerSize = 3; // Set marker size for better visibility
                sig.LineWidth = 2; // Set line width for better visibility
                Profile1.Refresh(); // Refresh the plot to show the updated data
                await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile 1 plot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void Wavelength2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(Wavelength2.Text))
                {
                    MessageBox.Show("Please enter a value for Wavelength 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double[] p = new double[3]{
                    1,
                    double.Parse(Wavelength2.Text),
                    1
                };
                await Profile2_Update(p);
            }
        }
        private async Task Profile2_Update(double[] p)
        {
            if (string.IsNullOrEmpty(Wavelength2.Text))
            {
                MessageBox.Show("Please enter a value for Profile 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(NoCluster.Text))
            {
                MessageBox.Show("Please enter a value for No. Cluster.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                try
                    {
                        if (Profile2.InvokeRequired)
                        {
                            Profile1.Invoke(new Action(async () => await Profile2_Update(p)));
                            return;
                        }
                        Profile2.Reset();
                        var plt = Profile2.Plot;
                        plt.Clear(); // Clear the previous plot
                        plt.XLabel("Wavelength (nm)");
                        plt.YLabel("Intensity");

                        var indicies = _label
                            .Select((value, index) => new { value, index })
                            .Where(x => x.value == int.Parse(NoCluster.Text) - 1)
                            .Select(x => x.index)
                            .ToList(); var clusterCenter = _recordsMatrix.Rows(indicies).Mean().ToArray(); // Filter the records matrix based on the selected cluster
                        _w_profile_2 = new double[20];
                        _profile_2 = new double[20];
                        _idx_wavelength_2 = ML.PeakFinder.FindPeak(_wavelengths, clusterCenter, double.Parse(Wavelength2.Text));

                        Array.Copy(_wavelengths, _idx_wavelength_2 - 10, _w_profile_2, 0, 20); // Copy the first 30 wavelengths
                        Array.Copy(clusterCenter, _idx_wavelength_2 - 10, _profile_2, 0, 20); // Copy the first 30 intensities from the cluster center
                        var sig = plt.Add.Scatter(
                            _w_profile_2, _profile_2,
                            color: ScottPlot.Color.FromColor(System.Drawing.Color.Blue) // Set the color for the plot line
                        );
                        double[] _w_profile_range = new double[_w_profile_2.Length * 3];
                        double start = _w_profile_2.Min();
                        double end = _w_profile_2.Max();
                        for (int i = 0; i < _w_profile_range.Length; i++)
                        {
                            _w_profile_range[i] = start + (end - start) * i / (_w_profile_range.Length - 1); // Create a range of wavelengths for the profile
                        }
                        var decomposedProfile = _w_profile_range
                            .Select(x => ML.Deconvolution.PseudoVoigt(
                            x, p[0], p[1], p[2], p[3], p[4])) // Decompose the profile using the PseudoVoigt function
                            .ToArray(); // Decompose the profile by subtracting the baseline
                        var sig_decomposed = plt.Add.Scatter(
                            _w_profile_range, decomposedProfile, // Decompose the profile by subtracting the baseline
                            color: ScottPlot.Color.FromColor(System.Drawing.Color.Red) // Set the color for the decomposed profile
                        );
                        sig.MarkerSize = 3; // Set marker size for better visibility
                        sig.LineWidth = 2; // Set line width for better visibility
                        Profile2.Refresh(); // Refresh the plot to show the updated data
                        await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating profile 1 plot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
        }
        private async void Deconvolution_Click(object sender, EventArgs e)
        {
            try
            {
                int profiles = 5;
                double[] best_result_1 = new double[profiles*4+1]; // Initialize best result for profile 1
                double[] best_result_2 = new double[profiles*4+1]; // Initialize arrays to hold the best results for both profiles
                var value_1 = 100000.0; // Initialize value for profile 1
                var value_2 = 100000.0; // Initialize value for profile 2
                int best_Num_Components_1 = 0; // Initialize best number of components for profile 1
                int best_Num_Components_2 = 0; // Initialize best number of components for profile 2
                // Here you would add the logic to append the data based on the provided paths and wavelengths.
                for (int i = 0; i < profiles; i++)
                {
                    var (temp_value_1, result_1) = ML.Deconvolution.VoigtOptimization(_w_profile_1, _profile_1, 11,i+1); // Perform Gaussian Mixture Model deconvolution
                    var (temp_value_2, result_2) = ML.Deconvolution.VoigtOptimization(_w_profile_2, _profile_2, 11,i+1); // Perform Gaussian Mixture Model deconvolution
                    if (temp_value_1<value_1)
                    {
                        best_Num_Components_1 = i + 1;
                        value_1 = temp_value_1;

                        for (int j = 0; j < (i+1); j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                best_result_1[j + profiles*k] = result_1[j + k * (i + 1)]; // Store the best result for profile 1
                            }
                        }
                        best_result_1[profiles*4] = result_1[(i + 1) * 4];
                    }
                    if (temp_value_2<value_2)
                    {
                        best_Num_Components_2 = i + 1;
                        value_2 = temp_value_2;

                        for (int j = 0; j < (i + 1); j++)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                best_result_2[j + profiles * k] = result_2[j + k * (i + 1)]; // Store the best result for profile 1
                            }
                        }
                        best_result_2[profiles*4] = result_2[(i + 1) * 4];
                    }   
                }
                int findPeak_1 = best_Num_Components_1/2, findPeak_2 = best_Num_Components_2/2;

                // Calculat area of Voigt profile using the formula: Area = Amplitude * (sigma * sqrt(2 * pi) + gamma)
                var _area_1 = best_result_1[findPeak_1] * 
                    (best_result_1[2 * best_Num_Components_1 + findPeak_1] * Math.Sqrt(2 * Math.PI) +
                    best_result_1[profiles * best_Num_Components_1 + findPeak_1]); // Area for profile 1
                var _area_2 = best_result_2[findPeak_2] * 
                    (best_result_2[2 * best_Num_Components_2 + findPeak_2] * Math.Sqrt(2 * Math.PI) +
                    best_result_2[profiles * best_Num_Components_2 + findPeak_2]); // Area for profile 2

                Area1.Text = _area_1.ToString();
                Area2.Text = _area_2.ToString();
                Ratio.Text = (_area_1 / _area_2).ToString();

                double[] p1 = new double[5]{
                    best_result_1[findPeak_1],
                    best_result_1[profiles+ findPeak_1],
                    best_result_1[2*profiles + findPeak_1],
                    best_result_1[3*profiles + findPeak_1],
                    best_result_1[4*profiles],
                };
                double[] p2 = new double[5]{
                    best_result_2[findPeak_2],
                    best_result_2[profiles+ findPeak_2],
                    best_result_2[2*profiles + findPeak_2],
                    best_result_2[3*profiles + findPeak_2],
                    best_result_2[4*profiles],
                };
                // Update the profiles with the deconvolved parameters

                await Task.WhenAll(
                    Profile1_Update(p1), // Update profile 1 with the deconvolved parameters
                    Profile2_Update(p2) // Update profile 2 with the deconvolved parameters
                );

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during deconvolution: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void Condition_TextChanged(object sender, EventArgs e)
        {
            await Task.CompletedTask;
        }
        private async void Append_Click(object sender, EventArgs e)
        {
            if (_conditions == null || _conditions.Length == 0)
            {
                _conditions = new double[] { double.Parse(Condition.Text) }; // Initialize conditions if not already set
                _ratios = new double[] { double.Parse(Ratio.Text) };
            }
            else if (_conditions.Contains(double.Parse(Condition.Text)))
            {
                _ratios[Array.IndexOf(_conditions, double.Parse(Condition.Text))] = double.Parse(Ratio.Text); // Update the ratio if condition already exists
            }
            else
            {
                Array.Resize(ref _conditions, _conditions.Length + 1); // Resize the array to add a new condition
                _conditions[_conditions.Length - 1] = double.Parse(Condition.Text); // Add the new condition

                Array.Resize(ref _ratios, _ratios.Length + 1); // Resize the array to add a new ratio
                _ratios[_ratios.Length - 1] = double.Parse(Ratio.Text); // Add the new ratio
                
                //Sort the ratios into ascending order to _conditions
                Array.Sort(_conditions, _conditions); // Sort the ratios and conditions together
                Array.Sort(_conditions, _ratios); // Sort the ratios and conditions together
            }
            await Result_Update();
        }
        private async Task Result_Update()
        {
            try
            {
                if (Result.InvokeRequired)
                {
                    Result.Invoke(new Action(async () => await Result_Update()));
                    return;
                }
                Result.Reset();
                var plt = Result.Plot;
                plt.Clear(); // Clear the previous plot
                if (string.IsNullOrEmpty(XLabel.Text))
                {
                    plt.XLabel("Condition", size: 25);
                }
                else
                {
                    plt.XLabel(XLabel.Text, size: 25); // Set X label from user input
                }
                if (string.IsNullOrEmpty(YLabel.Text))
                {
                    plt.YLabel("Ratio", size: 25);
                }
                else
                {
                    plt.YLabel(YLabel.Text, size: 25); // Set Y label from user input
                }
                plt.Axes.FrameWidth(3); // Set frame width for better visibility
                plt.Axes.SetLimitsX(_conditions.Min()*0.9,_conditions.Max()*1.1);
                plt.Axes.SetLimitsY(_ratios.Min()*0.9, _ratios.Max()*1.1);
                if (_conditions != null && _ratios != null && _conditions.Length > 0 && _ratios.Length > 0)
                {
                    var sig = plt.Add.Scatter(_conditions, _ratios, color: ScottPlot.Color.FromColor(System.Drawing.Color.Blue)); // Add a scatter line plot for conditions vs ratios
                    sig.MarkerSize = 3; // Set marker size for better visibility
                    sig.LineWidth = 3; // Set line width for better visibility
                }
                Result.Refresh(); // Refresh the plot to show the updated data
                await Task.CompletedTask; // Placeholder for any asynchronous operation you might want to perform
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating result plot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void XLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(XLabel.Text))
                {
                    MessageBox.Show("Please enter a value for X Label.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await Result_Update(); // Update the result plot with the new X label
            }
        }
        private async void YLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(YLabel.Text))
                {
                    MessageBox.Show("Please enter a value for Y Label.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                await Result_Update(); // Update the result plot with the new Y label
            }
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            if (_conditions == null || _conditions.Length == 0)
            {
                MessageBox.Show("No conditions to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(Condition.Text) || !double.TryParse(Condition.Text, out double conditionToDelete))
            {
                MessageBox.Show("Please enter a valid condition to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_conditions.Contains(conditionToDelete))
            {
                MessageBox.Show("Condition not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int index = Array.IndexOf(_conditions, conditionToDelete); // Find the index of the condition to delete
            if (index >= 0)
            {
                // Remove the condition and corresponding ratio
                _conditions = _conditions.Where((val, idx) => idx != index).ToArray();
                _ratios = _ratios.Where((val, idx) => idx != index).ToArray();
                await Result_Update(); // Update the result plot after deletion
            }
            else
            {
                MessageBox.Show("Condition not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
