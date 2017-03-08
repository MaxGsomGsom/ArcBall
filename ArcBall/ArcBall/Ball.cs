//#define TEST 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ArcBall
{
    //класс шара
    public class Ball : IBall
    {

        int radius; //диаметр шара
        double x, y, x_prev, y_prev; //координаты на текущем и предпоследнем шаге
        int speed; //скорость
        Graphics g; //объект для отрисовки
        Collision curCollision; //индикатор столкновения с объектом
        double slideX; //скольжение по оси х
        int power; //сила удара шара


        //функция считывает нажатую клавишу
        internal Dictionary<int, short> keyPressed = new Dictionary<int, short>();
#if !TEST
        [DllImport("USER32.dll")]
        static extern short GetAsyncKeyState(int keyCode);
#else
        internal short GetAsyncKeyState(int keyCode)
        {
            if (keyPressed.ContainsKey(keyCode))
                return keyPressed[keyCode];
            else return 0;
        }
#endif


        //конструктор
        public Ball(Graphics g, double x, double y, int radius)
        {
            this.g = g; // 1
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
            g.FillEllipse(Brushes.Black, (int)x, (int)y, radius, radius); // 1
        }

        //функция проверки столкновения с другими объектами
        public bool TestIntersection(IGameObjectSquare obj)
        {
            curCollision = Collision.none; // 1

            //если есть пересечение с объектом
            if ((x + radius) > obj.X && x < (obj.X + obj.Size_X) && (y + radius) > obj.Y && y < (obj.Y + obj.Size_Y)) // 2
            {
                double dxS = Math.Abs(obj.X - (x + radius)); // 3
                double dxL = Math.Abs(obj.X + obj.Size_X - x);
                double dyS = Math.Abs(obj.Y - (y + radius));
                double dyL = Math.Abs(obj.Y + obj.Size_Y - y);

                //определение стороны столкновения
                if (dxS < dxL && dxS < dyS && dxS < dyL && !(obj is IPlatform)) // 4
                {
                    curCollision = Collision.left; // 5
                }
                else if (dxL < dxS && dxL < dyS && dxL < dyL && !(obj is IPlatform)) // 6
                {
                    curCollision = Collision.right; // 7
                }
                else if (dyS < dxS && dyS < dxL && dyS < dyL) // 8
                {
                    curCollision = Collision.top; // 9
                }
                else if (!(obj is IPlatform)) // 10
                {
                    curCollision = Collision.bottom; // 11
                }

            }

            if (curCollision != Collision.none) // 12
                return true; // 13
            else
                return false; //14


        }

        //функция изменения угла движения шара, при столкновеннии с движущейся платформой
        public void Slide()
        {
            //определение нажатия клавиши
            Keys key = Keys.None; // 1
            if (GetAsyncKeyState(0x25) != 0) // 2
                key = Keys.Left; // 3
            else if (GetAsyncKeyState(0x27) != 0) // 4
                key = Keys.Right; // 5

            //задание угла
            if (key == Keys.Left) // 6
                slideX = -0.5; // 7
            else if (key == Keys.Right) // 8
                slideX = 0.5; // 9
        }

        //функция движения шара
        public void Move()
        {
            //определение скорости движения по предыдущим и текущим координатам
            double length = Math.Sqrt(Math.Abs(y - y_prev) * Math.Abs(y - y_prev) + Math.Abs(x - x_prev) * Math.Abs(x - x_prev)); // 1

            //если скорость = 0, то ожидание нажатия клавиши пробел
            if (length == 0 && GetAsyncKeyState(0x20) != 0) // 2
            {
                x_prev = x + (new Random()).Next(-2, 2); // 3
                y_prev = y + 1;
            }
            //если скорость ненулевая
            else if (length != 0)
            {
                //опредление угла движения
                double x_buf = 0, y_buf = 0; // 4

                double y_sin = (y - y_prev) / length;
                double x_cos = (x - x_prev) / length;

                x_cos += slideX;
                slideX = 0;
                if (y_sin < 0.1 && y_sin > -0.1) // 5
                    y_sin = -0.5; // 6

                //изменение направления движения в зависимости от угла движения и грани блока, на которую попал шар
                if (curCollision == Collision.none) // 7
                {
                    x_buf = x_cos * speed + x; // 8
                    y_buf = y_sin * speed + y;
                }
                else if (curCollision == Collision.bottom || curCollision == Collision.top) // 9
                {
                    x_buf = x_cos * speed + x; // 10
                    y_buf = -y_sin * speed + y;
                }
                else if (curCollision == Collision.left || curCollision == Collision.right) // 11
                {
                    x_buf = -x_cos * speed + x; // 12
                    y_buf = y_sin * speed + y;
                }

                //установка новых координат
                x_prev = x; // 13
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

        internal double X_prev
        {
            get { return x_prev; }
            set { x_prev = value; }
        }

        internal double Y_prev
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

        internal Collision Collision
        {
            get { return curCollision; }
            set { curCollision = value; }

        }
        #endregion
    }
}
