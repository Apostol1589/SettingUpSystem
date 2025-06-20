Copy-Item -Path "D:\(01) Soft_For_Shop\Podsobka\bsClr" -Destination "C:\Windows\BsClr" -Recurse -Force

Register-ScheduledTask -Xml (Get-Content -Path "D:\(01) Soft_For_Shop\Podsobka\BS_upd.xml" -Raw) -TaskName "BS_upd" -Force

Register-ScheduledTask -Xml (Get-Content -Path "D:\(01) Soft_For_Shop\Podsobka\BS_temp_clr.xml" -Raw) -TaskName "BS_temp_clr" -Force

Register-ScheduledTask -Xml (Get-Content -Path "D:\(01) Soft_For_Shop\Podsobka\bs_DownloadUpload.xml" -Raw) -TaskName "bs_DownloadUpload" -Force
