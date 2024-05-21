
### Component names   

**crow** => Code Execution Engine, .NET 6 long running background service   
**lynx** => Platform, .NET 6 API   
**ibex** => Task Manager, .NET 6 API   
**dolphin** => Platform frontend, Reactjs and JQuery   
**shark** =>  Task Manager frontend, Reactjs


The Code Execution Engine was developed for Ubuntu 22.04, and some of the file locations are hard coded, hence, will not work out of the box.
Change the file locations according to your distribution.
It has one dependency which can be found at (https://github.com/ioi/isolate)

In order to run tests, you need to have an instance of Selenium Grid running to direct tests to.

The SQL Server backup files are in the respective folder.
