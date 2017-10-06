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
using System.Windows.Shapes;

namespace ColorBalls
{
    /// <summary>
    /// Логика взаимодействия для GameOverSplash.xaml
    /// </summary>
    public partial class GameOverSplash : Window
    {
        public delegate void Starting();
        public event Starting OnTryAgain;

        public GameOverSplash()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnTryAgain?.Invoke();
            this.Close();
        }
    }
}
