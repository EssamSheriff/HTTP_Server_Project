using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10, // HTTP/1.0
        HTTP11,// HTTP/1.1
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        public string M;
        Dictionary<string, string> headerLines = new Dictionary<string, string>();
        string[] req_line;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }


        public bool ParseRequest()

        {
            //TODO: parse the receivedRequest using the \r\n delimeter
            requestLines = requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            /*
           // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
                  Console.WriteLine("*********************");
                  foreach (string x in requestLines) { 
                   Console.WriteLine(x);
                  }
                  Console.WriteLine("*********************");
            */
            //Console.WriteLine("111111");
            if (requestLines.Length < 3 )
            {
  
                return false;
                // Parse Request line
            }
           //Console.WriteLine("2222");

            if (!ParseRequestLine())
                return false;
            // Validate blank line exists
            //Console.WriteLine("3333333");

            if (!ValidateBlankLine())
            {
                return false;
            }
            // Load header lines into HeaderLines dictionary
            //Console.WriteLine("4444444444");

            if (!LoadHeaderLines())
                return false;

            //Console.WriteLine("5555555555555");
            return true;
           // throw new NotImplementedException();
        }

        private bool ParseRequestLine()

        {
            string[] tokens = requestLines[0].Split(' ');
  /*          Console.WriteLine("+++++++++++++++++++");
            foreach (string t in tokens) {
                Console.WriteLine(t);
            }
            Console.WriteLine("+++++++++++++++++++");
*/
            if ( tokens[0] != "GET" && tokens[0] != "HEAD" && tokens[0] != "POST")
            {
                //if((enum)tokens[0] == RequestMethod.GET)
                return false;
            }
            //Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            // method = RequestMethod.GET;
            M = tokens[0];
            relativeURI = tokens[1];
            if (!ValidateIsURI(relativeURI))
                return false;

      //      Console.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBB");
            //string[] ht = tokens[2].Split('/'); // http
            //string[] ver = ht[1].Split('.'); // 1    1
            //string http_ver = ht[0] + ver[0] + ver[1];  // http11
          //  Console.WriteLine("VER  "+http_ver);
            if (tokens[2] != "HTTP/1.1" && tokens[2] != "HTTP/0.9" && tokens[2] != "HTTP/1.0")
            {
                return false;
            }
            //   
          //  httpVersion = HTTPVersion.HTTP11;
            return true; 
//            throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
          //  Console.WriteLine("VER  " + uri);
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
     //       Console.WriteLine("AAAAAAAA");
     /*       for (int i = 1; i < requestLines.Length - 2; i++)
            {
                if (requestLines[i] == "")
                {
                    Console.WriteLine("XXXXXXXXX");
                    return false;
                }
            }*/

     //       Console.WriteLine("CCCCCCCCCCCCCCCCCCCCCCCCCC");
            for (int i = 1; i < requestLines.Length - 2; i++)
            {
                string[] parts = requestLines[i].Split(new string[] { ": " }, StringSplitOptions.None);
                headerLines.Add(parts[0], parts[1]);
            }
         /*   Console.WriteLine("--------------------------------");
            foreach (KeyValuePair<string, string> kvp in headerLines)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }
            Console.WriteLine("--------------------------------");*/

            return true;

        //    throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            string [] RL = requestString.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
            if (RL.Length < 2) {
           //     Console.WriteLine("--------------------------------  " + requestString);
                return false;
            }


            return true;

            //    throw new NotImplementedException();
        }

    }
}
