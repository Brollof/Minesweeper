using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    class Minesweeper
    {
        private int width, height;
        private int mines;
        private Cell[,] cells;
        private Random random = new Random();
        private bool gameOver = false;

        public const int cellSize = 40;
        public const int trim = 2;
        private const int vis = cellSize - trim;

        public Cell[,] Fields
        {
            get { return this.cells; }
        }

        public Minesweeper(int width, int height, int mines)
        {
            Init(width, height, mines);
        }

        public Minesweeper(int size, int mines)
        {
            Init(size, size, mines);
        }

        public bool GameOver
        {
            get { return this.gameOver; }
            set { this.gameOver = value; }
        }

        private void Init(int width, int height, int mines)
        {
            this.width = width;
            this.height = height;
            this.cells = new Cell[this.width, this.height];
            this.mines = mines;
            this.gameOver = false;

            Cell.Flaggs = 0;
            Cell.Mines = 0;
            
            GenerateFields();
            InitMinesNumber();
        }

        public void Draw(Graphics canvas)
        {
            DrawGame(canvas);
            if (gameOver == true)
            {
                DrawGameOver(canvas);
            }
        }

        private void DrawGame(Graphics canvas)
        {
            foreach (Cell cell in cells)
            {
                if (cell.Revealed == true)
                {
                    DrawRevealed(canvas, cell);
                }
                else if (cell.Flagged == true)
                {
                    DrawFlagged(canvas, cell);
                }
                else
                {
                    DrawUnknown(canvas, cell);
                }
            }
        }
        private void DrawRevealed(Graphics canvas, Cell cell)
        {
            Point pos = new Point(cellSize * cell.Pos.X, cellSize * cell.Pos.Y);

            if (cell.Mine == true)
            {
                canvas.FillRectangle(Brushes.Red, pos.X, pos.Y, vis, vis);
            }
            else
            {
                Font font = new Font(FontFamily.GenericSansSerif, 25);
                canvas.FillRectangle(Brushes.Azure, pos.X, pos.Y, vis, vis);
                if (cell.Number > 0)
                    canvas.DrawString(cell.Number.ToString(), font, Brushes.Green, pos.X + 3, pos.Y);
            }
        }

        private void DrawFlagged(Graphics canvas, Cell cell)
        {
            Point pos = new Point(cellSize * cell.Pos.X, cellSize * cell.Pos.Y);
            canvas.FillRectangle(Brushes.Black, pos.X, pos.Y, vis, vis);
        }

        private void DrawUnknown(Graphics canvas, Cell cell)
        {
            Point pos = new Point(cellSize * cell.Pos.X, cellSize * cell.Pos.Y);
            canvas.FillRectangle(Brushes.Navy, pos.X, pos.Y, vis, vis);
        }

        public bool LeftClick(Point loc)
        {
            int x = loc.X / cellSize;
            int y = loc.Y / cellSize;
            Cell cell = cells[x, y];

            if (cell.Flagged == true)
                return false;

            if (cell.Mine == true) // game over
                return true;

            Reveal(cell);
            
            return false;
        }

        public void RightClick(Point loc)
        {
            int x = loc.X / cellSize;
            int y = loc.Y / cellSize;
            Cell cell = cells[x, y];

            if (cell.Revealed == false)
            {
                if (cell.Flagged == false)
                {
                    cell.Flagged = true;
                    Cell.Flaggs++;
                }
                else
                {
                    cell.Flagged = false;
                    Cell.Flaggs--;
                }
            }
        }

        private void GenerateFields()
        {
            List<Cell> cellList = new List<Cell>();
            int i = 0;
            // Add mine fields to list
            for (i = 0; i < this.mines; i++ )
            {
                cellList.Add(new Cell(0,0, true));
            }
            // Add safe fields to list
            for (i = 0; i < this.width*this.height-this.mines; i++)
            {
                cellList.Add(new Cell(0,0, false));
            }

            // Shuffle list
            cellList = cellList.Distinct().OrderBy(x => System.Guid.NewGuid().ToString()).ToList();

            i = 0;
            // List to array
            foreach (var cell in cellList)
            {
                cells[i % this.width, (int)(i / this.height)] = cell;
                i++;
            }

            // Update cells posisiton
            for (i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    cells[i, j].Pos = new Point(i, j);
                }
            }
        }

        private void Flood(Cell cell)
        {
            int row = cell.Pos.X;
            int col = cell.Pos.Y;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if ((i != row || j != col) && i >= 0 && j >= 0 && i < this.width && j < this.height)
                    {
                        if (cells[i, j].Mine == false && cells[i, j].Revealed == false)
                            Reveal(cells[i, j]);
                    }
                }
            }
        }
        private void Reveal(Cell cell)
        {
            cell.Revealed = true;
            if (cell.Number == 0)
                Flood(cell);
        }

        private void InitMinesNumber()
        {
            for (int i = 0; i < this.width; i++)
            {
                for (int j = 0; j < this.height; j++)
                {
                    ComputeMines(i, j);
                }
            }
        }

        private void ComputeMines(int row, int col)
        {
            int mines = 0;
            for(int i = row - 1; i <= row + 1; i++)
            {
                for(int j = col - 1; j <= col + 1; j++)
                {
                    if((i != row || j != col) && i >= 0 && j >= 0 && i<this.width && j<this.height)
                    {
                        if (cells[i, j].Mine == true)
                            mines++;
                    }
                }
            }
            cells[row, col].Number = mines;
        }

        private void DrawGameOver(Graphics canvas)
        {
            foreach(Cell cell in cells)
            {
                if(cell.Mine == true)
                {
                    Point pos = new Point(cellSize * cell.Pos.X, cellSize * cell.Pos.Y);
                    canvas.FillRectangle(Brushes.Red, pos.X, pos.Y, vis, vis);
                }
            }
        }

        public bool CheckWinCondition()
        {
            foreach(Cell cell in cells)
            {
                if (cell.Mine == false && cell.Revealed == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
