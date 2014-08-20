src\packages\OpenCover.4.5.2506\OpenCover.Console.exe -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -targetargs:src\SleepingFish.Test\bin\Debug\SleepingFish.Test.dll -output:report.xml
rmdir TestResults
src\packages\ReportGenerator.1.9.1.0\ReportGenerator.exe -reports:report.xml -targetdir:Coverage
del report.xml