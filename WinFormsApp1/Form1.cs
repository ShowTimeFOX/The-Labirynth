using GameLibrary;
using System.Drawing.Drawing2D;
using TheLabirynth;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Game game;
        private Player player;
        public Form1()
        {
            player = new Player();
            game = new Game(player);

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void button1_MouseCaptureChanged(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.A)
                MessageBox.Show("idziesz do AAAA");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.W)
            {
                //gdzie jestes?
                int x = player.Coordinates.XCoordinate;
                int y = player.Coordinates.YCoordinate;
                //w ktor¹ strone patrzysz?
                EDirection direction = player.Direction;
                //czy œciana na ktora patrzysz nie jest solidna?
                if (game.Labirynth[x, y].Walls[(int)direction].WallType != EWallType.Solid) {
                    //tak -> mozna isc
                    //labirynt[x, y] idz tam gdzie patrzysz; +1 w danym kierunku
                    switch(direction) { 
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
                    //rysuj pokój
                    Invalidate();
                }
                
                //nie mozna isc i tyle
                //dzwiêk moze mozna dac jakis czy cos ewentualnie

                //MessageBox.Show("idziesz do przodu");
            }
            if (e.KeyCode == Keys.D)
                MessageBox.Show("obrót w prawo");
            if (e.KeyCode == Keys.A)
                MessageBox.Show("obrót w lewo");
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
            
            Graphics g = e.Graphics;
            //gdzie jestes?
            int x = player.Coordinates.XCoordinate;
            int y = player.Coordinates.YCoordinate;
            label1.Text = $"X: {x}; Y: {y}";
            //w ktor¹ strone patrzysz?
            EDirection direction = player.Direction;
            //patrz na to gdzie jestes i weŸ potem sciane z lewej i sciane z prawej
            String front = game.Labirynth[x, y].Walls[(int)direction].ImagePath;
            String left = game.Labirynth[x, y].Walls[(int)direction.Previous()].ImagePath;
            String right = game.Labirynth[x, y].Walls[(int)direction.Next()].ImagePath;
            String floor = game.Labirynth[x, y].Walls[(int)direction.Next()].ImageDirectory + "floor.png";
            Image f1 = Bitmap.FromFile(front);
            Image f2 = Bitmap.FromFile(left);
            Image f3 = Bitmap.FromFile(right);
            Image f4 = Bitmap.FromFile(floor);
            
            //g.drawImage...
            g.DrawImage(f1, new Rectangle(0,0,Width,Height));
            g.DrawImage(f2, new Rectangle(0,0,Width,Height));
            g.DrawImage(f3, new Rectangle(0,0,Width,Height));
            g.DrawImage(f4, new Rectangle(0,0,Width,Height));


            //----------------------------------------------------
            // Wczytaj obrazy
            //Image floor = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\floor.png");
            //Image wall_east = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\wall_east_solid.png");
            //Image wall_west = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\wall_west_solid.png");
            //Image wall_north = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\wall_north_empty.png");

            // Ustaw rozmiar formularza na podstawie wielkoœci obrazu
            //this.ClientSize = new Size(floor.Width, floor.Height);
            
            // Rysowanie obrazów na formularzu
            //g.DrawImage(floor, new Rectangle(0, 0, Width, Height));
            //g.DrawImage(wall_west, new Rectangle(0, 0, Width, Height));
            //g.DrawImage(wall_east, new Rectangle(0, 0, Width, Height));


        }
    }
}
