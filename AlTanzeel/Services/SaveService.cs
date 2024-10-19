using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSampleMaUI.Services
{
    public abstract partial class SaveService
    {
            public abstract void SaveAndView(string filename, string contentType, MemoryStream stream);
    }
}
