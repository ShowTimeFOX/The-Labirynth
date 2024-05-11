using GameLibrary;
using System.Drawing.Drawing2D;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private Game game;
        private Player player;
        public Form1()
        {
            player = new Player();
            Game game = new Game(player);

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
                MessageBox.Show("idziesz do przodu");
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

            // Wczytaj obrazy
            Image floor = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\floor.png");
            Image wall_east = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\wall_east_solid.png");
            Image wall_west = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\wall_west_solid.png");
            Image wall_north = Bitmap.FromFile(@"C:\Users\barte\Desktop\The Labirynth\img\wall_north_empty.png");

            // Ustaw rozmiar formularza na podstawie wielkoœci obrazu
            //this.ClientSize = new Size(floor.Width, floor.Height);
            Graphics g = e.Graphics;
            // Rysowanie obrazów na formularzu
            g.DrawImage(floor, new Rectangle(0, 0, Width, Height));
            g.DrawImage(wall_west, new Rectangle(0, 0, Width, Height));
            g.DrawImage(wall_east, new Rectangle(0, 0, Width, Height));


        }
    }
}
