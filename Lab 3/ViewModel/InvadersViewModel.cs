using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab_3.Model;
using Lab_3.View;
using Lab_3.Model.Eventargs;
using System.Windows.Input;

namespace Lab_3.ViewModel
{
    class InvadersViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<FrameworkElement>
        _sprites = new ObservableCollection<FrameworkElement>();
        public INotifyCollectionChanged Sprites { get { return _sprites; } }
        public bool GameOver { get { return _model.GameOver; } }
        private readonly ObservableCollection<object> _lives =
        new ObservableCollection<object>();
        public INotifyCollectionChanged Lives { get { return _lives; } }
        public bool Paused { get; set; }
        private bool _lastPaused = true;
        public static double Scale { get; private set; }
        public int Score { get; private set; }

        public Size PlayAreaSize
        {
            set
            {
                Scale = value.Width / 405;
                _model.UpdateAllShipsAndStars();
                RecreateScanLines();
            }
        }


        private readonly InvadersModel _model = new InvadersModel();
        public InvadersViewModel()
        {
            Scale = 1;
            _model.ShipChanged += ModelShipChangedEventHandler;
            _model.ShotMoved += ModelShotMovedEventHandler;
            _model.StarChanged += ModelStarChangedEventHandler;
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += TimerTickEventHandler;
            _model.EndGame();
        }

        

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private FrameworkElement _playerControl = null;
        private bool _playerFlashing = false;
        private readonly Dictionary<Invader, FrameworkElement> _invaders =
        new Dictionary<Invader, FrameworkElement>();
        private readonly Dictionary<FrameworkElement, DateTime> _shotInvaders =
        new Dictionary<FrameworkElement, DateTime>();
        private readonly Dictionary<Shot, FrameworkElement> _shots =
        new Dictionary<Shot, FrameworkElement>();
        private readonly Dictionary<Point, FrameworkElement> _stars =
        new Dictionary<Point, FrameworkElement>();
        private readonly List<FrameworkElement> _scanLines =
        new List<FrameworkElement>();
        public void StartGame()
        {
            Paused = false;
            foreach (var invader in _invaders.Values) _sprites.Remove(invader);
            foreach (var shot in _shots.Values) _sprites.Remove(shot);
            _model.StartGame();
            OnPropertyChanged("GameOver");
            _timer.Start();
        }

        private void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        private void RecreateScanLines()
        {
            foreach (FrameworkElement scanLine in _scanLines)
                if (_sprites.Contains(scanLine))
                    _sprites.Remove(scanLine);
            _scanLines.Clear();
            for (int y = 0; y < 300; y += 2)
            {
                FrameworkElement scanLine = InvadersHelper.ScanLineFactory(y, 400, Scale);
                _scanLines.Add(scanLine);
                _sprites.Add(scanLine);
            }
        }

        private void TimerTickEventHandler(object sender, object e)
        {
            if(_lastPaused != Paused)
            {
                OnPropertyChanged()
            }

            if (!Paused)
            {

            }
        }
        private void ModelShipChangedEventHandler(object sender, ShipChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ModelShotMovedEventHandler(object sender, ShotMovedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ModelStarChangedEventHandler(object sender, StarChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private DateTime? _leftAction = null;
        private DateTime? _rightAction = null;

        internal void KeyDown(Key key)
        {
            if (key == Key.Space)
            {
                _model.FireShot();
            }
            if (key == Key.Left)
            {
                _leftAction = DateTime.Now;
            }
            if (key == Key.Right)
            {
                _rightAction = DateTime.Now;
            }
        }

        internal void KeyUp(Key key)
        {
            if (key == Key.Left)
            {
                _leftAction = null;
            }
            if (key == Key.Right)
            {
                _rightAction = null;
            }
        }

    }
}
