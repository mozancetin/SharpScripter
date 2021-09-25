using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class MoveCursorHelper : Form
    {
        private bool stop = false;
        public Point coords;
        public bool cancelled = true;
        private bool selected = false;

        public MoveCursorHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            if (GivenParameters != null) { FillWithGivenParameters(GivenParameters); }
        }

        private void FillWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            num1.Value = Convert.ToInt32(GivenParameters["x"]);
            num2.Value = Convert.ToInt32(GivenParameters["y"]);
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.45;
            this.label3.Text = "Waiting for a click...";
            okBtn.Enabled = false;
            selectButton.Enabled = false;
            num1.Enabled = false;
            num2.Enabled = false;
            stop = false;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stop)
            {
                timer1.Stop();
                return;
            }

            if (MouseButtons == MouseButtons.Left)
            {
                stop = true;
                coords = MousePosition;
                num1.Value = coords.X;
                num2.Value = coords.Y;
                label3.Text = "Move To:";
                okBtn.Enabled = true;
                selectButton.Enabled = true;
                num2.Enabled = true;
                num1.Enabled = true;
                this.Opacity = 1;
                selected = true;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            cancelled = false;
            if (!selected)
            {
                coords = new Point(Convert.ToInt32(num1.Value), Convert.ToInt32(num2.Value));
            }
            this.Close();
        }
    }
}
