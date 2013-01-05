using System;
using System.Drawing;
using System.Windows.Forms;

namespace WLWStaticAnchorManager
{
    public partial class ExtendedTextbox : UserControl
    {
        private System.Windows.Forms.TextBox textBox1;

        public ExtendedTextbox()
        {
            InitializeComponent();
            this.textBox1.Resize += new EventHandler(textBox1_Resize);

            this.textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.OnTextChanged(e);
        }

        void textBox1_Resize(object sender, EventArgs e)
        {
            textBox1.Top = 1;
            textBox1.Left = 1;
            this.Width = textBox1.Width + 2;
            this.Height = textBox1.Height + 2;
        }


        public Color TextBoxBorderColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }


        public Color TextBoxBackColor
        {
            get { return this.textBox1.BackColor; }
            set { this.textBox1.BackColor = value; }
        }


        public override string Text
        {
            get { return this.textBox1.Text; }
            set { this.textBox1.Text = value; }
        }


        public HorizontalAlignment TextAlign
        {
            get { return this.textBox1.TextAlign; }
            set { this.textBox1.TextAlign = value; }
        }


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


        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(1, 1);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 0;
            // 
            // ExtendedTextbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ExtendedTextbox";
            this.Size = new System.Drawing.Size(102, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
