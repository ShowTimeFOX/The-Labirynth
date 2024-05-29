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
            pictureBoxMonster = new PictureBox();
            timerBossMotion = new System.Windows.Forms.Timer(components);
            panelBackground = new Panel();
            buttonFight = new Button();
            buttonItem = new Button();
            panelPlayerControls = new Panel();
            pictureBoxHit = new PictureBox();
            labelHpPlayer = new Label();
            hpBarPlayer = new ProgressBar();
            hpBarMonster = new ProgressBar();
            timerHitBox = new System.Windows.Forms.Timer(components);
            labelDamage = new Label();
            timerHitPoints = new System.Windows.Forms.Timer(components);
            panelOverlay = new Panel();
            labelGameOver = new Label();
            buttonExit = new Button();
            buttonReplay = new Button();
            labelHpMonster = new Label();
            labelDamagePlayer = new Label();
            timerHitPointsPlayer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pictureBoxMonster).BeginInit();
            panelPlayerControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHit).BeginInit();
            panelOverlay.SuspendLayout();
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
            // pictureBoxMonster
            // 
            pictureBoxMonster.BackColor = Color.Black;
            pictureBoxMonster.BackgroundImageLayout = ImageLayout.Center;
            pictureBoxMonster.ErrorImage = null;
            pictureBoxMonster.Location = new Point(202, 46);
            pictureBoxMonster.Margin = new Padding(3, 2, 3, 2);
            pictureBoxMonster.Name = "pictureBoxMonster";
            pictureBoxMonster.Size = new Size(330, 340);
            pictureBoxMonster.TabIndex = 3;
            pictureBoxMonster.TabStop = false;
            // 
            // timerBossMotion
            // 
            timerBossMotion.Interval = 50;
            timerBossMotion.Tick += timer1_Tick;
            // 
            // panelBackground
            // 
            panelBackground.BackColor = Color.Black;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Name = "panelBackground";
            panelBackground.Size = new Size(200, 100);
            panelBackground.TabIndex = 5;
            panelBackground.Visible = false;
            // 
            // buttonFight
            // 
            buttonFight.Font = new Font("SimSun", 18F, FontStyle.Bold, GraphicsUnit.Point);
            buttonFight.Location = new Point(3, 47);
            buttonFight.Name = "buttonFight";
            buttonFight.Size = new Size(224, 55);
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
            buttonItem.Size = new Size(218, 55);
            buttonItem.TabIndex = 7;
            buttonItem.Text = "ITEM";
            buttonItem.UseVisualStyleBackColor = true;
            buttonItem.Click += buttonItem_Click;
            // 
            // panelPlayerControls
            // 
            panelPlayerControls.BackColor = SystemColors.WindowFrame;
            panelPlayerControls.Controls.Add(pictureBoxHit);
            panelPlayerControls.Controls.Add(labelHpPlayer);
            panelPlayerControls.Controls.Add(hpBarPlayer);
            panelPlayerControls.Controls.Add(buttonFight);
            panelPlayerControls.Controls.Add(buttonItem);
            panelPlayerControls.Location = new Point(576, 652);
            panelPlayerControls.Name = "panelPlayerControls";
            panelPlayerControls.Size = new Size(456, 181);
            panelPlayerControls.TabIndex = 8;
            panelPlayerControls.Visible = false;
            // 
            // pictureBoxHit
            // 
            pictureBoxHit.BackColor = SystemColors.ActiveCaption;
            pictureBoxHit.BackgroundImage = (Image)resources.GetObject("pictureBoxHit.BackgroundImage");
            pictureBoxHit.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxHit.Location = new Point(0, 47);
            pictureBoxHit.Name = "pictureBoxHit";
            pictureBoxHit.Size = new Size(456, 134);
            pictureBoxHit.TabIndex = 10;
            pictureBoxHit.TabStop = false;
            pictureBoxHit.Visible = false;
            pictureBoxHit.Paint += pictureBoxHit_Paint;
            // 
            // labelHpPlayer
            // 
            labelHpPlayer.AutoSize = true;
            labelHpPlayer.Font = new Font("SimSun", 18F, FontStyle.Bold, GraphicsUnit.Point);
            labelHpPlayer.Location = new Point(279, 7);
            labelHpPlayer.Name = "labelHpPlayer";
            labelHpPlayer.Size = new Size(166, 24);
            labelHpPlayer.TabIndex = 11;
            labelHpPlayer.Text = "HP 100 / 100";
            labelHpPlayer.Click += labelHpPlayer_Click;
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
            // hpBarMonster
            // 
            hpBarMonster.ForeColor = Color.Crimson;
            hpBarMonster.Location = new Point(1338, 27);
            hpBarMonster.Name = "hpBarMonster";
            hpBarMonster.Size = new Size(190, 23);
            hpBarMonster.Step = 1;
            hpBarMonster.Style = ProgressBarStyle.Continuous;
            hpBarMonster.TabIndex = 9;
            hpBarMonster.Value = 14;
            // 
            // timerHitBox
            // 
            timerHitBox.Interval = 5;
            timerHitBox.Tick += timerHitBox_Tick;
            // 
            // labelDamage
            // 
            labelDamage.AutoSize = true;
            labelDamage.Font = new Font("Tahoma", 22.2F, FontStyle.Bold, GraphicsUnit.Point);
            labelDamage.ForeColor = Color.Red;
            labelDamage.Location = new Point(850, 115);
            labelDamage.Name = "labelDamage";
            labelDamage.Size = new Size(53, 36);
            labelDamage.TabIndex = 10;
            labelDamage.Text = "50";
            labelDamage.Visible = false;
            // 
            // timerHitPoints
            // 
            timerHitPoints.Interval = 20;
            timerHitPoints.Tick += timerHitPoints_Tick;
            // 
            // panelOverlay
            // 
            panelOverlay.BackColor = Color.FromArgb(64, 64, 64);
            panelOverlay.Controls.Add(labelGameOver);
            panelOverlay.Controls.Add(buttonExit);
            panelOverlay.Controls.Add(buttonReplay);
            panelOverlay.Location = new Point(614, 166);
            panelOverlay.Name = "panelOverlay";
            panelOverlay.Size = new Size(300, 200);
            panelOverlay.TabIndex = 11;
            panelOverlay.Visible = false;
            panelOverlay.Paint += panelOverlay_Paint;
            // 
            // labelGameOver
            // 
            labelGameOver.AutoSize = true;
            labelGameOver.Font = new Font("SimSun", 22.2F, FontStyle.Bold, GraphicsUnit.Point);
            labelGameOver.ForeColor = SystemColors.ButtonHighlight;
            labelGameOver.Location = new Point(78, 29);
            labelGameOver.Name = "labelGameOver";
            labelGameOver.Size = new Size(157, 30);
            labelGameOver.TabIndex = 0;
            labelGameOver.Text = "GAME OVER";
            // 
            // buttonExit
            // 
            buttonExit.Font = new Font("SimSun", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            buttonExit.Location = new Point(104, 134);
            buttonExit.Name = "buttonExit";
            buttonExit.Size = new Size(112, 43);
            buttonExit.TabIndex = 2;
            buttonExit.Text = "Exit";
            buttonExit.UseVisualStyleBackColor = true;
            buttonExit.Click += buttonExit_Click;
            // 
            // buttonReplay
            // 
            buttonReplay.Font = new Font("SimSun", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            buttonReplay.Location = new Point(104, 72);
            buttonReplay.Name = "buttonReplay";
            buttonReplay.Size = new Size(112, 44);
            buttonReplay.TabIndex = 1;
            buttonReplay.Text = "Replay";
            buttonReplay.UseVisualStyleBackColor = true;
            buttonReplay.Click += buttonReplay_Click;
            // 
            // labelHpMonster
            // 
            labelHpMonster.AutoSize = true;
            labelHpMonster.BackColor = SystemColors.ActiveCaptionText;
            labelHpMonster.Font = new Font("SimSun", 18F, FontStyle.Bold, GraphicsUnit.Point);
            labelHpMonster.ForeColor = SystemColors.Control;
            labelHpMonster.Location = new Point(1351, 60);
            labelHpMonster.Name = "labelHpMonster";
            labelHpMonster.Size = new Size(166, 24);
            labelHpMonster.TabIndex = 12;
            labelHpMonster.Text = "HP 100 / 100";
            // 
            // labelDamagePlayer
            // 
            labelDamagePlayer.AutoSize = true;
            labelDamagePlayer.Font = new Font("Tahoma", 22.2F, FontStyle.Bold, GraphicsUnit.Point);
            labelDamagePlayer.ForeColor = Color.Red;
            labelDamagePlayer.Location = new Point(974, 600);
            labelDamagePlayer.Name = "labelDamagePlayer";
            labelDamagePlayer.Size = new Size(53, 36);
            labelDamagePlayer.TabIndex = 13;
            labelDamagePlayer.Text = "50";
            labelDamagePlayer.Visible = false;
            // 
            // timerHitPointsPlayer
            // 
            timerHitPointsPlayer.Interval = 20;
            timerHitPointsPlayer.Tick += timerHitPointsPlayer_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(1540, 845);
            Controls.Add(labelDamagePlayer);
            Controls.Add(panelOverlay);
            Controls.Add(labelHpMonster);
            Controls.Add(labelDamage);
            Controls.Add(hpBarMonster);
            Controls.Add(panelPlayerControls);
            Controls.Add(panelBackground);
            Controls.Add(pictureBoxMonster);
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
            PreviewKeyDown += Form1_PreviewKeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBoxMonster).EndInit();
            panelPlayerControls.ResumeLayout(false);
            panelPlayerControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHit).EndInit();
            panelOverlay.ResumeLayout(false);
            panelOverlay.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private PictureBox pictureBoxMonster;
        private Panel panelBackground;
        private Button buttonFight;
        private Button buttonItem;
        private Panel panelPlayerControls;
        private ProgressBar hpBarMonster;
        private ProgressBar hpBarPlayer;
        private Label labelHpPlayer;
        private PictureBox pictureBoxHit;
        private System.Windows.Forms.Timer timerHitBox;
        private Label labelDamage;
        private System.Windows.Forms.Timer timerHitPoints;
        private System.Windows.Forms.Timer timerBossMotion;
        private Panel panelOverlay;
        private Label labelGameOver;
        private Button buttonExit;
        private Button buttonReplay;
        private Label labelHpMonster;
        private Label labelDamagePlayer;
        private System.Windows.Forms.Timer timerHitPointsPlayer;
    }
}
