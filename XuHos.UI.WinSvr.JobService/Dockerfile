From microsoft/dotnet-framework:4.7.1-windowsservercore-1709
SHELL ["powershell"]

WORKDIR c:/app
ARG source=.
ARG type=mq
ARG name=mqservice

ENV type=$type
ENV name=$name

ENV db:server=.
Env db:name=KMEHosp
ENV db:uid=sa
ENV db:pwd=123456

COPY $source c:/app

CMD ["XuHos.UI.WinSvr.JobService.exe -c -t $type -n $name & ping -t 127.0.0.1"]
