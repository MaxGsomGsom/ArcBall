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
    public partial class GameForm : Form
    {
        Field f;
        int level, lifes, score;
        int fieldSize = 600;

        public GameForm()
        {
            InitializeComponent();

            level = 1;
            score = 0;
            lifes = 3;

            f = new Field(this.pictureBox1.CreateGraphics(), level, lifes, score, fieldSize, fieldSize);
            f.NextLevel += f_NextLevel;
            f.Timer.Tick += timer_Elapsed;
            f.GameOver += f_GameOver;
            pictureBox1.Width = fieldSize;
            pictureBox1.Height = fieldSize;
            this.Width = fieldSize+40;
            this.Height = fieldSize+90;
            Levellabel.Left = Width / 2 - 50;
            scoreLabel.Left = Width - 100;
        }

        void f_GameOver(object sender, EventArgs e)
        {
            MessageBox.Show("Игра окончена.\nВаш счёт: "+score, "Игра окончена");
            this.Close();
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            lifesLabel.Text = "Жизни: " + f.Lifes;
            scoreLabel.Text = "Счёт: " + f.Score;
            Levellabel.Text = "Уровень: " + level;
            
        }



        void f_NextLevel(object sender, EventArgs e)
        {
            f.Timer.Stop();

            if (level < 3)
            {
                level++;
                lifes = f.Lifes;
                score = f.Score;
                f = new Field(this.pictureBox1.CreateGraphics(), level, lifes, score, fieldSize, fieldSize);
                f.NextLevel += f_NextLevel;
                f.Timer.Tick += timer_Elapsed;
                f.GameOver += f_GameOver;
            }
            else f_GameOver(this, new EventArgs());
        }




    }
}
