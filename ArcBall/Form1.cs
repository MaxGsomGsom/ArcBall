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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            

            Field f = new Field(this.pictureBox1.CreateGraphics());
            pictureBox1.Width = 600;
            pictureBox1.Height = 600;
        }
    }
}
