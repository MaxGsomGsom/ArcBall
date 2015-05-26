using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcBall
{
    //главная игровая форма
    public partial class GameForm : Form
    {
        Field f;
        int level, lifes, score;
        //размер игрового поля
        int fieldSize = 600;

        //конструктор
        public GameForm()
        {
            InitializeComponent();
            //установка начальных значений
            level = 1;
            score = 0;
            lifes = 3;

            //создание уровня
            f = new Field(this.pictureBox1.CreateGraphics(), level, lifes, score, fieldSize, fieldSize);
            //назначение обработчиков событиям
            f.NextLevel += f_NextLevel;
            f.Timer.Tick += timer_Elapsed;
            f.GameOver += f_GameOver;
            //установка размеров элементов формы
            pictureBox1.Width = fieldSize;
            pictureBox1.Height = fieldSize;
            this.Width = fieldSize+40;
            this.Height = fieldSize+90;
            Levellabel.Left = Width / 2 - 50;
            scoreLabel.Left = Width - 100;
        }

        //обработчик события окончания игры
        void f_GameOver(object sender, EventArgs e)
        {
            MessageBox.Show("Игра окончена.\nВаш счёт: "+score, "Игра окончена");
            this.Close();
        }

        //обновление информации по каждому тику таймера
        private void timer_Elapsed(object sender, EventArgs e)
        {
            lifesLabel.Text = "Жизни: " + f.Lifes;
            scoreLabel.Text = "Счёт: " + f.Score;
            Levellabel.Text = "Уровень: " + level;
            
        }


        //обработчик события новый уровень
        void f_NextLevel(object sender, EventArgs e)
        {
            f.Timer.Stop();

            if (level < 3)
            {
                //чтение значений с предыдущего уровня
                level++;
                lifes = f.Lifes;
                score = f.Score;
                //создание уровня
                f = new Field(this.pictureBox1.CreateGraphics(), level, lifes, score, fieldSize, fieldSize);
                //назначение обработчиков событиям
                f.NextLevel += f_NextLevel;
                f.Timer.Tick += timer_Elapsed;
                f.GameOver += f_GameOver;
            }
            else f_GameOver(this, new EventArgs());
        }




    }
}
