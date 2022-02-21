using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
     
         public Response(StatusCode code, string contentType, string content, string redirectoinPath , Request r)
        {
          //  throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            string statusLine = GetStatusLine(code);
            string fullContentType = "Content-Type: " + contentType + "\r\n";
            string date = "Date: " + DateTime.Now + "\r\n";
            string contentLength = "Content-Length: " + content.Length + "\r\n";

            responseString = statusLine;
            responseString += fullContentType;
            responseString += contentLength;
            responseString += date;

            // TODO: Create the request string
            if (code == StatusCode.Redirect)
            {

                string location = "Location: " + redirectoinPath + "\r\n";
                responseString += location;
            }

            responseString += "\r\n";
            if(r.M != "HEAD")
            responseString += content;
            //Console.WriteLine("RES: " + responseString);
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
           // int num = ((int)code).ToString();
            //Console.WriteLine("num: " + num);
            string statusLine = "HTTP/1.1 " + ((int)code).ToString() + " " + code.ToString() + "\r\n";
            return statusLine;
        }
    }
}
