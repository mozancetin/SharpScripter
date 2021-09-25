using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class RGBHelper : Form
    {
        private bool stop = false;
        public bool cancelled = true;
        public Point coords;
        public bool FromCursor = false;

        public RGBHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            if (GivenParameters != null) { FillWithGivenParameters(GivenParameters); }

        }

        private void FillWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            numericUpDown1.Value = Convert.ToInt32(GivenParameters["x"]);
            numericUpDown2.Value = Convert.ToInt32(GivenParameters["y"]);
            checkBox1.Checked = Convert.ToBoolean(GivenParameters["FromCursor"]);
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
                coords = MousePosition;
                numericUpDown1.Value = coords.X;
                numericUpDown2.Value = coords.Y;
                stop = true;
                this.Opacity = 1;
                button2.Text = "Get By Clicking...";
                button2.Enabled = true;
                button1.Enabled = true;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                checkBox1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stop = false;
            this.Opacity = 0.3;
            button2.Text = "Waiting for a click";
            button2.Enabled = false;
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            checkBox1.Enabled = false;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                FromCursor = true;
            }
            cancelled = false;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = !checkBox1.Checked;
            numericUpDown2.Enabled = !checkBox1.Checked;
            button2.Enabled = !checkBox1.Checked;
        }
    }
}
