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
        private static readonly int margin = 25;
        public static Panel GenerateGraphicalGrid(int panelWidth, int panelHeight)
        {
            Panel gridPanel = new Panel();
            gridPanel.BackColor = Color.White;
            gridPanel.Location = new Point(margin, margin);
            gridPanel.Size = new Size(panelWidth, panelHeight);
            return gridPanel;
        }
    }
}
