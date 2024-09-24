using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.PDF
{
    public class OCR
    {
        public static void Process(string sourcePdfFilePath, string destinationtxtFilePath, bool hidePageStartComment = false)
        {
            if (System.IO.File.Exists(destinationtxtFilePath))
            {
                System.IO.File.Delete(destinationtxtFilePath);
            }

            PdfReader reader = new PdfReader(sourcePdfFilePath); 
            iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                 
                string pageText = GetPageText(page, i - 1);

                if (!hidePageStartComment)
                {
                    System.IO.File.AppendAllText(destinationtxtFilePath, "Page " + i.ToString() + "..." + Environment.NewLine);
                }
                System.IO.File.AppendAllText(destinationtxtFilePath, pageText + Environment.NewLine); 
            }

            pdfDoc.Close(); 
        }


        static string GetPageText(PdfPage page, int pageIndex)
        { 

            return PdfTextExtractor.GetTextFromPage(page); 
        }
    }
}
