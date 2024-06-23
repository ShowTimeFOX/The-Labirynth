using GameLibrary;
using Microsoft.VisualBasic.Devices;
using NAudio.Dmo;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Media;
using System.Runtime.InteropServices;
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

        private SoundPlayer musicPlayer;

        private WaveOutEvent stepSoundPlayer;
        private AudioFileReader stepSoundReader;
        private WaveOutEvent otherMusicPlayer;
        private AudioFileReader otherMusicReader;
        private Game game;
        private Player player;
        private Monster monster;
        private Image compassPointerImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));
        private Image originalCompassImage = Image.FromFile(Path.Combine("..", "..", "..", "..", "img/compass_pointer.png"));
        private Rectangle imageRectangle;

        int xPosition = 50;
        int yPosition = 50;
        int centerX = 150; // �rodkowy punkt �semki w poziomie
        int centerY = 150; // �rodkowy punkt �semki w pionie
        int amplitudeX = 100; // Amplituda �semki w poziomie
        int amplitudeY = 50; // Amplituda �semki w pionie
        double angle = 0; // K�t do obliczania pozycji
        double angleStep = 0.1; // Krok zmiany k�ta dla pr�dko�ci ruchu
        int mouseX = 0;
        int mouseY = 0;

        bool enableWalk = true;
        bool setMonster = false;
        bool isGameOver = false;

        private int linePosition; // Pozycja pozioma linii
        private int lineWidth; // Szeroko�� linii
        private int lineHeight; // Wysoko�� linii
        private int lineSpeed = 10; // szybkosc linii
        private bool enableSpace; // Flaga okre�laj�ca, czy spacja mzoe byc nacisnieta
        private int damage; // Warto�� uszkodzenia zadawanego przez gracza

        private bool playerRound = true; //czyja kolej na bicie

        static int animationStep = 0;
        static int animationStepPlayer = 0;

        private TaskCompletionSource<bool> spaceKeyPressTcs;

        private bool isFinalBoss = false;

        public Form1()
        {
            player = new Player("Dzban", null, 100, 100, 30, 20);
            game = new Game(player);
            this.MouseClick += Form1_MouseClick;
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/main.wav"));
            //backgroundMusicPlayer = new WaveOutEvent();
            //backgroundMusicPlayer.Init(backgroundMusicReader);
            //backgroundMusicReader.Volume = 0.7f;
            //backgroundMusicPlayer.Play();
            ///////////
            musicPlayer = new SoundPlayer();
            musicPlayer.SoundLocation = Path.Combine("..", "..", "..", "..", "sounds/main.wav");
            musicPlayer.PlayLooping();


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
            hpBarMonster.Maximum = 100; // Maksymalna warto�� HP
            hpBarMonster.Value = 100; // Aktualna warto�� HP
            hpBarMonster.ForeColor = Color.Red; //to to nie dziala
            panelBackground.Controls.Add(hpBarMonster);
            panelBackground.Controls.Add(labelHpMonster);

            labelDamage.Location = new Point(Width / 2, 115);
            panelBackground.Controls.Add(labelDamage);

            hpBarPlayer.Minimum = 0;
            hpBarPlayer.Maximum = 100;
            hpBarPlayer.Value = 100;

            labelHpPlayer.Text = $"{player.Name} HP {player.HPCurrent} / {player.HPMax}";

            //pictureBox1.BackColor = Color.Transparent; - g�wno psuje animacje
            pictureBoxMonster.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxMonster.Visible = false;


            centerX = Width / 2 - pictureBoxMonster.Width / 2;
            centerY = Height / 2 - pictureBoxMonster.Height / 2 - 100; // jak bedzie giga boss to nie moze zaslaniac obslugi walki
            //Debug.WriteLine("WHOAAAA "+pictureBoxMonster.Width);
            timerBossMotion.Start();

            panelBackground.Controls.Add(labelDamagePlayer);

            panelBackground.Controls.Add(panelOverlay);
            panelOverlay.BringToFront();

            panelOverlay.Visible = false;
            panelOverlay.Location = new Point(Width / 2 - panelOverlay.Width / 2, Height / 2 - panelOverlay.Height / 2);

            panelOverlay.Controls.Add(labelGameOver);
            panelOverlay.Controls.Add(buttonReplay);
            panelOverlay.Controls.Add(buttonExit);
            labelKomunikat.Text = $"STRENGTH: {player.Strength}";
            labelHpPlayer.Text = $"{player.Name} HP {player.HPCurrent} / {player.HPMax}";

            labelKomunikat.Visible = true;
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;// to zapobiega temu dziwkowi systemowemu
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;

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

            if (!enableWalk && enableSpace)
            {
                Debug.WriteLine("spacja start");
                if (e.KeyCode == Keys.Space)
                {
                    Debug.WriteLine("spacja spacja");

                    // Obliczenie odleg�o�ci pozycji kreski od �rodka obrazu
                    int pictureBoxCenterX = pictureBoxHit.Width / 2;
                    int lineCenterX = linePosition + lineWidth / 2;
                    int distanceFromCenter = Math.Abs(lineCenterX - pictureBoxCenterX);

                    // Maksymalna odleg�o�� od �rodka PictureBox (maksymalna warto�� obra�e�)
                    int maxDistance = pictureBoxHit.Width / 2;

                    // Oblicz znormalizowan� odleg�o��
                    double normalizedDistance = (double)distanceFromCenter / maxDistance * 100;

                    // Warto�� obra�e� zale�na od znormalizowanej odleg�o�ci od �rodka
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
                        //potwor umar�
                        game.Labirynth[x, y].Monster = null;
                        game.Labirynth[x, y].HasMonster = false;
                        hpBarMonster.Value = 0;
                        //pokaz pokoj
                        panelBackground.Visible = false;
                        //muzyka zwyciestwo                        
                        otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/win.mp3"));
                        if (isFinalBoss)
                        {

                            otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/barczak/stawiam ci 5.mp3"));
                            timerVoice.Stop();
                        }
                        otherMusicPlayer = new WaveOutEvent();
                        otherMusicPlayer.Init(otherMusicReader);
                        otherMusicPlayer.Volume = 1f;
                        otherMusicPlayer.Play();
                        //zmiana muzyki na background
                        //backgroundMusicPlayer.Stop();
                        musicPlayer.Stop();
                        //backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/main.mp3"));
                        //backgroundMusicPlayer = new WaveOutEvent();
                        //backgroundMusicPlayer.Init(backgroundMusicReader);
                        //backgroundMusicPlayer.Volume = 1f;
                        //backgroundMusicPlayer.Play();
                        musicPlayer = new SoundPlayer();
                        musicPlayer.SoundLocation = Path.Combine("..", "..", "..", "..", "sounds/main.wav");
                        if (isFinalBoss)
                        {
                            //ZAKO�CZENIE
                            //obs�uga zako�czenia
                            //ciemne t�o
                            timerBossMotion.Stop();
                            panelBackground.Visible = true;
                            pictureBoxMonster.Image = null;
                            pictureBoxMonster.Visible = true;
                            pictureBoxMonster.BackColor = Color.Black;
                            pictureBoxMonster.Location = new Point(0, 0);
                            pictureBoxMonster.Size = new Size(Width, Height);
                            //muzyka final
                            musicPlayer.SoundLocation = Path.Combine("..", "..", "..", "..", "sounds/final victory.wav");
                            //napisy
                            richTextBox1.Visible = true;
                            //richTextBox1.Height = richTextBox1.GetPreferredSize(new Size(richTextBox1.Width, 0)).Height;
                            richTextBox1.BringToFront();
                            richTextBox1.Location = new Point(200, 0);

                            //this.AutoScroll = true;
                            richTextBox1.Size = new Size(Width - 400, Height);
                            timerScroll.Start();
                            richTextBox1.LoadFile(Path.Combine("..", "..", "..", "..", "img/zakonczenie.rtf"));
                            richTextBox1.Scale(new SizeF(2, 2));

                            //labelEndText.Visible = true;
                            labelEndText.BringToFront();
                            labelEndText.Location = new Point(0, 0);
                            labelEndText.Text = "\r\n\r\n\r\n\r\n\r\nBRAWO !!!\r\n\r\nSta�em na szczycie swoich mo�liwo�ci, pe�en determinacji i gotowy stawi� czo�a ostatniej pr�bie. Przed nami, po wielu trudnych pytaniach o bazy danych i niezliczonych �amig��wkach logicznych, stan�� Bazodanowiec � uosobienie wszystkich wyzwa�, kt�re musieli�my pokona�.\r\nKa�de pytanie, ka�de zagadnienie, to by�y nasze bitwy. Ale w ko�cu, gdy ostatni� odpowied� znalaz�em na kartach jego zaszyfrowanych danych, hala naszej walki wype�ni�a si� triumfem. Czu�em, jak duma miesza si� z ulg�. Pokona�em potwora, kt�ry pr�bowa� zgubi� nas swoimi labiryntami i pu�apkami logicznymi.\r\nTo zwyci�stwo to nie tylko koniec wyzwa�, ale pocz�tek nowej ery. Ludzie, kt�rych broni�em, mog� teraz patrze� w przysz�o�� bez obawy przed trudnymi pytaniami i zagadkami. Widzia�em ich u�miechy i s�owa wdzi�czno�ci.\r\nPatrz� teraz na nasz �wiat, kt�ry sta� si� lepszy dzi�ki pokonaniu Bazodanowca. To by� symbolem naszych trudnych pyta� i wyzwa�, kt�re uda�o si� nam pokona�. Wiem teraz, �e ka�de trudne pytanie jest do przej�cia, a przysz�o�� jest pe�na mo�liwo�ci.\r\n\r\n\r\n\r\nWydaje mi si� ze zajebiscie to wymy�li�em\r\nESSA z wami\r\n;3\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n";
                        }
                        musicPlayer.PlayLooping();
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

                    // Wy�wietlenie label z warto�ci� obra�e�
                    enableSpace = false;
                    linePosition = 0;
                    pictureBoxHit.Visible = false;
                    timerHitBox.Stop();
                    timerHitPoints.Start();
                }
                spaceKeyPressTcs.TrySetResult(true);
                return; // to tu MUSI byc
            }

            if (enableWalk)
            {
                if (e.KeyCode == Keys.W && !isFinalBoss)
                {
                    //gdzie jestes?
                    //int x = player.Coordinates.XCoordinate;
                    //int y = player.Coordinates.YCoordinate;
                    //w ktor� strone patrzysz?
                    EDirection direction = player.Direction;
                    //czy �ciana na ktora patrzysz nie jest solidna?

                    int width = game.Labirynth.GetLength(0); // Szeroko�� tablicy
                    int height = game.Labirynth.GetLength(1); // Wysoko�� tablicy

                    bool IsValidMove(int x, int y)
                    {
                        if ((direction == EDirection.North && x == 2 && y == 2) || (direction == EDirection.South && x == 2 && y == 1))
                        {
                            var kluczyk = player.Inventory
                                .Any(i => i.Name.Equals("kluczyk_zielony")); // NO I MAMY ZAPYTANIA LINQ ESSA
                            return kluczyk;
                        }
                        else if ((direction == EDirection.North && x == 5 && y == 3) || (direction == EDirection.South && x == 5 && y == 2))
                        {
                            var kluczyk = player.Inventory
                                .Any(i => i.Name.Equals("kluczyk_zolty"));
                            return kluczyk;
                        }
                        else if ((direction == EDirection.North && x == 1 && y == 3) || (direction == EDirection.South && x == 1 && y == 2))
                        {
                            var kluczyk = player.Inventory
                                .Any(i => i.Name.Equals("kluczyk_czerwony"));
                            return kluczyk;
                        }
                        else if ((direction == EDirection.West && x == 1 && y == 4) || (direction == EDirection.East && x == 2 && y == 4))
                        {
                            var kluczyk = player.Inventory
                                .Any(i => i.Name.Equals("kluczyk_niebieski"));
                            return kluczyk;
                        }
                        return x >= 0 && x < width && y >= 0 && y < height;
                    }


                    void MovePlayer(int newX, int newY)
                    {
                        if (IsValidMove(newX, newY))
                        {
                            player.Coordinates.XCoordinate = newX;
                            player.Coordinates.YCoordinate = newY;

                            if (!game.Map.discoveredMapCoordinates.Contains(new Coordinates(newX, newY)))
                            {
                                game.Map.discoveredMapCoordinates.Add(new Coordinates(newX, newY));
                            }
                            // Odtwarzanie d�wi�ku kroku
                            stepSoundReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/walk_cutted.mp3"));
                            stepSoundPlayer = new WaveOutEvent();
                            stepSoundPlayer.Init(stepSoundReader);
                            stepSoundReader.Position = 0;
                            stepSoundPlayer.Play();

                            // Sprawdzenie, czy gracz jest na ko�cu labiryntu
                            if (player.Coordinates.XCoordinate == 5 && player.Coordinates.YCoordinate == 5)
                            {
                                isFinalBoss = true;
                                labelDamagePlayer.BringToFront();
                                labelDamage.BringToFront();
                                Debug.WriteLine(":KONIEC");
                                Invalidate();

                                otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/try to open door.mp3"));
                                otherMusicPlayer = new WaveOutEvent();
                                otherMusicPlayer.Init(otherMusicReader);
                                otherMusicPlayer.Volume = 1f;
                                otherMusicPlayer.Play();
                                Thread.Sleep(2000);
                                //gdzie sie wybierasz
                                otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/barczak/gdzie sie wybierasz.mp3"));
                                otherMusicPlayer = new WaveOutEvent();
                                otherMusicPlayer.Init(otherMusicReader);
                                otherMusicPlayer.Volume = 1f;
                                otherMusicPlayer.Play();
                                Thread.Sleep(4000);

                                otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/barczak/sbd.mp3"));
                                otherMusicPlayer = new WaveOutEvent();
                                otherMusicPlayer.Init(otherMusicReader);
                                otherMusicPlayer.Volume = 1f;
                                otherMusicPlayer.Play();

                                timerVoice.Start();
                            }
                        }
                        else
                        {
                            //nie mozna isc
                            // Odtwarzanie d�wi�ku kroku
                            stepSoundReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/try to open door.mp3"));
                            stepSoundPlayer = new WaveOutEvent();
                            stepSoundPlayer.Volume = .5f;
                            stepSoundPlayer.Init(stepSoundReader);
                            stepSoundReader.Position = 0;
                            stepSoundPlayer.Play();
                        }
                    }

                    if (game.Labirynth[x, y].Walls[(int)direction].WallType != EWallType.Solid)
                    {
                        int newX = player.Coordinates.XCoordinate;
                        int newY = player.Coordinates.YCoordinate;

                        switch (direction)
                        {
                            case EDirection.North:
                                newY += 1;
                                break;
                            case EDirection.East:
                                newX += 1;
                                break;
                            case EDirection.South:
                                newY -= 1;
                                break;
                            case EDirection.West:
                                newX -= 1;
                                break;
                        }

                        MovePlayer(newX, newY);



                        // Rysowanie pokoju
                        Invalidate();
                    }

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
            if (e.KeyCode == Keys.X && game.Labirynth[x, y].HasMonster) //CHEAT
            {
                game.Labirynth[x, y].Monster.HPCurrent = 1;
            }

            if (e.KeyCode == Keys.M)
            {
                game.Map.isMapShown = !game.Map.isMapShown; //To po prostu je�li nie ma mapy poka�, jak jest to nie pokazuj
                if (game.Map.isMapShown) enableWalk = false;
                else enableWalk = true;
                Invalidate();
            }


        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Pobierz wsp�rz�dne klikni�cia mysz�
            mouseX = e.X;
            mouseY = e.Y;
            Debug.WriteLine(mouseX);
            Debug.WriteLine(mouseY);

            // Sprawdzenie klikni�cia
            Point clickPoint = new Point(mouseX, mouseY); // mouseX i mouseY to wsp�rz�dne klikni�cia
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;
            if (imageRectangle.Contains(clickPoint) && game.Labirynth[x, y].item != null)
            {
                otherMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/key-get.mp3"));
                otherMusicPlayer = new WaveOutEvent();
                otherMusicPlayer.Init(otherMusicReader);
                otherMusicPlayer.Volume = .7f;
                otherMusicPlayer.Play();
                if(game.Labirynth[x, y].item is ItemDamage)
                {
                    Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    player.Strength += 30;
                    labelKomunikat.Text = $"STRENGTH: {player.Strength}";
                }
                else player.Inventory.Add(game.Labirynth[x, y].item); // Dodanie itemu do ekwipunku

                game.Labirynth[x, y].item = null;
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


        //MOJE METODY UPO�LEDZONE DO RYSOWANIA ITEM�W

        private void RysujKluczNaPod�odzePoLewej(Graphics g, EDirection polozenie1, EDirection polozenie2)
        {
            byte[] imageData = game.Labirynth[player.Coordinates.XCoordinate, player.Coordinates.YCoordinate].item.daneZdjecia;
            int width = Width / 16;
            int height = Height / 16;

            using (Image key = Image.FromStream(new MemoryStream(imageData)))
            {
                Bitmap bmp = new Bitmap(width, height);
                using (Graphics gBmp = Graphics.FromImage(bmp))
                {
                    if (player.Direction == polozenie1)
                    {
                        int posX = 300;
                        int posY = 800;
                        // Rysowanie obrazu
                        gBmp.DrawImage(key, new Rectangle(0, 0, width, height));
                        imageRectangle = new Rectangle(posX, posY, width, height);
                    }
                    else if (player.Direction == polozenie2)
                    {
                        int posX = 1100;
                        int posY = 720;
                        gBmp.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                        gBmp.RotateTransform(30);
                        gBmp.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                        // Rysowanie obrazu
                        gBmp.DrawImage(key, new Rectangle(0, 0, width, height));
                        imageRectangle = new Rectangle(posX, posY, width, height);
                    }
                }
                g.DrawImage(bmp, imageRectangle);
            }
        }

        private void RysujKluczNaPod�odzePoLewejSouthEast(Graphics g)
        {
            byte[] imageData = game.Labirynth[player.Coordinates.XCoordinate, player.Coordinates.YCoordinate].item.daneZdjecia;
            int width = Width / 16;
            int height = Height / 16;

            using (Image key = Image.FromStream(new MemoryStream(imageData)))
            {
                Bitmap bmp = new Bitmap(width, height);
                using (Graphics gBmp = Graphics.FromImage(bmp))
                {
                    if (player.Direction == EDirection.South)
                    {
                        int posX = 300;
                        int posY = 800;
                        // Rysowanie obrazu
                        gBmp.DrawImage(key, new Rectangle(0, 0, width, height));
                        imageRectangle = new Rectangle(posX, posY, width, height);
                    }
                    else if (player.Direction == EDirection.East)
                    {
                        int posX = 1100;
                        int posY = 720;
                        gBmp.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                        gBmp.RotateTransform(30);
                        gBmp.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                        // Rysowanie obrazu
                        gBmp.DrawImage(key, new Rectangle(0, 0, width, height));
                        imageRectangle = new Rectangle(posX, posY, width, height);
                    }
                }
                g.DrawImage(bmp, imageRectangle);
            }
        }





        private void RysujApteczkeNaScianie(Graphics g, EDirection polozenie1, EDirection polozenie2)
        {
            byte[] imageData = game.Labirynth[player.Coordinates.XCoordinate, player.Coordinates.YCoordinate].item.daneZdjecia;
            int width = Width / 12;
            int height = Width / 12;

            using (Image key = Image.FromStream(new MemoryStream(imageData)))
            {
                Bitmap bmp = new Bitmap(width, height);
                using (Graphics gBmp = Graphics.FromImage(bmp))
                {
                    if (player.Direction == polozenie1)
                    {
                        int posX = 300;
                        int posY = 200;
                        //gBmp.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                        // gBmp.ScaleTransform(0.1f, 0.1f);
                        //gBmp.RotateTransform(-90);

                        // gBmp.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                        // Rysowanie obrazu
                        Matrix matrix = new Matrix();
                        matrix.Shear(0f, -0.4f);
                        matrix.Scale(0.9f, 0.9f);
                        gBmp.Transform = matrix;
                        gBmp.DrawImage(key, new Rectangle(0, 25, width - 20, height));
                        imageRectangle = new Rectangle(posX, posY, width, height);
                    }
                    else if (player.Direction == polozenie2)
                    {
                        int posX = 1300;
                        int posY = 200;
                        //gBmp.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                        //gBmp.RotateTransform(30);
                        //gBmp.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                        // Rysowanie obrazu
                        gBmp.ScaleTransform(0.9f, 0.9f);
                        gBmp.DrawImage(key, new Rectangle(0, 0, width, height));
                        imageRectangle = new Rectangle(posX, posY, width, height);
                    }
                }
                g.DrawImage(bmp, imageRectangle);
            }
        }



        private async void Form1_Paint(object sender, PaintEventArgs e)
        {
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;
            pictureBoxMonster.Visible = false;

            //POTW�R!!!
            try
            {
                if (game.Labirynth[x, y].HasMonster)
                {
                    int itemHealthCount = player.Inventory.OfType<ItemHealth>().Count();
                    buttonItem.Text = $"HEAL ({itemHealthCount})";
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
                    if (isFinalBoss)
                    {
                        pictureBoxMonster.Size = new Size(800, 600);
                        pictureBoxMonster.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        pictureBoxMonster.Size = new Size(600, 400);
                        pictureBoxMonster.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    //backgroundMusicPlayer.Stop();
                    musicPlayer.Stop();
                    musicPlayer = new SoundPlayer();
                    musicPlayer.SoundLocation = Path.Combine("..", "..", "..", "..", "sounds/bossFight1.wav");
                    if (isFinalBoss)
                        musicPlayer.SoundLocation = Path.Combine("..", "..", "..", "..", "sounds/Final Showdown.wav");
                    musicPlayer.PlayLooping();
                    //backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/bossFight1.mp3"));
                    //if (isFinalBoss)
                    //    backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/Final Showdown.mp3"));
                    //backgroundMusicPlayer = new WaveOutEvent();
                    //backgroundMusicPlayer.Init(backgroundMusicReader);
                    //backgroundMusicPlayer.Volume = 0.7f;
                    //backgroundMusicPlayer.Play();
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


            if (game.Labirynth[x, y].item != null && game.Labirynth[x, y].item is ItemLock || game.Labirynth[x, y].item is ItemDamage)
            {
                RysujKluczNaPod�odzePoLewej(g, game.Labirynth[x, y].item.polozenie1, game.Labirynth[x, y].item.polozenie2);
            }
            else if (game.Labirynth[x, y].item != null && game.Labirynth[x, y].item is ItemHealth)
            {
                RysujApteczkeNaScianie(g, game.Labirynth[x, y].item.polozenie1, game.Labirynth[x, y].item.polozenie2);
            }




            if (game.Map.isMapShown == true) //Odcz�wam b�l jak patrz� na to g�wno....
            {
                using (Image bigMap_img = Image.FromStream(new MemoryStream(game.Map.BigMap)))
                {
                    g.DrawImage(bigMap_img, new Rectangle(0, 0, Width, Height));
                }

                List<Coordinates> cordy = game.Map.discoveredMapCoordinates;

                // Tym mo�na przesuwa� gdzie ta mapa ma si� wy�wietlac na tym zdj�ciu tej poszarpanej mapy
                int baseXposition = 1500;
                int baseYposition = 8500;
                foreach (var w in cordy)
                {
                    byte[] fragment = game.Map.mapFragments[w];
                    int XCoordinate = w.XCoordinate;
                    int YCoordinate = w.YCoordinate;

                    int Xpostion = baseXposition + XCoordinate * 1910;
                    int Ypostion = baseYposition + YCoordinate * (-1000);

                    if (player.Coordinates.Equals(w))
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
            int itemHealthCount = player.Inventory.OfType<ItemHealth>().Count();
            

            if(itemHealthCount > 0 || player.HPCurrent == player.HPMax) 
            {
                if ((player.HPCurrent + 20) <= player.HPMax) player.HPCurrent += 20; //TUTAJ ODBYWA SI� LECZENIE
                else player.HPCurrent = player.HPMax;
                ItemHealth itemToRemove = player.Inventory.OfType<ItemHealth>().FirstOrDefault();
                player.Inventory.Remove(itemToRemove);
                hpBarPlayer.Value = player.HPCurrent;
                itemHealthCount = player.Inventory.OfType<ItemHealth>().Count();
                buttonItem.Text = $"HEAL ({itemHealthCount})";
            }
            else buttonItem.Enabled = false;
        }

        private async void buttonFight_Click(object sender, EventArgs e)
        {
            enableSpace = true;
            // Walka
            pictureBoxHit.Visible = true;

            if (isFinalBoss)
            {
                lineSpeed = random.Next(12, 20);
                // Losowo wybierz kierunek startu linii: 0 - od lewej, 1 - od prawej
                int startDirection = random.Next(0, 2);
                if (startDirection == 0)
                {
                    linePosition = 0;
                    lineSpeed = Math.Abs(lineSpeed); // Ustaw dodatni� pr�dko��
                }
                else
                {
                    linePosition = pictureBoxHit.Width;
                    lineSpeed = -Math.Abs(lineSpeed); // Ustaw ujemn� pr�dko��
                }
            }
            else
            {
                linePosition = 0;
                lineSpeed = Math.Abs(lineSpeed); // Standardowa dodatnia pr�dko��
            }

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
            linePosition += lineSpeed;

            if (isFinalBoss)
            {
                if (linePosition > pictureBoxHit.Width || linePosition < 0)
                {
                    // Resetuj pozycj� i pr�dko�� po wyj�ciu poza pole
                    if (linePosition > pictureBoxHit.Width)
                    {
                        linePosition = pictureBoxHit.Width;
                        lineSpeed = -Math.Abs(lineSpeed); // Ustaw pr�dko�� na ujemn�
                    }
                    else if (linePosition < 0)
                    {
                        linePosition = 0;
                        lineSpeed = Math.Abs(lineSpeed); // Ustaw pr�dko�� na dodatni�
                    }

                    pictureBoxHit.Visible = false; // Ukryj PictureBox
                    damage = 0; // Zeruj obra�enia
                    labelDamage.Text = "MISS";
                    spaceKeyPressTcs.TrySetResult(true);
                }
            }
            else
            {
                // Standardowy ruch linii od lewej do prawej
                if (linePosition > pictureBoxHit.Width)
                {
                    linePosition = 0; // Zresetuj pozycj� linii
                    pictureBoxHit.Visible = false; // Ukryj PictureBox
                    damage = 0; // Zeruj obra�enia
                    labelDamage.Text = "MISS";
                    spaceKeyPressTcs.TrySetResult(true);
                }
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
                        //backgroundMusicPlayer.Stop();
                        musicPlayer.Stop();
                        backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/game_over.mp3"));
                        if (isFinalBoss)
                        {
                            timerVoice.Stop();
                            otherMusicPlayer.Stop();
                            backgroundMusicReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/barczak/portier.mp3"));
                        }
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
            musicPlayer = new SoundPlayer();
            musicPlayer.SoundLocation = Path.Combine("..", "..", "..", "..", "sounds/main.wav");
            musicPlayer.PlayLooping();

            player = new Player("player", null, 100, 100, 30, 20);
            game = new Game(player);
            panelOverlay.Visible = false;
            panelBackground.Visible = false;
            buttonFight.Enabled = true;
            buttonItem.Enabled = true;
            buttonFight.BackColor = Color.White;
            buttonItem.BackColor = Color.White;
            isFinalBoss = false;
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

        private void pictureBoxHit_Click(object sender, EventArgs e)
        {

        }

        private void labelDamagePlayer_Click(object sender, EventArgs e)
        {

        }


        private void timerVoice_Tick(object sender, EventArgs e)
        {
            try
            {
                int index;
                do
                {
                    // Losowanie indeksu pliku d�wi�kowego
                    index = random.Next(soundFiles.Length);
                } while (index == lastPlayedIndex); // Sprawdzenie, czy wybrany indeks nie jest r�wny ostatnio odtwarzanemu

                string soundFilePath = Path.Combine("..", "..", "..", "..", "sounds", "barczak", soundFiles[index]);

                if (File.Exists(soundFilePath))
                {
                    if (otherMusicPlayer != null)
                    {
                        otherMusicPlayer.Stop();
                        otherMusicPlayer.Dispose();
                    }

                    otherMusicReader = new AudioFileReader(soundFilePath);
                    otherMusicPlayer = new WaveOutEvent();
                    otherMusicPlayer.Init(otherMusicReader);
                    otherMusicPlayer.Volume = 1f;
                    otherMusicPlayer.Play();

                    // Zapisanie ostatnio odtwarzanego indeksu
                    lastPlayedIndex = index;
                }
                else
                {
                    MessageBox.Show($"Plik d�wi�kowy nie zosta� znaleziony: {soundFilePath}", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wyst�pi� nieoczekiwany b��d podczas odtwarzania d�wi�ku: {ex.Message}", "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Zmienna do przechowywania ostatnio odtwarzanego indeksu
        private int lastPlayedIndex = -1;
        private string[] soundFiles = { "portier.mp3", "zarabiac.mp3", "cholernie.mp3", "sbd.mp3", "boja sie.mp3", "mercedes.mp3", "myslimy.mp3", "wazna cecha.mp3", "cisza.mp3", "podchwytliwe pytanie.mp3", "po cholere.mp3" };
        private Random random = new Random();

        private int scrollSpeed = 1; // Pr�dko�� przewijania

        private void timerScroll_Tick(object sender, EventArgs e)
        {

            richTextBox1.Top -= scrollSpeed;
        }

        private void panelBackground_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBoxMonster_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxItem_Click(object sender, EventArgs e)
        {

        }
    }
}
