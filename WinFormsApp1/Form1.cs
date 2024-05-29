using GameLibrary;
using NAudio.Dmo;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Media;
using System.Threading;
using System.Timers;
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
        private Monster monster;
        private Image compassPointerImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));
        private Image originalCompassImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));

        int xPosition = 50;
        int yPosition = 50;
        int centerX = 150; // Œrodkowy punkt ósemki w poziomie
        int centerY = 150; // Œrodkowy punkt ósemki w pionie
        int amplitudeX = 100; // Amplituda ósemki w poziomie
        int amplitudeY = 50; // Amplituda ósemki w pionie
        double angle = 0; // K¹t do obliczania pozycji
        double angleStep = 0.1; // Krok zmiany k¹ta dla prêdkoœci ruchu

        bool enableWalk = true;
        bool setMonster = false;
        bool isGameOver = false;

        private int linePosition; // Pozycja pozioma linii
        private int lineWidth; // Szerokoœæ linii
        private int lineHeight; // Wysokoœæ linii
        private int lineSpeed = 8; // szybkosc linii
        private bool enableSpace; // Flaga okreœlaj¹ca, czy spacja zosta³a naciœniêta
        private int damage; // Wartoœæ uszkodzenia zadawanego przez gracza

        private bool playerRound = true; //czyja kolej na bicie

        static int animationStep = 0;
        static AutoResetEvent animationFinished = new AutoResetEvent(false);

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
            backgroundMusicReader.Volume = 0.7f; // Przyk³adowo: 20% g³oœnoœci
            backgroundMusicPlayer.Play();

            //hitBox
            linePosition = 0;
            lineWidth = 10;
            lineHeight = pictureBoxHit.Height;
            damage = 0;
            enableSpace = false;

            panelBackground.Visible = false;
            panelPlayerControls.Visible = false;
            panelBackground.Location = new Point(0, 0);
            panelBackground.Size = new Size(Width, Height);
            panelBackground.Controls.Add(pictureBoxMonster);

            panelBackground.Controls.Add(panelPlayerControls);

            panelPlayerControls.Controls.Add(buttonFight);
            panelPlayerControls.Controls.Add(buttonItem);
            //pictureBoxHit.Size = new Size(panelPlayerControls.Width, panelPlayerControls.Height);

            //HP BAR przeciwnik
            //hpBarMonster.Location = new Point(panelBackground.Width - 300, 100);
            //hpBarMonster.Size = new Size(200, 30);
            hpBarMonster.Minimum = 0;
            hpBarMonster.Maximum = 100; // Maksymalna wartoœæ HP
            hpBarMonster.Value = 100; // Aktualna wartoœæ HP
            hpBarMonster.ForeColor = Color.Red; //to to nie dziala
            panelBackground.Controls.Add(hpBarMonster);
            panelBackground.Controls.Add(labelHpMonster);

            labelDamage.Location = new Point(Width / 2, 115);
            panelBackground.Controls.Add(labelDamage);

            hpBarPlayer.Minimum = 0;
            hpBarPlayer.Maximum = 100;
            hpBarPlayer.Value = 100;

            labelHpPlayer.Text = $"HP {player.HPCurrent} / {player.HPMax}";

            //pictureBox1.BackColor = Color.Transparent; - gówno psuje animacje
            pictureBoxMonster.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxMonster.Visible = false;
            centerX = Width / 2 - 165;
            centerY = Height / 2 - 340;
            timerBossMotion.Start();

            panelBackground.Controls.Add(labelDamagePlayer);

            panelBackground.Controls.Add(panelOverlay);
            panelOverlay.BringToFront();

            panelOverlay.Visible = false;
            panelOverlay.Location = new Point(Width / 2 - panelOverlay.Width / 2, Height / 2 - panelOverlay.Height / 2);

            panelOverlay.Controls.Add(labelGameOver);
            panelOverlay.Controls.Add(buttonReplay);
            panelOverlay.Controls.Add(buttonExit);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;

            if (e.KeyCode == Keys.Escape)
            {
                // Wyœwietlenie komunikatu potwierdzaj¹cego wyjœcie
                //DialogResult result = MessageBox.Show("Czy na pewno chcesz wyjœæ?", "Potwierdzenie wyjœcia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //// Jeœli u¿ytkownik potwierdzi, zamknij aplikacjê
                //if (result == DialogResult.Yes)
                //{
                //    this.Close(); // Zamkniêcie aplikacji
                //}
                this.Close(); // Zamkniêcie aplikacji
            }

            if (!enableWalk && enableSpace)
            {
                if (e.KeyCode == Keys.Space)
                {
                    // Obliczenie odleg³oœci pozycji kreski od œrodka obrazu
                    //int distanceFromCenter = Math.Abs(linePosition + lineWidth / 2 - 388 / 2);
                    int pictureBoxCenterX = pictureBoxHit.Width / 2;
                    int lineCenterX = linePosition + lineWidth / 2;
                    int distanceFromCenter = Math.Abs(lineCenterX - pictureBoxCenterX);

                    //Debug.WriteLine("Width: " + pictureBoxHit.Width);
                    //Debug.WriteLine("dystans: " + distanceFromCenter);

                    // Maksymalna odleg³oœæ od œrodka PictureBox (maksymalna wartoœæ obra¿eñ)
                    int maxDistance = pictureBoxHit.Width / 2;

                    // Oblicz znormalizowan¹ odleg³oœæ
                    double normalizedDistance = (double)distanceFromCenter / maxDistance * 100;

                    // Wartoœæ obra¿eñ zale¿na od znormalizowanej odleg³oœci od œrodka
                    int damage2 = Math.Max(100 - (int)normalizedDistance, 0);
                    damage = (player.Strength + damage2) / 10;


                    Debug.WriteLine($"DAMAGE: {damage}");

                    labelDamage.Text = $"-{damage}";
                    labelDamage.Visible = true;

                    //monster.HPCurrent -= player.Strength;
                    monster.HPCurrent -= damage;
                    if (monster.HPCurrent > 0)
                        hpBarMonster.Value = monster.HPCurrent;


                    if (monster.HPCurrent <= 0)
                    {
                        //potwor umar³
                        game.Labirynth[x, y].Monster = null;
                        game.Labirynth[x, y].HasMonster = false;
                        hpBarMonster.Value = 0;
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
                        //zwieksz szybkosc linii
                        lineSpeed += 2;
                    }

                    labelHpMonster.Text = $"HP {monster.HPCurrent} / {monster.HPMax}";

                    // Wyœwietlenie label z wartoœci¹ obra¿eñ
                    //ShowDamageLabel(damage);
                    enableSpace = false;
                    linePosition = 0;
                    pictureBoxHit.Visible = false;
                    timerHitBox.Stop();
                    timerHitPoints.Start();
                }
                return; // to tu MUSI byc
            }


            if (e.KeyCode == Keys.W)
            {
                //gdzie jestes?
                //int x = player.Coordinates.XCoordinate;
                //int y = player.Coordinates.YCoordinate;
                //w ktor¹ strone patrzysz?
                EDirection direction = player.Direction;
                //czy œciana na ktora patrzysz nie jest solidna?
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
                    // TODO: tutaj trzeba daæ sprawdzanie czy nie wyjdzie za labirynt xd
                    stepSoundReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/walk_cutted.mp3"));
                    stepSoundPlayer = new WaveOutEvent();
                    stepSoundPlayer.Init(stepSoundReader);
                    stepSoundReader.Position = 0;
                    stepSoundPlayer.Play();
                    //rysuj pokój
                    Invalidate();
                }

                //nie mozna isc i tyle
                //dzwiêk moze mozna dac jakis czy cos ewentualnie

                //MessageBox.Show("idziesz do przodu");
            }


            if (e.KeyCode == Keys.D) //odwrócenie gracza w prawo
            {
                player.Direction = EnumExtensions.Next(player.Direction);

                Invalidate();
            }
            if (e.KeyCode == Keys.A) //odwrócenie gracza w lewo
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

            //POTWÓR!!!
            try
            {
                if (game.Labirynth[x, y].HasMonster)
                {
                    hpBarMonster.Maximum = game.Labirynth[x, y].Monster.HPMax;
                    hpBarMonster.Value = game.Labirynth[x, y].Monster.HPCurrent;
                    hpBarPlayer.Maximum = player.HPMax;
                    hpBarPlayer.Value = player.HPCurrent;
                    labelHpPlayer.Text = $"HP {player.HPCurrent} / {player.HPMax}";
                    // 1. przygotuj pole walki
                    //zrob ciemno
                    panelBackground.Visible = true;
                    panelPlayerControls.Visible = true;

                    //blokuj chodzenie
                    enableWalk = false;
                    monster = game.Labirynth[x, y].Monster;
                    labelHpMonster.Text = $"HP {monster.HPCurrent} / {monster.HPMax}";

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

            //w ktor¹ strone patrzysz?
            EDirection direction = player.Direction;
            string d = direction.ToString();
            char f = char.ToUpper(d[0]);


            label2.Text = $"Facing: {direction}";
            label3.Text = $"{f}";

            //GetWall pierwszy argument to pozycja z "oczu gracza" czyli po prostu
            //wskazanie gdzie to zdjêcie ma siê wyœwietliæ na ekranie
            //drugie to jak odwrócony jest gracz
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
        // Metoda aktualizuj¹ca obrót obrazu kompasu
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
            // Oblicz now¹ pozycjê x i y
            xPosition = (int)(centerX + amplitudeX * Math.Sin(angle));
            yPosition = (int)(centerY + amplitudeY * Math.Sin(2 * angle));

            // Zwiêksz k¹t
            angle += angleStep;

            // Ustaw now¹ pozycjê pictureBox1
            pictureBoxMonster.Location = new Point(xPosition, yPosition);
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {
            //otworzyc ekwipunek
            //TODO
        }

        private async void buttonFight_Click(object sender, EventArgs e)
        {
            enableSpace = true;
            //walka
            pictureBoxHit.Visible = true;
            timerHitBox.Start();
            playerRound = false;
            Thread.Sleep(15); // Czas na prze³¹czenie tur
            monsterAttack();
        }


        private void pictureBoxHit_Paint(object sender, PaintEventArgs e)
        {
            // Rysowanie linii
            e.Graphics.FillRectangle(Brushes.White, linePosition, pictureBoxHit.Height / 2 - lineHeight / 2, lineWidth, lineHeight);
        }

        private void timerHitBox_Tick(object sender, EventArgs e)
        {
            // Poruszanie siê linii z lewej do prawej
            linePosition += lineSpeed;
            if (linePosition > pictureBoxHit.Width) // jesli wyjdzie za pole pictureBox
            {
                //timerHitBox.Stop(); // Zatrzymaj timer
                linePosition = 0; // Zresetuj pozycjê linii
                pictureBoxHit.Visible = false; // Ukryj PictureBox
                damage = 0; // Zeruj obra¿enia
                labelDamage.Text = "MISS";

            }
            pictureBoxHit.Invalidate();
        }

        private async void monsterAttack()
        {
            //potwor bije
            if (!playerRound)
            {
                //czy potwor zyje
                if (monster != null && monster.HPCurrent > 0)
                {
                    //wylacz przyciski
                    buttonFight.Enabled = false;
                    buttonItem.Enabled = false;
                    buttonItem.BackColor = Color.Gray;
                    buttonFight.BackColor = Color.Gray;
                    await Task.Delay(1000);
                    //animacja bicia
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

                        buttonFight.Enabled = true;
                        buttonItem.Enabled = true;
                        buttonItem.BackColor = Color.White;
                        buttonFight.BackColor = Color.White;
                        return;
                    }
                    hpBarPlayer.Value -= damageToPlayer;
                    //Debug.WriteLine($"damage playera: {damageToPlayer}");
                    labelDamagePlayer.Visible = true;
                    labelDamagePlayer.Text = $"-{damageToPlayer}";
                    timerHitPointsPlayer.Start();
                    await Task.Delay(1000);
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
            enableWalk = true;
            setMonster = false;
            isGameOver = false;
            playerRound = true;
            player = new Player("player", null, 100, 100, 30, 20);
            game = new Game(player);
            panelOverlay.Visible = false;
            panelBackground.Visible = false;
            buttonFight.Enabled = true;
            buttonItem.Enabled = true;
            buttonFight.BackColor = Color.White;
            buttonItem.BackColor = Color.White;
            Invalidate();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelHpPlayer_Click(object sender, EventArgs e)
        {

        }

        private void timerHitPoints_Tick(object sender, EventArgs e)
        {
            labelDamage.Top -= 5;
            animationStep++;
            if (animationStep == 15)
            {
                labelDamage.Location = new Point(Width / 2, 115);
                animationStep = 0;
                labelDamage.Visible = false;
                timerHitPoints.Stop();
            }
        }

        private void timerHitPointsPlayer_Tick(object sender, EventArgs e)
        {
            labelDamagePlayer.Top -= 5;
            animationStep++;
            if (animationStep == 15)
            {
                labelDamagePlayer.Location = new Point(974, 600);
                animationStep = 0;
                labelDamagePlayer.Visible = false;
                timerHitPointsPlayer.Stop();
            }
        }
    }
}
