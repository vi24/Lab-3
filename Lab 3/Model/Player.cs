using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_3.Model
{
    public class Player : Ship
    {
        public static readonly Size PlayerSize = new Size(25, 15);
        const double SPEED = 10;

        public Player(Point point, Size size): base(point, size)
        {}

        public override void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (Location.X > PlayerSize.Width)
                        Location = new Point(Location.X - SPEED, Location.Y);
                    break;
                case Direction.Right:
                    if (Location.X < InvadersModel.PlayAreaSize.Width - PlayerSize.Width) ;
                        Location = new Point(Location.X + SPEED, Location.Y);
                    break;
            }
        }


    }
}
