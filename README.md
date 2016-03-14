# Logstash Toolbox

Logstash provider for the ASP.NET Core logging framework.


## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

- [Installation](#installation)
- [Startup](#startup)
- [Logging](#logging)
- [Correlation](#correlation)
- [LogMessage](#logmessage)
- [Sample](#sample)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Installation

To add the toolbox to a project, you add the package to the project.json :

``` json 
"dependencies": {
    "Toolbox.Logstash":  "0.7.0"
 }
``` 

In Visual Studio you can use the NuGet Package Manager to do this.


## Startup

To start using the Logstash toolbox, call the _AddLogstashHttp_ extension method on the _**ILoggerFactory**_. 
You have to pass in the IApplicationBuilder instance, so this is best done in de Configure method of the Startup class.   

You can pass in the needed options, like this :

``` csharp 
loggerFactory.AddLogstashHttp(app, opt => 
            {
                opt.AppId = "MyApplication";
                opt.Index = "MyApplication";
                opt.MessageVersion = "1";
                opt.MinimumLevel = LogLevel.Information;
                opt.Url = "http://my.logstash.url/api/v2/messages";
            });
```  

or specify a IConfiguration instance that contains the needed options :

``` csharp  
loggerFactory.AddLogstashHttp(Configuration.GetSection("Logstash"));   // where you have previously loaded the Configuration instance from a config file
```  

The configuration section has the following layout : 

``` json 
{
    "AppId": "MyApplication",
    "Index": "MyApplicationIndex",
    "MessageVersion": "1",
    "MinimumLevel": "Information",
    "Url": "http://my.logstash.url/api/v2/messages"   
}
```  

- **AppId**  
  The id or name of the application. This value will be used in the source of the messages.
- **Index**    
  The Logstash index to write to.
- **MessageVersion**  
  The version of your message structure. This field can be used in Logstash during parsing (different parsing logic depending on the message format).
- **MinimumLevel**  
  The minimum level that has to be written to Logstash.
- **Url**  
  The url to the Logstash HTTP endpoint.

## Logging

The Logstash toolbox adds a provider to the ASP.NET Core logging framework, so you can use the standard logging functions :

``` csharp  
Logger.LogDebug("this is a debug log message.");
Logger.LogInformation("this is an information log message.");
// ...
```  

## Correlation

One of the advantages of using the Logstash toolbox is that it adds metadata to the messages. If you want ameaningful correlation id in the messages, 
so that you can trace the messages back to their origin, add the Correlation Toolbox to your pipeline. The Correlation Toolbox will read the correlation header
from an incoming HTTP request, keep it in memory and pass it on whenever you do a further call to another API. When this chain of calls logs their messages with the 
correlation id to the Logstash logs, you can trace back the call over several messages.  
  
More info about the Correlation Toolbox is found here : https://github.com/digipolisantwerp/correlation_aspnetcore.   

## LogMessage

The message that the Logstash Toolbox sends to Logstash has the following layouyt : 

``` json  
{
    "Header": {
        "Correlation": {
            "CorrelationId":"f0b208e1-7fc4-4585-8419-a089229978a6",
            "ApplicationId":"LogstashToolboxSample"
        },
        "TimeStamp":"2016-03-13T10:51:29.5352657+01:00",
        "Source": {
            "ApplicationId":"LogstashToolboxSample",
            "ComponentId":"Microsoft.AspNet.Hosting.Internal.HostingEngine"
        },
        "IPAddress":"158.149.0.167",
        "ProcessId":"15720",
        "ThreadId":"7",
        "Index":"LogstashToolboxSample",
        "VersionNumber":"1"
    },
    "Body": {
        "User": {
            "UserId":"userid",
            "IPAddress":"158.149.0.167"
        },
        "Level":2,
        "Content":"This is an information log message.",
        "VersionNumber":"1"
    }
}
``` 

The _**Content**_ field is the message that is passed to the Log methods of the ASP.NET Core Logger. If you need more structure in this message, you can send a JSON string.

## Sample

The samples directory contains an API project that can be used as a guideline.