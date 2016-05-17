using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;
using BlockBreaker.Blocks;
using Microsoft.VisualStudio.QualityTools.UnitTestFramework;

namespace BlockBreaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        //  Attribute:
        Ball ball = new Ball();
        bool InGame = false;    // Indikator für aktives Spiel
        public DispatcherTimer timer;
        int level;
        int _blocks;
        int points = 0;//12.02 Henrik Added
        int livePoints = 3; //12.02 Henrik Added 
        bool loose = false;
        bool win = false;
        List<Blocks.Block> blocks;
        Test test = new Test();

        //die Höhenposition des paddle von oben gemessen (Y-Koordinate)
        double PaddleTop
        {
            get { return Canvas.GetTop(paddle); }
        }

        // die Breitenposition des paddle von links gemessen (X-Koordinate)
        double PaddleLeft
        {
            get { return Canvas.GetLeft(paddle); }
        }

        TimeSpan _time;

        public TimeSpan Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            PathBall.DataContext = ball;    //hier wird das ball-Objekt an die Oberfläche Übergeben
            SetBall();  //ball wird gesetzt
            AddBlocks();
            lCanvas.Visibility = Visibility.Hidden; //12.02 Henrik Added canvas label, only show by loosing
            lLivePoints.Content = "♥Balls: " + livePoints;
            lPlayCount.Content = "Points: 0";
        }
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
                //infoPanel.Level = level;
            }
        }
        public int Blocks
        {
            get { return _blocks; }
            set
            {
                _blocks = value;
                OnPropertyChanged("Blocks");
                //infoPanel.Level = level;
            }
        }


        // Spiel Intervall
        void timer_Tick(object sender, EventArgs e)
        {
            //Time += TimeSpan.FromMilliseconds(5); // die Zeitanzeige

            ball.MoveBall();
            CheckWallCollisions();
            CheckPaddleCollisions();
            CheckBlockCollisions();
        }


        //setzt den Ball
        //Breitenposition des paddle links + Hälfte der Breite des paddle , Höhenposition - die Größe des Balls
        private void SetBall()
        {
            ball.Set(new Point(PaddleLeft + paddle.Width / 2, PaddleTop - ball.Radius)); //ein Punkt( Y-Koordinate , X-Koordinate )
            //ball.SpeedX = 0;
            //ball.SpeedY = 0;
        }
        //startet ein neues Spiel
        private void StartGame()
        {
            lLivePoints.Content = "♥Balls: " + livePoints;
            if (InGame == false)
            {
                InGame = true;
                this.Cursor = Cursors.None; //blendet die Maus im Spielbereich aus
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                test.testInitLive(livePoints);
            }
            else return;
        }


        //beendet das laufende Spiel
        private void StopGame()
        {
            if (timer != null)
                timer.Stop();
            timer = null;
            InGame = false;
            this.Cursor = Cursors.Arrow;
            SetBall();
        }


        private void AddBlocks()
        {
            blocks = new List<Blocks.Block>();
            Random rnd = new Random();

            for (int i = 0; i < 2 + Level; i++)
            {
                for (int j = 0; j < 9; j++)
                {

                    Blocks.Block b;
                    if (rnd.NextDouble() > 0.8)
                    {
                        b = new SpezialBlock(2);
                    }
                    else
                    {
                        b = new NormalBlock();
                    }

                    double left = j * 50 + 25;
                    double top = i * 20 + 50;
                    b.SetValue(Canvas.LeftProperty, left);
                    b.SetValue(Canvas.TopProperty, top);
                    blocks.Add(b);
                    MainCanvas.Children.Add(b);
                    Blocks = blocks.Count;
                }
            }
            test.testList(blocks);
        }
        //Start-Button
        private void button1_Click(object sender, RoutedEventArgs e)    //Start Button
        {
            StartGame();
        }
        //Stop-Button
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            StopGame();
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
            bRestart.Visibility = Visibility.Hidden;

        }
        //weitere Buttons stehen noch aus:

        //"Pause Button"

        //Mausbewegung
        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this);  //Position der Maus 

            if (p.X < paddle.Width / 2)
            {
                p.X = paddle.Width / 2;
            }
            if (p.X > MainCanvas.ActualWidth - paddle.Width / 2)
            {
                p.X = MainCanvas.ActualWidth - paddle.Width / 2;
            }

            Canvas.SetLeft(paddle, p.X - paddle.Width / 2); //setzt das paddle, am Mittelpunkt, an die Breitenposition der Maus

            if (!InGame)    //falls das Spiel aus ist, setzt er den Ball neu
            {
                ball.Set(new Point(p.X, PaddleTop - ball.Radius));
            }
        }


        //wenn die Maus den Spielbereich verlässt, aber noch weiter sich im Fenster befindet, wir Sie wieder mit "Arrow" angezeigt
        private void GameCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }


        //mit der linken Maus-Button wir das Spiel gestartet
        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!InGame && !loose && !win)
                StartGame();
        }

        //Tastenfunktionen wärend eines Spiels
        private void Window_KeyDown(object sender, KeyEventArgs e)  //Trigger auf Tastatur
        {
            if (InGame) // läuft das Spiel?
            {
                switch (e.Key)
                {
                    case Key.Left:  //bewegt paddle nach links
                        if (Canvas.GetLeft(paddle) < 20)
                        {
                            Canvas.SetLeft(paddle, PaddleLeft - Canvas.GetLeft(paddle));
                        }
                        else
                        {
                            Canvas.SetLeft(paddle, PaddleLeft - 20);
                        }
                        break;

                    case Key.Right:  //bewegt paddle nach rechts
                        if (MainCanvas.ActualWidth - 20 < Canvas.GetLeft(paddle) + paddle.Width)
                        {
                            Canvas.SetLeft(paddle, MainCanvas.ActualWidth - paddle.Width);
                        }
                        else
                        {
                            Canvas.SetLeft(paddle, PaddleLeft + 20); ;
                        }

                        break;
                    case Key.Escape:
                        StopGame();    //bricht spiel ab
                        break;


                }
            }
        }
        // Eventhändler:
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        //Prüft ob eine Kollision mit einer Wand vorliegt
        private void CheckWallCollisions()
        {
            if (ball.Top > MainCanvas.ActualHeight)//bottom "wall"
            {
                StopGame();
                //12.02 Henrik Added live count
                if (livePoints <= 0)
                {
                    lCanvas.Visibility = Visibility; //Visible true
                    lCanvas.Content = "!You loose!";
                    loose = true;
                    StopGame();
                    button1.Visibility = Visibility.Hidden;
                    button2.Visibility = Visibility.Hidden;
                }
                livePoints--;
            }
            else if (ball.Right > MainCanvas.ActualWidth)
                ball.SpeedX *= -1;
            else if (ball.Left < 0)
                ball.SpeedX *= -1;
            else if (ball.Top < 0)
                ball.SpeedY *= -1;
        }


        //Prüft ob eine Kollision mit dem Paddel vorliegt
        private void CheckPaddleCollisions()
        {
            if (ball.Direction.Y < 0)   //wenn der ball nach oben fliegt, brich die methode ab
                return;

            if (ball.Bottom <= PaddleTop || ball.Top > PaddleTop + paddle.Height) //bricht die Methode ab, wenn der ball-Boden überhalb oder die Ball-Decke unterhalb des paddle liegt
                return;
            // abfrage müsste noch etwas angepasst werden, da sie den fall nicht einbezieht, wenn der ball 0-9 überhalb liegt, dann fliegt der ball beim nächsten loop in den paddle rein 

            //Positionen:
            double paddleCentre = PaddleLeft + paddle.Width / 2; //  paddle Hälfte
            double paddleRight = PaddleLeft + paddle.Width; // paddle ganz rechts
            double paddleRightEnd = paddleRight - paddle.Width / 4; // paddle 3/4 von links
            double paddleLeftEnd = PaddleLeft + paddle.Width / 4;   // paddle 1/4 von links

            // Treffer linkes Flanke
            if (PaddleLeft <= ball.Position.X && ball.Position.X < paddleLeftEnd)
            {
                ball.SpeedX = -6;
                ball.SpeedY *= -1;
            }

            // Treffer linkes Front
            else if (paddleLeftEnd <= ball.Position.X && ball.Position.X < paddleCentre)
            {
                ball.SpeedX = ball.SpeedX < 0 ? -Ball.SPEED : Ball.SPEED;
                ball.SpeedY *= -1;
            }

            //Treffer Mitte, Ball von links kommend
            else if (ball.Position.X == paddleCentre && ball.SpeedX > 0)
            {
                ball.SpeedX = ball.SpeedX < 0 ? -Ball.SPEED : Ball.SPEED;
                ball.SpeedY *= -1;
            }

            //Treffer Mitte, Ball von rechts kommend
            else if (ball.Position.X == paddleCentre && ball.SpeedX < 0)
            {
                ball.SpeedX = ball.SpeedX > 0 ? Ball.SPEED : -Ball.SPEED;
                ball.SpeedY *= -1;
            }

            // Treffer rechte Front
            else if (paddleCentre < ball.Position.X && ball.Position.X <= paddleRightEnd)
            {
                ball.SpeedX = ball.SpeedX > 0 ? Ball.SPEED : -Ball.SPEED;
                ball.SpeedY *= -1;
            }

            // Treffer rechte Flanke
            else if (paddleRightEnd < ball.Position.X && ball.Position.X <= paddleRight)
            {
                ball.SpeedX = 6;
                ball.SpeedY *= -1;
            }
        }

        private int bottomCount;
        private int topCount;
        private int leftCount;
        private int rightCount;

        ////Prüft ob eine Kollision mit einem Block vorliegt
        private void CheckBlockCollisions()
        {

            List<Blocks.Block> blockhits = new List<Blocks.Block>();

            foreach (Blocks.Block b in blocks)
            //for (int i = 0; i < anzahlBlocks;i++)
            {
                //Blocks.Block b = blocks[i];
                if (b.BlockHit(ball))
                {
                    switch (b.HitLocation(ball))
                    {
                        case "bottom":
                            bottomCount++;
                            break;

                        case "top":
                            topCount++;
                            break;

                        case "left":
                            leftCount++;
                            break;

                        case "right":
                            rightCount++;
                            break;

                        case "noHit":
                            break;
                    }
                    blockhits.Add(b);
                }


                if (blocks.Count == blockhits.Count)
                {  //Henrik 12.02 stop game, if all blocks are destroyed
                    lCanvas.Visibility = Visibility;
                    bRestart.Visibility = Visibility;
                    lCanvas.Content = "congratulation, you win :)";
                    button1.Visibility = Visibility.Hidden;
                    button2.Visibility = Visibility.Hidden;
                    StopGame();
                    win = true;
                }
            }

            foreach (Blocks.Block b in blockhits)
            {

                if (b.IsDistroyed)
                {
                    lPlayCount.Content = "Points: " + ++points; //Henrik 12.02 set PlayPoints into the label
                    if (b.hit())
                    {
                        MainCanvas.Children.Remove(b);
                        blocks.Remove(b);
                    }
                    else
                    {
                        b.IsDistroyed = false;
                    }
                }
            }

            if (bottomCount > 0)
            {
                ball.SpeedY *= -1;
                bottomCount = 0;
            }
            else if (topCount > 0)
            {
                ball.SpeedY *= -1;
                topCount = 0;
            }
            else if (leftCount > 0)
            {
                ball.SpeedX *= -1;
                leftCount = 0;
            }
            else if (rightCount > 0)
            {
                ball.SpeedX *= -1;
                rightCount = 0;
            }
        }
        private void RestartGame()
        { //12.02 Henrik Added
            bRestart.Visibility = Visibility;
            button1.Visibility = Visibility;
            button2.Visibility = Visibility;
            livePoints = 3;
            points = 0;
            lCanvas.Visibility = Visibility.Hidden;
            win = false;
            loose = false;
            AddBlocks();
            test.testInitLive(livePoints);
        }
    }
}
