using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Metadata;
using DarkCreekWay.OSI.Microsoft.Windows.Registry;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    /// <summary>
    /// The Component Registration Manager provides methods for registering, reading registration information and unregistering COM components.
    /// </summary>
    /// <remarks>
    /// The <see cref="ComRegisterFunctionAttribute"/> and <see cref="ComUnregisterFunctionAttribute"/> are not supported.
    /// See also
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.comregisterfunctionattribute"> ComRegisterFunctionAttribute Class</a>
    /// <a href="https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.comunregisterfunctionattribute"> ComUnregisterFunctionAttribute Class - Microsoft Docs</a>
    /// </remarks>

#if NET5_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform( "windows" )]
#endif
    public class ComponentRegistrationManager {

        const string s_CLSIDRegKeyName = @"SOFTWARE\Classes\CLSID";
        const string s_ProgIdKeyName = "ProgId";
        const string s_AssemblyRegValueName = "Assembly";
        const string s_ClassRegValueName = "Class";
        const string s_CodebaseRegValueName = "Codebase";
        const string s_RuntimeVersionRegValueName = "RuntimeVersion";

        IRegistry _registry;

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentRegistrationManager"/> class.
        /// </summary>
        public ComponentRegistrationManager()
        : this( new WindowsRegistry() ) {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentRegistrationManager"/> class using a specific <see cref="IRegistry"/> implementation.
        /// </summary>
        /// <param name="registry">An <see cref="IRegistry"/> implementation.</param>
        public ComponentRegistrationManager( IRegistry registry ) {

            Debug.Assert( registry != null );
            _registry = registry;
        }

        /// <summary>
        /// Reads component registration information from the Windows Registry.
        /// </summary>
        /// <param name="clsid">The CLSID of the component.</param>
        /// <param name="scope">The <see cref="ComponentRegistrationScope"/> to read the component registration information from.</param>
        /// <returns>A <see cref="ComponentRegistrationInformation"/> instance on success. Returns null, when no information is found.</returns>
        /// <remarks>
        /// The method returns any information it can find for a component in the windows registry.
        /// The returned data needs to be validated by the caller prior for completness and accuracy.
        /// </remarks>
        public ComponentRegistrationInformation ReadByClsid( Guid clsid, ComponentRegistrationScope scope ) {

            return ReadByClsid( Utils.GuidToClsid( clsid ), scope );
        }

        /// <summary>
        /// Reads component registration information from the Windows Registry,
        /// </summary>
        /// <param name="clsid">The CLSID of the component</param>
        /// <param name="scope">The <see cref="ComponentRegistrationScope"/> to read the component registration information from.</param>
        /// <returns>A <see cref="ComponentRegistrationInformation"/> instance on success. Returns null, when no information is found.</returns>
        /// <remarks>
        /// The method returns any information it can find for a component in the windows registry.
        /// The returned data needs to be validated by the caller prior for completness and accuracy.
        /// </remarks>
        public ComponentRegistrationInformation ReadByClsid( string clsid, ComponentRegistrationScope scope ) {

            // TODO: Ensure, that partial results are also passed back to caller. This is currently not the case for all cases.

            if( string.IsNullOrEmpty( clsid ) ) {
                throw new ArgumentNullException( nameof( clsid ) );
            }

            RegistryHive hive = GetHiveForScopeOrThrow( scope );

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( hive, RegistryView.Default ) ) {

                using( IRegistryKey clsidKey = hiveKey.OpenSubKey( s_CLSIDRegKeyName ) ) {
                    using( IRegistryKey componentKey = clsidKey.OpenSubKey( clsid ) ) {

                        if( null == componentKey ) {
                            return null;
                        }

                        string progId;

                        using( IRegistryKey progIdKey = componentKey.OpenSubKey( s_ProgIdKeyName ) ) {
                            progId = progIdKey?.GetString( "", "" ) ?? "";
                        }

                        // Detect ServerType (InprocServer, InprocServer32, LocalServer, LocalServer32
                        ComponentServerType? serverType = null;

                        string[] serverKeyNames = componentKey.GetSubKeyNames();

                        if( serverKeyNames.Length == 0 ) {
                            return null;
                        }

                        // TODO: Improve ServerType detection
                        // This detection logic has an issue, as it does not take into consideration, that there could be 2 in parallel for 16 and 32 bit
                        foreach( string serverKeyName in serverKeyNames ) {
                            if( true == Enum.TryParse( serverKeyName, true, out ComponentServerType detectedServerType ) ) {
                                serverType = detectedServerType;
                                break;
                            }
                        }

                        if( !serverType.HasValue ) {
                            return null;
                        }

                        using( IRegistryKey serverKey = componentKey.OpenSubKey( Enum.GetName( typeof( ComponentServerType ), serverType ) ) ) {

                            string path = serverKey.GetString( "" );

                            _ = Enum.TryParse( serverKey.GetString( "ThreadingModel" ), true, out ComponentThreadingModel threadingModel );

                            // .NET Core COM component registration
                            if( path.EndsWith( ".comhost.dll" ) ) {

                                // TODO: Calculate component library path based on ComHost path

                                DotNetCoreComponentRegistrationInformation dotNetCore = new DotNetCoreComponentRegistrationInformation() {
                                    CLSID = Guid.Parse( clsid ),
                                    ComHost = path,
                                    Codebase = Utils.GetLibraryPath( path ),
                                    ProgId = progId,
                                    ServerType = serverType.Value,
                                    ThreadingModel = threadingModel,
                                };

                                return dotNetCore;
                            }

                            // .NET Framework COM component registration
                            if( path.EndsWith( "mscoree.dll", StringComparison.OrdinalIgnoreCase ) ) {
                                // What, if default reg value is manipulated/corrupted, so that this reading logic would miss the .NET Framework specifics ?
                                // Probably, this reading logic needs some revision.

                                DotNetFrameworkComponentRegistrationInformation dotNetFrameworkComponentInfo = new DotNetFrameworkComponentRegistrationInformation() {
                                    DotNetShimPath = path,
                                    CLSID = Guid.Parse( clsid ),
                                    ProgId = progId,
                                    ServerType = serverType.Value,
                                    ThreadingModel = threadingModel,
                                    SharedTypeInformation = GetValues( serverKey ),
                                };

                                string[] versionKeyNames = serverKey.GetSubKeyNames();
                                DotNetFrameworkComponentRegistrationTypeInformation versionedTypeInfo = null;

                                foreach( string versionKeyName in versionKeyNames ) {
                                    using( IRegistryKey versionedKey = serverKey.OpenSubKey( versionKeyName ) ) {
                                        versionedTypeInfo = GetValues( versionedKey );
                                        versionedTypeInfo.AssemblyVersion = versionKeyName;
                                        dotNetFrameworkComponentInfo.VersionedTypeInformation.Add( versionedTypeInfo );
                                    }
                                }

                                return dotNetFrameworkComponentInfo;
                            }

                            // Native component registration
                            NativeComponentRegistrationInformation native = new NativeComponentRegistrationInformation() {
                                CLSID = Guid.Parse( clsid ),
                                Path = path,
                                ProgId = progId,
                                ServerType = serverType.Value,
                                ThreadingModel = threadingModel,
                            };

                            return native;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes component registraion information to the Windows Registry.
        /// </summary>
        /// <param name="componentRegistrationInformation">The component registration information.</param>
        /// <param name="scope">The <see cref="ComponentRegistrationScope"/>.</param>
        public void Register( ComponentRegistrationInformation componentRegistrationInformation, ComponentRegistrationScope scope ) {

            if( componentRegistrationInformation is DotNetFrameworkComponentRegistrationInformation dotNetFrameworkComponentRegistrationInformation ) {
                Register( dotNetFrameworkComponentRegistrationInformation, scope );
                return;
            }

            if( componentRegistrationInformation is DotNetCoreComponentRegistrationInformation dotNetCoreComponentRegistrationInformation ) {
                Register( dotNetCoreComponentRegistrationInformation, scope );
                return;
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Registers a COM component, implemented with the .NET Framework.
        /// </summary>
        /// <param name="componentRegistrationInformation">The COM component information.</param>
        /// <param name="scope">The <see cref="ComponentRegistrationScope"/>.</param>
        public void Register( DotNetFrameworkComponentRegistrationInformation componentRegistrationInformation, ComponentRegistrationScope scope ) {

            RegistryHive hive = GetHiveForScopeOrThrow( scope );

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( hive, RegistryView.Default ) ) {

                using( IRegistryKey classesClsidKey = hiveKey.OpenSubKey( @"Software\Classes\CLSID\", true ) ) {

                    using( IRegistryKey clsidKey = classesClsidKey.CreateSubKey( Utils.GuidToClsid( componentRegistrationInformation.CLSID ), true ) ) {

                        using( IRegistryKey serverKey = clsidKey.CreateSubKey( componentRegistrationInformation.ServerType.ToString(), true ) ) {

                            switch( componentRegistrationInformation.ServerType ) {

                                case ComponentServerType.InprocServer32: {
                                        serverKey.SetValue( "ThreadingModel", componentRegistrationInformation.ThreadingModel.ToString(), RegistryValueKind.String );
                                        serverKey.SetValue( "", componentRegistrationInformation.DotNetShimPath );
                                        break;
                                    }

                                case ComponentServerType.InprocServer:
                                case ComponentServerType.LocalServer:
                                case ComponentServerType.LocalServer32: {
                                        throw new NotImplementedException();
                                    }
                                default: {
                                        throw new ArgumentException();
                                    }
                            }

                            // Write shared values
                            SetValues( serverKey, componentRegistrationInformation.SharedTypeInformation );

                            // Write Version-Dependent subkeys and values

                            if( null == componentRegistrationInformation.VersionedTypeInformation || componentRegistrationInformation.VersionedTypeInformation.Count == 0 ) {
                                // Case 1: Caller did not provide a list of version-dependent values
                                using( IRegistryKey versionedKey = serverKey.CreateSubKey( componentRegistrationInformation.SharedTypeInformation.AssemblyVersion ) ) {
                                    SetValues( versionedKey, componentRegistrationInformation.SharedTypeInformation );
                                }
                            }
                            else {
                                // Case 2: Caller provided a list of version-dependent data
                                foreach( DotNetFrameworkComponentRegistrationTypeInformation entry in componentRegistrationInformation.VersionedTypeInformation ) {
                                    using( IRegistryKey versionedKey = serverKey.CreateSubKey( entry.AssemblyVersion ) ) {
                                        SetValues( versionedKey, entry );
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers a COM component, implemented with .NET Core.
        /// </summary>
        /// <param name="dotNetCoreComponentRegistrationInformation">The COM component information.</param>
        /// <param name="scope">The registration scope.</param>
        public void Register( DotNetCoreComponentRegistrationInformation dotNetCoreComponentRegistrationInformation, ComponentRegistrationScope scope ) {

            RegistryHive hive = GetHiveForScopeOrThrow( scope );

            using( IRegistryKey hiveKey = _registry.OpenBaseKey( hive, RegistryView.Default ) ) {

                using( IRegistryKey classesClsidKey = hiveKey.OpenSubKey( @"Software\Classes\CLSID\", true ) ) {

                    using( IRegistryKey clsidKey = classesClsidKey.CreateSubKey( Utils.GuidToClsid( dotNetCoreComponentRegistrationInformation.CLSID ), true ) ) {

                        using( IRegistryKey serverKey = clsidKey.CreateSubKey( dotNetCoreComponentRegistrationInformation.ServerType.ToString(), true ) ) {

                            switch( dotNetCoreComponentRegistrationInformation.ServerType ) {

                                case ComponentServerType.InprocServer32: {
                                        serverKey.SetValue( "ThreadingModel", dotNetCoreComponentRegistrationInformation.ThreadingModel.ToString(), RegistryValueKind.String );
                                        serverKey.SetValue( "", dotNetCoreComponentRegistrationInformation.ComHost );
                                        break;
                                    }

                                case ComponentServerType.InprocServer:
                                case ComponentServerType.LocalServer:
                                case ComponentServerType.LocalServer32: {
                                        throw new NotImplementedException();
                                    }
                                default: {
                                        throw new ArgumentException();
                                    }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers all COM components defined by <see cref="ComponentLibraryMetadata"/>
        /// </summary>
        /// <param name="libraryMetadata">The component library metadata.</param>
        /// <param name="scope">The registration scope.</param>
        public void Register( ComponentLibraryMetadata libraryMetadata, ComponentRegistrationScope scope ) {

            for( int i = 0; i < libraryMetadata.Components.Count; i++ ) {
                Register( libraryMetadata.Components[i], scope );
            }
        }

        /// <summary>
        /// Unregisters a COM component.
        /// </summary>
        /// <param name="clsid">The clsid of the COM component.</param>
        /// <param name="scope">The registration scope.</param>
        public void Unregister( string clsid, ComponentRegistrationScope scope ) {

            RegistryHive hive = GetHiveForScopeOrThrow( scope );
            using( IRegistryKey hiveKey = _registry.OpenBaseKey( hive, RegistryView.Default ) ) {
                using( IRegistryKey classesClsidKey = hiveKey.OpenSubKey( s_CLSIDRegKeyName, true ) ) {

                    if( null == classesClsidKey ) {
                        return;
                    }

                    classesClsidKey.DeleteSubKeyTree( clsid, false );
                }
            }
        }

        /// <summary>
        /// Unregisters a COM component.
        /// </summary>
        /// <param name="clsid">The clsid of the COM component.</param>
        /// <param name="scope">The registration scope.</param>
        public void Unregister( Guid clsid, ComponentRegistrationScope scope ) {

            RegistryHive hive = GetHiveForScopeOrThrow( scope );
            using( IRegistryKey hiveKey = _registry.OpenBaseKey( hive, RegistryView.Default ) ) {
                using( IRegistryKey classesClsidKey = hiveKey.OpenSubKey( s_CLSIDRegKeyName, true ) ) {

                    if( null == classesClsidKey ) {
                        return;
                    }

                    classesClsidKey.DeleteSubKeyTree( Utils.GuidToClsid( clsid ), false );
                }
            }
        }

        /// <summary>
        /// Unregisters a COM component.
        /// </summary>
        /// <param name="componentRegistrationInformation">The component registration information.</param>
        /// <param name="scope">The registration scope.</param>
        public void Unregister( ComponentRegistrationInformation componentRegistrationInformation, ComponentRegistrationScope scope ) {

            Unregister( componentRegistrationInformation.CLSID, scope );
        }

        /// <summary>
        /// Returns the <see cref="RegistryHive"/> for a given <see cref="ComponentRegistrationScope"/>
        /// </summary>
        /// <param name="scope">The <see cref="ComponentRegistrationScope"/></param>
        /// <returns>The <see cref="RegistryHive"/> value that corresponds to the given <see cref="ComponentRegistrationScope"/></returns>
        static RegistryHive GetHiveForScopeOrThrow( ComponentRegistrationScope scope ) {

            if( scope == ComponentRegistrationScope.None || scope == ComponentRegistrationScope.Unknown ) {
                throw new ArgumentException( nameof( scope ) );
            }

            return scope == ComponentRegistrationScope.System ? RegistryHive.LocalMachine : RegistryHive.CurrentUser;
        }

        static void SetValues( IRegistryKey key, DotNetFrameworkComponentRegistrationTypeInformation typeInfo ) {

            key.SetString( s_AssemblyRegValueName, typeInfo.AssemblyName );
            key.SetString( s_ClassRegValueName, typeInfo.TypeName );
            key.SetString( s_CodebaseRegValueName, typeInfo.Codebase );
            key.SetString( s_RuntimeVersionRegValueName, typeInfo.RuntimeVersion );

        }

        static DotNetFrameworkComponentRegistrationTypeInformation GetValues( IRegistryKey key ) {

            return new DotNetFrameworkComponentRegistrationTypeInformation() {
                AssemblyName = key.GetString( s_AssemblyRegValueName ),
                TypeName = key.GetString( s_ClassRegValueName ),
                Codebase = key.GetString( s_CodebaseRegValueName ),
                RuntimeVersion = key.GetString( s_RuntimeVersionRegValueName ),
            };

        }
    }
}
