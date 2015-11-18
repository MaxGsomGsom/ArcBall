#define TEST 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace ArcBall
{
    //класс шара
    public class Ball
    {

        int radius; //диаметр шара
        double x, y, x_prev, y_prev; //координаты на текущем и предпоследнем шаге
        int speed; //скорость
        Graphics g; //объект для отрисовки
        Collision curCollision; //индикатор столкновения с объектом
        double slideX; //скольжение по оси х
        int power; //сила удара шара


        //функция считывает нажатую клавишу
#if !TEST
        [DllImport("USER32.dll")]
        static extern short GetAsyncKeyState(int keyCode);
#else
        public Dictionary<int, short> keyPressed = new Dictionary<int, short>();
        public short GetAsyncKeyState(int keyCode)
        {
            if (keyPressed.ContainsKey(keyCode))
                return keyPressed[keyCode];
            else return 0;
        }
#endif


        //конструктор
        public Ball(Graphics g, double x, double y, int radius)
        {
            this.g = g;
            //координаты
            this.x = x;
            this.y = y;
            //радиус
            this.radius = radius;
            //скорость
            speed = 4;
            slideX = 0;
            //мощность
            power = 1;

            //предыдущие координаты для просчета траетории
            x_prev = x;
            y_prev = y;

        }

        //функция отрисовки шара
        public void Draw()
        {
            g.FillEllipse(Brushes.Black, (int)x, (int)y, radius, radius);
        }

        //функция проверки столкновения с другими объектами
        public bool TestIntersection(double blockX, double blockY, double blockSizeX, double blockSizeY, bool isPlatform=false)
        {
            curCollision = Collision.none;

            //если есть пересечение с объектом
            if ((x+radius) > blockX && x < (blockX + blockSizeX) && (y+radius) > blockY && y < (blockY + blockSizeY))
            {
                double dxS = Math.Abs(blockX - (x+radius));
                double dxL = Math.Abs(blockX + blockSizeX - x);
                double dyS = Math.Abs(blockY - (y+radius));
                double dyL = Math.Abs(blockY + blockSizeY - y);

                //определение стороны столкновения
                if (dxS < dxL && dxS < dyS && dxS < dyL && !isPlatform)
                {
                    curCollision = Collision.left;
                }
                else if (dxL < dxS && dxL < dyS && dxL < dyL && !isPlatform)
                {
                    curCollision = Collision.right;
                }
                else if (dyS < dxS && dyS < dxL && dyS < dyL)
                {
                    curCollision = Collision.top;
                }
                else if (!isPlatform)
                {
                    curCollision = Collision.bottom;
                }

            }

            if (curCollision != Collision.none) return true;
            else return false;

            
        }

        //функция изменения угла движения шара, при столкновеннии с движущейся платформой
        public void Slide()
        {
            //определение нажатия клавиши
            Keys key = Keys.None;
            if (GetAsyncKeyState(0x25) != 0) key = Keys.Left;
            else if (GetAsyncKeyState(0x27) != 0) key = Keys.Right;

            //задание угла
            if (key == Keys.Left) slideX = -0.5;
            else if (key == Keys.Right) slideX = 0.5;
        }

        //функция движения шара
        public void Move()
        {
           //определение скорости движения по предыдущим и текущим координатам
            double length = Math.Sqrt(Math.Abs(y - y_prev) * Math.Abs(y - y_prev) + Math.Abs(x - x_prev) * Math.Abs(x - x_prev));

            //если скорость = 0, то ожидание нажатия клавиши пробел
            if (length == 0)
            {
                if (GetAsyncKeyState(0x20) != 0)
                {
                    x_prev = x + (new Random()).Next(-2, 2);
                    y_prev = y + 1;
                }
            }
            //если скорость ненулевая
            else
            {
                //опредление угла движения
                double x_buf = 0, y_buf = 0;

                double y_sin = (y - y_prev) / length;
                double x_cos = (x - x_prev) / length;

                x_cos += slideX;
                slideX = 0;
                if (y_sin < 0.1 && y_sin > -0.1) y_sin = -0.5;

                //изменение направления движения в зависимости от угла движения и грани блока, на которую попал шар
                if (curCollision == Collision.none)
                {
                    x_buf = x_cos * speed + x;
                    y_buf = y_sin * speed + y;
                }
                else if (curCollision == Collision.bottom || curCollision == Collision.top)
                {
                    x_buf = x_cos * speed + x;
                    y_buf = -y_sin * speed + y;
                }
                else if (curCollision == Collision.left || curCollision == Collision.right)
                {
                    x_buf = -x_cos * speed + x;
                    y_buf = y_sin * speed + y;
                }

                //установка новых координат
                x_prev = x;
                y_prev = y;
                x = x_buf;
                y = y_buf;
            }
        }

        //поля для доступа к свойствам извне
        #region Fields
        public double X
        {
            get { return x; }
        }

        public double Y
        {
            get { return y; }
        }

        public double X_prev
        {
            get { return x_prev; }
            set { x_prev = value; }
        }

        public double Y_prev
        {
            get { return y_prev; }
            set { y_prev = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public int Power
        {
            get { return power; }
            set { power = value; }
        }

        public Collision Collision
        {
            get { return curCollision; }
            set { curCollision = value; }

        } 
        #endregion
    }
}
