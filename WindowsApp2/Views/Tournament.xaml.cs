using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsApp2.Models;
using WindowsApp2.ViewModels;
using System.Diagnostics;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsApp2.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tournament : Page
    {
        public Tournament()
        {
            this.InitializeComponent();
            turniejeList.ItemsSource = TournamentViewModel.turnieje;
        }

        public void ItemClick(object sender, ItemClickEventArgs e)
        {
            int index;
            index = TournamentViewModel.ItemClick(sender,e);
            var buttons = AllChildren(turniejeList).Where(x => x is Button);
            foreach (Button button in buttons) button.Visibility = Visibility.Collapsed;
            buttons.ElementAt(index).Visibility=Visibility.Visible;
        }

        public IEnumerable<Control> AllChildren(DependencyObject parent)
        {
            for (int index = 0; index < VisualTreeHelper.GetChildrenCount(parent); index++)
            {
                var child = VisualTreeHelper.GetChild(parent, index);
                if (child is Control)
                    yield return child as Control;
                foreach (var item in AllChildren(child))
                    yield return item;
            }
        }
    }
}
