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
        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImage;

        public MainWindow()
        {
            InitializeComponent();
            gridImage = SetupGrid();
        }

        // this will add control required by the grid
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < cols; c++) {
                    
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
        
    }
}
