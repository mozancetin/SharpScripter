using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class SleepHelper : Form
    {
        public string seconds;
        public SleepHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            if (GivenParameters != null) { FillWithGivenParameters(GivenParameters); }
        }

        private void FillWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            numericUpDown1.Value = Convert.ToInt32(GivenParameters["sec"]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            seconds = numericUpDown1.Value.ToString();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 86400;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 3600;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value += 60;
        }
    }
}
