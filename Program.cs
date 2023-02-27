// This program writes to a file the specific data for emails
// Refer to the ReadMe for context of how to use and what is needed
// Author: Aaron Arseneau
// Date: February 26, 2023

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using RestSharp;
using RestSharp.Authenticators;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class EmailReportGenerator
{
    static void Main(string[] args)
    {
        const string username = "apiuser"; // <--- Put username here
        string password = ""; // <--- Put password here
        
        // We will get this password from a file, because I will be uploading this on GitHub.
        // However if you wish to just copy the password and paste it in the string "password", that will work as well.

        if (File.Exists("HiddenPassword.csv") && password == "") // If we are using the file for the password
        {
            using (StreamReader reader = new StreamReader("HiddenPassword.csv")) // Will auto close the file
            {
                password = reader.ReadLine();
            }
        }

        // Define the path and name of the output file
        string outputFile = "EmailReport.csv";
        if (!File.Exists(outputFile)) // If file doesnt exist, end program
        {
            Console.WriteLine("Error, could not find file. Please Read Documentation");
            return;
        }

        StreamWriter writer = new StreamWriter(outputFile);


        // Write the header row to the output file
        writer.WriteLine("Email Message ID, Email Name, Recipients, Opens, Clicks, Unsubscribes, Bounces, Top Variant");

        string resourcePath = "https://api.myngp.com/v2/broadcastEmails"; // API location


        // Step 1, Get all emails
        //------------------------------------------------------------------------------------------------------------------------


        List<Email> emails = new List<Email>(); // All of our Emails

        // Create a RestClient object and Authenticate it
        var client = new RestClient(resourcePath);
        client.Authenticator = new HttpBasicAuthenticator(username, password);

        var request = new RestRequest("", Method.Get);

        // Execute the request and get the response
        var response = client.Execute(request);

        // Deserialize the JSON response into a C# object
        var jsonArray = JObject.Parse(response.Content)["items"];

        foreach (var item in jsonArray) // Get Each emailID and Name and add it to our List of emails
        {
            Email email = new Email
            {
                EmailMessageId = item["emailMessageId"].Value<int>(),
                Name = item["name"].Value<string>()
            };
            emails.Add(email);
        }


        // Step2, Manipulate the Data and Store it for later
        //------------------------------------------------------------------------------------------------------------------------


        // Constantly changing variables
        var client2 = new RestClient();
        var request2 = new RestRequest();
        dynamic response2;

        foreach (var email in emails) // For each email we have
        {
            resourcePath += "/" + email.EmailMessageId; // Change resource path to get the Variants

            client2 = new RestClient(resourcePath + "?$expand=statistics");
            client2.Authenticator = new HttpBasicAuthenticator(username, password);
            request2 = new RestRequest("", Method.Get);
            response2 = JObject.Parse(client2.Execute(request2).Content); // Response 2 will hold our Data

            // Store all the new data in the Email
            email.Statistics.Recipients = (int)response2["statistics"]["recipients"];
            email.Statistics.Opens = (int)response2["statistics"]["opens"];
            email.Statistics.Bounces = (int)response2["statistics"]["bounces"];
            email.Statistics.Clicks = (int)response2["statistics"]["clicks"];
            email.Statistics.Unsubscribes = (int)response2["statistics"]["unsubscribes"];

            resourcePath = Regex.Replace(resourcePath, @"(.*\/broadcastEmails).*", "$1"); // Reset ResourcePath for next use

            email.GetTopVariant(response2, resourcePath, username, password); // Gets and Sets the TopVariant in the email
        }


        // Step3, Output all Emails to Console
        //------------------------------------------------------------------------------------------------------------------------


        // mail Message ID, Email Name, Recipients, Opens, Clicks, Unsubscribes, Bounces, Top Variant
        foreach (var email in emails)
        {
            writer.WriteLine(
                email.EmailMessageId + ", " +
                email.Name + ", " +
                email.Statistics.Recipients + ", " +
                email.Statistics.Opens + ", " +
                email.Statistics.Clicks + ", " +
                email.Statistics.Unsubscribes + ", " +
                email.Statistics.Bounces + ", " +
                email.TopVariant
                );
        }

        writer.Close(); // Close the output file

        // Let user know where file is located and that the program succesfully completed.
        Console.WriteLine("Email report complete, file is {0}", outputFile);
    }
}


