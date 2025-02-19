### What is CoreWCF? 

CoreWCF is a port of the service side of Windows Communication Foundation (WCF) to .NET Core. The goal of this project is to enable existing WCF services to move to .NET Core.

### Package Status

The latest released packages can be found at Nuget.org:

| Package                                                                                      | NuGet Stable                                                                                     | Downloads                                                                                     |
|:---------------------------------------------------------------------------------------------|:------------------------------------------------------------------------------------------------:|:---------------------------------------------------------------------------------------------:|
| [CoreWCF.Primitives](https://www.nuget.org/packages/CoreWCF.Primitives/)                     | ![CoreWCF.Primitives](https://img.shields.io/nuget/v/CoreWCF.Primitives.svg)                     | ![CoreWCF.Primitives](https://img.shields.io/nuget/dt/CoreWCF.Primitives)                     |
| [CoreWCF.Http](https://www.nuget.org/packages/CoreWCF.Http/)                                 | ![CoreWCF.Http](https://img.shields.io/nuget/v/CoreWCF.Http.svg)                                 | ![CoreWCF.Http](https://img.shields.io/nuget/dt/CoreWCF.Http)                                 |
| [CoreWCF.NetTcp](https://www.nuget.org/packages/CoreWCF.NetTcp/)                             | ![CoreWCF.NetTcp](https://img.shields.io/nuget/v/CoreWCF.NetTcp.svg)                             | ![CoreWCF.NetTcp](https://img.shields.io/nuget/dt/CoreWCF.NetTcp)                             |
| [CoreWCF.ConfigurationManager](https://www.nuget.org/packages/CoreWCF.ConfigurationManager/) | ![CoreWCF.ConfigurationManager](https://img.shields.io/nuget/v/CoreWCF.ConfigurationManager.svg) | ![CoreWCF.ConfigurationManager](https://img.shields.io/nuget/dt/CoreWCF.ConfigurationManager) |
| [CoreWCF.WebHttp](https://www.nuget.org/packages/CoreWCF.WebHttp/) | ![CoreWCF.WebHttp](https://img.shields.io/nuget/v/CoreWCF.WebHttp.svg) | ![CoreWCF.WebHttp](https://img.shields.io/nuget/dt/CoreWCF.WebHttp) |

### Code Quality

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=CoreWCF_CoreWCF)

### Announcements

To keep up to date on what's going on with CoreWCF, you can subscribe to the [announcements](https://github.com/CoreWCF/announcements) repo to be notified about major changes and other noteworthy announcements.

### How do I get started?

* Install the Nuget packages listed above, either via the Package Manager Console or the UI.
* See [Walkthrough](Documentation/Walkthrough.md) for a step by step guide to creating a service and referencing it from a client project.
* The [Samples](https://github.com/CoreWCF/samples) repo has examples for multiple scenarios.
* The [Blog](https://corewcf.github.io/) has details on the design philosophy and a deep dive into the features are included in each new release.

### Use CoreWCF project templates (dotnet CLI or VisualStudio)

* Install CoreWCF project templates, create a directory for your project and cd inside the directory and initialize your project
```cmd
dotnet new --install CoreWCF.Templates 
dotnet new corewcf --name MyService
```
* `CoreWCF Service` project template creates a minimal ASP.NET Core web application thats exposes the well-known WCF default service using a `BasicHttpBinding`.
It supports the following arguments:
  * `--framework`: `net6.0` (default), `net5.0`, `netcoreapp3.1`, `net48`, `net472` and `net462` are valid values.
  * `--use-program-main`: whether to turn off ASP.NET Core minimal API hosting. This option only affects `net6.0` projects as other target require a `Startup` and a `Program` class. 
  * `--no-https`: whether to turn off HTTPS and use `BasicHttpSecurityMode.None`. Default is HTTPS enabled using `BasicHttpSecurityMode.Transport`.
  * `--no-wsdl`: whether to turn off WSDL metadata feature.

### Development Builds

There are pre-release packages available for development builds of main from a NuGet feed hosted in Azure DevOps. You can download the packages by adding the following package source to your list of feeds.

    `https://pkgs.dev.azure.com/dotnet/CoreWCF/_packaging/CoreWCF/nuget/v3/index.json`

If you are using a nuget.config file with only the default nuget.org package source, after adding the CoreWCF feed it would look like this:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="CoreWCF" value="https://pkgs.dev.azure.com/dotnet/CoreWCF/_packaging/CoreWCF/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

### How do I contribute?

Please see the [CONTRIBUTING.md](CONTRIBUTING.md) file for details.

### License, etc.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

CoreWCF is Copyright &copy; 2019 .NET Foundation and other contributors under the [MIT license](LICENSE).

### .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
