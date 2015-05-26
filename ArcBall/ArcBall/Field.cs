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
    //класс игрового поля
    class Field
    {
        int  sizeX, sizeY, lifes, score;
        Graphics g;
        Timer t;
        Ball ball;
        Platform platform;
        List<Block> blocks;

        //события
        public event EventHandler NextLevel;
        public event EventHandler GameOver;

        
        BufferedGraphics buf;

        //конструктор
        public Field(Graphics g, int level, int lifes, int score, int sizeX, int sizeY)
        {
            this.g = g;
            //размер поля
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            //жизни
            this.lifes = lifes;
            //счет
            this.score = score;

            //графический контекст для буферизации вывода на экран
            BufferedGraphicsContext context = new BufferedGraphicsContext();
            buf = context.Allocate(g, new Rectangle(0, 0, sizeX, sizeY));

            //основной таймер
            t = new Timer();
            t.Interval = 10;
            t.Tick += GameStep;

            //список блоков
            blocks = new List<Block>();

            //загрузка уровня
            LoadLevel(level);

            t.Start();
        }


        //функция, просчитывающая один игровой кадр
        void GameStep(object sender, EventArgs e)
        {
            //просчет движения платформы
            platform.Move(sizeX);

            bool isCollision = false;
            //заливка поля белым фоном
            buf.Graphics.Clear(Color.White);
            buf.Graphics.DrawRectangle(Pens.Black, 0, 0, sizeX-1, sizeY-1);

            //просчет столкновений шара с блоками
            foreach (Block b in blocks)
            {
                isCollision = ball.TestIntersection(b.X, b.Y, b.Size_X, b.Size_Y);
                //если есть столкновение
                if (isCollision)
                {
                    //повреждение блока
                    b.Damage(ball.Power);
                    score += 100;
                    //если блок уничтожен
                    if (b.Health <= 0)
                    {
                        //удаление блока
                        blocks.Remove(b);
                        score += 100;
                        //применение бонуса
                        if (b.Bonus != Bonus.none) UseBonus(b.Bonus);
                        //проверка на окончание уровня
                        if (blocks.Count == 0) NextLevel(this, new EventArgs());
                    }

                    break;
                }
            }

            //проверка столкновения шара с платформой
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(platform.X, platform.Y, platform.Size_X, platform.Size_Y, true);
                if (isCollision) ball.Slide();
            }

            //проверка столкновения со стенами поля
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

            //если шар улетел вниз, вычитание жизни
            if (ball.Y > sizeY)
            {
                LoseBall();
            }
            else
            {
                //просчет движения шара и его отрисовка
                ball.Move();
                ball.Draw();
            }

            //отрисовка блоков
            foreach (Block b in blocks)
            {
                b.Draw(); 
            }
            //отрисовка платформы
            platform.Draw();

            //рендеринг кадра
            try
            {
                buf.Render();
            }
            catch { }
            
        }

        //функция применения бонуса в зависимости от свойства блока
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

        //функция потери шара
        private void LoseBall()
        {
            lifes--;

            if (lifes > 0)
            {
                ball = new Ball(buf.Graphics, sizeX/2, sizeY*0.8, 20);
            }
            //если жизней больше нет - конец игры
            else
            {
                Timer.Stop();
                GameOver(this, new EventArgs());

                
            }
        }

        //функция загрузки уровня из файла
        //уровень состоит из списка блоков
        //каждый блок описывается строкой в файле: координата Х - координата У - ширина - высота - прочность - тип бонуса
        void LoadLevel(int level)
        {
            //считывание всех строк
            string[] lines = File.ReadAllLines("levels\\" + level + ".txt");

            //парсинг каждой строки и создание блока
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string[] line = lines[i].Split(' ');
                    blocks.Add(new Block(buf.Graphics, Convert.ToInt32(line[0]), Convert.ToInt32(line[1]), Convert.ToInt32(line[2]), Convert.ToInt32(line[3]), Convert.ToInt32(line[4]), (Bonus)Convert.ToInt32(line[5])));
                }
                catch { }
            }

            //создание шара и платформы
            ball = new Ball(buf.Graphics, sizeX / 2, sizeY * 0.8, 20);
            platform = new Platform(buf.Graphics, sizeX / 2 - sizeX / 8, sizeY * 0.9, sizeX / 4, 20);

        }

        //поля для доступа к свойствам из вне
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
