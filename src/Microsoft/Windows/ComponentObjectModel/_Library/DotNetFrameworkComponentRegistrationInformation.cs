using System.Collections.Generic;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Defines and provides component information, specific to .NET Framework based components.
    /// </summary>
    /// <remarks>
    /// .NET Core uses a different approach. For these types, use the NativeComponentInformation type.
    /// </remarks>
    public class DotNetFrameworkComponentRegistrationInformation
    : ManagedComponentRegistrationInformation {

        /// <summary>
        /// The path to the .NET Framework Shim - Usually %systemroot%\mscoree.dll
        /// </summary>
        /// <remarks>The value is written to the (DEFAULT) [REG_SZ] Reg Value. You cannot use envronment variables here.</remarks>
        public string DotNetShimPath {
            get; set;
        }

        /// <summary>
        /// .NET Framework specific type information, which should be shared between versioned registrations
        /// </summary>
        public DotNetFrameworkComponentRegistrationTypeInformation SharedTypeInformation {
            get; set;
        }

        /// <summary>
        /// Provides version dependent registration information.
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/zy736xtk(v=vs.100)">Version-Dependent Registry Keys - Microsoft Docs</a>
        /// <a href="https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/65y5x3xf(v=vs.100)">Configuring a COM Application for Side-by-Side Execution - Microsoft Docs</a>
        /// </remarks>
        public List<DotNetFrameworkComponentRegistrationTypeInformation> VersionedTypeInformation {
            get;
        } = new List<DotNetFrameworkComponentRegistrationTypeInformation>();

    }
}
