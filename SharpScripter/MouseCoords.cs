using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpScripter
{
    public partial class MouseCoords : Form
    {
        public bool stop = false;
        public Point position;
        public MouseCoords()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void locateBtn_Click(object sender, EventArgs e)
        {
            stop = false;
            locateBtn.Enabled = false;
            label2.Text = "Click to Select Location";
            this.Opacity = 0.25;
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
                position = MousePosition;
                label1.Text = "X :  " + position.X + ",   Y :  " + position.Y;
                button1.Enabled = true;
                locateBtn.Enabled = true;
                label2.Text = "Coordinates";
                this.Opacity = 1;
                MessageBox.Show(new Form { TopMost = true }, "Koordinatlar alındı!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("x = " + position.X + ", y = " + position.Y);
            MessageBox.Show("Panoya kopyalandı!");
        }
    }
}
