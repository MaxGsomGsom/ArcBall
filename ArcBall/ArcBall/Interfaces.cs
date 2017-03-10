using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcBall
{

    public interface IGameObject
    {
        double X { get; }
        double Y { get; }
        void Draw();
    }

    public interface IGameObjectSquare : IGameObject
    {
        int Size_X { get; set; }
        int Size_Y { get; set; }
    }

    public interface IBall : IGameObject
    {
        int Power { get; set; }
        int Radius { get; set; }
        int Speed { get; set; }

        bool TestIntersection(IGameObjectSquare obj);
        void Slide(Keys key);
        void Move(Keys key);
    }

    public interface IPlatform : IGameObjectSquare
    {
        int Speed { get; set; }

        void Move(Keys key);
    }

    public interface IBlock : IGameObjectSquare
    {
        int Health { get; }

        void Damage(int power);

    }

    public interface IBonusBlock : IBlock
    {
        Bonus Bonus { get; }

        void ActivateBonus(IField field);

    }

    public interface IField
    {
        int Lifes { get; set; }
        int Score { get; }
        IBall Ball { get; }
        IPlatform Platform { get; }
        IList<IBlock> Blocks { get; }

        event EventHandler NextLevel;
        event EventHandler GameOver;
    }

}
