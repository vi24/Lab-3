using Lab_3.Model.Eventargs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_3.Model
{
    class InvadersModel
    {
        public readonly static Size PlayAreaSize = new Size(400, 300);
        public const int MaximumPlayerShots = 3;
        public const int InitialStarCount = 50;
        private readonly Random _random = new Random();
        public int Score { get; private set; }
        public int Wave { get; private set; }
        public int Lives { get; private set; }
        public bool GameOver { get; private set; }
        private DateTime? _playerDied = null;
        public bool PlayerDying { get { return _playerDied.HasValue; } }

        private Player _player;
        private readonly List<Invader> _invaders = new List<Invader>();
        private readonly List<Shot> _playerShots = new List<Shot>();
        private readonly List<Shot> _invaderShots = new List<Shot>();
        private readonly List<Point> _stars = new List<Point>();
        private Direction _invaderDirection = Direction.Left;
        private bool _justMovedDown = false;
        private DateTime _lastUpdated = DateTime.MinValue;

        public event EventHandler<ShipChangedEventArgs> ShipChanged;
        public event EventHandler<ShotMovedEventArgs> ShotMoved;
        public event EventHandler<StarChangedEventArgs> StarChanged;

        public InvadersModel()
        {
            EndGame();
        }
        public void EndGame()
        {
            GameOver = true;
        }
        // You'll need to finish the rest of the InvadersModel class

        public void StartGame()
        {
            GameOver = false;
            foreach(Invader invader in _invaders)
            {
                OnShipChanged(invader, true);
            }
            _invaders.Clear();
            foreach(Shot shot in _playerShots)
            {
                OnShotMoved(shot, true);
            }
            _playerShots.Clear();
            foreach(Shot shot in _invaderShots)
            {
                OnShotMoved(shot, true);
            }
            _invaderShots.Clear();
            foreach (Point point in _stars)
            {
                OnStarChanged(point, true);
            }
            _stars.Clear();

            for(int i = 0; i <= InitialStarCount; i++)
            {
                AddStar();
            }
        }

        public void FireShot()
        {
            if(_playerShots.Count < MaximumPlayerShots)
            {
                Shot shot = new Shot(_player.Location, Direction.Up);
                _playerShots.Add(shot);
                OnShotMoved(shot, false);
            }

        }

        public void MovePlayer(Direction direction)
        {
            if(_playerDied == null)
            {
                _player.Move(direction);
                OnShipChanged(_player, false);
            }
        }

        public void Twinkle()
        {
            int coin = _random.Next(1);
            bool tooManyStars = (_stars.Count + 1 > (double)InitialStarCount * 1.5);
            bool tooFewStars = (_stars.Count - 1 < _stars.Count - (double)InitialStarCount * 0.15);

            switch (coin)
            {
                case 0:
                    if (!tooManyStars)
                    {
                        AddStar();
                    }
                    break;
                case 1:
                    if (!tooFewStars)
                    {
                        RemoveStar();
                    }
                    break;
            }
        }

        public void Update(bool paused)
        {
            if (GameOver) return;

            if (paused) return;

            if(_playerDied == null)
            {
                foreach(Invader invader in _invaders)
                {
                    invader.Move(_invaderDirection);
                    OnShipChanged(invader, false);
                }

                foreach(Shot shot in _playerShots)
                {
                    UpdateShot(shot);
                }

                foreach(Shot shot in _invaderShots)
                {
                    UpdateShot(shot);
                }
            }


        }

        public void UpdateAllShipsAndStars()
        {
            foreach (Shot shot in _playerShots)
                OnShotMoved(shot, false);
            foreach (Invader ship in _invaders)
                OnShipChanged(ship, false);
            OnShipChanged(_player, false);
            foreach (Point star in _stars)
                OnStarChanged(star, false);
        }

        public void OnShipChanged(Ship shipUpdated, bool killed)
        {
            if(ShipChanged != null)
            {
                ShipChanged(this, new ShipChangedEventArgs(shipUpdated, true));
            }
        }

        public void OnShotMoved(Shot shot, bool disappeared)
        {
            if(ShotMoved != null)
            {
                ShotMoved(this, new ShotMovedEventArgs(shot, disappeared));
            }
        }

        public void OnStarChanged(Point point, bool disappeared)
        {
            if(StartChanged != null)
            {
                StartChanged(this, new StarChangedEventArgs(point, disappeared));
            }
        }

        private void AddStar()
        {
            Point point = new Point(_random.Next((int)PlayAreaSize.Width), _random.Next((int)PlayAreaSize.Height));
            if (!_stars.Contains(point))
            {
                _stars.Add(point);
                OnStarChanged(point, false);
            }
        }

        private void RemoveStar()
        {
            Point star = _stars[_random.Next(_stars.Count)];
            if (_stars.Contains(star))
            {
                _stars.Remove(star);
                OnStarChanged(star, true);
            }

        }

        private void UpdateShot(Shot shot)
        {
            shot.Move();
            if (shot.Location.X > PlayAreaSize.Width || shot.Location.Y > PlayAreaSize.Height)
            {
                _playerShots.Remove(shot);
                OnShotMoved(shot, true);
            }
            else
            {
                OnShotMoved(shot, false);
            }
        }

        private void NextWave()
        {
            Wave++;
            _invaders.Clear();

        }

        private void CheckForPlayerCollisions()
        {
            foreach(Shot shot in _invaderShots)
            {
                if (_player.Area.Contains(shot.Location))
                {
                    Lives--;
                    if(Lives > 0)
                    {
                        _playerDied = DateTime.Now;
                        OnShipChanged(_player, true);
                    }
                    else
                    {
                        EndGame();
                    }
                }
            }
        }

        private void CheckForInvaderCollisions()
        {
            foreach(Shot shot in _playerShots)
            {
                var deadInvader = from invader in _invaders
                                  where invader.Area.Contains(shot.Location)
                                  select invader;

                if(deadInvader is Invader)
                {
                    _invaders.Remove((Invader)deadInvader);
                    _playerShots.Remove(shot);
                }
            }
            if (ReachedBottom())
            {
                EndGame();
            }
        }

        private void MoveInvaders()
        {
            double millisecondsBetweenMovements = Math.Min(10 - Wave, 1) * (2 * _invaders.Count());
            TimeSpan timePassed = DateTime.Now - _lastUpdated;
            if (timePassed > TimeSpan.FromMilliseconds(millisecondsBetweenMovements))
            {

                var invadersOnLeftBoundary = from invader in _invaders
                                             where invader.Area.Left < Invader.HORIZONTAL_MOVE_INTERVAL
                                             select invader;
                var invadersOnRightBoundary = from invader in _invaders
                                              where invader.Area.Right < Invader.HORIZONTAL_MOVE_INTERVAL
                                              select invader;

                if (!_justMovedDown)
                {
                    if (invadersOnLeftBoundary.Count() > 0)
                    {
                        foreach (Invader invader in _invaders)
                        {
                            invader.Move(Direction.Down);
                            OnShipChanged(invader, false);
                        }
                        _invaderDirection = Direction.Right;
                    }
                    else if (invadersOnRightBoundary.Count() > 0)
                    {
                        foreach (Invader invader in _invaders)
                        {
                            invader.Move(Direction.Down);
                            OnShipChanged(invader, false);
                        }
                        _invaderDirection = Direction.Left;
                    }
                    _justMovedDown = true;
                }
                else
                {
                    _justMovedDown = false;
                    foreach (Invader invader in _invaders)
                    {
                        invader.Move(_invaderDirection);
                        OnShipChanged(invader, false);
                    }
                }
            }
        }
        private void ReturnFire()
        {
            if (_invaderShots.Count >= Wave + 1 ||_random.Next(10) < 10 - Wave) return;

            var invaderGroups = from invader in _invaders
                                group invader by invader.Location.X 
                                into invaderGroup
                                orderby invaderGroup.Key descending
                                select invaderGroup;

            var randomGroup = invaderGroups.ToList()[_random.Next(invaderGroups.ToList().Count())];

            var bottomInvader = randomGroup.Last();

            Point shotLocation = new Point(bottomInvader.Area.X + bottomInvader.Area.Width / 2, bottomInvader.Area.Bottom + 2);
            Shot invaderShot = new Shot(shotLocation, Direction.Down);
            _invaderShots.Add(invaderShot);
            OnShotMoved(invaderShot, false);

        }

        private bool ReachedBottom()
        {
            var invaderGroups = from invader in _invaders
                                group invader by invader.Location.X
                                into invaderGroup
                                orderby invaderGroup.Key descending
                                select invaderGroup;

            var biggestInvaderGroup = invaderGroups.OrderByDescending(i => i.Count()).First();

            var bottomInvader = biggestInvaderGroup.Last();

            if (bottomInvader.Location.Y <= 0)
            {
                return true;
            }

            return false;
        }

    }
}
