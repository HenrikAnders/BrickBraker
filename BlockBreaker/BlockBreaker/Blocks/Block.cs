using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace BlockBreaker.Blocks
{
    public abstract class Block : UIElement
    {
        // private Fields
        internal DrawingVisual _block;
        internal const double _height = 20;
        internal const double _width = 50;


        // Properties
        public bool IsDistroyed = false;
        public abstract int Value { get; }
        private double _Left { get { return (double)this.GetValue(Canvas.LeftProperty); } }
        private double _Right { get { return _Left + _width; } }
        private double _Top { get { return (double)this.GetValue(Canvas.TopProperty); } }
        private double _Bottom { get { return _Top + _height; } }


        // overrides from Visual class
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _block;
        }


        public bool BlockHit(Ball ball)
        {
            if (ball.Top > this._Bottom)
                return false;
            else if (ball.Bottom < this._Top)
                return false;
            else if (ball.Left > this._Right)
                return false;
            else if (ball.Right < this._Left)
                return false;

            this.IsDistroyed = true;
            return true;
        }


        public string HitLocation(Ball ball)
        {
            string _location = "noHit";
            
            //treffer von unten
            if (ball.Bottom > this._Bottom)
            {
                _location = "bottom";
            }

            //treffer von oben
            else if (ball.Top < this._Top )
            {
                _location = "top";
            }

            //treffer von links
            else if (ball.Left < this._Left)
            {
                 _location = "left";
            }

            //treffer von rechts
            else if (ball.Right > this._Right)
            {
                _location = "right";
            }

            return _location;
        }

        //public virtual void Distroy(MainWindow window);

    }
}
