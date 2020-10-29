using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sudoku
{
    public partial class MainPage : ContentPage
    {
        protected View _gridView;
        protected View _buttonView;
        protected View _btnView;
        protected WinnerView _winnerView;
        protected GridData _gridData;
        protected Label _winner;

        public async void OnDisplayAlertButtonClicked()
        {
            await DisplayAlert("Alert", "This is an alert.", "OK");
        }

        public MainPage()
        {
            InitializeComponent();

            _gridData = new GridData(1);
            _gridData.RemoveGridValue(48);
            

            Label mainTitleView = new Label
            {
                // Add options for the title view
                Text = "Sudoku",
                FontSize = 12,
            };

      

            ContentView contentGrid = new ContentView
            {
          
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.FillAndExpand,
               
        
            };
            _gridView = new GridView(_gridData);
            contentGrid.Content = _gridView;

            ContentView keyPadContainer = new ContentView
            {
         
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
               
            };
            _buttonView = new ButtonView((GridView)_gridView,_gridData);
            keyPadContainer.Content = _buttonView;

            StackLayout stack = new StackLayout
            {
                Children =
               {
                   mainTitleView,
                   contentGrid,
                   keyPadContainer
               }
           };
            Content = stack;
           
        }
        
     

    }
}
