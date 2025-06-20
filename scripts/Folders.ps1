New-Item -Path "D:\Base\RetailUkr" -ItemType Directory -Force | Out-Null
Set-ItemProperty -Path "D:\Base" -Name Attributes -Value 'Hidden'

New-Item -ItemType Directory -Path "D:\BackUpBase" -Force | Out-Null
New-SmbShare -Name "BackUpBase$" -Path "D:\BackUpBase" -FullAccess "Everyone" | Out-Null

New-Item -ItemType Directory -Path "D:\BackUpBaseK1" -Force | Out-Null
New-SmbShare -Name "BackUpBaseK1$" -Path "D:\BackUpBaseK1" -FullAccess "Everyone" | Out-Null

New-Item -ItemType Directory -Path "D:\BackUpBaseK2" -Force | Out-Null
New-SmbShare -Name "BackUpBaseK2$" -Path "D:\BackUpBaseK2" -FullAccess "Everyone" | Out-Null

$accessRuleFull = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "FullControl", "ContainerInherit,ObjectInherit", "None", "Allow")
$accessRuleModify = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")

$acl1 = Get-Acl "D:\BackUpBase"
$acl1.SetAccessRule($accessRuleFull)
$acl1.AddAccessRule($accessRuleModify)
Set-Acl "D:\BackUpBase" $acl1

$acl2 = Get-Acl "D:\BackUpBaseK1"
$acl2.SetAccessRule($accessRuleFull)
$acl2.AddAccessRule($accessRuleModify)
Set-Acl "D:\BackUpBaseK1" $acl2

$acl3 = Get-Acl "D:\BackUpBaseK2"
$acl3.SetAccessRule($accessRuleFull)
$acl3.AddAccessRule($accessRuleModify)
Set-Acl "D:\BackUpBaseK2" $acl3

$ruleKasa1 = New-Object System.Security.AccessControl.FileSystemAccessRule("Kasa1", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
$ruleKasa2 = New-Object System.Security.AccessControl.FileSystemAccessRule("Kasa2", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
$rulePodsobka = New-Object System.Security.AccessControl.FileSystemAccessRule("Podsobka", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")

$aclBase = Get-Acl "D:\Base"
$aclBase.SetAccessRule($ruleKasa1)
$aclBase.AddAccessRule($ruleKasa2)
$aclBase.AddAccessRule($rulePodsobka)
Set-Acl -Path "D:\Base" -AclObject $aclBase

$aclUni = Get-Acl "D:\uniscalesdriver"
$aclUni.SetAccessRule($ruleKasa1)
$aclUni.AddAccessRule($ruleKasa2)
$aclUni.AddAccessRule($rulePodsobka)
Set-Acl -Path "D:\uniscalesdriver" -AclObject $aclUni
