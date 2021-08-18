# Shell Handlers - DevNotes

A Shell Extension Handler is always associated to one or more File Classes or File Extensions (ProgIDs)

In order to register the extension handler, some registry subkeys needs to be created underneath a specific progID, 
for associating the progid with the extension handler.

The keys have to point to the CLSID of a COM Server containing the shell extension handler.
Different Shell handler extension types are written to different subkeys

## Registy layout example for a shortcut menu handler

Registry layout for the association of a shortcut menu Handler with the "All Files" class.

+ HKEY_CLASSES_ROOT\Classes
|
|-+ * ; All Files Class
  |
  |-+ ShellEx ; Shell Extensions live here
    |
    |-+ ContextMenuHandlers ; Type of Shell Extension, here ContextMenuHandlers
      |
      |-+ <HandlerName>; Name of the Handler || CLSID
        |
        + (DEFAULT) - [REG_SZ] - <CLSID> ; CLSID of COM Server implementing the Handler logic.

## Online resources

* [Handler Types, Handler Names, Registry SubKey Names - Microsoft Docs](https://docs.microsoft.com/en-us/windows/win32/shell/reg-shell-exts)
* [Context Menu Handlers - Microsoft Docs](https://docs.microsoft.com/en-us/windows/win32/shell/context-menu-handlers)

## Tasks from Source

### ShellExtensionHandlerType.cs

* TODO: Inspect online resources for additional interesting stuff
  * [Shell Preview Handler](https://docs.microsoft.com/en-us/windows/win32/shell/how-to-register-a-preview-handler)
  * <https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ipreviewhandler>