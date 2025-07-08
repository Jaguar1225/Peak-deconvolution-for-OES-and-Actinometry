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

        private void FolderPath_Click(object sender, EventArgs e)
        {
            if (FolderPathDialog.ShowDialog() == DialogResult.OK)
            {
                FolderPath.Text = FolderPathDialog.SelectedPath;
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
        private void SaveData_Click(object sender, EventArgs e)
        {

        }
        private void SaveGraph_Click(object sender, EventArgs e)
        {

        }
        private void RangeFind_Click(object sender, EventArgs e)
        {

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
