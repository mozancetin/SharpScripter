using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class ClickHelper : Form
    {
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        private bool stop = false;
        public ClickHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            if (GivenParameters != null)
            {
                FillWithParameters(GivenParameters);
            }
        }

        private void FillWithParameters(Dictionary<string, string> GivenParameters)
        {
            if (GivenParameters["FromCursor"] == "1") { checkBox1.Checked = true; }
            else
            {
                numericUpDown1.Value = Convert.ToInt32(GivenParameters["x"]);
                numericUpDown2.Value = Convert.ToInt32(GivenParameters["y"]);
            }
            if (GivenParameters["doubleclick"] == "1") { checkBox2.Checked = true; }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = !checkBox1.Checked;
            numericUpDown2.Enabled = !checkBox1.Checked;
            button2.Enabled = !checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Parameters.Add("FromCursor", "1");
            }
            else
            {
                Parameters.Add("x", numericUpDown1.Value.ToString());
                Parameters.Add("y", numericUpDown2.Value.ToString());
                Parameters.Add("FromCursor", "0");
            }

            if (checkBox2.Checked) { Parameters.Add("doubleclick", "1"); }
            else { Parameters.Add("doubleclick", "0"); }

            this.Close();
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
                Point coords = MousePosition;
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
                checkBox2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stop = false;
            this.Opacity = 0.45;
            button2.Text = "Waiting for a click";
            button2.Enabled = false;
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            timer1.Start();
        }
    }
}
