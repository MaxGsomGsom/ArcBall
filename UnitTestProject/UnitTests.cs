using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using ArcBall;

namespace UnitTestProject
{

    [TestClass]
    public class UnitTestBall
    {
        Bitmap img = null;
        Ball b = null;
        Graphics g = null;

        //инициализация объекта для тестирования
        [TestInitialize]
        public void Init()
        {
            img = new Bitmap(100, 100);
            g = Graphics.FromImage(img);

            g.Clear(Color.White);



            b = new Ball(g, 50, 50, 10);
            
        }

        //проверка конструктора
        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsTrue(b.X == 50);
            Assert.IsTrue(b.Y == 50);
            Assert.IsTrue(b.Radius == 10);

            Assert.IsTrue(b.Power > 0);
            Assert.IsTrue(b.Speed > 0);
        }

        //проверка отрисовки
        [TestMethod]
        public void TestDraw()
        {
            b.Draw();

            Assert.IsTrue(img.GetPixel((int)b.X+5, (int)b.Y+5).ToArgb() != Color.White.ToArgb());
        }

        //проверка положения шара при отсутсвии движения
        [TestMethod]
        public void TestMoveZero()
        {
            b.X_prev = b.X;
            b.Y_prev = b.Y;
            b.keyPressed.Clear();
            b.Move();

            Assert.IsTrue(b.X == b.X_prev);
            Assert.IsTrue(b.Y == b.Y_prev);
        }

        //проверка положения шара при отсутсвии движения и нажатии кнопки запуска
        [TestMethod]
        public void TestMoveZeroStart()
        {
            b.X_prev = b.X;
            b.Y_prev = b.Y;
            b.keyPressed.Add(0x20, 1);

            b.Move();
           
            Assert.IsTrue(b.Y < b.Y_prev);
        }

        //проверка положения шара при движении вправо вниз
        [TestMethod]
        public void TestMoveRightDown()
        {
            b.X_prev = b.X-1;
            b.Y_prev = b.Y-1;

            b.Collision = Collision.none;

            b.Move();


            Assert.IsTrue(b.X > b.X_prev);
            Assert.IsTrue(b.Y > b.Y_prev);
        }

        //проверка положения шара при движении влево
        [TestMethod]
        public void TestMoveLeft()
        {
            b.X_prev = b.X + 1;
            b.Collision = Collision.none;

            b.Move();

            Assert.IsTrue(b.X < b.X_prev);
        }

        //проверка положения шара при движении вправо
        [TestMethod]
        public void TestMoveRight()
        {
            b.X_prev = b.X - 1;
            b.Collision = Collision.none;

            b.Move();

            Assert.IsTrue(b.X > b.X_prev);
        }

        //проверка положения шара при движении вверх
        [TestMethod]
        public void TestMoveTop()
        {
            b.Y_prev = b.Y+1;
            b.Collision = Collision.none;

            b.Move();

            Assert.IsTrue(b.Y < b.Y_prev);
        }

        //проверка положения шара при движении вниз
        [TestMethod]
        public void TestMoveDown()
        {
            b.Y_prev = b.Y - 1;
            b.Collision = Collision.none;

            b.Move();

            Assert.IsTrue(b.Y > b.Y_prev);
        }

        //проверка положения шара при отражении от объекта слева
        [TestMethod]
        public void TestCollisionLeft()
        {
            b.X_prev = b.X + 1;
            b.Collision = Collision.left;

            b.Move();

            Assert.IsTrue(b.X > b.X_prev);
        }

        //проверка положения шара при отражении от объекта справа
        [TestMethod]
        public void TestCollisionRight()
        {
            b.X_prev = b.X - 1;
            b.Collision = Collision.left;

            b.Move();

            Assert.IsTrue(b.X < b.X_prev);
        }

        //проверка положения шара при отражении от объекта сверху
        [TestMethod]
        public void TestCollisionTop()
        {
            b.Y_prev = b.Y + 1;
            b.Collision = Collision.top;

            b.Move();

            Assert.IsTrue(b.Y > b.Y_prev);
        }

        //проверка положения шара при отражении от объекта снизу
        [TestMethod]
        public void TestCollisionDown()
        {
            b.Y_prev = b.Y - 1;
            b.Collision = Collision.top;

            b.Move();

            Assert.IsTrue(b.Y < b.Y_prev);
        }

        //проверка положения шара при отражении от объекта снизу справа
        [TestMethod]
        public void TestCollisionRightDown()
        {
            b.X_prev = b.X - 1;
            b.Y_prev = b.Y - 1;
            b.Collision = Collision.bottom;

            b.Move();

            Assert.IsTrue(b.X > b.X_prev);
            Assert.IsTrue(b.Y < b.Y_prev);
        }

        //проверка положения шара при скольжении вправо
        [TestMethod]
        public void TestSlideRight()
        {
            b.keyPressed.Clear();
            b.keyPressed.Add(0x27, 1);
            b.Y_prev = b.Y - 1;
            b.X_prev = b.X;
            b.Collision = Collision.bottom;
            b.Slide();

            b.Move();

            Assert.IsTrue(b.Y < b.Y_prev);
            Assert.IsTrue(b.X > b.X_prev);
        }

        //проверка положения шара при скольжении влево
        [TestMethod]
        public void TestSlideLeft()
        {
            b.keyPressed.Clear();
            b.keyPressed.Add(0x25, 1);
            b.Y_prev = b.Y - 1;
            b.X_prev = b.X;
            b.Collision = Collision.bottom;
            b.Slide();

            b.Move();

            Assert.IsTrue(b.Y < b.Y_prev);
            Assert.IsTrue(b.X < b.X_prev);
        }

        //проверка пересечения шара с объектом сверху
        [TestMethod]
        public void TestIntersectionTop()
        {
            b.Collision = Collision.none;
            b.Y_prev = b.Y - 1;

            b.TestIntersection(b.X, b.Y+10-1, 20, 20);
            

            Assert.IsTrue(b.Collision == Collision.top);
        }

        //проверка пересечения шара с объектом снизу
        [TestMethod]
        public void TestIntersectionDown()
        {
            b.Collision = Collision.none;
            b.Y_prev = b.Y + 1;

            b.TestIntersection(b.X, b.Y - 20 + 1, 20, 20);


            Assert.IsTrue(b.Collision == Collision.bottom);
        }

        //проверка пересечения шара с объектом слева
        [TestMethod]
        public void TestIntersectionLeft()
        {
            b.Collision = Collision.none;
            b.X_prev = b.X - 1;

            b.TestIntersection(b.X+10-1, b.Y, 20, 20);


            Assert.IsTrue(b.Collision == Collision.left);
        }

        //проверка пересечения шара с объектом справа
        [TestMethod]
        public void TestIntersectionRight()
        {
            b.Collision = Collision.none;
            b.X_prev = b.X + 1;

            b.TestIntersection(b.X - 20 + 1, b.Y, 20, 20);


            Assert.IsTrue(b.Collision == Collision.right);
        }

        //проверка отсутвия пересечения шара с объектом
        [TestMethod]
        public void TestIntersectionNone()
        {
            b.Collision = Collision.none;
            b.Y_prev = b.Y - 1;
            b.X_prev = b.X - 1;

            b.TestIntersection(0, 0, 20, 20);


            Assert.IsTrue(b.Collision == Collision.none);
        }


    }
}
