using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //number of rows and cols
        private readonly int _rows = 15, _cols = 15;
        private readonly Image[,] _gridImage;
        private readonly Dictionary<GridValue, ImageSource> _gridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Food, Images.Food },
            { GridValue.Snake, Images.Body }
        };
        private GameState _gameState;
        
        
        public MainWindow()
        {
            InitializeComponent();
            _gridImage = SetupGrid();
            _gameState = new GameState(_rows, _cols);
        }

        // this will add control required by the grid
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[_rows, _cols];
            GameGrid.Rows = _rows;
            GameGrid.Columns = _cols;

            for (int r = 0; r < _rows; r++) {
                for (int c = 0; c < _cols; c++) {
                    
                    // initially set empty images
                    Image image = new Image { 
                        Source = Images.Empty
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);

                }
            }

            return images;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
            await GameLoop();
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // if game is over then perform nothing on key press
            if (_gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    _gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    _gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    _gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    _gameState.ChangeDirection(Direction.Down);
                    break;
            }
        }

        private async Task GameLoop()
        {
            while (!_gameState.GameOver)
            {
                await Task.Delay(500);
                _gameState.Move();
                Draw();
            }
        }

        private void Draw()
        {
            DrawGrid();
            ScoreText.Text = $"SCORE {_gameState.Score}";
        }

        private void DrawGrid()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    GridValue gridValue = _gameState.Grid[r, c];
                    _gridImage[r, c].Source = _gridValToImage[gridValue];
                }
            }
        }
    }
}
