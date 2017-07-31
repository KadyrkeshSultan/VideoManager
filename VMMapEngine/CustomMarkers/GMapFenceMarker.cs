using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace VMMapEngine.CustomMarkers
{
    [Serializable]
    public class GMapFenceMarker : GMapMarker, ISerializable
    {
        [NonSerialized]
        public Pen Pen;
        [NonSerialized]
        public GMapMarkerGoogleGreen InnerMarker;

        

        public GMapFenceMarker(PointLatLng p)
            :base(p)
        {
            Pen = new Pen(Brushes.Blue, 3f);
            Size = new Size(11, 11);
            this.Offset = new Point(-this.Size.Width / 2, -this.Size.Height / 2);
        }

        
        protected GMapFenceMarker(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
        }

        
        public override void OnRender(Graphics g)
        {
            g.DrawRectangle(Pen, new Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height));
        }

        
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            GetObjectData(info, context);
        }
    }
}
