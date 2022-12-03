using CanteenManagementApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public class ItemOrder: ObservableObject
    {
        public Item Item { get; set; }
        public int Amount { get; set; } = 0;
       
    }
}
