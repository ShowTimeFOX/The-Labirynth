using GameLibrary;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Media;
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
        private WaveOutEvent bossFightPlayer;
        private AudioFileReader bossFightReader;
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

            panel1.Visible = false;
            panel2.Visible = false;
            panel1.Location = new Point(0, 0);
            panel1.Size = new Size(Width, Height);
            panel1.Controls.Add(pictureBox1);

            //panel2.Size = new Size(Width / 2, Height/4);
            //panel2.Size = new Size(500, 200);
            //panel2.Location = new Point(0, 0); // Przyklejenie do dolnej kraw�dzi panelu1
            Debug.WriteLine(panel2.Height);
            panel1.Controls.Add(panel2);

            //buttonFight.Location = new Point(0, 0);
            //buttonItem.Location = new Point(200, 0);
            panel2.Controls.Add(buttonFight);
            panel2.Controls.Add(buttonItem);


            //HP BAR przeciwnik
            hpBarEnemy.Location = new Point(panel1.Width - 300, 100);
            hpBarEnemy.Size = new Size(200, 30);
            hpBarEnemy.Minimum = 0;
            hpBarEnemy.Maximum = 100; // Maksymalna warto�� HP
            hpBarEnemy.Value = 100; // Aktualna warto�� HP
            hpBarEnemy.ForeColor = Color.Red;
            panel1.Controls.Add(hpBarEnemy);

            labelHpPlayer.Text = $"HP {player.HPCurrent} / {player.HPCurrent}";

            //pictureBox1.BackColor = Color.Transparent; - g�wno psuje animacje
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Visible = false;
            centerX = Width / 2 - 165;
            centerY = Height / 2 - 340;
            timerBossMotion.Start();

        }

        private void button1_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

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
                //tutaj bedzie bicie potwora
                if (e.KeyCode == Keys.Space)
                {
                    MessageBox.Show("Naci�ni�to spacj� w odpowiednim momencie!", "Gratulacje!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            pictureBox1.Visible = false;


            UpdateCompassPointerImage();
            Graphics g = e.Graphics;
            g.Clear(this.BackColor);

            //gdzie jestes?
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;
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
            //System.Diagnostics.Debug.WriteLine($"------------------------------------");
            //System.Diagnostics.Debug.WriteLine($"Frontowa �ciana:");
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

            //using (Image front_img = Image.FromStream(new MemoryStream(front)))
            //using (Image left_img = Image.FromStream(new MemoryStream(left)))
            //using (Image right_img = Image.FromStream(new MemoryStream(right)))
            //using (Image floor_img = Image.FromStream(new MemoryStream(floor)))
            //{
            //    g.DrawImage(left_img, new Rectangle(0, 0, Width-960, Height));//G
            //    g.DrawImage(right_img, new Rectangle(960, 0, Width-960, Height));//G
            //    g.DrawImage(front_img, new Rectangle(0, 0, Width, Height));//G
            //    g.DrawImage(floor_img, new Rectangle(0, Height-360+12, Width, 360));//G
            //}



            using (Bitmap im = new Bitmap(Path.Combine("..", "..", "..", "..", "img/compass.png")))
            {
                g.DrawImage(im, new Rectangle(Width - 200, Height - 200, 165, 165));
                g.DrawImage(compassPointerImage, new Point(Width - 200, Height - 200));

            }

            //POTW�R!!!
            try
            {
                if (game.Labirynth[x, y].HasMonster)
                {
                    //zrob mu ciemno
                    //SolidBrush blackBrush = new SolidBrush(Color.Black);
                    //g.FillRectangle(blackBrush, new Rectangle(0, 0, Width, Height));
                    panel1.Visible = true;
                    panel2.Visible = true;

                    //blokuj chodzenie
                    enableWalk = false;

                    //tuaj nie moze byc za bardzo usinga: https://stackoverflow.com/questions/12680618/exception-parameter-is-not-valid-on-passing-new-image-to-picturebox
                    //using (Image monster = Image.FromFile(game.Labirynth[x, y].Monster.ImagePath))
                    Bitmap monster = new Bitmap(game.Labirynth[x, y].Monster.ImagePath);
                    //rysuj w picturebox
                    pictureBox1.Visible = true;
                    pictureBox1.Image = monster;
                    bossFightReader = new AudioFileReader(Path.Combine("..", "..", "..", "..", "sounds/bossFight1.mp3"));
                    bossFightPlayer = new WaveOutEvent();
                    bossFightPlayer.Init(bossFightReader);
                    bossFightPlayer.Volume = 0.7f; // Przyk�adowo: 20% g�o�no�ci
                    backgroundMusicPlayer.Stop();
                    bossFightPlayer.Play();
                }
            }
            catch (OutOfMemoryException ex)
            {

                Debug.WriteLine(ex.Data);
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Oblicz now� pozycj� x i y
            xPosition = (int)(centerX + amplitudeX * Math.Sin(angle));
            yPosition = (int)(centerY + amplitudeY * Math.Sin(2 * angle));

            // Zwi�ksz k�t
            angle += angleStep;

            // Ustaw now� pozycj� pictureBox1
            pictureBox1.Location = new Point(xPosition, yPosition);
        }

        private void buttonItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonFight_Click(object sender, EventArgs e)
        {
            pictureBoxHit.Visible = true;

            hpBarEnemy.Value -= player.Strength;
            Debug.WriteLine(player.Strength);
        }

        private void pictureBoxHit_Paint(object sender, PaintEventArgs e)
        {
            // Rysowanie linii
            //e.Graphics.FillRectangle(Brushes.Black, linePosition, pictureBox.Height / 2 - lineHeight / 2, lineWidth, lineHeight);
        }
    }
}
