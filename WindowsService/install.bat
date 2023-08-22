SC CREATE "WindowsService" start= auto binpath= "%~d0%~p0\WindowsService.exe" DisplayName= "VladislavAntonyuk.WindowsService"
SC START "WindowsService"
pause