namespace Peak_deconvolution_for_OES_and_Actinometry
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.FolderPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.FolderPath = new System.Windows.Forms.TextBox();
            this.FolderPathLabel = new System.Windows.Forms.Label();
            this.Result = new ScottPlot.WinForms.FormsPlot();
            this.Profile1 = new ScottPlot.WinForms.FormsPlot();
            this.ProfileLabel1 = new System.Windows.Forms.Label();
            this.GraphLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.TextBox();
            this.YLabelText = new System.Windows.Forms.Label();
            this.XLabelText = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.TextBox();
            this.Append = new System.Windows.Forms.Button();
            this.Deconvolution = new System.Windows.Forms.Button();
            this.SaveGraph = new System.Windows.Forms.Button();
            this.SaveData = new System.Windows.Forms.Button();
            this.RangeFind = new System.Windows.Forms.Button();
            this.RangePlot = new ScottPlot.WinForms.FormsPlot();
            this.Wavelength1 = new System.Windows.Forms.TextBox();
            this.ProfileLabel2 = new System.Windows.Forms.Label();
            this.Profile2 = new ScottPlot.WinForms.FormsPlot();
            this.Wavelength2 = new System.Windows.Forms.TextBox();
            this.Area1 = new System.Windows.Forms.TextBox();
            this.Area2 = new System.Windows.Forms.TextBox();
            this.GraphPath = new System.Windows.Forms.TextBox();
            this.SaveGraphLabel = new System.Windows.Forms.Label();
            this.SaveDataLabel = new System.Windows.Forms.Label();
            this.DataPath = new System.Windows.Forms.TextBox();
            this.RatioLabel = new System.Windows.Forms.Label();
            this.AreaLabel1 = new System.Windows.Forms.Label();
            this.AreaLabel2 = new System.Windows.Forms.Label();
            this.Ratio = new System.Windows.Forms.TextBox();
            this.ConditionLabel = new System.Windows.Forms.Label();
            this.Condition = new System.Windows.Forms.TextBox();
            this.GraphPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.DataPathDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.Delete = new System.Windows.Forms.Button();
            this.LoadData = new System.Windows.Forms.Button();
            this.FileList = new System.Windows.Forms.ListBox();
            this.Eps = new System.Windows.Forms.TextBox();
            this.EpsLabel = new System.Windows.Forms.Label();
            this.NoClusterLabel = new System.Windows.Forms.Label();
            this.NoCluster = new System.Windows.Forms.TextBox();
            this.OesPlot = new ScottPlot.WinForms.FormsPlot();
            this.SuspendLayout();
            // 
            // FolderPath
            // 
            this.FolderPath.Location = new System.Drawing.Point(84, 12);
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Size = new System.Drawing.Size(218, 21);
            this.FolderPath.TabIndex = 0;
            this.FolderPath.Text = "C:\\Users\\User\\Desktop\\Test";
            this.FolderPath.Click += new System.EventHandler(this.FolderPath_Click);
            // 
            // FolderPathLabel
            // 
            this.FolderPathLabel.AutoSize = true;
            this.FolderPathLabel.Location = new System.Drawing.Point(10, 15);
            this.FolderPathLabel.Name = "FolderPathLabel";
            this.FolderPathLabel.Size = new System.Drawing.Size(68, 12);
            this.FolderPathLabel.TabIndex = 1;
            this.FolderPathLabel.Text = "Folder path";
            // 
            // Result
            // 
            this.Result.DisplayScale = 0F;
            this.Result.Location = new System.Drawing.Point(586, 176);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(414, 327);
            this.Result.TabIndex = 2;
            // 
            // Profile1
            // 
            this.Profile1.DisplayScale = 0F;
            this.Profile1.Location = new System.Drawing.Point(8, 315);
            this.Profile1.Name = "Profile1";
            this.Profile1.Size = new System.Drawing.Size(278, 188);
            this.Profile1.TabIndex = 3;
            // 
            // ProfileLabel1
            // 
            this.ProfileLabel1.AutoSize = true;
            this.ProfileLabel1.Location = new System.Drawing.Point(15, 291);
            this.ProfileLabel1.Name = "ProfileLabel1";
            this.ProfileLabel1.Size = new System.Drawing.Size(84, 12);
            this.ProfileLabel1.TabIndex = 4;
            this.ProfileLabel1.Text = "Current profile";
            // 
            // GraphLabel
            // 
            this.GraphLabel.AutoSize = true;
            this.GraphLabel.Location = new System.Drawing.Point(595, 161);
            this.GraphLabel.Name = "GraphLabel";
            this.GraphLabel.Size = new System.Drawing.Size(39, 12);
            this.GraphLabel.TabIndex = 5;
            this.GraphLabel.Text = "Graph";
            // 
            // YLabel
            // 
            this.YLabel.Location = new System.Drawing.Point(633, 139);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(218, 21);
            this.YLabel.TabIndex = 6;
            this.YLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.YLabel_KeyDown);
            // 
            // YLabelText
            // 
            this.YLabelText.AutoSize = true;
            this.YLabelText.Location = new System.Drawing.Point(588, 142);
            this.YLabelText.Name = "YLabelText";
            this.YLabelText.Size = new System.Drawing.Size(44, 12);
            this.YLabelText.TabIndex = 7;
            this.YLabelText.Text = "Y label";
            // 
            // XLabelText
            // 
            this.XLabelText.AutoSize = true;
            this.XLabelText.Location = new System.Drawing.Point(588, 115);
            this.XLabelText.Name = "XLabelText";
            this.XLabelText.Size = new System.Drawing.Size(44, 12);
            this.XLabelText.TabIndex = 8;
            this.XLabelText.Text = "X label";
            // 
            // XLabel
            // 
            this.XLabel.Location = new System.Drawing.Point(633, 112);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(218, 21);
            this.XLabel.TabIndex = 9;
            this.XLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.XLabel_KeyDown);
            // 
            // Append
            // 
            this.Append.Location = new System.Drawing.Point(156, 253);
            this.Append.Name = "Append";
            this.Append.Size = new System.Drawing.Size(136, 31);
            this.Append.TabIndex = 10;
            this.Append.Text = "Append";
            this.Append.UseVisualStyleBackColor = true;
            this.Append.Click += new System.EventHandler(this.Append_Click);
            // 
            // Deconvolution
            // 
            this.Deconvolution.Location = new System.Drawing.Point(14, 253);
            this.Deconvolution.Name = "Deconvolution";
            this.Deconvolution.Size = new System.Drawing.Size(136, 31);
            this.Deconvolution.TabIndex = 11;
            this.Deconvolution.Text = "Deconvolution";
            this.Deconvolution.UseVisualStyleBackColor = true;
            this.Deconvolution.Click += new System.EventHandler(this.Deconvolution_Click);
            // 
            // SaveGraph
            // 
            this.SaveGraph.Location = new System.Drawing.Point(722, 75);
            this.SaveGraph.Name = "SaveGraph";
            this.SaveGraph.Size = new System.Drawing.Size(136, 31);
            this.SaveGraph.TabIndex = 12;
            this.SaveGraph.Text = "Save graph";
            this.SaveGraph.UseVisualStyleBackColor = true;
            this.SaveGraph.Click += new System.EventHandler(this.SaveGraph_Click);
            // 
            // SaveData
            // 
            this.SaveData.Location = new System.Drawing.Point(858, 75);
            this.SaveData.Name = "SaveData";
            this.SaveData.Size = new System.Drawing.Size(136, 31);
            this.SaveData.TabIndex = 13;
            this.SaveData.Text = "Save data";
            this.SaveData.UseVisualStyleBackColor = true;
            this.SaveData.Click += new System.EventHandler(this.SaveData_Click);
            // 
            // RangeFind
            // 
            this.RangeFind.Location = new System.Drawing.Point(324, 34);
            this.RangeFind.Name = "RangeFind";
            this.RangeFind.Size = new System.Drawing.Size(100, 31);
            this.RangeFind.TabIndex = 14;
            this.RangeFind.Text = "Range find";
            this.RangeFind.UseVisualStyleBackColor = true;
            this.RangeFind.Click += new System.EventHandler(this.RangeFind_Click);
            // 
            // RangePlot
            // 
            this.RangePlot.DisplayScale = 0F;
            this.RangePlot.Location = new System.Drawing.Point(12, 69);
            this.RangePlot.Name = "RangePlot";
            this.RangePlot.Size = new System.Drawing.Size(274, 178);
            this.RangePlot.TabIndex = 15;
            // 
            // Wavelength1
            // 
            this.Wavelength1.Location = new System.Drawing.Point(105, 288);
            this.Wavelength1.Name = "Wavelength1";
            this.Wavelength1.Size = new System.Drawing.Size(57, 21);
            this.Wavelength1.TabIndex = 16;
            this.Wavelength1.Text = "703";
            this.Wavelength1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Wavelength1_KeyDown);
            // 
            // ProfileLabel2
            // 
            this.ProfileLabel2.AutoSize = true;
            this.ProfileLabel2.Location = new System.Drawing.Point(267, 291);
            this.ProfileLabel2.Name = "ProfileLabel2";
            this.ProfileLabel2.Size = new System.Drawing.Size(84, 12);
            this.ProfileLabel2.TabIndex = 17;
            this.ProfileLabel2.Text = "Current profile";
            // 
            // Profile2
            // 
            this.Profile2.DisplayScale = 0F;
            this.Profile2.Location = new System.Drawing.Point(292, 315);
            this.Profile2.Name = "Profile2";
            this.Profile2.Size = new System.Drawing.Size(278, 188);
            this.Profile2.TabIndex = 18;
            // 
            // Wavelength2
            // 
            this.Wavelength2.Location = new System.Drawing.Point(361, 288);
            this.Wavelength2.Name = "Wavelength2";
            this.Wavelength2.Size = new System.Drawing.Size(57, 21);
            this.Wavelength2.TabIndex = 19;
            this.Wavelength2.Text = "750";
            this.Wavelength2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Wavelength2_KeyDown);
            // 
            // Area1
            // 
            this.Area1.Location = new System.Drawing.Point(204, 288);
            this.Area1.Name = "Area1";
            this.Area1.Size = new System.Drawing.Size(57, 21);
            this.Area1.TabIndex = 20;
            // 
            // Area2
            // 
            this.Area2.Location = new System.Drawing.Point(459, 288);
            this.Area2.Name = "Area2";
            this.Area2.Size = new System.Drawing.Size(67, 21);
            this.Area2.TabIndex = 21;
            // 
            // GraphPath
            // 
            this.GraphPath.Location = new System.Drawing.Point(383, 12);
            this.GraphPath.Name = "GraphPath";
            this.GraphPath.Size = new System.Drawing.Size(218, 21);
            this.GraphPath.TabIndex = 22;
            this.GraphPath.Click += new System.EventHandler(this.GraphPath_Click);
            // 
            // SaveGraphLabel
            // 
            this.SaveGraphLabel.AutoSize = true;
            this.SaveGraphLabel.Location = new System.Drawing.Point(308, 15);
            this.SaveGraphLabel.Name = "SaveGraphLabel";
            this.SaveGraphLabel.Size = new System.Drawing.Size(69, 12);
            this.SaveGraphLabel.TabIndex = 23;
            this.SaveGraphLabel.Text = "Save graph";
            // 
            // SaveDataLabel
            // 
            this.SaveDataLabel.AutoSize = true;
            this.SaveDataLabel.Location = new System.Drawing.Point(606, 15);
            this.SaveDataLabel.Name = "SaveDataLabel";
            this.SaveDataLabel.Size = new System.Drawing.Size(61, 12);
            this.SaveDataLabel.TabIndex = 25;
            this.SaveDataLabel.Text = "Save data";
            // 
            // DataPath
            // 
            this.DataPath.Location = new System.Drawing.Point(673, 12);
            this.DataPath.Name = "DataPath";
            this.DataPath.Size = new System.Drawing.Size(218, 21);
            this.DataPath.TabIndex = 24;
            this.DataPath.Click += new System.EventHandler(this.DataPath_Click);
            // 
            // RatioLabel
            // 
            this.RatioLabel.AutoSize = true;
            this.RatioLabel.Location = new System.Drawing.Point(424, 262);
            this.RatioLabel.Name = "RatioLabel";
            this.RatioLabel.Size = new System.Drawing.Size(33, 12);
            this.RatioLabel.TabIndex = 26;
            this.RatioLabel.Text = "Ratio";
            // 
            // AreaLabel1
            // 
            this.AreaLabel1.AutoSize = true;
            this.AreaLabel1.Location = new System.Drawing.Point(169, 291);
            this.AreaLabel1.Name = "AreaLabel1";
            this.AreaLabel1.Size = new System.Drawing.Size(31, 12);
            this.AreaLabel1.TabIndex = 27;
            this.AreaLabel1.Text = "Area";
            // 
            // AreaLabel2
            // 
            this.AreaLabel2.AutoSize = true;
            this.AreaLabel2.Location = new System.Drawing.Point(424, 291);
            this.AreaLabel2.Name = "AreaLabel2";
            this.AreaLabel2.Size = new System.Drawing.Size(31, 12);
            this.AreaLabel2.TabIndex = 28;
            this.AreaLabel2.Text = "Area";
            // 
            // Ratio
            // 
            this.Ratio.Location = new System.Drawing.Point(459, 259);
            this.Ratio.Name = "Ratio";
            this.Ratio.Size = new System.Drawing.Size(67, 21);
            this.Ratio.TabIndex = 29;
            // 
            // ConditionLabel
            // 
            this.ConditionLabel.AutoSize = true;
            this.ConditionLabel.Location = new System.Drawing.Point(298, 262);
            this.ConditionLabel.Name = "ConditionLabel";
            this.ConditionLabel.Size = new System.Drawing.Size(58, 12);
            this.ConditionLabel.TabIndex = 30;
            this.ConditionLabel.Text = "Condition";
            // 
            // Condition
            // 
            this.Condition.Location = new System.Drawing.Point(362, 259);
            this.Condition.Name = "Condition";
            this.Condition.Size = new System.Drawing.Size(56, 21);
            this.Condition.TabIndex = 31;
            this.Condition.TextChanged += new System.EventHandler(this.Condition_TextChanged);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(586, 75);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(136, 31);
            this.Delete.TabIndex = 32;
            this.Delete.Text = "Delete Condition";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // LoadData
            // 
            this.LoadData.Location = new System.Drawing.Point(151, 35);
            this.LoadData.Name = "LoadData";
            this.LoadData.Size = new System.Drawing.Size(87, 30);
            this.LoadData.TabIndex = 33;
            this.LoadData.Text = "Load data";
            this.LoadData.UseVisualStyleBackColor = true;
            this.LoadData.Click += new System.EventHandler(this.LoadData_Click);
            // 
            // FileList
            // 
            this.FileList.FormattingEnabled = true;
            this.FileList.ItemHeight = 12;
            this.FileList.Location = new System.Drawing.Point(12, 44);
            this.FileList.Name = "FileList";
            this.FileList.Size = new System.Drawing.Size(133, 16);
            this.FileList.Sorted = true;
            this.FileList.TabIndex = 34;
            this.FileList.SelectedIndexChanged += new System.EventHandler(this.FileList_SelectedIndexChanged);
            // 
            // Eps
            // 
            this.Eps.Location = new System.Drawing.Point(269, 39);
            this.Eps.Name = "Eps";
            this.Eps.Size = new System.Drawing.Size(49, 21);
            this.Eps.TabIndex = 35;
            // 
            // EpsLabel
            // 
            this.EpsLabel.AutoSize = true;
            this.EpsLabel.Location = new System.Drawing.Point(239, 44);
            this.EpsLabel.Name = "EpsLabel";
            this.EpsLabel.Size = new System.Drawing.Size(27, 12);
            this.EpsLabel.TabIndex = 36;
            this.EpsLabel.Text = "Eps";
            // 
            // NoClusterLabel
            // 
            this.NoClusterLabel.AutoSize = true;
            this.NoClusterLabel.Location = new System.Drawing.Point(430, 45);
            this.NoClusterLabel.Name = "NoClusterLabel";
            this.NoClusterLabel.Size = new System.Drawing.Size(62, 12);
            this.NoClusterLabel.TabIndex = 38;
            this.NoClusterLabel.Text = "No. Clust.";
            // 
            // NoCluster
            // 
            this.NoCluster.Location = new System.Drawing.Point(493, 40);
            this.NoCluster.Name = "NoCluster";
            this.NoCluster.Size = new System.Drawing.Size(43, 21);
            this.NoCluster.TabIndex = 37;
            this.NoCluster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NoCluster_KeyDown);
            // 
            // OesPlot
            // 
            this.OesPlot.DisplayScale = 0F;
            this.OesPlot.Location = new System.Drawing.Point(292, 69);
            this.OesPlot.Name = "OesPlot";
            this.OesPlot.Size = new System.Drawing.Size(274, 178);
            this.OesPlot.TabIndex = 39;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 515);
            this.Controls.Add(this.OesPlot);
            this.Controls.Add(this.NoClusterLabel);
            this.Controls.Add(this.NoCluster);
            this.Controls.Add(this.EpsLabel);
            this.Controls.Add(this.Eps);
            this.Controls.Add(this.FileList);
            this.Controls.Add(this.LoadData);
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.Condition);
            this.Controls.Add(this.ConditionLabel);
            this.Controls.Add(this.Ratio);
            this.Controls.Add(this.AreaLabel2);
            this.Controls.Add(this.AreaLabel1);
            this.Controls.Add(this.RatioLabel);
            this.Controls.Add(this.SaveDataLabel);
            this.Controls.Add(this.DataPath);
            this.Controls.Add(this.SaveGraphLabel);
            this.Controls.Add(this.GraphPath);
            this.Controls.Add(this.Area2);
            this.Controls.Add(this.Area1);
            this.Controls.Add(this.Wavelength2);
            this.Controls.Add(this.Profile2);
            this.Controls.Add(this.ProfileLabel2);
            this.Controls.Add(this.Wavelength1);
            this.Controls.Add(this.RangePlot);
            this.Controls.Add(this.RangeFind);
            this.Controls.Add(this.SaveData);
            this.Controls.Add(this.SaveGraph);
            this.Controls.Add(this.Deconvolution);
            this.Controls.Add(this.Append);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.XLabelText);
            this.Controls.Add(this.YLabelText);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.GraphLabel);
            this.Controls.Add(this.ProfileLabel1);
            this.Controls.Add(this.Profile1);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.FolderPathLabel);
            this.Controls.Add(this.FolderPath);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog FolderPathDialog;
        private System.Windows.Forms.TextBox FolderPath;
        private System.Windows.Forms.Label FolderPathLabel;
        private ScottPlot.WinForms.FormsPlot Result;
        private ScottPlot.WinForms.FormsPlot Profile1;
        private System.Windows.Forms.Label ProfileLabel1;
        private System.Windows.Forms.Label GraphLabel;
        private System.Windows.Forms.TextBox YLabel;
        private System.Windows.Forms.Label YLabelText;
        private System.Windows.Forms.Label XLabelText;
        private System.Windows.Forms.TextBox XLabel;
        private System.Windows.Forms.Button Append;
        private System.Windows.Forms.Button Deconvolution;
        private System.Windows.Forms.Button SaveGraph;
        private System.Windows.Forms.Button SaveData;
        private System.Windows.Forms.Button RangeFind;
        private ScottPlot.WinForms.FormsPlot RangePlot;
        private System.Windows.Forms.TextBox Wavelength1;
        private System.Windows.Forms.Label ProfileLabel2;
        private ScottPlot.WinForms.FormsPlot Profile2;
        private System.Windows.Forms.TextBox Wavelength2;
        private System.Windows.Forms.TextBox Area1;
        private System.Windows.Forms.TextBox Area2;
        private System.Windows.Forms.TextBox GraphPath;
        private System.Windows.Forms.Label SaveGraphLabel;
        private System.Windows.Forms.Label SaveDataLabel;
        private System.Windows.Forms.TextBox DataPath;
        private System.Windows.Forms.Label RatioLabel;
        private System.Windows.Forms.Label AreaLabel1;
        private System.Windows.Forms.Label AreaLabel2;
        private System.Windows.Forms.TextBox Ratio;
        private System.Windows.Forms.Label ConditionLabel;
        private System.Windows.Forms.TextBox Condition;
        private System.Windows.Forms.FolderBrowserDialog GraphPathDialog;
        private System.Windows.Forms.FolderBrowserDialog DataPathDialog;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button LoadData;
        private System.Windows.Forms.ListBox FileList;
        private System.Windows.Forms.TextBox Eps;
        private System.Windows.Forms.Label EpsLabel;
        private System.Windows.Forms.Label NoClusterLabel;
        private System.Windows.Forms.TextBox NoCluster;
        private ScottPlot.WinForms.FormsPlot OesPlot;
    }
}

