using CanteenManagementApp.Core;
using System;

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
