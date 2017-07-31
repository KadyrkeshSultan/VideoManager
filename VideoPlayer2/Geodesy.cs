using System;

namespace VideoPlayer2
{
    public static class Geodesy
    {
        
        public static double RadToDeg(double radians)
        {
            return radians * (180.0 / Math.PI);
        }

        
        public static double DegToRad(double degrees)
        {
            return degrees * (Math.PI / 180.0);
        }

        
        public static double Bearing(double lat1, double long1, double lat2, double long2)
        {
            lat1 = Geodesy.DegToRad(lat1);
            long1 = Geodesy.DegToRad(long1);
            lat2 = Geodesy.DegToRad(lat2);
            long2 = Geodesy.DegToRad(long2);
            double num = long2 - long1;
            return Geodesy.ConvertToBearing(Geodesy.RadToDeg(Math.Atan2(Math.Sin(num) * Math.Cos(lat2), Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(num))));
        }

        
        public static double ConvertToBearing(double deg)
        {
            return (deg + 360.0) % 360.0;
        }
    }
}
