using System;
using System.Reflection;

[assembly : AssemblyVersion("1.0.0.0"), AssemblyTitle("Plugin")]
[assembly : AssemblyDescription("Provides interface and attributes for writing custom plugins and core dlls for GraphAnalyzer application.")]
namespace Plugin
{
    public interface IPlugin : IDisposable
    {
        void Init();
        void DoSomeAction();
    }
}
