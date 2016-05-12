﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LeePathfinderLib
{
    public struct Cell//Todo: add constructors
    {
        public int Row;
        public int Column;
    }
    public class Pathfinder
    {
        private readonly int[,] _maze;
        private readonly int _rowCount;
        private readonly int _columnCount;
        private readonly Cell _start;
        private readonly Cell _end;
        private List<Cell> _currentCells = new List<Cell>();
        private bool _pathFound;

        public Pathfinder(int[,] maze)
        {
            _maze = maze;
            _rowCount = _maze.GetLength(0);
            _columnCount = _maze.GetLength(1);
            for (var i = 0; i < _rowCount; i++)
            {
                for (var j = 0; j < _columnCount; j++)
                {
                    if (_maze[i, j] == 2)
                    {
                        _maze[i,j]= Int32.MaxValue;
                        _end.Row = i;
                        _end.Column = j;
                    }
                    else if(_maze[i,j]==1)
                    {
                        _maze[i, j] = 0;
                        _start.Row = i;
                        _start.Column = j;
                    }
                    else if(_maze[i,j]==0)
                    {
                        _maze[i,j]= Int32.MaxValue;
                    }
                }
                _currentCells.Add(_start);
            }
        }

        public bool Solve()
        {
            while (_currentCells.Count != 0&&_maze[_end.Row,_end.Column]==Int32.MaxValue)
            {
                var newCells = new List<Cell>();
                foreach (var cell in _currentCells)
                {
                    var value = _maze[cell.Row, cell.Column]+1;//Value to write in nearby cells
                    if (cell.Row != 0 && _maze[cell.Row - 1, cell.Column] != -1 && _maze[cell.Row-1, cell.Column]>value)
                    {
                        newCells.Add(new Cell {Row = cell.Row-1,Column = cell.Column});
                        _maze[cell.Row - 1, cell.Column] = value;
                    }
                    if (cell.Row != _rowCount-1 && _maze[cell.Row + 1, cell.Column] != -1 && _maze[cell.Row + 1, cell.Column] > value)
                    {
                        newCells.Add(new Cell { Row = cell.Row + 1, Column = cell.Column });
                        _maze[cell.Row + 1, cell.Column] = value;
                    }
                    if (cell.Column != 0 && _maze[cell.Row, cell.Column-1] != -1 && _maze[cell.Row, cell.Column-1] > value)
                    {
                        newCells.Add(new Cell { Row = cell.Row, Column = cell.Column -1});
                        _maze[cell.Row, cell.Column-1] = value;
                    }
                    if (cell.Column != _columnCount-1 && _maze[cell.Row, cell.Column + 1] != -1 && _maze[cell.Row, cell.Column + 1] > value)
                    {
                        newCells.Add(new Cell { Row = cell.Row, Column = cell.Column + 1 });
                        _maze[cell.Row, cell.Column + 1] = value;
                    }
                }
                _currentCells = newCells;
            }
            _pathFound= _maze[_end.Row, _end.Column] != Int32.MaxValue;
            return _pathFound;
        }

        public IEnumerable<Cell> BackTrace()
        {
            var next = new Cell {Row=_end.Row,Column = _end.Column};
            var min = Int32.MaxValue;
            var path = new List<Cell>();
            while (!next.Equals(_start))
            {
                path.Add(next);
                var current = new Cell();
                if (next.Row != 0 && _maze[next.Row - 1, next.Column] != -1 && _maze[next.Row - 1, next.Column] < min)
                {
                    current.Row = next.Row-1;
                    current.Column = next.Column;
                    min = _maze[current.Row, current.Column];
                }
                if (next.Row != _rowCount-1 && _maze[next.Row + 1, next.Column] != -1 && _maze[next.Row + 1, next.Column] < min)
                {
                    current.Row = next.Row + 1;
                    current.Column = next.Column;
                    min = _maze[current.Row, current.Column];
                }
                if (next.Column != 0 && _maze[next.Row, next.Column-1] != -1 && _maze[next.Row, next.Column-1] < min)
                {
                    current.Row = next.Row;
                    current.Column = next.Column-1;
                    min = _maze[current.Row, current.Column];
                }
                if (next.Column != _columnCount-1 && _maze[next.Row, next.Column + 1] != -1 && _maze[next.Row, next.Column + 1] < min)
                {
                    current.Row = next.Row;
                    current.Column = next.Column + 1;
                    min = _maze[current.Row, current.Column];
                }
                next = current;
            }
            return path;
        }
    }
}
