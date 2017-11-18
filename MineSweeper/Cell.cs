using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Cell
    {
        public static int Mines = 0;
        public static int Flaggs = 0;
        private Point pos;
        private bool revealed = false;
        private bool mine = false;
        private bool flagged = false;
        private int number = 0;

        public Cell(int x, int y, bool mine)
        {
            pos = new Point(x, y);
            this.mine = mine;
            if (this.mine == true)
                Cell.Mines++;
        }

        public bool Mine
        {
            get { return this.mine; }
        }

        public bool Revealed
        {
            get { return this.revealed; }
            set { this.revealed = value; }
        }

        public int Number
        {
            get { return this.number; }
            set { this.number = value; }
        }

        public bool Flagged
        {
            get { return this.flagged; }
            set { this.flagged = value; }
        }

        public Point Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }
    }
}
