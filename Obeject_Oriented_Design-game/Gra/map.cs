using System;
using System.Collections.Generic;

namespace Gra
{
    /*===========*/
    /*    MAP    */
    /*===========*/

    enum Tile
    {
        Wall,
        NotWall
    }

    class Cell
    {
        private Tile isWall;
        private List<IItem> items;
        private Enemy? enemy;

        public Tile IsWall { get => isWall; set => isWall = value; }

        public Cell(Tile isWall, List<IItem>? items = null)
        {
            isWall = isWall;
            this.items = items ?? new List<IItem>();
        }        

        public bool movable()
        {
            return (isWall != Tile.Wall);
        }

        public IItem? PeekItem()
        {
            return items.Count > 0 ? items[^1] : null;
        }

        public IItem? extractItem()
        {
            if (items.Any())
            {
                IItem w = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return w;
            }
            else
            {
                return null;
            }
        }

        public void SetEnemy(Enemy? E)
        {
            enemy = E;
        }

        public Enemy? GetEnemy()
        {
            return enemy;
        }

        public void putItem(IItem I)
        {
            items.Add(I);
        }

        public (char, ConsoleColor) Render()
        {
            if (isWall == Tile.Wall)
            {
                return ('█', ConsoleColor.Gray);
            }
            else if (enemy != null)
            {
                return enemy.Render();
            }
            else if (items.Count > 0)
            {
                return items[^1].Render();
            }
            else
            {
                return (' ', ConsoleColor.Black);
            }
        }
    }


    class Map
    {
        private int height;
        private int width;
        private Cell[,] grid;

        public int Height => height;
        public int Width => width;

        public bool tileAviable(int X, int Y) // tells if tile exists and is not wall
        {
            return (X >= 0 && X < width && Y >= 0 && Y < height && grid[X, Y].movable());
        }

        public Map(int _width = 20, int _height = 40, Func<int, int, Cell> MapGenerator = null)
        {
            height = _height;
            width = _width;
            grid = new Cell[width, height];

            MapGenerator ??= (x, y) => new Cell(Tile.NotWall); // default

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid[i, j] = MapGenerator(i, j);
                }
            }
        }
        public Cell GetCell(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return grid[x, y];
            }
            throw new ArgumentOutOfRangeException("Coordinates are out of map bounds.");
        }

        public (char, ConsoleColor) GetRenderAt(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return grid[x, y].Render();
            }
            return (' ', ConsoleColor.Black);
        }

        public IItem? peekItem(int x, int y)
        {
            return tileAviable(x, y) ? grid[x, y].PeekItem() : null;
        }

        public string getTileStr(int x, int y)
        {
            if (!tileAviable(x, y))
                return "";

            if (GetEnemy(x, y) != null)
                return GetEnemy(x, y).getName();
            return grid[x, y].PeekItem()?.ToString() ?? "";
        }

        public IItem? extractItem(int x, int y)
        {
            if (tileAviable(x, y))
            {
                return grid[x, y].extractItem();
            }
            else
            {
                return null;
            }
        }

        public void putItem(int x, int y, IItem I)
        {
            grid[x, y].putItem(I);
        }

        public void SetEnemy(Enemy? E)
        {            
            grid[E.getX, E.getY].SetEnemy(E);
        }

        public void RemoveEnemy(int x, int y)
        {
            grid[x, y].SetEnemy(null);
        }

        public Enemy? GetEnemy(int x, int y)
        {
            return grid[x, y].GetEnemy();
        }

    }
}
