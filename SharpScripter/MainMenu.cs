using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SharpScripter
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void rgbSniperBtn_Click(object sender, EventArgs e)
        {
            RGBSniper RGBSniperWindow = new RGBSniper();
            this.Hide();
            this.TopMost = false;
            RGBSniperWindow.ShowDialog();
            this.TopMost = true;
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectArea selectedArea = new SelectArea();
            this.Hide();
            this.TopMost = false;
            selectedArea.ShowDialog();
            this.TopMost = true;
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Opacity = 0;
            string path = Utils.GetScreenshot(0, 0, 1920, 1080);
            this.Opacity = 1;
            MessageBox.Show("Screenshot başarıyla şu konuma kaydedildi: " + path);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MouseCoords locateWindow = new MouseCoords();
            this.Hide();
            this.TopMost = false;
            locateWindow.ShowDialog();
            this.TopMost = true;
            this.Show();
        }

        private void ScripterBtn_Click(object sender, EventArgs e)
        {
            Scripter scripterWindow = new Scripter();
            this.Hide();
            this.TopMost = false;
            scripterWindow.ShowDialog();
            this.TopMost = true;
            this.Show();
        }
    }
}
