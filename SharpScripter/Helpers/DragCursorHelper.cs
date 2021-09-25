using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class DragCursorHelper : Form
    {
        private bool stop = false;
        private int count = 0;
        private bool down = false;
        public bool cancelled = true;
        public Point startPoint;
        public Point endPoint;
        private Area firstClick;
        private Area secondClick;

        public DragCursorHelper(Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            if (GivenParameters != null) { FillWithGivenParameters(GivenParameters); }
        }

        private void FillWithGivenParameters(Dictionary<string, string> GivenParameters)
        {
            num1.Value = Convert.ToInt32(GivenParameters["x1"]);
            num2.Value = Convert.ToInt32(GivenParameters["y1"]);
            num3.Value = Convert.ToInt32(GivenParameters["x2"]);
            num4.Value = Convert.ToInt32(GivenParameters["y2"]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count > 1) { stop = true; }

            if (stop)
            {
                timer1.Stop();
                Thread.Sleep(500);
                firstClick.Close();
                firstClick.Dispose();
                secondClick.Close();
                secondClick.Dispose();
                return;
            }

            if (MouseButtons == MouseButtons.Left && !down)
            {
                down = true;
                if (count == 0)
                {
                    startPoint = MousePosition;
                    num1.Value = startPoint.X;
                    num2.Value = startPoint.Y;
                    label5.Text = "START POINT:";
                    firstClick = new Area(startPoint, new Point(startPoint.X, startPoint.Y));
                    firstClick.Show();
                }
                else if (count == 1)
                {
                    endPoint = MousePosition;
                    num3.Value = endPoint.X;
                    num4.Value = endPoint.Y;
                    label6.Text = "END POINT:";
                    okBtn.Enabled = true;
                    selectButton.Enabled = true;
                    num4.Enabled = true;
                    num3.Enabled = true;
                    num2.Enabled = true;
                    num1.Enabled = true;
                    this.Opacity = 1;
                    secondClick = new Area(endPoint, new Point(endPoint.X, endPoint.Y));
                    secondClick.Show();
                }
                count++;
            }
            else if (MouseButtons == MouseButtons.None && down)
            {
                down = false;
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.45;
            this.label5.Text = "Waiting for a click...";
            this.label6.Text = "Waiting for a click...";
            okBtn.Enabled = false;
            selectButton.Enabled = false;
            num1.Enabled = false;
            num2.Enabled = false;
            num3.Enabled = false;
            num4.Enabled = false;
            stop = false;
            timer1.Start();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            startPoint = new Point(Convert.ToInt32(num1.Value), Convert.ToInt32(num2.Value));
            endPoint = new Point(Convert.ToInt32(num3.Value), Convert.ToInt32(num4.Value));
            cancelled = false;
            this.Close();
        }
    }
}
