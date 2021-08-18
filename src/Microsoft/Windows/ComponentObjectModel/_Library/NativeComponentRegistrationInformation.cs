namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// Provides information about COM components, implemented in native code.
    /// </summary>
    public class NativeComponentRegistrationInformation
    : ComponentRegistrationInformation {

        /// <summary>
        /// The local path to the file, implementing the component.
        /// </summary>
        /// <remarks>
        /// The related registry key depends on the <seealso cref="ComponentServerType"/>.
        /// <see cref="ComponentServerType.LocalServer"/> - [LocalServer (Reg_SZ)]
        /// <see cref="ComponentServerType.LocalServer32"/> - [(DEFAULT) (REG_SZ)] + [ServerExecutable (REG_SZ)]
        /// <see cref="ComponentServerType.InprocServer"/> - [(DEFAULT) (REG_SZ)]
        /// <see cref="ComponentServerType.InprocServer32"/> - [(DEFAULT) (REG_SZ)]
        /// </remarks>
        public string Path {
            get; set;
        }
    }
}
