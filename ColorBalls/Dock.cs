using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ColorBalls.Decription;
using Timer = System.Timers.Timer;

namespace ColorBalls
{
    public class Dock
    {
        private static ImageBrush purple = new ImageBrush(new BitmapImage(new Uri("Resources/purple.png", UriKind.Relative)));
        private static ImageBrush green = new ImageBrush(new BitmapImage(new Uri("Resources/green.png", UriKind.Relative)));
        private static ImageBrush yellow = new ImageBrush(new BitmapImage(new Uri("Resources/yellow.png", UriKind.Relative)));

        private ImageBrush[] colors =
        {
            purple, green, yellow
        };

        public delegate void Redrawing(Ball[,] balls, Ball[] nextBalls);
        public event Redrawing Redraw;

        public delegate void EndOfGame();
        public event EndOfGame EndGame;

        public delegate void Score(int score);
        public event Score SendScore;
        
        private List<Ball> CheckedBalls = new List<Ball>();

        private Ball[,] _dock = new Ball[10,7];
        private Ball[] _dockNext = new Ball[10];
        private int[] DeletedAmount = new int[10];

        private string _currentColor;
        private bool Stoped;
        Random random = new Random();

        /// <summary>
        /// Конструктор игрового поля
        /// </summary>
        public Dock()
        {
            for (int x = 0; x < 10; x++)
                for (int y = 3; y < 7; y++)
                    _dock[x, y] = GetRandomBall(x, y);

            CreateNewLine();
        }

        public void SetNewLine()
        {
            // Перенос всех элементов поля на 1 ед. вверх
            for (int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
            {
                if (y >= 1 && _dock[x, y] != null)
                {
                    _dock[x, y - 1] = _dock[x, y];
                    _dock[x, y] = null;
                    _dock[x, y - 1]?.SetCord(x, y - 1);
                }
            }

            // Перенос линии в поле
            for (int i = 0; i < 10; i++)
            {
                _dock[i, 6] = _dockNext[i];
                _dock[i, 6].SetCord(i, 7);
                _dockNext[i] = null;

                _dock[i, 6].MouseMove += OnMove;
                _dock[i, 6].Click += OnClick;
                _dock[i, 6].Delete += OnDelete;

                _dock[i, 6].SetCord(i, 6);
            }

            CreateNewLine();
            DoRedraw();
        }

        public void LoadNewBall(int index)
        {
            DoubleAnimation OpacityAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1)
            };

