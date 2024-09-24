using System;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using log4net;
using log4net.Appender;
//using FS.Common.IO;
//using FS.Common.Configuration;
//using LogentriesCore;
//using LogentriesCore.Net;
using System.Threading.Tasks;

namespace FS.Common.Diagnostics.Loggers
{


    /// <summary>
    /// This class can be used by any object in any app.  It is designed to let an object log any applicaiton data, event, error, etc..
    /// The output destination of these log entries are configured by the application configuration data (see LoadDestinationSettings).
    /// When creating this class, use a factory class to build it.  The object created should use the AT.Base.Iapplicationlog interface.
    /// the output destinations, log entry severities, and events are listed as enumerations in Iapplicationlog.
    /// The log files are placed in dated folders.  These folders are automatically deleted after 7 days.
    /// All destinations can be enabled or disabled, and configured to accept all or fewer severity levels.
    /// </summary>
    public class ApplicationLog
    {
        private const string APPLICATION_LOG_DATED_DIRECTORY_NAME_FORMAT = "MM-dd-yyyy";
        private const double APPLICATION_LOG_DATED_DIRECTORY_MAX_NUM_DAYS_AVAILABLE = 60;
        private string _applicationName = string.Empty;
        private string _logRootDirectory = string.Empty;
        private string _dateFolderName = string.Empty;
        private string _logFileNameBase = string.Empty;
        private System.Collections.ArrayList _destinations = null;
        private bool _useUTCDateTime = false;

        private bool _destinationEnabledDB = false;
        private bool _destinationEnabledErrorLogFile = false;
        private bool _destinationEnabledEventViewer = false;
        private bool _destinationEnabledAzureCloud = false;
        private bool _destinationEnabledLogFile = false;
        private bool _destinationEnabledProcessIDXMLFile = false;
        private bool _destinationEnabledProcessIDLogFile = false;
        private bool _destinationEnabledXMLFile = false;
        private bool _destinationEnabledEmail = false;
        private bool _destinationEnabledLog4Net = false;
        private bool _destinationEnabledBroadcast = false;
        private bool _destinationEnabledConsole = false;

        private bool _isAzureCloudDeployment = false;
        private bool _isLoggingDisabled = false;

        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelDB = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelErrorLogFile = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelEventViewer = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelAzureCloud = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelLogFile = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelProcessIDXMLFile = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelProcessIDLogFile = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelXMLFile = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelEmail = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelLog4Net = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelBroadcast = ApplicationLogEntrySeverities.ErrorOccurred;
        private ApplicationLogEntrySeverities _maximumLoggableSeverityLevelConsole = ApplicationLogEntrySeverities.ErrorOccurred;

        private static readonly log4net.ILog _log4Net = 
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ////requred to keep references
        //private LogentriesAppender loggentries_appender = new LogentriesAppender(); 

        public ApplicationLog(string applicationName, bool useUTCDateTime)
        {
            this._useUTCDateTime = useUTCDateTime;
            Init(applicationName);
        }
        public ApplicationLog(string applicationName)
        {
             Init(applicationName);
       }
        public ApplicationLog()
        {
            Init(BuildApplicationName());
        }
        /// <summary>
        /// constructor. It loads the log entry destinations, creates the nessesary log directories, and deletes old logs of the parent app
        /// </summary>
        public void Init(string applicationName)
        {

            //get log directory
            LogRootDirectory = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("LogRootDirectory", "c:\\logs\\");

            //get App name
            ApplicationName = applicationName;

            //get destination data 
            Destinations = new ArrayList();
            AddGlobalDestinations();
            LogFileNameBase = "Log";

            //get global destinations
            LoadDestinationSettings();

            InitializeApplicationLogDirectory();

            //get app destinations from config file
        }

        #region Class Properties
        /// <summary>
        /// This enables/disables the ability for a log entry to be written to the db.
        /// </summary>
        private bool DestinationEnabledDB
        {
            get
            {
                return _destinationEnabledDB;
            }
            set
            {
                _destinationEnabledDB = value;
            }
        }

        private bool UseUTCDateTime
        {
            get
            {
                return this._useUTCDateTime;
            }
            set
            {
                _useUTCDateTime = value;
            }
        }
        /// <summary>
        /// This enables/disables the ability for an error event log entry to be written to the error log files.
        /// </summary>
        private bool DestinationEnabledErrorLogFile
        {
            get
            {
                return _destinationEnabledErrorLogFile;
            }
            set
            {
                _destinationEnabledErrorLogFile = value;
            }
        }
        /// <summary>
        /// This enables/disables the ability for a log entry to be written to the windows event viewer.
        /// </summary>
        private bool DestinationEnabledEventViewer
        {
            get
            {
                return _destinationEnabledEventViewer;
            }
            set
            {
                _destinationEnabledEventViewer = value;
            }
        }


        /// <summary>
        /// This enables/disables the ability for a log entry to be written to the windows event viewer.
        /// </summary>
        private bool DestinationEnabledAzureCloud
        {
            get
            {
                return _destinationEnabledAzureCloud;
            }
            set
            {
                _destinationEnabledAzureCloud = value;
            }
        }

        /// <summary>
        /// This enables/disables the ability for a log entry to be written to a log file.
        /// </summary>
        private bool DestinationEnabledLogFile
        {
            get
            {
                return _destinationEnabledLogFile;
            }
            set
            {
                _destinationEnabledLogFile = value;
            }
        }
        /// <summary>
        /// This enables/disables the ability for a log entry to be written to a log file with name of the processid passed into the log entry.
        /// </summary>
        private bool DestinationEnabledProcessIDXMLFile
        {
            get
            {
                return _destinationEnabledProcessIDXMLFile;
            }
            set
            {
                _destinationEnabledProcessIDXMLFile = value;
            }
        }
        /// <summary>
        /// This enables/disables the ability for a log entry to be written to a log file with name of the processid passed into the log entry.
        /// </summary>
        private bool DestinationEnabledProcessIDLogFile
        {
            get
            {
                return _destinationEnabledProcessIDLogFile;
            }
            set
            {
                _destinationEnabledProcessIDLogFile = value;
            }
        }
        /// <summary>
        /// This enables/disables the ability for a log entry to be written to an xml file.
        /// </summary>
        private bool DestinationEnabledXMLFile
        {
            get
            {
                return _destinationEnabledXMLFile;
            }
            set
            {
                _destinationEnabledXMLFile = value;
            }
        }
        /// <summary>
        /// This enables/disables the ability for a log entry to be written to an email.
        /// </summary>
        private bool DestinationEnabledEmail
        {
            get
            {
                return _destinationEnabledEmail;
            }
            set
            {
                _destinationEnabledEmail = value;
            }
        }

        private bool DestinationEnabledLog4Net
        {
            get
            {
                return _destinationEnabledLog4Net;
            }
            set
            {
                _destinationEnabledLog4Net = value;
            }
        }
        private bool DestinationEnabledBroadcast
        {
            get
            {
                return _destinationEnabledBroadcast;
            }
            set
            {
                _destinationEnabledBroadcast = value;
            }
        }

        private bool DestinationEnabledConsole
        {
            get
            {
                return _destinationEnabledConsole;
            }
            set
            {
                _destinationEnabledConsole = value;
            }
        }

        /// <summary>
        /// This limits the possible log entries that can be written to the db.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelDB
        {
            get
            {
                return _maximumLoggableSeverityLevelDB;
            }
            set
            {
                _maximumLoggableSeverityLevelDB = value;
            }
        }
        /// <summary>
        /// This limits the possible log entries that can be written to the db.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelErrorLogFile
        {
            get
            {
                return _maximumLoggableSeverityLevelErrorLogFile;
            }
            set
            {
                _maximumLoggableSeverityLevelErrorLogFile = value;
            }
        }
        /// <summary>
        /// This limits the possible log entries that can be written to the windows event viewer.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelEventViewer
        {
            get
            {
                return _maximumLoggableSeverityLevelEventViewer;
            }
            set
            {
                _maximumLoggableSeverityLevelEventViewer = value;
            }
        }


