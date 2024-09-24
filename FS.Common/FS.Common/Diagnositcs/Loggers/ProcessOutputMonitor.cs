using System;

namespace FS.Common.Diagnostics.Loggers
{
    /// <summary>
    /// Summary description for ProcessOutputMonitor.
    /// </summary>
    public class ProcessOutputMonitor
    {
        //purpose: in processes that are analogous to an assembly line, collect data to determine level of output, 
        //processing time per unit output, number of defects per x units, etc. at each portion of the process 
        //to apply six sigma methodology (or similar).
        private const string LOG_DATED_FILENAME_NAME_FORMAT = "MM-dd-yyyy";
        private const double LOG_MAX_NUM_DAYS_AVAILABLE = 7;
        System.DateTime _startDateTime = System.DateTime.MinValue;
        System.DateTime _stopDateTime = System.DateTime.MinValue;
        System.Int64 _lastTic = 0;
        System.Int64 _ticCount = 0;
        System.Int64 _outputCount = 0;
        System.Int64 _errorCount = 0;
        string _unitLabel = string.Empty;
        string _optionalUnitSubKey1 = string.Empty;
        string _optionalUnitSubKey2 = string.Empty;
        bool _locked = false;
        bool _started = false;
        bool _paused = false;
        string _fileFolder = string.Empty;
        string _fileName = string.Empty;

        public ProcessOutputMonitor(string unitLabel, string optionalUnitSubKey1, string optionalUnitSubKey2)
        {
            _unitLabel = unitLabel;
            _optionalUnitSubKey1 = optionalUnitSubKey1;
            _optionalUnitSubKey2 = optionalUnitSubKey2;
            _locked = false;
            //TODO write to file /COMMONDATA/diagnostics/processoutputmonitor/machinename.log
            _fileFolder = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("LogRootDirectory", "c:\\logs\\").Trim().TrimEnd("\\".ToCharArray()) +
                @"\processoutputmonitor\";
            _fileName = _fileFolder + System.DateTime.Now.ToUniversalTime().ToString(LOG_DATED_FILENAME_NAME_FORMAT) + ".log";

        }
        public void Start()
        {
            if (_locked == true)
            {
                throw (new Exception("ProcessOutputMonitor object expired.  Stop() has been executed."));
            }
            _startDateTime = System.DateTime.Now;
            _started = true;
            _lastTic = Environment.TickCount;
        }
        public void Pause()
        {
            System.Int64 holdTic = 0;

            if (_locked == true)
            {
                throw (new Exception("ProcessOutputMonitor object expired.  Stop() has been executed."));
            }
            if (_started == false)
            {
                throw (new Exception("Invalid funciton call Pause(). ProcessOutputMonitor must be started first."));
            }
            _paused = true;
            holdTic = Environment.TickCount;
            if (holdTic >= _lastTic)
            {
                _ticCount = _ticCount + (holdTic - _lastTic);
            }
            else
            {
                _ticCount = _ticCount + holdTic;
            }
            _lastTic = holdTic;
        }
        public void Resume()
        {
            if (_locked == true)
            {
                throw (new Exception("ProcessOutputMonitor object expired.  Stop() has been executed."));
            }
            if (_started == false)
            {
                throw (new Exception("Invalid funciton call Resume(). ProcessOutputMonitor must be started first."));
            }
            if (_paused == false)
            {
                throw (new Exception("Invalid funciton call Resume(). ProcessOutputMonitor must be paused first."));
            }
            _paused = false;
            _lastTic = Environment.TickCount;
        }
        public void Stop()
        {
            System.Int64 holdTic = 0;

            if (_locked == true)
            {
                throw (new Exception("ProcessOutputMonitor object expired.  Stop() has been executed."));
            }
            if (_started == false)
            {
                throw (new Exception("Invalid funciton call Stop(). ProcessOutputMonitor must be started first."));
            }
            _paused = false;
            _started = false;
            _locked = true;
            _stopDateTime = System.DateTime.Now;
            holdTic = Environment.TickCount;
            if (holdTic >= _lastTic)
            {
                _ticCount = _ticCount + (holdTic - _lastTic);
            }
            else
            {
                _ticCount = _ticCount + holdTic;
            }
            _lastTic = holdTic;
            WriteRecord();
        }
        public void AddOutput(System.Int64 additionalOutputCount)
        {
            if (_locked == true)
            {
                throw (new Exception("ProcessOutputMonitor object expired.  Stop() has been executed."));
            }
            _outputCount = _outputCount + additionalOutputCount;
        }
        public void AddError(System.Int64 additionalErrorCount)
        {
            if (_locked == true)
            {
                throw (new Exception("ProcessOutputMonitor object expired.  Stop() has been executed."));
            }
            _errorCount = _errorCount + additionalErrorCount;
        }

        private void WriteRecord()
        {
            string record = string.Empty;
            System.IO.StreamWriter writer = null;

            System.IO.Directory.CreateDirectory(_fileFolder);
            RemoveOldLogs();
            record = record + _unitLabel.PadRight(100, " ".ToCharArray()[0]);
            record = record + _optionalUnitSubKey1.PadRight(100, " ".ToCharArray()[0]);
            record = record + _optionalUnitSubKey2.PadRight(100, " ".ToCharArray()[0]);
            record = record + _startDateTime.ToUniversalTime().ToString().PadRight(25, " ".ToCharArray()[0]);
            record = record + _stopDateTime.ToUniversalTime().ToString().PadRight(25, " ".ToCharArray()[0]);
            record = record + _ticCount.ToString().PadRight(50, " ".ToCharArray()[0]);
            record = record + _outputCount.ToString().PadRight(50, " ".ToCharArray()[0]);
            record = record + _errorCount.ToString().PadRight(50, " ".ToCharArray()[0]);
            record = record + System.Guid.NewGuid().ToString().PadRight(50, " ".ToCharArray()[0]);
            writer = new System.IO.StreamWriter(_fileName, true);
            writer.WriteLine(record);
            writer.Close();
        }
        private void RemoveOldLogs()
        {
            string[] stringFiles = System.IO.Directory.GetFiles(_fileFolder);
            foreach (string stringFile in stringFiles)
            {
                if (DateTime.Compare(System.IO.File.GetCreationTime(stringFile).AddDays(LOG_MAX_NUM_DAYS_AVAILABLE), System.DateTime.Now.ToUniversalTime()) < 0)
                {
                    System.IO.File.Delete(stringFile);
                }
            }
        }

    }
}