/// <summary>
/// Represents a Email Object
/// Email can get its TopVariant
/// </summary>
class Email
{
    // Emails main ID
    [JsonProperty("emailMessageId")]
    public int EmailMessageId { get; set; }

    // Emails main Name
    [JsonProperty("name")]
    public string Name { get; set; }

    // Emails TopVariant, Starts as "", updated with GetTopVariant()
    public string TopVariant { get; set; } = "";

    // Emails Variants
    [JsonProperty("variants")]
    public List<EmailMessageVariant> Variants { get; set; } = new List<EmailMessageVariant>();

    // Emails Statistics
    [JsonProperty("statistics")]
    public EmailMessageStatistics Statistics { get; set; } = new EmailMessageStatistics();

    /// <summary>
    /// Sets the TopVariant for the Email
    /// TopVariant is the one with the highest amount of Opens
    /// </summary>
    /// <param name="response2">All of the Email Variants received from the API</param>
    /// <param name="resourcePath">Location of API HTTP</param>
    /// <param name="username">Username for Auth of API</param>
    /// <param name="password">Password for Auth of API</param>
    public void GetTopVariant(dynamic response2, string resourcePath, string username, string password)
    {
        foreach (var variant in response2["variants"]) // Creates new Variant to store in List
        {
            EmailMessageVariant emailVar = new EmailMessageVariant
            {
                EmailMessageVariantId = (int)variant["emailMessageVariantId"],
                Name = (string)variant["name"],
                Subject = (string)variant["subject"]
            };
            this.Variants.Add(emailVar);
        }

        // Used to keep track of which Variant is the current TopVariant
        int maxOpens = 0;
        int maxID = 0;

        foreach (EmailMessageVariant item2 in this.Variants)
        {
            var client = new RestClient(resourcePath + "/" + this.EmailMessageId + "/" + "variants" + "/" + item2.EmailMessageVariantId + "?$expand=statistics");
            client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("", Method.Get);
            
            var response = client.Execute(request).Content;
            var emailData = JObject.Parse(response)["statistics"];

            if ((int)emailData["opens"] > maxOpens) // If new Email Variant open count > Previous, update
            {
                maxOpens = (int)emailData["opens"];
                maxID = item2.EmailMessageVariantId;
            }
        }
        this.TopVariant = Variants.FirstOrDefault(v => v.EmailMessageVariantId == maxID).Name; // Set TopVariant
    }
}

/// <summary>
/// Represents the EmailMessageVariant
/// Used in Emails class
/// </summary>
class EmailMessageVariant
{
    // Variants specific ID
    [JsonProperty("emailMessageVariantId")]
    public int EmailMessageVariantId { get; set; }

    // Variants specific Name
    [JsonProperty("name")]
    public string Name { get; set; }

    // Variants specific Subject
    [JsonProperty("subject")]
    public string Subject { get; set; }

}

/// <summary>
/// Represents EmailMessageStatistics object
/// Used in Email class
/// </summary>
class EmailMessageStatistics
{
    // Total Email Recipients
    [JsonProperty("recipients")]
    public int Recipients { get; set; }

    // Total Email Opens
    [JsonProperty("opens")]
    public int Opens { get; set; }

    // Total Email Clicks
    [JsonProperty("clicks")]
    public int Clicks { get; set; }

    // Total Email Unsubscribes
    [JsonProperty("unsubscribes")]
    public int Unsubscribes { get; set; }

    // Total Email Bounces
    [JsonProperty("bounces")]
    public int Bounces { get; set; }
}