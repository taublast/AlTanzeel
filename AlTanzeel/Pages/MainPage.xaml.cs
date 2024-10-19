using AlTanzeel.ViewModel;
using IOSServices;
using QuranParser;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using PointF = Syncfusion.Drawing.PointF;

namespace AlTanzeel;

public partial class MainPage
{
    private MainViewModel vm;
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        //Create a new PDF document.
        var document = new PdfDocument();
        //Add a page to the document.
        var page = document.Pages.Add();
        //Create PDF graphics for the page.
        var graphics = page.Graphics;
        //Set the standard font.
        PdfFont font = new PdfTrueTypeFont("pdms-saleem-quranfont.ttf", 20);
        //Create PDF string format.
        var format = new PdfStringFormat
        {
            //Set text alignement.
            Alignment = PdfTextAlignment.Left,
            ComplexScript = true
        };
        //Draw the text.
        graphics.DrawString("الحمدالله", font, PdfBrushes.Black, new PointF(0, 0), format);

        // foreach (var aya in vm.SelectedAyasForTranslation)
        // {
        //     graphics.DrawString("الحمدالله", font, PdfBrushes.Black, new PointF(0, 0), format);
        //     break;
        // }
        using MemoryStream ms = new();
        //Save the PDF document to MemoryStream.
        document.Save(ms);
        //Close the PDF document.
        document.Close(true);
        ms.Position = 0;
        //Saves the memory stream as file using the SaveService instance.
        var save = new SaveService();
        save.SaveAndView("output.pdf", "application/pdf", ms);
    }
}