            _dockNext[index].GetEllipse.BeginAnimation(Ellipse.OpacityProperty, OpacityAnimation);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoRedraw()
        {
            Redraw?.Invoke(_dock, _dockNext);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReSet()
        {
            for (int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
            {
                if (_dock[x, y] == null)
                {
                    int counter = 0;
                    for(int i = 1; y - i >= 0; i++)
                        if (_dock[x, y - i] != null)
                        {
                            _dock[x, y - counter] = _dock[x, y - i];
                            _dock[x, y - i] = null;

                            _dock[x, y - counter].SetCord(x, y - counter);
                            counter++;
                        }
                }
            }
            CheckForEmptyColumn();
            DoRedraw();
        }

        private void StopGame()
        {
            EndGame?.Invoke();
        }

        private void CreateNewLine()
        {
            for (int i = 0; i < 10; i++)
            {
                if (DeletedAmount[i] == 6)
                {
                    Stoped = true;
                    StopGame();
                    break;
                }
                int selectColor = random.Next(0, 3);

                _dockNext[i] = new Ball(i, 0, colors[selectColor]);

                if (selectColor == 0) _dockNext[i].Color = "purple";
                else if (selectColor == 1) _dockNext[i].Color = "green";
                else _dockNext[i].Color = "yellow";

                _dockNext[i].GetEllipse.Opacity = 0;
            }
            SetDeletedAmount();
        }


        private void SetDeletedAmount()
        {
            for (int x = 0; x < 10; x++)
            {
                DeletedAmount[x] = 0;
                for (int y = 0; y < 7; y++)
                {
                    if (_dock[x, y] != null) DeletedAmount[x]++;
                }
            }
        }
        /// <summary>
        /// Обработчик события клика левой кнопкой мыши. Удаление групы шаров.
        /// </summary>
        private void OnDelete()
        {
            if (CheckedBalls.Count >= 3)
            {
                for (int x = 0; x < 10; x++)
                for (int y = 0; y < 7; y++)
                    if (_dock[x,y] != null && _dock[x, y].Checked){ _dock[x, y] = null; DeletedAmount[x]--; }

                SendScore?.Invoke(CheckedBalls.Count);
                ClearCheckedBalls();
                ReSet();
            }
        }

        /// <summary>
        /// Обработчик события движения над шаром.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void OnMove(int x, int y)
        {
            if (!_dock[x, y].Checked)
            {
                ClearCheckedBalls();

                _currentColor = _dock[x, y].Color;

                SetCheckBalls(x, y);

                foreach (Ball ball in CheckedBalls)
                {
                    ball.Check();
                    ball.StopCanDelete();
                }
            }
        }

        /// <summary>
        /// Обработчик события клика над шаром.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void OnClick(int x, int y)
        {
            if (CheckedBalls.Count >= 3)
                foreach (Ball ball in CheckedBalls)
                    ball.CanDelete();
        }

        /// <summary>
        /// Очистка массива выбраных шаров.
        /// </summary>
        private void ClearCheckedBalls()
        {
            foreach (Ball ball in CheckedBalls)
            {
                ball.StopCanDelete();
                ball.Uncheck();
            }
            CheckedBalls.Clear();
        }

        /// <summary>
        /// Метод определяющий возможность удаления групы шаров.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetCheckBalls(int x, int y)
        {
            if(x - 1 >= 0 && _dock[x - 1,y]?.Color == _currentColor && !_dock[x - 1, y].Checked && !CheckedBalls.Contains(_dock[x - 1, y]))
            { CheckedBalls.Add(_dock[x - 1, y]); SetCheckBalls(x - 1, y);}

            if (x + 1 < 10 && _dock[x + 1, y]?.Color == _currentColor && !_dock[x + 1, y].Checked && !CheckedBalls.Contains(_dock[x + 1, y]))
            { CheckedBalls.Add(_dock[x + 1, y]); SetCheckBalls(x + 1, y); }

            if (y - 1 >= 0 && _dock[x, y - 1]?.Color == _currentColor && !_dock[x, y - 1].Checked && !CheckedBalls.Contains(_dock[x, y - 1]))
            { CheckedBalls.Add(_dock[x, y - 1]); SetCheckBalls(x, y - 1); }

            if (y + 1 < 7 && _dock[x, y + 1]?.Color == _currentColor && !_dock[x, y + 1].Checked && !CheckedBalls.Contains(_dock[x, y + 1]))
            { CheckedBalls.Add(_dock[x, y + 1]); SetCheckBalls(x, y + 1); }
        }

        /// <summary>
        /// Метод для проверки поля на пустые колонки.
        /// </summary>
        private void CheckForEmptyColumn()
        {
            for (int i = 4; i > 0; i--)
            {
                if (DeletedAmount[i] == 0)
                {
                    int counter = 0;
                    while (i - counter > 0 && DeletedAmount[i - ++counter] == 0) ;

                    for (int j = 0; j < 7; j++)
                    {
                        if (_dock[i - counter, j] != null)
                        {
                            _dock[i, j] = _dock[i - counter, j];
                            DeletedAmount[i - counter]--;
                            DeletedAmount[i]++;
                            _dock[i - counter, j] = null;
                            _dock[i, j]?.SetCord(i, j);
                        }
                    }
                }
                if (DeletedAmount[9 - i] == 0)
                {
                    int counter = 9;
                    while (counter - i < 9 && DeletedAmount[++counter - i] == 0) ;

                    for (int j = 0; j < 7; j++)
                    {
                        if (_dock[counter - i, j] != null)
                        {
                            _dock[9 - i, j] = _dock[counter - i, j];
                            DeletedAmount[counter - i]--;
                            DeletedAmount[9 - i]++;
                            _dock[counter - i, j] = null;
                            _dock[9 - i, j]?.SetCord(9 - i, j);
                        }
                    }
                }
            }
        }

        private Ball GetRandomBall(int x, int y)
        {
            int selectColor = random.Next(0, 3);
            Ball ball = new Ball(x, y, colors[selectColor]);

            if (selectColor == 0) ball.Color = "purple";
            else if (selectColor == 1) ball.Color = "green";
            else ball.Color = "yellow";

            ball.MouseMove += OnMove;
            ball.Click += OnClick;
            ball.Delete += OnDelete;

            return ball;
        }
    }
}
