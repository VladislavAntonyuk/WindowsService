SC CREATE "ScreenBrightnessService" start= auto binpath= "%~d0%~p0\ScreenBrightnessService.exe"
SC config "ScreenBrightnessService" obj= "DOMAIN\USERNAME" password= "PASSWORD" type= own
SC START "ScreenBrightnessService"