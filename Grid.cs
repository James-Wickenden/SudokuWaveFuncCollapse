using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuWaveFuncCollapse
{
    public class Grid
    {
        // Sub for generating the grid panel and the labels that represent sudoku tiles
        public static Panel GenerateGraphicalGrid(int gridLength, int margin)
        {
            // First, generate the sudoku panel
            Panel gridPanel = new Panel
            {
                BackColor = Color.White,
                Location = new Point(margin, margin),
                Size = new Size(gridLength, gridLength)
            };

            // Now, generate the boxes in the grid, and the cells in the boxes
            Panel[] boxes = new Panel[9];
            Label[] cells = new Label[81];
            int boxLength = gridLength / 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    boxes[i + (j * 3)] = new Panel
                    {
                        Size = new Size(boxLength, boxLength),
                        Location = new Point(boxLength * i, boxLength * j),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    PopulateBox(cells, boxes[i + (j * 3)], i + (j * 3), boxLength);

                    gridPanel.Controls.Add(boxes[i + (j * 3)]);
                }
            }

            return gridPanel;
        }

        private static void PopulateBox(Label[] cells, Panel boxPanel, int boxIndex, int boxLength)
        {
            int cellLength = boxLength / 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cells[(boxIndex * 9) + i + (j * 3)] = new Label()
                    {
                        Size = new Size(cellLength, cellLength),
                        Location = new Point(cellLength * i, cellLength * j),
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    boxPanel.Controls.Add(cells[(boxIndex * 9) + i + (j * 3)]);
                }
            }
        }
    }
}
