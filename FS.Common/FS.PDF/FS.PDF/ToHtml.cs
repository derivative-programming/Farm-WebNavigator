using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp;
using PdfSharp.Pdf;

namespace FS.PDF
{
    public class ToHtml
    {
        public static async void Process(string sourceHTMLFilePath, string destinationPDFFilePath)
        {
            var htmlData = await System.IO.File.ReadAllTextAsync(sourceHTMLFilePath);
			PdfDocument pdf = PdfGenerator.GeneratePdf(htmlData, PageSize.A4);
			pdf.Save(destinationPDFFilePath);
		}

         
    }
}
