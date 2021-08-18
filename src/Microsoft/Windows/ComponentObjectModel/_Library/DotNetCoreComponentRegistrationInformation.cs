namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Defines and provides component information, specific to .NET Core based components.
    /// </summary>
    public class DotNetCoreComponentRegistrationInformation
    : ManagedComponentRegistrationInformation {

        /// <summary>
        /// The full qualified path to the .NET Core COM hosting library.
        /// </summary>
        public string ComHost {
            get; set;
        }

        /// <summary>
        /// The full qualified path to the .NET Core Assembly  implementing the COM component.
        /// </summary>
        public string Codebase {
            get; set;
        }
    }
}
