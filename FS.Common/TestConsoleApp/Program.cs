using System;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            System.Collections.Generic.List<string> files = ND.Common.Compression.Compression.DecompressZipFileReturnList(@"C:\Users\vince\AppData\Local\Temp\7a2d2ecb-30de-4cdf-a1dd-979c57bef914\LS-313.zip");
        }
    }
}
