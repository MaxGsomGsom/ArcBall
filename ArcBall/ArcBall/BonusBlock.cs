using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcBall
{
    public class BonusBlock : Block, IBonusBlock
    {
        Bonus bonus;

        public BonusBlock(Graphics g, double x, double y, int sizeX, int sizeY, int health, Bonus bonus) : base(g, x, y, sizeX, sizeY, health)
        {
            //тип бонуса
            this.bonus = bonus;
        }


        public new void Draw()
        {
            base.Draw();
            //отрисовка звездочки, если есть бонус
            g.DrawString("●", new Font(FontFamily.GenericSerif, 16, FontStyle.Bold), Brushes.Black, (int)x, (int)y);

        }

        //применить бонус
        public void ActivateBonus(IField field)
        {
            switch (bonus)
            {
                case Bonus.life:
                    field.Lifes++;
                    break;
                case Bonus.fastPlatform:
                    field.Platform.Speed *= 2;
                    break;
                case Bonus.slowPlatform:
                    field.Platform.Speed /= 2;
                    break;
                case Bonus.fastBall:
                    field.Ball.Speed *= 2;
                    break;
                case Bonus.slowBall:
                    field.Ball.Speed /= 2;
                    break;
                case Bonus.bigPlatform:
                    field.Platform.Size_X *= 2;
                    break;
                case Bonus.smallPlatform:
                    field.Platform.Size_X /= 2;
                    break;
                case Bonus.bigBall:
                    field.Ball.Radius *= 2;
                    break;
                case Bonus.smallBall:
                    field.Ball.Radius /= 2;
                    break;
                case Bonus.strongBall:
                    field.Ball.Power *= 2;
                    break;
            }
        }

        public Bonus Bonus
        {
            get { return bonus; }
        }
    }
}
