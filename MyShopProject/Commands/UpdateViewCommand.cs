using Microsoft.Maps.MapControl.WPF.Overlays;
using MyShopProject.View;
using MyShopProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        }
    }
}
