namespace Demo
{
    partial class frmManage
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
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.button2 = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.dropdown = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(190, 77);
            this.button1.TabIndex = 0;
            this.button1.Text = "Refresh Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(270, 26);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(518, 384);
            this.cartesianChart1.TabIndex = 1;
            this.cartesianChart1.Text = "cartesianChart1";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(3, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 77);
            this.button2.TabIndex = 2;
            this.button2.Text = "Export Stock";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Export
            // 
            this.Export.AutoEllipsis = true;
            this.Export.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Export.Location = new System.Drawing.Point(3, 213);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(190, 77);
            this.Export.TabIndex = 3;
            this.Export.Text = "Create PDF";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Location = new System.Drawing.Point(3, 388);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(190, 50);
            this.button3.TabIndex = 4;
            this.button3.Text = "Open File";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.Location = new System.Drawing.Point(3, 325);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(190, 42);
            this.button4.TabIndex = 5;
            this.button4.Text = "Run Export";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // dropdown
            // 
            this.dropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dropdown.FormattingEnabled = true;
            this.dropdown.Location = new System.Drawing.Point(3, 296);
            this.dropdown.Name = "dropdown";
            this.dropdown.Size = new System.Drawing.Size(190, 23);
            this.dropdown.TabIndex = 7;
            // 
            // frmManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dropdown);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cartesianChart1);
            this.Controls.Add(this.button1);
            this.Name = "frmManage";
            this.Text = "frmManage";
            this.ResumeLayout(false);

        }

        #endregion

        private PrintDialog printDialog1;
        private SaveFileDialog saveFileDialog1;
        private Button button1;
        private LiveCharts.WinForms.CartesianChart cartesianChart1;
        private Button button2;
        private Button Export;
        private Button button3;
        private Button button4;
        private ComboBox dropdown;
    }
}