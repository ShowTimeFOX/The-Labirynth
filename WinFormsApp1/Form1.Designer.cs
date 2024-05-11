namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(800, 450);
            DoubleBuffered = true;
            KeyPreview = true;
            Name = "Form1";
            Text = "The_Labirynth";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            KeyPress += Form1_KeyPress;
            PreviewKeyDown += Form1_PreviewKeyDown;
            ResumeLayout(false);
        }

        #endregion
    }
}
