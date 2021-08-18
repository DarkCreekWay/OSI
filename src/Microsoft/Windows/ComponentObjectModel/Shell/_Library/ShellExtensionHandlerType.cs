using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell {

    /// <summary>
    /// Describes the different types of Shell Extension Handlers.
    /// </summary>
    /// <remarks>
    /// <a href="https://docs.microsoft.com/en-us/windows/win32/shell/reg-shell-exts">Registering Shell Extension Handlers - Microsoft Docs</a>
    /// </remarks>
    [SuppressMessage( "Usage", "CA2211: Non - constant fields should not be visible", Justification = "Properties provided for coding convinience")]
    [SuppressMessage( "Style", "IDE0079: Remove unnecessary suppression", Justification = "IntelliSense false positive" )]
    public class ShellExtensionHandlerType {

        /// <summary>
        /// Column Provider handler.
        /// </summary>
        public static ShellExtensionHandlerType ColumnProvider    = new ShellExtensionHandlerType( "ColumnHandlers" , true);

        /// <summary>
        /// Context Menu handler.
        /// </summary>
        /// <remarks>
        /// Alias for ShortcutMenu. Microsoft documentation uses the term ShortcutMenu
        /// </remarks>
        public static ShellExtensionHandlerType ContextMenu       = new ShellExtensionHandlerType( "ContextMenuHandlers", true);

        /// <summary>
        /// Shortcut Menu handler.
        /// </summary>
        public static ShellExtensionHandlerType ShortcutMenu      = new ShellExtensionHandlerType( "ContextMenuHandlers", true);

        /// <summary>
        /// Copyhook handler.
        /// </summary>
        public static ShellExtensionHandlerType Copyhook          = new ShellExtensionHandlerType( "CopyHookHandlers", true);

        /// <summary>
        /// Drag and Drop handler.
        /// </summary>
        public static ShellExtensionHandlerType DragAndDrop       = new ShellExtensionHandlerType( "DragDropHandlers", true);

        /// <summary>
        /// Property sheet handler.
        /// </summary>
        public static ShellExtensionHandlerType PropertySheet     = new ShellExtensionHandlerType( "PropertySheetHandlers", true);

        /// <summary>
        /// Data handler.
        /// </summary>
        public static ShellExtensionHandlerType Data              = new ShellExtensionHandlerType( "DataHandler", false);

        /// <summary>
        /// Drop handler.
        /// </summary>
        public static ShellExtensionHandlerType Drop              = new ShellExtensionHandlerType( "DropHandler", false);

        /// <summary>
        /// Icon handler.
        /// </summary>
        public static ShellExtensionHandlerType Icon              = new ShellExtensionHandlerType( "IconHandler", false);

        /// <summary>
        /// Thumbnail image handler.
        /// </summary>
        public static ShellExtensionHandlerType ThumbnailImage    = new ShellExtensionHandlerType( "{E357FCCD-A995-4576-B01F-234630154E96}", false);

        /// <summary>
        /// Infotip handler.
        /// </summary>
        public static ShellExtensionHandlerType InfoTip           = new ShellExtensionHandlerType( "{00021500-0000-0000-C000-000000000046}", false);

        /// <summary>
        /// Shell link
        /// </summary>
        /// <remarks>UNICODE version.</remarks>
        public static ShellExtensionHandlerType ShellLink         = new ShellExtensionHandlerType( "{000214F9-0000-0000-C000-000000000046}", false);

        /// <summary>
        /// Structured storage.
        /// </summary>
        public static ShellExtensionHandlerType StructuredStorage = new ShellExtensionHandlerType( "{0000000B-0000-0000-C000-000000000046}", false);

        /// <summary>
        /// Metadata.
        /// </summary>
        public static ShellExtensionHandlerType Metadata          = new ShellExtensionHandlerType( "PropertyHandler", false);

        /// <summary>
        /// Pin to Start Menu.
        /// </summary>
        public static ShellExtensionHandlerType PinToStartMenu    = new ShellExtensionHandlerType( "{a2a9545d-a0c2-42b4-9708-a0b2badd77c8}", false);

        /// <summary>
        /// Pin to Taskbar.
        /// </summary>
        public static ShellExtensionHandlerType PinToTaskBar      = new ShellExtensionHandlerType( "{90AA3A4E-1CBA-4233-B8BB-535773D48449}", false);

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="registrySubkey">The Registry subkey name.</param>
        /// <param name="supportsMultipleHandlers">Indicates, that the OS supports multiple handlers of the same type.</param>
        public ShellExtensionHandlerType( string registrySubkey, bool supportsMultipleHandlers ) {
            RegistrySubkey = registrySubkey;
            SupportsMultipleHandlers = supportsMultipleHandlers;
        }

        /// <summary>
        /// Gets the registry subkey name.
        /// </summary>
        public string RegistrySubkey {
            get;
        }

        /// <summary>
        /// Get the information, if the handler type supports multiple handlers.
        /// </summary>
        public bool SupportsMultipleHandlers {
            get;
        }
    }
}
