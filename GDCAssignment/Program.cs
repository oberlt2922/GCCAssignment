using System;
using System.IO;
using System.Text.RegularExpressions;

namespace GDCAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            GetUserInput();
        }

        public static void GetUserInput()
        {
            while(true)
            {
                //prompt for and reads input
                Console.WriteLine("Please enter the name of a csv filename, or enter -999 to exit.");
                string? fileName = Console.ReadLine();
                
                //re prompts if no input was entered
                if(fileName == null || fileName == "")
                {
                    continue;
                }
                
                //checks if input ends with .csv and removes it
                if(fileName.EndsWith(".csv"))
                {
                    fileName = fileName.Substring(0, fileName.Length - 4);
                }

                //quits the application
                if(fileName == "-999")
                {
                    break;
                }

                //Gets the file content
                GetFileContent(fileName);
                break;//removing this break will cause applicaiton to rerun until user manually quits by entering -999
            }
        }

        public static void GetFileContent(string fileName)
        {
            try//attempts to get the content of the file then calls validate method
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string[] lines = System.IO.File.ReadAllLines($"{currentDirectory}\\{fileName}.csv");
                ValidateEmails(lines);
            }
            catch(FileNotFoundException)//if file does not exist, displays error message and re prompts for input
            {
                Console.WriteLine($"\nNo csv file named \"{fileName}\" does not exist");
                GetUserInput();
            }
        }

        public static void ValidateEmails(string[] lines)
        {
            List<string> validEmails = new List<string>();
            List<string> invalidEmails = new List<string>();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            foreach(string line in lines)
            {
                try
                {
                    //if no email value, IndexOutOfRangeException will be thrown, or third elem will be ""
                    string email = line.Split(',')[2];
                    if(email == "")
                    {
                        continue;
                    }

                    //compared email to regex and places email in appropriate list
                    Match match = regex.Match(email);
                    if(match.Success)
                    {
                        validEmails.Add(email);
                    }
                    else
                    {
                        invalidEmails.Add(email);
                    }
                    
                    OutputLists(validEmails, invalidEmails);//outputs the lists of emails
                }
                catch(IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }

        //outputs the lists of emails
        public static void OutputLists(List<string> validEmails, List<string> invalidEmails)
        {
            Console.WriteLine("\nValid Emails:");
            foreach(string email in validEmails)
            {
                Console.WriteLine(email);
            }

            Console.WriteLine("\nInvalid Emails:");
            foreach(string email in invalidEmails)
            {
                Console.WriteLine(email);
            }

            Console.WriteLine();
        }
    }
}