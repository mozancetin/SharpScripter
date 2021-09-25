using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class ScreenshotHelper : Form
    {
        public Point startPoint;
        public Point endPoint;
        public bool cancelled = true;
        public bool fromSelect = false;

        public ScreenshotHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            num3.Maximum = Screen.PrimaryScreen.Bounds.Width;
            num4.Maximum = Screen.PrimaryScreen.Bounds.Height;
            if (GivenParameters != null) { FillWithGivenParameters(GivenParameters); }
        }

        private void FillWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            num1.Value = Convert.ToInt32(GivenParameters["x1"]);
            num2.Value = Convert.ToInt32(GivenParameters["y1"]);
            num3.Value = Convert.ToInt32(GivenParameters["x2"]);
            num4.Value = Convert.ToInt32(GivenParameters["y2"]);
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            SelectArea areaWindow = new SelectArea(true);
            this.Hide();
            areaWindow.ShowDialog();
            startPoint = areaWindow.startPoint;
            endPoint = areaWindow.endPoint;
            num1.Value = startPoint.X;
            num2.Value = startPoint.Y;
            num3.Value = endPoint.X;
            num4.Value = endPoint.Y;
            fromSelect = true;
            this.Show();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (!fromSelect)
            {
                startPoint = new Point(Convert.ToInt32(num1.Value), Convert.ToInt32(num2.Value));
                endPoint = new Point(Convert.ToInt32(num3.Value), Convert.ToInt32(num4.Value));
            }
            cancelled = false;
            this.Close();
        }
    }
}
