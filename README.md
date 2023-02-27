This program is a console application that generates an email report and writes it to a CSV file. The program retrieves data from an API endpoint, processes the data, and outputs the result to a file.

<h1> Requirements </h1>
* <u>.NET 3.1 or later</u>
* <u>RestSharp library 108.0.3 or later</u>
* <u>Newtonsoft.Json library 13.0.2</u>

* A file named EmailReport.csv in the same directory as the program (used to output the data)

<h2> How to Install </h2>
To install the RestSharp library, run the following command in the Package Manager Console:

- Install-Package RestSharp -Version X.X.X

To install the Newtonsoft.Json library, run the following command in the Package Manger Console:

- Install-Package Newtonsoft.Json -Version X.X.X


<h1> Usage </h1>
* Open the command prompt or terminal.
Navigate to the directory containing the executable file.
Run the command dotnet EmailReportGenerator.dll.
The program will prompt for a password, which should be entered.
The program will generate an email report and write it to a CSV file named "EmailReport.csv" in the same directory as the executable file.
Notes
The password can be entered manually or read from a file named "HiddenPassword.csv" located in the same directory as the executable file.
The program reads data from an API endpoint located at https://api.myngp.com/v2/broadcastEmails.
The program requires a username to access the API endpoint. The username is hard-coded into the program as apiuser.
The program outputs the following data for each email:
Email Message ID
Email Name
Recipients
Opens
Clicks
Unsubscribes
Bounces
Top Variant
The output file is overwritten each time the program is run.
If the output file cannot be found, the program will display an error message and exit.
The program outputs a message indicating the location of the output file when it has completed successfully.
