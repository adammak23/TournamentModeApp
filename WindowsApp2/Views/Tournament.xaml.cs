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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsApp2.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tournament : Page
    {
        List<TournamentModel> turnieje = new List<TournamentModel>();
        public Tournament()
        {
            this.InitializeComponent();
           // Tournament tournamentmodel = new Tournament();
            
            turnieje.Add(new TournamentModel("1v1", "EUNE", 100, 1, "blindpick", "Elimination", "Howling Abyss") { });
            turnieje.Add(new TournamentModel("3v3 Blindpick BO5", "EUNE", 100, 3, "blindpick", "Best of 5", "Twisted Treeline") { });
            turnieje.Add(new TournamentModel("5v5 random elimination", "EUNE", 100, 5, "random", "Elimination", "Summoner's Rift") { });
            turniejeList.ItemsSource = turnieje;
            
        }
    }
}
