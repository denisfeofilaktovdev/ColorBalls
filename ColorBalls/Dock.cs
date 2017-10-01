using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorBalls.Decription;

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

        public delegate void Redrawing(Ball[,] balls);
        public event Redrawing Redraw;

        private static List<Ball> CanCheckedBalls = new List<Ball>();

        private static Ball[,] _dock = new Ball[10,7];
        private int[] DeletedAmount = new int[10];

        private string _currentColor;
        
        /// <summary>
        /// 
        /// </summary>
        public Dock()
        {
            Random random = new Random();

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 7; y++)
                {
                    int selectColor = random.Next(0, 3);

                    _dock[x, y] = new Ball(x, y, colors[selectColor]);

                    if (selectColor == 0) _dock[x, y].Color = "purple";
                    else if (selectColor == 1) _dock[x, y].Color = "green";
                    else _dock[x, y].Color = "yellow";

                    _dock[x, y].MouseMove += OnMove;
                    _dock[x, y].Click += OnClick;
                    _dock[x, y].Delete += OnDelete;
                }

            for (int i = 0; i < 10; i++)
            {
                DeletedAmount[i] = 7;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoRedraw()
        {
            Redraw?.Invoke(_dock);
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

        private void OnColumnRemove()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnDelete()
        {
            if (CanCheckedBalls.Count >= 3)
            {
                for (int x = 0; x < 10; x++)
                for (int y = 0; y < 7; y++)
                    if (_dock[x,y] != null && _dock[x, y].Checked){ _dock[x, y] = null; DeletedAmount[x]--; }

                ClearCanCheckedBalls();
                ReSet();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void OnMove(int x, int y)
        {
            if (!_dock[x, y].Checked)
            {
                ClearCanCheckedBalls();

                _currentColor = _dock[x, y].Color;

                SetCanCheckBalls(x, y, true, true);

                foreach (Ball ball in CanCheckedBalls)
                {
                    ball.Check();
                    ball.StopCanDelete();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void OnClick(int x, int y)
        {
            if (CanCheckedBalls.Count >= 3)
                foreach (Ball ball in CanCheckedBalls)
                    ball.CanDelete();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearCanCheckedBalls()
        {
            foreach (Ball ball in CanCheckedBalls)
            {
                ball.StopCanDelete();
                ball.Uncheck();
            }
            CanCheckedBalls.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="line"></param>
        /// <param name="column"></param>
        private void SetCanCheckBalls(int x, int y, bool line, bool column)
        {
            if(x - 1 >= 0 && _dock[x - 1,y]?.Color == _currentColor && !_dock[x - 1, y].Checked && !CanCheckedBalls.Contains(_dock[x - 1, y]))
            { CanCheckedBalls.Add(_dock[x - 1, y]); SetCanCheckBalls(x - 1, y, true, true);}

            if (x + 1 < 10 && _dock[x + 1, y]?.Color == _currentColor && !_dock[x + 1, y].Checked && !CanCheckedBalls.Contains(_dock[x + 1, y]))
            { CanCheckedBalls.Add(_dock[x + 1, y]); SetCanCheckBalls(x + 1, y, true, true); }

            if (y - 1 >= 0 && _dock[x, y - 1]?.Color == _currentColor && !_dock[x, y - 1].Checked && !CanCheckedBalls.Contains(_dock[x, y - 1]))
            { CanCheckedBalls.Add(_dock[x, y - 1]); SetCanCheckBalls(x, y - 1, true, true); }

            if (y + 1 < 7 && _dock[x, y + 1]?.Color == _currentColor && !_dock[x, y + 1].Checked && !CanCheckedBalls.Contains(_dock[x, y + 1]))
            { CanCheckedBalls.Add(_dock[x, y + 1]); SetCanCheckBalls(x, y + 1, true, true); }
        }

        //TODO: Написать Функцию перемещения элемента
        /// <summary>
        /// 
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
    }
}
