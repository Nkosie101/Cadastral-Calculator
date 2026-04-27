using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Surveyors_Calculator.View;
using Microsoft.Maui.Storage;

namespace Surveyors_Calculator
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CalculationsMenuPage), typeof(CalculationsMenuPage));
            Routing.RegisterRoute(nameof(CalculationsPage), typeof(CalculationsPage));
            Routing.RegisterRoute(nameof(Input), typeof(Input));
            Routing.RegisterRoute(nameof(QuickTransform), typeof(QuickTransform));
            Routing.RegisterRoute(nameof(Settings), typeof(Settings));
            Routing.RegisterRoute(nameof(Recent), typeof(Recent));
            Routing.RegisterRoute(nameof(About), typeof(About));
            Routing.RegisterRoute(nameof(Conversions), typeof(Conversions));

        }

        private async void OnAboutClicked(object sender, EventArgs e)
        {
            // Closes the flyout
            AppShell.Current.FlyoutIsPresented = false;

            // Navigates to the About Page
            // This keeps the TabBar of the "Home" FlyoutItem visible 
            // IF AboutPage is registered as a route.
            await Shell.Current.GoToAsync("About");
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // Closes the flyout
            AppShell.Current.FlyoutIsPresented = false;

            // Navigates to the About Page
            // This keeps the TabBar of the "Home" FlyoutItem visible 
            // IF AboutPage is registered as a route.
            await Shell.Current.GoToAsync("Settings");
        }

        private async void OnToolBoxClicked(object sender, EventArgs e)
        {
            // Closes the flyout
            AppShell.Current.FlyoutIsPresented = false;

            // Navigates to the About Page
            // This keeps the TabBar of the "Home" FlyoutItem visible 
            // IF AboutPage is registered as a route.
            await Shell.Current.GoToAsync("CalculationsMenuPage");
        }
    }
}