using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuWaveFuncCollapse
{
    class WaveCollapseModel
    {
        // Maps indices to the string that represents the sudoku grid starting configuration
        private Dictionary<int, string> sudokuMap = new Dictionary<int, string>();
        private readonly string filename = "sudos.txt";

        public WaveCollapseModel()
        {
            ParseSudokus(filename);
        }

        // Take the sudoku board and advance it by one move.
        // Do this by picking the cell with the least entropy and filling it out,
        // Then reducing the entropy of affected cells
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

        // When the board is created, fill out the model information by reducing entropy in initial cells.
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

        // For a picked cell to be filled out, eliminate it as an option in the cells in its row, col, and box
        private void ReduceEntropyInModel(Sudoku sudoku, Cell picked)
        {
            int pickedIndex = picked.CellIndex;
            int pickedValue = picked.Value;
            // Array of list of cells to have their entropy reduced.
            // There will be some duplicates in here, so they are tested for.
            List<Cell>[] affectedCellLists = { sudoku.GetCellRow(pickedIndex),
                                               sudoku.GetCellCol(pickedIndex),
                                               sudoku.GetCellBox(pickedIndex) };

            foreach (List<Cell> affectedCells in affectedCellLists)
            {
                foreach (Cell affectedCell in affectedCells)
                {
                    // Test for duplicate cells across the affectedCellLists 
                    if (affectedCell.Value != pickedValue &&
                        affectedCell.Options.Contains(pickedValue) &&
                        !affectedCell.Filled)
                    {
                        // Decrement entropy and remove the picked cell as an option
                        affectedCell.Entropy -= 1;
                        affectedCell.Options.Remove(pickedValue);
                    }
                }
            }
        }

        // Parses the text file for the sudokus at the provided filename
        private void ParseSudokus(string filename)
        {
            string sudokuFileString = System.IO.File.ReadAllText(filename);
            string[] sudokuFileLines = sudokuFileString.Split('\n');

            for (int i=0;i< (sudokuFileLines.Length/10);i++)
            {
                string sudokuBuiltStr = "";
                int sudokuNo = int.Parse(sudokuFileLines[(i * 10)].Split(' ').Last());
                for (int j=1;j<10;j++)
                {
                    sudokuBuiltStr += sudokuFileLines[(i * 10)+j];
                }

                // Strip newline chars
                sudokuBuiltStr = sudokuBuiltStr.Replace("\n", "").Replace("\r", "");
                // Replace 0s with spaces for drawing empty cells
                sudokuBuiltStr = sudokuBuiltStr.Replace('0', ' ');
                sudokuMap[sudokuNo] = sudokuBuiltStr;
            }
        }

        public string GetSudokuSetupString(int sudokuIndex)
        {
            return sudokuMap[sudokuIndex];
        }
    }

    class Sudoku
    {
        public Cell[] Cells = new Cell[81];
        
        // Create a new sudoku object, setting up the array of cells.
        public Sudoku(WaveCollapseModel model, GraphicGrid graphicGrid, string sudokuStartingConfig)
        {
            CreateCells(graphicGrid, sudokuStartingConfig);
            model.ReduceInitialEntropy(this);
        }

        private void CreateCells(GraphicGrid graphicGrid, string sudokuStartingConfig = "")
        {
            // Iterate through the sudoku grid rows and columns and create the cells at each location
            if (sudokuStartingConfig == "") sudokuStartingConfig = new string(' ', 81);

            for (int i=0;i<9;i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cells[i + (j * 9)] = new Cell(i, j, graphicGrid.GetBoxCellMap()[i + (j * 9)], sudokuStartingConfig[i + (j * 9)]);
                    graphicGrid.UpdateCellLabel(i + (j * 9), sudokuStartingConfig[i + (j * 9)].ToString());
                }
            }
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
