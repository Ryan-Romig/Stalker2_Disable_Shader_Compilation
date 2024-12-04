# Stalker2_Disable_Shader_Compilation
GUI created using C# with WPF to toggle shader compilation in the S.T.A.L.K.E.R 2 - Heart of Chornobyl video game


Application will detect if you installed the game with Steam or with Xbox game pass and write

`[SystemSettings]`<br>
`r.PSOWarmup.WarmupMaterials=0`

to a file to the `%LOCALAPPDATA%\Stalker2\Saved\Config\Windows` directory with the filename `Engine.ini`.



this will disable the shader compilation when you start the game. 
changing the 0 to a 1 or deleting the Engine.ini file will re-enable the shader compilation. 

Use this application to quickly toggle shader compilation on or off

It is recommended to re-enable shader compilation after a graphics driver update or game patch. 
![Screenshot 2024-12-03 190358](https://github.com/user-attachments/assets/f457de64-4881-4775-adec-b6a0671c1bdb)

