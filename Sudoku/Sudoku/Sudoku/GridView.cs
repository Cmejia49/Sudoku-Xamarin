using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Sudoku
{
   public class GridView:ContentView
    {
        protected GridData _gridData;
        protected Label[][] _labels;
        protected GridCell _currentSelectedCell = null;
        protected Label _winner;
        public GridView(GridData gridData)
        {
            Debug.Assert(gridData.IsSquare(), "SudokuGridView only support square grids (for now) !");

            _gridData = gridData;

            _labels = new Label[gridData.W][];
            for (int i = 0; i < gridData.W; i++)
                _labels[i] = new Label[gridData.H];
       

            var baseGrid = new Grid
            {
                BackgroundColor = Color.White
            };
            baseGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            baseGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            baseGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            baseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            baseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            baseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var grid = new Grid
            {
                BackgroundColor = Color.Black,
                ColumnSpacing = 1,
                RowSpacing = 1
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            for (int m = 0;m<9;m++)
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Star) });

            for (int m = 0; m <9; m++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });


            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        
            for ( int i = 0; i < gridData.H; i++)
            {
                for( int j = 0; j < gridData.W; j++)
                {
                   GridCell curCell = _gridData.GetGridCell(i, j);
            
                    var cell = new ContentView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(0),
                        BackgroundColor = GetCellBaseColor(curCell),
                    };

                  cell.Content = _labels[i][j] = new Label

                    {

                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = GetCellText(_gridData.GetCell(i, j)),
                        
                      // BackgroundColor = GetCellBaseColor(i,j)

                  };
                    TapGestureRecognizer tap = new TapGestureRecognizer();
                    EventHandler myFunc = (object sender, EventArgs e) => {
                
                    OnCellClick(curCell);
                    };
                    tap.Tapped += myFunc;
                    cell.GestureRecognizers.Add(tap);

                    grid.Children.Add(cell, j+1, i+1);
                }
            }
            baseGrid.Children.Add(grid, 1,1);
            this.Content = baseGrid;

        }
        // Action on when you click on a cell
        public void OnCellClick(GridCell curCell)
        {
            _currentSelectedCell = curCell;
            if (curCell.IsEditable)
            {
              
                Update();
                curCell.IsSelected = true;
                _currentSelectedCell = null;
            }
            else
            {
                Update();
                _currentSelectedCell = null;
         
            }
        }
        public Color GetCellBaseColor(GridCell curCell)
        {
            int iBloc = curCell.GetCoordX / _gridData.BlocH;
            int jBloc = curCell.GetCoordY / _gridData.BlocW;
            return (((iBloc + jBloc) % 2) == 0) ? Color.White : Color.LightGray;
        }


     
        public String GetCellText(int gridValue)
        {
            return (gridValue != 0) ? gridValue.ToString() : "";
        }
        public void Update()
        {
            for (int i = 0; i < _gridData.H; i++)
            {
                for (int j = 0; j < _gridData.W; j++)
                {
                    // get cell from sudokuGridData
                    GridCell curCell = _gridData.GetGridCell(i, j);
                    curCell.IsSelected = false;
                    // get label
                    var label = _labels[i][j];

                    // set label text
                    label.Text = GetCellText(curCell.Value);
                     

                    // get gridView cell
                    ContentView cell = label.Parent as ContentView;

                    cell.BackgroundColor = GetCellBaseColor(curCell);

                    if (_currentSelectedCell == curCell)
                    {
                     
                        cell.BackgroundColor = Color.Green;

                        if(_currentSelectedCell == curCell && _currentSelectedCell.IsEditable)
                        {
                            label.TextColor = Color.Black;
                        }
                    }           

                }
            }
        }

    }
}
