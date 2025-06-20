Set-TimeZone -Id 'FLE Standard Time'

Set-Service -Name w32time -StartupType Automatic
Start-Service w32time

Set-ItemProperty -Path 'HKLM:\SYSTEM\CurrentControlSet\Services\w32time\Parameters' -Name 'Type' -Value 'NTP'

w32tm /config /manualpeerlist:'time.windows.com' /syncfromflags:manual /reliable:yes /update

Restart-Service w32time