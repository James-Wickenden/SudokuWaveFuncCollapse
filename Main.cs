using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuWaveFuncCollapse
{
    public partial class Main : Form
    {
        private readonly int sideLength = 800;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(sideLength+70, sideLength+90);

            Panel back = new Panel();
            back.Size = this.Size;
            back.BackColor = Color.Black;
            back.Padding = new Padding(20);
            this.Controls.Add(back);

            Panel gridPanel = Grid.GenerateGraphicalGrid(sideLength, sideLength);
            back.Controls.Add(gridPanel);
        }
    }
}
