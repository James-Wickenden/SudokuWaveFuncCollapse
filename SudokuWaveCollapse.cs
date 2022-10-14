using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuWaveFuncCollapse
{
    class SudokuWaveCollapse
    {
        
    }

    class Sudoku
    {
        private Cell[] cells;

        public Sudoku(Dictionary<int,int> boxCellMap)
        {
            // Create the new array of cells for the sudoku
            cells = new Cell[81];

            for (int i=0;i<9;i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i + (j * 9)] = new Cell(i, j, boxCellMap[i + (j * 9)]);
                }
            }
        }

        public Cell[] Cells { get => cells; set => cells = value; }

        // Methods for extracting columns, rows, and boxes for a cell.
        // Used for calculating cell entropy and validity.
        public List<Cell> GetCellColumn(int cellIndex)
        {
            List<Cell> colCells = new List<Cell>();
            int gridHorizontalOffset = cellIndex % 9;
            for (int i = 0; i < 9; i++)
            {
                colCells.Add(cells[gridHorizontalOffset + (i * 9)]);
            }
            return colCells;
        }

        public List<Cell> GetCellRow(int cellIndex)
        {
            List<Cell> rowCells = new List<Cell>();
            int gridVerticalOffset = cellIndex - (cellIndex % 9);
            for (int i = 0; i < 9; i++)
            {
                rowCells.Add(cells[gridVerticalOffset + i]);
            }
            return rowCells;
        }

        public List<Cell> GetCellBox(int cellIndex)
        {
            List<Cell> boxCells = new List<Cell>();

            int boxIndex = cells[cellIndex].BoxIndex;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cells[i + (j * 9)].BoxIndex == boxIndex) boxCells.Add(cells[i + (j * 9)]);
                }
            }

            return boxCells;
        }
    }

    class Cell
    {
        private int entropy;
        private int value;
        private int boxIndex;
        private Tuple<int,int> position;

        public Cell(int cell_x, int cell_y, int boxIndex)
        {
            entropy = 8;
            value = -1;
            position = new Tuple<int, int>(cell_x, cell_y);
            this.boxIndex = boxIndex;
        }

        public int Value { get => value; set => this.value = value; }
        public int Entropy { get => entropy; set => entropy = value; }
        public Tuple<int, int> Position { get => position; set => position = value; }
        public int BoxIndex { get => boxIndex; set => boxIndex = value; }
    }
}
