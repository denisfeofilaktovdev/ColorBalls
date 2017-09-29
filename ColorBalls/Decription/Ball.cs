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
        public int Row => row;
        private int column;
        public int Column => column;
        private ImageBrush brush;

        public int index;
        public bool Checked = false;
        public string Color;

        public delegate void Clicking(int x, int y);
        public event Clicking Click;
        public delegate void Deleting();
        public event Deleting Delete;

        private Ellipse ball;
        public Ellipse GetEllipse => ball;

        private Storyboard storyboard = new Storyboard();
        private Storyboard fallStoryboard = new Storyboard();

        private DoubleAnimation opacityAnim = new DoubleAnimation
        {
            From = 0.3,
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

        public Ball()
        {
            
        }

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
                Margin = new Thickness(thick),
                Opacity = 1
            };

            ball.MouseEnter += (sender, args) =>{(sender as Ellipse).Opacity = 0.5;};
            ball.MouseLeave += (sender, args) => {(sender as Ellipse).Opacity = 1;};
            ball.MouseDown += (sender, args) =>
            {
                if(args.RightButton == MouseButtonState.Pressed) Delete?.Invoke();
                else Click?.Invoke(column, row);
            };

            ball.SetValue(Grid.RowProperty, row);
            ball.SetValue(Grid.ColumnProperty, column);
        }

        public void Check()
        {
            ball.Stroke = new SolidColorBrush(Colors.Black);
            Checked = true;
        }

        public void Uncheck()
        {
            ball.Stroke = new SolidColorBrush(Colors.Transparent);
            Checked = false;
        }

        public void StartAnimation()
        {
            Storyboard.SetTarget(opacityAnim, ball);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(UIElement.OpacityProperty));
           
            storyboard.Children.Add(opacityAnim);
            //ball.BeginAnimation(Ellipse.OpacityProperty, opacityAnim);

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
            //ball.BeginAnimation(Ellipse.MarginProperty, fallAnim);
        }

        public void StopCanDelete()
        {
            fallStoryboard.Stop();
            ball.Fill = brush;
        }
    }


}