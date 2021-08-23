using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        private static IList<Assembly> staticAssemblies;
        private IList<Assembly> assemblies;

        /// <summary>
        /// All assemblies needed to resolves Types
        /// by default all Assemblies loaded in the current AppDomain
        /// </summary>
        public virtual IList<Assembly> Assemblies
        {
            get { return assemblies ?? (assemblies = staticAssemblies) ?? (assemblies = staticAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList()); }
            set { assemblies = value; }
        }

        /// <summary>
        /// All Namespaces Where to find types
        /// </summary>
        public virtual IList<string> Namespaces { get; set; } = new List<string>()
        {
            "System",
            "System.Linq",
            "System.IO",
            "System.Text",
            "System.Text.RegularExpressions",
            "System.ComponentModel",
            "System.Dynamic",
            "System.Collections",
            "System.Collections.Generic",
            "System.Collections.Specialized",
            "System.Globalization"
        };

        /// <summary>
        /// To add or remove specific types to manage in expression.
        /// </summary>
        public virtual IList<Type> Types { get; set; } = new List<Type>();

        /// <summary>
        /// A list of type to block an keep un usable in Expression Evaluation for security purpose
        /// </summary>
        public virtual IList<Type> TypesToBlock { get; set; } = new List<Type>();

        /// <summary>
        /// A list of statics types where to find extensions methods
        /// </summary>
        public virtual IList<Type> StaticTypesForExtensionsMethods { get; set; } = new List<Type>()
        {
            typeof(Enumerable) // For Linq extension methods
        };
    }
}
