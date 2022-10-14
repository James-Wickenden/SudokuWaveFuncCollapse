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
        public static Label[] GenerateGraphicalGrid(int gridLength, int margin)
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
                    // For each box, create it and give it a border
                    boxes[i + (j * 3)] = new Panel
                    {
                        Size = new Size(boxLength, boxLength),
                        Location = new Point(boxLength * i, boxLength * j),
                        BorderStyle = BorderStyle.FixedSingle
                        
                    };

                    Label lbl_index = new Label();
                    lbl_index.Text = '(' + i.ToString() + ',' + j.ToString() + ')' + '='+ (i + (j * 3)).ToString(); 
                    boxes[i + (j * 3)].Controls.Add(lbl_index);
                    //gridPanel.Controls.Add(boxes[i + (j * 3)]);
                }
            }

            PopulateBoxes(cells, boxes, boxLength);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    gridPanel.Controls.Add(boxes[i + (j * 3)]);
                }
            }

            return cells;
        }

        // Add the cells inside the boxes, using the same code as for boxes inside the grid
        private static void PopulateBoxes(Label[] cells, Panel[] boxes, int boxLength)
        {
            int cellLength = boxLength / 3;
            string tablecalcstring = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int boxIndex = ((i / 3) * 3) + (j / 3);
                    cells[i + (j * 9)] = new Label()
                    {
                        Size = new Size(cellLength, cellLength),
                        Location = new Point(cellLength * i, cellLength * j),
                        BorderStyle = BorderStyle.FixedSingle,
                        Text = i.ToString() + ',' + j.ToString() + " = " + boxIndex.ToString()
                    };

                    
                    tablecalcstring += i.ToString() + ',' + j.ToString() + " = " + boxIndex.ToString() + '\n';
                    boxes[boxIndex].Controls.Add(cells[i + (j * 9)]);
                }
            }
            MessageBox.Show(tablecalcstring);
        }
    }
}
