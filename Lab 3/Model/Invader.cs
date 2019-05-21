using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_3.Model
{
    public class Invader: Ship
    {
        public Size InvaderSize { get; set; }
        public const int HORIZONTAL_MOVE_INTERVAL = 5;
        public const int VERTICALPIXELSPEED = 15;
        public InvaderType InvaderType { get; set; }
        public int Score { get; set; }

        public Invader (InvaderType invaderType, Point point, Size size): base(point, size)
        {
            InvaderType = invaderType;
        }


        public override void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    Location = new Point(Location.X + HORIZONTAL_MOVE_INTERVAL, Location.Y);
                    break;
                case Direction.Left:
                    Location = new Point(Location.X - HORIZONTAL_MOVE_INTERVAL, Location.Y);
                    break;
                case Direction.Down:
                    Location = new Point(Location.X, Location.Y + VERTICALPIXELSPEED);
                    break;
            }
        }
    }
}
