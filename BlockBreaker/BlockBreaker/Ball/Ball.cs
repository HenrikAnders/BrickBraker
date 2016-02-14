using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;
using System.ComponentModel; 

namespace BlockBreaker
{
    public class Ball : INotifyPropertyChanged
    {
        //Attribute:

        public const int SPEED = 3;

        private int _radius = 6;
        public int Radius   
        {
            get { return _radius; }
        }

        private int _speedX = SPEED;
        public int SpeedX  
        {
            get { return _speedX; }
            set { _speedX = value; }
        }

        private int _speedY = -SPEED;
        public int SpeedY   
        {
            get { return _speedY; }
            set { _speedY = value; }
        }

        public Vector Direction
        {
            get
            {
                return new Vector(_speedX / _speedX, _speedY / _speedY);
            }
        }

        private Point _position;
        public Point Position  
        {
            get { return _position; }
            set
            {
                _position = value;

            }
        }

        public double Top { get { return _position.Y - _radius; } }
        public double Bottom { get { return _position.Y + _radius; } }
        public double Left { get { return _position.X - _radius; } }
        public double Right { get { return _position.X + _radius; } }

        //Alle public Attribute werden mit DataContext im MainWindow Konstruktor an die xaml übergeben


        //Methoden:

        public void MoveBall()
        {
            _position.X += _speedX;
            _position.Y += _speedY;
            OnPropertyChanged("Position");
        }
        public void Set(Point p)
        {
            _position.X = p.X;
            _position.Y = p.Y;
            SpeedX = SPEED;
            SpeedY = -SPEED;
            OnPropertyChanged("Position");
        }



        //EventHändler, hier für die Bewegung des balls
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
