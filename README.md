# DotNET-MVC-Enumerator

##About

This tool is used to enumerate .NET MVC controller end points and also list out any filter attributes assigned to them. The tool provides the ability to search endpoints that contain a specific attribute or alternatively missing an attribute. The output will be saved as a CSV file which can be used to assist your manual code review of the application or perform further analysis. 

For additional details on the tool refer to our blog post: 

<add link here>


###Supported Platforms:

 .NET Framework 4.5 

###Installation

Fetch the .sln file and you should be good to go. Build it in Visual Studio supporting .NET version 4.5+.
All the dependencies are in the packages/ directory. 

Note: If Visual Studio complains about a missing app.config file, delete it from the solution explorer and the build should work.

###Usage

The basic usage of the script is as follows:

    > DotNotMVCEnumerator.exe -h

    Usage: DotNetMVCEnumerator.exe

    [-d Directory To Scan  *Required]

    [-a Attribute Name to Search]

    [-n Attribute Name To Perform Negative Search]

    [-o File Name to Output Results as CSV]

    [-h Display Usage Help Text]
    
The tool runs against a given directory and identifies:

    All classes that inherit from a Controller class
    All public methods within Controller class
    Attributes assigned to a method including the ones set at the class level

Sample Usage 1 - Scan code within a directory and write output to the ‘results.csv’ file.

    > DotNetMVCEnumerator.exe -d “C:\Code” -o results.csv

Sample Usage 2 - Scan code and only include results of methods containing the ‘CSRFTokenValidate’ attribute filter. The output is written to the console and to the ‘results.csv’ file.

    > DotNetMVCEnumerator.exe -d “C:\Code” -o results.csv -a CSRFTokenValidate



