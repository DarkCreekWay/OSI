using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace DarkCreekWay.OSI.Microsoft.Windows.ComponentObjectModel.Metadata {

    /// <summary>
    /// Reads COM component metadata from COM Library files.
    /// </summary>
    /// <remarks>
    /// Backround information about Metadata reading with .NET
    /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.reflection.metadata.metadatareader">MetadataReader Class - Microsoft Docs</see>
    /// </remarks>
    public class ComponentLibraryMetadataReader {

        ICustomAttributeTypeProvider<Type> _customAttributeTypeProvider;
        Dictionary<string,Type>            _supportedCustomAttributes;

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentLibraryMetadataReader"/>
        /// </summary>
        public ComponentLibraryMetadataReader() {

            _customAttributeTypeProvider = CreateCustomAttributeTypeProvider();
            _supportedCustomAttributes = new Dictionary<string, Type>( 24 );

            AddSupportedAttributes( CreateSupportedAttributes() );
            AddSupportedAttributes( CreateAdditionalSupportedAttributes() );
        }

        /// <summary>
        /// Provides the <see cref="ComponentThreadingModel"/> fallback value to be used, when no attribute was associated with a type.
        /// </summary>
        /// <value>Defaults to <see cref="ComponentThreadingModel.Undefined"/></value>
        protected virtual ComponentThreadingModel FallbackThreadingModel => ComponentThreadingModel.Undefined;

        /// <summary>
        /// Provides the <see cref="ComponentServerType"/> fallback value to be used, when no attribute was associated with a type.
        /// </summary>
        /// <value>Defaults to <see cref="ComponentServerType.Undefined"/></value>
        protected virtual ComponentServerType FallbackServerType => ComponentServerType.Undefined;


        /// <summary>
        /// Returns an implementing type of the <see cref="ICustomAttributeTypeProvider{TType}"/> interface
        /// </summary>
        /// <returns>an implementing type of the <see cref="ICustomAttributeTypeProvider{TType}"/> interface</returns>
        /// <remarks>
        /// The returned instance is used for converting metadata to attribute types.
        /// </remarks>
        protected virtual ICustomAttributeTypeProvider<Type> CreateCustomAttributeTypeProvider() => new CustomAttributeTypeProvider();

        /// <summary>
        /// Additional IEnumerable of Types derieved from the Attribute type to be read from assemblies and/or types.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> where T = <see cref="Type"/>. Instances must be derieved from the <see cref="Attribute"/> class.</returns>
        /// <remarks>
        /// The returned types are used in addition to the set of prediefined attributes. Inheritors can overwrite this method for specifying additional attributes.
        /// </remarks>
        protected virtual IEnumerable<Type> CreateAdditionalSupportedAttributes() {
            return null;
        }

        /// <summary>
        /// IEnumerable of Types derieved from the Attribute type to be read from assemblies and/or types.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> where T = <see cref="Type"/>. Instances must be derieved from the <see cref="Attribute"/> class.</returns>
        /// <remarks>
        /// The returned types defines the attributes always supported. Inheritors can overwrite the <see cref="CreateAdditionalSupportedAttributes"/> Method for specifing additional attributes.
        /// </remarks>
        IEnumerable<Type> CreateSupportedAttributes() {

            return new List<Type>() {
                { typeof( AssemblyConfigurationAttribute        ) },
                { typeof( AssemblyCopyrightAttribute            ) },
                { typeof( AssemblyCompanyAttribute              ) },
                { typeof( AssemblyDescriptionAttribute          ) },
                { typeof( AssemblyFileVersionAttribute          ) },
                { typeof( AssemblyInformationalVersionAttribute ) },
                { typeof( AssemblyProductAttribute              ) },
                { typeof( AssemblyTitleAttribute                ) },
                { typeof( AssemblyTrademarkAttribute            ) },
                { typeof( CompilationRelaxationsAttribute       ) },
                { typeof( ComVisibleAttribute                   ) },
                { typeof( GuidAttribute                         ) },
                { typeof( ProgIdAttribute                       ) },
                { typeof( RuntimeCompatibilityAttribute         ) },
                { typeof( TargetFrameworkAttribute              ) },
                { typeof( ComponentServerTypeAttribute          ) },
                { typeof( ComponentThreadingModelAttribute      ) },

                // Omitted due to nested type definition of enum. Reading nested types is currently not implemented.
                // { typeof( DebuggableAttribute                   ) },
            };
        }

        /// <summary>
        /// Reads COM component information from a component library
        /// </summary>
        /// <param name="path">Full qualified path to the component library file</param>
        /// <returns>A <see cref="ComponentLibraryMetadata"/> instance</returns>
        /// <remarks>
        /// The method also accepts the path to a .NET Core ComHost file. ([file].comhost.dll)
        /// In this case, the path to the .NET core assembly gets calculated from the comhost path and the metadata gets read from the assembly.
        /// </remarks>
        public ComponentLibraryMetadata Read( string path ) {

            // Method does not scan directories
            FileAttributes fileAttributes = File.GetAttributes(path);
            if ( (fileAttributes & FileAttributes.Directory) == FileAttributes.Directory ) {
                throw new NotSupportedException();
            }

            string libraryPath = Utils.GetLibraryPath(path);
            if ( !File.Exists( libraryPath ) ) {
                throw new FileNotFoundException();
            }

            ComponentLibraryMetadata result = new ComponentLibraryMetadata() {
                Path = libraryPath,
            };

            try {
                result.AssemblyName = AssemblyName.GetAssemblyName( libraryPath );
                result.ImageType = ComponentLibraryImageType.Managed;
            }
            catch ( BadImageFormatException ) {
                throw new NotImplementedException();
            }

            using ( Stream stream = File.OpenRead( libraryPath ) ) {

                using ( PEReader reader = new PEReader( stream ) ) {

                    MetadataReader metadataReader = reader.GetMetadataReader();

                    // Process assembly information

                    // Build list of referenced assemblies. Currently not needed, so not uncommented
                    //foreach ( AssemblyReferenceHandle referenceHandle in metadataReader.AssemblyReferences ) {
                    //    AssemblyReference assemblyReference = metadataReader.GetAssemblyReference(referenceHandle);
                    //    AssemblyName      assemblyName      = assemblyReference.GetAssemblyName();
                    //}

                    result.AssemblyRuntimeVersion = metadataReader.MetadataVersion;
                    result.Attributes = ReadCustomAttributes( metadataReader, metadataReader.CustomAttributes, HandleKind.AssemblyDefinition );

                    if ( true == result.Attributes.TryGet( out TargetFrameworkAttribute targetFrameworkAttribute ) ) {

                        FrameworkName frameworkName = new FrameworkName(targetFrameworkAttribute.FrameworkName);

                        result.AssemblyTargetFrameworkVersion = frameworkName.Version;

                        switch ( frameworkName.Identifier ) {

                            case ".NETFramework": {
                                result.AssemblyTargetFramework = ComponentLibraryTargetFramework.DotNetFramework;
                                break;
                            }

                            case ".NETCoreApp": {
                                result.AssemblyTargetFramework = ComponentLibraryTargetFramework.DotNetCore;

                                break;
                            }

                            default: {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    ReadTypes( metadataReader, result );
                }
            }

            return result;
        }

        /// <summary>
        /// Reads <see cref="ComponentRegistrationInformation"/> from a given <see cref="TypeDefinition"/>
        /// </summary>
        /// <param name="reader">The <see cref="MetadataReader"/> instance to read from</param>
        /// <param name="typeDefinition">The <see cref="TypeDefinition"/> instance to read from</param>
        /// <param name="customAttributes">The <see cref="ComponentAttributeCollection"/> instance associated with the TypeDefinition</param>
        /// <param name="libraryMetadata">The <see cref="ComponentLibraryMetadata"/> instance for the component library file</param>
        /// <returns>A <see cref="ComponentRegistrationInformation"/> instance on success, null, otherwise</returns>
        /// <remarks>
        /// The returned instance gets added to the ComponentLinraryMetadata instance by the caller of this method.
        /// </remarks>
        protected virtual ComponentRegistrationInformation ReadComponentInformation( MetadataReader reader, TypeDefinition typeDefinition, ComponentAttributeCollection customAttributes, ComponentLibraryMetadata libraryMetadata ) {

            if ( false == customAttributes.TryGet( out GuidAttribute guidAttribute ) ) {
                return null;
            }

            string clsid         = guidAttribute.Value;
            string typeNamespace = reader.GetString( typeDefinition.Namespace );
            string typeName      = reader.GetString( typeDefinition.Name );
            string fullTypeName  = typeNamespace + "." + typeName;

            switch ( libraryMetadata.AssemblyTargetFramework ) {

                case ComponentLibraryTargetFramework.DotNetFramework: {

                    string shimPath = Path.Combine(Environment.SystemDirectory, "mscoree.dll" );

                    if ( !File.Exists( shimPath ) ) {
                        throw new InvalidOperationException( "mscoree.dll not found on this system." );
                    }

                    return new DotNetFrameworkComponentRegistrationInformation() {
                        Attributes = customAttributes,
                        TypeName = fullTypeName,
                        CLSID = Utils.ClsidToGuid( clsid ),
                        ProgId = customAttributes.Get<ProgIdAttribute>()?.Value ?? "",
                        DotNetShimPath = shimPath,
                        ThreadingModel = customAttributes.Get<ComponentThreadingModelAttribute>()?.ThreadingModel ?? FallbackThreadingModel,
                        ServerType = customAttributes.Get<ComponentServerTypeAttribute>()?.ServerType ?? FallbackServerType,
                        SharedTypeInformation = new DotNetFrameworkComponentRegistrationTypeInformation() {
                            Attributes = customAttributes,
                            TypeName = fullTypeName,
                            Codebase = libraryMetadata.Path,
                            AssemblyName = libraryMetadata.AssemblyName.FullName,
                            AssemblyVersion = libraryMetadata.AssemblyName.Version.ToString(),
                            AssemblyHasStrongName = libraryMetadata.AssemblyHasStrongName,
                            RuntimeVersion = libraryMetadata.AssemblyRuntimeVersion,
                        },
                        VersionedTypeInformation = {
                                        { new DotNetFrameworkComponentRegistrationTypeInformation() {
                                            Attributes            = customAttributes,
                                            TypeName              = fullTypeName,
                                            Codebase              = libraryMetadata.Path,
                                            AssemblyName          = libraryMetadata.AssemblyName.FullName,
                                            AssemblyVersion       = libraryMetadata.AssemblyName.Version.ToString(),
                                            AssemblyHasStrongName = libraryMetadata.AssemblyHasStrongName,
                                            RuntimeVersion        = libraryMetadata.AssemblyRuntimeVersion,
                                        } },
                                    },
                    };
                }

                case ComponentLibraryTargetFramework.DotNetCore: {
                    return new DotNetCoreComponentRegistrationInformation() {
                        Attributes = customAttributes,
                        TypeName = fullTypeName,
                        ComHost = Utils.GetComHostPath( libraryMetadata.Path ),
                        Codebase = libraryMetadata.Path,
                        CLSID = Utils.ClsidToGuid( clsid ),
                        ProgId = customAttributes.Get<ProgIdAttribute>()?.Value ?? "",
                        ThreadingModel = customAttributes.Get<ComponentThreadingModelAttribute>()?.ThreadingModel ?? FallbackThreadingModel,
                        ServerType = customAttributes.Get<ComponentServerTypeAttribute>()?.ServerType ?? FallbackServerType,
                    };
                }

                case ComponentLibraryTargetFramework.Unknown:
                default: {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Reads <see cref="Attribute"/> instances from a given <see cref="ComponentAttributeCollection"/> for a specific <see cref="HandleKind"/>
        /// </summary>
        /// <param name="reader">The <see cref="MetadataReader"/> instance to read from.</param>
        /// <param name="customAttributeHandles">The <see cref="ComponentAttributeCollection"/> instance to read from.</param>
        /// <param name="handleKind">The <see cref="HandleKind"/> to read.</param>
        /// <returns>A <see cref="ComponentAttributeCollection"/> instance, containing <see cref="Attribute"/> instances, as supported and read from the given HandleCollection</returns>
        ComponentAttributeCollection ReadCustomAttributes( MetadataReader reader, CustomAttributeHandleCollection customAttributeHandles, HandleKind handleKind ) {

            ComponentAttributeCollection result = new ComponentAttributeCollection();

            foreach ( CustomAttributeHandle handle in customAttributeHandles ) {

                CustomAttribute customAttribute = reader.GetCustomAttribute(handle);

                if ( customAttribute.Parent.Kind != handleKind ) {
                    continue;
                }

                MemberReference constructorReference = reader.GetMemberReference((MemberReferenceHandle) customAttribute.Constructor);
                TypeReference   typeReference        = reader.GetTypeReference((TypeReferenceHandle) constructorReference.Parent);

                string typeNamespace = reader.GetString(typeReference.Namespace);
                string typeName      = reader.GetString(typeReference.Name);
                string fullTypeName  = typeNamespace + "." + typeName;

                if ( false == _supportedCustomAttributes.TryGetValue( fullTypeName, out Type attributeType ) ) {
                    continue;
                }

                CustomAttributeValue<Type> customAttributeValue = customAttribute.DecodeValue(_customAttributeTypeProvider);

                //  Building array of types is only required for Type.GetConstructor - which is not used at the moment.
                //Type[] types = new Type[attributeValue.FixedArguments.Length];
                //for ( int i = 0 ; i < types.Length ; i++ ) {
                //    types[i] = (Type)attributeValue.FixedArguments[i].Type;
                //}

                // Build array of arguments to be passed to Activator.CreateInstance
                object[] args = new object[customAttributeValue.FixedArguments.Length];
                for ( int i = 0 ; i < args.Length ; i++ ) {

                    Type argType = customAttributeValue.FixedArguments[i].Type;
                    //Type argType = (Type)val.FixedArguments[i].Type;
                    if ( argType.IsEnum ) {
                        args[i] = Enum.Parse( argType, customAttributeValue.FixedArguments[i].Value.ToString() );
                        continue;
                    }

                    if ( argType.IsArray ) {
                        ImmutableArray<CustomAttributeTypedArgument<Type>> source = (ImmutableArray<CustomAttributeTypedArgument<Type>>)customAttributeValue.FixedArguments[i].Value;
                        object[] target = (object[])Array.CreateInstance( argType.GetElementType(), source.Length );

                        for ( int j = 0 ; i < target.Length ; j++ ) {

                            target[j] = source[j].Value;
                        }

                        args[i] = target;
                        continue;
                    }

                    args[i] = customAttributeValue.FixedArguments[i].Value;
                }

                Attribute attribute = (Attribute)Activator.CreateInstance( attributeType, args );

                // Assign values to Fields/Properties of attribute instance as passed as named arguments.
                for ( int i = 0 ; i < customAttributeValue.NamedArguments.Length ; i++ ) {

                    if ( CustomAttributeNamedArgumentKind.Property == customAttributeValue.NamedArguments[i].Kind ) {
                        PropertyInfo propertyInfo = attributeType.GetProperty(customAttributeValue.NamedArguments[i].Name);
                        propertyInfo.SetValue( attribute, customAttributeValue.NamedArguments[i].Value );
                        continue;
                    }

                    if ( CustomAttributeNamedArgumentKind.Field == customAttributeValue.NamedArguments[i].Kind ) {
                        FieldInfo fieldInfo = attributeType.GetField(customAttributeValue.NamedArguments[i].Name);
                        fieldInfo.SetValue( attribute, customAttributeValue.NamedArguments[i].Value );
                        continue;
                    }
                }

                result.Add( attribute );
            }

            return result;

        }

        /// <summary>
        /// Iterates <see cref="TypeDefinition"/> instances of the given <see cref="MetadataReader"/> instance.
        /// It invokes inspection and conversion of TypeDefinition instances to <see cref="ComponentRegistrationInformation"/> instances and adds them to the given <see cref="ComponentLibraryMetadata"/> instance.
        /// </summary>
        /// <param name="reader">The <see cref="MetadataReader"/> instance to read from.</param>
        /// <param name="libraryMetadata">The <see cref="ComponentLibraryMetadata"/> instance to add <see cref="ComponentRegistrationInformation"/> instances to.</param>
        protected virtual void ReadTypes( MetadataReader reader, ComponentLibraryMetadata libraryMetadata ) {

            bool libraryIsComVisible = libraryMetadata.Attributes.Get<ComVisibleAttribute>()?.Value ?? false;

            foreach ( TypeDefinitionHandle typeDefinitionHandle in reader.TypeDefinitions ) {

                TypeDefinition typeDefinition = reader.GetTypeDefinition( typeDefinitionHandle );
                string typeName               = reader.GetString( typeDefinition.Name );

                if ( typeName == "<Module>" && typeDefinition.Namespace.IsNil ) {
                    continue;
                }

                if ( !TypeImplementsComponentRequirements( reader, typeDefinition ) ) {
                    continue;
                }

                ComponentAttributeCollection customAttributes = ReadCustomAttributes(reader, typeDefinition.GetCustomAttributes(), HandleKind.TypeDefinition);

                // Validate COM Visibility

                bool typeIsComVisible = customAttributes.Get<ComVisibleAttribute>()?.Value ?? false;

                if ( !typeIsComVisible ) {

                    if ( libraryMetadata.AssemblyTargetFramework == ComponentLibraryTargetFramework.DotNetCore ) {
                        continue;
                    }

                    if ( libraryMetadata.AssemblyTargetFramework == ComponentLibraryTargetFramework.DotNetFramework ) {
                        if ( !libraryIsComVisible ) {
                            continue;
                        }
                    }
                }

                ComponentRegistrationInformation componentInformation = ReadComponentInformation(reader, typeDefinition, customAttributes, libraryMetadata);

                if ( null == componentInformation ) {
                    continue;
                }

                libraryMetadata.Components.Add( componentInformation );
            }
        }

        /// <summary>
        /// Tests a <see cref="TypeDefinition"/> instance against COM component requirements.
        /// </summary>
        /// <param name="reader">The <see cref="MetadataReader"/> instance to read from.</param>
        /// <param name="typeDefinition">The <see cref="TypeDefinition"/> instance to test</param>
        /// <returns>true, if the type fullfills COM component requirements. false otherwise.</returns>
        protected virtual bool TypeImplementsComponentRequirements( MetadataReader reader, TypeDefinition typeDefinition ) {

            // COM registration and activation requires the type to be a class.
            // The class has to be public, not abstact, not static and not generic
            if ( (typeDefinition.Attributes & TypeAttributes.Public) != TypeAttributes.Public ||
                 (typeDefinition.Attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract ||
                 typeDefinition.GetGenericParameters().Count != 0 ) {

                return false;
            }

            // COM registration and activation requires the type to provide a public parameterless constructor

            bool hasPublicParameterlessConstructor = false;

            foreach ( MethodDefinitionHandle methodDefinitionHandle in typeDefinition.GetMethods() ) {

                MethodDefinition methodDefinition = reader.GetMethodDefinition(methodDefinitionHandle);
                string methodName = reader.GetString(methodDefinition.Name);

                if ( methodName != ".ctor" ) {
                    continue;
                }

                if ( (methodDefinition.Attributes & MethodAttributes.Static) == MethodAttributes.Static ) {
                    continue;
                }

                if ( (methodDefinition.Attributes & MethodAttributes.Public) != MethodAttributes.Public ) {
                    continue;
                }

                if ( methodDefinition.GetParameters().Count != 0 ) {
                    continue;
                }

                hasPublicParameterlessConstructor = true;
            }

            return hasPublicParameterlessConstructor;

        }

        /// <summary>
        /// Add Types to the internal map of supported attributes.
        /// </summary>
        /// <param name="attributeTypes">Types to be added.</param>
        void AddSupportedAttributes( IEnumerable<Type> attributeTypes ) {

            if ( attributeTypes == null ) {
                return;
            }

            foreach ( Type attributeType in attributeTypes ) {
                _supportedCustomAttributes.Add( attributeType.FullName, attributeType );
            }
        }
    }
}
