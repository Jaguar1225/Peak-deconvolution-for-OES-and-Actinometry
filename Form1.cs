using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Peak_deconvolution_for_OES_and_Actinometry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private string _folderPath, _filePath, _dataPath, _graphPath;

        private List<Utills.Csvhandler.OESIntensityRecord> _records;
        private double _variance;
        private float[] _wavelengths;
        private double[,] _recordsMatrix;
        private double[] _timestamp, _scores;

        private DataLogger _rangeSignal;


        private void FolderPath_Click(object sender, EventArgs e)
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
        private void DataPath_Click(object sender, EventArgs e)
        {
            if (DataPathDialog.ShowDialog() == DialogResult.OK)
            {
                FolderPath.Text = DataPathDialog.SelectedPath;
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
        private void GraphPath_Click(object sender, EventArgs e)
        {
            if (GraphPathDialog.ShowDialog() == DialogResult.OK)
            {
                GraphPath.Text = GraphPathDialog.SelectedPath;
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

        private void LoadData_Click(object sender, EventArgs e)
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
            (_scores, _variance) = ML.PCA.Transform(_recordsMatrix); // Perform PCA transformation on the records matrix
            RangePlot_Update(_timestamp, _scores); // Initialize the range plot with the timestamp and scores
        }

        private void SaveData_Click(object sender, EventArgs e)
        {

        }
        private void SaveGraph_Click(object sender, EventArgs e)
        {

        }
        private void RangeFind_Click(object sender, EventArgs e)
        {

        }
        private void RangePlot_Update(double[] timestamp, double[] score)
        {
            try
            {
                if (RangePlot.InvokeRequired)
                {
                    RangePlot.Invoke(new Action(() => RangePlot_Update(timestamp, score)));
                    return;
                }
                RangePlot.Reset();
                var plt = RangePlot.Plot;
                plt.YLabel("Score");
                plt.XLabel("Time (s)");

                _rangeSignal = plt.Add.DataLogger();
                _rangeSignal.Clear();
                _rangeSignal.LineWidth = 2;
                _rangeSignal.MarkerSize = 1;

                for (int i = 0; i < timestamp.Length; i++)
                {
                    _rangeSignal.Add(timestamp[i], score[i]);
                }
                RangePlot.Refresh(); // Refresh the plot to show the updated data
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating range plot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Wavelength1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Wavelength1.Text))
            {
                MessageBox.Show("Please enter a value for Wavelength 1.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Wavelength2_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Wavelength2.Text))
            {
                MessageBox.Show("Please enter a value for Wavelength 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Deconvolution_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderPath.Text) || string.IsNullOrEmpty(DataPath.Text) || string.IsNullOrEmpty(GraphPath.Text) || string.IsNullOrEmpty(RangeFind.Text) || string.IsNullOrEmpty(Wavelength1.Text) || string.IsNullOrEmpty(Wavelength2.Text))
            {
                MessageBox.Show("Please fill in all fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Here you would add the logic to append the data based on the provided paths and wavelengths.
            MessageBox.Show("Data appended successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Condition_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Condition.Text))
            {
                MessageBox.Show("Please enter a value for Condition.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Append_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderPath.Text) || string.IsNullOrEmpty(DataPath.Text) || string.IsNullOrEmpty(GraphPath.Text) || string.IsNullOrEmpty(RangeFind.Text) || string.IsNullOrEmpty(Wavelength1.Text) || string.IsNullOrEmpty(Wavelength2.Text))
            {
                MessageBox.Show("Please fill in all fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Here you would add the logic to append the data based on the provided paths and wavelengths.
            MessageBox.Show("Data appended successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FolderPath.Text) || string.IsNullOrEmpty(DataPath.Text) || string.IsNullOrEmpty(GraphPath.Text) || string.IsNullOrEmpty(RangeFind.Text) || string.IsNullOrEmpty(Wavelength1.Text) || string.IsNullOrEmpty(Wavelength2.Text))
            {
                MessageBox.Show("Please fill in all fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Here you would add the logic to delete the data based on the provided paths and wavelengths.
            MessageBox.Show("Data deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void XLabel_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(XLabel.Text))
            {
                MessageBox.Show("Please enter a value for X Label.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void YLabel_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(YLabel.Text))
            {
                MessageBox.Show("Please enter a value for Y Label.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
