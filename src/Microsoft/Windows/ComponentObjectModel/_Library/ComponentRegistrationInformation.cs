using System;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Defines and common component information.
    /// </summary>
    public abstract class ComponentRegistrationInformation {

        /// <summary>
        /// The CLSID of the Component
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/com/clsid-key-hklm">CLSID Key (Microsoft Docs)</a>
        /// </remarks>
        public Guid CLSID {
            get; set;
        }

        /// <summary>
        /// The ProgId of the component. (Optional)
        /// </summary>
        /// <remarks>
        /// <a href="https://docs.microsoft.com/en-us/windows/win32/com/-progid--key">ProgID Key (Microsoft Docs</a>
        /// </remarks>
        public string ProgId {
            get; set;
        }

        /// <summary>
        /// The ServerType of the component
        /// </summary>
        /// <remarks>
        /// </remarks>
        public ComponentServerType ServerType { get; set; } = ComponentServerType.Undefined;

        /// <summary>
        /// The threading model of the component.Use in conjunction with components of <seealso cref="ComponentServerType.InprocServer32"/>
        /// </summary>
        public ComponentThreadingModel ThreadingModel { get; set; } = ComponentThreadingModel.Undefined;
    }
}
