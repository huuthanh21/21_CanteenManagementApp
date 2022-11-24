using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public class Book : INotifyPropertyChanged, ICloneable
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string ImagePath { get; set; }
        public string PublishedYear { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        private void TestMethod()
        {
            if (PropertyChanged != null)
                return;
        }
    }
}
