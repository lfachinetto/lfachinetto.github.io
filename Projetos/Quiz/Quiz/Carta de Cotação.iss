; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{CB580E69-8B2E-42DF-8D1C-DFEB16E9F19C}
AppName=Carta de Cota��o
AppVersion=7.0
AppVerName=Carta de Cota��o  
AppPublisher=Lucas Mezzomo Fachinetto
DefaultDirName={pf}\Carta de Cota��o
DefaultGroupName=Carta de Cota��o
DisableProgramGroupPage=yes
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Files]
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\Carta de Cotacao.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\Carta de Cotacao.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\itextsharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\AsyncBridge.Net35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\System.Threading.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Documentos\Visual Studio 2017\Projects\Carta de Cotacao\Carta de Cotacao\bin\Release\Extras.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\Carta de Cota��o"; Filename: "{app}\Carta de Cotacao.exe"; IconFilename: "{app}\Carta de Cotacao.exe"; IconIndex: 0
Name: "{commondesktop}\Carta de Cota��o"; Filename: "{app}\Carta de Cotacao.exe"; IconFilename: "{app}\Carta de Cotacao.exe"; IconIndex: 0; Tasks: desktopicon

[Run]
Filename: "{app}\Carta de Cotacao.exe"; Description: "{cm:LaunchProgram,Carta de Cota��o}"; Flags: nowait postinstall skipifsilent

