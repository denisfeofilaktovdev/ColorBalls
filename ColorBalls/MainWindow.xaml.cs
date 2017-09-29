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
using ColorBalls.Decription;

namespace ColorBalls
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game CurrentGame;

        public MainWindow()
        {
            InitializeComponent();

            CurrentGame = new Game();
            CurrentGame.dock.Redraw += OnRedraw;
            CurrentGame.Start();
        }

        public void OnRedraw(Ball[,] balls)
        {
            GridDock.Children.Clear();
            for (int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
            {
                if (balls[x,y] != null) GridDock.Children.Add(balls[x, y].GetEllipse);
            }
        }
    }
}
