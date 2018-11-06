# Required tools
1. Sign up for free Azure account and get Api key: https://azure.microsoft.com/en-us/services/cognitive-services/face/
2. Download Visual Studio Community edition and install Workflow Foundation from options. https://visualstudio.microsoft.com/downloads/

## Platform
1. Windows only
2. Requires .net framework 4.7.2
3. Requires Workflow Foundation runtimes, read more at https://docs.microsoft.com/en-us/dotnet/framework/windows-workflow-foundation/whats-new-in-wf-in-dotnet

## Before compiling and running
Please note that you will need to open up main workflow (Workflow1.xaml) and change values assigned to ApiKey, ApiUrl and ImageFolderPath variables to match personal settings, without valid values demo will cause exception and fail.

# Application context
This application will map out set folder, find all files with .jpg extension and submit them to Azure Face API, download detection result in Json and save it to the set folder with same name as the image file (except with .json extension instead of .jpg).
## Workflow1
![Workflow1](https://github.com/malrock/tyloeng051118/blob/master/workflow/Workflow1.jpg)
## Image processing loop
![process images loop](https://github.com/malrock/tyloeng051118/blob/master/workflow/Process%20each%20image.jpg)
