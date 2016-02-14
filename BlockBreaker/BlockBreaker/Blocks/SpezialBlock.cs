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
        
        // Constructor
        public SpezialBlock()
        {
            _block = new DrawingVisual();
            DrawingContext context = _block.RenderOpen();
            Rect rectangle = new Rect(0, 0, _width, _height);
            context.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 1.0), rectangle);
            context.Close();
        }


        //public override void Distroy(MainWindow window)
        //{
        //    var bonus = new Bonus(this, window);
        //    window.MainCanvas.Children.Add(bonus);
        //}
    }
}
