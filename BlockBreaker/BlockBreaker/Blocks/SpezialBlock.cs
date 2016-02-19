using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace BlockBreaker.Blocks
{
    class SpezialBlock : Block
    {
        public override int Value { get { return 500; } }
        int livePoints;
        // Constructor
        public SpezialBlock(int livePoints)
        {
            this.livePoints = livePoints;
            _block = new DrawingVisual();
            DrawingContext context = _block.RenderOpen();
            Rect rectangle = new Rect(0, 0, _width, _height);
            context.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 1.0), rectangle);
            context.Close();
        }
        public int GetLivePoints() {
            return livePoints;
        }
        public void setLivePoints(int livePoints) {
            this.livePoints = livePoints;
        }

        public override bool hit()
        {
            livePoints--;
            return livePoints<=0;
        }

        //public override void Distroy(MainWindow window)
        //{
        //    var bonus = new Bonus(this, window);
        //    window.MainCanvas.Children.Add(bonus);
        //}
    }
}
