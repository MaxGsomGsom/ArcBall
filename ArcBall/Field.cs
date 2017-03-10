using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

[assembly: InternalsVisibleTo("UnitTestProject")]
namespace ArcBall
{
    //класс игрового поля
    public class Field : IField
    {
        int sizeX, sizeY, lifes, score;
        Graphics g;
        Timer t;
        Ball ball;
        Platform platform;
        List<IBlock> blocks;
        Block leftWall, rightWall, topWall;

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
            blocks = new List<IBlock>();

            //загрузка уровня
            LoadLevel(level);

            leftWall = new Block(g, -1000, -1000, 1000 - 5, sizeY + 1000, int.MaxValue);
            rightWall = new Block(g, sizeX + 5, -1000, 1000, sizeY + 1000, int.MaxValue);
            topWall = new Block(g, -1000, -1000, sizeX + 1000, 1000 - 5, int.MaxValue);

            t.Start();
        }


        //определение нажатия клавиши
        [DllImport("USER32.dll")]
        static extern short GetAsyncKeyState(int keyCode);

        private Keys DetectKeys()
        {
            Keys key = Keys.None;
            if (GetAsyncKeyState(0x25) != 0)
                key = Keys.Left;
            else if (GetAsyncKeyState(0x27) != 0)
                key = Keys.Right;
            else if (GetAsyncKeyState(0x20) != 0)
                key = Keys.Space;

            return key;
        }


        //функция, просчитывающая один игровой кадр
        internal void GameStep(object sender, EventArgs e)
        {
            //просчет движения платформы
            platform.Move(DetectKeys());

            bool isCollision = false;
            //заливка поля белым фоном
            buf.Graphics.Clear(Color.White);
            buf.Graphics.DrawRectangle(Pens.Black, 0, 0, sizeX - 1, sizeY - 1);

            //просчет столкновений шара с блоками
            foreach (IBlock b in blocks)
            {
                isCollision = ball.TestIntersection(b);
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
                        if (b is IBonusBlock) (b as IBonusBlock).ActivateBonus(this);
                    }

                    break;
                }
            }

            //проверка на окончание уровня
            if (blocks.Count == 0) NextLevel(this, new EventArgs());

            //проверка столкновения шара с платформой
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(platform);
                if (isCollision) ball.Slide(DetectKeys());
            }

            //проверка столкновения со стенами поля
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(leftWall);
            }
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(rightWall);
            }
            if (!isCollision)
            {
                isCollision = ball.TestIntersection(topWall);
            }

            //если шар улетел вниз, вычитание жизни
            if (ball.Y > sizeY + 100 || ball.Y < -100 || ball.X < -100 || ball.X > sizeX + 100)
            {
                LoseBall();
            }
            else
            {
                //просчет движения шара и его отрисовка
                ball.Move(DetectKeys());
                ball.Draw();
            }

            //отрисовка блоков
            foreach (IBlock b in blocks)
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

        //функция потери шара
        internal void LoseBall()
        {
            lifes--;

            if (lifes > 0)
            {
                ball = new Ball(buf.Graphics, sizeX / 2, sizeY * 0.8, 20);
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
        private void LoadLevel(int level)
        {
            string[] lines;
            try
            {
                //считывание всех строк
                lines = File.ReadAllLines("levels\\" + level + ".txt");
            }
            catch { throw new IOException("Level loading error"); }

            //парсинг каждой строки и создание блока
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    string[] line = lines[i].Split(' ');

                    if (line.Length == 5 || Convert.ToInt32(line[5]) == 0)
                        blocks.Add(new Block(buf.Graphics, Convert.ToInt32(line[0]), Convert.ToInt32(line[1]), Convert.ToInt32(line[2]), Convert.ToInt32(line[3]), Convert.ToInt32(line[4])));
                    else
                        blocks.Add(new BonusBlock(buf.Graphics, Convert.ToInt32(line[0]), Convert.ToInt32(line[1]), Convert.ToInt32(line[2]), Convert.ToInt32(line[3]), Convert.ToInt32(line[4]), (Bonus)Convert.ToInt32(line[5])));
                }
                catch { }
            }

            //создание шара и платформы
            ball = new Ball(buf.Graphics, sizeX / 2, sizeY * 0.8, 20);
            platform = new Platform(buf.Graphics, sizeX / 2 - sizeX / 8, sizeY * 0.9, sizeX / 4, 20, sizeX);

        }

        //поля для доступа к свойствам из вне
        public Timer Timer
        {
            get { return t; }
        }


        public int Score
        {
            get { return score; }
        }

        public int Lifes
        {
            get { return lifes; }
            set { lifes = value; }
        }

        public IBall Ball
        {
            get { return ball; }
        }

        public IPlatform Platform
        {
            get { return platform; }
        }

        public IList<IBlock> Blocks
        {
            get { return blocks; }
        }
    }


}
