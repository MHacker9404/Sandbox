using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MediatorFormDemo
{
    internal class MarkerMediator
    {
        private List<Marker> _markers = new List<Marker>();

        public Marker CreateMarker()
        {
            var marker = new Marker();
            marker.SetMediator(this);
            _markers.Add(marker);
            return marker;
        }

        public void SendLocation(Point point, Marker marker)
        {
            _markers.Where(m => m != marker).ToList().ForEach(m => m.ReceiveLocation(point));
        }
    }
}