        /// <summary>
        /// This limits the possible log entries that can be written to the windows event viewer.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelAzureCloud
        {
            get
            {
                return _maximumLoggableSeverityLevelAzureCloud;
            }
            set
            {
                _maximumLoggableSeverityLevelAzureCloud = value;
            }
        }

        /// <summary>
        /// This limits the possible log entries that can be written to the log file.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelLogFile
        {
            get
            {
                return _maximumLoggableSeverityLevelLogFile;
            }
            set
            {
                _maximumLoggableSeverityLevelLogFile = value;
            }
        }
        /// <summary>
        /// This limits the possible log entries that can be written to the process id xml log file.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelProcessIDXMLFile
        {
            get
            {
                return _maximumLoggableSeverityLevelProcessIDXMLFile;
            }
            set
            {
                _maximumLoggableSeverityLevelProcessIDXMLFile = value;
            }
        }
        /// <summary>
        /// This limits the possible log entries that can be written to the process id  log file.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelProcessIDLogFile
        {
            get
            {
                return _maximumLoggableSeverityLevelProcessIDLogFile;
            }
            set
            {
                _maximumLoggableSeverityLevelProcessIDLogFile = value;
            }
        }
        /// <summary>
        /// This limits the possible log entries that can be written to an xml log file.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelXMLFile
        {
            get
            {
                return _maximumLoggableSeverityLevelXMLFile;
            }
            set
            {
                _maximumLoggableSeverityLevelXMLFile = value;
            }
        }
        /// <summary>
        /// This limits the possible log entries that can be written to an email.  Log entries with a severity less than or equal to the maximum severity level will be written.
        /// </summary>
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelEmail
        {
            get
            {
                return _maximumLoggableSeverityLevelEmail;
            }
            set
            {
                _maximumLoggableSeverityLevelEmail = value;
            }
        }
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelLog4Net
        {
            get
            {
                return _maximumLoggableSeverityLevelLog4Net;
            }
            set
            {
                _maximumLoggableSeverityLevelLog4Net = value;
            }
        }
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelBroadcast
        {
            get
            {
                return _maximumLoggableSeverityLevelBroadcast;
            }
            set
            {
                _maximumLoggableSeverityLevelBroadcast = value;
            }
        }
        private ApplicationLogEntrySeverities MaximumLoggableSeverityLevelConsole
        {
            get
            {
                return _maximumLoggableSeverityLevelConsole;
            }
            set
            {
                _maximumLoggableSeverityLevelConsole = value;
            }
        }
        /// <summary>
        /// this contains all available destinations that log entries can be written to.
        /// </summary>
        private System.Collections.ArrayList Destinations
        {
            get
            {
                return _destinations;
            }
            set
            {
                _destinations = value;
            }
        }
        /// <summary>
        /// property used to manipulate member variable _applicationName.  This will be the name of one of the directories that
        /// a log file will reside in. it will also exist in the log entry.
        /// </summary>
        private string ApplicationName
        {
            get
            {
                return _applicationName;
            }
            set
            {
                _applicationName = value;
            }
        }
        /// <summary>
        /// property used to manipulate member variable _logRootDirectory.  This contains the root directory of the Log files.
        /// </summary>
        private string LogRootDirectory
        {
            get
            {
                return _logRootDirectory;
            }
            set
            {
                _logRootDirectory = value.Trim().TrimEnd("\\".ToCharArray());
            }
        }
        /// <summary>
        /// property used to manipulate member variable _dateFolderName.  This is the name of the last used log folder named with
        /// the current date.
        /// </summary>
        private string DateFolderName
        {
            get
            {
                return _dateFolderName;
            }
            set
            {
                _dateFolderName = value;
            }
        }
        /// <summary>
        /// property used to manipulate member variable _logFileNameBase.  This contains the file extension of the log file.
        /// </summary>
        private string LogFileNameBase
        {
            get
            {
                return _logFileNameBase;
            }
            set
            {
                _logFileNameBase = value;
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// This determines the Applicaiton name that should be used, given all available data.
        /// </summary>
        /// <returns></returns>
        private string BuildApplicationName()
        {
            string[] applicationNameElements = null;
            string[] applicationNameElements2 = null;
            string directoryToTest = string.Empty;
            string defaultSetting = string.Empty;
            string result = string.Empty;

            applicationNameElements = AppDomain.CurrentDomain.FriendlyName.Split("/".ToCharArray());
            applicationNameElements2 = applicationNameElements[applicationNameElements.GetUpperBound(0)].Split("-".ToCharArray());
            defaultSetting = applicationNameElements2[applicationNameElements.GetLowerBound(0)].Trim();
            result = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationName", defaultSetting);
            return result;
        }

        /// <summary>
        /// This is used to add a log entry output destination.  It is added to the member variable _destinations, an array holding all 
        /// active destinations for this class instance.
        /// </summary>
        /// <param name="requestedDestination">pass in a desired destination of all log entries.</param>
        private void AddDestination(ApplicationLogEntryDestinations requestedDestination)
        {
            Destinations.Add(requestedDestination);
        }

        /// <summary>
        /// This is used to read the application configuration data and configure all allowable output destinations.  
        /// Destinations may be enabled or disabled, or configured to accept all or fewer severity levels.
        /// </summary>
        private void LoadDestinationSettings()
        {
            DestinationEnabledDB = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.DB.Enabled", "false"));
            DestinationEnabledErrorLogFile = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.ErrorLogFile.Enabled", "false"));
            DestinationEnabledEventViewer = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.EventViewer.Enabled", "false"));
            DestinationEnabledAzureCloud = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.AzureCloud.Enabled", "false"));
            DestinationEnabledLogFile = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.LogFile.Enabled", "false"));
            DestinationEnabledProcessIDXMLFile = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.ProcessIDXMLFile.Enabled", "false"));
            DestinationEnabledProcessIDLogFile = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.ProcessIDLogFile.Enabled", "false"));
            DestinationEnabledXMLFile = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.XMLFile.Enabled", "false"));
            DestinationEnabledEmail = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.Enabled", "false"));
            DestinationEnabledLog4Net = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Log4Net.Enabled", "false"));
            DestinationEnabledBroadcast = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Broadcast.Enabled", "false"));
            DestinationEnabledConsole = Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Console.Enabled", "false"));

            MaximumLoggableSeverityLevelDB = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.DB.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelErrorLogFile = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.ErrorLogFile.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelEventViewer = (ApplicationLogEntrySeverities)Convert.ToInt32(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.EventViewer.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelAzureCloud = (ApplicationLogEntrySeverities)Convert.ToInt32(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.AzureCloud.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelLogFile = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.LogFile.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelProcessIDXMLFile = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.ProcessIDXMLFile.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelProcessIDLogFile = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.ProcessIDLogFile.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelXMLFile = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.XMLFile.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelEmail = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelLog4Net = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Log4Net.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelBroadcast = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Broadcast.MaximumLoggableSeverity", "1"));
            MaximumLoggableSeverityLevelConsole = (ApplicationLogEntrySeverities)Convert.ToInt16(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Console.MaximumLoggableSeverity", "1"));

            if (DestinationEnabledAzureCloud)
                _isAzureCloudDeployment = true;
            _isLoggingDisabled = true;
            if (DestinationEnabledAzureCloud ||
                DestinationEnabledBroadcast ||
                DestinationEnabledConsole ||
                DestinationEnabledDB ||
                DestinationEnabledEmail ||
                DestinationEnabledErrorLogFile ||
                DestinationEnabledEventViewer ||
                DestinationEnabledLog4Net ||
                DestinationEnabledLogFile ||
                DestinationEnabledProcessIDLogFile ||
                DestinationEnabledProcessIDXMLFile ||
                DestinationEnabledXMLFile)
                _isLoggingDisabled = false;
        }

        /// <summary>
        /// This loads the destinations that all instances of this class should have. 
        /// </summary>
        private void AddGlobalDestinations()
        {
            // load all destinations.  they are turned off by default. Use the app configuration file to turn each on.
            AddDestination(ApplicationLogEntryDestinations.Log4Net);
            AddDestination(ApplicationLogEntryDestinations.ErrorLogFile);
            AddDestination(ApplicationLogEntryDestinations.LogFile);
            AddDestination(ApplicationLogEntryDestinations.ProcessID_LogFile);
            AddDestination(ApplicationLogEntryDestinations.ProcessID_XMLFile);
            AddDestination(ApplicationLogEntryDestinations.EventViewer);
            AddDestination(ApplicationLogEntryDestinations.AzureCloud);
            AddDestination(ApplicationLogEntryDestinations.None);
            AddDestination(ApplicationLogEntryDestinations.Broadcast);
            AddDestination(ApplicationLogEntryDestinations.Console);
        }

        /// <summary>
        /// used to clear all current log entry destinations
        /// </summary>
        private void ClearDestinations()
        {
            Destinations.Clear();
            AddGlobalDestinations();
        }
        /// <summary>
        /// used to caculate the directory that the log file should reside in.
        /// </summary>
        /// <returns>The full path of the log directory</returns>
        private string GetApplicationLogDirectory()
        {
            return LogRootDirectory + "\\" + ApplicationName + "\\" + DateFolderName + "\\";
        }

        /// <summary>
        /// Used to Initialize member variable that hodls the current date folder name, creates the current log folder, 
        /// and removes all old logs for this app
        /// </summary>
        private void InitializeApplicationLogDirectory()
        {
            if (_isAzureCloudDeployment || _isLoggingDisabled)
                return;
            //get date folder name
            if (this._useUTCDateTime)
            {
                DateFolderName = System.DateTime.Now.ToUniversalTime().ToString(APPLICATION_LOG_DATED_DIRECTORY_NAME_FORMAT);
            }
            else
            {
                DateFolderName = System.DateTime.Now.ToString(APPLICATION_LOG_DATED_DIRECTORY_NAME_FORMAT);
            }
            //validate directory
            if (!FS.Common.IO.Directory.DriveExists(GetApplicationLogDirectory()))
                throw new System.Exception("Drive Not Found");
            FS.Common.IO.Directory.CreateDirectory(GetApplicationLogDirectory());
            RemoveOldApplicationLogs();
        }

        /// <summary>
        /// this checks to see if a new day has past, and a new dated folder should be created.
        /// </summary>
        private void ValidateApplicationLogDirectory()
        {
            if (_isAzureCloudDeployment || _isLoggingDisabled)
                return;

            if (this._useUTCDateTime)
            {
                if (DateFolderName != System.DateTime.Now.ToUniversalTime().ToString(APPLICATION_LOG_DATED_DIRECTORY_NAME_FORMAT))
                {
                    InitializeApplicationLogDirectory();
                }
            }
            else
            {
                if (DateFolderName != System.DateTime.Now.ToString(APPLICATION_LOG_DATED_DIRECTORY_NAME_FORMAT))
                {
                    InitializeApplicationLogDirectory();
                }
            }
        }
        /// <summary>
        /// this searches for the current applicaitons old logs and deletes them.
        /// </summary>
        private void RemoveOldApplicationLogs()
        {
            if (_isAzureCloudDeployment || _isLoggingDisabled)
                return;

            string[] stringDirectories = System.IO.Directory.GetDirectories(LogRootDirectory + "\\" + ApplicationName + "\\");

            foreach (string stringDir in stringDirectories)
            {
                if (this._useUTCDateTime)
                {
                    if (DateTime.Compare(System.IO.Directory.GetCreationTime(stringDir).AddDays(APPLICATION_LOG_DATED_DIRECTORY_MAX_NUM_DAYS_AVAILABLE), System.DateTime.Now.ToUniversalTime()) < 0)
                    {
                        System.IO.Directory.Delete(stringDir, true);
                    }
                }
                else
                {
                    if (DateTime.Compare(System.IO.Directory.GetCreationTime(stringDir).AddDays(APPLICATION_LOG_DATED_DIRECTORY_MAX_NUM_DAYS_AVAILABLE), System.DateTime.Now) < 0)
                    {
                        try
                        {
                            System.IO.Directory.Delete(stringDir, true);
                        }
                        catch { }
                    }
                }
                //stringPathName = GetPathName(stringFullPath);
                //create node for directories 
            }
        }

        /// <summary>
        /// This is called by a parent object to create a log entry.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any string data that should be logged.</param>
        public void AddEntry(System.Guid processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, string entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            AddEntry(processID, entrySeverity, entryEvent, (object)entryDescription);
        }
        public async Task AddEntryAsync(Objects.SessionContext sessionContext, System.Guid processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, string entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            await AddEntryAsync(sessionContext, processID, entrySeverity, entryEvent, (object)entryDescription);
        }
        public void AddEntry(string processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, string entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            AddEntry(processID, entrySeverity, entryEvent, (object)entryDescription);
        }
        public async Task AddEntryAsync(Objects.SessionContext sessionContext, string processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, string entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            await AddEntryAsync(sessionContext, processID, entrySeverity, entryEvent, (object)entryDescription);
        }
        /// <summary>
        /// This is called by a parent object to create a log entry.
        /// </summary>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any string data that should be logged.</param>
        public void AddEntry(ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, string entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            AddEntry("", entrySeverity, entryEvent, (object)entryDescription);
        }
        public async Task AddEntryAsync(Objects.SessionContext sessionContext, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, string entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            await AddEntryAsync(sessionContext, "", entrySeverity, entryEvent, (object)entryDescription);
        }

        public void AddEntry(Exception ex)
        {
            string errortext = string.Empty;
            errortext += System.Environment.NewLine;
            errortext += System.Environment.NewLine + "Message: " + ex.Message;
            errortext += System.Environment.NewLine + "Source: " + ex.Source;
            errortext += System.Environment.NewLine + "StackTrace: " + ex.StackTrace;
            errortext += System.Environment.NewLine;
            if (ex.InnerException != null)
            {
                errortext += System.Environment.NewLine;
                errortext += System.Environment.NewLine + "     Inner Exception...";
                errortext += System.Environment.NewLine + "     Message: " + ex.InnerException.Message;
                errortext += System.Environment.NewLine + "     Source: " + ex.InnerException.Source;
                errortext += System.Environment.NewLine + "     StackTrace: " + ex.InnerException.StackTrace;
                errortext += System.Environment.NewLine;
            }
            AddEntry(FS.Common.Diagnostics.Loggers.ApplicationLogEntrySeverities.ErrorOccurred,
FS.Common.Diagnostics.Loggers.ApplicationLogEntryEvents.ErrorOccurred,
errortext);

        }
        public async Task AddEntryAsync(Objects.SessionContext sessionContext, Exception ex)
        {
            string errortext = string.Empty;
            errortext += System.Environment.NewLine;
            errortext += System.Environment.NewLine + "Message: " + ex.Message;
            errortext += System.Environment.NewLine + "Source: " + ex.Source;
            errortext += System.Environment.NewLine + "StackTrace: " + ex.StackTrace;
            errortext += System.Environment.NewLine;
            if (ex.InnerException != null)
            {
                errortext += System.Environment.NewLine;
                errortext += System.Environment.NewLine + "     Inner Exception...";
                errortext += System.Environment.NewLine + "     Message: " + ex.InnerException.Message;
                errortext += System.Environment.NewLine + "     Source: " + ex.InnerException.Source;
                errortext += System.Environment.NewLine + "     StackTrace: " + ex.InnerException.StackTrace;
                errortext += System.Environment.NewLine;
            }
            await AddEntryAsync(sessionContext, FS.Common.Diagnostics.Loggers.ApplicationLogEntrySeverities.ErrorOccurred,
FS.Common.Diagnostics.Loggers.ApplicationLogEntryEvents.ErrorOccurred,
errortext);

        }

        /// <summary>
        /// This is called by a parent object to create a log entry.
        /// </summary>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any xmlnode that should be logged.</param>
        public void AddEntry(ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, XmlNode entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            AddEntry("", entrySeverity, entryEvent, (object)entryDescription);
        }
        public async Task AddEntryAsync(Objects.SessionContext sessionContext, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, XmlNode entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            await AddEntryAsync(sessionContext, "", entrySeverity, entryEvent, (object)entryDescription);
        }
        /// <summary>
        /// Private function that handles all public overloaded AddEntry functions.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        private void AddEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            try
            {
                ValidateApplicationLogDirectory();
                ApplicationLogEntryDestinations[] destinationArray = (ApplicationLogEntryDestinations[])Destinations.ToArray(typeof(ApplicationLogEntryDestinations));

                for (int x = 0; x <= Destinations.Count - 1; x++)
                {
                    switch (destinationArray[x])
                    {
                        case ApplicationLogEntryDestinations.EventViewer:
                            if (MaximumLoggableSeverityLevelEventViewer >= entrySeverity && DestinationEnabledEventViewer == true)
                            {
                                AddEventViewerEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.AzureCloud:
                            if (MaximumLoggableSeverityLevelAzureCloud >= entrySeverity && DestinationEnabledAzureCloud == true)
                            {
                                AddAzureCloudEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.ProcessID_XMLFile:
                            if (MaximumLoggableSeverityLevelProcessIDXMLFile >= entrySeverity && DestinationEnabledProcessIDXMLFile == true)
                            {
                                AddProcessIDXMLFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.LogFile:
                            if (MaximumLoggableSeverityLevelLogFile >= entrySeverity && DestinationEnabledLogFile == true)
                            {
                                AddLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.ErrorLogFile:
                            if (MaximumLoggableSeverityLevelErrorLogFile >= entrySeverity && DestinationEnabledErrorLogFile == true)
                            {
                                if (entrySeverity == ApplicationLogEntrySeverities.ErrorOccurred)
                                {
                                    AddErrorLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                                }
                            }
                            break;
                        case ApplicationLogEntryDestinations.ProcessID_LogFile:
                            if (MaximumLoggableSeverityLevelProcessIDLogFile >= entrySeverity && DestinationEnabledProcessIDLogFile == true)
                            {
                                AddProcessIDLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.XMLFile:
                            if (MaximumLoggableSeverityLevelXMLFile >= entrySeverity && DestinationEnabledXMLFile == true)
                            {
                                AddXMLFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Email:
                            if (MaximumLoggableSeverityLevelEmail >= entrySeverity && DestinationEnabledEmail == true)
                            {
                                AddEmailEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Log4Net:
                            if (MaximumLoggableSeverityLevelLog4Net >= entrySeverity && DestinationEnabledLog4Net == true)
                            {
                                AddLog4NetEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Broadcast:
                            if (MaximumLoggableSeverityLevelBroadcast >= entrySeverity && DestinationEnabledBroadcast == true)
                            {
                                AddBroadcastEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Console:
                            if (MaximumLoggableSeverityLevelConsole >= entrySeverity && DestinationEnabledConsole == true)
                            {
                                AddConsoleEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                            //					case ApplicationLogEntryDestinations.DB:
                            //						if(MaximumLoggableSeverityLevelDB >= entrySeverity && DestinationEnabledDB == true)
                            //						{
                            //							AddDBEntry(processID, entrySeverity,entryEvent,entryDescription);
                            //						}
                            //						break;
                    }
                }
            }
            catch (Exception objException)
            {
                //TODO: log to event log
                try
                {
                    //if (!EventLog.SourceExists("VRFramework"))
                    //{
                    //    EventLog.CreateEventSource("VRFramework", "Application");
                        

                    //}

                    //EventLog.WriteEntry("VRFramework", "FS.Common.Diagnostics.Loggers.ApplicationLog.AddEntry ERROR: " + objException.Message);

                }
                catch (Exception  )
                {
                    //give up for now, I don't want the logs to mess up the app...
                }
            }
        }



        private async Task AddEntryAsync(Objects.SessionContext sessionContext, object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            //check current date against datefoldername,  update and validate directory if necessary
            //iterate through all destinations, write to each
            try
            {
                ValidateApplicationLogDirectory();
                ApplicationLogEntryDestinations[] destinationArray = (ApplicationLogEntryDestinations[])Destinations.ToArray(typeof(ApplicationLogEntryDestinations));

                for (int x = 0; x <= Destinations.Count - 1; x++)
                {
                    switch (destinationArray[x])
                    {
                        case ApplicationLogEntryDestinations.EventViewer:
                            if ((MaximumLoggableSeverityLevelEventViewer >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced  >= (int)entrySeverity)) && DestinationEnabledEventViewer == true)
                            {
                                AddEventViewerEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.AzureCloud:
                            if ((MaximumLoggableSeverityLevelAzureCloud >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledAzureCloud == true)
                            {
                                AddAzureCloudEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.ProcessID_XMLFile:
                            if ((MaximumLoggableSeverityLevelProcessIDXMLFile >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledProcessIDXMLFile == true)
                            {
                                AddProcessIDXMLFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.LogFile:
                            if ((MaximumLoggableSeverityLevelLogFile >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledLogFile == true)
                            {
                                AddLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.ErrorLogFile:
                            if ((MaximumLoggableSeverityLevelErrorLogFile >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledErrorLogFile == true)
                            {
                                if (entrySeverity == ApplicationLogEntrySeverities.ErrorOccurred)
                                {
                                    AddErrorLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                                }
                            }
                            break;
                        case ApplicationLogEntryDestinations.ProcessID_LogFile:
                            if ((MaximumLoggableSeverityLevelProcessIDLogFile >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledProcessIDLogFile == true)
                            {
                                AddProcessIDLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.XMLFile:
                            if ((MaximumLoggableSeverityLevelXMLFile >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledXMLFile == true)
                            {
                                AddXMLFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Email:
                            if ((MaximumLoggableSeverityLevelEmail >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledEmail == true)
                            {
                                AddEmailEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Log4Net:
                            if ((MaximumLoggableSeverityLevelLog4Net >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && DestinationEnabledLog4Net == true)
                            {
                                AddLog4NetEntry(processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Broadcast:
                            if ((MaximumLoggableSeverityLevelBroadcast >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && (DestinationEnabledBroadcast == true ||(sessionContext.LoggingBroadcastForcedSet && sessionContext.LoggingBroadcastForced)))
                            {
                                await AddBroadcastEntryAsync(sessionContext, processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                        case ApplicationLogEntryDestinations.Console:
                            if ((MaximumLoggableSeverityLevelConsole >= entrySeverity || (sessionContext.LoggingVerboseLevelForcedSet && sessionContext.LoggingVerboseLevelForced >= (int)entrySeverity)) && (DestinationEnabledConsole == true))
                            {
                                await AddConsoleEntryAsync(sessionContext, processID, entrySeverity, entryEvent, entryDescription);
                            }
                            break;
                            //					case ApplicationLogEntryDestinations.DB:
                            //						if(MaximumLoggableSeverityLevelDB >= entrySeverity && DestinationEnabledDB == true)
                            //						{
                            //							AddDBEntry(processID, entrySeverity,entryEvent,entryDescription);
                            //						}
                            //						break;
                    }
                }
            }
            catch (Exception objException)
            {
                //TODO: log to event log
                try
                {
                    //if (!EventLog.SourceExists("VRFramework"))
                    //{
                    //    EventLog.CreateEventSource("VRFramework", "Application");


                    //}

                    //EventLog.WriteEntry("VRFramework", "FS.Common.Diagnostics.Loggers.ApplicationLog.AddEntry ERROR: " + objException.Message);

                }
                catch (Exception)
                {
                    //give up for now, I don't want the logs to mess up the app...
                }
            }
        }
        /// <summary>
        /// This is used to build one string that contains all the log entry data, formatted for the event viewer.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        /// <returns>A log entry string formatted for the event viewer</returns>
        private string BuildEventViewerEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = "";
            string entryDescriptionString = string.Empty;
            XmlNode entryDescriptionXmlNode = null;
            System.Guid processUID = System.Guid.Empty;
            string processIDString = "";

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (this._useUTCDateTime)
            {
                if (entryDescription is System.String)
                {
                    entryDescriptionString = (string)entryDescription;
                    logEntryLine = "UTC Datetime:".PadRight(25) + System.DateTime.Now.ToUniversalTime().ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionString + "\r\n";
                }
                if (entryDescription is XmlNode)
                {
                    entryDescriptionXmlNode = (XmlNode)entryDescription;
                    logEntryLine = "UTC Datetime:".PadRight(25) + System.DateTime.Now.ToUniversalTime().ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionXmlNode.OuterXml.ToString() + "\r\n";
                }
            }
            else
            {
                if (entryDescription is System.String)
                {
                    entryDescriptionString = (string)entryDescription;
                    logEntryLine = "Datetime:".PadRight(25) + System.DateTime.Now.ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionString + "\r\n";
                }
                if (entryDescription is XmlNode)
                {
                    entryDescriptionXmlNode = (XmlNode)entryDescription;
                    logEntryLine = "Datetime:".PadRight(25) + System.DateTime.Now.ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionXmlNode.OuterXml.ToString() + "\r\n";
                }
            }
            return logEntryLine;
        }
        /// <summary>
        /// Used to send a log entry to the Event Viewer.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        private void AddEventViewerEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
            logEntryLine = BuildEventViewerEntry(processID, entrySeverity, entryEvent, entryDescription);
            //switch (entrySeverity)
            //{
            //    case ApplicationLogEntrySeverities.ErrorOccurred:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Error);
            //        break;
            //    case ApplicationLogEntrySeverities.Information_HighDetail:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Information);
            //        break;
            //    case ApplicationLogEntrySeverities.Information_LowDetail:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Information);
            //        break;
            //    case ApplicationLogEntrySeverities.Information_MidDetail:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Information);
            //        break;
            //    case ApplicationLogEntrySeverities.Warning:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Warning);
            //        break;
            //}
        }





        /// <summary>
        /// This is used to build one string that contains all the log entry data, formatted for the event viewer.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        /// <returns>A log entry string formatted for the event viewer</returns>
        private string BuildAzureCloudEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = "";
            string entryDescriptionString = string.Empty;
            XmlNode entryDescriptionXmlNode = null;
            System.Guid processUID = System.Guid.Empty;
            string processIDString = "";

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (this._useUTCDateTime)
            {
                if (entryDescription is System.String)
                {
                    entryDescriptionString = (string)entryDescription;
                    logEntryLine = "UTC Datetime:".PadRight(25) + System.DateTime.Now.ToUniversalTime().ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionString + "\r\n";
                }
                if (entryDescription is XmlNode)
                {
                    entryDescriptionXmlNode = (XmlNode)entryDescription;
                    logEntryLine = "UTC Datetime:".PadRight(25) + System.DateTime.Now.ToUniversalTime().ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionXmlNode.OuterXml.ToString() + "\r\n";
                }
            }
            else
            {
                if (entryDescription is System.String)
                {
                    entryDescriptionString = (string)entryDescription;
                    logEntryLine = "Datetime:".PadRight(25) + System.DateTime.Now.ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionString + "\r\n";
                }
                if (entryDescription is XmlNode)
                {
                    entryDescriptionXmlNode = (XmlNode)entryDescription;
                    logEntryLine = "Datetime:".PadRight(25) + System.DateTime.Now.ToString() + "\r\n" + "ApplicationName:".PadRight(20) + ApplicationName.PadRight(40) + "\r\n" + "ProcessID:".PadRight(20) + processIDString.PadRight(40) + "\r\n" + "Severity:".PadRight(27) + ApplicationLogEntrySeveritiesToString(entrySeverity) + "\r\n" + "Event:".PadRight(27) + ApplicationLogEntryEventsToString(entryEvent) + "\r\n" + "Description:".PadRight(25) + entryDescriptionXmlNode.OuterXml.ToString() + "\r\n";
                }
            }
            return logEntryLine;
        }
        /// <summary>
        /// Used to send a log entry to the Event Viewer.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        private void AddAzureCloudEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
            logEntryLine = BuildAzureCloudEntry(processID, entrySeverity, entryEvent, entryDescription);
            System.Diagnostics.Trace.WriteLine(logEntryLine);
            //switch (entrySeverity)
            //{
            //    case ApplicationLogEntrySeverities.ErrorOccurred:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Error);
            //        break;
            //    case ApplicationLogEntrySeverities.Information_HighDetail:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Information);
            //        break;
            //    case ApplicationLogEntrySeverities.Information_LowDetail:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Information);
            //        break;
            //    case ApplicationLogEntrySeverities.Information_MidDetail:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Information);
            //        break;
            //    case ApplicationLogEntrySeverities.Warning:
            //        EventLog.WriteEntry(ApplicationName, logEntryLine, System.Diagnostics.EventLogEntryType.Warning);
            //        break;
            //}
        }


        /// <summary>
        /// Used to create a string describing the format of a log entry in a log file.
        /// </summary>
        /// <returns>A log entry header string formatted for the event viewer</returns>
        private string BuildLogFileHeaderEntry()
        {
            string logEntryLine = "";
            string delimiter = " :: ";

            if (this._useUTCDateTime)
            {
                logEntryLine = "UTC DateTime".PadRight(22) + delimiter + "Application Name".PadRight(40) + delimiter + "Process ID".PadRight(36) + delimiter + "Log Entry Severity".PadRight(22) + delimiter + "Log Entry Event".PadRight(25) + delimiter + "Description" + "\r\n";
            }
            else
            {
                logEntryLine = "DateTime".PadRight(22) + delimiter + "Application Name".PadRight(40) + delimiter + "Process ID".PadRight(36) + delimiter + "Log Entry Severity".PadRight(22) + delimiter + "Log Entry Event".PadRight(25) + delimiter + "Description" + "\r\n";
            }
            return logEntryLine;
        }

        /// <summary>
        /// This is used to build one string that contains all the log entry data, formatted for a log file.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        /// <returns>A log entry string formatted for log file</returns>
        private string BuildLogFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = "";
            string entryDescriptionString = string.Empty;
            string delimiter = " :: ";
            XmlNode entryDescriptionXmlNode = null;
            System.Guid processUID = System.Guid.Empty;
            string processIDString = "";

            string strTimeFormatted = "";

            if (this._useUTCDateTime)
            {
                strTimeFormatted = System.DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fffff").PadRight(22);

            }
            else
            {
                strTimeFormatted = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff").PadRight(22);

            }

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (entryDescription is System.String)
            {
                entryDescriptionString = (string)entryDescription;
                logEntryLine = strTimeFormatted + delimiter + ApplicationName.PadRight(40) + delimiter + processIDString.PadRight(80) + delimiter + ApplicationLogEntrySeveritiesToString(entrySeverity).PadRight(22) + delimiter + ApplicationLogEntryEventsToString(entryEvent).PadRight(25) + delimiter + entryDescriptionString + "\r\n";
            }
            else if (entryDescription is XmlNode)
            {
                entryDescriptionXmlNode = (XmlNode)entryDescription;
                logEntryLine = strTimeFormatted + delimiter + ApplicationName.PadRight(40) + delimiter + processIDString.PadRight(80) + delimiter + ApplicationLogEntrySeveritiesToString(entrySeverity).PadRight(22) + delimiter + ApplicationLogEntryEventsToString(entryEvent).PadRight(25) + delimiter + entryDescriptionXmlNode.OuterXml.ToString() + "\r\n";
            }
            else
            {
                logEntryLine = strTimeFormatted + delimiter + ApplicationName.PadRight(40) + delimiter + processIDString.PadRight(80) + delimiter + ApplicationLogEntrySeveritiesToString(entrySeverity).PadRight(22) + delimiter + ApplicationLogEntryEventsToString(entryEvent).PadRight(25) + delimiter + entryDescription.ToString() + "\r\n";

            }

            return logEntryLine;
        }

        private string BuildLog4NetFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = "";
            string entryDescriptionString = string.Empty;
            string delimiter = " :: ";
            XmlNode entryDescriptionXmlNode = null;
            System.Guid processUID = System.Guid.Empty;
            string processIDString = ""; 

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (entryDescription is System.String)
            {
                entryDescriptionString = (string)entryDescription;
                logEntryLine =  ApplicationName.PadRight(40) + delimiter + processIDString.PadRight(46) + delimiter + ApplicationLogEntryEventsToString(entryEvent).PadRight(25) + delimiter + entryDescriptionString;
            }
            else if (entryDescription is XmlNode)
            {
                entryDescriptionXmlNode = (XmlNode)entryDescription;
                logEntryLine = ApplicationName.PadRight(40) + delimiter + processIDString.PadRight(46) + delimiter + ApplicationLogEntryEventsToString(entryEvent).PadRight(25) + delimiter + entryDescriptionXmlNode.OuterXml.ToString();
            }
            else
            {
                logEntryLine = ApplicationName.PadRight(40) + delimiter + processIDString.PadRight(46) + delimiter + ApplicationLogEntryEventsToString(entryEvent).PadRight(25) + delimiter + entryDescription.ToString();

            }

            return logEntryLine;
        }
        /// <summary>
        /// Used to send a log entry to a log file
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        private void AddLogFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;

            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + LogFileNameBase + ".log") == false)
            {
                logEntryLine = BuildLogFileHeaderEntry();
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + LogFileNameBase + ".log", logEntryLine);
            }
            try
            {
                logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + LogFileNameBase + ".log", logEntryLine);
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddLog4NetEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
             
            try
            {
                logEntryLine = BuildLog4NetFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                switch (entrySeverity)
                {
                    case ApplicationLogEntrySeverities.Information_HighDetail:
                        _log4Net.Debug(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_MidDetail:
                        _log4Net.Info(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_LowDetail:
                        _log4Net.Info(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Warning:
                        _log4Net.Warn(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.ErrorOccurred:
                        _log4Net.Error(logEntryLine);
                        break;
                    default:
                        throw new Exception("severity not handled in log4net");
                }
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void AddBroadcastEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;

            try
            {
                logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription).Replace("\r\n","");

                Console.WriteLine(logEntryLine);
                switch (entrySeverity)
                {
                    case ApplicationLogEntrySeverities.Information_HighDetail:
                        FS.Common.PubSub.Publisher.SendMessage("log",logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("app:" + ApplicationName, logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_MidDetail:
                        FS.Common.PubSub.Publisher.SendMessage("log", logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("app:" + ApplicationName, logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_LowDetail:
                        FS.Common.PubSub.Publisher.SendMessage("log", logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("app:" + ApplicationName, logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Warning:
                        FS.Common.PubSub.Publisher.SendMessage("log", logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("app:" + ApplicationName, logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("warning", logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.ErrorOccurred:
                        FS.Common.PubSub.Publisher.SendMessage("log", logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("app:" + ApplicationName, logEntryLine);
                        FS.Common.PubSub.Publisher.SendMessage("error", logEntryLine);
                        break;
                    default:
                        throw new Exception("severity not handled in Broadcast");
                }
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task AddBroadcastEntryAsync(Objects.SessionContext sessionContext, object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
            //string processNote = string.Empty;
            //if (processID is null)
            //{
            //    processID = sessionContext.UserName;
            //}
            //if (processID is string && processID.ToString() == string.Empty)
            //{
            //    processID = sessionContext.UserName;
            //}

            try
            {
                logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription).Replace("\r\n", "");
                switch (entrySeverity)
                {
                    case ApplicationLogEntrySeverities.Information_HighDetail:
                        await FS.Common.PubSub.Publisher.SendMessageAsync("log", logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("app:" + ApplicationName, logEntryLine);
                        if(sessionContext.UserName.Length > 0)
                            await FS.Common.PubSub.Publisher.SendMessageAsync("user:" + sessionContext.UserName, logEntryLine);
                        if (sessionContext.LoggingDimensions != null)
                        {
                            foreach(string dimensionName in sessionContext.LoggingDimensions.Keys)
                            { 
                                string dimensionValue = sessionContext.LoggingDimensions[dimensionName];
                                await FS.Common.PubSub.Publisher.SendMessageAsync("dimension@" + dimensionName + ":" + dimensionValue, logEntryLine);
                            }
                        }
                        break;
                    case ApplicationLogEntrySeverities.Information_MidDetail:
                        await FS.Common.PubSub.Publisher.SendMessageAsync("log", logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("app:" + ApplicationName, logEntryLine);
                        if (sessionContext.UserName.Length > 0)
                            await FS.Common.PubSub.Publisher.SendMessageAsync("user:" + sessionContext.UserName, logEntryLine);

                        if (sessionContext.LoggingDimensions != null)
                        {
                            foreach (string dimensionName in sessionContext.LoggingDimensions.Keys)
                            {
                                string dimensionValue = sessionContext.LoggingDimensions[dimensionName];
                                await FS.Common.PubSub.Publisher.SendMessageAsync("dimension@" + dimensionName + ":" + dimensionValue, logEntryLine);
                            }
                        }
                        break;
                    case ApplicationLogEntrySeverities.Information_LowDetail:
                        await FS.Common.PubSub.Publisher.SendMessageAsync("log", logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("app:" + ApplicationName, logEntryLine);
                        if (sessionContext.UserName.Length > 0)
                            await FS.Common.PubSub.Publisher.SendMessageAsync("user:" + sessionContext.UserName, logEntryLine);

                        if (sessionContext.LoggingDimensions != null)
                        {
                            foreach (string dimensionName in sessionContext.LoggingDimensions.Keys)
                            {
                                string dimensionValue = sessionContext.LoggingDimensions[dimensionName];
                                await FS.Common.PubSub.Publisher.SendMessageAsync("dimension@" + dimensionName + ":" + dimensionValue, logEntryLine);
                            }
                        }
                        break;
                    case ApplicationLogEntrySeverities.Warning:
                        await FS.Common.PubSub.Publisher.SendMessageAsync("log", logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("app:" + ApplicationName, logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("warning", logEntryLine);
                        if (sessionContext.UserName.Length > 0)
                            await FS.Common.PubSub.Publisher.SendMessageAsync("user:" + sessionContext.UserName, logEntryLine);

                        if (sessionContext.LoggingDimensions != null)
                        {
                            foreach (string dimensionName in sessionContext.LoggingDimensions.Keys)
                            {
                                string dimensionValue = sessionContext.LoggingDimensions[dimensionName];
                                await FS.Common.PubSub.Publisher.SendMessageAsync("dimension@" + dimensionName + ":" + dimensionValue, logEntryLine);
                            }
                        }
                        break;
                    case ApplicationLogEntrySeverities.ErrorOccurred:
                        await FS.Common.PubSub.Publisher.SendMessageAsync("log", logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("app:" + ApplicationName, logEntryLine);
                        await FS.Common.PubSub.Publisher.SendMessageAsync("error", logEntryLine);
                        if (sessionContext.UserName.Length > 0)
                            await FS.Common.PubSub.Publisher.SendMessageAsync("user:" + sessionContext.UserName, logEntryLine);

                        if (sessionContext.LoggingDimensions != null)
                        {
                            foreach (string dimensionName in sessionContext.LoggingDimensions.Keys)
                            {
                                string dimensionValue = sessionContext.LoggingDimensions[dimensionName];
                                await FS.Common.PubSub.Publisher.SendMessageAsync("dimension@" + dimensionName + ":" + dimensionValue, logEntryLine);
                            }
                        }
                        break;
                    default:
                        throw new Exception("severity not handled in Broadcast");
                }
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void AddConsoleEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;

            try
            {
                logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription).Replace("\r\n", "");

                Console.WriteLine(logEntryLine);
                switch (entrySeverity)
                {
                    case ApplicationLogEntrySeverities.Information_HighDetail:
                        Console.WriteLine(logEntryLine); 
                        break;
                    case ApplicationLogEntrySeverities.Information_MidDetail:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_LowDetail:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Warning:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.ErrorOccurred:
                        Console.WriteLine(logEntryLine);
                        break;
                    default:
                        throw new Exception("severity not handled in Console");
                }
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task AddConsoleEntryAsync(Objects.SessionContext sessionContext, object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
            //string processNote = string.Empty;
            //if (processID is null)
            //{
            //    processID = sessionContext.UserName;
            //}
            //if (processID is string && processID.ToString() == string.Empty)
            //{
            //    processID = sessionContext.UserName;
            //}

            try
            {
                logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription).Replace("\r\n", "");
                switch (entrySeverity)
                {
                    case ApplicationLogEntrySeverities.Information_HighDetail:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_MidDetail:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Information_LowDetail:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.Warning:
                        Console.WriteLine(logEntryLine);
                        break;
                    case ApplicationLogEntrySeverities.ErrorOccurred:
                        Console.WriteLine(logEntryLine);
                        break;
                    default:
                        throw new Exception("severity not handled in Console");
                }
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void AddProcessIDLogFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
            System.Guid processUID = System.Guid.Empty;
            string processIDString = "";

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + processIDString + "_" + LogFileNameBase + ".log") == false)
            {
                logEntryLine = BuildLogFileHeaderEntry();
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + processIDString + "_" + LogFileNameBase + ".log", logEntryLine);
            }
            try
            {
                logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + processIDString + "_" + LogFileNameBase + ".log", logEntryLine);
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Used to send a log entry to multiple error files.  Theses error files exists in the root log folder, the application log folder, and the dated applicaiton log folder.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        private void AddErrorLogFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {
            string logEntryLine = string.Empty;
            string logHeaderLine = string.Empty;

            logEntryLine = BuildLogFileEntry(processID, entrySeverity, entryEvent, entryDescription);
            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + LogFileNameBase + ".log") == false)
            {
                logHeaderLine = BuildLogFileHeaderEntry();
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + LogFileNameBase + ".log", logHeaderLine);
            }
            FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + LogFileNameBase + ".err", logEntryLine);
            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + LogFileNameBase + ".log") == false)
            {
                logHeaderLine = BuildLogFileHeaderEntry();
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + LogFileNameBase + ".log", logHeaderLine);
            }
            FS.Common.IO.File.AppendToFile(LogRootDirectory + "\\" + ApplicationName + "\\" + LogFileNameBase + ".err", logEntryLine);
            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + LogFileNameBase + ".log") == false)
            {
                logHeaderLine = BuildLogFileHeaderEntry();
                FS.Common.IO.File.AppendToFile(GetApplicationLogDirectory() + LogFileNameBase + ".log", logHeaderLine);
            }
            FS.Common.IO.File.AppendToFile(LogRootDirectory + "\\" + LogFileNameBase + ".err", logEntryLine);

        }


        /// <summary>
        /// Used to send a log entry to a log file in an xml format.   one file exists for every processid.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="EntryDescription">This holds any data that should be logged.</param>
        private void AddProcessIDXMLFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object EntryDescription)
        {
            XmlDocument xmlLogDocument = new XmlDocument();
            System.Guid processUID = System.Guid.Empty;
            string processIDString = "";

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + processIDString + "_" + LogFileNameBase + ".xml") == true)
            {
                xmlLogDocument.Load(GetApplicationLogDirectory() + processIDString + "_" + LogFileNameBase + ".xml");
            }
            else
            {
                xmlLogDocument = new XmlDocument();
                xmlLogDocument.AppendChild(xmlLogDocument.CreateElement("APPLICATIONLOGS"));
            }

            XmlElement XmlLogEntryRoot;
            XmlElement XmlLogEntryChild;
            XmlNode test;

            XmlLogEntryRoot = xmlLogDocument.CreateElement("APPLICATIONLOG");
            XmlLogEntryChild = xmlLogDocument.CreateElement("ApplicationName");
            XmlLogEntryChild.InnerText = ApplicationName;
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("PROCESSID");
            XmlLogEntryChild.InnerText = processIDString;
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("UTCDATETIME");
            XmlLogEntryChild.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("DATETIME");
            XmlLogEntryChild.InnerText = System.DateTime.Now.ToString();
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("SEVERITY");
            XmlLogEntryChild.InnerText = ApplicationLogEntrySeveritiesToString(entrySeverity);
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("EVENT");
            XmlLogEntryChild.InnerText = ApplicationLogEntryEventsToString(entryEvent);
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("DESCRIPTION");
            if (EntryDescription is System.String)
            {
                XmlLogEntryChild.InnerText = (string)EntryDescription;
            }
            else
            {
                test = (XmlNode)EntryDescription;
                XmlLogEntryChild.InnerXml = test.OuterXml;
                //XmlLogEntryChild.AppendChild ((XmlNode)EntryDescription);
                //XmlLogEntryChild.AppendChild (test);
            }

            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            xmlLogDocument.DocumentElement.AppendChild(XmlLogEntryRoot);
            try
            {
                xmlLogDocument.Save(GetApplicationLogDirectory() + processIDString + "_" + LogFileNameBase + ".xml");
            }
            catch (System.IO.IOException ex)
            {
                string exdesc = "";
                exdesc = ex.Message;
            }
        }
        private void AddXMLFileEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object EntryDescription)
        {
            XmlDocument xmlLogDocument = new XmlDocument();
            System.Guid processUID = System.Guid.Empty;
            string processIDString = "";

            if (processID is System.Guid)
            {
                processUID = (System.Guid)processID;
                processIDString = processUID.ToString();
            }
            if (processID is string)
            {
                processIDString = (string)processID;
            }

            if (FS.Common.IO.File.FileExists(GetApplicationLogDirectory() + LogFileNameBase + ".xml") == true)
            {
                xmlLogDocument.Load(GetApplicationLogDirectory() + LogFileNameBase + ".xml");
            }
            else
            {
                xmlLogDocument = new XmlDocument();
                xmlLogDocument.AppendChild(xmlLogDocument.CreateElement("APPLICATIONLOGS"));
            }

            XmlElement XmlLogEntryRoot;
            XmlElement XmlLogEntryChild;
            XmlNode test;

            XmlLogEntryRoot = xmlLogDocument.CreateElement("APPLICATIONLOG");
            XmlLogEntryChild = xmlLogDocument.CreateElement("ApplicationName");
            XmlLogEntryChild.InnerText = ApplicationName;
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("PROCESSID");
            XmlLogEntryChild.InnerText = processIDString;
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("UTCDATETIME");
            XmlLogEntryChild.InnerText = System.DateTime.Now.ToUniversalTime().ToString();
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("DATETIME");
            XmlLogEntryChild.InnerText = System.DateTime.Now.ToString();
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("SEVERITY");
            XmlLogEntryChild.InnerText = ApplicationLogEntrySeveritiesToString(entrySeverity);
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("EVENT");
            XmlLogEntryChild.InnerText = ApplicationLogEntryEventsToString(entryEvent);
            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            XmlLogEntryChild = xmlLogDocument.CreateElement("DESCRIPTION");
            if (EntryDescription is System.String)
            {
                XmlLogEntryChild.InnerText = (string)EntryDescription;
            }
            else
            {
                test = (XmlNode)EntryDescription;
                XmlLogEntryChild.InnerXml = test.OuterXml;
                //XmlLogEntryChild.AppendChild ((XmlNode)EntryDescription);
                //XmlLogEntryChild.AppendChild (test);
            }

            XmlLogEntryRoot.AppendChild(XmlLogEntryChild);
            xmlLogDocument.DocumentElement.AppendChild(XmlLogEntryRoot);
            try
            {
                xmlLogDocument.Save(GetApplicationLogDirectory() + LogFileNameBase + ".xml");
            }
            catch (System.IO.IOException ex)
            {
                string exdesc = "";
                exdesc = ex.Message;
            }
        }


        /// <summary>
        /// used to create a string that describes an event.
        /// </summary>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <returns>A string containing the event name.</returns>
        private string ApplicationLogEntryEventsToString(ApplicationLogEntryEvents entryEvent)
        {
            string result = "";
            switch (entryEvent)
            {
                case ApplicationLogEntryEvents.AcquireRequestState:
                    result = "AcquireRequestState";
                    break;
                case ApplicationLogEntryEvents.AuthenticateRequest:
                    result = "AuthenticateRequest";
                    break;
                case ApplicationLogEntryEvents.AuthorizeRequest:
                    result = "AuthorizeRequest";
                    break;
                case ApplicationLogEntryEvents.BeginRequest:
                    result = "BeginRequest";
                    break;
                case ApplicationLogEntryEvents.Disposed:
                    result = "Disposed";
                    break;
                case ApplicationLogEntryEvents.EndRequest:
                    result = "EndRequest";
                    break;
                case ApplicationLogEntryEvents.ErrorOccurred:
                    result = "Error";
                    break;
                case ApplicationLogEntryEvents.Initialize:
                    result = "Initialize";
                    break;
                case ApplicationLogEntryEvents.PostRequestHandlerExecute:
                    result = "PostRequestHandlerExecute";
                    break;
                case ApplicationLogEntryEvents.PreRequestHandlerExecute:
                    result = "PreRequestHandlerExecute";
                    break;
                case ApplicationLogEntryEvents.PreSendRequestContent:
                    result = "PreSendRequestContent";
                    break;
                case ApplicationLogEntryEvents.PreSendRequestHeaders:
                    result = "PreSendRequestHeaders";
                    break;
                case ApplicationLogEntryEvents.ReleaseRequestState:
                    result = "ReleaseRequestState";
                    break;
                case ApplicationLogEntryEvents.ResolveRequestCache:
                    result = "ResolveRequestCache";
                    break;
                case ApplicationLogEntryEvents.Undefined:
                    result = "";
                    break;
                case ApplicationLogEntryEvents.UpdateRequestCache:
                    result = "UpdateRequestCache";
                    break;
                default:
                    result = "Unknown";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Used to create a string that describes a severity level.
        /// </summary>
        /// <param name="entrySeverity">This describes the severity of the log entry.</param>
        /// <returns>A string containing the severity name.</returns>
        private string ApplicationLogEntrySeveritiesToString(ApplicationLogEntrySeverities entrySeverity)
        {
            switch (entrySeverity)
            {
                case ApplicationLogEntrySeverities.Information_HighDetail:
                    return "Information_HighDetail";
                case ApplicationLogEntrySeverities.Information_MidDetail:
                    return "Information_MidDetail";
                case ApplicationLogEntrySeverities.Information_LowDetail:
                    return "Information_LowDetail";
                case ApplicationLogEntrySeverities.Warning:
                    return "Warning ";
                case ApplicationLogEntrySeverities.ErrorOccurred:
                    return "Error";
                default:
                    return "Undefined";
            }
        }



        /// <summary>
        /// Used to send a log entry to an email address.
        /// </summary>
        /// <param name="processID">This is used to distinguish different processes through or instances of the application.</param>
        /// <param name="entrySeverity">This describes the severity of the log entry. A high detail entry is less likely to be written than an error or a low detail entry.</param>
        /// <param name="entryEvent">This describes the event being logged.</param>
        /// <param name="entryDescription">This holds any data that should be logged.</param>
        private void AddEmailEntry(object processID, ApplicationLogEntrySeverities entrySeverity, ApplicationLogEntryEvents entryEvent, object entryDescription)
        {

            //String messageBuffer  = "";

            //System.Web.Mail.MailMessage mailhandle = null;

            //// send email about the error
            //if (FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.ToEmailNotification", "").Length != 0
            //    && FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.SMTPHost", "").Length != 0)
            //{
            //    string logEntryLine = string.Empty;
            //    logEntryLine = BuildEventViewerEntry(processID, entrySeverity, entryEvent, entryDescription);

            //    mailhandle = new System.Web.Mail.MailMessage();
            //    mailhandle.To = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.ToEmailNotification", "");
            //    mailhandle.From = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.FromEmailNotification", "");
            //    mailhandle.Cc = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.CCEmailNotification", "");

            //    mailhandle.Subject = "BoostCTR " + ApplicationName + " ApplicationLog Notification";

            //    mailhandle.Body = logEntryLine;

            //    System.Web.Mail.SmtpMail.SmtpServer.Insert(0, FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("ApplicationLog.Email.SMTPHost", ""));
            //    System.Web.Mail.SmtpMail.Send(mailhandle);
            //}
        }
        #endregion



        //
        //		public void AddEntry(ApplicationLogEntryEvents entryEvent,string entryDescription)
        //		{
        //			AddEntry(ApplicationLogEntrySeverities.Information_MidDetail, entryEvent, entryDescription);
        //		}
        //		public void AddEntry(ApplicationLogEntryEvents entryEvent,XmlNode entryDescription)
        //		{
        //			AddEntry(ApplicationLogEntrySeverities.Information_MidDetail, entryEvent, entryDescription);
        //		}
        //		public void AddEntry(ApplicationLogEntryEvents entryEvent)
        //		{
        //			AddEntry(ApplicationLogEntrySeverities.Information_MidDetail, entryEvent, "");
        //		}
        //		public void AddEntry(string entryDescription)
        //		{
        //			AddEntry(ApplicationLogEntrySeverities.Information_MidDetail, ApplicationLogEntryEvents.Undefined, "");
        //		}
        //		public void AddEntry(XmlNode entryDescription)
        //		{
        //			AddEntry(ApplicationLogEntrySeverities.Information_MidDetail, ApplicationLogEntryEvents.Undefined, "");
        //		}
        //		public void AddEntry(Exception ex)
        //		{
        //			AddEntry(ApplicationLogEntrySeverities.ErrorOccurred, ApplicationLogEntryEvents.Undefined, ex.Description);
        //		}

        //boolean CacheEnabled
        //StartCache()
        //ClearCache()
        //GetCache()
        //int CacheCapacity
        //FlushCache()
        //collection LogEntries

        //AddEntryDestination(Destination)
        //now	//AddEntryDestination(Severity, Destination, string Filename)
        //now	//AddEntryDestination(Destination, string Filename)
        //now	//collection InformationEntryDestinations
        //collection WarningEntryDestinations
        //now	//collection ErrorEntryDestinations
        //string LoggingNamespaceTypeMethod
        //string ProcessID1
        //string ProcessID2
        //AddEventViewerEntry()
        //AddDBEntry()


        //comma delimited list of destinations for each severity level	
        //if no description sent in for event, log tick


        //db table: 
        //ApplicationLog

        //columns:	
        //ApplicationName or ApplicationID
        //LoggingNamespaceTypeMethod
        //ProcessID1
        //ProcessID2
        //ApplicationEventLookupID
        //ApplicationLogSeverityLookupID
        //Description
        //ComputerName
        //IPAddress
        //DateTime
        //InsertDateTime
        //InsertApp
        //UpdateDateTime
        //UpdateApp
    }

}
