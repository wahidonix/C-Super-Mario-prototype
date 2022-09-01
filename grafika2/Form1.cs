using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace grafika2
{
    public partial class Form1 : Form
    {
        //player globals
        int playerX=10;
        int playerY=450;
        int playerSizeX=60;
        int playerSizeY=60;
        int score = 0;

        //enemy
        int enemyX=620;
        int enemyY;
        int enemySpeed = 4;

        //game physics
        int gravity=7;
        int force = 8;
        
        
        //boooleans
        bool goleft = false;
        bool goright = false;
        bool playerleft = false;
        bool grounded = false;
        bool jumping = false;
        bool gameover = false;
        bool dead = false;


        //sprites
        Bitmap player1 = new
            Bitmap(Properties.Resources.mario);
        Bitmap enemy = new
            Bitmap(Properties.Resources.goomba);
        Bitmap backgroundSprite = new
            Bitmap(Properties.Resources.background);


        //flag coordinates
        Point[] points = { new Point(1130, 165), new Point(1180, 150), new Point(1180, 180) };


        //coins
        Point coin1 = new Point(200, 500);
        Point coin2 = new Point(370, 500);
        Point coin3 = new Point(450, 390);
        Point coin4 = new Point(560, 500);
        public Form1()
        {
            InitializeComponent();
        }

        public void gameArea_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.DrawImage(backgroundSprite, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            Pen pn = new Pen(Color.Green, 8);
            g.DrawLine(pn, 1180, 180, 1180, pictureBox1.Height - 80);
            g.DrawImage(player1, playerX, playerY, playerSizeX, playerSizeY);
            g.DrawPolygon(new Pen(Color.GreenYellow), points);
            Brush brush = new SolidBrush(Color.GreenYellow);
            g.FillPolygon(brush, points);
        }
        void Finish()
        {
            if(playerX > 1180-playerSizeX && playerY >= pictureBox1.Height - 80 - playerSizeY)
            {
                gameover = true;
            }
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
            if (jumping)
            {
                jumping = false;
            }
        }
        void checkGrounded()
        {
            if (playerY > pictureBox1.Height - 80 - playerSizeY)
            {
                grounded = true;
                pictureBox1.Invalidate();
                force = 8;
            }
            else
            {
                grounded = false;
            }
        }

        void createCoins()
        {
            Graphics g = pictureBox1.CreateGraphics();
            Pen pn = new Pen(Color.Black);
            g.DrawEllipse(pn, new Rectangle(coin1.X, coin1.Y, 20, 20));
            g.FillEllipse(new SolidBrush(Color.Gold), new Rectangle(coin1.X, coin1.Y, 20, 20));

            g.DrawEllipse(pn, new Rectangle(coin2.X, coin2.Y, 20, 20));
            g.FillEllipse(new SolidBrush(Color.Gold), new Rectangle(coin2.X, coin2.Y, 20, 20));

            g.DrawEllipse(pn, new Rectangle(coin3.X, coin3.Y, 20, 20));
            g.FillEllipse(new SolidBrush(Color.Gold), new Rectangle(coin3.X, coin3.Y, 20, 20));

            g.DrawEllipse(pn, new Rectangle(coin4.X, coin4.Y, 20, 20));
            g.FillEllipse(new SolidBrush(Color.Gold), new Rectangle(coin4.X, coin4.Y, 20, 20));
        }
        void createEnemy()
        {
            Graphics g = pictureBox1.CreateGraphics();
            enemyY = pictureBox1.Height - 110;
            g.DrawImage(enemy, enemyX, enemyY, 30, 30);
            pictureBox1.Invalidate();
        }
        void checkCoinCollision()
        {
            if (playerX > coin1.X && playerX < coin1.X + 30 && playerY > coin1.Y - 30)
            {
                coin1.X += 1500;
                score += 100;
            }
            if (playerX > coin2.X && playerX < coin2.X + 30 && playerY > coin2.Y - 30)
            {
                coin2.X += 1500;
                score += 100;
            }
            if (playerX > coin3.X && playerX < coin3.X + 30 && playerY > coin3.Y - 20 && playerY < coin3.Y)
            {
                coin3.X += 1500;
                score += 100;
            }
            if (playerX > coin4.X && playerX < coin4.X + 20 && playerY > coin4.Y - 20)
            {
                coin4.X += 1500;
                score += 100;
            }
        }
        void checkEnemyCollision()
        {
            if (playerX > enemyX && playerX < enemyX+20 && playerY > enemyY-45)
            {
                dead = true;
            }
            if (playerX > enemyX && playerX < enemyX + 20 && playerY < enemyY - 45)
            {
                enemyX = 1800;
                score += 300;
            }
        }

        private void player_Tick(object sender, EventArgs e)
        {
            label1.Text = "grounded:" + grounded.ToString() + " jumping:" + jumping.ToString() + " X:"+playerX.ToString() + " Y:"+playerY.ToString();
            label2.Text = "Score: " + score;
            checkCoinCollision();
            checkEnemyCollision();
            createCoins();
            createEnemy();
            enemyX += enemySpeed;
            Graphics g = pictureBox1.CreateGraphics();
            if (gameover)
            {
                player.Stop();
                finishTimer.Start();

            }
            if (dead)
            {
                player.Stop();
                deadTimer.Start();
            }
            if (enemyX < 600)
            {
                enemySpeed = 4;
            }
            if(enemyX > 1100)
            {
                enemySpeed = -4;
            }
            playerY += gravity;
            g.DrawImage(player1, playerX, playerY, playerSizeX, playerSizeY);
            pictureBox1.Invalidate();
            checkGrounded();
            Finish();
            if (jumping && force < 0)
            {
                jumping = false;
                gravity = 7;
            }
            if (jumping)
            {
                gravity = -14;
                force -= 1;
            }
            else
            {
                gravity = 6;
            }
            if (grounded && !jumping)
            {
                gravity = 0;
            }
            if (goleft == true && playerX>3)
            {
                playerX -= 5;
                if(playerleft == false)
                {
                    playerleft = true;
                    player1.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                g.DrawImage(player1, playerX, playerY, playerSizeX, playerSizeY);

                pictureBox1.Invalidate();
            }
            if (goright == true && playerX<pictureBox1.Width-playerSizeX)
            {
                playerX += 5;
                if(playerleft == true)
                {
                    playerleft = false;
                    player1.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                g.DrawImage(player1, playerX, playerY, playerSizeX, playerSizeY);
                pictureBox1.Invalidate();
            }


        }

        private void finishTimer_Tick(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            Font fnt = new Font("Verdana", 32);
            g.DrawString("YOU WIN", fnt, new SolidBrush(Color.Black),
            pictureBox1.Width / 2 - 100, pictureBox1.Height / 2 - 50);
            playerX -= 5;
            if (playerleft == false)
            {
                playerleft = true;
                player1.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            g.DrawImage(player1, playerX, playerY, playerSizeX, playerSizeY);
            pictureBox1.Invalidate();
            g.DrawString("YOU WIN", fnt, new SolidBrush(Color.Black),
            pictureBox1.Width/2-100, pictureBox1.Height/2-50);
        }

        private void deadTimer_Tick(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            Font fnt = new Font("Verdana", 32);
            g.DrawString("YOU LOST", fnt, new SolidBrush(Color.Black),
            pictureBox1.Width / 2 - 100, pictureBox1.Height / 2 - 50);
            player1.RotateFlip(RotateFlipType.Rotate90FlipY);
            pictureBox1.Invalidate();
            g.DrawString("YOU LOST", fnt, new SolidBrush(Color.Black),
            pictureBox1.Width / 2 - 100, pictureBox1.Height / 2 - 50);
        }
    }
}
