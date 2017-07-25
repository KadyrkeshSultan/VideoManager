using System;
using System.Windows.Forms;
using System.Management;
using VMInterfaces;
using VMModels.Model;

namespace VideoManager
{
    public partial class Form1 : Form
    {
        private ManagementEventWatcher watcher = null;

        public Form1()
        {
            InitializeComponent();
            watcher = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            watcher.EventArrived += new EventArrivedEventHandler(watcher_EventArrived);
            watcher.Query = query;
            watcher.Start();
            watcher.WaitForNextEvent();
        }

        private void watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string str = e.NewEvent.Properties["DriveName"].Value.ToString();
            MessageBox.Show(str);
        }
    }
}
