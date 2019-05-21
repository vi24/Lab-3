using System;
using System.Windows;

namespace Lab_3.Model.Eventargs
{
    class StarChangedEventArgs : EventArgs
    {
        public Point Point { get; private set; }
        public bool Disappeared { get; private set; }
        public StarChangedEventArgs(Point point, bool disappeared)
        {
            Point = point;
            Disappeared = disappeared;
        }
    }
}
