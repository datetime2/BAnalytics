@echo YB.Mall.AutoTask 服务开始安装
 C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %cd%\BAnalytics.MessageHandling.WindowsService.exe
 net start BAnalytics.MessageHandling.WindowsService
pause