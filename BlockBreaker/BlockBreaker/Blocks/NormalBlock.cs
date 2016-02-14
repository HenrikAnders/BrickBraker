using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace BlockBreaker.Blocks
{
    class NormalBlock : Block
    {
        public override int Value { get { return 100; } }


        // Constructor
        public NormalBlock()
        {
            _block = new DrawingVisual();
            DrawingContext context = _block.RenderOpen();
            Rect rectangle = new Rect(0, 0, _width, _height);
            context.DrawRectangle(Brushes.Blue, new Pen(Brushes.Black, 1.0), rectangle);
            context.Close();
        }
    }
}
