using GameLibrary;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using TheLabirynth;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private WaveOutEvent backgroundMusicPlayer;
        private AudioFileReader backgroundMusicReader;
        private WaveOutEvent stepSoundPlayer;
        private AudioFileReader stepSoundReader;
        //private WaveOutEvent backgroundMusicPlayer;
        //private AudioFileReader backgroundMusicReader;
        private Game game;
        private Player player;
        private Image compassPointerImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));
        private Image originalCompassImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));

        int xPosition = 50;
        int yPosition = 50;
        int centerX = 150; // �rodkowy punkt �semki w poziomie
        int centerY = 150; // �rodkowy punkt �semki w pionie
        int amplitudeX = 100; // Amplituda �semki w poziomie
        int amplitudeY = 50; // Amplituda �semki w pionie
        double angle = 0; // K�t do obliczania pozycji
        double angleStep = 0.1; // Krok zmiany k�ta dla pr�dko�ci ruchu

        bool enableWalk = true;
        bool setMonster = false;
        bool isGameOver = false;

        private int linePosition; // Pozycja pozioma linii
        private int lineWidth; // Szeroko�� linii
        private int lineHeight; // Wysoko�� linii
        private bool isSpacePressed; // Flaga okre�laj�ca, czy spacja zosta�a naci�ni�ta
        private int damage; // Warto�� uszkodzenia zadawanego przez gracza

        private bool playerRound = true; //czyja kolej na bicie

        public Form1()
        {

            player = new Player("player", null, 100, 100, 30, 20);
            game = new Game(player);
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/main.wav"));
            backgroundMusicPlayer = new WaveOutEvent();
            backgroundMusicPlayer.Init(backgroundMusicReader);
            backgroundMusicReader.Volume = 0.7f; // Przyk�adowo: 20% g�o�no�ci
            backgroundMusicPlayer.Play();

            //hitBox
            linePosition = 0;
            lineWidth = 10;
            lineHeight = 100;
            damage = 0;
            isSpacePressed = false;

            panelBackground.Visible = false;
            panelPlayerControls.Visible = false;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Size = new Size(Width, Height);
            panelBackground.Controls.Add(pictureBoxMonster);

            panelBackground.Controls.Add(panelPlayerControls);

            panelPlayerControls.Controls.Add(buttonFight);
            panelPlayerControls.Controls.Add(buttonItem);
            pictureBoxHit.Size = new Size(panelPlayerControls.Width, panelPlayerControls.Height);

            //HP BAR przeciwnik
            hpBarEnemy.Location = new Point(panelBackground.Width - 300, 100);
            hpBarEnemy.Size = new Size(200, 30);
            hpBarEnemy.Minimum = 0;
            hpBarEnemy.Maximum = 100; // Maksymalna warto�� HP
            hpBarEnemy.Value = 100; // Aktualna warto�� HP
            hpBarEnemy.ForeColor = Color.Red; //to to nie dziala
            panelBackground.Controls.Add(hpBarEnemy);

            hpBarPlayer.Minimum = 0;
            hpBarPlayer.Maximum = 100;
            hpBarPlayer.Value = 100;

            labelHpPlayer.Text = $"HP {player.HPCurrent} / {player.HPMax}";

            //pictureBox1.BackColor = Color.Transparent; - g�wno psuje animacje
            pictureBoxMonster.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxMonster.Visible = false;
            centerX = Width / 2 - 165;
            centerY = Height / 2 - 340;
            timerBossMotion.Start();

            panelOverlay.Visible = false;
            panelOverlay.Location = new Point(Width / 2 - panelOverlay.Width / 2, Height / 2 - panelOverlay.Height / 2);

            panelOverlay.Controls.Add(labelGameOver);
            panelOverlay.Controls.Add(buttonReplay);
            panelOverlay.Controls.Add(buttonExit);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Wy�wietlenie komunikatu potwierdzaj�cego wyj�cie
                //DialogResult result = MessageBox.Show("Czy na pewno chcesz wyj��?", "Potwierdzenie wyj�cia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //// Je�li u�ytkownik potwierdzi, zamknij aplikacj�
                //if (result == DialogResult.Yes)
                //{
                //    this.Close(); // Zamkni�cie aplikacji
                //}
                this.Close(); // Zamkni�cie aplikacji
            }

            if (!enableWalk)
            {
                if (e.KeyCode == Keys.Space)
                {
                    // Obliczenie odleg�o�ci pozycji kreski od �rodka obrazu
                    //int distanceFromCenter = Math.Abs(linePosition + lineWidth / 2 - 388 / 2);
                    int pictureBoxCenterX = pictureBoxHit.Width / 2;
                    int lineCenterX = linePosition + lineWidth / 2;
                    int distanceFromCenter = Math.Abs(lineCenterX - pictureBoxCenterX);

                    Debug.WriteLine("Width: " + pictureBoxHit.Width);
                    Debug.WriteLine("dystans: " + distanceFromCenter);

                    // Maksymalna odleg�o�� od �rodka PictureBox (maksymalna warto�� obra�e�)
                    int maxDistance = pictureBoxHit.Width / 2;

                    // Oblicz znormalizowan� odleg�o��
                    double normalizedDistance = (double)distanceFromCenter / maxDistance * 100;

                    // Warto�� obra�e� zale�na od znormalizowanej odleg�o�ci od �rodka
                    damage = Math.Max(100 - (int)normalizedDistance, 0);


                    Debug.WriteLine($"DAMAGE: {damage}");

                    damageLabel.Text = $"-{damage}";
                    damageLabel.Visible = true;

                    // Wy�wietlenie label z warto�ci� obra�e�
                    //ShowDamageLabel(damage);

                }
                return;
            }


            if (e.KeyCode == Keys.W)
            {
                //gdzie jestes?
                int x = player.Coordinates.XCoordinate;
                int y = player.Coordinates.YCoordinate;
                //w ktor� strone patrzysz?
                EDirection direction = player.Direction;
                //czy �ciana na ktora patrzysz nie jest solidna?
                if (game.Labirynth[x, y].Walls[(int)direction].WallType != EWallType.Solid)
                {
                    //tak -> mozna isc
                    //labirynt[x, y] idz tam gdzie patrzysz; +1 w danym kierunku
                    switch (direction)
                    {
                        case EDirection.North:
                            player.Coordinates.YCoordinate += 1;

                            break;
                        case EDirection.East:
                            player.Coordinates.XCoordinate += 1;

                            break;
                        case EDirection.South:
                            player.Coordinates.YCoordinate -= 1;

                            break;
                        case EDirection.West:
                            player.Coordinates.XCoordinate -= 1;

                            break;
                    }
                    // TODO: tutaj trzeba da� sprawdzanie czy nie wyjdzie za labirynt xd
                    stepSoundReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/walk_cutted.mp3"));
                    stepSoundPlayer = new WaveOutEvent();
                    stepSoundPlayer.Init(stepSoundReader);
                    stepSoundReader.Position = 0;
                    stepSoundPlayer.Play();
                    //rysuj pok�j
                    Invalidate();
                }

                //nie mozna isc i tyle
                //dzwi�k moze mozna dac jakis czy cos ewentualnie

                //MessageBox.Show("idziesz do przodu");
            }


            if (e.KeyCode == Keys.D) //odwr�cenie gracza w prawo
            {
                player.Direction = EnumExtensions.Next(player.Direction);

                Invalidate();
            }
            if (e.KeyCode == Keys.A) //odwr�cenie gracza w lewo
            {

                player.Direction = EnumExtensions.Previous(player.Direction);

                Invalidate();

            }

        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                MessageBox.Show("Left");
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;
            pictureBoxMonster.Visible = false;

            //POTW�R!!!
            try
            {
                if (game.Labirynth[x, y].HasMonster)
                {

                    hpBarEnemy.Maximum = game.Labirynth[x,y].Monster.HPMax; 
                    hpBarEnemy.Value = game.Labirynth[x, y].Monster.HPCurrent;
                    hpBarPlayer.Maximum = player.HPMax;
                    hpBarPlayer.Value = player.HPCurrent;
                    // 1. przygotuj pole walki
                    //zrob ciemno
                    panelBackground.Visible = true;
                    panelPlayerControls.Visible = true;

                    //blokuj chodzenie
                    enableWalk = false;
                    Monster monster = game.Labirynth[x, y].Monster;

                    Bitmap monsterImage = new Bitmap(monster.ImagePath);
                    //rysuj w picturebox
                    pictureBoxMonster.Visible = true;
                    pictureBoxMonster.Image = monsterImage;
                    backgroundMusicPlayer.Stop();
                    backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/bossFight1.mp3"));
                    backgroundMusicPlayer = new WaveOutEvent();
                    backgroundMusicPlayer.Init(backgroundMusicReader);
                    backgroundMusicPlayer.Volume = 0.7f;
                    backgroundMusicPlayer.Play();
                    return;
                }
            }
            catch (OutOfMemoryException ex)
            {
                Debug.WriteLine(ex.Data);
            }

            if (isGameOver)
            {
                return;

            }

            UpdateCompassPointerImage();
            Graphics g = e.Graphics;
            g.Clear(this.BackColor);

            //gdzie jestes?

            label1.Text = $"X: {x}; Y: {y}";

            //w ktor� strone patrzysz?
            EDirection direction = player.Direction;
            string d = direction.ToString();
            char f = char.ToUpper(d[0]);


            label2.Text = $"Facing: {direction}";
            label3.Text = $"{f}";

            //GetWall pierwszy argument to pozycja z "oczu gracza" czyli po prostu
            //wskazanie gdzie to zdj�cie ma si� wy�wietli� na ekranie
            //drugie to jak odwr�cony jest gracz
            //trzecie to koordynaty x oraz y pokoju
            //To jest chyba do zmiany bo to kosmiczne druciarstwo
            byte[] front = game.GetWall(EDirection.North, direction, x, y);
            byte[] left = game.GetWall(EDirection.West, direction.Previous(), x, y);
            byte[] right = game.GetWall(EDirection.East, direction.Next(), x, y);
            byte[] floor = game.GetFloor();

            //tak jest zajebiscie NIE RUSZAC!!!
            using (Image front_img = Image.FromStream(new MemoryStream(front)))
            using (Image left_img = Image.FromStream(new MemoryStream(left)))
            using (Image right_img = Image.FromStream(new MemoryStream(right)))
            using (Image floor_img = Image.FromStream(new MemoryStream(floor)))
            {
                g.DrawImage(floor_img, new Rectangle(0, 0, Width, Height));//G
                g.DrawImage(front_img, new Rectangle(0, 0, Width, Height));//G
                g.DrawImage(left_img, new Rectangle(0, 0, Width, Height));//G
                g.DrawImage(right_img, new Rectangle(0, 0, Width, Height));//G
            }

            using (Bitmap im = new Bitmap(Path.Combine("..", "..", "..", "..", "img/compass.png")))
            {
                g.DrawImage(im, new Rectangle(Width - 200, Height - 200, 165, 165));
                g.DrawImage(compassPointerImage, new Point(Width - 200, Height - 200));

            }


        }
        // Metoda aktualizuj�ca obr�t obrazu kompasu
        private void UpdateCompassPointerImage()
        {
            switch (player.Direction)
            {
                case EDirection.North:
                    compassPointerImage = new Bitmap(originalCompassImage);
                    compassPointerImage.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    break;
                case EDirection.East:
                    compassPointerImage = new Bitmap(originalCompassImage);
                    compassPointerImage.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case EDirection.South:
                    compassPointerImage = new Bitmap(originalCompassImage);
                    compassPointerImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                    break;
                case EDirection.West:
                    compassPointerImage = new Bitmap(originalCompassImage);
                    compassPointerImage.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
            }
        }

        //poruszanie sie potwora
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Oblicz now� pozycj� x i y
            xPosition = (int)(centerX + amplitudeX * Math.Sin(angle));
            yPosition = (int)(centerY + amplitudeY * Math.Sin(2 * angle));

            // Zwi�ksz k�t
            angle += angleStep;

            // Ustaw now� pozycj� pictureBox1
            pictureBoxMonster.Location = new Point(xPosition, yPosition);
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {
            //otworzyc ekwipunek
            //TODO
        }

        private void buttonFight_Click(object sender, EventArgs e)
        {
            //pictureBoxHit.Visible = true;
            //timerHitBox.Start();
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;
            Monster monster = game.Labirynth[x, y].Monster;
            monster.HPCurrent -= player.Strength;

            if (monster.HPCurrent <= 0)
            {
                game.Labirynth[x,y].Monster = null;
                game.Labirynth[x,y].HasMonster = false;
                hpBarEnemy.Value = 0;
                //pokaz pokoj
                panelBackground.Visible = false;
                //zmiana muzyki na background
                backgroundMusicPlayer.Stop();
                backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/main.mp3"));
                backgroundMusicPlayer = new WaveOutEvent();
                backgroundMusicPlayer.Init(backgroundMusicReader);
                backgroundMusicPlayer.Volume = 1f;
                backgroundMusicPlayer.Play();
                //wlacz chodzenie
                enableWalk = true;
            }
            else
            {
                hpBarEnemy.Value = monster.HPCurrent;
                Debug.WriteLine(player.Strength);
                playerRound = false;
                monsterAttack();
                //timerHitPoints.Start();

            }
        }


        private void pictureBoxHit_Paint(object sender, PaintEventArgs e)
        {
            // Rysowanie linii
            e.Graphics.FillRectangle(Brushes.White, linePosition, pictureBoxHit.Height / 2 - lineHeight / 2, lineWidth, lineHeight);
        }

        private void timerHitBox_Tick(object sender, EventArgs e)
        {
            // Poruszanie si� linii z lewej do prawej
            linePosition += 8;
            if (linePosition > pictureBoxHit.Width) // jesli wyjdzie za pole pictureBox
            {
                timerHitBox.Stop(); // Zatrzymaj timer
                linePosition = 0; // Zresetuj pozycj� linii
                pictureBoxHit.Visible = false; // Ukryj PictureBox
                damage = 0; // Zeruj obra�enia
            }

            pictureBoxHit.Invalidate(); // Od�wie�enie PictureBox
        }

        private async void monsterAttack()
        {
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;

            Monster monster = game.Labirynth[x, y].Monster;
            //potwor bije
            if (!playerRound)
            {
                //czy potwor zyje
                if (monster.HPCurrent > 0)
                {
                    //przygotuj

                    //wylacz przyciski
                    buttonFight.Enabled = false;
                    buttonItem.Enabled = false;
                    buttonItem.BackColor = Color.Gray;
                    buttonFight.BackColor = Color.Gray;
                    await Task.Delay(1000);
                    //animacja bicia
                    //AnimateFade(Color.Red, Color.Transparent, 1000); // Animacja od czerwonego do przezroczystego w ci�gu 1 sekundy
                    //damage
                    Random rnd = new Random();
                    int damageToPlayer = rnd.Next(monster.Strength - monster.Strength / 2, monster.Strength + monster.Strength / 2);
                    player.HPCurrent -= damageToPlayer;
                    labelHpPlayer.Text = $"HP {player.HPCurrent} / {player.HPMax}";
                    if (player.HPCurrent <= 0)
                    {
                        panelOverlay.Visible = true;
                        hpBarPlayer.Value = 0;
                        backgroundMusicPlayer.Stop();
                        backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/game_over.mp3"));
                        backgroundMusicPlayer = new WaveOutEvent();
                        backgroundMusicPlayer.Init(backgroundMusicReader);
                        backgroundMusicPlayer.Volume = 0.7f;

                        backgroundMusicPlayer.Play();
                        return;
                    }
                    hpBarPlayer.Value -= damageToPlayer;
                    Debug.WriteLine($"damage playera: {damageToPlayer}");
                    await Task.Delay(500);
                    buttonFight.Enabled = true;
                    buttonItem.Enabled = true;
                    buttonItem.BackColor = Color.White;
                    buttonFight.BackColor = Color.White;
                    playerRound = true;
                }
            }
        }

        private void panelOverlay_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonReplay_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("jezcze raaaaaaaaaaaaaz");
            enableWalk = true;
            setMonster = false;
            isGameOver = false;
            player = new Player("player", null, 100, 100, 30, 20);
            game = new Game(player);
            Invalidate();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
