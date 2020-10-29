using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sudoku
{

    public class GridData
    {
        //Randomize need to be initialized once
        protected Random rdm = new Random();

        // members
       
        protected int _blockW;
        protected int _blockH;
        protected int _width;
        protected int _height;

        protected int _nSymbols;
        private GridCell[][] _cells;
       
  
        protected int[] _coordGrid;

        // getters
        public int BlocW { get => _blockW; }
        public int BlocH { get => _blockH; }
        public int W { get => _width; }
        public int H { get => _height; }

        public bool IsSquare()
        {
            return W == H;
        }

        // All value from specific Line in tab int[]
        public int[] GetLineGrid(int line)
        {
            int[] tabLine = new int[_width];
            for (int i = 0; i < _width; i++)
            {
                tabLine[i] = _cells[line][i].Value;
            }

            return tabLine;
        }
        // All value from specific Col int tab int[]
        public int[] GetColGrid(int col)
        {
            int[] tabCol = new int[_height];
            for (int i = 0; i < _height; i++)
            {
                tabCol[i] = _cells[i][col].Value;
            }
            return tabCol;
        }
        // Get specific cell
        public GridCell GetGridCell(int line, int column)
        {
          
            return _cells[line][column];
        }

        public int GetCell(int line, int column)
        {
             return GetGridCell(line, column).Value;
        }

        // constructor
        public GridData(int difficulty, int blockW = 3, int blockH = 3)
        {
            _blockW = blockW;
            _blockH = blockH;
            _nSymbols = _blockW * _blockH;
            _width = _height = _nSymbols;

            // Allocate cells
            _cells = new GridCell[_width][];
            for (int i = 0; i < _width; i++)
            {
                _cells[i] = new GridCell[_height];
            }

            GenerateRandomGrid(difficulty);
            for (int i = 0; i <= 20; i++)
            {
                RandomizeRows(10);
                RandomizeColumns(10);
            }
            
           
       
        }

        public void GenerateRandomGrid(int difficulty)
        {
            // Generate the first row of block
            int lineStart = 1;
            for (int j = 0; j < _blockH; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    _cells[i][j] = new GridCell(((lineStart + i - 1) % _nSymbols) + 1, i, j);
                }
                lineStart += _blockW;
            }

            // Generate next rows
            for (int j = _blockH; j < _height; j++)
            {
                int i = 0;
                while (i < _width)
                {
                    int iBlock = i;
                    for (int k = 0; k < _blockW; k++)
                    {
                        int column = (((i + 1) - iBlock) % _blockW) + iBlock;

                        _cells[i][j] = new GridCell(_cells[column][j - _blockH].Value, i, j);
                        i++;
                    }
                }
            }
            // Log();
        }

        // Randomize helper
        public void RandomizeHelper(int blockNb, out int offsetA, out int offsetB)
        {
            int rBlock = rdm.Next(0, blockNb);

            // Do Randomize While Column A is not like Column B 
            int rA, rB;
            rA = rdm.Next(1, (blockNb + 1));
            do
            {
                rB = rdm.Next(1, (blockNb + 1));
            } while (rA == rB);
            offsetA = ((rBlock * _blockW) + rA) - 1;
            offsetB = ((rBlock * _blockW) + rB) - 1;
        }

        // Randomize columns
        public void RandomizeColumns(int nbTimes)
        {
            for (int nb = 0; nb <= nbTimes; nb++)
            {
                // Get offset to be modified via RandomizeHelper method
                RandomizeHelper(_blockW, out int offsetA, out int offsetB);

                // Do value permutation on each SudokuGridCell concern value
                int tempCellValue;
                for (int i = 0; i < _nSymbols; i++)
                {
                    tempCellValue = _cells[offsetB][i].Value;
                    _cells[offsetB][i].Value = _cells[offsetA][i].Value;
                    _cells[offsetA][i].Value = tempCellValue;
                }
            }
        }

        //Randomize rows
        public void RandomizeRows(int nbTimes)
        {
            for (int nb = 0; nb <= nbTimes; nb++)
            {
                // Get offset to be modified via RandomizeHelper method
                RandomizeHelper(_blockH, out int offsetA, out int offsetB);
                // Do Permutation
                for (int i = 0; i < _width; i++)
                {
                    int tempValue;
                    tempValue = _cells[i][offsetB].Value;
                    _cells[i][offsetB].Value = _cells[i][offsetA].Value;
                    _cells[i][offsetA].Value = tempValue;
                }
            }
        }

        // Hide nb in param values from grid 
        public void RemoveGridValue(int nbRemovedCell)
        {
            for (int i = 0; i < nbRemovedCell; i++)
            {
                bool isValueInGrid = false;
                int numLine = 0;
                int numCol = 0;
                // While there is no value in cell
                while (!isValueInGrid)
                {
                    // We generate two numbers beetween 1 and the number of Symbols
                    numLine = rdm.Next(0, _nSymbols);
                    numCol = rdm.Next(0, _nSymbols);
                    // If cell is null we loop else we put the new value 0 in cell
                    if (_cells[numLine][numCol].Value != 0)
                    {
                        _cells[numLine][numCol].Value = 0;
                        _cells[numLine][numCol].IsEditable = true;
                        isValueInGrid = true;
                    }
                }
            }
        }


     
        public void SetCellValue(GridCell c, int v)
        {
            if (ValueIsValid(c, v))
            {
                c.Value = v;
            }
        }

        // Is value in cell's possible values list
        public bool ValueIsValid(GridCell c, int v)
        {
            if (c != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Get the selected cell
        public GridCell GetSelectedCell()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_cells[x][y].IsSelected)
                    {
                        return _cells[x][y];
                    }
                }
            }
            return null;
        }

        bool blockTestHelper(int x, int y)
        {
            int x0 = (x / _blockW) * _blockW;
            int y0 = (y / _blockH) * _blockH;


            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (m != i || n != j)
                            {
                                if (_cells[x0 + m][y0 + n].Value == _cells[x0 + i][y0 + j].Value)
                                {
                                    Debug.Write("Error in Block");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool sudokuChecker()
        {
            //col test
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                { 
                    for (int otherCol = x + 1; otherCol < _nSymbols; otherCol++)
                    {
                        if (_cells[y][x].Value == _cells[y][otherCol].Value)  
                        {
                            Debug.WriteLine("error in Col");
                            return false;
                        }
                    }
                }
            }


            //row test
            for (int x = 0; x < _height; x++)
            {
                for (int y = 0; y < _width; y++)
                {
                
                    for (int otherRow = x + 1; otherRow < _nSymbols; otherRow++)
                    {
                        if (_cells[x][y].Value == _cells[otherRow][y].Value)
                        {
                            Debug.WriteLine("error in ROw");
                            return false;
                        }
                    }
                }
            }

            //block test
            for (int i = 0; i < _height; i += _blockH )
            {
                for (int j = 0; j < _width; j += _blockW)
                {
                    blockTestHelper(i, j);
                }
            }

            return true;
        }


        public bool gridCheck()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (_cells[i][j].Value == 0)
                    {
                        return false;
                    }

                }
            }
            return true;

        }


        public void clear()
        {

            for(int y = 0; y < _height; y++)
            {
                for(int x = 0; x < _width; x++)
                {
                    if(_cells[y][x].IsEditable == true)
                    {
                        _cells[y][x].Value = 0;
                    }
                }
            }
        }
        //Sudoku Solver
        public bool sudokuSolverHelper(int row , int col , int value)
        {
            int _row = row;
            int _col = col;
            int _value = value;
            //col test
            for(int i = 0; i < 9; i++)
            {
                if(_cells[_row][i].Value == _value)
                {
                    return false;
                }
            }

            // row test
            for(int j = 0; j < 9; j++)
            {
                if(_cells[j][_col].Value == _value)
                {
                    return false;
                }
            }

            //block test
            int x0 = (_col / _blockW) * 3;
            int y0 = (_row / _blockH) * 3;

            for(int i = 0;i<3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(_cells[y0 + i][x0 + j].Value == _value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void solve(GridView c)
        {
            
            for (int y = 0; y <_height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_cells[y][x].Value == 0)
                    {
                        for (int value = 1; value < 10; value++)
                        {
                            if (sudokuSolverHelper(y, x, value))
                            {
                                _cells[y][x].Value = value;
                                solve(c);
                                _cells[y][x].Value = 0;
                            }
                            
                        }
                        return;
                    }
                }
            }
            c.Update();
        }
    }
}
