using System.Runtime.InteropServices;
using System.Reflection;

#if DEBUG
[assembly: AssemblyProduct("MyInfluxDbClient (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("MyInfluxDbClient (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription("MyInfluxDbClient - a simple async client for InfluxDb.")]
[assembly: AssemblyCompany("Daniel Wertheim")]
[assembly: AssemblyCopyright("Copyright © 2017 Daniel Wertheim")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]