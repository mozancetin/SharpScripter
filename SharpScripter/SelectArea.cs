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
    public partial class SelectArea : Form
    {
        public Point startPoint;
        public Point endPoint;
        public bool stop = false;
        public int count = 0;
        public bool down = false;
        public bool hide;
        public Area area;
        public SelectArea(bool hide = false)
        {
            InitializeComponent();
            timer1.Start();
            this.hide = hide;
            area = new Area(new Point(0, 0), new Point(0, 0));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count > 1 || stop)
            {
                timer1.Stop();
                return;
            }

            if (count == 1 || !stop)
            {
                area.Size = new Size(MousePosition.X - startPoint.X, MousePosition.Y - startPoint.Y);
            }

            if (!down)
            {
                if (MouseButtons == MouseButtons.Left)
                {
                    if (count == 0)
                    {
                        startPoint = MousePosition;
                        startPointLbl.Text = "Başlangıç:   " + "X = " + startPoint.X.ToString() + "   Y = " + startPoint.Y.ToString();
                        label1.Text = "Lütfen Bitiş Noktasını Seçin";
                        count++;
                        down = true;
                        area = new Area(startPoint, new Point(0, 0)) { TopMost = true };
                        area.ShowDialog();

                    }
                    else if (count == 1)
                    {
                        endPoint = MousePosition;
                        endPointLbl.Text = "Bitiş:   " + "X = " + endPoint.X.ToString() + "   Y = " + endPoint.Y.ToString();
                        count++;
                        stop = true;
                        timer1.Stop();
                        if (!hide)
                        {
                            this.TopMost = true;
                            label1.Text = "Seçilen Alanı Kapatmak İçin Üzerine Tıklayın.";
                            area.Close();
                            area.Dispose();
                            area = new Area(startPoint, endPoint) { TopMost = true };
                            area.ShowDialog();
                            this.TopMost = false;
                            label1.Text = "Değerler yukarıda gördüğünüz gibi. Pencereyi kapatabilirsiniz.";
                            this.Opacity = 1;
                        }
                        else
                        {
                            area.Close();
                            area.Dispose();
                            this.Hide();
                        }
                        return;
                    }
                }

                else if (MouseButtons == MouseButtons.Right)
                {
                    area.Close();
                    this.Opacity = 1;
                    endPointLbl.Text = "İşlem iptal edildi!";
                    label1.Text = "Pencereyi kapatabilirsiniz.";
                    count++;
                    stop = true;
                    timer1.Stop();
                    return;
                }
            }
            else
            {
                if (MouseButtons == MouseButtons.None)
                {
                    down = false;
                }
            }
        }
    }
}
