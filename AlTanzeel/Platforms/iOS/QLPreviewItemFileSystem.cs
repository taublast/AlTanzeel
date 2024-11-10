using Foundation;
using QuickLook;

public class QLPreviewItemFileSystem : QLPreviewItem
{
    private readonly string _fileName, _filePath;

    public QLPreviewItemFileSystem(string fileName, string filePath)
    {
        _fileName = fileName;
        _filePath = filePath;
    }

    public override string PreviewItemTitle => _fileName;

    public override NSUrl PreviewItemUrl => NSUrl.FromFilename(_filePath);
}

public class QLPreviewItemBundle : QLPreviewItem
{
    private readonly string _fileName, _filePath;

    public QLPreviewItemBundle(string fileName, string filePath)
    {
        _fileName = fileName;
        _filePath = filePath;
    }

    public override string PreviewItemTitle => _fileName;

    public override NSUrl PreviewItemUrl
    {
        get
        {
            var documents = NSBundle.MainBundle.BundlePath;
            var lib = Path.Combine(documents, _filePath);
            var url = NSUrl.FromFilename(lib);
            return url;
        }
    }
}