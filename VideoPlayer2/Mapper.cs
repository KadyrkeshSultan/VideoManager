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
            Markers = new GMapOverlay(mainMap, "VideoPlay_Mapper_1");
            mainMap.Overlays.Add(Markers);
            RoutesOverlay = new GMapOverlay(mainMap, "VideoPlay_Mapper_2");
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
            TotalPts = 0.0;
            bool flag = false;
            pts = new List<PointLatLng>();
            string path = file + fileExt;
            int num1 = 0;
            if (File.Exists(path))
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string str1;
                    while ((str1 = streamReader.ReadLine()) != null)
                    {
                        ++num1;
                        if (str1.Contains("VideoPlay_Mapper_3"))
                        {
                            string str2 = "VideoPlay_Mapper_4";
                            string str3 = "VideoPlay_Mapper_5";
                            string[] strArray = str1.Split(',');
                            if (strArray[0].ToUpper().StartsWith("VideoPlay_Mapper_6"))
                            {
                                str2 = "VideoPlay_Mapper_7";
                                strArray[0] = strArray[0].Substring(1);
                            }
                            else
                                strArray[0] = strArray[0].Substring(1);
                            if (strArray[1].ToUpper().StartsWith("VideoPlay_Mapper_8"))
                            {
                                str3 = "VideoPlay_Mapper_9";
                                strArray[1] = strArray[1].Substring(1);
                            }
                            else
                                strArray[1] = strArray[1].Substring(1);
                            try
                            {
                                if (Convert.ToDouble(strArray[0]) == 0.0 && Convert.ToDouble(strArray[1]) == 0.0)
                                {
                                    str1 = string.Format("VideoPlay_Mapper_10", str2, str3);
                                }
                                else
                                {
                                    int num2 = strArray[0].IndexOf("VideoPlay_Mapper_11");
                                    int num3 = strArray[1].IndexOf("VideoPlay_Mapper_12");
                                    double num4 = Convert.ToDouble(strArray[0].Substring(0, strArray[0].Length - (strArray[0].Length - num2 + 2))) + Convert.ToDouble(strArray[0].Substring(num2 - 2)) / 60.0;
                                    double num5 = Convert.ToDouble(strArray[1].Substring(0, strArray[1].Length - (strArray[1].Length - num3 + 2))) + Convert.ToDouble(strArray[1].Substring(num3 - 2)) / 60.0;
                                    str1 = string.Format("VideoPlay_Mapper_13", str2, num4, str3, num5);
                                }
                            }
                            catch
                            {
                            }
                        }
                        try
                        {
                            Pos.Add(str1);
                            string[] strArray = str1.Split(';');
                            strArray[0] = !strArray[0].ToUpper().StartsWith("VideoPlay_Mapper_14") ? "VideoPlay_Mapper_15" + strArray[0].Substring(1) : strArray[0].Substring(1);
                            strArray[1] = !strArray[1].ToUpper().StartsWith("VideoPlay_Mapper_16") ? strArray[1].Substring(1) : "VideoPlay_Mapper_17" + strArray[1].Substring(1);
                            TotalPts += Convert.ToDouble(strArray[0]);
                            PointLatLng pointLatLng = new PointLatLng(Convert.ToDouble(strArray[0]), Convert.ToDouble(strArray[1]));
                            pts.Add(pointLatLng);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            if (pts.Count > 0)
                flag = true;
            maproute = new GMapRoute(pts, "VideoPlay_Mapper_18");
            pts2 = pts;
            double distance = maproute.Distance;
            return flag;
        }

        
        public static void UpdatePosition(int idx)
        {
            try
            {
                int count = pts2.Count;
                if (count <= 0 || idx > count - 1)
                    return;
                string[] strArray1 = Pos[idx].Split(';');
                strArray1[0] = !strArray1[0].ToUpper().StartsWith("VideoPlay_Mapper_19") ? "VideoPlay_Mapper_20" + strArray1[0].Substring(1) : strArray1[0].Substring(1);
                strArray1[1] = !strArray1[1].ToUpper().StartsWith("VideoPlay_Mapper_21") ? strArray1[1].Substring(1) : "VideoPlay_Mapper_22" + strArray1[1].Substring(1);
                if (idx > 2)
                {
                    string[] strArray2 = Pos[idx - 1].Split(';');
                    strArray2[0] = !strArray2[0].ToUpper().StartsWith("VideoPlay_Mapper_23") ? "VideoPlay_Mapper_24" + strArray2[0].Substring(1) : strArray2[0].Substring(1);
                    strArray2[1] = !strArray2[1].ToUpper().StartsWith("VideoPlay_Mapper_25") ? strArray2[1].Substring(1) : "VideoPlay_Mapper_26" + strArray2[1].Substring(1);
                    LastLat = Convert.ToDouble(strArray2[0]);
                    LastLng = Convert.ToDouble(strArray2[1]);
                    if (LastLat != 0.0 && LastLng != 0.0)
                    {
                        double num = CalcDist(new PointLatLng(LastLat, LastLng), new PointLatLng(Convert.ToDouble(strArray1[0]), Convert.ToDouble(strArray1[1]))) * 5280.0 * 0.681818 * 0.621371;
                        CompassBearing = (int)Geodesy.Bearing(LastLat, LastLng, Convert.ToDouble(strArray1[0]), Convert.ToDouble(strArray1[1]));
                        compass.SetData((int)num, CompassBearing);
                    }
                }
                PointLatLng pointLatLng = new PointLatLng(Convert.ToDouble(strArray1[0]), Convert.ToDouble(strArray1[1]));
                mainMap.Overlays.Clear();
                Markers.Markers.Clear();
                GMapMarker gmapMarker = new GMapMarkerGoogleGreen(mainMap.Position);
                Markers.Markers.Add(gmapMarker);
                mainMap.Overlays.Add(Markers);
                mainMap.Position = pointLatLng;
            }
            catch (Exception ex)
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
