using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0},
            {Direction.Right, 90},
            {Direction.Down, 180},
            {Direction.Left, 270}
        };
        private GameState _gameState;
        private bool _gameRunning;
        
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
            GameGrid.Width = GameGrid.Height * (_cols / (double)_rows);
            for (int r = 0; r < _rows; r++) {
                for (int c = 0; c < _cols; c++) {
                    // initially set empty images
                    Image image = new Image { 
                        Source = Images.Empty,
                        // insures only head image is roated not its upper-left image in the grid
                        RenderTransformOrigin = new Point(0.5,0.5)
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private async Task RunGame()
        {
            await ShowCountDown();
            Draw();
            Overlay.Visibility = Visibility.Hidden; // Hide the visibility of overlay
            await GameLoop();
            await ShowGameOver();
            _gameState = new GameState(_rows, _cols);
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /*
             * By default, PreviewKeyDown then KeyDown event is called in a row, in case of event
             * But by making e.Handled to true, it tells the eventHandler that it is already been handled
             *  and no need to check further events in row 
             */
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!_gameRunning)
            {
                _gameRunning = true;
                await RunGame();
                /*
                 * Wait for game to get over, then make _gameRunning to false 
                 */
                _gameRunning = false;
            }
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
                await Task.Delay(100);
                _gameState.Move();
                Draw();
            }
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
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
                    //insures only head image gets rotated
                    _gridImage[r,c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawSnakeHead()
        {
            Position headPos = _gameState.HeadPosition();
            Image image = _gridImage[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[_gameState.Dir];
            // Rotate the image by the amount of degree
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(_gameState.SnakePositions());

            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                _gridImage[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; --i)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}
