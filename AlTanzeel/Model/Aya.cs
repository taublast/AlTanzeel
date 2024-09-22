namespace QuranParser
{
    // Class to represent a single Aya (verse)
    public class Aya
    {
        public int Index { get; set; }
        public string Text { get; set; }
    }

    // Class to represent a Sura (chapter) with a list of Ayas (verses)
    public class Sura
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<Aya> Ayas { get; set; } = new List<Aya>();

        // Computed property that returns Surah name and Index
        public string SurahInfo => $"{Index}. {Name}";
    }
}
