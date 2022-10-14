using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuWaveFuncCollapse
{
    class WaveCollapseModel
    {
        public Cell AdvanceModel(Sudoku sudoku)
        {
            List<Cell> zeroEntropyCells = new List<Cell>();
            Cell[] cells = sudoku.Cells;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cells[i + (j * 9)].Entropy == 0 && !cells[i + (j * 9)].Filled) 
                        zeroEntropyCells.Add(cells[i + (j * 9)]);
                }
            }

            if (zeroEntropyCells.Count == 0) return null;
            Cell picked = zeroEntropyCells[(new Random()).Next(zeroEntropyCells.Count)];
            picked.Value = picked.Options[0];
            picked.Filled = true;
            ReduceEntropyInModel(sudoku, picked);

            return picked;
        }

        public void ReduceInitialEntropy(Sudoku sudoku)
        {
            Cell[] cells = sudoku.Cells;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cells[i + (j * 9)].Filled) ReduceEntropyInModel(sudoku, cells[i + (j * 9)]);
                }
            }
        }

        private void ReduceEntropyInModel(Sudoku sudoku, Cell picked)
        {
            int pickedIndex = picked.CellIndex;
            int pickedValue = picked.Value;
            List<Cell>[] affectedCellLists = { sudoku.GetCellRow(pickedIndex),
                                               sudoku.GetCellCol(pickedIndex),
                                               sudoku.GetCellBox(pickedIndex) };

            foreach (List<Cell> affectedCells in affectedCellLists)
            {
                foreach (Cell affectedCell in affectedCells)
                {
                    if (affectedCell.Value != pickedValue &&
                        affectedCell.Options.Contains(pickedValue) &&
                        !affectedCell.Filled)
                    {
                        affectedCell.Entropy -= 1;
                        affectedCell.Options.Remove(pickedValue);
                    }
                }
            }
        }
    }

    class Sudoku
    {
        public Cell[] Cells = new Cell[81];
        public WaveCollapseModel Model = new WaveCollapseModel();

        public Sudoku(Dictionary<int,int> boxCellMap, GraphicGrid graphicGrid)
        {
            string loadedSudokuGrids = System.IO.File.ReadAllText("sudokus.txt");
            string[] sudokuStrs = loadedSudokuGrids.Split('G');
            for(int i = 1; i < sudokuStrs.Length; i++)
            {
                int index = sudokuStrs[i].IndexOf(System.Environment.NewLine);
                sudokuStrs[i] = sudokuStrs[i].Substring(index + System.Environment.NewLine.Length);
            }
            string randomSudoku = sudokuStrs[(new Random()).Next(sudokuStrs.Length)];
            CreateCells(boxCellMap, graphicGrid, randomSudoku.Replace("\n", "").Replace("\r", ""));
        }
        
        public Sudoku(Dictionary<int, int> boxCellMap, GraphicGrid graphicGrid, string filename)
        {
            string loadedSudokuGrid = System.IO.File.ReadAllText(filename);
            loadedSudokuGrid = loadedSudokuGrid.Replace("\n", "").Replace("\r", "");
            CreateCells(boxCellMap, graphicGrid, loadedSudokuGrid);
        }

        private void CreateCells(Dictionary<int, int> boxCellMap, GraphicGrid graphicGrid, string loadedSudokuGrid = "")
        {
            // Iterate through the sudoku grid rows and columns and create the cells at each location
            if (loadedSudokuGrid=="") loadedSudokuGrid = new string(' ', 81);

            for (int i=0;i<9;i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cells[i + (j * 9)] = new Cell(i, j, boxCellMap[i + (j * 9)], loadedSudokuGrid[i + (j * 9)]);
                    graphicGrid.UpdateCellLabel(i + (j * 9), loadedSudokuGrid[i + (j * 9)].ToString());
                }
            }

            Model.ReduceInitialEntropy(this);
        }

        // Methods for extracting columns, rows, and boxes for a cell.
        // Used for calculating cell entropy and validity.
        public List<Cell> GetCellCol(int cellIndex)
        {
            List<Cell> colCells = new List<Cell>();
            int gridHorizontalOffset = cellIndex % 9;
            for (int i = 0; i < 9; i++)
            {
                colCells.Add(Cells[gridHorizontalOffset + (i * 9)]);
            }
            return colCells;
        }

        public List<Cell> GetCellRow(int cellIndex)
        {
            List<Cell> rowCells = new List<Cell>();
            int gridVerticalOffset = cellIndex - (cellIndex % 9);
            for (int i = 0; i < 9; i++)
            {
                rowCells.Add(Cells[gridVerticalOffset + i]);
            }
            return rowCells;
        }

        public List<Cell> GetCellBox(int cellIndex)
        {
            List<Cell> boxCells = new List<Cell>();

            int boxIndex = Cells[cellIndex].BoxIndex;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (Cells[i + (j * 9)].BoxIndex == boxIndex) boxCells.Add(Cells[i + (j * 9)]);
                }
            }

            return boxCells;
        }
    }

    class Cell
    {
        public int Entropy;
        public List<int> Options;
        public bool Filled; 
        public int Value;
        public int BoxIndex;
        public int CellIndex;
        public Tuple<int,int> Position;

        public Cell(int cell_x, int cell_y, int boxIndex, char cValue)
        {
            Entropy = 8;
            Options = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Filled = false;
            Value = -1;
            if (char.IsNumber(cValue))
            {
                Value = int.Parse(cValue.ToString());
                Options.Remove(Value);
                Filled = true;
            }

            Position = new Tuple<int, int>(cell_x, cell_y);
            CellIndex = cell_x + (cell_y * 9);
            BoxIndex = boxIndex;
        }
    }
}
