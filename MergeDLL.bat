@echo off
echo.
echo Memorization Merge DLL
echo.
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /wildcards /out:Memorization\bin\Debug\Memorization_DLLMerged.exe Memorization\bin\Debug\Memorization.exe Memorization\bin\Debug\*.dll /targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319"
pause