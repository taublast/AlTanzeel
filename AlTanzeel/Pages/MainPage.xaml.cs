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
            CreatePdfPages(Pdf.GetPaperSizeInInches(PaperFormat.A4), 150); // A4 page for 150 DPI
        else
            // Handle the case where permission is denied
            Console.WriteLine("Permission denied to access storage.");
    }

    private string GenerateFileName(DateTime timeStamp, string extension)
    {
        var filename = $"DrawnUi_{timeStamp:yyyy-MM-dd_HHmmss}.{extension}";

        return filename;
    }

    async Task CreatePdfPages(SKSize inches, int dpi)
    {
        //setup our report to print
        var paper = Pdf.GetPaperSizePixels(inches, dpi);
        var vendor = "DrawnUI";
        var filename = GenerateFileName(DateTime.Now, "pdf");

        //introduce page margins
        var margins = 0.1f * dpi; //add margins 0.1 inches, change this as you wish
        var pageSizeAccountMargins = new SKSize(paper.Width - margins * 2, paper.Height - margins * 2);

        //====================
        // Create our report to be printed
        //====================
        var content = new QuizReport(viewModel: vm)
        {
            HorizontalOptions = LayoutOptions.Fill,
            BindingContext = vm
        };

        //====================
        // Create wrappers for output
        //====================
        var ctx = content.BindingContext;
        SkiaScroll viewport = null; //to scroll though visible parts for each page
        SkiaLayout wrapper = new() //need wrapper for margins, will use padding
        {
            //Background = Colors.Red, //debug margins
            BindingContext = ctx,
            Padding = new(margins),
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
            Children = new List<SkiaControl>()
                {
                    new SkiaScroll()
                    {
                        Tag = "PdfScroll",
                        BackgroundColor = Colors.White,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalOptions = LayoutOptions.Fill,
                        Content = content,
                    }.With((c) =>
                    {
                        viewport = c;
                    })},
        };

        //====================
        // Render and share
        //====================
        Files.CheckPermissionsAsync(async () =>
         {

             //in this example we will split a large content into pages
             //when we do not for on a single page format
             try
             {
                 _lockLogs = true;
                 string fullFilename = null;
                 var subfolder = "Pdf";

                 //====================
                 //STEP 1: Measure content
                 //====================
                 var scale = 1; //do not change this

                 wrapper.Measure(paper.Width, paper.Height, scale);
                 wrapper.Arrange(new SKRect(0, 0, paper.Width, paper.Height),
                     wrapper.MeasuredSize.Pixels.Width, wrapper.MeasuredSize.Pixels.Height, scale);

                 var contentSize = new SKSize(content.MeasuredSize.Units.Width, content.MeasuredSize.Units.Height);

                 //====================
                 //STEP 2: Render pages
                 //====================
                 var pages = Pdf.SplitToPages(contentSize, pageSizeAccountMargins);

                 //we need a local file to ba saved in order to share it
                 fullFilename = Files.GetFullFilename(filename, StorageType.Cache, subfolder);

                 if (File.Exists(fullFilename))
                 {
                     File.Delete(fullFilename);
                 }

                 using (var ms = new MemoryStream())
                 using (var stream = new SKManagedWStream(ms))
                 {
                     using (var document = SKDocument.CreatePdf(stream, new SKDocumentPdfMetadata
                     {
                         Author = vendor,
                         Producer = vendor,
                         Subject = this.Title
                     }))
                     {
                         foreach (PdfPagePosition page in pages)
                         {
                             viewport.ViewportOffsetY = -page.Position.Y;

                             using (var canvas = document.BeginPage(paper.Width, paper.Height))
                             {
                                 var ctx = new SkiaDrawingContext()
                                 {
                                     Canvas = canvas,
                                     Width = paper.Width,
                                     Height = paper.Height
                                 };

                                 //first rendering to launch loading images and first layout
                                 wrapper.Render(ctx, new SKRect(0, 0, paper.Width, paper.Height), scale);

                                 //in our specific case we have images inside that load async,
                                 //so wait for them and render final result
                                 
                                 //second rendering required to reflect layout changes and async images are loaded
                                 canvas.Clear(SKColors.White); //non-transparent reserves space inside pdf
                                 wrapper.Render(ctx, new SKRect(0, 0, paper.Width, paper.Height), scale);
                             }
                             document.EndPage();
                         }

                         document.Close();
                     }

                     ms.Position = 0;
                     var bytes = ms.ToArray();

                     var file = Files.OpenFile(fullFilename, StorageType.Cache, subfolder);

                     // Write the bytes to the FileStream of the FileDescriptor
                     await file.Handler.WriteAsync(bytes, 0, bytes.Length);

                     // Ensure all bytes are written to the underlying device
                     await file.Handler.FlushAsync();

                     Files.CloseFile(file, true);
                     await Task.Delay(500); //we need this for slow file system
                 }
                 
                 Console.WriteLine($"TAKASUR:: {fullFilename}");
                 //can share the file now
                 Files.Share("PDF", new string[] { fullFilename });
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