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
        private int sudokuIndex = 1;

        private Panel backPanel;
        private Sudoku sudoku;
        private GraphicGrid graphicGrid;
        private WaveCollapseModel model;

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

            // Next, create the sudoku and its model
            model = new WaveCollapseModel();
            string sudokuSetupStr = model.GetSudokuSetupString(sudokuIndex);
            sudoku = new Sudoku(model, graphicGrid, sudokuSetupStr);

        }

        private void ConfigureForm()
        {
            // Set the size of the window
            // The addition is to allow space for margins, and the bar at the top
            int mywidth = sideLength + (margin * 2) + 20;
            int myheight = sideLength + (margin * 2) + 140;
            this.Size = new Size(mywidth, myheight);
            this.Text = "Sudoku Wave Collapse Function Solver " + sudokuIndex.ToString();

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

        // Add buttons for advancing the sudoku, and for moving to the next board
        // (There are 50 boards; once it reaches the end, it loops back to board 1).
        private void AddButtons()
        {
            Button buttonAdvance = new Button() {
                Width = 210, Height = 50, Top = 500, BackColor=Color.White, 
                Text = "Advance",
                Left = 25
            };
            backPanel.Controls.Add(buttonAdvance);
            buttonAdvance.Click += (sender, args) => {
                // Get the updated cell from the model advancing, and fill it in on the grid panel
                Cell picked = model.AdvanceModel(sudoku); 
                if (!(picked is null))
                {
                    graphicGrid.UpdateCellLabel(picked.CellIndex, picked.Value.ToString());
                }
            };

            Button buttonRestart = new Button()
            {
                Width = 210, Height = 50, Top = 500, BackColor=Color.White,
                Text = "Next",
                Left = 265
            };
            backPanel.Controls.Add(buttonRestart);
            buttonRestart.Click += (sender, args) => {
                // Create the new sudoku, which automatically updates the labels on the board
                sudokuIndex = (sudokuIndex % 50) + 1;
                string sudokuSetupStr = model.GetSudokuSetupString(sudokuIndex);
                sudoku = new Sudoku(model, graphicGrid, sudokuSetupStr);
                this.Text = "Sudoku Wave Collapse Function Solver " + sudokuIndex.ToString();
            };
        }
    }
}
