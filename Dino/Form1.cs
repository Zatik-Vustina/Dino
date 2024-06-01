using Dino.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dino
{
    public partial class Form1 : Form
    {
        Player player;
        Timer mainTimer;
        bool isPaused = false;

        public Form1()
        {
            InitializeComponent();

            this.Width = 700;
            this.Height = 300;
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(DrawGame);
            this.KeyUp += new KeyEventHandler(OnKeyboardUp);
            this.KeyDown += new KeyEventHandler(OnKeyboardDown);
            mainTimer = new Timer();
            mainTimer.Interval = 16;
            mainTimer.Tick += new EventHandler(Update);

            Init();
        }

        private void OnKeyboardDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (!player.physics.isJumping)
                    {
                        player.physics.isCrouching = true;
                        player.physics.isJumping = false;
                        player.physics.transform.size.Height = 25;
                        player.physics.transform.position.Y = 174;
                    }
                    break;
                case Keys.Escape:
                    isPaused = !isPaused;
                    if (isPaused)
                    {
                        mainTimer.Stop();
                    }
                    else
                    {
                        mainTimer.Start();
                    }
                    Invalidate(); 
                    break;
            }
        }

        private void OnKeyboardUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (!player.physics.isCrouching)
                    {
                        player.physics.isCrouching = false;
                        player.physics.AddForce();
                    }
                    break;
                case Keys.Down:
                    player.physics.isCrouching = false;
                    player.physics.transform.size.Height = 50;
                    player.physics.transform.position.Y = 150.2f;
                    break;
                case Keys.R:
                    if (player.isDead)
                    {
                        Init();
                    }
                    break;
            }
        }

        public void Init()
        {
            GameController.Init();
            player = new Player(new PointF(50, 149), new Size(50, 50));
            isPaused = false;
            mainTimer.Start();
            Invalidate();
        }

        public void Update(object sender, EventArgs e)
        {
            if (!player.isDead && !isPaused)
            {
                player.score++;
                this.Text = "Dino - Score: " + player.score;
                if (player.physics.Collide())
                {
                    player.isDead = true; 
                    mainTimer.Stop(); 
                }
                player.physics.ApplyPhysics();
                GameController.MoveMap();
            }
            Invalidate();
        }

        private void DrawGame(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            player.DrawSprite(g);
            GameController.DrawObjets(g);

            if (isPaused)
            {
                Font font = new Font("Arial", 24);
                g.DrawString("Paused", font, Brushes.White, new PointF(300, 100));
            }
        }
    }
}
