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

        public Sudoku()
        {
            cells = new Cell[81];
            for (int i=0;i<cells.Length;i++)
            {
                cells[i] = new Cell(i);
            }
        }

        public Cell[] Cells { get => cells; set => cells = value; }

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
    }

    class Cell
    {
        private int entropy;
        private int value;

        public Cell(int value)
        {
            Entropy = 8;
            Value = value;
        }

        public int Value { get => value; set => this.value = value; }
        public int Entropy { get => entropy; set => entropy = value; }

    }
}
