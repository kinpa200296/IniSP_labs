using System;

namespace Plugin
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class CoreDllAttribute : Attribute
    {
        public string Name { get; private set; }
        public string AuthorName { get; private set; }
        public string Description { get; private set; }
        public string CreationDate { get; set; }

        public CoreDllAttribute(string name = "<unknown>", string authorName = "<unknown>",
            string description = "No description.")
        {
            Name = name;
            AuthorName = authorName;
            Description = description;
        }
    }
}
