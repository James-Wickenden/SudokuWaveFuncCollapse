using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuWaveFuncCollapse
{
    public class GraphicGrid
    {
        private Panel gridPanel;
        private Label[] cells;
        private static Dictionary<int, int> boxCellMap;

        public GraphicGrid(int gridLength, int margin) {
            // First, generate the sudoku panel
            gridPanel = new Panel
            {
                BackColor = Color.White,
                Location = new Point(margin, margin),
                Size = new Size(gridLength, gridLength)
            };

            // Create the map that links a cell index to the box it lives in
            boxCellMap = new Dictionary<int, int>();

            // Then, the boxes and cells in the grid
            cells = GenerateGraphicalGridLabels(gridLength, gridPanel);
        }

        // Sub for generating the grid panel and the labels that represent sudoku tiles
        private static Label[] GenerateGraphicalGridLabels(int gridLength, Panel gridPanel)
        {
            // Now, generate the boxes in the grid, and the cells in the boxes
            Panel[] boxes = new Panel[9];
            Label[] cells = new Label[81];
            int boxLength = gridLength / 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // For each box, create it and give it a border
                    boxes[i + (j * 3)] = new Panel
                    {
                        Size = new Size(boxLength, boxLength),
                        Location = new Point(boxLength * i, boxLength * j),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    // Then fill the box with cells, so that the result is sequentially ordered wrt the grid as a whole
                    PopulateBox(cells, boxes[i + (j * 3)], i + (j * 3), boxLength);
                    gridPanel.Controls.Add(boxes[i + (j * 3)]);
                }
            }

            return cells;
        }

        // Add the cells inside a given box, using the same code as for boxes inside the grid
        private static void PopulateBox(Label[] cells, Panel box, int boxIndex, int boxLength)
        {
            int cellLength = boxLength / 3;

            // Calculates the index of the top left cell for that box wrt 1D cell and box arrays. For example:
            // Box(0) -> Cells ( 0, 1, 2, 9,10,11,18,19,20)
            // Box(3) -> Cells (27,28,29,36,37,38,45,46,47)
            // Box(8) -> Cells (60,61,62,69,70,71,78,79,80)
            int topLeftCellIndex = (((boxIndex / 3) * 3) * 9) + ((boxIndex % 3) * 3);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Then use the top left cell index for our box to get our cell's index
                    int cellIndex = (topLeftCellIndex + i) + (j * 9);
                    cells[cellIndex] = new Label()
                    {
                        Size = new Size(cellLength, cellLength),
                        Location = new Point(cellLength * i, cellLength * j),
                        BorderStyle = BorderStyle.FixedSingle,
                        Text = cellIndex.ToString()
                    };

                    boxCellMap[cellIndex] = boxIndex;
                    box.Controls.Add(cells[cellIndex]);
                }
            }
        }

        public Label[] GetCells()
        {
            return cells;
        }

        public void UpdateCellLabel(int cellIndex, string newText)
        {
            cells[cellIndex].Text = newText;
        }

        public Panel GetGridPanel()
        {
            return gridPanel;
        }

        public Dictionary<int,int> GetBoxCellMap()
        {
            return boxCellMap;
        }
    }
}
