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
    //форма помощи
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
            textBox1.Select(0, 0);
        }
    }
}
