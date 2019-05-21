﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_3.Model
{
    public class Shot
    {
        public const double ShotPixelsPerSecond = 95;
        public Point Location { get; private set; }
        public static Size ShotSize = new Size(2, 10);
        private Direction _direction;
        public Direction Direction { get; private set; }
        private DateTime _lastMoved;
        public Shot(Point location, Direction direction)
        {
            Location = location;
            _direction = direction;
            _lastMoved = DateTime.Now;
        }
        public void Move()
        {
            TimeSpan timeSinceLastMoved = DateTime.Now - _lastMoved;
            double distance = timeSinceLastMoved.Milliseconds
            * ShotPixelsPerSecond / 1000;
            if (Direction == Direction.Up) distance *= -1;
            Location = new Point(Location.X, Location.Y + distance);
            _lastMoved = DateTime.Now;
        }
    }
}
