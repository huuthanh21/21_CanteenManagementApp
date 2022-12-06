using CanteenManagementApp.Core;

namespace CanteenManagementApp.MVVM.Model
{
    public class ItemOrder: ObservableObject
    {
        public Item Item { get; set; }
        public int Amount { get; set; } = 0;
       
    }
}
