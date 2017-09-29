using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorBalls.Decription;

namespace ColorBalls
{
    public class Game
    {
        public Dock dock;

        public Game()
        {
            dock = new Dock();
        }

        public void Start()
        {
            dock.DoRedraw();
        }

        public void Resume()
        {
            dock.ReSet();
        }
    }
}
