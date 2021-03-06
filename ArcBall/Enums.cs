﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcBall
{
    //перечисление бонусов
    public enum Bonus
    {
        none = 0,
        life = 1,
        fastPlatform = 2,
        slowPlatform = 3,
        fastBall = 4,
        slowBall = 5,
        bigPlatform = 6,
        smallPlatform = 7,
        bigBall = 8,
        smallBall = 9,
        strongBall = 10
    }

    //перечисление нажатых кнопок
    public enum Keys
    {
        None = 0,
        Left = 1,
        Right = 2,
        Space = 3
    }

    //перечисление сторон блока, с которыми может столкнуться шар
    public enum Collision
    {
        none = 0,
        top = 1,
        right = 2,
        bottom = 3,
        left = 4
    }
}
