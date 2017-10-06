using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ColorBalls.Properties;

namespace ColorBalls.Decription
{
    public class Ball
    {
        private int width = 60;
        private int thick = 5;

        private int row;
        private int column;
        private ImageBrush brush;
        
        public bool Checked = false;
        public string Color;

        public delegate void Clicking(int x, int y);
        public event Clicking MouseMove;
        public event Clicking Click;
        public delegate void Deleting();
        public event Deleting Delete;

        private Ellipse ball;
        public Ellipse GetEllipse => ball;

        private Storyboard storyboard = new Storyboard();
        private Storyboard fallStoryboard = new Storyboard();

        private DoubleAnimation opacityAnim = new DoubleAnimation
        {
            From = 0.5,
            To = 1,
            Duration = TimeSpan.FromSeconds(1),
            RepeatBehavior = RepeatBehavior.Forever,
            AutoReverse = true
        };

        private ThicknessAnimation fallAnim = new ThicknessAnimation
        {
            From = new Thickness(5, 0, 5, 10),
            To = new Thickness(5, 5, 5, 5),
            Duration = TimeSpan.FromSeconds(2),
            RepeatBehavior = RepeatBehavior.Forever,
            EasingFunction = new BounceEase()
        };

        public Ball(int x, int y, ImageBrush color)
        {
            column = x;
            row = y;
            brush = color;

            ball = new Ellipse()
            {
                Name = "ball" + x + y,
                Fill = color,
                Stretch = Stretch.UniformToFill,
                Width = width,
                //Margin = new Thickness(thick + 70*column, thick + 70*row, thick, thick),
                Margin = new Thickness(thick),
                Opacity = 1,
                ToolTip = new ToolTip()
                {
                    Content = new StackPanel() { Children = { new TextBlock() { Text = row + " " + column } } }
                },
                
            };

            ball.MouseEnter += (sender, args) =>{MouseMove?.Invoke(column, row);};
            ball.MouseDown += (sender, args) =>
            {
                if(args.RightButton == MouseButtonState.Pressed) Delete?.Invoke();
                else Click?.Invoke(column, row);
            };

            //ball.SetValue(Canvas.LeftProperty, (thick + 70 * column)/700);
            //ball.SetValue(Canvas.TopProperty, thick + 70 * row);

            Canvas.SetLeft(ball, thick + 70 * column);
            Canvas.SetTop(ball, thick + 70 * row);

            ball.SetValue(Grid.RowProperty, row);
            ball.SetValue(Grid.ColumnProperty, column);
        }

        public void SetCord(int x, int y)
        {
            DoubleAnimation MoveTopAnimation = new DoubleAnimation()
            {
                From = thick + 70 * row,
                To = thick + 70 * y,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CircleEase()
            };

            DoubleAnimation MoveLeftAnimation = new DoubleAnimation()
            {
                From = thick + 70 * column,
                To = thick + 70 * x,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CircleEase()
            };

            MoveTopAnimation.Completed += (sender, args) => { Canvas.SetTop(ball, thick + 70 * y); };
            MoveLeftAnimation.Completed += (sender, args) => { Canvas.SetLeft(ball, thick + 70 * x); };

            ball.BeginAnimation(Canvas.TopProperty, MoveTopAnimation);
            ball.BeginAnimation(Canvas.LeftProperty, MoveLeftAnimation);

            row = y;
            column = x;
        }

        public void Check()
        {
            ball.Effect = new DropShadowEffect()
            {
                Color = Colors.AliceBlue,
                ShadowDepth = 0,
                Opacity = 0.7,
                BlurRadius = 15
            };
            Checked = true;
        }

        public void Uncheck()
        {
            ball.Effect = null;
            Checked = false;
        }

        public void StartAnimation()
        {
            Storyboard.SetTarget(opacityAnim, ball);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(UIElement.OpacityProperty));
           
            storyboard.Children.Add(opacityAnim);
            storyboard.Begin();
        }

        public void StopAnimation()
        {
            storyboard.Stop();
        }

        public void CanDelete()
        {
            ball.Fill = new ImageBrush(new BitmapImage(new Uri("Resources/" + Color + "D.png", UriKind.Relative)));

            Storyboard.SetTarget(fallAnim, ball);
            Storyboard.SetTargetProperty(fallAnim, new PropertyPath(Ellipse.MarginProperty));

            fallStoryboard.Children.Add(fallAnim);
            fallStoryboard.Begin();
        }

        public void StopCanDelete()
        {
            fallStoryboard.Stop();
            ball.Fill = brush;
        }
    }


}