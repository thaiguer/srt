using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Srt.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        TextBlock1.Text = DateTime.Now.ToString();
    }
}