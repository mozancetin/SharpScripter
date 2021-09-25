using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpScripter.Helpers
{
    public partial class KeyHelper : Form
    {
        private Keys k = Keys.None;
        public Keys key;
        public string keyText;
        public bool hold;
        public int secs;
        public KeyHelper(Dictionary<string, Keys> GivenKeyParameters = null, Dictionary<string, string> GivenParameters = null)
        {
            InitializeComponent();
            this.KeyPreview = true;
            if (GivenKeyParameters != null)
            {
                FillWithParameters(GivenKeyParameters, GivenParameters);
            }
        }

        private void FillWithParameters(Dictionary<string, Keys> keyParameters, Dictionary<string, string> Parameters)
        {
            Keys keyParameter = keyParameters["key"];
            hold = Convert.ToBoolean(Parameters["hold"]);
            if (hold)
            {
                secs = Convert.ToInt32(Parameters["sec"]);
                holdCheck.Checked = true;
                num1.Value = secs;
            }
            k = keyParameter;
            k = KeyControl(keyParameter);
        }

        private Keys KeyControl(Keys key)
        {
            if (key == Keys.OemQuestion) { label1.Text = "Ö"; }
            else if (key == Keys.Oem7) { label1.Text = "İ"; }
            else if (key == Keys.Oem6) { label1.Text = "Ü"; }
            else if (key == Keys.Oem5) { label1.Text = "Ç"; }
            else if (key == Keys.Oem1) { label1.Text = "Ş"; }
            else if (key == Keys.OemOpenBrackets) { label1.Text = "Ğ"; }
            else if (key == Keys.Return)
            {
                label1.Text = "Enter";
                key = Keys.Enter;
            }
            else { label1.Text = k.ToString(); }

            return key;
        }

        private void KeyHelper_KeyDown(object sender, KeyEventArgs e)
        {
            label1.BackColor = Color.FromArgb(255, 255, 102);
            k = e.KeyCode;
            k = KeyControl(k);
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            key = k;
            keyText = label1.Text;
            if (holdCheck.Checked)
            {
                hold = true;
                secs = Convert.ToInt32(num1.Value);
            }
            else { hold = false; }
            this.Close();
        }

        private void KeyHelper_KeyUp(object sender, KeyEventArgs e)
        {
            label1.BackColor = Color.FromArgb(180, 180, 180);
        }

        private void holdCheck_CheckedChanged(object sender, EventArgs e)
        {
            num1.Enabled = holdCheck.Checked;
            if (!button1.Enabled && k != Keys.None) { button1.Enabled = true; }

        }
    }
}
