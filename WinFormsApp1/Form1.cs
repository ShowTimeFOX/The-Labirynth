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
        private WaveOutEvent otherMusicPlayer;
        private AudioFileReader otherMusicReader;
        private Game game;
        private Player player;
        private Monster monster;
        private Image compassPointerImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));
        private Image originalCompassImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));

        int xPosition = 50;
        int yPosition = 50;
        int centerX = 150; // årodkowy punkt Ûsemki w poziomie
        int centerY = 150; // årodkowy punkt Ûsemki w pionie
        int amplitudeX = 100; // Amplituda Ûsemki w poziomie
        int amplitudeY = 50; // Amplituda Ûsemki w pionie
        double angle = 0; // Kπt do obliczania pozycji
        double angleStep = 0.1; // Krok zmiany kπta dla prÍdkoúci ruchu

        bool enableWalk = true;
        bool setMonster = false;
        bool isGameOver = false;

        private int linePosition; // Pozycja pozioma linii
        private int lineWidth; // SzerokoúÊ linii
        private int lineHeight; // WysokoúÊ linii
        private int lineSpeed = 10; // szybkosc linii
        private bool enableSpace; // Flaga okreúlajπca, czy spacja mzoe byc nacisnieta
        private int damage; // WartoúÊ uszkodzenia zadawanego przez gracza

        private bool playerRound = true; //czyja kolej na bicie

        static int animationStep = 0;
        static int animationStepPlayer = 0;

        private TaskCompletionSource<bool> spaceKeyPressTcs;

        public Form1()
        {
            player = new Player("Jacek", null, 100, 100, 30, 20);
            game = new Game(player);
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/main.wav"));
            backgroundMusicPlayer = new WaveOutEvent();
            backgroundMusicPlayer.Init(backgroundMusicReader);
            backgroundMusicReader.Volume = 0.7f;
            //backgroundMusicPlayer.PlaybackStopped += OnPlaybackStopped; //podwaja poczatek
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
            /////////////////////////////////////////////////////////////////
            //labelDamagePlayer.BringToFront();
            //panelPlayerControls.Controls.Add(labelDamagePlayer);

            panelPlayerControls.Controls.Add(buttonFight);
            panelPlayerControls.Controls.Add(buttonItem);

            hpBarMonster.Minimum = 0;
            hpBarMonster.Maximum = 100; // Maksymalna wartoúÊ HP
            hpBarMonster.Value = 100; // Aktualna wartoúÊ HP
            hpBarMonster.ForeColor = Color.Red; //to to nie dziala
            panelBackground.Controls.Add(hpBarMonster);
            panelBackground.Controls.Add(labelHpMonster);

            labelDamage.Location = new Point(Width / 2, 115);
            panelBackground.Controls.Add(labelDamage);

            hpBarPlayer.Minimum = 0;
            hpBarPlayer.Maximum = 100;
            hpBarPlayer.Value = 100;

            labelHpPlayer.Text = $"{player.Name} HP {player.HPCurrent} / {player.HPMax}";

            //pictureBox1.BackColor = Color.Transparent; - gÛwno psuje animacje
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
            e.Handled = true;
            e.SuppressKeyPress = true;// to zapobiega temu dziwkowi systemowemu
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;

            if (e.KeyCode == Keys.Escape)
            {
                // Wyúwietlenie komunikatu potwierdzajπcego wyjúcie
                //DialogResult result = MessageBox.Show("Czy na pewno chcesz wyjúÊ?", "Potwierdzenie wyjúcia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //// Jeúli uøytkownik potwierdzi, zamknij aplikacjÍ
                //if (result == DialogResult.Yes)
                //{
                //    this.Close(); // ZamkniÍcie aplikacji
                //}
                this.Close(); // ZamkniÍcie aplikacji
            }

            if (!enableWalk && enableSpace)
            {
                Debug.WriteLine("spacja start");
                if (e.KeyCode == Keys.Space)
                {
                    Debug.WriteLine("spacja spacja");

                    // Obliczenie odleg≥oúci pozycji kreski od úrodka obrazu
                    int pictureBoxCenterX = pictureBoxHit.Width / 2;
                    int lineCenterX = linePosition + lineWidth / 2;
                    int distanceFromCenter = Math.Abs(lineCenterX - pictureBoxCenterX);

                    // Maksymalna odleg≥oúÊ od úrodka PictureBox (maksymalna wartoúÊ obraøeÒ)
                    int maxDistance = pictureBoxHit.Width / 2;

                    // Oblicz znormalizowanπ odleg≥oúÊ
                    double normalizedDistance = (double)distanceFromCenter / maxDistance * 100;

                    // WartoúÊ obraøeÒ zaleøna od znormalizowanej odleg≥oúci od úrodka
                    int damage2 = Math.Max(100 - (int)normalizedDistance, 0);
                    damage = (player.Strength + damage2) / 10;
                    //Debug.WriteLine($"DAMAGE: {damage}");

                    labelDamage.Text = $"-{damage}";
                    labelDamage.Visible = true;

                    monster.HPCurrent -= damage;
                    if (monster.HPCurrent > 0)
                        hpBarMonster.Value = monster.HPCurrent;

                    if (monster.HPCurrent <= 0)
                    {
                        //potwor umar≥
                        game.Labirynth[x, y].Monster = null;
                        game.Labirynth[x, y].HasMonster = false;
                        hpBarMonster.Value = 0;
                        //pokaz pokoj
                        panelBackground.Visible = false;
                        //muzyka zwyciestwo
                        otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/win.mp3"));
                        otherMusicPlayer = new WaveOutEvent();
                        otherMusicPlayer.Init(otherMusicReader);
                        otherMusicPlayer.Volume = 1f;
                        otherMusicPlayer.Play();
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

                    labelHpMonster.Text = $"{monster.Name} HP {monster.HPCurrent} / {monster.HPMax}";

                    //dzwiek potwora
                    //otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/ouch1.mp3"));
                    //otherMusicPlayer = new WaveOutEvent();
                    //otherMusicPlayer.Init(otherMusicReader);
                    //otherMusicPlayer.Volume = 0.5f;
                    //otherMusicPlayer.Play();

                    // Wyúwietlenie label z wartoúciπ obraøeÒ
                    enableSpace = false;
                    linePosition = 0;
                    pictureBoxHit.Visible = false;
                    timerHitBox.Stop();
                    timerHitPoints.Start();
                }
                spaceKeyPressTcs.TrySetResult(true);
                return; // to tu MUSI byc
            }


            if (e.KeyCode == Keys.W)
            {
                //gdzie jestes?
                //int x = player.Coordinates.XCoordinate;
                //int y = player.Coordinates.YCoordinate;
                //w ktorπ strone patrzysz?
                EDirection direction = player.Direction;
                //czy úciana na ktora patrzysz nie jest solidna?
                if (game.Labirynth[x, y].Walls[(int)direction].WallType != EWallType.Solid)
                {
                    //tak -> mozna isc
                    //labirynt[x, y] idz tam gdzie patrzysz; +1 w danym kierunku
                    switch (direction)
                    {
                        case EDirection.North:
                            player.Coordinates.YCoordinate += 1;
                            if (!game.Map.discoveredMapCoordinates.Contains(player.Coordinates))
                            {
                                game.Map.discoveredMapCoordinates.Add(new Coordinates(player.Coordinates.XCoordinate, player.Coordinates.YCoordinate));
                            }
                            //trzeba to bedzie poprawiÊ ALE TO P”èNIEJ
                            break;
                        case EDirection.East:
                            player.Coordinates.XCoordinate += 1;
                            //Dodawanie do mapy miejsc w ktÛrych by≥ gracz to jest straszne od strony kodu...
                            if (!game.Map.discoveredMapCoordinates.Contains(player.Coordinates))
                            {
                                game.Map.discoveredMapCoordinates.Add(new Coordinates(player.Coordinates.XCoordinate, player.Coordinates.YCoordinate));
                            }
                            break;
                        case EDirection.South:
                            player.Coordinates.YCoordinate -= 1;
                            if (!game.Map.discoveredMapCoordinates.Contains(player.Coordinates))
                            {
                                game.Map.discoveredMapCoordinates.Add(new Coordinates(player.Coordinates.XCoordinate, player.Coordinates.YCoordinate));
                            }
                            break;
                        case EDirection.West:
                            player.Coordinates.XCoordinate -= 1;
                            if (!game.Map.discoveredMapCoordinates.Contains(player.Coordinates))
                            {
                                game.Map.discoveredMapCoordinates.Add(new Coordinates(player.Coordinates.XCoordinate, player.Coordinates.YCoordinate));
                            }
                            break;
                    }
                    // TODO: tutaj trzeba daÊ sprawdzanie czy nie wyjdzie za labirynt xd
                    stepSoundReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/walk_cutted.mp3"));
                    stepSoundPlayer = new WaveOutEvent();
                    stepSoundPlayer.Init(stepSoundReader);
                    stepSoundReader.Position = 0;
                    stepSoundPlayer.Play();
                    //rysuj pokÛj
                    Invalidate();
                }
            }
            if (e.KeyCode == Keys.D) //odwrÛcenie gracza w prawo
            {
                player.Direction = EnumExtensions.Next(player.Direction);

                Invalidate();
            }
            if (e.KeyCode == Keys.A) //odwrÛcenie gracza w lewo
            {
                player.Direction = EnumExtensions.Previous(player.Direction);
                Invalidate();
            }
            if (e.KeyCode == Keys.M)
            {
                game.Map.isMapShown = !game.Map.isMapShown; //To po prostu jeúli nie ma mapy pokaø, jak jest to nie pokazuj
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

            //POTW”R!!!
            try
            {
                if (game.Labirynth[x, y].HasMonster)
                {
                    hpBarMonster.Maximum = game.Labirynth[x, y].Monster.HPMax;
                    hpBarMonster.Value = game.Labirynth[x, y].Monster.HPCurrent;
                    hpBarPlayer.Maximum = player.HPMax;
                    hpBarPlayer.Value = player.HPCurrent;
                    labelHpPlayer.Text = $"{player.Name} HP {player.HPCurrent} / {player.HPMax}";
                    // 1. przygotuj pole walki
                    //zrob ciemno
                    panelBackground.Visible = true;
                    panelPlayerControls.Visible = true;

                    //blokuj chodzenie
                    enableWalk = false;
                    monster = game.Labirynth[x, y].Monster;
                    labelHpMonster.Text = $"{monster.Name} HP {monster.HPCurrent} / {monster.HPMax}";

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

            //w ktorπ strone patrzysz?
            EDirection direction = player.Direction;
            string d = direction.ToString();
            char f = char.ToUpper(d[0]);


            label2.Text = $"Facing: {direction}";
            label3.Text = $"{f}";

            //GetWall pierwszy argument to pozycja z "oczu gracza" czyli po prostu
            //wskazanie gdzie to zdjÍcie ma siÍ wyúwietliÊ na ekranie
            //drugie to jak odwrÛcony jest gracz
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

            if(game.Map.isMapShown == true) //OdczÛwam bÛl jak patrzÍ na to gÛwno....
            {
                using (Image bigMap_img = Image.FromStream(new MemoryStream(game.Map.BigMap)))
                {
                    g.DrawImage(bigMap_img, new Rectangle(0, 0, Width, Height));
                }

                List<Coordinates> cordy = game.Map.discoveredMapCoordinates;

                // Tym moøna przesuwaÊ gdzie ta mapa ma siÍ wyúwietlac na tym zdjÍciu tej poszarpanej mapy
                int baseXposition = 1500;
                int baseYposition = 8500;
                foreach (var w in cordy)
                {
                    byte[] fragment = game.Map.mapFragments[w];
                    int XCoordinate = w.XCoordinate;
                    int YCoordinate = w.YCoordinate;

                    int Xpostion = baseXposition + XCoordinate * 1910;
                    int Ypostion = baseYposition + YCoordinate * (-1000);

                    if(player.Coordinates.Equals(w))
                    {
                        //int newWidth = Width - 500;
                        //int newHeight = Height - 200;
                        int pointerXPostion = Xpostion + 250;
                        int pointerYPostion = Ypostion + 100;
                        using (Image pointerTodraw = Image.FromStream(new MemoryStream(game.Map.pointer)))
                        {
                            float scaleFactor = 0.1f;
                            g.ScaleTransform(scaleFactor, scaleFactor);

                            g.DrawImage(pointerTodraw, new Rectangle(pointerXPostion, pointerYPostion, 1400, 800));
                        }
                        g.ResetTransform();
                    }

                    using (Image fragmentToDraw = Image.FromStream(new MemoryStream(fragment)))
                    {
                        float scaleFactor = 0.1f;
                        g.ScaleTransform(scaleFactor, scaleFactor);
                        g.DrawImage(fragmentToDraw, new Rectangle(Xpostion, Ypostion, Width, Height));
                    }
                    g.ResetTransform();
                }

            }

        }
        // Metoda aktualizujπca obrÛt obrazu kompasu
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
            // Oblicz nowπ pozycjÍ x i y
            xPosition = (int)(centerX + amplitudeX * Math.Sin(angle));
            yPosition = (int)(centerY + amplitudeY * Math.Sin(2 * angle));

            // ZwiÍksz kπt
            angle += angleStep;

            // Ustaw nowπ pozycjÍ pictureBox1
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
            linePosition = 0;
            timerHitBox.Start();
            playerRound = false;

            spaceKeyPressTcs = new TaskCompletionSource<bool>();
            await spaceKeyPressTcs.Task;
            await monsterAttack();
        }


        private void pictureBoxHit_Paint(object sender, PaintEventArgs e)
        {
            // Rysowanie linii
            e.Graphics.FillRectangle(Brushes.White, linePosition, pictureBoxHit.Height / 2 - lineHeight / 2, lineWidth, lineHeight);
        }

        private void timerHitBox_Tick(object sender, EventArgs e)
        {
            // Poruszanie siÍ linii z lewej do prawej
            linePosition += lineSpeed;
            if (linePosition > pictureBoxHit.Width) // jesli wyjdzie za pole pictureBox
            {
                linePosition = 0; // Zresetuj pozycjÍ linii
                pictureBoxHit.Visible = false; // Ukryj PictureBox
                damage = 0; // Zeruj obraøenia
                labelDamage.Text = "MISS";
                spaceKeyPressTcs.TrySetResult(true);
            }
            pictureBoxHit.Invalidate();
        }

        private async Task monsterAttack()
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
                    Random rnd = new Random();
                    int damageToPlayer = rnd.Next(monster.Strength - monster.Strength / 2, monster.Strength + monster.Strength / 2);
                    player.HPCurrent -= damageToPlayer;
                    labelHpPlayer.Text = $"{player.Name} HP {player.HPCurrent} / {player.HPMax}";
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

                    //player dzwiek obrazen
                    //otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/player_hurt.mp3"));
                    //otherMusicPlayer = new WaveOutEvent();
                    //otherMusicPlayer.Init(otherMusicReader);
                    //otherMusicPlayer.Volume = 0.5f;
                    //otherMusicPlayer.Play();
                    //Debug.WriteLine(labelDamagePlayer.Location);
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
            labelDamage.Top -= 3;
            animationStep++;
            if (animationStep == 25)
            {
                labelDamage.Location = new Point(Width / 2, 115);
                animationStep = 0;
                labelDamage.Visible = false;
                timerHitPoints.Stop();
            }
        }

        private void timerHitPointsPlayer_Tick(object sender, EventArgs e)
        {
            labelDamagePlayer.Top -= 3;
            animationStepPlayer++;
            if (animationStepPlayer == 25)
            {
                labelDamagePlayer.Location = new Point(1113, 800);

                //labelDamagePlayer.Location = new Point(panelPlayerControls.Width, Height - panelPlayerControls.Height);
                animationStepPlayer = 0;
                labelDamagePlayer.Visible = false;
                timerHitPointsPlayer.Stop();
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            // Ustawienie pozycji na poczπtek i ponowne odtwarzanie
            backgroundMusicReader.Position = 0;
            backgroundMusicPlayer.Play();
        }

        private void pictureBoxHit_Click(object sender, EventArgs e)
        {

        }

        private void labelDamagePlayer_Click(object sender, EventArgs e)
        {

        }
    }
}
