using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell.Native;
using DarkCreekWay.OSI.Microsoft.Windows.Registry;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Shell {

    /// <summary>
    /// Provides functions for managing shell extensions
    /// </summary>
#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public class ShellExtensionManager {

        IRegistry _registry;

        /// <summary>
        /// Creats a new instance.
        /// </summary>
        public ShellExtensionManager() : this( new WindowsRegistry() ) {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="registry">A <seealso cref="IRegistry"/> implementation</param>
        /// <remarks>
        /// Primarily used for injecting a Mock registry.
        /// </remarks>
        public ShellExtensionManager( IRegistry registry ) {

            Debug.Assert( registry != null );
            _registry = registry;
        }

        /// <summary>
        /// Registers a shell extension and notifies the Shell about the registration event.
        /// </summary>
        /// <param name="shellExtensionInformation">Information about the shell extension.</param>
        /// <param name="scope">The registration scope of the shell extension.</param>
        public void Register( ShellExtensionInformation shellExtensionInformation, ComponentRegistrationScope scope ) {

            if( null == shellExtensionInformation ) {
                throw new ArgumentNullException( nameof( shellExtensionInformation ) );
            }

            foreach( string progId in shellExtensionInformation.Associations ) {

                Associate( progId, shellExtensionInformation.ShellHandlerType, shellExtensionInformation.Name, shellExtensionInformation.CLSID.ToString( "B" ), scope );

            }

            NotifyShell();
        }

        /// <summary>
        /// Unregisters a shell extension and notifies the Shell about the unregistration event.
        /// </summary>
        /// <param name="shellExtensionInformation">Information about the shell extension.</param>
        /// <param name="scope">The registration scope of the shell extension.</param>
        public void Unregister( ShellExtensionInformation shellExtensionInformation, ComponentRegistrationScope scope ) {

            if( null == shellExtensionInformation ) {
                throw new ArgumentNullException( nameof( shellExtensionInformation ) );
            }

            foreach( string progId in shellExtensionInformation.Associations ) {

                Disassociate( progId, shellExtensionInformation.ShellHandlerType, shellExtensionInformation.Name, shellExtensionInformation.CLSID.ToString( "B" ), scope );

            }

            NotifyShell();
        }

        /// <summary>
        /// Retrieves the registration state for a given shell extension.
        /// </summary>
        /// <param name="shellExtensionInformation">Information about the shell extension.</param>
        /// <param name="scope">The registration scope of the shell extension.</param>
        /// <returns>The registration state.</returns>
        public ShellExtensionRegistrationState GetRegistrationState( ShellExtensionInformation shellExtensionInformation, ComponentRegistrationScope scope ) {

            int count = 0;

            using( IRegistryKey classesKey = GetScopedClassesKey( scope, false ) ) {

                foreach( string progid in shellExtensionInformation.Associations ) {

                    bool associated = IsAssociated( classesKey, progid, shellExtensionInformation.ShellHandlerType.RegistrySubkey, shellExtensionInformation.CLSID.ToString( "B" ) );
                    if( associated ) {
                        ++count;
                    }
                }

                if( count == 0 ) {
                    return ShellExtensionRegistrationState.NotRegistered;
                }

                if( count < shellExtensionInformation.Associations.Count ) {
                    return ShellExtensionRegistrationState.PartiallyRegistered;
                }

                if( count == shellExtensionInformation.Associations.Count ) {
                    return ShellExtensionRegistrationState.Registered;
                }

                return ShellExtensionRegistrationState.Unknown;
            }
        }

        /// <summary>
        /// Test for an association between a progId to a specific shell extension.
        /// </summary>
        /// <param name="progId">The progId to check for the association.</param>
        /// <param name="handlerType">The handler type.</param>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="scope">The registration scope.</param>
        /// <returns>true, if the association is available. Otherwise, false.</returns>
        [SuppressMessage( "Performance", "CA1822: Mark members as static", Justification = "Performance is good enough. So favoring access as instance member over performance." )]
        [SuppressMessage( "Style", "IDE0079: Remove unnecessary suppression", Justification = "IntelliSense false positive" )]
        public bool IsAssociated( string progId, string handlerType, string clsid, ComponentRegistrationScope scope ) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test for an association between a collection of progids and a shell extension.
        /// </summary>
        /// <param name="progids">The collection of progIds.</param>
        /// <param name="handlerType">The shell extension type.</param>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="scope">The registration scope.</param>
        /// <returns>true, if all progids are associated with the shell extension. Otherwise, false.</returns>
        public bool IsAssociated( ShellExtensionAssociationCollection progids, string handlerType, string clsid, ComponentRegistrationScope scope ) {

            using( IRegistryKey classesKey = GetScopedClassesKey( scope, false ) ) {

                foreach( string progid in progids ) {
                    if( false == IsAssociated( classesKey, progid, handlerType, clsid ) ) {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Tests for an association between a progid and a shell extension.
        /// </summary>
        /// <param name="classesKey">The classes registry key.</param>
        /// <param name="progId">The progId</param>
        /// <param name="shellHandlerTypeName">The name of the shell handler type.</param>
        /// <param name="clsid">The clsid of the shell handler.</param>
        /// <returns>true, if the progid is associated woth the shell extension. Otherwise, false.</returns>
        static bool IsAssociated( IRegistryKey classesKey, string progId, string shellHandlerTypeName, string clsid ) {

            // Enumerate all registered shell handlers of a specific types and check for a given clsid

            using( IRegistryKey progidKey = classesKey.OpenSubKey( progId ) ) {

                if( null == progidKey ) {
                    return false;
                }

                using( IRegistryKey shellHandlerTypeRegKey = progidKey.OpenSubKey( $@"shellex\{shellHandlerTypeName}" ) ) {

                    if( shellHandlerTypeRegKey == null ) {
                        return false;
                    }

                    foreach( string shellHandlerName in shellHandlerTypeRegKey.GetSubKeyNames() ) {

                        using( IRegistryKey shellHandlerKey = shellHandlerTypeRegKey.OpenSubKey( shellHandlerName ) ) {

                            string defaultHandlerValue = shellHandlerKey.GetString( "" );

                            if( defaultHandlerValue == clsid ) {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Associates a progId to a shell extension.
        /// </summary>
        /// <param name="progId">The progId.</param>
        /// <param name="handlerType">The type of the shell extension.</param>
        /// <param name="name">The name of the shell extension.</param>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="scope">The registration scope.</param>
        public void Associate( string progId, ShellExtensionHandlerType handlerType, string name, string clsid, ComponentRegistrationScope scope ) {

            if( string.IsNullOrEmpty( progId ) ) {
                throw new ArgumentNullException( nameof( progId ) );
            }

            if( scope != ComponentRegistrationScope.User && scope != ComponentRegistrationScope.System ) {
                throw new ArgumentOutOfRangeException( nameof( scope ) );
            }

            if( null == handlerType ) {
                throw new ArgumentNullException( nameof( handlerType ) );
            }

            if( string.IsNullOrEmpty( name ) ) {
                throw new ArgumentNullException( nameof( name ) );
            }

            // Length tested with Windows 10 x64
            if( name.Length > 63 ) {
                // TODO: Investigate, if registry sees the length in bytes or as count of chars. Modify length check based on research.
                throw new ArgumentException( "Parameter must not exceed a length of 63", nameof( name ) );
            }

            if( string.IsNullOrEmpty( clsid ) ) {
                throw new ArgumentNullException( nameof( clsid ) );
            }

            using( IRegistryKey classesKey = GetScopedClassesKey( scope, true ) ) {

                using( IRegistryKey progidKey = classesKey.CreateSubKey( progId, true ) ) {

                    using( IRegistryKey shellExKey = progidKey.CreateSubKey( "shellEx", true ) ) {
                        using( IRegistryKey handlerTypeKey = shellExKey.CreateSubKey( handlerType.RegistrySubkey, true ) ) {

                            if( handlerType.SupportsMultipleHandlers == false ) {
                                throw new NotImplementedException();
                            }

                            using( IRegistryKey handlerKey = handlerTypeKey.CreateSubKey( name, true ) ) {
                                handlerKey.SetValue( "", clsid, RegistryValueKind.String );
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Associates a predefined shell object with a shell extension.
        /// </summary>
        /// <param name="predefinedShellObject">The predefined shell object.</param>
        /// <param name="type">The type of the shell extension.</param>
        /// <param name="name">The name of the shell extension.</param>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="scope">The registration scope.</param>
        public void Associate( PredefinedShellObject predefinedShellObject, ShellExtensionHandlerType type, string name, string clsid, ComponentRegistrationScope scope ) {

            Associate( predefinedShellObject.ProgId, type, name, clsid, scope );
        }

        /// <summary>
        /// Disassociates a shell extension from a progId.
        /// </summary>
        /// <param name="progid">The progid.</param>
        /// <param name="type">The type of the shell extension.</param>
        /// <param name="name">The name of the shell extension.</param>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="scope">The registration scope.</param>
        public void Disassociate( string progid, ShellExtensionHandlerType type, string name, string clsid, ComponentRegistrationScope scope ) {

            if( string.IsNullOrEmpty( progid ) ) {
                throw new ArgumentNullException( nameof( progid ) );
            }

            if( scope != ComponentRegistrationScope.User && scope != ComponentRegistrationScope.System ) {
                throw new ArgumentOutOfRangeException( nameof( scope ) );
            }

            if( null == type ) {
                throw new ArgumentNullException( nameof( type ) );
            }

            if( string.IsNullOrEmpty( name ) ) {
                throw new ArgumentNullException( nameof( name ) );
            }

            if( string.IsNullOrEmpty( clsid ) ) {
                throw new ArgumentNullException( nameof( clsid ) );
            }

            using( IRegistryKey classesKey = GetScopedClassesKey( scope, true ) ) {

                using( IRegistryKey progidKey = classesKey.OpenSubKey( progid, true ) ) {

                    if( null == progidKey ) {
                        // No progid key in registry. No more work to do.
                        return;
                    }

                    using( IRegistryKey shellexKey = progidKey.OpenSubKey( "shellex", true ) ) {

                        if( null == shellexKey ) {
                            return;
                        }

                        if( type.SupportsMultipleHandlers == false ) {
                            throw new NotImplementedException();
                        }

                        using( IRegistryKey handlerTypeKey = shellexKey.OpenSubKey( type.RegistrySubkey, true ) ) {

                            int subKeyCount = handlerTypeKey.SubKeyCount;

                            // 1. Try - Get handler subkey by name
                            using( IRegistryKey handlerKey = handlerTypeKey.OpenSubKey( name, true ) ) {

                                if( null != handlerKey ) {

                                    string registeredClsid = handlerKey.GetString( "" );

                                    if( null == registeredClsid ) {
                                        throw new NotImplementedException();
                                    }

                                    if( !registeredClsid.Equals( clsid, StringComparison.OrdinalIgnoreCase ) ) {
                                        // Registered clsid is different from expected clsid
                                        throw new NotImplementedException();
                                    }

                                    handlerKey.Close();

                                    handlerTypeKey.DeleteSubKeyTree( name, false );
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Disassociates a shell extension from a predefined shell object.
        /// </summary>
        /// <param name="predefinedShellObject">The predefined shell object.</param>
        /// <param name="type">The type of the shell extension.</param>
        /// <param name="name">The name of the shell extension.</param>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="scope">The registration scope.</param>
        public void Disassociate( PredefinedShellObject predefinedShellObject, ShellExtensionHandlerType type, string name, string clsid, ComponentRegistrationScope scope ) {

            Disassociate( predefinedShellObject.ProgId, type, name, clsid, scope );
        }

        /// <summary>
        /// Notifies the Shell about changes.
        /// </summary>
        /// <remarks>
        /// Applications that register new handlers of any type must call SHChangeNotify with the SHCNE_ASSOCCHANGED flag to instruct the Shell to invalidate the icon and thumbnail cache.
        /// This will also load new icon and thumbnail handlers that have been registered. However, icon overlay handlers are not reloaded.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Performance", "CA1822:Mark members as static", Justification = "By design. Clients have access to an object instance anyway." )]
        public void NotifyShell() {

            NativeMethods.SHChangeNotify( SHCNE.ASSOCCHANGED, SHCNF.IDLIST, IntPtr.Zero, IntPtr.Zero );
        }

        /// <summary>
        /// Tests, if shell extensions needs to be approved by an administrator.
        /// </summary>
        /// <remarks>
        /// <seealso href="https://docs.microsoft.com/en-us/windows/win32/shell/reg-shell-exts#registering-shell-extension-handlers-1">Registering Shell Extension Handlers - Microsoft Docs</seealso>
        /// </remarks>
        /// <returns>true, if approval is configured. Otherwise, false.</returns>
        public bool NeedsApproval() {

            // Check, if Admin limited usage of shell extensions to approved extensions only.See also

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( RegistryHive.CurrentUser, RegistryView.Default ) ) {
                using( IRegistryKey explorerKey = hiveKey.OpenSubKey( @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer" ) ) {
                    if( null == explorerKey ) {
                        return false;
                    }

                    string value = explorerKey.GetString( "EnforceShellExtensionSecurity" );

                    if( null == value ) {
                        return false;
                    }

                    int v = int.Parse( value );
                    if( v == 0 ) {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Test, if a shell extension is approved.
        /// </summary>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <returns></returns>
        public bool IsApproved( string clsid ) {

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Default ) ) {
                using( IRegistryKey approvedKey = hiveKey.OpenSubKey( @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved" ) ) {

                    string approvedClsidValue = approvedKey.GetString( clsid );
                    return approvedClsidValue != null;  // Test for null means, is there any value within Registry or is the value missing

                }
            }
        }

        /// <summary>
        /// Add a shell extension to the list of approved extensions.
        /// </summary>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <param name="serverName">The name of the shell extension.</param>
        /// <remarks>
        /// This call may fail, when invoking process has no administrative privileges or the process is not elevated.
        /// </remarks>
        public void AddApproval( string clsid, string serverName ) {

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Default ) ) {
                using( IRegistryKey approvedKey = hiveKey.OpenSubKey( @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true ) ) {
                    approvedKey.SetValue( clsid, serverName, RegistryValueKind.String );
                }
            }
        }

        /// <summary>
        /// Removes a shell extension from the list of approved extensions.
        /// </summary>
        /// <param name="clsid">The clsid of the shell extension.</param>
        /// <remarks>
        /// This call may fail, when invoking process has no administrative privileges or the process is not elevated.
        /// </remarks>
        public void RemoveApproval( string clsid ) {

            // This may fail, if current user is not Admin AND/OR process is not elevated
            using( IRegistryKey hiveKey = _registry.OpenBaseKey( RegistryHive.LocalMachine, RegistryView.Default ) ) {
                using( IRegistryKey approvedKey = hiveKey.OpenSubKey( @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true ) ) {
                    approvedKey.DeleteValue( clsid, false );
                }
            }
        }

        /// <summary>
        /// Retrieves the classes registry key for a specific registration scope.
        /// </summary>
        /// <param name="scope">The registration scope.</param>
        /// <param name="writable">Controls, if the returned Registry Key is opened writable.</param>
        /// <returns>The scoped classes registry key.</returns>
        IRegistryKey GetScopedClassesKey( ComponentRegistrationScope scope, bool writable ) {

            RegistryHive hive;

            switch( scope ) {
                case ComponentRegistrationScope.System: {
                        hive = RegistryHive.LocalMachine;
                        break;
                    }
                case ComponentRegistrationScope.User: {
                        hive = RegistryHive.CurrentUser;
                        break;
                    }
                case ComponentRegistrationScope.None:
                case ComponentRegistrationScope.Unknown:
                default: {
                        throw new ArgumentOutOfRangeException( nameof( scope ) );
                    }
            }

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( hive, RegistryView.Default ) ) {
                return hiveKey.OpenSubKey( @"Software\Classes\", writable );
            }
        }
    }
}
