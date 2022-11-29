using CanteenManagementApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public class ReportItem : ObservableObject, ICloneable
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
