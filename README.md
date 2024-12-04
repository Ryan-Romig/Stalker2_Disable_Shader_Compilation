# Stalker2_Disable_Shader_Compilation
GUI created using C# with WPF to toggle shader compilation in the S.T.A.L.K.E.R 2 - Heart of Chornobyl video game


Application will detect if you installed the game with Steam or with Xbox game pass and write a Engine.ini file to the `%LOCALAPPDATA%\AppData\Local\Stalker2\Saved\Config\Windows` directory with the text 
[SystemSettings]
r.PSOWarmup.WarmupMaterials=0

this will disable the shader compilation when you start the game. 
changing the 0 to a 1 or deleting the Engine.ini file will re-enable the shader compilation. 

Use this application to quickly toggle shader compilation on or off

It is recommended to re-enable shader compilation after a graphics driver update or game patch. 
