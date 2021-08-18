namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Provides information about COM components, implemented with the .NET framework
    /// </summary>
    public class DotNetFrameworkComponentRegistrationTypeInformation
        : ManagedComponentRegistrationInformation {

        /// <summary>
        /// The version of the assembly.
        /// </summary>
        public string AssemblyVersion {
            get; set;
        }

        /// <summary>
        /// the name of the assembly.
        /// </summary>
        public string AssemblyName {
            get; set;
        }

        /// <summary>
        /// Indicates, if the assembly has a strong name.
        /// </summary>
        public bool AssemblyHasStrongName {
            get; set;
        }

        /// <summary>
        /// The .NET Framework runtime version, required by the assembly.
        /// </summary>
        public string RuntimeVersion {
            get; set;
        }

        /// <summary>
        /// The codebase of the assembly.
        /// </summary>
        public string Codebase {
            get; set;
        }
    }
}
