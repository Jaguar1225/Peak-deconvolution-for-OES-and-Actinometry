using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Peak_deconvolution_for_OES_and_Actinometry
{
    public partial class Loading : Form
    {
        private ProgressBar progressBar;
        public Loading(string message = "Working process")
        {
            InitializeComponent();
            SetMessage(message);
            this.LoadingBar.Style = ProgressBarStyle.Marquee; // Set the style to Marquee for indefinite loading
            this.LoadingBar.MarqueeAnimationSpeed = 30; // Adjust the speed of the marquee animation

            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Prevent resizing
            this.ControlBox = false; // Remove the close button
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen
            this.TopMost = true; // Keep the form on top of other windows
        }
        public void SetMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(SetMessage), message);
                return;
            }
            Message.Text = message;
        }
        public void UpdateProgress(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(UpdateProgress), value);
                return;
            }
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Progress value must be between 0 and 100.");
            LoadingBar.Value = value;
        }

        public void ShowLoading()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ShowLoading));
                return;
            }
            Thread thread = new Thread(() =>
            {
                this.ShowDialog(); // Show the loading form as a modal dialog
            });
            thread.SetApartmentState(ApartmentState.STA); // Set the thread to STA for UI operations
            thread.Start();
        }
        public void HideLoading()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(()=> this.Close()));
            }
            else
            {
                this.Close();
            }
        }
    }
}
