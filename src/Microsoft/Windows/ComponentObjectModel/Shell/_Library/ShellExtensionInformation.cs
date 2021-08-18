using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell {

    /// <summary>
    /// Provides information about a shell extension registration.
    /// </summary>
    public class ShellExtensionInformation {

        string _name;

        /// <summary>
        /// The CLSID of the COM component.
        /// </summary>
        public Guid CLSID {
            get; set;
        }

        /// <summary>
        /// The name of the shell extenion.
        /// </summary>
        public string Name {
            get => _name;
            set {

                if ( string.IsNullOrEmpty( value ) ) {
                    throw new ArgumentNullException( nameof( value ) );
                }

                if ( value.Length > 63 ) {
                    throw new ArgumentOutOfRangeException( "Value must not exceed length of 63 bytes", nameof( Name ) );
                }

                _name = value;
            }
        }

        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName {
            get; set;
        }

        /// <summary>
        /// The <seealso cref="ShellExtensionHandlerType"/>.
        /// </summary>
        public ShellExtensionHandlerType ShellHandlerType {
            get; set;
        }

        /// <summary>
        /// Gets a collection of associations.
        /// </summary>
        public ShellExtensionAssociationCollection Associations {
            get;
        } = new ShellExtensionAssociationCollection();

    }
}
