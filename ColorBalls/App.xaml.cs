using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ColorBalls
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public App()
        {
            InitializeComponent();

            Splash splash = new Splash();
            splash.Show();
            int counter = 0;

            MainWindow window = new MainWindow();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) =>
            {
                counter++;
                if (counter == 3)
                {
                    splash.Close();
                    Run(window);
                    (sender as DispatcherTimer).Stop();
                }
            };

            timer.Start();
        }

        [STAThread]
        public static void Main()
        {
            App app = new App();
        }
    }
}
