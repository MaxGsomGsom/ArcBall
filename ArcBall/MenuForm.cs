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
    //форма главного меню
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        //обработчик нажатия кнопки старт
        private void button1start_Click(object sender, EventArgs e)
        {
            GameForm game = new GameForm();
            game.Show();
        }

        //обработчик нажатия кнопки помощь
        private void button3help_Click(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            help.Show();
        }
    }
}
