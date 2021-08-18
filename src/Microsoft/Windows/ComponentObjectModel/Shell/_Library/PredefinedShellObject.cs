using System.Diagnostics.CodeAnalysis;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell {

    /// <summary>
    /// Predefined Shell Objects describe properties of Shell Handlers.
    /// </summary>
    /// <remarks>
    /// <a href="https://docs.microsoft.com/en-us/windows/win32/shell/handlers#predefined-shell-objects">Predefined Shell Objects - Microsoft Docs</a>
    /// <br/>
    /// Network sub types are excluded in favor of keeping the PredefinedShellObject type simple
    /// Network\Type\# : // All objects of type #. Possible Handlers: Shortcut menu, Property Sheet, Verbs
    /// network_provider_name : // Provider_name. All objects provided by network provider "network_provider_name"Possible Handlers: Shortcut menu, Property Sheet, Verbs
    /// </remarks>
    [SuppressMessage( "Usage", "CA2211: Non - constant fields should not be visible", Justification = "Properties provided for coding convinience")]
    [SuppressMessage( "Style", "IDE0079: Remove unnecessary suppression", Justification = "IntelliSense false positive" )]
    public class PredefinedShellObject {

        /// <summary>
        /// All files.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject Files = new PredefinedShellObject( "*" );

        /// <summary>
        /// All files and file folders.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject FileSystemObjects = new PredefinedShellObject( "AllFileSystemObjects" );

        /// <summary>
        /// All folders.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject Folders = new PredefinedShellObject( "Folder" );

        /// <summary>
        /// File folders.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject Directories = new PredefinedShellObject( "Directory" );

        /// <summary>
        /// Directory Background.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: File folder background	Shortcut Menu only
        /// </remarks>
        public static PredefinedShellObject DirectoryBackground = new PredefinedShellObject( "Directory\\Background" );

        /// <summary>
        /// All drives in MyComputer, such as "C:\".
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject Drive = new PredefinedShellObject( "Drive" );

        /// <summary>
        /// Entire network (under My Network Places).
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject Network = new PredefinedShellObject( "Network" );

        /// <summary>
        /// All network shares.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject NetShare = new PredefinedShellObject( "NetShare" );

        /// <summary>
        /// All network servers.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut menu, Property Sheet, Verbs
        /// </remarks>
        public static PredefinedShellObject NetServer = new PredefinedShellObject( "NetServer" );

        /// <summary>
        /// All printers.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Shortcut Menu, Property Sheet
        /// </remarks>
        public static PredefinedShellObject Printers = new PredefinedShellObject( "Printers" );

        /// <summary>
        /// Audio CD in CD drive.
        /// </summary>
        /// <remarks>
        /// Possible Handlers: Verbs
        /// </remarks>
        public static PredefinedShellObject AudioCD = new PredefinedShellObject( "AudioCD" );

        /// <summary>
        /// DVD drive (Windows 2000). Possible Handlers: Shortcut Menu, Property Sheet, Verbs
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static PredefinedShellObject DVD = new PredefinedShellObject( "DVD" );

        /// <summary>
        /// Provides default/fallback, if Shell cannot identify the filetype.
        /// </summary>
        /// <remarks>
        /// Currently, no information are available, if and which shell handlers are supported.
        /// </remarks>
        public static PredefinedShellObject Unknown = new PredefinedShellObject( "Unknown" );

        /// <summary>
        /// Constructs a PredefinedShellObject instance for a given <paramref name="progid"/>
        /// </summary>
        /// <param name="progid">The progid.</param>
        public PredefinedShellObject( string progid ) {
            ProgId = progid;
        }

        /// <summary>
        /// Gets the ProgId.
        /// </summary>
        public string ProgId {
            get;
        }
    }
}
