using Microsoft.Maui.Controls;

namespace Surveyors_Calculator.Templates;

public partial class MenuItem : ContentView
{
    public MenuItem()
    {
        InitializeComponent();
        //MenuItems = new ObservableCollection<MenuItemModel>();
    }


    /*public static readonly BindableProperty IconProperty =
                BindableProperty.Create(nameof(Icon), typeof(string), typeof(MenuItem));

    public static readonly BindableProperty TextProperty =
                BindableProperty.Create(nameof(Text), typeof(string), typeof(MenuItem));

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }*/

    /*public class MenuItemModel
    {
        public string Icon { get; set; }
        public string Text { get; set; }
    }

    public ObservableCollection<MenuItemModel> MenuItems { get; set; }

    MenuItems = new ObservableCollection<MenuItemModel>
{
    new MenuItemModel { Icon = "icon1.png", Text = "Menu Item 1" },
    new MenuItemModel { Icon = "icon2.png", Text = "Menu Item 2" },
};*/




}

