using AlTanzeel.ViewModel;
using DrawnUi.Maui.Draw;
using DrawnUi.Maui.Infrastructure;
using SkiaSharp;

namespace AlTanzeel.Pages;

public partial class MainPage
{
    private string _BindableText;
    private bool _lockLogs;
    private string TheOpeningText = "بِسۡمِ ٱللَّهِ ٱلرَّحۡمَٰنِ ٱلرَّحِيمِ";

    private MainViewModel vm;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.StorageWrite>();

        if (status == PermissionStatus.Granted)
            CreatePdf((float)595.2); // A4 page for 150 DPI
        else
            // Handle the case where permission is denied
            Console.WriteLine("Permission denied to access storage.");
    }

    private string GenerateFileName(DateTime timeStamp, string extension)
    {
        var filename = $"DrawnUi_{timeStamp:yyyy-MM-dd_HHmmss}.{extension}";

        return filename;
    }

    private async Task CreatePdf(float width)
    {
        var vendor = "DrawnUI";
        var filename = GenerateFileName(DateTime.Now, "pdf");

        var layout = new QuizReport(viewModel: vm)
        {
            BindingContext = vm
        };

        Files.CheckPermissionsAsync(async () =>
        {
            try
            {
                _lockLogs = true;
                string fullFilename = null;
                var subfolder = "Pdf";
                var scale = 1; // Keep scale constant
                var destination = new SKRect(0, 0, width, float.PositiveInfinity);

                // Measure the layout
                var measured = layout.Measure(destination.Width, destination.Height, scale);
                var totalHeight = measured.Units.Height;
                var pageHeight = 841.89f; // A4 height in points
                var pageCount = (int)Math.Ceiling(totalHeight / pageHeight);

                fullFilename = Files.GetFullFilename(filename, StorageType.Cache, subfolder);

                if (File.Exists(fullFilename)) File.Delete(fullFilename);

                using (var ms = new MemoryStream())
                using (var stream = new SKManagedWStream(ms))
                {
                    using (var document = SKDocument.CreatePdf(stream, new SKDocumentPdfMetadata
                    {
                        Author = vendor,
                        Producer = vendor,
                        Subject = Title
                    }))
                    {
                        for (int i = 0; i < pageCount; i++)
                        {
                            var currentHeight = i * pageHeight;
                            var nextHeight = Math.Min(totalHeight, currentHeight + pageHeight);

                            using (var canvas = document.BeginPage(width, pageHeight))
                            {
                                var ctx = new SkiaDrawingContext
                                {
                                    Canvas = canvas,
                                    Width = width,
                                    Height = pageHeight
                                };

                                // Adjust visible area for the current page
                                var visibleArea = new SKRect(0, currentHeight, width, nextHeight);

                                // Render only the visible portion
                                layout.Render(ctx, visibleArea, scale);
                            }

                            document.EndPage();
                        }

                        document.Close();
                    }

                    ms.Position = 0;
                    var content = ms.ToArray();

                    var file = Files.OpenFile(fullFilename, StorageType.Cache, subfolder);

                    await file.Handler.WriteAsync(content, 0, content.Length);
                    await file.Handler.FlushAsync();

                    Files.CloseFile(file, true);
                    await Task.Delay(500);
                }

                Files.Share("PDF", new[] { fullFilename });
            }
            catch (Exception e)
            {
                Super.Log(e);
            }
            finally
            {
                _lockLogs = false;
            }
        });
    }

}