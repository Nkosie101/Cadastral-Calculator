using System;
using System.IO;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Surveyors_Calculator
{
    public partial class App : Application
    {


        private readonly SqliteConnectionFactory _connectionFactory;

        public App(SqliteConnectionFactory connectionFactory)
        {

            InitializeComponent();

            //MainPage = new AppShell();

            _connectionFactory = connectionFactory;

            bool isDark = Preferences.Default.Get("AppThemeDark", false);
            if (isDark)
            {
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Light;
            }

        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {

            ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

            await database.CreateTableAsync<CoordinateDTO>();
            await database.CreateTableAsync<History>();
            //database.CreateTable<CoordinateDTO>();
            base.OnStart();


        }
    }
}