using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Sudoku
{

    public class ButtonView:ContentView
    {
        protected GridView _gridView;
        protected GridData _gridData;
        protected Label _label;

        public ButtonView(GridView gridView, GridData gridData)
        {
            _gridView = gridView;
            _gridData = gridData;
            Page p = new Page();

            var grid = new Grid
            {
                BackgroundColor = Color.Black,
                ColumnSpacing = 0.5,
                RowSpacing = 0.5
            };
            for (int i = 0; i < 3; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < 5; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }


            int numT = 1;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 1; j < 4; j++)
                {
                    Button btnNum = new Button
                    {
                        Text = numT.ToString(),
                        BorderWidth = 0,
                    };
                    btnNum.Clicked +=  (object sender, EventArgs e) =>
                    {
                         _gridData.SetCellValue(_gridData.GetSelectedCell(), int.Parse(btnNum.Text));
                        if (_gridData.gridCheck())
                        {
                          if(_gridData.sudokuChecker())
                            {

                                winnerPopUp();

                            }
                        }

                        _gridView.Update();
                    };
                    numT++;
                    grid.Children.Add(btnNum, j, i);
                }
            }

            Button btnClear = new Button
            {
                BorderWidth = 0,
                Text = "C"
            };
            btnClear.Clicked += (object sender, EventArgs e) =>
            {

                _gridData.clear();
                _gridView.Update();
            };

            Button btnSolve = new Button
            {
                BorderWidth = 0,
                Text = "S"
            };
            btnSolve.Clicked += (object sender, EventArgs e) =>
            {

                _gridData.clear();
                _gridData.solve(_gridView);
            };
            Button btnCheck = new Button
            {
                BorderWidth = 0,
                Text = "U"
            };
            Button btnQuit = new Button
            {
                BorderWidth = 0,
                Text = "Q"
            };

            btnQuit.Clicked += (object sender, EventArgs e) =>
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };

            grid.Children.Add(btnClear, 0, 0);
            Grid.SetRowSpan(btnClear, 3);
            grid.Children.Add(btnSolve, 4, 0);
            grid.Children.Add(btnCheck, 4, 1);
            grid.Children.Add(btnQuit, 4, 2);

            this.Content = grid;
        }
        async void winnerPopUp()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("congratulations", "congratulations", "try again?", "quit");

            Debug.WriteLine("Answer: " + answer);
            if(answer == false)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                (Application.Current).MainPage = new NavigationPage(new MainPage());
            }
          // return true;
        }
     
    
    }
}
