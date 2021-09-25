
namespace SharpScripter
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.rgbSniperBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ScripterBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rgbSniperBtn
            // 
            this.rgbSniperBtn.Font = new System.Drawing.Font("Impact", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rgbSniperBtn.Location = new System.Drawing.Point(23, 22);
            this.rgbSniperBtn.Name = "rgbSniperBtn";
            this.rgbSniperBtn.Size = new System.Drawing.Size(70, 70);
            this.rgbSniperBtn.TabIndex = 0;
            this.rgbSniperBtn.Text = "RGB Sniper";
            this.rgbSniperBtn.UseVisualStyleBackColor = true;
            this.rgbSniperBtn.Click += new System.EventHandler(this.rgbSniperBtn_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Impact", 14.25F);
            this.button2.Location = new System.Drawing.Point(99, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 70);
            this.button2.TabIndex = 2;
            this.button2.Text = "Select Area";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Impact", 14.25F);
            this.button1.Location = new System.Drawing.Point(175, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 70);
            this.button1.TabIndex = 3;
            this.button1.Text = "Screenshot";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Impact", 14.25F);
            this.button3.Location = new System.Drawing.Point(299, 22);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 70);
            this.button3.TabIndex = 4;
            this.button3.Text = "Find Coords";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ScripterBtn
            // 
            this.ScripterBtn.Font = new System.Drawing.Font("Impact", 14.25F);
            this.ScripterBtn.Location = new System.Drawing.Point(380, 22);
            this.ScripterBtn.Name = "ScripterBtn";
            this.ScripterBtn.Size = new System.Drawing.Size(104, 70);
            this.ScripterBtn.TabIndex = 5;
            this.ScripterBtn.Text = "Scripter";
            this.ScripterBtn.UseVisualStyleBackColor = true;
            this.ScripterBtn.Click += new System.EventHandler(this.ScripterBtn_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 111);
            this.Controls.Add(this.ScripterBtn);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.rgbSniperBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(520, 150);
            this.MinimumSize = new System.Drawing.Size(520, 150);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button rgbSniperBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button ScripterBtn;
    }
}