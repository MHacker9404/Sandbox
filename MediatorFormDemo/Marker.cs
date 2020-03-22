using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediatorFormDemo
{
    internal class Marker : Label
    {
        private MarkerMediator _mediator;
        private Point _mouseDownPoint;

        public Marker()
        {
            Text = "{Drag me}";
            TextAlign = ContentAlignment.MiddleCenter;
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
        }

        internal void SetMediator(MarkerMediator mediator)
        {
            _mediator = mediator;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Text = Location.ToString();
                Left = e.X + Left - _mouseDownPoint.X;
                Top = e.Y + Top - _mouseDownPoint.Y;
                _mediator.SendLocation(Location, this);
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseDownPoint = e.Location;
            }
        }

        public void ReceiveLocation(Point point)
        {
            var distance = CalcDistance(point);
            if (distance < 100 && BackColor != Color.Red)
            {
                BackColor = Color.Red;
            }
            else if (distance >= 100 && BackColor != Color.Green)
            {
                BackColor = Color.Green;
            }

            double CalcDistance(Point point) =>
                Math.Sqrt(Math.Pow(point.X - Location.X, 2) + Math.Pow(point.Y - Location.Y, 2));
        }
    }
}