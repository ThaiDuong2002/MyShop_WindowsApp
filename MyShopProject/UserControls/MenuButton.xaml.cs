using MyShopProject.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyShopProject.UserControls
{
    /// <summary>
    /// Interaction logic for MenuButton.xaml
    /// </summary>
    public partial class MenuButton : UserControl
    {
        public ICommand updateViewCommand { get; set; }
        public MenuButton()
        {
            InitializeComponent();

            updateViewCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => { });
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(MenuButton));

        public bool isActive
        {
            get { return (bool)GetValue(isActiveProperty); }
            set { SetValue(isActiveProperty, value); }
        }

        public static readonly DependencyProperty isActiveProperty = DependencyProperty.Register("isActive", typeof(bool), typeof(MenuButton));

        public MahApps.Metro.IconPacks.PackIconMaterialKind Icon
        {
            get { return (MahApps.Metro.IconPacks.PackIconMaterialKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(MahApps.Metro.IconPacks.PackIconMaterialKind), typeof(MenuButton));

        public string NameView
        {
            get { return (string)GetValue(NameViewProperty); }
            set { SetValue(NameViewProperty, value); }
        }

        public static readonly DependencyProperty NameViewProperty = DependencyProperty.Register("NameView", typeof(string), typeof(MenuButton));


        public Uri Navlink
        {
            get { return (Uri)GetValue(NavlinkProperty); }
            set { SetValue(NavlinkProperty, value); }
        }

        public static readonly DependencyProperty NavlinkProperty = DependencyProperty.Register("Navlink", typeof(Uri), typeof(MenuButton));
    }
}
