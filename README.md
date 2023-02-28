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

* Change username in code
* Change password either in file, or in code
* Run the program in your IDE
* If you get an error its most likely due to username or password mismatch

<h1> Notes </h1>

* The password can be entered manually or read from a file named "HiddenPassword.csv" located in the same directory as the executable file.
* The program reads data from an API endpoint located at https://api.myngp.com/v2/broadcastEmails.
* The program requires a username to access the API endpoint. The username is hard-coded into the program as apiuser.

<h1> Output </h1>

<u>The program outputs the following data for each email:</u>

* Email Message ID
* Email Name
* Recipients
* Opens
* Clicks
* Unsubscribes
* Bounces
* Top Variant

The output file is overwritten each time the program is run.
If the output file cannot be found, the program will display an error message and exit.
The program outputs a message indicating the location of the output file when it has completed successfully.


------------------------------------------------------------------------------------------------------------------------------------------------------------------------

<h1> Follow-up Questions </h1>

* <u>How long, roughly, did you spend working on this project? This won’t affect your evaluation; it helps us norm our problems to our expected time to complete them.<.u>
  * I spent roughly 6 hours in total on the project, ~1 hour researching the API, ~1 hour planning on how I would write it, ~3 hours writing the code, ~1 hour with       final touches on GitHub and questions. The coding section took longer than I had hoped, as I had realized a better and more clear way to do it about an hour in.

* <u>Give the steps needed to deploy and run your code, written so that someone else can follow and execute them.</u>
  * The steps are above to run the code.

* <u>What could you do to improve your code for this project if you had more time? Could you make it more efficient, easier to read, more maintainable? If it were your job to run this report monthly using your code, could it be made easier or more flexible? Give specifics where possible.</u>
  * Some instant things I noticed while writing the code were some fail safe checks, making sure no errors occurred during runtime and such. I believe I could make the code more efficient by saving some memory, however the memory it uses is not much. I believe this code is expandable and upgradable, I included comments to understand how the program works and hopefully the program would be easy to upkeep and maintain. One area where I believe could be better managed is getting the data from the API. I believe I made it a little bit more complicated than it needed to be.
  
* <u>Outline a testing plan for this report, imagining that you are handing it off to someone else to test. What did you do to test this as you developed it? What kinds of automated testing could be helpful here?</u>
  * Some automated tests that would significantly help would be for the email data. I would set up certain parameters and see what would happen if the API returned too much data. Some tests I did as I developed it were making sure no runtime errors could occur. My goal was the make the code as simple as possible, but efficient as possible. A test that I believe is not in the code, but should be tested is allowing the user to know if the AUTH was correct. 
 
 ------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  
- Overall I am glad with this program, I wrote it in C# as that is what I am most familiar with and Bonterra works in C#. This project was very fun to work on and I enjoy the challenge presented to me.
