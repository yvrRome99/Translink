# Translink
This program is in C# using Playwright for .NET and is for evaluation purposes.

To run the application you will need the following:
- Clone this project in Visual Studio
- .NET desktop development workload in Visual Studio
- Configure runsettings
- Install Playwright browsers

Clone Project
- Open Visual Studio
- Select Clone a repository
- Enter the repo location "https://github.com/yvrRome99/Translink.git"
- Clone

.NET Desktop Development Workload
- Open Visual Studio Installer
- Select .NET desktop development in the Workload tab
- Install

Configure Runsettings
- Open Visual Studio
- Open Test Exploer by going to the toolbar and selecting Test > Test Explorer
- In Test Explorer, click on the down arrow beside Options > Configure Run Settings > Select Solution Wide runsettings File
- Navigate to the project folder and select the TLink.runsettings file

Install Playwright Browsers (make sure you have permission to run scripts in PowerShell)
- Open Visual Studio
- Open the terminal: View > Terminal
- Navigate to the project foldeer
- Type: dotnet add package Microsoft.Playwright
- Type: dotnet build
- Type: .\bin\Debug\net8.0\playwright.ps1 install

To run the test
- In Test Explorer, right click on the "SearchForSchedule" test, then select Run
- Once the test has completed, you can view a HTML report from Test Explorer by clicking the Results.html link
