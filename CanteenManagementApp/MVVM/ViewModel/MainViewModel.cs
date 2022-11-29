using CanteenManagementApp.Core;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand CreateOrderViewCommand { get; set; }
        public RelayCommand CustomerViewCommand { get; set; }
        public RelayCommand MenuViewCommand { get; set; }
        public RelayCommand StorageViewCommand { get; set; }
        public RelayCommand ReportViewCommand { get; set; }

        public CreateOrderViewModel CreateOrderVM { get; set; }
        public CustomerViewModel CustomerVM { get; set; }
        public MenuViewModel MenuVM { get; set; }
        public StorageViewModel StorageVM { get; set; }
        public ReportViewModel ReportVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; }
        }

        public MainViewModel()  
        {
            //CreateOrderVM = new CreateOrderViewModel();
            CustomerVM = new CustomerViewModel();
            MenuVM = new MenuViewModel();
            StorageVM = new StorageViewModel();
            ReportVM = new ReportViewModel();

            CurrentView = CreateOrderVM;

            CreateOrderViewCommand = new RelayCommand(o => 
            {
                CurrentView = CreateOrderVM;
            });

            CustomerViewCommand = new RelayCommand(o =>
            {
                CurrentView = CustomerVM;
            });

            MenuViewCommand = new RelayCommand(o =>
            {
                CurrentView = MenuVM;
            });

            StorageViewCommand = new RelayCommand(o =>
            {
                CurrentView = StorageVM;
            });

            ReportViewCommand = new RelayCommand(o =>
            {
                CurrentView = ReportVM;
            });
        }
    }
}
