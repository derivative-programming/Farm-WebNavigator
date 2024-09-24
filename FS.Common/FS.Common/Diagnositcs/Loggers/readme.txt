
log4net.Layout.PatternLayout options... Since we use log4net in FS.Common.Diagnostics, many are useless
expression	value
%appdomain	[useless}the friendly name of the appdomain from which the log entry was made
%date		[use utcdate isntead]the local datetime when the log entry was made
%exception	[useless}a formatted form of the exception object in the log entry, if the entry contains an exception; otherwise, this format expression adds nothing to the log entry
%file		[useless}the file name from whichctive user logging the entry; this one is less reliable than %username; note that using %identity has a significant performance impact and I don't recommend using it
%level		the severity level of the log entry (DEBUG,INFO, etc)
%line		[useless}the source code line number from the log entry was made; note that using %file has a significant performance impact and I don't recommend using it
%identity	[useless}the user name of the a which the log entry was made; slow
%location	[useless}some rudimentary call stack information, including file name and line number at which the log entry was made; using
%logger		[useless}the name of the logger making the entry; more on this in a bit
%method		[useless}the name of the method in which the log entry was made; also slow
%message	the log message itself (don't forget this part!)
%newline	the value of Environment.NewLine
%timestamp	[useless}the milliseconds between the start of the application and the time the log entry was made
%type		[useless}the full typename of the object from which the log entry was made
%username	the Windows identity of user making the log entry; slow
%utcdate	the UTC datetime when the log entry was made
%%			a percent sign (%)

sample...
<conversionPattern value="date:%date appdomain:%appdomain exception:%exception file:%file identity:%identity line:%line location:%location logger:%logger method:%method timestamp:%timestamp type:%type username:%username utcdate:%utcdate level:%level logger:%logger - message:%message newline:%newline" />
date:2016-02-28 06:13:46,653 appdomain:LISM.CodeGenerator.vshost.exe exception: file:c:\VR\Source\hypermice\list-search\VS2013\Framework\FS.Common\Diagnositcs\Loggers\ApplicationLog.cs identity: line:972 location:FS.Common.Diagnostics.Loggers.ApplicationLog.AddLog4NetEntry(c:\VR\Source\hypermice\list-search\VS2013\Framework\FS.Common\Diagnositcs\Loggers\ApplicationLog.cs:972) logger:FS.Common.Diagnostics.Loggers.ApplicationLog method:AddLog4NetEntry timestamp:420 type:FS.Common.Diagnostics.Loggers.ApplicationLog username:vroche-2015\vroche utcdate:2016-02-28 11:13:46,653 level:DEBUG logger:FS.Common.Diagnostics.Loggers.ApplicationLog - message:2016-02-28 06:13:46.65369 :: codegenerator_ApplicationName            ::                                                :: Information_HighDetail :: Undefined                 :: Initialize
 newline:
date:2016-02-28 06:13:46,990 appdomain:LISM.CodeGenerator.vshost.exe exception: file:c:\VR\Source\hypermice\list-search\VS2013\Framework\FS.Common\Diagnositcs\Loggers\ApplicationLog.cs identity: line:972 location:FS.Common.Diagnostics.Loggers.ApplicationLog.AddLog4NetEntry(c:\VR\Source\hypermice\list-search\VS2013\Framework\FS.Common\Diagnositcs\Loggers\ApplicationLog.cs:972) logger:FS.Common.Diagnostics.Loggers.ApplicationLog method:AddLog4NetEntry timestamp:757 type:FS.Common.Diagnostics.Loggers.ApplicationLog username:vroche-2015\vroche utcdate:2016-02-28 11:13:46,990 level:DEBUG logger:FS.Common.Diagnostics.Loggers.ApplicationLog - message:2016-02-28 06:13:46.99093 :: codegenerator_ApplicationName            ::                                                :: Information_HighDetail :: Undefined                 :: Initialize
 newline:
date:2016-02-28 06:13:47,210 appdomain:LISM.CodeGenerator.vshost.exe exception: file:c:\VR\Source\hypermice\list-search\VS2013\Framework\FS.Common\Diagnositcs\Loggers\ApplicationLog.cs identity: line:972 location:FS.Common.Diagnostics.Loggers.ApplicationLog.AddLog4NetEntry(c:\VR\Source\hypermice\list-search\VS2013\Framework\FS.Common\Diagnositcs\Loggers\ApplicationLog.cs:972) logger:FS.Common.Diagnostics.Loggers.ApplicationLog method:AddLog4NetEntry timestamp:976 type:FS.Common.Diagnostics.Loggers.ApplicationLog username:vroche-2015\vroche utcdate:2016-02-28 11:13:47,210 level:DEBUG logger:FS.Common.Diagnostics.Loggers.ApplicationLog - message:2016-02-28 06:13:47.21008 :: codegenerator_ApplicationName            ::                                                :: Information_HighDetail :: Undefined                 :: Initialize
 newline:


sample config...

<?xml version="1.0" encoding="utf-8"?>
<configuration>
  
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>
   
  <appSettings> 

   
    <add key="ApplicationName" value="codegenerator_ApplicationName" />
    <add key="LogRootDirectory" value="c:\\Logs" />

    <add key="ApplicationLog.Email.Enabled" value="false" />
    <add key="ApplicationLog.ErrorLogFile.Enabled" value="true" />
    <add key="ApplicationLog.EventViewer.Enabled" value="false" />
    <add key="ApplicationLog.Log4Net.Enabled" value="true" />
    <add key="ApplicationLog.LogFile.Enabled" value="true" />
    <add key="ApplicationLog.ProcessIDLogFile.Enabled" value="true" />
    <add key="ApplicationLog.ProcessIDXMLFile.Enabled" value="true" />
    <add key="ApplicationLog.XMLFile.Enabled" value="true" />
    <add key="ApplicationLog.Broadcast.Enabled" value="true" />
    <add key="ApplicationLog.Console.Enabled" value="true" />

    <add key="ApplicationLog.Email.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.ErrorLogFile.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.EventViewer.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.Log4Net.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.LogFile.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.ProcessIDLogFile.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.ProcessIDXMLFile.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.XMLFile.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.Broadcast.MaximumLoggableSeverity" value="5" />
    <add key="ApplicationLog.Console.MaximumLoggableSeverity" value="5" />
	
    <add key="Redis.Connection" value="localhost" />

    <add key="Logentries.Token" value="d5de3043-a293-4557-937a-c928fbefcfbd " />
  </appSettings>
  
  <log4net>
    <root>
      <level value="ALL" />
      <appender-ref ref="MyConsoleAppender" />
      <appender-ref ref="MyFileAppender" />
      <appender-ref ref="RollingFileAppender" />
      <appender-ref ref="LeAppender" />
    </root>
    <appender name="MyConsoleAppender" type="log4net.Appender.ConsoleAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %appdomain %exception %file %identity %line %location %logger %method %timestamp %type %username %utcdate %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="MyFileAppender" type="log4net.Appender.FileAppender">
      <file value="C:\logs\codegenerator_ApplicationName\log4net\application.log" />
      <appendToFile value="true" />
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <file value="C:\logs\codegenerator_ApplicationName\log4net\rolling.log" />
      <appendToFile value="true" />
      <rollingStyle value="Size" />
      <maxSizeRollBackups value="5" />
      <maximumFileSize value="10MB" />
      <staticLogFileName value="true" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %level %logger - %message%newline" />
      </layout>
    </appender>
    <appender name="LeAppender" type="log4net.Appender.LogentriesAppender, LogentriesLog4net">
      <Debug value="false" />
      <HttpPut value="false" />
      <Ssl value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <param name="ConversionPattern" value="%d{ddd MMM dd HH:mm:ss zzz yyyy} %logger %: %level%, %m, " />
      </layout>
    </appender>
  </log4net>
   
</configuration>
