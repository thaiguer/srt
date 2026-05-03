using Avalonia.Controls;
using Avalonia.Interactivity;
using Srt.Core.Core;
using Srt.Core.Model;
using System;
using System.Collections.Generic;

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

        var srtFile = new SrtFile();
        srtFile.FilePath = "C:\\devAux\\sample.srt";

        var fileHandler = new FileHandler();
        srtFile.OriginalContent = fileHandler.ReadContent(srtFile.FilePath);

        List<string> items = new()
        {
            "Apple",
            "Banana",
            "Orange"
        };

        srtFile.OriginalContent[3].Content = items;

        fileHandler.WriteContent("C:\\devAux\\new.srt", srtFile.OriginalContent);
    }
}