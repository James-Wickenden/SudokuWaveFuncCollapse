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
        private Cell[] cells = new Cell[81];
        public Cell[] Cells { get => cells; set => cells = value; }

        public Sudoku(Dictionary<int,int> boxCellMap)
        {
            CreateCells(boxCellMap);
        }
        
        public Sudoku(Dictionary<int, int> boxCellMap, string filename)
        {
            string loadedSudokuGrid = System.IO.File.ReadAllText(filename);
            loadedSudokuGrid = loadedSudokuGrid.Replace("\n", "").Replace("\r", "");
            CreateCells(boxCellMap, loadedSudokuGrid);
        }

        private void CreateCells(Dictionary<int, int> boxCellMap, string loadedSudokuGrid = "")
        {
            // Iterate through the sudoku grid rows and columns and create the cells at each location
            if (loadedSudokuGrid=="") loadedSudokuGrid = new string(' ', 81);

            for (int i=0;i<9;i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i + (j * 9)] = new Cell(i, j, boxCellMap[i + (j * 9)], loadedSudokuGrid[i + (j * 9)]);
                }
            }
        }

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
        public int Entropy;
        public bool Filled; 
        public int Value;
        public int BoxIndex;
        public int CellIndex;
        public Tuple<int,int> Position;

        public Cell(int cell_x, int cell_y, int boxIndex, char cValue)
        {
            Entropy = 8;
            Filled = false;
            Value = -1;
            if (char.IsNumber(cValue)) Value = int.Parse(cValue.ToString());

            Position = new Tuple<int, int>(cell_x, cell_y);
            CellIndex = cell_x + (cell_y * 9);
            BoxIndex = boxIndex;
        }
    }
}
