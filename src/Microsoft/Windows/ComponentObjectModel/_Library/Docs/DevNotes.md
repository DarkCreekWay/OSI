# ComponentObjectModel - DevNotes

## Taken from ComponentRegistrationManager

* *TODO: Check this attribute doc for more information and importance/relevance for Runtime Information, App Info, etc. [System.Runtime.Versioning.TargetFrameworkAttribute( ".NETFramework,Version=v4.7.2", FrameworkDisplayName = ".NET Framework 4.7.2" )]}
* *TODO: Add Unregister Method for removing a version-dependent subkey.
* *TODO: Add management of ProgId in Register and Unregister Methods.

### Registry Layout

* Registry Base Key(s)
  * System Scope : HKEY_LOCAL_MACHINE\Software\Classes
  * User Scope   : HKEY_CURRENT_USER\Software\Classes

CLSID
+ {CLSID}
|
|--+ InprocServer32
   |- (Default) - Reg_SZ - mscoree.dll || c:\windows\system32\mscoree.dll
   |- ThreadingModel - Reg_SZ - ?
   |- Assembly - Reg_SZ - - Full qualified assembly name
   |- Class - Reg_SZ - Full qualified type name
   |- Runtime Version - Reg_SZ - assembly.ImageRuntimeVersion
   |- Codebase - Reg_SZ - assembly.Codebase - Only, if not in GAC -> This should become the default for portable deployment models // user scoped deployments
   |
   |- DefaultIcon (Optional ?) -> Not sure about this value yet
   |
   |--+ {Version} -> Refer to MS Doc about version-dependent registry keys

### .NET Assembly and Type related features

- [X] Inspect a given .NET type against COM component requirements and return a ComponentInformation instance, which can be used for registration.
- [ ] Enlist all .NET types implementing a COM component and its details within an assembly.

### COM component feature areas
 - [X] Supports registration scopes (System + Current User)
 - [X] Add registration of .NET type as COM component
 - [X] Read registration of a COM component
 - [X] Update registration of .NET type as COM component -> Covered by Register()
 - [X] Remove registration of .NET type
 - [ ] Inspect registration of a COM component (Scope + common details + versioned details) (Whats the difference to "Read" ?)


