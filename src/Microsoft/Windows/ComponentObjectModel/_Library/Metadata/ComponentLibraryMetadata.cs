using System;
using System.Collections.Generic;
using System.Reflection;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Metadata {

    /// <summary>
    /// Provides and manages information about a COM component library
    /// </summary>
    /// <remarks>
    /// The name is focussed on "Libraries" instead of "Assemblies" to emphasize, that a caller can throw any type of Library (dll/exe) onto the reader.
    /// While the code cannor read com info from a native dll (currently), it does not throw / break on it.
    /// A library for COM can contain multiple components
    /// A library for COM can have supporting files (deps.json, tlb, reg)
    /// </remarks>
    public class ComponentLibraryMetadata {

        /// <summary>
        /// The full qualified path to the component library image (file)
        /// </summary>
        public string Path {
            get; set;
        }

        /// <summary>
        /// The <see cref="ComponentLibraryImageType"/> of the compoent library.
        /// </summary>
        public ComponentLibraryImageType ImageType {
            get; set;
        } = ComponentLibraryImageType.Unknown;

        /// <summary>
        /// The .NET Assembly Name of the component library.
        /// </summary>
        /// <remarks>
        /// This property is only set, if <see cref="ImageType"/> is set to <see cref="ComponentLibraryImageType.Managed"/>
        /// </remarks>
        public AssemblyName AssemblyName {
            get; set;
        }

        /// <summary>
        /// Returns a value, indicating, if the Assembly has a strong name.
        /// </summary>
        /// <remarks>
        /// This property is only set, if <see cref="ImageType"/> is set to <see cref="ComponentLibraryImageType.Managed"/>
        /// </remarks>
        public bool AssemblyHasStrongName {
            get => AssemblyName != null && AssemblyName.GetPublicKeyToken().Length > 0;
        }

        /// <summary>
        /// Returns the runtime version, as defined by the assembly.
        /// </summary>
        /// <remarks>
        /// This property is only set, if <see cref="ImageType"/> is set to <see cref="ComponentLibraryImageType.Managed"/>
        /// </remarks>
        public string AssemblyRuntimeVersion {
            get; set;
        }

        /// <summary>
        /// Returns the <see cref="ComponentLibraryTargetFramework"/> of the assembly.
        /// </summary>
        /// <remarks>
        /// This property is only set, if <see cref="ImageType"/> is set to <see cref="ComponentLibraryImageType.Managed"/>
        /// </remarks>
        public ComponentLibraryTargetFramework AssemblyTargetFramework {
            get; set;
        } = ComponentLibraryTargetFramework.Unknown;

        /// <summary>
        /// Returns the Target Framework Version of the assembly.
        /// </summary>
        public Version AssemblyTargetFrameworkVersion {
            get; set;
        }

        /// <summary>
        /// Returns attributes associated to the assembly.
        /// </summary>
        public ComponentAttributeCollection Attributes {
            get; set;
        }

        /// <summary>
        /// COM compoents, as defined in the component library.
        /// </summary>
        public List<ComponentRegistrationInformation> Components {
            get;
        } = new List<ComponentRegistrationInformation>();

    }
}
