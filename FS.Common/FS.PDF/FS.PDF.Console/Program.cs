// See https://aka.ms/new-console-template for more information 

var files = System.IO.Directory.GetFiles(@"C:\VR\MyDocs\Projects\rob_medical_debt\sample_medical_debt_case_2", "*.pdf", SearchOption.AllDirectories);
foreach (var file in files)
{
    try
    {
        FS.PDF.OCR.Process(file, file + ".ocr.txt");
    }
    catch { }
}

FS.PDF.OCR.Process(@"C:\VR\MyDocs\Projects\rob_medical_debt\sample_medical_debt_case_2\ocr-test-input.pdf",
    @"C:\VR\MyDocs\Projects\rob_medical_debt\sample_medical_debt_case_2\ocr-test-output.txt");


FS.PDF.Orientation.SyncPageOrientation(@"C:\VR\MyDocs\Projects\rob_medical_debt\sample_medical_debt_case_2\orientation-test-input.pdf",
    @"C:\VR\MyDocs\Projects\rob_medical_debt\sample_medical_debt_case_2\orientation-test-output.pdf");
