# PosInformatique.Net.HttpServer
**PosInformatique.Net.HttpServer** is a .NET managed library which allows developers to manage configuration of the integrated Windows HTTP server using the [HTTP Server API](https://msdn.microsoft.com/en-us/library/windows/desktop/aa364510.aspx).
This library contains the same features of the "netsh http" command line which is used by administrators to configure the integrated Windows HTTP server.

## Installing from NuGet
The **PosInformatique.Net.HttpServer** is available directly on the [NuGet](https://www.nuget.org/packages/PosInformatique.Net.HttpServer/) official website.
To download and install the library to your Visual Studio project using the following NuGet command line 
```
Install-Package PosInformatique.Net.HttpServer
```
NuGet package website: https://www.nuget.org/packages/PosInformatique.Net.HttpServer/
## Roadmap
Currently the **PosInformatique.Net.HttpServer** supports only "URL reservation" management.
I plan to develop the following features:
* SSL certificates registration support in order to register HTTPS url in the Windows HTTP server.
* PowerShell cmdlet (a new project will be create soon).

## Contributions
Do not hesitate to clone my code and submit some changes... It is a open source project, so everyone is welcome to improve this library...
By the way, I am french... So maybe you will remarks that my english is not really fluent... So do not hesitate to fix my resources strings or my documentation... Merci !
