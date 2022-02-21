using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static string reDir_Rules;
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 

            CreateRedirectionRulesFile();
            //Start server
            // 1) Make server object on port 1000
            Server s1 = new Server(1000, Program.reDir_Rules);
            // 2) Start Server
            s1.StartServer();
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            Program.reDir_Rules = System.IO.File.ReadAllText(@"H:\study\Level 3\term 5\Computer Networks\project\Template[2021-2022]\HTTPServer\redirectionRules.txt");
       /*     string[] lines = text.Split(',');
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
*/
        }

    }
}
