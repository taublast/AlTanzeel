using QuickLook;
using UIKit;

namespace CreatePdfDemoSample.Services;

public partial class SaveService
{
    public partial void SaveAndView(string filename, string contentType, MemoryStream stream)
    {
        var exception = string.Empty;
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var filePath = Path.Combine(path, filename);
        try
        {
            var fileStream = File.Open(filePath, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();
        }
        catch (Exception e)
        {
            exception = e.ToString();
        }

        if (contentType != "application/html" || exception == string.Empty)
        {
            var window = GetKeyWindow();
            if (window != null && window.RootViewController != null)
            {
                var uiViewController = window.RootViewController;
                if (uiViewController != null)
                {
                    QLPreviewController qlPreview = new();
                    QLPreviewItem item = new QLPreviewItemBundle(filename, filePath);
                    qlPreview.DataSource = new PreviewControllerDS(item);
                    uiViewController.PresentViewController(qlPreview, true, null);
                }
            }
        }
    }

    public UIWindow? GetKeyWindow()
    {
        foreach (var scene in UIApplication.SharedApplication.ConnectedScenes)
            if (scene is UIWindowScene windowScene)
                foreach (var window in windowScene.Windows)
                    if (window.IsKeyWindow)
                        return window;

        return null;
    }
}