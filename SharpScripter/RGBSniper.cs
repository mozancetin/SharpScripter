using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SharpScripter.Utils;

namespace SharpScripter
{
    public partial class RGBSniper : Form
    {
        public bool stop = false;
        public RGBSniper()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label1.Text.Replace(",", ""));
            MessageBox.Show("Panoya kopyalandı!");
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
                Color newColor = GetPixelColor(MousePosition);
                label1.Text = string.Join(", ", new byte[3] { newColor.R, newColor.G, newColor.B });
                button1.Enabled = true;
                getPixelButton.Enabled = true;
                label2.Text = "R G B (Red, Green, Blue)";
                colorLbl.BackColor = newColor;
                colorLbl.ForeColor = Color.FromArgb(255 - Convert.ToInt32(newColor.R), 255 - Convert.ToInt32(newColor.G), 255 - Convert.ToInt32(newColor.B));
                MessageBox.Show(new Form { TopMost = true }, "RGB Değeri Alındı!");
            }
        }

        private void getPixelButton_Click(object sender, EventArgs e)
        {
            stop = false;
            getPixelButton.Enabled = false;
            label2.Text = "Click to Select";
            timer1.Start();
        }
    }
}
