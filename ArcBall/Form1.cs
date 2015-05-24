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
    public partial class MainForm : Form
    {
        Field f;
        int level, lifes, score;

        public MainForm()
        {
            InitializeComponent();

            level = 1;
            score = 0;
            lifes = 3;

            f = new Field(this.pictureBox1.CreateGraphics(), level, lifes, score, 600, 600);
            f.NextLevel += f_NextLevel;
            f.timer.Tick += timer_Elapsed;
            pictureBox1.Width = 600;
            pictureBox1.Height = 600;
            //this.Width = 640;
            //this.Height = 680;
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            lifesLabel.Text = "Жизни: " + f.Lifes;
            scoreLabel.Text = "Счёт: " + f.Score;
            Levellabel.Text = "Уровень: " + level;
            
        }



        void f_NextLevel(object sender, EventArgs e)
        {
            level++;
            lifes = f.Lifes;
            score = f.Score;
            f = new Field(this.pictureBox1.CreateGraphics(), level, lifes, score, 600, 600);
            f.NextLevel += f_NextLevel;
            f.timer.Tick += timer_Elapsed;
        }


    }
}
