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
    public partial class SudokuForm : Form
    {
        private readonly int sideLength = 450;
        private static readonly int margin = 25;
        
        private Panel backPanel;
        private Sudoku sudoku;
        private GraphicGrid graphicGrid;

        public SudokuForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // First, configure the window and add a panel to it
            ConfigureForm();

            // Now, generate the sudoku grid panel and its label children
            graphicGrid = new GraphicGrid(sideLength, margin);
            backPanel.Controls.Add(graphicGrid.GetGridPanel());

            // Next, create the sudoku model
            sudoku = new Sudoku(graphicGrid.GetBoxCellMap(), graphicGrid);
        }

        private void ConfigureForm()
        {
            // Set the size of the window
            // The addition is to allow space for margins, and the bar at the top
            int mywidth = sideLength + (margin * 2) + 20;
            int myheight = sideLength + (margin * 2) + 140;
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

            AddButtons();
        }

        private void AddButtons()
        {
            Button buttonAdvance = new Button() {
                Width = 210, Height = 50, Top = 500, BackColor=Color.White, 
                Text = "Advance",
                Left = 25
            };
            backPanel.Controls.Add(buttonAdvance);
            buttonAdvance.Click += (sender, args) => {
                Cell picked = sudoku.Model.AdvanceModel(sudoku); 
                if (!(picked is null))
                {
                    graphicGrid.UpdateCellLabel(picked.CellIndex, picked.Value.ToString());
                }
            };

            Button buttonRestart = new Button()
            {
                Width = 210, Height = 50, Top = 500, BackColor=Color.White,
                Text = "Restart",
                Left = 265
            };
            backPanel.Controls.Add(buttonRestart);
            buttonRestart.Click += (sender, args) => {
                sudoku = new Sudoku(graphicGrid.GetBoxCellMap(), graphicGrid);
            };
        }
    }
}
