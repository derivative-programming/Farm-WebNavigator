using System;
using System.IO;
using System.Text;
using System.Collections;

namespace FS.Common.IO
{
    /// <summary>
    /// TextFieldParser - Removes 'field' values from fixed width or comma delimited records.
    /// </summary>
    /// <remarks>Note the following improvement requests.
    /// <list type="number">
    /// <item>
    /// <code>Submit mapping files.</code>
    /// <description>Allow type to receive configuration mappings for field assignments.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class TextFieldParser
    {

        // ------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------
        // TODO : Enhancement List
        //	- Receive configuration mappings for field assignments.
        // ------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------

        #region Enumerations

        public enum FileFormat
        {
            FixedWidth,
            Delimited,
            FixedRecordAndColumnWidth,
            DelimitedRecordDelimitedColumns,
            DelimitedRecordFixedColumns,
            FixedWidthColumnsVariableNumber
        }

        #endregion

        #region Delegates and Events

        public delegate void RecordFoundHandler(object source, RecordEventArgs e);
        public event RecordFoundHandler RecordFound;

        #endregion

        #region Private Fields

        private FileFormat _fileType = FileFormat.Delimited;
        private string _fileName = string.Empty;
        private TextFieldCollection _textFields;
        private char _delimiter = Convert.ToChar(",");
        private char _quoteChar = '"';
        private bool _hasHeaderRecord;
        private bool _cancelledOperation = false;
        private int _textFieldRecordSpan = 1;
        private int _recordLength = 0;
        private string _currentRecordRawData = string.Empty;
        private int _fileStartPosition = 0;
        private int _invalidInitalFileRecordCount = 0;

        private System.Int32 _currentLineNumber = 0;
        private System.Int32 _currentTextFieldRecordNumber = 0;
        private string _textFieldRecordSpanDelimiter = string.Empty;

        #endregion

        #region Constructors

        public TextFieldParser()
            : this(string.Empty, FileFormat.FixedWidth, false)
        {
        }

        public TextFieldParser(string fileName)
            : this(fileName, FileFormat.FixedWidth, false)
        {
        }

        public TextFieldParser(FileFormat fileType)
            : this(string.Empty, FileFormat.FixedWidth, false)
        {
        }

        public TextFieldParser(string fileName, FileFormat fileType, bool hasHeader)
        {
            _fileName = fileName;
            _fileType = fileType;
            _hasHeaderRecord = hasHeader;
            _textFields = new TextFieldCollection();
        }

        #endregion

        #region Public Properties

        public int FileStartPosition
        {
            get { return _fileStartPosition; }
            set { _fileStartPosition = value; }
        }
        public int InvalidInitalFileRecordCount
        {
            get { return _invalidInitalFileRecordCount; }
            set { _invalidInitalFileRecordCount = value; }
        }
        public string CurrentRecordRawData
        {
            get { return _currentRecordRawData; }
            set { _currentRecordRawData = value; }
        }
        public int TextFieldRecordSpan
        {
            get { return _textFieldRecordSpan; }
            set { _textFieldRecordSpan = value; }
        }
        public string TextFieldRecordSpanDelimiter
        {
            get { return _textFieldRecordSpanDelimiter; }
            set { _textFieldRecordSpanDelimiter = value; }
        }
        public int RecordLength
        {
            get { return _recordLength; }
            set { _recordLength = value; }
        }
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public FileFormat FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        public char Delimiter
        {
            get { return Convert.ToChar(_delimiter); }
            set
            {
                if ((value.ToString().Length == 0 & _fileType == FileFormat.Delimited) || (value.ToString().Length == 0 & _fileType == FileFormat.DelimitedRecordDelimitedColumns))
                {
                    throw new ApplicationException("You must specify a Delimiter when the FileType is 'FileFormat.Delimite'");
                }
                _delimiter = value;
            }
        }

        public TextFieldCollection TextFields
        {
            get { return _textFields; }
        }

        public System.Int32 CurrentLineNumber
        {
            get { return _currentLineNumber; }
            set
            {
                if (value < _currentLineNumber)
                {
                    throw new ApplicationException("You can not decriment the CurrentLineNumber.");
                }
            }
        }
        public System.Int32 CurrentTextFieldRecordNumber
        {
            get { return _currentTextFieldRecordNumber; }
        }

        public char QuoteCharacter
        {
            get { return _quoteChar; }
            set { _quoteChar = value; }
        }

        public bool HasHeaderRecord
        {
            get { return _hasHeaderRecord; }
            set { _hasHeaderRecord = value; }
        }

        public bool CancelledOperation
        {
            get { return _cancelledOperation; }
            set { _cancelledOperation = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetTypeCode : Seaches and returns System typecode value type based on field type assignment.
        /// </summary>
        /// <param name="typeCode">Search for this type code.</param>
        /// <returns>System type code value type.</returns>
        /// <remarks>The string value matches the enumerated types defined in the mapping definition's schema layout.</remarks>
        public System.TypeCode GetTypeCode(string typeCode)
        {

            System.TypeCode assignedType;

            if (typeCode == null || typeCode.Length == 0)
            {
                throw new ArgumentNullException("Null or invalid reference to type code identifier. Verify client mapping defintions.");
            }

            switch (typeCode)
            {

                case "Boolean":
                    assignedType = TypeCode.Boolean;
                    break;

                case "Byte":
                    assignedType = TypeCode.Byte;
                    break;

                case "Char":
                    assignedType = TypeCode.Char;
                    break;

                case "DateTime":
                    assignedType = TypeCode.DateTime;
                    break;

                case "DBNull":
                    assignedType = TypeCode.DBNull;
                    break;

                case "Decimal":
                    assignedType = TypeCode.Decimal;
                    break;

                case "Double":
                    assignedType = TypeCode.Double;
                    break;

                case "Empty":
                    assignedType = TypeCode.Empty;
                    break;

                case "Int16":
                    assignedType = TypeCode.Int16;
                    break;

                case "Int32":
                    assignedType = TypeCode.Int32;
                    break;

                case "Int64":
                    assignedType = TypeCode.Int64;
                    break;

                case "Object":
                    assignedType = TypeCode.Object;
                    break;

                case "SByte":
                    assignedType = TypeCode.SByte;
                    break;

                case "Single":
                    assignedType = TypeCode.Single;
                    break;

                case "String":
                    assignedType = TypeCode.String;
                    break;

                case "UInt16":
                    assignedType = TypeCode.UInt16;
                    break;

                case "UInt32":
                    assignedType = TypeCode.UInt32;
                    break;

                case "UInt64":
                    assignedType = TypeCode.UInt64;
                    break;

                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException("Invalid type code from mapping definition. Verify client assigned data types.");
            }

            return assignedType;

        }


        /// <summary>
        /// GetRecordCount - Read file to return number of records.
        /// </summary>
        /// <param name="fileName">File to parse.</param>
        /// <exception cref="ArgumentNullException">Thrown when file name is null or empty.</exception>
        /// <returns>Number of records in the file</returns>
        public System.Int32 GetRecordCount(string fileName)
        {

            StreamReader reader = null;
            System.Int32 lineNumber;
            string fileRecord = string.Empty;

            try
            {

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // verify file name
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                if (fileName == null || fileName.Length == 0)
                {
                    throw new ArgumentNullException("Invalid file name when parsing the file.");
                }

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // Set property
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                this.FileName = fileName;

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // Load file
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                reader = new StreamReader(fileName);

                lineNumber = 0;

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // Loop through file
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                while (reader.Peek() != -1)
                {
                    fileRecord = reader.ReadLine();
                    lineNumber++;
                }

                return lineNumber;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (reader != null)
                {
                    reader.Close();
                }

            }

        }

        public System.Int64 GetRecordCount2(string fileName)
        {
            if (fileName == null || fileName.Length == 0)
            {
                throw new ArgumentNullException("Invalid file name when parsing the file.");
            }
            if (this.TextFieldRecordSpan == 0)
            {
                throw new ArgumentNullException("TextFieldRecordSpan must be greater than zero.");
            }
            switch (this.FileType)
            {
                case FileFormat.Delimited:
                    return GetRecordCountNewLineRecord(fileName);
                case FileFormat.DelimitedRecordDelimitedColumns:
                    return GetRecordCountDelimitedRecord(fileName);
                case FileFormat.DelimitedRecordFixedColumns:
                    return GetRecordCountDelimitedRecord(fileName);
                case FileFormat.FixedRecordAndColumnWidth:
                    return GetRecordCountFixedLengthRecord(fileName);
                case FileFormat.FixedWidth:
                    return GetRecordCountNewLineRecord(fileName);
                case FileFormat.FixedWidthColumnsVariableNumber:
                    return GetRecordCountNewLineRecord(fileName);
                default:
                    throw new System.Exception("Invalid file type.");
            }
        }


        private System.Int64 GetRecordCountNewLineRecord(string fileName)
        {

            StreamReader reader = null;
            System.Int64 lineNumber;
            string fileRecord = string.Empty;
            int numberOfCharactersRead = 0;
            char[] buffer = null;

            try
            {
                this.FileName = fileName;
                reader = new StreamReader(fileName);
                if (this.FileStartPosition > 1)
                {
                    buffer = new char[1];
                    for (int k = 0; k < this.FileStartPosition - 1; k++)
                    {
                        if (reader.Peek() != -1)
                        {
                            numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        }
                    }
                }
                if (this.InvalidInitalFileRecordCount != 0)
                {
                    for (int j = 0; j < this.InvalidInitalFileRecordCount; j++)
                    {
                        for (int i = 0; i < this.TextFieldRecordSpan; i++)
                        {
                            if (reader.Peek() != -1)
                            {
                                fileRecord = reader.ReadLine();
                            }
                        }
                    }
                }
                lineNumber = 0;
                while (reader.Peek() != -1)
                {
                    for (int i = 0; i < this.TextFieldRecordSpan; i++)
                    {
                        if (reader.Peek() != -1)
                        {
                            fileRecord = reader.ReadLine();
                        }
                    }
                    lineNumber++;
                }
                return lineNumber;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }
        private System.Int64 GetRecordCountDelimitedRecord(string fileName)
        {

            StreamReader reader = null;
            System.Int64 lineNumber;
            string fileRecord = string.Empty;
            int numberOfCharactersRead = 0;
            char[] buffer = null;
            StringBuilder newRecord = null;

            try
            {
                if (this.TextFieldRecordSpanDelimiter.Length == 0)
                {
                    throw new System.Exception("Invalid record delimiter [FileFormat:=DelimitedRecord,TextFieldRecordSpanDelimiter:=" + this.TextFieldRecordSpanDelimiter + "].");
                }
                this.FileName = fileName;
                reader = new StreamReader(fileName);
                if (this.FileStartPosition > 1)
                {
                    buffer = new char[1];
                    for (int k = 0; k < this.FileStartPosition - 1; k++)
                    {
                        if (reader.Peek() != -1)
                        {
                            numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        }
                    }
                }
                if (this.InvalidInitalFileRecordCount != 0)
                {
                    if (reader.Peek() != -1)
                    {
                        for (int j = 0; j < this.InvalidInitalFileRecordCount; j++)
                        {
                            buffer = new char[1];
                            newRecord = new StringBuilder();
                            fileRecord = string.Empty;
                            while (fileRecord.EndsWith(this.TextFieldRecordSpanDelimiter) != true)
                            {
                                numberOfCharactersRead = reader.Read(buffer, 0, 1);
                                newRecord.Append(buffer);
                                fileRecord = newRecord.ToString();
                                if (fileRecord == this.TextFieldRecordSpanDelimiter)
                                {
                                    fileRecord = string.Empty;
                                }
                                if (reader.Peek() == -1)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                lineNumber = 0;
                while (reader.Peek() != -1)
                {
                    buffer = new char[1];
                    newRecord = new StringBuilder();
                    fileRecord = string.Empty;
                    while (fileRecord.EndsWith(this.TextFieldRecordSpanDelimiter) != true)
                    {
                        numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        newRecord.Append(buffer);
                        fileRecord = newRecord.ToString();
                        if (fileRecord == this.TextFieldRecordSpanDelimiter)
                        {
                            fileRecord = string.Empty;
                        }
                        if (reader.Peek() == -1)
                        {
                            break;
                        }
                    }
                    lineNumber++;
                }
                return lineNumber;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }
        private System.Int64 GetRecordCountFixedLengthRecord(string fileName)
        {
            System.Int64 fileLength = 0;
            System.Int64 recordLength = 0;
            System.IO.FileInfo fileInfo = null;
            System.Int64 recordCount = 0;

            if (this.RecordLength == 0)
            {
                throw new System.Exception("Invalid record length [FileFormat:=FixedRecordAndColumnWidth,RecordLength:=" + this.RecordLength + "].");
            }
            this.FileName = fileName;
            fileInfo = new System.IO.FileInfo(this.FileName);
            recordLength = this.RecordLength;
            fileLength = fileInfo.Length;
            if (this.FileStartPosition > 1)
            {
                fileLength = fileLength - (this.FileStartPosition - 1);
            }
            fileLength = fileLength - (this.InvalidInitalFileRecordCount * recordLength * this.TextFieldRecordSpan);
            recordCount = fileLength / recordLength;
            if (this.TextFieldRecordSpan > 1)
            {
                recordCount = recordCount / this.TextFieldRecordSpan;
            }
            return recordCount;
        }

        /// <summary>
        /// ParseFile - Parse file.
        /// </summary>
        public void ParseFile()
        {
            this.ParseFile(this.FileName);
        }

        /// <summary>
        /// ParseFile - Overload for Parse file.
        /// </summary>
        /// <param name="fileName">File to parase.</param>
        /// <exception cref="ArgumentNullException">Thrown when file name is null or empty.</exception>
        public void ParseFile(string fileName)
        {

            StreamReader reader = null;
            System.Int32 actualLineNumber = 0;
            string fileRecord = string.Empty;

            try
            {

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // verify file name
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                if (fileName == null || fileName.Length == 0)
                {
                    throw new ArgumentNullException("Invalid file name when parsing the file.");
                }

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // Set property
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                this.FileName = fileName;

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // Load file
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                reader = new StreamReader(fileName, Encoding.UTF8);

                actualLineNumber = 0;

                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                // Loop through file
                // ----------------------------------------------------------------------------------------------
                // ----------------------------------------------------------------------------------------------
                while (reader.Peek() != -1)
                {

                    fileRecord = reader.ReadLine();
                    actualLineNumber++;

                    // ----------------------------------------------------------------------------------------------
                    // ----------------------------------------------------------------------------------------------
                    // Check if there is any header record to ignore which means we miss the first record.
                    // ----------------------------------------------------------------------------------------------
                    // ----------------------------------------------------------------------------------------------
                    if (_hasHeaderRecord)
                    {
                        _hasHeaderRecord = false;
                    }
                    else
                    {
                        if (actualLineNumber >= _currentLineNumber)
                        {
                            this.CurrentLineNumber = actualLineNumber;

                            // ----------------------------------------------------------------------------------------------
                            // ----------------------------------------------------------------------------------------------
                            // Pull in a single record
                            // ----------------------------------------------------------------------------------------------
                            // ----------------------------------------------------------------------------------------------
                            Array fields = this.GetFieldArray(fileRecord);

                            if (fields.Length == _textFields.Count)
                            {

                                try
                                {

                                    // ----------------------------------------------------------------------------------------------
                                    // ----------------------------------------------------------------------------------------------
                                    // Loop through the fields and assign the values to load up a record.
                                    // ----------------------------------------------------------------------------------------------
                                    // ----------------------------------------------------------------------------------------------
                                    for (System.Int32 x = 0; x < _textFields.Count; x++)
                                    {
                                        _textFields[x].Value = fields.GetValue(x);
                                    }

                                    // ----------------------------------------------------------------------------------------------
                                    // ----------------------------------------------------------------------------------------------
                                    // Raise event back to the calling client passing current line number and fields.
                                    // ----------------------------------------------------------------------------------------------
                                    // ----------------------------------------------------------------------------------------------
                                    RecordEventArgs args = new RecordEventArgs(this.CurrentLineNumber, _textFields);
                                    RecordFound(this, args);

                                    // ----------------------------------------------------------------------------------------------
                                    // ----------------------------------------------------------------------------------------------
                                    // Check if client issued cancel request.
                                    // ----------------------------------------------------------------------------------------------
                                    // ----------------------------------------------------------------------------------------------
                                    if (_cancelledOperation)
                                    {
                                        return;
                                    }

                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            else
                            {

                                // ----------------------------------------------------------------------------------------------
                                // ----------------------------------------------------------------------------------------------
                                // Number of fields are not matching between the fields collection and the actual fields in the
                                // input stream.
                                // ----------------------------------------------------------------------------------------------
                                // ----------------------------------------------------------------------------------------------
                                throw new InvalidOperationException("Number of fields in the mapping text does not match the number found in the input stream at line number " + actualLineNumber.ToString() + ".");

                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception occurred on line number " + actualLineNumber.ToString() + ".", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }

        public void ParseFile2(string fileName)
        {
            if (fileName == null || fileName.Length == 0)
            {
                throw new ArgumentNullException("Invalid file name when parsing the file.");
            }
            if (this.TextFieldRecordSpan == 0)
            {
                throw new ArgumentNullException("TextFieldRecordSpan must be greater than zero.");
            }
            switch (this.FileType)
            {
                case FileFormat.Delimited:
                    ParseFileNewLineRecord(fileName);
                    break;
                case FileFormat.DelimitedRecordDelimitedColumns:
                    ParseFileDelimitedRecord(fileName);
                    break;
                case FileFormat.DelimitedRecordFixedColumns:
                    ParseFileDelimitedRecord(fileName);
                    break;
                case FileFormat.FixedRecordAndColumnWidth:
                    ParseFileFixedLengthRecord(fileName);
                    break;
                case FileFormat.FixedWidth:
                    ParseFileNewLineRecord(fileName);
                    break;
                case FileFormat.FixedWidthColumnsVariableNumber:
                    ParseFileNewLineRecord(fileName);
                    break;
                default:
                    throw new System.Exception("Invalid file type.");
            }
        }

        private void ParseFileNewLineRecord(string fileName)
        {
            StreamReader reader = null;
            System.Int32 actualLineNumber;
            string fileRecord = string.Empty;
            char[] buffer = null;
            int numberOfCharactersRead = 0;
            ArrayList fileRecordArrayList = null;

            try
            {
                fileRecordArrayList = new ArrayList();
                this.CurrentRecordRawData = string.Empty;
                this.FileName = fileName;
                reader = new StreamReader(fileName);
                actualLineNumber = 0;
                if (this.FileStartPosition > 1)
                {
                    buffer = new char[1];
                    for (int k = 0; k < this.FileStartPosition - 1; k++)
                    {
                        if (reader.Peek() != -1)
                        {
                            numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        }
                    }
                }
                if (this.InvalidInitalFileRecordCount != 0)
                {
                    for (int j = 0; j < this.InvalidInitalFileRecordCount; j++)
                    {
                        for (int l = 0; l < this.TextFieldRecordSpan; l++)
                        {
                            if (reader.Peek() != -1)
                            {
                                fileRecord = reader.ReadLine();
                            }
                        }
                    }
                }
                while (reader.Peek() != -1)
                {
                    this.CurrentRecordRawData = string.Empty;
                    fileRecordArrayList.Clear();
                    for (int i = 0; i < this.TextFieldRecordSpan; i++)
                    {
                        fileRecord = string.Empty;
                        fileRecord = reader.ReadLine();
                        actualLineNumber++;
                        fileRecordArrayList.Add(fileRecord);
                    }
                    if (_hasHeaderRecord)
                    {
                        _hasHeaderRecord = false;
                    }
                    else
                    {
                        if (actualLineNumber >= _currentLineNumber)
                        {
                            this._currentLineNumber = actualLineNumber;
                            this._currentTextFieldRecordNumber++;
                            Array fields = this.GetFieldArray(fileRecordArrayList);
                            //							if(fields.Length==_textFields.Count || this.FileType == FileFormat.DelimitedRecordDelimitedColumns || this.FileType == FileFormat.DelimitedRecordFixedColumns) 
                            //							{
                            //								try 
                            //								{
                            for (System.Int32 x = 0; x < _textFields.Count; x++)
                            {
                                if (x < fields.Length)
                                {
                                    _textFields[x].Value = fields.GetValue(x);
                                }
                                else
                                {
                                    _textFields[x].Value = null;
                                }
                            }
                            RecordEventArgs args = new RecordEventArgs(this.CurrentLineNumber, _textFields);
                            RecordFound(this, args);
                            if (_cancelledOperation)
                            {
                                return;
                            }
                            //								}
                            //								catch(Exception) 
                            //								{
                            //									throw;
                            //								}
                            //							}
                            //							else 
                            //							{
                            //								throw new InvalidOperationException("Number of fields in the mapping text does not match the number found in the input stream.");
                            //							}
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }
        private void ParseFileDelimitedRecord(string fileName)
        {
            StreamReader reader = null;
            System.Int32 actualLineNumber;
            string fileRecord = string.Empty;
            char[] buffer = null;
            StringBuilder newRecord = null;
            int numberOfCharactersRead = 0;
            ArrayList fileRecordArrayList = null;

            try
            {
                fileRecordArrayList = new ArrayList();
                this.CurrentRecordRawData = string.Empty;
                if (this.TextFieldRecordSpanDelimiter.Length == 0)
                {
                    throw new System.Exception("Invalid record delimiter [FileFormat:=DelimitedRecord,TextFieldRecordSpanDelimiter:=" + this.TextFieldRecordSpanDelimiter + "].");
                }
                this.FileName = fileName;
                reader = new StreamReader(fileName);
                actualLineNumber = 0;
                if (this.FileStartPosition > 1)
                {
                    buffer = new char[1];
                    for (int k = 0; k < this.FileStartPosition - 1; k++)
                    {
                        if (reader.Peek() != -1)
                        {
                            numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        }
                    }
                }
                if (this.InvalidInitalFileRecordCount != 0)
                {
                    for (int j = 0; j < this.InvalidInitalFileRecordCount; j++)
                    {
                        if (reader.Peek() != -1)
                        {
                            buffer = new char[1];
                            newRecord = new StringBuilder();
                            fileRecord = string.Empty;
                            while (fileRecord.EndsWith(this.TextFieldRecordSpanDelimiter) != true)
                            {
                                numberOfCharactersRead = reader.Read(buffer, 0, 1);
                                newRecord.Append(buffer);
                                fileRecord = newRecord.ToString();
                                if (fileRecord == this.TextFieldRecordSpanDelimiter)
                                {
                                    fileRecord = string.Empty;
                                }
                                if (reader.Peek() == -1)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                while (reader.Peek() != -1)
                {
                    this.CurrentRecordRawData = string.Empty;
                    fileRecordArrayList.Clear();
                    buffer = new char[1];
                    newRecord = new StringBuilder();
                    fileRecord = string.Empty;
                    while (fileRecord.EndsWith(this.TextFieldRecordSpanDelimiter) != true)
                    {
                        numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        newRecord.Append(buffer);
                        fileRecord = newRecord.ToString();
                        if (fileRecord == this.TextFieldRecordSpanDelimiter)
                        {
                            fileRecord = string.Empty;
                        }
                        if (reader.Peek() == -1)
                        {
                            break;
                        }
                    }
                    fileRecord = fileRecord.Replace(this.TextFieldRecordSpanDelimiter, "");
                    if (this.FileType == FileFormat.DelimitedRecordDelimitedColumns)
                    {
                        fileRecord = fileRecord.Replace("\r\n", this.Delimiter.ToString());
                    }
                    else
                    {
                        fileRecord = fileRecord.Replace("\r\n", "");
                    }
                    actualLineNumber++;
                    fileRecordArrayList.Add(fileRecord);
                    if (_hasHeaderRecord)
                    {
                        _hasHeaderRecord = false;
                    }
                    else
                    {
                        if (actualLineNumber >= _currentLineNumber)
                        {
                            this._currentLineNumber = actualLineNumber;
                            this._currentTextFieldRecordNumber++;
                            Array fields = this.GetFieldArray(fileRecordArrayList);

                            //							if(fields.Length==_textFields.Count || this.FileType == FileFormat.DelimitedRecordDelimitedColumns || this.FileType == FileFormat.DelimitedRecordFixedColumns) 
                            //							{
                            //								try 
                            //								{
                            for (System.Int32 x = 0; x < _textFields.Count; x++)
                            {
                                if (x < fields.Length)
                                {
                                    _textFields[x].Value = fields.GetValue(x);
                                }
                                else
                                {
                                    _textFields[x].Value = null;
                                }
                            }
                            RecordEventArgs args = new RecordEventArgs(this.CurrentLineNumber, _textFields);
                            RecordFound(this, args);
                            if (_cancelledOperation)
                            {
                                return;
                            }

                            //								}
                            //								catch(Exception) 
                            //								{
                            //									throw;
                            //								}
                            //							}
                            //							else 
                            //							{
                            //								throw new InvalidOperationException("Number of fields in the mapping text does not match the number found in the input stream.");
                            //
                            //							}

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }

        private void ParseFileFixedLengthRecord(string fileName)
        {
            StreamReader reader = null;
            System.Int32 actualLineNumber;
            string fileRecord = string.Empty;
            char[] buffer = null;
            StringBuilder newRecord = null;
            int numberOfCharactersRead = 0;
            ArrayList fileRecordArrayList = null;

            try
            {
                fileRecordArrayList = new ArrayList();
                this.CurrentRecordRawData = string.Empty;
                if (this.RecordLength == 0)
                {
                    throw new System.Exception("Invalid record length [FileFormat:=FixedRecordAndColumnWidth,RecordLength:=" + this.RecordLength + "].");
                }
                this.FileName = fileName;

                reader = new StreamReader(fileName);

                actualLineNumber = 0;
                if (this.FileStartPosition > 1)
                {
                    buffer = new char[1];
                    for (int k = 0; k < this.FileStartPosition - 1; k++)
                    {
                        if (reader.Peek() != -1)
                        {
                            numberOfCharactersRead = reader.Read(buffer, 0, 1);
                        }
                    }
                }
                buffer = new char[this.RecordLength];
                if (this.InvalidInitalFileRecordCount != 0)
                {
                    for (int j = 0; j < this.InvalidInitalFileRecordCount; j++)
                    {
                        for (int l = 0; l < this.TextFieldRecordSpan; l++)
                        {
                            if (reader.Peek() != -1)
                            {
                                numberOfCharactersRead = reader.Read(buffer, 0, this.RecordLength);
                            }
                        }
                    }
                }
                while (reader.Peek() != -1)
                {
                    this.CurrentRecordRawData = string.Empty;
                    fileRecordArrayList.Clear();
                    for (int i = 0; i < this.TextFieldRecordSpan; i++)
                    {
                        newRecord = new StringBuilder();
                        fileRecord = string.Empty;
                        numberOfCharactersRead = reader.Read(buffer, 0, this.RecordLength);
                        newRecord.Append(buffer);
                        fileRecord = newRecord.ToString();
                        actualLineNumber++;
                        fileRecordArrayList.Add(fileRecord);
                    }
                    if (_hasHeaderRecord)
                    {
                        _hasHeaderRecord = false;
                    }
                    else
                    {
                        if (actualLineNumber >= _currentLineNumber)
                        {
                            this._currentLineNumber = actualLineNumber;
                            this._currentTextFieldRecordNumber++;
                            Array fields = this.GetFieldArray(fileRecordArrayList);
                            //							if(fields.Length==_textFields.Count || this.FileType == FileFormat.DelimitedRecordDelimitedColumns || this.FileType == FileFormat.DelimitedRecordFixedColumns) 
                            //							{
                            //								try 
                            //								{
                            for (System.Int32 x = 0; x < _textFields.Count; x++)
                            {
                                if (x < fields.Length)
                                {
                                    _textFields[x].Value = fields.GetValue(x);
                                }
                                else
                                {
                                    _textFields[x].Value = null;
                                }
                            }

                            RecordEventArgs args = new RecordEventArgs(this.CurrentLineNumber, _textFields);
                            RecordFound(this, args);
                            if (_cancelledOperation)
                            {
                                return;
                            }

                            //								}
                            //								catch(Exception) 
                            //								{
                            //									throw;
                            //								}
                            //							}
                            //							else 
                            //							{
                            //								throw new InvalidOperationException("Number of fields in the mapping text does not match the number found in the input stream.");
                            //							}
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

        }
        private void ProcessFieldArray(Array fields)
        {
            //			if(fields.Length==_textFields.Count || this.FileType == FileFormat.DelimitedRecordDelimitedColumns || this.FileType == FileFormat.DelimitedRecordFixedColumns) 
            //			{
            //				try 
            //				{
            for (System.Int32 x = 0; x < _textFields.Count; x++)
            {
                if (x < fields.Length)
                {
                    _textFields[x].Value = fields.GetValue(x);
                }
                else
                {
                    _textFields[x].Value = null;
                }
            }

            RecordEventArgs args = new RecordEventArgs(this.CurrentLineNumber, _textFields);
            RecordFound(this, args);
            if (_cancelledOperation)
            {
                return;
            }

            //				}
            //				catch(Exception) 
            //				{
            //					throw;
            //				}
            //			}
            //			else 
            //			{
            //				throw new InvalidOperationException("Number of fields in the mapping text does not match the number found in the input stream.");
            //			}
        }
        #endregion

        #region Event Args

        public class RecordEventArgs : EventArgs
        {

            private System.Int32 _currentLineNumber;
            private TextFieldCollection _fieldMappings;

            public RecordEventArgs(System.Int32 currentLineNumber, TextFieldCollection fieldMappings)
            {
                _currentLineNumber = currentLineNumber;
                _fieldMappings = fieldMappings;
            }

            public System.Int32 CurrentLineNumber
            {
                get { return this._currentLineNumber; }
            }

            public TextFieldCollection FieldMappings
            {
                get { return this._fieldMappings; }
            }

        }

        #endregion

        #region Helper Functions

        private Array GetFieldArray(ArrayList fileRecordArrayList)
        {
            string fileRecord = string.Empty;
            Array array = null;

            if (fileRecordArrayList.Count != 0)
            {
                for (int i = 0; i < fileRecordArrayList.Count; i++)
                {
                    if (fileRecord.Length == 0 || (this.FileType != FileFormat.Delimited && this.FileType != FileFormat.DelimitedRecordDelimitedColumns))
                    {
                        fileRecord = fileRecord + fileRecordArrayList[i];
                    }
                    else
                    {
                        fileRecord = fileRecord + this.Delimiter + fileRecordArrayList[i];
                    }
                }
                this.CurrentRecordRawData = fileRecord;
                array = GetFieldArray(fileRecord);
            }
            return array;
        }
        private Array GetFieldArray(string fileRecord)
        {

            Array fields = null;
            ArrayList rawList = new ArrayList();
            System.Int32 mark = 0;

            switch (this.FileType)
            {

                case FileFormat.Delimited:

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Replace any embedded delimitor characters
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    fileRecord = RemoveDelimitedCharacters(fileRecord);
                    //fileRecord = fileRecord.Replace(this.Delimiter + " ", " ");

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Split out the columngs
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    string[] rawFields = fileRecord.Split(Convert.ToChar(this.Delimiter));

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Recombine any with quotes
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    RecombineQuotedFields(ref rawFields);

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Remove the extra elements
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    ExtractNullArrayElements(ref rawFields, ref fields);

                    break;
                case FileFormat.DelimitedRecordDelimitedColumns:

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Replace any embedded delimitor characters
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    fileRecord = RemoveDelimitedCharacters(fileRecord);
                    //fileRecord = fileRecord.Replace(this.Delimiter + " ", " ");

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Split out the columngs
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    string[] rawFields2 = fileRecord.Split(Convert.ToChar(this.Delimiter));

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Recombine any with quotes
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    RecombineQuotedFields(ref rawFields2);

                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    // Remove the extra elements
                    // --------------------------------------------------------------------------------------------------
                    // --------------------------------------------------------------------------------------------------
                    ExtractNullArrayElements(ref rawFields2, ref fields);

                    break;

                case FileFormat.FixedWidth:

                    for (System.Int32 x = 0; x < _textFields.Count; x++)
                    {

                        try
                        {

                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            // Extract the value and move the book mark
                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            rawList.Add(fileRecord.Substring(mark, _textFields[x].Length));
                            mark += _textFields[x].Length;

                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }

                    fields = rawList.ToArray();

                    break;
                case FileFormat.FixedWidthColumnsVariableNumber:

                    for (System.Int32 x = 0; x < _textFields.Count; x++)
                    {

                        try
                        {

                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            // Extract the value and move the book mark
                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            if (fileRecord.Length >= (mark + _textFields[x].Length))
                            {
                                rawList.Add(fileRecord.Substring(mark, _textFields[x].Length));
                            }
                            else if (fileRecord.Length > mark && fileRecord.Length < (mark + _textFields[x].Length))
                            {
                                rawList.Add(fileRecord.Substring(mark, fileRecord.Length - mark));
                            }
                            else
                            {
                                rawList.Add(string.Empty);
                            }
                            mark += _textFields[x].Length;

                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }

                    fields = rawList.ToArray();

                    break;
                case FileFormat.FixedRecordAndColumnWidth:

                    for (System.Int32 x = 0; x < _textFields.Count; x++)
                    {

                        try
                        {

                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            // Extract the value and move the book mark
                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            rawList.Add(fileRecord.Substring(mark, _textFields[x].Length));
                            mark += _textFields[x].Length;

                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }

                    fields = rawList.ToArray();

                    break;
                case FileFormat.DelimitedRecordFixedColumns:

                    for (System.Int32 x = 0; x < _textFields.Count; x++)
                    {

                        try
                        {

                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            // Extract the value and move the book mark
                            // --------------------------------------------------------------------------------------------------
                            // --------------------------------------------------------------------------------------------------
                            if ((mark + _textFields[x].Length) <= fileRecord.Length)
                            {
                                rawList.Add(fileRecord.Substring(mark, _textFields[x].Length));
                                mark += _textFields[x].Length;
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }

                    fields = rawList.ToArray();

                    break;
                default:
                    throw new InvalidOperationException("The specified FileType is not valid.");

            }

            return fields;

        }

        /// <summary>
        /// RemoveDelimitedCharacters : Go through current string and remove delimited characters inside double quotes.
        /// </summary>
        /// <param name="fileRecord">Record containing the columns.</param>
        /// <returns>Cleaned file record.</returns>
        private string RemoveDelimitedCharacters(string fileRecord)
        {

            bool foundQuote = false;
            StringBuilder newRecord;
            char c;

            newRecord = new StringBuilder(fileRecord.Length);

            for (System.Int16 x = 0; x < fileRecord.Length; x++)
            {

                c = fileRecord[x];

                //				if(c=='"') 
                //				{
                //					foundQuote = !foundQuote;
                //				}
                if (c == _quoteChar)
                {
                    foundQuote = !foundQuote;
                }
                if (c == this.Delimiter)
                {

                    // ------------------------------------------------------------------------------------------------------
                    // ------------------------------------------------------------------------------------------------------
                    // If found quote and we found a character that matches our delimiter then we need to pull this out.
                    // ------------------------------------------------------------------------------------------------------
                    // ------------------------------------------------------------------------------------------------------
                    if (!foundQuote)
                    {
                        newRecord.Append(c);
                    }

                }
                else
                {
                    newRecord.Append(c);
                }

            }

            return newRecord.ToString().TrimEnd();

        }

        /// <summary>
        /// RecombineQuotedFields : Remove leading and ending quotes for each column.
        /// </summary>
        /// <param name="fields">Array of fields to check quotes.</param>
        private void RecombineQuotedFields(ref string[] fields)
        {

            char firstChar;
            char lastChar;
            System.Int32 startIndex;
            char quoteChar;

            for (System.Int32 x = 0; x < fields.Length; x++)
            {

                // ------------------------------------------------------------------------
                // ------------------------------------------------------------------------
                // Get first and last character
                // ------------------------------------------------------------------------
                // ------------------------------------------------------------------------
                if (fields[x].Length > 0)
                {
                    firstChar = fields[x][0];
                    lastChar = fields[x][fields[x].Length - 1];
                }
                else
                {
                    // ------------------------------------------------------------------------
                    // ------------------------------------------------------------------------
                    // No field to process so move to next field
                    // ------------------------------------------------------------------------
                    // ------------------------------------------------------------------------
                    //continue;
                    firstChar = char.Parse("0");
                    lastChar = Convert.ToChar("0");
                }

                // ------------------------------------------------------------------------
                // ------------------------------------------------------------------------
                // Start the comparisons
                // ------------------------------------------------------------------------
                // ------------------------------------------------------------------------
                if (firstChar == _quoteChar)
                {
                    if (firstChar == lastChar)
                    {
                        fields.SetValue(fields[x].Substring(1, fields[x].Length - 2), x);
                    }
                    else
                    {
                        startIndex = x;
                        quoteChar = firstChar;

                        do
                        {

                            x++;
                            firstChar = fields[x][0];
                            lastChar = fields[x][fields[x].Length - 1];

                            // ------------------------------------------------------------------------
                            // ------------------------------------------------------------------------
                            // This field better not start with a quote
                            // ------------------------------------------------------------------------
                            // ------------------------------------------------------------------------
                            if (firstChar == _quoteChar & fields[x].Length > 1)
                            {
                                throw new InvalidOperationException("There was an unclosed quotation mark.");
                            }

                            // ------------------------------------------------------------------------
                            // ------------------------------------------------------------------------
                            // Recombine the items
                            // ------------------------------------------------------------------------
                            // ------------------------------------------------------------------------
                            fields.SetValue(string.Concat(fields[startIndex].ToString(), this.Delimiter, fields[x].ToString()), startIndex);

                            // ------------------------------------------------------------------------
                            // ------------------------------------------------------------------------
                            // Flush the unused array element
                            // ------------------------------------------------------------------------
                            // ------------------------------------------------------------------------
                            Array.Clear(fields, x, 1);

                        }
                        while (lastChar != firstChar);

                    }
                }

            }

        }

        private void ExtractNullArrayElements(ref string[] input, ref Array output)
        {

            System.Int32 x = 0;
            System.Int32 count = 0;
            System.Int32 mark = 0;

            // ------------------------------------------------------------------------
            // ------------------------------------------------------------------------
            // Get actual field count
            // ------------------------------------------------------------------------
            // ------------------------------------------------------------------------
            for (x = 0; x < input.Length; x++)
            {
                if (input[x] != null) count++;
            }

            // ------------------------------------------------------------------------
            // ------------------------------------------------------------------------
            // Resize output array
            // ------------------------------------------------------------------------
            // ------------------------------------------------------------------------
            output = Array.CreateInstance(typeof(string), count);
            for (x = 0; x < input.Length; x++)
            {
                if (input[x] != null)
                {
                    output.SetValue(input[x], mark);
                    mark++;
                }
            }

        }

        #endregion

    }

    #region TextField

    public class TextField
    {

        private const System.Int32 NO_LENGTH = 0x0FFF;

        // Variable declarations
        private string _name = string.Empty;
        private TypeCode _dataType;
        private System.Int32 _length;
        private bool _quoted;
        private object _value;

        public TextField(string name, TypeCode dataType, System.Int32 length, bool quoted)
        {

            this.Name = name;
            this.DataType = dataType;
            this.Length = length;
            this.Quoted = quoted;

        }

        public TextField(string name, TypeCode dataType, bool quoted)
            : this(name, dataType, NO_LENGTH, quoted)
        {
        }

        public TextField(string name, TypeCode dataType, System.Int32 length)
            : this(name, dataType, length, false)
        {
        }


        /// <summary>
        /// display name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new MethodAccessException("Invalid value for name.");
                }
                _name = value;
            }
        }

        public TypeCode DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public System.Int32 Length
        {
            get { return _length; }
            set
            {
                if (value < 1 & value != NO_LENGTH)
                {
                    throw new MethodAccessException("You can not set the Length property to a zero or negative value.");
                }
                _length = value;
            }
        }

        public bool Quoted
        {
            get { return _quoted; }
            set { _quoted = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }

    #endregion

    #region TextFieldCollection

    public class TextFieldCollection : System.Collections.CollectionBase
    {

        #region Constructors

        /// <summary>
        /// TextFieldCollection - Default constructor
        /// </summary>
        public TextFieldCollection()
            : base()
        {
        }

        /// <summary>
        /// TextFieldCollection : Overload
        /// </summary>
        /// <param name="textValue">TextFieldCollection type.</param>
        /// <exception cref="ArgumentNullException">Throws exception if text field collection is null.</exception>
        public TextFieldCollection(TextFieldCollection textValue)
            : base()
        {
            if (textValue == null)
            {
                throw new ArgumentNullException("Invalid reference to text field collection.");
            }

            this.AddRange(textValue);
        }

        /// <summary>
        /// TextFieldCollection : Overload
        /// </summary>
        /// <param name="textValues">Arry of text fields</param>
        /// <exception cref="ArgumentNullException">Throws exception if textfield array is null.</exception>
        public TextFieldCollection(TextField[] textValues)
            : base()
        {
            if (textValues == null)
            {
                throw new ArgumentNullException("Invalid reference to text field array.");
            }

            this.AddRange(textValues);
        }

        #endregion

        /// <summary>
        ///     Represents the 'TextField' item at the specified index position.
        /// </summary>
        /// <param name='intIndex'>
        ///     The zero-based index of the entry to locate in the collection.
        /// </param>
        /// <value>
        ///     The entry at the specified index of the collection.
        /// </value>
        public TextField this[System.Int32 intIndex]
        {
            get { return List[intIndex] as TextField; }
            set { List[intIndex] = value; }
        }

        /// <summary>
        ///     Adds a 'TextField' item with the specified value to the 'TextFieldCollection'
        /// </summary>
        /// <param name='texValue'>
        ///     The 'TextField' to add.
        /// </param>
        /// <returns>
        ///     The index at which the new element was inserted.
        /// </returns>
        public System.Int32 Add(TextField textField)
        {
            return this.List.Add(textField);
        }

        /// <summary>
        ///     Adds the contents of another 'TextFieldCollection' at the end of this instance.
        /// </summary>
        /// <param name='texValue'>
        ///     A 'TextFieldCollection' containing the objects to add to the collection.
        /// </param>
        public void AddRange(TextField[] textValue)
        {
            for (System.Int32 i = 0; i < textValue.Length; i++)
            {
                this.Add(textValue[i]);
            }
        }

        /// <summary>
        ///     Adds the contents of another 'TextFieldCollection' at the end of this instance.
        /// </summary>
        /// <param name='texValue'>
        ///     A 'TextFieldCollection' containing the objects to add to the collection.
        /// </param>
        public void AddRange(TextFieldCollection textValue)
        {
            for (System.Int32 i = 0; i < textValue.Count; i++)
            {
                this.Add(textValue[i]);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the 'TextFieldCollection' contains the specified value.
        /// </summary>
        /// <param name='texValue'>
        ///     The item to locate.
        /// </param>
        /// <returns>
        ///     True if the item exists in the collection; false otherwise.
        /// </returns>
        public bool Contains(TextField textValue)
        {
            return List.Contains(textValue);
        }

        /// <summary>
        ///     Copies the 'TextFieldCollection' values to a one-dimensional System.Array
        ///     instance starting at the specified array index.
        /// </summary>
        /// <param name='texArray'>
        ///     The one-dimensional System.Array that represents the copy destination.
        /// </param>
        /// <param name='intIndex'>
        ///     The index in the array where copying begins.
        /// </param>
        public void CopyTo(TextField[] textArray, System.Int32 index)
        {
            List.CopyTo(textArray, index);
        }

        /// <summary>
        ///     Returns the index of a 'TextField' object in the collection.
        /// </summary>
        /// <param name='texValue'>
        ///     The 'TextField' object whose index will be retrieved.
        /// </param>
        /// <returns>
        ///     If found, the index of the value; otherwise, -1.
        /// </returns>
        public System.Int32 IndexOf(TextField textValue)
        {
            return List.IndexOf(textValue);
        }

        /// <summary>
        ///     Inserts an existing 'TextField' into the collection at the specified index.
        /// </summary>
        /// <param name='intIndex'>
        ///     The zero-based index where the new item should be inserted.
        /// </param>
        /// <param name='texValue'>
        ///     The item to insert.
        /// </param>
        public void Insert(System.Int32 index, TextField textValue)
        {
            List.Insert(index, textValue);
        }

        #region TextFieldEnumerator

        public class TextFieldEnumerator : System.Collections.IEnumerator
        {

            private System.Collections.IEnumerator enBase;
            private System.Collections.IEnumerable enLocal;

            #region Constructors

            /// <summary>
            /// TextFieldEnumerator : Overload constructor
            /// </summary>
            /// <param name="textMappings">Text mappings</param>
            /// <exception cref="ArgumentNullException">Null reference to text mappings.</exception>
            public TextFieldEnumerator(TextFieldCollection textMappings)
                : base()
            {

                this.enLocal = textMappings as System.Collections.IEnumerable;
                if (this.enLocal == null)
                {
                    throw new ArgumentNullException("Invalid reference for text mappings.");
                }

                this.enBase = enLocal.GetEnumerator();

            }

            #endregion

            #region IEnumerator Members

            /// <summary>
            /// Reset : Sets the enumerator to the first element in the collection
            /// </summary>
            public void Reset()
            {
                enBase.Reset();
            }

            /// <summary>
            /// Current - Implements IEnumerable.Current property
            /// </summary>
            public object Current
            {
                get { return enBase.Current; }
            }

            /// <summary>
            /// MoveNext : Advances the enumerator to the next element of the collection
            /// </summary>
            public bool MoveNext()
            {
                return enBase.MoveNext();
            }

            #endregion

            /// <summary>
            /// Current : Gets the current element from the collection (strongly typed)
            /// </summary>
            //			public TextField Current 
            //			{
            //				get { return enBase.Current as TextField; }
            //			}

        }
        #endregion

    }

    #endregion

}
