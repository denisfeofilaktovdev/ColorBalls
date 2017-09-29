using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using ColorBalls.Decription;
using ColorBalls.Properties;

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

        private static List<Ball> CheckedBalls = new List<Ball>();
        private static List<Ball> CanCheckedBalls = new List<Ball>();

        private static Ball[,] _dock = new Ball[10,7];
        public Ball[,] balls => _dock;

        private string currentColor;
        private int currentColumn;
        private int currentRow;
        private bool isLine;
        
        public Dock()
        {
            Random random = new Random();

            for(int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
            {
                int selectColor = random.Next(0, 3);
                
                _dock[x,y] = new Ball(x, y, colors[selectColor]);

                if (selectColor == 0) _dock[x, y].Color = "purple";
                else if (selectColor == 1) _dock[x, y].Color = "green";
                else _dock[x, y].Color = "yellow";

                _dock[x,y].Click += OnClick;
                _dock[x,y].Delete += OnDelete;
            }
        }

        public void DoRedraw()
        {
            Redraw?.Invoke(_dock);
        }

        public void ReSet()
        {
            Random random = new Random();

            for (int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
            {
                if (_dock[x, y] == null)
                {
                    int selectColor = random.Next(0, 3);

                    _dock[x, y] = new Ball(x, y, colors[selectColor]);

                    if (selectColor == 0) _dock[x, y].Color = "purple";
                    else if (selectColor == 1) _dock[x, y].Color = "green";
                    else _dock[x, y].Color = "yellow";

                    _dock[x, y].Click += OnClick;
                    _dock[x, y].Delete += OnDelete;
                }
            }
            DoRedraw();
        }

        private ImageBrush GetRandomColor()
        {
            Random random = new Random();

            return colors[random.Next(0, 3)];
        }

        private void OnDelete()
        {
            for (int x = 0; x < 10; x++)
            for (int y = 0; y < 7; y++)
            {
                if (_dock[x, y].Checked) _dock[x, y] = null;
            }
            CheckedBalls.Clear();
            CanCheckedBalls.Clear();
            ReSet();
        }

        private void OnClick(int x, int y)
        {
            if (CheckedBalls.Count == 0) GetFirstBall(x, y);
            else if (CheckedBalls.Count == 1)
            {
                ClearCanCheckedBalls();
                if (balls[x, y].Color == currentColor)
                {
                    if (CheckedBalls[0].Column == x && Math.Abs(CheckedBalls[0].Row - y) == 1)
                    {
                        isLine = false;
                        CheckedBalls.Add(_dock[x, y]);
                        _dock[x, y].Check();
                        SetCanCheckBalls(x, y, isLine, !isLine);
                    }
                    else if (CheckedBalls[0].Row == y && Math.Abs(CheckedBalls[0].Column - x) == 1)
                    {
                        isLine = true;
                        CheckedBalls.Add(_dock[x, y]);
                        _dock[x, y].Check();
                        SetCanCheckBalls(x, y, isLine, !isLine);
                    }
                    else if (CheckedBalls[0].Column == x && Math.Abs(CheckedBalls[0].Row) == y)
                    {
                        _dock[x,y].Uncheck();
                        CanCheckedBalls.Clear();
                        CheckedBalls.Clear();
                    }
                }
            }
            else
            {
                int count = CheckedBalls.Count;
                for (var index = 0; index < CheckedBalls.Count; index++)
                {
                    Ball ball = CheckedBalls[index];
                    if (x == ball.Column && y == ball.Row)
                    {
                        ClearCanCheckedBalls();
                        _dock[x, y].Uncheck();
                        CheckedBalls.RemoveAt(index);
                        if(CheckedBalls.Count == 1) SetCanCheckBalls(CheckedBalls[0].Column, CheckedBalls[0].Row, true, true);
                        else SetCanCheckBalls(x, y, isLine, !isLine);
                        break;
                    }
                }
                if(count == CheckedBalls.Count)
                    foreach (Ball ball in CanCheckedBalls)
                        if (x == ball.Column && y == ball.Row)
                        {
                            ClearCanCheckedBalls();
                            CheckedBalls.Add(_dock[x, y]);
                            _dock[x, y].Check();
                            SetCanCheckBalls(x, y, isLine, !isLine);
                            break;
                        }
            }

            foreach (Ball ball in CanCheckedBalls){ ball.StartAnimation(); ball.StopCanDelete(); }
            foreach (Ball ball in CheckedBalls)
            {
                if(CheckedBalls.Count >= 3) ball.CanDelete();
                else ball.StopCanDelete();
                ball.StopAnimation();
            }
        }

        public void Remove(int x, int y)
        {
            _dock[x, y] = null;
        }

        private void GetFirstBall(int x, int y)
        {
            currentColor = _dock[x, y].Color;

            _dock[x,y].Check();
            SetCanCheckBalls(x, y, true, true);

            CheckedBalls.Add(_dock[x, y]);
        }

        private void ClearCanCheckedBalls()
        {
            foreach (Ball ball in CanCheckedBalls)
            {
                ball.StopAnimation();
            }
            CanCheckedBalls.Clear();
        }

        private void SetCanCheckBalls(int x, int y, bool line, bool column)
        {
            int i = x + 1, j = y;

            if (line)
            {
                while (i > 0 && _dock[--i, j].Color == currentColor) if(!_dock[i,j].Checked) CanCheckedBalls.Add(_dock[i, j]);
                i = x;
                while (i < 9 && _dock[++i, j].Color == currentColor) if(!_dock[i, j].Checked) CanCheckedBalls.Add(_dock[i, j]);
            }
            j = y + 1;
            if (column)
            {
                i = x;
                while (j > 0 && _dock[i, --j].Color == currentColor) if(!_dock[i, j].Checked) CanCheckedBalls.Add(_dock[i, j]);
                j = y;
                while (j < 6 && _dock[i, ++j].Color == currentColor) if(!_dock[i, j].Checked) CanCheckedBalls.Add(_dock[i, j]);
            }
        }
    }
}
