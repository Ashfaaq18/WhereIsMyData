﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WhereIsMyData.Models
{
    public class MyAppInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private ulong currentdataRecv;
        public ulong CurrentDataRecv
        {
            get { return currentdataRecv; }
            set { currentdataRecv = value; OnPropertyChanged("CurrentDataRecv"); }
        }

        private ulong currentdataSend;
        public ulong CurrentDataSend
        {
            get { return currentdataSend; }
            set { currentdataSend = value; OnPropertyChanged("CurrentDataSend"); }
        }

        private ulong totaldataRecv;
        public ulong TotalDataRecv
        {
            get { return totaldataRecv; }
            set { totaldataRecv = value; OnPropertyChanged("TotalDataRecv"); }
        }
        private ulong totaldataSend;
        public ulong TotalDataSend
        {
            get { return totaldataSend; }
            set { totaldataSend = value; OnPropertyChanged("TotalDataSend"); }
        }

        public ImageSource Icon { get; set; }

        public string Image { get; set; }
        public MyAppInfo(string nameP, ulong dataRecvP, ulong dataSendP, System.Drawing.Icon icon)
        {
            Name = nameP;
            TotalDataRecv = dataRecvP;
            TotalDataSend = dataSendP;
            CurrentDataRecv = 0;
            CurrentDataSend = 0;
            if(icon != null)
            {
                ImageSource im = IconToImgSource.ToImageSource(icon);
                Icon = im;
                im.Freeze();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
