using AlTanzeel.ViewModel;
using DrawnUi.Maui.Draw;

namespace AlTanzeel.Pages;

public partial class QuizReport : SkiaLayout
{
    private MainViewModel ViewModel;

    public QuizReport(MainViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        Console.WriteLine($"TAKASUR:: {ViewModel.SelectedSura.Name}");
        Console.WriteLine($"TAKASUR:: {ViewModel.SelectedAyasForTranslation.Count.ToString()}");
    }
}