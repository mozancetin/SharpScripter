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
    public partial class Area : Form
    {
        public Point startPoint;
        public Area(Point startPoint, Point endPoint, Color? windowColor = null)
        {
            InitializeComponent();
            this.startPoint = startPoint;
            (int width, int height) = (endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            this.Location = startPoint;
            this.Size = new Size(width, height);
            this.BackColor = windowColor ?? Color.LawnGreen;
        }

        private void Area_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Area_Load(object sender, EventArgs e)
        {

        }
    }
}
