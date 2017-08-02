using IMAPI2.Interop;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;

namespace CDBurnCtrl
{
    public class CDUtils
    {
        public CDUtils()
        {
        }

        public bool DetectCDRoms(ref vComboBox lst)
        {
            bool flag = false;
            MsftDiscMaster2 gClass0 = null;
            try
            {
                try
                {
                    gClass0 = (MsftDiscMaster2)(new GClass0());
                    if (gClass0.IsSupportedEnvironment)
                    {
                        foreach (string str in gClass0)
                        {
                            MsftDiscRecorder2 msftDiscRecorder2Class = (MsftDiscRecorder2)(new MsftDiscRecorder2Class());
                            msftDiscRecorder2Class.InitializeDiscRecorder(str);
                            string str1 = msftDiscRecorder2Class.VolumePathNames[0].ToString();
                            string productId = msftDiscRecorder2Class.ProductId;
                            ListItem listItem = new ListItem()
                            {
                                Text = string.Format("{0} [{1}]", str1, productId),
                                Tag = msftDiscRecorder2Class,
                                ImageIndex = 0
                            };
                            lst.Items.Add(listItem);
                            flag = true;
                        }
                        if (lst.Items.Count > 0)
                        {
                            lst.SelectedIndex = 0;
                        }
                    }
                }
                catch (COMException cOMException)
                {
                    throw;
                }
            }
            finally
            {
                if (gClass0 != null)
                {
                    Marshal.ReleaseComObject(gClass0);
                }
            }
            return flag;
        }

        public bool DetectMedia(ListItem li, ref Label lbl)
        {
            bool flag = false;
            lbl.Text = string.Empty;
            MsftFileSystemImage msftFileSystemImage = null;
            GInterface6 msftDiscFormat2DataClass = null;
            IDiscRecorder2 tag = (IDiscRecorder2)li.Tag;
            try
            {
                try
                {
                    msftDiscFormat2DataClass = (GInterface6)(new MsftDiscFormat2DataClass());
                    if (msftDiscFormat2DataClass.IsCurrentMediaSupported(tag))
                    {
                        msftDiscFormat2DataClass.Recorder = tag;
                        lbl.Text = this.GetMediaTypeString(msftDiscFormat2DataClass.CurrentPhysicalMediaType);
                        flag = true;
                    }
                    else
                    {
                        lbl.Text = "No Media";
                    }
                }
                catch (COMException cOMException)
                {
                }
            }
            finally
            {
                if (msftDiscFormat2DataClass != null)
                {
                    Marshal.ReleaseComObject(msftDiscFormat2DataClass);
                }
                if (msftFileSystemImage != null)
                {
                    Marshal.ReleaseComObject(msftFileSystemImage);
                }
            }
            return flag;
        }

        public string GetMediaTypeString(IMAPI_MEDIA_PHYSICAL_TYPE mediaType)
        {
            switch (mediaType)
            {
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDROM:
                    {
                        return "CD-ROM";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDR:
                    {
                        return "CD-R";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_CDRW:
                    {
                        return "CD-RW";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDROM:
                    {
                        return "DVD ROM";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDRAM:
                    {
                        return "DVD-RAM";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSR:
                    {
                        return "DVD+R";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSRW:
                    {
                        return "DVD+RW";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSR_DUALLAYER:
                    {
                        return "DVD+R Dual Layer";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHR:
                    {
                        return "DVD-R";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHRW:
                    {
                        return "DVD-RW";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDDASHR_DUALLAYER:
                    {
                        return "DVD-R Dual Layer";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK:
                    {
                        return "random-access writes";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DVDPLUSRW_DUALLAYER:
                    {
                        return "DVD+RW DL";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDROM:
                    {
                        return "HD DVD-ROM";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDR:
                    {
                        return "HD DVD-R";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_HDDVDRAM:
                    {
                        return "HD DVD-RAM";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDROM:
                    {
                        return "Blu-ray DVD (BD-ROM)";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDR:
                    {
                        return "Blu-ray media";
                    }
                case IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_BDRE:
                    {
                        return "Blu-ray Rewritable media";
                    }
                default:
                    {
                        return "Unknown Media Type";
                    }
            }
        }
    }
}