﻿using System;
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
    public partial class Main : Form
    {
        private readonly int sideLength = 450;
        private static readonly int margin = 25;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the size of the window
            // The addition is to allow space for margins, and the bar at the top
            int mywidth = sideLength + (margin * 2) + 20;
            int myheight = sideLength + (margin * 2) + 40;
            this.Size = new Size(mywidth, myheight);
            this.Text = "Sudoku Wave Collapse Function Solver";

            // Create a background panel to attach the grid panel to.
            // This is a workaround for margins which are weird and seem to only kick in when docked, which i don't care for
            Panel borderBack = new Panel
            {
                Size = this.Size,
                BackColor = Color.Black
            };
            this.Controls.Add(borderBack);

            // Now, generate the sudoku grid and its label children
            Label[] cells = Grid.GenerateGraphicalGrid(sideLength, margin);
            Panel gridPanel = (Panel)cells[0].Parent.Parent;
            borderBack.Controls.Add(gridPanel);

            for (int i=0;i<cells.Length;i++)
            {
                cells[i].Text = i.ToString();
            }
        }
    }
}
