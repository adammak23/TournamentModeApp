using WindowsApp2.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using WindowsApp2.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WindowsApp2.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
    }
    
}

