# DotNET-MVC-Enumerator

##About

This script can be used to enumerate all the endpoints in an .NET MVC application and also list out the HTTP methods suported by them, if any. Additionally, if any security directives are supported by the application, we can search them by passing it as an argument. They will be listed out if they are set at either the class level or the method level itself. 
And yeah, the ouput can be saved in a CSV format if specified.


###Supported Platforms:

 .NET Framework 4.5 

###Installation

Fetch the .sln file and you should be good to go. Build it in Visual Studio supporting .NET version 4.5+.
All the dependencies are in the packages/ directory. 

###Usage

The basic usage of the script is as follows:

Path\to\CodeBase\> DotNetMVCEnumerator.exe [-a attribute] [-o output file for csv]

It recursively searches for all .cs files from the current directory and attempts to list out the Entry points.If we specify an attribute, it lists out its presence or absence in each method as well.The output by default is presented on the console. However, the results can be obtained in csv format if we specify the name of the output file using the -o flag.

