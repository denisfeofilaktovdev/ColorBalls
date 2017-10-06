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
using System.Windows.Threading;
using ColorBalls.Decription;

namespace ColorBalls
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO: Стартовое окно
        //TODO: Окно проиграша
        //TODO: Кнопка рестарт
        //TODO: Кнопка начать игру
        //TODO: Подсчет рекордов с файлом о достижениях
        //TODO: Уровни сложности


        private Dock CurrentDock;
        private DispatcherTimer timer = new DispatcherTimer();
        private int count = 0;

        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += TimerOnTick;
            timer.Interval = TimeSpan.FromMilliseconds(500);

            StartGame();
        }

        private void CurrentDockOnEndGame()
        {
            //NextBallsCanvas.Children.Clear();
            //MainPanel.Children.Clear();

            timer.Stop();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (count < 10) CurrentDock.LoadNewBall(count++);
            else
            {
                count = 0;
                CurrentDock.SetNewLine();
            }
        }

        private void OnRedraw(Ball[,] balls, Ball[] nextBalls)
        {
            NextBallsCanvas.Children.Clear();
            MainPanel.Children.Clear();

            for (int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
                if (balls[x, y] != null) MainPanel.Children.Add(balls[x, y].GetEllipse);

            for (int i = 0; i < 10; i++)
                if (nextBalls[i] != null) NextBallsCanvas.Children.Add(nextBalls[i].GetEllipse);
        }

        private void StartGame()
        {
            CurrentDock = new Dock();
            CurrentDock.Redraw += OnRedraw;
            CurrentDock.EndGame += CurrentDockOnEndGame;
            CurrentDock.SendScore += CurrentDockOnSendScore;
            CurrentDock.DoRedraw();
            timer.Start();
        }

        private void CurrentDockOnSendScore(int score)
        {
            lblScore.Content = (Convert.ToInt32(lblScore.Content) + score).ToString();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Ellipse).Opacity = 0.5;
            StartGame();
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Ellipse).Opacity = 1;
        }
    }
}
