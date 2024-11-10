using CommunityToolkit.Mvvm.ComponentModel;

namespace QuranParser;

// Class to represent a single Aya (verse)
public partial class Aya : ObservableObject
{
    // Property to hold whether the Aya is selected
    [ObservableProperty] private bool isSelected;

    public Aya()
    {
        IsSelected = false;
    }

    public int Index { get; set; }
    public string Text { get; set; }
    public string Bismillah { get; set; }

    // Computed property that returns Surah name and Index
    public string AyaWithIndex => $"{Index}. {Text}";
}

// Class to represent a Surah (chapter) with a list of Ayas (verses)
public class Surah
{
    public int Index { get; set; }
    public string Name { get; set; }
    public List<Aya> Ayas { get; set; } = new();

    // Computed property that returns Surah name and Index
    public string SurahInfo => $"{Index}. {Name}";
}

public enum SearchQueryType
{
    Surah,
    Aya
}

public enum TranslationVersesDataSetType
{
    Verse,
    WordsMeanings
}

public partial class WordForWordsMeaning : ObservableObject
{
    [ObservableProperty] public bool isSelected;

    public WordForWordsMeaning()
    {
        IsSelected = false;
    }

    public double Id { get; set; }
    public string Word { get; set; }
}