using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ArcBall
{
    class Field
    {
        int level, lives, score, sizeX, sizeY;
        Graphics g;
        Timer t;
        Ball ball;
        Platform platform;
        List<Block> blocks;

        BufferedGraphics buf;

        public Field(Graphics g)
        {
            this.g = g;
            level = 1;
            lives=3;
            score = 0;
            sizeX = 600;
            sizeY = 600;

            BufferedGraphicsContext context = new BufferedGraphicsContext();
            buf = context.Allocate(g, new Rectangle(0, 0, sizeX, sizeY));


            t = new Timer(10);
            t.Elapsed += GameStep;

            blocks = new List<Block>();
            NextLevel();

            t.Start();
        }


        void GameStep(object sender, ElapsedEventArgs e)
        {
            platform.Move(sizeX);

            bool isCollision = false;
            buf.Graphics.Clear(Color.White);

            foreach (Block b in blocks)
            {
                isCollision = ball.TestIntersection(b.X, b.Y, b.Size_X, b.Size_Y);
                if (isCollision)
                {
                    b.Damage(ball.Power);
                    if (b.Health == 0)
                    {
                        blocks.Remove(b);
                        if (b.Bonus != Bonus.none) UseBonus(b.Bonus);
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
                isCollision = ball.TestIntersection(-1000, -1000, 1000, sizeY+1000);
            }
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(sizeX, -1000, 1000, sizeY+1000);
            }
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(-1000, -1000, sizeX+1000, 1000);
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


            buf.Render();
            
        }

        private void UseBonus(Bonus bonus)
        {
            switch (bonus)
            {
                case Bonus.life:
                    lives++;
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
            lives--;

            if (lives > 0)
            {
                ball = new Ball(buf.Graphics, 300, 300, 20);
            }
            else
            {
                timer.Stop();
                
            }
        }

        void NextLevel()
        {
            blocks.Add(new Block(buf.Graphics, 10, 10, 500, 10, 1, Bonus.none));
            ball = new Ball(buf.Graphics, 300, 300, 20);
            platform = new Platform(buf.Graphics, 100, 500, 200, 20);
        }


        Timer timer {
            get { return t; }
        }
    }


}
