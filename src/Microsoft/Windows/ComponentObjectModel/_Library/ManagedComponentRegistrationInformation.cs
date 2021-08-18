using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Provides information about COM components, implemented in managed code.
    /// </summary>
    public abstract class ManagedComponentRegistrationInformation
    : ComponentRegistrationInformation {

        /// <summary>
        /// The full qualified name of the component implementing class
        /// </summary>
        public string TypeName {
            get; set;
        }

        /// <summary>
        /// Collection of <see cref="Attribute"/> instances associated with a managed type implementing the component/>
        /// </summary>
        public ComponentAttributeCollection Attributes {
            get; set;
        }
    }
}
