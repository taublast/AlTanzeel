﻿using AlTanzeel.ViewModel;
using DrawnUi.Maui.Draw;
using DrawnUi.Maui.Infrastructure;
using SkiaSharp;

namespace AlTanzeel;

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

    public string BindableText
    {
        get => _BindableText;
        set
        {
            if (_BindableText != value)
            {
                _BindableText = value;
                OnPropertyChanged();
            }
        }
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
        //setup our report to print
        BindableText = "This text came from bindings";
        var vendor = "DrawnUI";
        var filename = GenerateFileName(DateTime.Now, "pdf");

        var layout = new QuizReport
        {
            BindingContext = this //whatever you want, you can have bindings inside your report
        };

        //render and share
        Files.CheckPermissionsAsync(async () =>
        {
            try
            {
                _lockLogs = true;
                string fullFilename = null;
                var subfolder = "Pdf";
                var scale = 1; //do not change this
                var destination = new SKRect(0, 0, width, float.PositiveInfinity);
                var measured = layout.Measure(destination.Width, destination.Height, scale);

                //prepare DrawingRect
                layout.Arrange(new SKRect(0, 0, layout.MeasuredSize.Pixels.Width, layout.MeasuredSize.Pixels.Height),
                    layout.MeasuredSize.Pixels.Width, layout.MeasuredSize.Pixels.Height, scale);

                var reportSize = new SKSize(measured.Units.Width, measured.Units.Height);

                //we need a local file to ba saved in order to share it
                fullFilename = Files.GetFullFilename(filename, StorageType.Cache, subfolder);

                if (File.Exists(fullFilename)) File.Delete(fullFilename);

                var area = new SKRect(layout.DrawingRect.Left, layout.DrawingRect.Top, reportSize.Width,
                    reportSize.Height);

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
                        using (var canvas = document.BeginPage(reportSize.Width, reportSize.Height))
                        {
                            var ctx = new SkiaDrawingContext
                            {
                                Canvas = canvas,
                                Width = reportSize.Width,
                                Height = reportSize.Height
                            };

                            //with no async stuff this is enough for most cases
                            layout.Render(ctx, new SKRect(0, 0, reportSize.Width, reportSize.Height), scale);
                        }

                        document.EndPage();
                        document.Close();
                    }

                    ms.Position = 0;
                    var content = ms.ToArray();

                    var file = Files.OpenFile(fullFilename, StorageType.Cache, subfolder);

                    // Write the bytes to the FileStream of the FileDescriptor
                    await file.Handler.WriteAsync(content, 0, content.Length);

                    // Ensure all bytes are written to the underlying device
                    await file.Handler.FlushAsync();

                    Files.CloseFile(file, true);
                    await Task.Delay(500);
                }

                //can share the file now
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