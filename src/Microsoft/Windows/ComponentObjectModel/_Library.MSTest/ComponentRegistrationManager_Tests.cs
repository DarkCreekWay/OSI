using System;
using System.Reflection;

using DarkCreekWay.OSI.Microsoft.Windows.Registry;
using DarkCreekWay.OSI.Microsoft.Windows.Registry.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel {

    [TestClass]
    [TestCategory( "OSI.Microsoft.Windows.ComponentObjectModel" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0059:Remove unnecessary value assignment", Justification = "Not relevant for Unit Tests" )]
    public class ComponentRegistrationManager_Tests {

        //! .NET Core and COM
        //
        //  .NET Core builds a shim dll for exposed COM components automatically on compile.
        //  The shim dll is named <assemblyname>.comhost.dll
        //  The shim dll can be used for registering with regsvr32
        //  The shim dll is used as the InprocServer32
        //  => for .NET Core, No special assembly registry handling is needed.

        // This test Method has a dependency to the ContextMenuShellExtension. But I want the tests to be isolated from specific parts of the FileOps Manager App, etc.
        // SO for now, this test is commented out.
        //[TestMethod]
        //public void ComComponentManager_API_TDD() {

        //    //! For now, my implementation is targeting .NET Framework.
        //    //
        //    //! Observations and conclusions
        //    //  COM components heavily depend on the Microsoft Windows Registry
        //    //  The registration of a COM components makes a component available to the COM subsystem. This is true for any type of COM component.
        //    //  The registration/association of a COM component as Shell Extension handles shell specific details. This requires additional steps.
        //    //  Both topics are related to each other, but are focussing on specific areas.
        //    //  This indicates, that the required functionalities should be encapsulated in distinct types.

        //    IRegistry registry = new WindowsRegistry();
        //    //IRegistry registry = new InMemoryRegistry(RegistryView.Registry64);
        //    //using ( IRegistryKey hiveKey = registry.OpenBaseKey( RegistryHive.CurrentUser, RegistryView.Default ) ) {
        //    //    hiveKey.CreateSubKey( @"SOFTWARE\Classes\CLSID" );
        //    //}

        //    Type implementingType = typeof(ContextMenuExtension); // The type, implementing the COM component

        //    // Create the component manager instance
        //    ComponentManager componentManager = new ComponentManager(TestSetup.GetLoggingService(), registry);

        //    // Create a ComponentInformation instance for the implementing type.

        //    // Inspect implementing type, build a ComponentInfo instance from it or throw, if implementing type exposes problems
        //    ComponentInformation componentInfo = componentManager.Inspect(implementingType);

        //    // As an alternative, a ComponentInformationInstance could be defined manually too.
        //    //DotNetFrameworkComponentInformation componentInfo = new DotNetFrameworkComponentInformation() {
        //    //    CLSID = implementingType.GUID,
        //    //    DotNetShimPath = @"C:\windows\system32\mscoree.dll",
        //    //    ServerType = ComponentServerType.InprocServer32,
        //    //    ThreadingModel = ComponentThreadingModel.Both,
        //    //    SharedTypeInformation = new DotNetFrameworkComponentTypeInformation() {
        //    //        AssemblyName = implementingType.Assembly.FullName,
        //    //        ClassName = implementingType.FullName,
        //    //        RuntimeVersion = implementingType.Assembly.ImageRuntimeVersion,
        //    //        Codebase = implementingType.Assembly.CodeBase
        //    //    }
        //    //};

        //    // Register Component
        //    componentManager.Register( componentInfo, ComponentRegistrationScope.User );

        //}

        [TestMethod]
        public void ReadComponentRegistrationInformationByClsid_TDD() {

            IRegistry registry = CreateRegistryWithTestData();
            ComponentRegistrationManager componentRegistrationManager = new ComponentRegistrationManager( registry );
            ComponentRegistrationInformation actualComponentRegistrationInformation = componentRegistrationManager.ReadByClsid( s_ComponentGuid, ComponentRegistrationScope.User );
        }

        [TestMethod]
        public void ReadDotNetCoreComponentRegistrationInformation() {

            // Works only, if RegFile from Reference Files has been registered beforehand

            ComponentRegistrationManager componentRegistratrionManager = new ComponentRegistrationManager();
            ComponentRegistrationInformation componentRegistrationInformation = componentRegistratrionManager.ReadByClsid( "{93DEE2FF-1446-4119-A78D-60858BD38E9D}", ComponentRegistrationScope.System );

        }

        [TestMethod]
        public void CompareComponentRegistrationInformation_TDD() {

            IRegistry registry = CreateRegistryWithTestData();

            ComponentRegistrationManager componentRegistrationManager = new ComponentRegistrationManager( registry );
            ComponentRegistrationInformation actualComponentRegistrationInformation = componentRegistrationManager.ReadByClsid( s_ComponentGuid, ComponentRegistrationScope.User );

            // AssemblyName comparison

            AssemblyName expectedAssemblyName = new AssemblyName( GetType().Assembly.FullName );

            // For testing purposes, lets assume, somebody tempered with the casing of the assembly name.
            AssemblyName actualAssemblyName = new AssemblyName( ( (DotNetFrameworkComponentRegistrationInformation)actualComponentRegistrationInformation ).SharedTypeInformation.AssemblyName.ToLowerInvariant() );

            // This comparison only compares the simple assembly name and ignores version, public key token etc.
            // see also https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assemblyname.referencematchesdefinition#remarks

            bool assemblyNameReferenceMatch = AssemblyName.ReferenceMatchesDefinition( actualAssemblyName, expectedAssemblyName );
            Console.WriteLine( $"AssemblyName Reference Match: {assemblyNameReferenceMatch}" );

            // Is it good enough to compare by fullname property ?
            // This comparison includes all relevant aspects (name, culture, version, public key token)
            bool assemblyNameFullNameMatch = expectedAssemblyName.FullName.Equals( actualAssemblyName.FullName );
            Console.WriteLine( $"AssemblyName FullName Match: {assemblyNameFullNameMatch}" );

            // Some concerns:
            //
            // Does the runtime implicitly search for the assembly and read the data from it ?
            // - One indicator for this is, that the casing of the actualAssemblyName is corrected "magically"
            // - What, if we do not have the assembly in direct reach / not found ?
            // Does the public key token change, if there is an assembly with a different culture ?
            // - This could be tested.
            // Would it probably better, to compare name and version only ? (Separate properties are available on the AssemblyName type for this. The only value not exposed as property is the publickeytoken
            // For this, a call to GetPublicKey() is needed (See below) or GetPublicKeyToken
            //
            //#if FEATURE_APTCA
            //        internal string GetNameWithPublicKey()
            //        {
            //            byte[] key = GetPublicKey();

            //            // The following string should not be localized because it is used in security decisions.
            //            return Name + ", PublicKey=" + System.Security.Util.Hex.EncodeHexString(key);
            //        }
            //#endif
            //ShellExtensionInformation shellExtensionInformation = shellExtensionManager.?
            ;

            // Get public Key Token from AssemblyName instance
            byte[] expectedPublicKeyToken = expectedAssemblyName.GetPublicKeyToken();

            // Codebase comparison
            Uri expectedCodebase = new Uri( GetType().Assembly.Location );
            Uri actualCodebase = new Uri( ( (DotNetFrameworkComponentRegistrationInformation)actualComponentRegistrationInformation ).SharedTypeInformation.Codebase );
            // Codebases differ in casing.
            // On Windows -> NTFS Filesystem, this should not be a problem, as it treats different casings as same path. So the comparison is considered a safe approach.
            bool codebaseMatch = expectedCodebase.LocalPath.Equals( actualCodebase.LocalPath, StringComparison.InvariantCultureIgnoreCase );

            Console.WriteLine( $"Codebase Match : {codebaseMatch}" );

        }

        static Guid s_ComponentGuid = new Guid( "30E0369A-D1DB-451B-95B6-F57F33216BD6" );

        IRegistry CreateRegistryWithTestData() {

            IRegistry registry = new InMemoryRegistry( RegistryView.Registry64 );
            Assembly assembly = GetType().Assembly;

            using( IRegistryKey hiveKey = registry.OpenBaseKey( RegistryHive.CurrentUser, RegistryView.Default ) ) {

                using( IRegistryKey clsidKey = hiveKey.CreateSubKey( @"SOFTWARE\Classes\CLSID" ) ) {

                    // .NET Framework component registration
                    using( IRegistryKey componentKey = clsidKey.CreateSubKey( s_ComponentGuid.ToString( "B" ) ) ) {
                        using( IRegistryKey inprocServer32Key = componentKey.CreateSubKey( "InprocServer32" ) ) {

                            inprocServer32Key.SetValue( "", "mscoree.dll" );
                            inprocServer32Key.SetValue( "ThreadingModel", "Both" );
                            inprocServer32Key.SetValue( "Assembly", assembly.FullName.ToLowerInvariant() );
                            inprocServer32Key.SetValue( "Class", GetType().FullName );
                            //inprocServer32Key.SetValue( "CodeBase", GetType().Assembly.CodeBase.ToLowerInvariant() );
                            inprocServer32Key.SetValue( "CodeBase", assembly.Location.ToLowerInvariant() );
                            inprocServer32Key.SetValue( "RuntimeVersion", assembly.ImageRuntimeVersion );

                            // TODO: Add Versioned Keys and values.
                        }
                    }
                }
            }

            return registry;

        }
    }
}
