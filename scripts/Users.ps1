Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
secedit /export /cfg C:\SecurityPolicy.cfg
(gc C:\SecurityPolicy.cfg) -replace 'PasswordComplexity = 1', 'PasswordComplexity = 0' | Out-File C:\SecurityPolicy.cfg
secedit /configure /db secedit.sdb /cfg C:\SecurityPolicy.cfg /areas SECURITYPOLICY
Remove-Item C:\SecurityPolicy.cfg

New-LocalGroup -Name "bsbackup" -Description "Backup Group"

New-LocalUser -Name "Kasa1" -Password ("Kasa1" | ConvertTo-SecureString -AsPlainText -Force) -PasswordNeverExpires -UserMayNotChangePassword
Add-LocalGroupMember -Group "bsbackup" -Member "Kasa1"
Add-LocalGroupMember -Group "Users" -Member "Kasa1"
Add-LocalGroupMember -Group "Remote Desktop Users" -Member "Kasa1"

New-LocalUser -Name "Kasa2" -Password ("Kasa2" | ConvertTo-SecureString -AsPlainText -Force) -PasswordNeverExpires -UserMayNotChangePassword
Add-LocalGroupMember -Group "bsbackup" -Member "Kasa2"
Add-LocalGroupMember -Group "Users" -Member "Kasa2"
Add-LocalGroupMember -Group "Remote Desktop Users" -Member "Kasa2"

New-LocalUser -Name "Podsobka" -Password ("Podsobka" | ConvertTo-SecureString -AsPlainText -Force) -PasswordNeverExpires -UserMayNotChangePassword
Add-LocalGroupMember -Group "bsbackup" -Member "Podsobka"
Add-LocalGroupMember -Group "Users" -Member "Podsobka"
Add-LocalGroupMember -Group "Remote Desktop Users" -Member "Podsobka"
