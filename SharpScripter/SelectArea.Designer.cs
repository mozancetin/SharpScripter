
namespace SharpScripter
{
    partial class SelectArea
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.startPointLbl = new System.Windows.Forms.Label();
            this.endPointLbl = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Impact", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(460, 89);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lütfen Başlangıç Noktasını Seçin";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startPointLbl
            // 
            this.startPointLbl.Font = new System.Drawing.Font("Impact", 24F);
            this.startPointLbl.Location = new System.Drawing.Point(12, 19);
            this.startPointLbl.Name = "startPointLbl";
            this.startPointLbl.Size = new System.Drawing.Size(460, 39);
            this.startPointLbl.TabIndex = 1;
            this.startPointLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // endPointLbl
            // 
            this.endPointLbl.Font = new System.Drawing.Font("Impact", 24F);
            this.endPointLbl.Location = new System.Drawing.Point(12, 72);
            this.endPointLbl.Name = "endPointLbl";
            this.endPointLbl.Size = new System.Drawing.Size(460, 39);
            this.endPointLbl.TabIndex = 2;
            this.endPointLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SelectArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.Controls.Add(this.endPointLbl);
            this.Controls.Add(this.startPointLbl);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 350);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "SelectArea";
            this.Opacity = 0.5D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GetArea";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label startPointLbl;
        private System.Windows.Forms.Label endPointLbl;
        private System.Windows.Forms.Timer timer1;
    }
}