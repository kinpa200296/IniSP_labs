using System;

namespace Plugin
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PluginMethodAttribute : Attribute
    {
        public string ClassName { get; private set; }
        public string AuthorName { get; private set; }
        public string Description { get; private set; }
        public string CreationDate { get; set; }

        public PluginMethodAttribute(string className = "<unknown>", string authorName = "<unknown>",
            string description = "No description.")
        {
            ClassName = className;
            AuthorName = authorName;
            Description = description;
        }
    }
}
