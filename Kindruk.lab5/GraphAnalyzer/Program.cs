using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Plugin;

namespace GraphAnalyzer
{
    public class Program
    {
        private static FileInfo[] _coreDllFiles, _pluginFiles;
        private static readonly List<Assembly> CoreDlls = new List<Assembly>(), Plugins = new List<Assembly>();
        private static readonly List<Type> PluginTypes = new List<Type>();
        private static readonly List<MethodInfo> PluginMethods = new List<MethodInfo>();
        private static string _path;

        public static void Init()
        {
            _path = Directory.GetCurrentDirectory();
            var coreDllsFolder =
                new DirectoryInfo(_path + "\\" +
                                  ConfigurationManager.ConnectionStrings["CoreDllsFolder"].ConnectionString);
            var pluginsFolder =
                new DirectoryInfo(_path + "\\" + ConfigurationManager.ConnectionStrings["PluginsFolder"].ConnectionString);
            _coreDllFiles = coreDllsFolder.GetFiles("*.dll");
            _pluginFiles = pluginsFolder.GetFiles("*.dll");
        }

        public static bool LoadCoreDlls()
        {
            foreach (var file in _coreDllFiles)
            {
                CoreDlls.Add(Assembly.LoadFrom(file.FullName));
            }
            foreach (var coreDll in CoreDlls)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["CoreDllCheck"].ConnectionString,
                    coreDll.GetName().Name);
                var coreDllAttribute = (CoreDllAttribute)coreDll.GetCustomAttribute(typeof(CoreDllAttribute));
                var referencedAssemblies = coreDll.GetReferencedAssemblies();
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName()).ToArray();
                foreach (
                    var referencedAssembly in
                        referencedAssemblies.Where(
                            referencedAssembly =>
                                allAssemblies.All(assembly => assembly.FullName != referencedAssembly.FullName)))
                {
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["DllMissed"].ConnectionString,
                        coreDll.GetName().Name, referencedAssembly.FullName);
                    return false;
                }
                if (coreDllAttribute != null)
                {
                    Console.WriteLine(
                        ConfigurationManager.ConnectionStrings["CoreDllSuccessfullyLoaded"].ConnectionString,
                        coreDll.GetName().Name, coreDllAttribute.Name);
                }
                else
                {
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["NotACoreDll"].ConnectionString,
                        coreDll.GetName().Name);
                }
            }
            return true;
        }

        public static bool LoadPlugins()
        {
            foreach (var file in _pluginFiles)
            {
                Plugins.Add(Assembly.LoadFrom(file.FullName));
            }
            foreach (var plugin in Plugins)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["PluginCheck"].ConnectionString,
                    plugin.GetName().Name);
                var referencedAssemblies = plugin.GetReferencedAssemblies();
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName()).ToArray();
                foreach (
                    var referencedAssembly in
                        referencedAssemblies.Where(
                            referencedAssembly =>
                                allAssemblies.All(assembly => assembly.FullName != referencedAssembly.FullName)))
                {
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["DllMissed"].ConnectionString,
                        plugin.GetName().Name, referencedAssembly.FullName);
                    return false;
                }
                var isPlugin = false;
                foreach (
                    var type in
                        plugin.GetTypes()
                            .Where(
                                x =>
                                    x.GetCustomAttributes().Count(a => a.GetType() == typeof (PluginClassAttribute)) ==
                                    1))
                {
                    var foundSomething = false;
                    if (type.GetInterface("Plugin.IPlugin") == typeof (IPlugin))
                    {
                        Console.WriteLine(ConfigurationManager.ConnectionStrings["PluginClassFound"].ConnectionString,
                            type.Name);
                        PluginTypes.Add(type);
                        foundSomething = true;
                        isPlugin = true;
                    }
                    foreach (
                        var method in
                            type.GetMethods()
                                .Where(
                                    x =>
                                        x.GetCustomAttributes().Count(a => a.GetType() == typeof (PluginMethodAttribute)) ==
                                        1))
                    {
                        Console.WriteLine(ConfigurationManager.ConnectionStrings["PluginMethodFound"].ConnectionString,
                            method.Name, type.Name);
                        PluginMethods.Add(method);
                        foundSomething = true;
                        isPlugin = true;
                    }
                    if (!foundSomething)
                    {
                        Console.WriteLine(ConfigurationManager.ConnectionStrings["NotAPluginClass"].ConnectionString,
                            type.Name);
                    }
                }
                if (!isPlugin)
                {
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["NotAPlugin"].ConnectionString,
                        plugin.GetName().Name);
                }
            }
            return true;
        }

        public static void ShowPluginsInfoHandler()
        {
            foreach (var info in PluginTypes.Select(plugin => (PluginClassAttribute) plugin.GetCustomAttribute(typeof(PluginClassAttribute))))
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["PluginInfoFormat"].ConnectionString,
                    info.ClassName, info.AssemblyName, info.AuthorName, info.Description);
            }
        }

        public static void ShowCoreDllsInfoHandler()
        {
            foreach (var info in CoreDlls.Select(coreDll => (CoreDllAttribute)coreDll.GetCustomAttribute(typeof(CoreDllAttribute))))
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["CoreDllInfoFormat"].ConnectionString,
                    info.Name, info.AuthorName, info.Description);
            }
        }

        public static void ShowMethodsInfoHandler()
        {
            foreach (var method in PluginMethods)
            {
                var info = (PluginMethodAttribute) method.GetCustomAttribute(typeof (PluginMethodAttribute));
                Console.WriteLine(ConfigurationManager.ConnectionStrings["MethodInfoFormat"].ConnectionString,
                    method.Name, info.ClassName, info.AuthorName, info.Description);
            }
        }

        public static void RunPluginHandler()
        {
            Console.Clear();
            ShowPluginsInfoHandler();
            var count = 0;
            foreach (var plugin in PluginTypes)
            {
                var info = (PluginClassAttribute) plugin.GetCustomAttribute(typeof (PluginClassAttribute));
                count++;
                Console.WriteLine(ConfigurationManager.ConnectionStrings["PluginListFormat"].ConnectionString, count,
                    info.ClassName);
            }
            var s = Console.ReadLine();
            int key;
            while (!int.TryParse(s, out key) || key > count || key < 1)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["InvalidKey"].ConnectionString, s);
                Console.Write(ConfigurationManager.ConnectionStrings["InputAgain"].ConnectionString);
                s = Console.ReadLine();
            }
            Console.Clear();
            var pluginType = (IPlugin) Activator.CreateInstance(PluginTypes[key - 1]);
            pluginType.Init();
            pluginType.DoSomeAction();
            pluginType.Dispose();
            Console.WriteLine(ConfigurationManager.ConnectionStrings["PressAnyKey"].ConnectionString);
            Console.ReadKey();
        }

        public static void RunMethodHandler()
        {
            Console.Clear();
            ShowMethodsInfoHandler();
            var count = 0;
            foreach (var method in PluginMethods)
            {
                var info = (PluginMethodAttribute)method.GetCustomAttribute(typeof(PluginMethodAttribute));
                count++;
                Console.WriteLine(ConfigurationManager.ConnectionStrings["MethodListFormat"].ConnectionString, count,
                    method, info.ClassName);
            }
            var s = Console.ReadLine();
            int key;
            while (!int.TryParse(s, out key) || key > count || key < 1)
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["InvalidKey"].ConnectionString, s);
                Console.Write(ConfigurationManager.ConnectionStrings["InputAgain"].ConnectionString);
                s = Console.ReadLine();
            }
            Console.Clear();
            var pluginType = Activator.CreateInstance(PluginMethods[key - 1].DeclaringType);
            PluginMethods[key - 1].Invoke(pluginType, null);
            Console.WriteLine(ConfigurationManager.ConnectionStrings["PressAnyKey"].ConnectionString);
            Console.ReadKey();
        }

        public static void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("1. " + ConfigurationManager.ConnectionStrings["MainMenuRunPlugin"].ConnectionString);
            Console.WriteLine("2. " + ConfigurationManager.ConnectionStrings["MainMenuRunMethod"].ConnectionString);
            Console.WriteLine("3. " + ConfigurationManager.ConnectionStrings["MainMenuShowCoreDllsInfo"].ConnectionString);
            Console.WriteLine("4. " + ConfigurationManager.ConnectionStrings["MainMenuShowPluginsInfo"].ConnectionString);
            Console.WriteLine("5. " + ConfigurationManager.ConnectionStrings["MainMenuShowMethodsInfo"].ConnectionString);
            Console.WriteLine("6. " + ConfigurationManager.ConnectionStrings["MainMenuExit"].ConnectionString);
            Console.Write(ConfigurationManager.ConnectionStrings["MainMenuInputInvitation"].ConnectionString);
        }

        public static bool MainMenuHandler()
        {
            PrintMainMenu();
            var s = Console.ReadLine();
            int key;
            while (!int.TryParse(s, out key))
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["InvalidKey"].ConnectionString, s);
                Console.Write(ConfigurationManager.ConnectionStrings["InputAgain"].ConnectionString);
                s = Console.ReadLine();
            }
            switch (key)
            {
                case 1:
                    RunPluginHandler();
                    break;
                case 2:
                    RunMethodHandler();
                    break;
                case 3:
                    Console.Clear();
                    ShowCoreDllsInfoHandler();
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["PressAnyKey"].ConnectionString);
                    Console.ReadKey();
                    break;
                case 4:
                    Console.Clear();
                    ShowPluginsInfoHandler();
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["PressAnyKey"].ConnectionString);
                    Console.ReadKey();
                    break;
                case 5:
                    Console.Clear();
                    ShowMethodsInfoHandler();
                    Console.WriteLine(ConfigurationManager.ConnectionStrings["PressAnyKey"].ConnectionString);
                    Console.ReadKey();
                    break;
                case 6:
                    return false;
            }
            return true;
        }

        public static void Run()
        {
            while (MainMenuHandler()){}
        }

        public static void Main()
        {
            Init();
            if (!LoadCoreDlls())
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["FatalError"].ConnectionString);
                Console.ReadKey();
                return;
            }
            if (!LoadPlugins())
            {
                Console.WriteLine(ConfigurationManager.ConnectionStrings["FatalError"].ConnectionString);
                Console.ReadKey();
                return;
            }
            Console.WriteLine(ConfigurationManager.ConnectionStrings["PressAnyKey"].ConnectionString);
            Console.ReadKey();
            Run();
        }
    }
}
