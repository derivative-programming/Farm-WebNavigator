using System.Drawing;
using System.Reflection.PortableExecutable;
using System;
using System.IO; 
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.IO.Image;
using System.Drawing.Imaging;
using PdfiumViewer;
using System.Runtime.Intrinsics.Arm;
using static iText.Kernel.Pdf.Colorspace.PdfDeviceCs;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace FS.PDF
{
    public class Orientation
    {
        public static void SyncPageOrientation(string sourcePdfFilePath, string destinationPdfFilePath)
        {
              

            PdfReader reader = new PdfReader(sourcePdfFilePath);
            PdfWriter writer = new PdfWriter(destinationPdfFilePath);
            iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader, writer);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);

                // Determine if this page needs to be rotated
                // This part of the logic is up to you to implement
                bool needsRotation = ShouldRotatePage(page, sourcePdfFilePath, i - 1);

                if (needsRotation)
                {
                    // Rotate the page by 90 degrees
                    page.SetRotation(90);
                }
            }

            pdfDoc.Close(); // Save the changes
        }


        static bool ShouldRotatePage(PdfPage page, string pdfPath, int pageIndex)
        {
            // Implement your logic to determine if the page should be rotated
            // For example, you might check the size or existing rotation of the page
            // Return true if the page needs rotation, false otherwise

            string text = PdfTextExtractor.GetTextFromPage(page);
            TextOrientation textOrientation = AnalyzeTextOrientation(
                ConvertPageToImage(pdfPath, pageIndex));
            if (text.Length == 0)
                return true;
            return false;
        }

         
        private static Image ConvertPageToImage(string sourcePdfFilePath, int pageNumber)
        {
            string imagePath = Path.GetTempFileName(); 
            imagePath = Path.ChangeExtension(imagePath, ".png");
            // Implementation depends on the library used
            using (var document = PdfiumViewer.PdfDocument.Load(sourcePdfFilePath))
            {
                var dpi = 300;
                return document.Render(pageNumber, dpi, dpi, PdfRenderFlags.Annotations);
            } 
        } 


        private static TextOrientation AnalyzeTextOrientation(Image systemDrawingImage)
        {
            using (Bitmap bitmap = ConvertToFormat(systemDrawingImage, PixelFormat.Format32bppRgb))
            {
                using (Image<Bgr, byte> image = BitmapToImage(bitmap))// new Image<Bgr, byte>(bitmap))
                {
                    //using (Image<Emgu.CV.Structure.Gray, byte> gray = image.Convert<Emgu.CV.Structure.Gray, byte>())
                    //using (Image<Emgu.CV.Structure.Gray, byte> edges = gray.Canny(100, 200))
                    ////using (VectorOfVec4f lines = new VectorOfVec4f())
                    ////{
                    ////    //CvInvoke.HoughLinesP(edges, lines, 1, Math.PI / 180, threshold, minLineLength, maxLineGap);
                    ////    CvInvoke.HoughLinesP(edges, lines, 1, Math.PI / 180, 50, 30, 10);

                    ////    int horizontalLines = 0;
                    ////    int verticalLines = 0;

                    ////    foreach (PointF line in lines.ToArray())
                    ////    {
                    ////        float angle = CalculateAngle(line);
                    ////        if (IsHorizontal(angle)) horizontalLines++;
                    ////        if (IsVertical(angle)) verticalLines++;
                    ////    }

                    ////    return horizontalLines > verticalLines ? TextOrientation.Normal : TextOrientation.Sideways;
                    ////}
                    //using (VectorOfFloat lines = new VectorOfFloat())
                    //{
                    //    CvInvoke.HoughLinesP(edges, lines, 1, Math.PI / 180, 50, 30, 10);
                    //    float[] lineArray = lines.ToArray();

                    //    int horizontalLines = 0;
                    //    int verticalLines = 0;

                    //    for (int i = 0; i < lineArray.Length; i += 4)
                    //    {
                    //        PointF start = new PointF(lineArray[i], lineArray[i + 1]);
                    //        PointF end = new PointF(lineArray[i + 2], lineArray[i + 3]);
                    //        float angle = CalculateAngle(start, end);

                    //        if (IsHorizontal(angle)) horizontalLines++;
                    //        if (IsVertical(angle)) verticalLines++;
                    //    }

                    //    return horizontalLines > verticalLines ? TextOrientation.Normal : TextOrientation.Sideways;
                    //}
                    int horizontal = 0;
                    int vertical = 0;

                    using (Image<Emgu.CV.Structure.Gray, byte> gray = image.Convert<Emgu.CV.Structure.Gray, byte>())
                    using (Image<Emgu.CV.Structure.Gray, byte> thresh = gray.ThresholdBinary(new Emgu.CV.Structure.Gray(120), new Emgu.CV.Structure.Gray(255)))
                    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                    {
                        CvInvoke.FindContours(thresh, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                        for (int i = 0; i < contours.Size; i++)
                        {
                            using (VectorOfPoint contour = contours[i])
                            {
                                Rectangle rect = CvInvoke.BoundingRectangle(contour);
                                //if (rect.Width * rect.Height > 10)
                                //    horizontal = horizontal;
                                if (rect.Width == rect.Height)
                                    continue;
                                if(Math.Abs(rect.Width - rect.Height) < 5)
                                {
                                    //these are probably letters
                                    if (rect.Width > rect.Height)
                                    {
                                        vertical++;
                                    }
                                    else
                                    {
                                        horizontal++;
                                    }
                                }
                                else
                                {
                                    //if (rect.Width == image.Width &&
                                    //    rect.Height == image.Height)
                                    //    continue;
                                    //if (rect.Width > rect.Height)
                                    //    horizontal = horizontal + (rect.Height * rect.Width);
                                    //else
                                    //    vertical = vertical + (rect.Height * rect.Width);
                                }
                            }
                        }
                    }

                    return horizontal > vertical ? TextOrientation.Normal : TextOrientation.Sideways;

                }
            }
            return TextOrientation.Normal;
        }
        static float CalculateAngle(PointF start, PointF end)
        {
            // Calculate the angle of the line
            return (float)(Math.Atan2(end.Y - start.Y, end.X - start.X) * (180 / Math.PI));
        }
        //static void AnalyzeLineOrientations(VectorOfPointF lines)
        //{
        //    int verticalCount = 0;
        //    int horizontalCount = 0;

        //    foreach (PointF line in lines.ToArray())
        //    {
        //        float angle = CalculateLineAngle(line);
        //        if (IsVertical(angle))
        //            verticalCount++;
        //        else
        //            horizontalCount++;
        //    }

        //    if (verticalCount > horizontalCount)
        //        Console.WriteLine("Text is predominantly vertical.");
        //    else
        //        Console.WriteLine("Text is predominantly horizontal.");
        //}

        static float CalculateAngle(PointF line)
        {
            // Calculate the angle of the line
            return (float)(Math.Atan2(line.Y - line.X, 1) * (180 / Math.PI));
        }

        static bool IsHorizontal(float angle)
        {
            // Define what range of angles you consider to be horizontal
            return Math.Abs(angle) < 30 || Math.Abs(angle) > 150;
        }

        static bool IsVertical(float angle)
        {
            // Define what range of angles you consider to be vertical
            return Math.Abs(angle) > 60 && Math.Abs(angle) < 120;
        }

        //static float CalculateLineAngle(PointF line)
        //{
        //    float dx = line.X - line.Y;
        //    float dy = line.Z - line.W;
        //    float angle = (float)(Math.Atan2(dy, dx) * (180.0 / Math.PI));
        //    return angle;
        //}

        //static bool IsVertical(float angle)
        //{
        //    // Adjust the angle threshold as needed
        //    return Math.Abs(angle) > 45 && Math.Abs(angle) < 135;
        //}

        static Bitmap ConvertToFormat(Image image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }

            return copy;
        }
        private static Image<Bgr, byte> BitmapToImage(Bitmap bitmap)
        {
            // Lock the bitmap's bits
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            // Create an Emgu CV Image from the bitmap data
            Image<Bgr, byte> image = new Image<Bgr, byte>(bitmap.Width, bitmap.Height, bmpData.Stride, bmpData.Scan0);

            // Unlock the bits
            bitmap.UnlockBits(bmpData);

            return image;
        }

        // Enum to represent text orientation
        enum TextOrientation
        {
            Normal,
            Sideways
        }
    }
}