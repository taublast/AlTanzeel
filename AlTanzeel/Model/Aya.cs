namespace QuranParser
{
    // Class to represent a single Aya (verse)
    public class Aya
    {
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
        public List<Aya> Ayas { get; set; } = new List<Aya>();

        // Computed property that returns Surah name and Index
        public string SurahInfo => $"{Index}. {Name}";
    }

    public enum SearchQueryType
    {
        Surah,
        Aya
    }
}
