using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcBall
{
    class Field
    {
        int  sizeX, sizeY, lifes, score;
        Graphics g;
        Timer t;
        Ball ball;
        Platform platform;
        List<Block> blocks;

        public event EventHandler NextLevel;
        public event EventHandler GameOver;


        BufferedGraphics buf;

        public Field(Graphics g, int level, int lifes, int score, int sizeX, int sizeY)
        {
            this.g = g;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.lifes = lifes;
            this.score = score;

            BufferedGraphicsContext context = new BufferedGraphicsContext();
            buf = context.Allocate(g, new Rectangle(0, 0, sizeX, sizeY));


            t = new Timer();
            t.Interval = 10;
            t.Tick += GameStep;

            blocks = new List<Block>();

            LoadLevel(level);

            t.Start();
        }



        void GameStep(object sender, EventArgs e)
        {
            platform.Move(sizeX);

            bool isCollision = false;
            buf.Graphics.Clear(Color.White);

            buf.Graphics.DrawRectangle(Pens.Black, 0, 0, sizeX-1, sizeY-1);

            foreach (Block b in blocks)
            {
                isCollision = ball.TestIntersection(b.X, b.Y, b.Size_X, b.Size_Y);
                if (isCollision)
                {
                    b.Damage(ball.Power);
                    score += 100;
                    if (b.Health <= 0)
                    {
                        blocks.Remove(b);
                        score += 100;
                        if (b.Bonus != Bonus.none) UseBonus(b.Bonus);
                        if (blocks.Count == 0) NextLevel(this, new EventArgs());
                    }


                    break;
                }
            }

            if (!isCollision)
            {
                isCollision = ball.TestIntersection(platform.X, platform.Y, platform.Size_X, platform.Size_Y, true);
                if (isCollision) ball.Slide();
            }

            if (!isCollision)
            {
                isCollision = ball.TestIntersection(-1000, -1000, 1000-5, sizeY+1000);
            }
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(sizeX+5, -1000, 1000, sizeY+1000);
            }
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(-1000, -1000, sizeX+1000, 1000-5);
            }


            if (ball.Y > sizeY)
            {
                LoseBall();
            }
            else
            {
                ball.Move();
                ball.Draw();
            }


            foreach (Block b in blocks)
            {
                b.Draw(); 
            }
            platform.Draw();

            try
            {
                buf.Render();
            }
            catch { }
            
        }

        private void UseBonus(Bonus bonus)
        {
            switch (bonus)
            {
                case Bonus.life:
                    lifes++;
                    break;
                case Bonus.fastPlatform:
                    platform.Speed *= 2;
                    break;
                case Bonus.slowPlatform:
                    platform.Speed /= 2;
                    break;
                case Bonus.fastBall:
                    ball.Speed *= 2;
                    break;
                case Bonus.slowBall:
                    ball.Speed /= 2;
                    break;
                case Bonus.bigPlatform:
                    platform.Size_X *= 2;
                    break;
                case Bonus.smallPlatform:
                    platform.Size_X /= 2;
                    break;
                case Bonus.bigBall:
                    ball.Radius *= 2;
                    break;
                case Bonus.smallBall:
                    ball.Radius /= 2;
                    break;
                case Bonus.strongBall:
                    ball.Power *= 2;
                    break;
            }
        }

        private void LoseBall()
        {
            lifes--;

            if (lifes > 0)
            {
                ball = new Ball(buf.Graphics, sizeX/2, sizeY*0.8, 20);
            }
            else
            {
                Timer.Stop();
                GameOver(this, new EventArgs());

                
            }
        }

        void LoadLevel(int level)
        {
            string[] lines = File.ReadAllLines("levels\\" + level + ".txt");

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string[] line = lines[i].Split(' ');
                    blocks.Add(new Block(buf.Graphics, Convert.ToInt32(line[0]), Convert.ToInt32(line[1]), Convert.ToInt32(line[2]), Convert.ToInt32(line[3]), Convert.ToInt32(line[4]), (Bonus)Convert.ToInt32(line[5])));
                }
                catch { }
            }

            ball = new Ball(buf.Graphics, sizeX / 2, sizeY * 0.8, 20);
            platform = new Platform(buf.Graphics, sizeX / 2 - sizeX / 8, sizeY * 0.9, sizeX / 4, 20);

        }


        public Timer Timer {
            get { return t; }
        }


        public int Score
        {
            get { return score; }
        }

        public int Lifes
        {
            get { return lifes; }
        }
    }


}
