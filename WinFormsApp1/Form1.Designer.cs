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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            timerBossMotion = new System.Windows.Forms.Timer(components);
            panel1 = new Panel();
            buttonFight = new Button();
            buttonItem = new Button();
            panel2 = new Panel();
            pictureBoxHit = new PictureBox();
            labelHpPlayer = new Label();
            hpBarPlayer = new ProgressBar();
            hpBarEnemy = new ProgressBar();
            timerHitBox = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHit).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ControlLightLight;
            label1.Font = new Font("Sylfaen", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(10, 7);
            label1.Name = "label1";
            label1.Size = new Size(56, 22);
            label1.TabIndex = 0;
            label1.Text = "label1";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.ControlLightLight;
            label2.Font = new Font("Sylfaen", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(10, 27);
            label2.Name = "label2";
            label2.Size = new Size(56, 22);
            label2.TabIndex = 1;
            label2.Text = "label2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ControlLightLight;
            label3.Font = new Font("Sylfaen", 24F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(10, 56);
            label3.Name = "label3";
            label3.Size = new Size(42, 42);
            label3.TabIndex = 2;
            label3.Text = "N";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.ErrorImage = null;
            pictureBox1.Location = new Point(202, 46);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(330, 340);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // timerBossMotion
            // 
            timerBossMotion.Interval = 50;
            timerBossMotion.Tick += timer1_Tick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 100);
            panel1.TabIndex = 5;
            panel1.Visible = false;
            // 
            // buttonFight
            // 
            buttonFight.Font = new Font("SimSun", 18F, FontStyle.Bold, GraphicsUnit.Point);
            buttonFight.Location = new Point(3, 47);
            buttonFight.Name = "buttonFight";
            buttonFight.Size = new Size(152, 55);
            buttonFight.TabIndex = 6;
            buttonFight.Text = "FIGHT";
            buttonFight.UseVisualStyleBackColor = true;
            buttonFight.Click += buttonFight_Click;
            // 
            // buttonItem
            // 
            buttonItem.Font = new Font("SimSun", 18F, FontStyle.Bold, GraphicsUnit.Point);
            buttonItem.Location = new Point(233, 47);
            buttonItem.Name = "buttonItem";
            buttonItem.Size = new Size(152, 55);
            buttonItem.TabIndex = 7;
            buttonItem.Text = "ITEM";
            buttonItem.UseVisualStyleBackColor = true;
            buttonItem.Click += buttonItem_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = SystemColors.WindowFrame;
            panel2.Controls.Add(pictureBoxHit);
            panel2.Controls.Add(labelHpPlayer);
            panel2.Controls.Add(hpBarPlayer);
            panel2.Controls.Add(buttonFight);
            panel2.Controls.Add(buttonItem);
            panel2.Location = new Point(576, 652);
            panel2.Name = "panel2";
            panel2.Size = new Size(388, 181);
            panel2.TabIndex = 8;
            panel2.Visible = false;
            // 
            // pictureBoxHit
            // 
            pictureBoxHit.BackColor = SystemColors.ActiveCaption;
            pictureBoxHit.BackgroundImage = (Image)resources.GetObject("pictureBoxHit.BackgroundImage");
            pictureBoxHit.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxHit.Enabled = false;
            pictureBoxHit.Location = new Point(0, 47);
            pictureBoxHit.Name = "pictureBoxHit";
            pictureBoxHit.Size = new Size(388, 134);
            pictureBoxHit.TabIndex = 10;
            pictureBoxHit.TabStop = false;
            pictureBoxHit.Visible = false;
            pictureBoxHit.Paint += pictureBoxHit_Paint;
            // 
            // labelHpPlayer
            // 
            labelHpPlayer.AutoSize = true;
            labelHpPlayer.Font = new Font("MS Reference Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point);
            labelHpPlayer.Location = new Point(279, 3);
            labelHpPlayer.Name = "labelHpPlayer";
            labelHpPlayer.Size = new Size(44, 28);
            labelHpPlayer.TabIndex = 11;
            labelHpPlayer.Text = "HP";
            // 
            // hpBarPlayer
            // 
            hpBarPlayer.ForeColor = Color.Lime;
            hpBarPlayer.Location = new Point(3, 3);
            hpBarPlayer.Name = "hpBarPlayer";
            hpBarPlayer.Size = new Size(270, 28);
            hpBarPlayer.Step = 1;
            hpBarPlayer.Style = ProgressBarStyle.Continuous;
            hpBarPlayer.TabIndex = 10;
            hpBarPlayer.Value = 14;
            // 
            // hpBarEnemy
            // 
            hpBarEnemy.ForeColor = Color.Crimson;
            hpBarEnemy.Location = new Point(748, 13);
            hpBarEnemy.Name = "hpBarEnemy";
            hpBarEnemy.Size = new Size(100, 23);
            hpBarEnemy.Step = 1;
            hpBarEnemy.Style = ProgressBarStyle.Continuous;
            hpBarEnemy.TabIndex = 9;
            hpBarEnemy.Value = 14;
            // 
            // timerHitBox
            // 
            timerHitBox.Interval = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(1540, 845);
            Controls.Add(hpBarEnemy);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "The_Labirynth";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            KeyPress += Form1_KeyPress;
            PreviewKeyDown += Form1_PreviewKeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHit).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerBossMotion;
        private Panel panel1;
        private Button buttonFight;
        private Button buttonItem;
        private Panel panel2;
        private ProgressBar hpBarEnemy;
        private ProgressBar hpBarPlayer;
        private Label labelHpPlayer;
        private PictureBox pictureBoxHit;
        private System.Windows.Forms.Timer timerHitBox;
    }
}
