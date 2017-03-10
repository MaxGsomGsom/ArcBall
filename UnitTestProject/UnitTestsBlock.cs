using ArcBall;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsBlock
    {
        Bitmap img = null;
        Graphics g = null;
        Field field = null;
        Block b = null;

        //инициализация объекта для тестирования
        [TestInitialize]
        public void Init()
        {
            img = new Bitmap(100, 100);
            g = Graphics.FromImage(img);

            g.Clear(Color.White);

            field = new Field(g, 1, 3, 0, 200, 200);
            b = new Block(g, 0, 0, 20, 20, 10);
        }


        //проверка конструктора
        [TestMethod]
        public void TestConstructor()
        {
            Assert.IsTrue(b.X == 0);
            Assert.IsTrue(b.Y == 0);

            Assert.IsTrue(b.Size_X == 20);
            Assert.IsTrue(b.Size_Y == 20);

            Assert.IsTrue(b.Health > 0);
        }


        //проверка бонуса ускорения и замедления шарика
        [TestMethod]
        public void TestSpeedBallBonus()
        {
            int speed1 = field.Ball.Speed;

            BonusBlock bonus1 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.fastBall);
            bonus1.ActivateBonus(field);

            int speed2 = field.Ball.Speed;

            BonusBlock bonus2 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.slowBall);
            bonus2.ActivateBonus(field);
            int speed3 = field.Ball.Speed;

            Assert.IsTrue(speed1 < speed2 && speed2 > speed3);
        }

        //проверка бонуса увеличения и уменьшения шарика
        [TestMethod]
        public void TestRadiusBallBonus()
        {
            int rad1 = field.Ball.Radius;

            BonusBlock bonus1 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.bigBall);
            bonus1.ActivateBonus(field);

            int rad2 = field.Ball.Radius;

            BonusBlock bonus2 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.smallBall);
            bonus2.ActivateBonus(field);
            int rad3 = field.Ball.Radius;

            Assert.IsTrue(rad1 < rad2 && rad2 > rad3);
        }

        //проверка бонуса увеличения и уменьшения платформы
        [TestMethod]
        public void TestSizePlatformBonus()
        {
            int size1 = field.Platform.Size_X;

            BonusBlock bonus1 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.bigPlatform);
            bonus1.ActivateBonus(field);

            int size2 = field.Platform.Size_X;

            BonusBlock bonus2 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.smallPlatform);
            bonus2.ActivateBonus(field);
            int size3 = field.Platform.Size_X;

            Assert.IsTrue(size1 < size2 && size2 > size3);
        }

        //проверка бонуса ускорения и замедления платформы
        [TestMethod]
        public void TestSpeedPlatformBonus()
        {
            int speed1 = field.Platform.Speed;

            BonusBlock bonus1 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.fastPlatform);
            bonus1.ActivateBonus(field);

            int speed2 = field.Platform.Speed;

            BonusBlock bonus2 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.slowPlatform);
            bonus2.ActivateBonus(field);
            int speed3 = field.Platform.Speed;

            Assert.IsTrue(speed1 < speed2 && speed2 > speed3);
        }

        //проверка бонуса усиления шара
        [TestMethod]
        public void TestStrongBallBonus()
        {
            int power1 = field.Ball.Power;

            BonusBlock bonus1 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.strongBall);
            bonus1.ActivateBonus(field);

            int power2 = field.Ball.Power;

            Assert.IsTrue(power1 < power2);
        }

        //проверка бонуса жизнь
        [TestMethod]
        public void TestLifeBonus()
        {
            int life1 = field.Lifes;

            BonusBlock bonus1 = new BonusBlock(g, 0, 0, 20, 20, 0, Bonus.life);
            bonus1.ActivateBonus(field);

            int life2 = field.Lifes;

            Assert.IsTrue(life1 < life2);
        }

        //проверка повреждени блока
        [TestMethod]
        public void TestDamageBlock()
        {
            int health1 = b.Health;
            b.Damage(1);
            int health2 = b.Health;

            Assert.IsTrue(health1 == (health2 + 1));
        }

        //проверка отрисовки
        [TestMethod]
        public void TestDraw()
        {
            b.Draw();

            Assert.IsTrue(img.GetPixel((int)b.X + 5, (int)b.Y + 5).ToArgb() != Color.White.ToArgb());
        }

        //проверка столкновения с шаром 
        [TestMethod]
        public void TestCollideWithBall()
        {
            Ball ball = new Ball(g, 5, 5, 10);
            bool collide = ball.TestIntersection(b);

            ball = new Ball(g, 500, 500, 10);
            bool noCollide = !ball.TestIntersection(b);


            Assert.IsTrue(collide && noCollide);
        }
    }
}
