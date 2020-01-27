New-Item -ItemType Directory -Force -Path ..\Publish

$appPath = (get-item $pwd).parent.parent.FullName
$fileName = (Split-Path -Leaf -Path $appPath) + ".zip"
$fullName = $pwd.Path + "\..\Publish\" + $fileName

Compress-Archive *.dll -Force $fullName
Compress-Archive *.exe -Update $fullName