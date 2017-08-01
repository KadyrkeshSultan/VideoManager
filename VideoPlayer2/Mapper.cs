using VMMapEngine;
using Compass;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.IO;

namespace VideoPlayer2
{
    public static class Mapper
    {
        private static GMapControl mainMap;
        private static GMapOverlay Markers;
        private static GMapOverlay RoutesOverlay;
        private static GMapRoute maproute;
        private static List<PointLatLng> pts;
        private static List<PointLatLng> pts2;
        private static List<string> Pos;
        private static VMCompass compass;
        private static double TotalPts;
        private static double LastLat;
        private static double LastLng;
        private static int CompassBearing;

        
        static Mapper()
        {
            mainMap = null;
            pts = new List<PointLatLng>();
            pts2 = new List<PointLatLng>();
            Pos = new List<string>();
            compass = new VMCompass();
            TotalPts = 0.0;
            LastLat = 0.0;
            LastLng = 0.0;
            CompassBearing = 0;
        }

        
        public static void SetMapper(VMMap mapEng)
        {
            mainMap = mapEng.GetMapControl;
            Markers = new GMapOverlay(mainMap, "Vehicle");
            mainMap.Overlays.Add(Markers);
            RoutesOverlay = new GMapOverlay(mainMap, "Routes");
            mainMap.Overlays.Add(RoutesOverlay);
            mainMap.MarkersEnabled = true;
            mainMap.Zoom = mainMap.MaxZoom;
        }

        
        public static double GPSDataPoints()
        {
            return TotalPts;
        }

        
        public static void ClearPoints()
        {
            pts = new List<PointLatLng>();
            Pos.Clear();
        }

        
        public static void ShowCompass(bool b)
        {
            if (mainMap == null)
                return;
            try
            {
                if (b)
                    mainMap.Controls.Add(compass);
                else
                    mainMap.Controls.Clear();
            }
            catch
            {
            }
        }

        
        public static void MapHide()
        {
            mainMap.Hide();
        }

        
        public static void MapShow()
        {
            mainMap.Show();
        }

        
        public static bool LoadDataPoints(string file, string fileExt)
        {
            TotalPts = 0;
            bool flag = false;
            pts = new List<PointLatLng>();
            string str = string.Concat(file, fileExt);
            int num = 0;
            if (File.Exists(str))
            {
                using (StreamReader streamReader = new StreamReader(str))
                {
                    while (true)
                    {
                        string str1 = streamReader.ReadLine();
                        string str2 = str1;
                        if (str1 == null)
                        {
                            break;
                        }
                        num++;
                        if (str2.Contains(","))
                        {
                            string str3 = "S";
                            string str4 = "E";
                            string[] strArrays = str2.Split(new char[] { ',' });
                            if (!strArrays[0].ToUpper().StartsWith("N"))
                            {
                                strArrays[0] = strArrays[0].Substring(1);
                            }
                            else
                            {
                                str3 = "N";
                                strArrays[0] = strArrays[0].Substring(1);
                            }
                            if (!strArrays[1].ToUpper().StartsWith("W"))
                            {
                                strArrays[1] = strArrays[1].Substring(1);
                            }
                            else
                            {
                                str4 = "W";
                                strArrays[1] = strArrays[1].Substring(1);
                            }
                            try
                            {
                                if (Convert.ToDouble(strArrays[0]) != 0 || Convert.ToDouble(strArrays[1]) != 0)
                                {
                                    int num1 = strArrays[0].IndexOf(".");
                                    int num2 = strArrays[1].IndexOf(".");
                                    string str5 = strArrays[0].Substring(0, strArrays[0].Length - (strArrays[0].Length - num1 + 2));
                                    string str6 = strArrays[0].Substring(num1 - 2);
                                    double num3 = Convert.ToDouble(str5) + Convert.ToDouble(str6) / 60;
                                    string str7 = strArrays[1].Substring(0, strArrays[1].Length - (strArrays[1].Length - num2 + 2));
                                    string str8 = strArrays[1].Substring(num2 - 2);
                                    double num4 = Convert.ToDouble(str7) + Convert.ToDouble(str8) / 60;
                                    object[] objArray = new object[] { str3, num3, str4, num4 };
                                    str2 = string.Format("{0}{1};{2}{3}", objArray);
                                }
                                else
                                {
                                    str2 = string.Format("{0}00.0000;{1}00.0000", str3, str4);
                                }
                            }
                            catch
                            {
                            }
                        }
                        try
                        {
                            Mapper.Pos.Add(str2);
                            string[] strArrays1 = str2.Split(new char[] { ';' });
                            if (!strArrays1[0].ToUpper().StartsWith("N"))
                            {
                                strArrays1[0] = string.Concat("-", strArrays1[0].Substring(1));
                            }
                            else
                            {
                                strArrays1[0] = strArrays1[0].Substring(1);
                            }
                            if (!strArrays1[1].ToUpper().StartsWith("W"))
                            {
                                strArrays1[1] = strArrays1[1].Substring(1);
                            }
                            else
                            {
                                strArrays1[1] = string.Concat("-", strArrays1[1].Substring(1));
                            }
                            TotalPts += Convert.ToDouble(strArrays1[0]);
                            PointLatLng pointLatLng = new PointLatLng(Convert.ToDouble(strArrays1[0]), Convert.ToDouble(strArrays1[1]));
                            pts.Add(pointLatLng);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if (pts.Count > 0)
            {
                flag = true;
            }
            maproute = new GMapRoute(pts, "ROUTE");
            pts2 = pts;
            double distance = maproute.Distance;
            return flag;
        }

        
        public static void UpdatePosition(int idx)
        {
            try
            {
                int count = pts2.Count;
                if (count > 0 && idx <= count - 1)
                {
                    string[] strArrays = Pos[idx].Split(new char[] { ';' });
                    if (!strArrays[0].ToUpper().StartsWith("N"))
                    {
                        strArrays[0] = string.Concat("-", strArrays[0].Substring(1));
                    }
                    else
                    {
                        strArrays[0] = strArrays[0].Substring(1);
                    }
                    if (!strArrays[1].ToUpper().StartsWith("W"))
                    {
                        strArrays[1] = strArrays[1].Substring(1);
                    }
                    else
                    {
                        strArrays[1] = string.Concat("-", strArrays[1].Substring(1));
                    }
                    if (idx > 2)
                    {
                        string[] strArrays1 = Pos[idx - 1].Split(new char[] { ';' });
                        if (!strArrays1[0].ToUpper().StartsWith("N"))
                        {
                            strArrays1[0] = string.Concat("-", strArrays1[0].Substring(1));
                        }
                        else
                        {
                            strArrays1[0] = strArrays1[0].Substring(1);
                        }
                        if (!strArrays1[1].ToUpper().StartsWith("W"))
                        {
                            strArrays1[1] = strArrays1[1].Substring(1);
                        }
                        else
                        {
                            strArrays1[1] = string.Concat("-", strArrays1[1].Substring(1));
                        }
                        LastLat = Convert.ToDouble(strArrays1[0]);
                        LastLng = Convert.ToDouble(strArrays1[1]);
                        if (LastLat != 0 && LastLng != 0)
                        {
                            double num = CalcDist(new PointLatLng(LastLat, LastLng), new PointLatLng(Convert.ToDouble(strArrays[0]), Convert.ToDouble(strArrays[1])));
                            double num1 = num * 5280 * 0.681818 * 0.621371;
                            CompassBearing = (int)Geodesy.Bearing(LastLat, LastLng, Convert.ToDouble(strArrays[0]), Convert.ToDouble(strArrays[1]));
                            compass.SetData((int)num1, CompassBearing);
                        }
                    }
                    PointLatLng pointLatLng = new PointLatLng(Convert.ToDouble(strArrays[0]), Convert.ToDouble(strArrays[1]));
                    mainMap.Overlays.Clear();
                    Markers.Markers.Clear();
                    GMapMarker gMapMarkerGoogleGreen = new GMapMarkerGoogleGreen(mainMap.Position);
                    Markers.Markers.Add(gMapMarkerGoogleGreen);
                    mainMap.Overlays.Add(Markers);
                    mainMap.Position = pointLatLng;
                }
            }
            catch (Exception exception)
            {
            }
        }

        
        private static double CalcDist(PointLatLng p1, PointLatLng p2)
        {
            double num1 = Math.PI * p1.Lat / 180.0;
            double num2 = Math.PI * p1.Lng / 180.0;
            double num3 = Math.PI * p2.Lat / 180.0;
            double num4 = Math.PI * p2.Lng / 180.0;
            return 6371.0 * Math.Acos(Math.Cos(num1) * Math.Cos(num3) * Math.Cos(num2) * Math.Cos(num4) + Math.Cos(num1) * Math.Sin(num2) * Math.Cos(num3) * Math.Sin(num4) + Math.Sin(num1) * Math.Sin(num3));
        }
    }
}
