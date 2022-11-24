using CanteenManagementApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public class Item: ObservableObject
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public double Price { get; set; }
        public string Describe { get; set; }
        public double Amount { get; set; }
        public string ImagePath { get; set; }
    }
}
