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
    public partial class WriteHelper : Form
    {
        public string text;
        public WriteHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            if (GivenParameters != null)
            {
                FillWithGivenParameters(GivenParameters);
            }
        }

        private void FillWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            textBox1.Text = GivenParameters["text"];
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            text = textBox1.Text;
            this.Close();
        }
    }
}
