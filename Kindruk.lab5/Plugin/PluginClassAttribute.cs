using System;

namespace Plugin
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginClassAttribute : Attribute
    {
        public string AssemblyName { get; private set; }
        public string ClassName { get; private set; }
        public string AuthorName { get; private set; }
        public string Description { get; private set; }
        public string CreationDate { get; set; }

        public PluginClassAttribute(string assemblyName = "<unknown>", string className = "<unknown>",
            string authorName = "<unknown>", string description = "No description.")
        {
            AssemblyName = assemblyName;
            ClassName = className;
            AuthorName = authorName;
            Description = description;
        }
    }
}
