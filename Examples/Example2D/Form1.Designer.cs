using System;
using System.Windows.Forms;
using TDF.WinFormsPanel;

namespace Example2D
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.directXPanel1 = new TDF.WinFormsPanel.DirectXPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // directXPanel1
            // 
            this.directXPanel1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.directXPanel1.AllowDrop = true;
            this.directXPanel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.directXPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directXPanel1.Location = new System.Drawing.Point(0, 0);
            this.directXPanel1.Name = "directXPanel1";
            this.directXPanel1.Size = new System.Drawing.Size(772, 515);
            this.directXPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 515);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directXPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DirectXPanel directXPanel1;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private Label label1;
    }
}

