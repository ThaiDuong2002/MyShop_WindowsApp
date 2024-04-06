using MyShopProject.View;
using MyShopProject.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.Commands
{
    public class UpdateViewCommand : ICommand
    {
        MainViewModel viewModel;
        Window creatingForm;
        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public Window setCreatingForm
        {
            get { return creatingForm; }
            set { creatingForm = value; }
        }

        public void Execute(object parameter)
        {
            Trace.WriteLine(parameter.ToString());

            if (parameter.ToString() == "Dashboard")
            {
                viewModel.SelectedViewModel = new DashboardViewModel();
            }
            else if (parameter.ToString() == "QLKH")
            {
                viewModel.SelectedViewModel = new QLKHViewModel();
            }
            else if (parameter.ToString() == "QLLOAISP")
            {
                viewModel.SelectedViewModel = new ProductCategoryManagementViewModel();
            }
            else if (parameter.ToString() == "QLSP")
            {
                viewModel.SelectedViewModel = new ProductManagementViewModel();
            }
            else if (parameter.ToString() == "QLDH")
            {
                viewModel.SelectedViewModel = new OrderManagementViewModel();
            }
            else if (parameter.ToString() == "QLKM")
            {
                viewModel.SelectedViewModel = new PromotionManagementViewModel();
            }
            else if (parameter.ToString() == "DB")
            {
                viewModel.SelectedViewModel = new BackupRestoreDBViewModel();
            }
            else if (parameter.ToString() == "TKDTVLN")
            {
                viewModel.SelectedViewModel = new RevenueStatisticsViewModel();
            }
            else if (parameter.ToString() == "TKSP")
            {
                viewModel.SelectedViewModel = new ProductStatisticsViewModel();
            }
            else if (parameter.ToString() == "TKBH")
            {
                viewModel.SelectedViewModel = new ReportProductViewModel();
            }
            else if (parameter.ToString() == "dang_xuat")
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    LoginView screen = new LoginView();
                    screen.Show();
                    Window myWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.Name == "dashboard")!;

                    // Kiểm tra nếu cửa sổ tồn tại và đang được hiển thị
                    if (myWindow != null && myWindow.IsVisible)
                    {
                        // Tắt cửa sổ
                        myWindow.Close();
                    }
                }
                return;
            }
        }
    }
}
