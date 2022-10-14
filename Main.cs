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
        private readonly int sideLength = 450;
        private static readonly int margin = 25;
        private Panel backPanel;
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // First, configure the window and add a panel to it
            ConfigureForm();

            // Now, generate the sudoku grid panel and its label children
            Grid graphicGrid = new Grid(sideLength, margin);
            backPanel.Controls.Add(graphicGrid.GetGridPanel());

            // Next, create the sudoku model
            Sudoku sudoku = new Sudoku(graphicGrid.GetBoxCellMap());
            

            for (int i = 0; i < 9; i++)
            {
                for (int j=0;j<9;j++)
                {
                    //graphicGrid.UpdateCellLabel(i + (j * 9), sudoku.Cells[i + (j * 9)].BoxIndex.ToString());
                }
            }
        }

        private void ConfigureForm()
        {
            // Set the size of the window
            // The addition is to allow space for margins, and the bar at the top
            int mywidth = sideLength + (margin * 2) + 20;
            int myheight = sideLength + (margin * 2) + 40;
            this.Size = new Size(mywidth, myheight);
            this.Text = "Sudoku Wave Collapse Function Solver";

            // Create a background panel to attach the grid panel to.
            // This is a workaround for margins which are weird and seem to only kick in when docked, which i don't care for
            backPanel = new Panel
            {
                Size = this.Size,
                BackColor = Color.Black
            };
            this.Controls.Add(backPanel);
        }
    }
}
