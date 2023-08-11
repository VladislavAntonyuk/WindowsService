SC CREATE "WindowssService" start= auto binpath= "%~d0%~p0\WindowsService.exe"
SC config "WindowsService" obj= "DOMAIN\USERNAME" password= "PASSWORD" type= own
SC START "WindowsService"