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
        public Item _item { get; set; }
        public int _amount { get; set; } = 0;
       
    }
}
