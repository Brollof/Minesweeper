using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class form1 : Form
    {
        Minesweeper ms;

        public form1()
        {
            InitializeComponent();
        }

        private void NewGame()
        {
            int xCellNum = 10;
            int yCellNum = 10;
            int mines = 20;

            int width = Minesweeper.cellSize * xCellNum;
            int height = Minesweeper.cellSize * yCellNum;

            ms = new Minesweeper(xCellNum, yCellNum, mines);

            pbField.Location = new Point(0, 0);
            pbField.Height = height;
            pbField.Width = width;

            this.ClientSize = new System.Drawing.Size(width - Minesweeper.trim, height - Minesweeper.trim);
            pbField.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewGame();
        }

        private void pbField_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ms.Draw(g);
        }

        private void pbField_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if(ms.GameOver == true)
                {
                    DialogResult res = MessageBox.Show("New game?", "You are loser!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if(res == DialogResult.Yes)
                    {
                        NewGame();
                        return;
                    }
                    else
                    {
                        this.Close();
                    }
                }

                bool over = ms.LeftClick(e.Location);
                if (over == true)
                {
                    ms.GameOver = true;
                }
                pbField.Invalidate();
            }
            else if(e.Button == MouseButtons.Right)
            {
                // flag mine
                ms.RightClick(e.Location);
                pbField.Invalidate();
            }

            if (ms.CheckWinCondition())
            {
                DialogResult res = MessageBox.Show("You won!\n\nNew game?", "Congratulations!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (res == DialogResult.Yes)
                {
                    NewGame();
                    return;
                }
                else
                {
                    this.Close();
                }
            }
        }


    }
}
