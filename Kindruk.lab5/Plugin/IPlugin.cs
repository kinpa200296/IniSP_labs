using System;
using System.Reflection;

namespace Plugin
{
    public interface IPlugin : IDisposable
    {
        void Init();
        void DoSomeAction();
    }
}
