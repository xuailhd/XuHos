#--------------------------------------------------------
# Auth£ºguoming/geniusming@qq.com
# Date£º2017Äê10ÔÂ17ÈÕ
#--------------------------------------------------------
FROM microsoft/iis:10.0.14393.206
MAINTAINER "geniusming <geniusming@qq.com>"
SHELL ["powershell"]

ARG source=.
EXPOSE 80
WORKDIR c:/inetpub/wwwroot/

RUN Install-WindowsFeature NET-Framework-45-ASPNET ; \
    Install-WindowsFeature Web-Asp-Net45

COPY $source c:/inetpub/wwwroot/

RUN Remove-WebSite -Name 'Default Web Site'
RUN New-Website -Name 'Default Web Site' -Port 80 \
    -PhysicalPath 'c:/inetpub/wwwroot/' -ApplicationPool '.NET v4.5'

CMD Write-Host IIS Started... ; \
    while ($true) { Start-Sleep -Seconds 3600 }