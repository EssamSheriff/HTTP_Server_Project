using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        private Socket serverSocket;
        private int port;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);

            //TODO: initialize this.serverSocket
            this.port = portNumber;
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, portNumber);

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(ipEnd);

        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);
           
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                    Socket S = serverSocket.Accept();
                   // Console.WriteLine("New client accepted: {0}", S.RemoteEndPoint);

                    Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                    thread.Start(S);
              
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket Sub_sockets = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Sub_sockets.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            string request_string = "Hello";
            int len;
            Byte[] data  ;
           // Console.WriteLine("handel Coo");//
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    data = new byte[1024 * 1024];
                        len = Sub_sockets.Receive(data);
                 //   Console.WriteLine("Data " + data );
                  //  Console.WriteLine("Len  " + len);
                    // TODO: break the while loop if receivedLen==0
                   if (len == 0)
                    {
                            Console.WriteLine("client closed");
                            break;
                    }

                       request_string = Encoding.ASCII.GetString(data, 0, len);

                    // TODO: Create a Request object using received request string
                   // request_string = "http://localhost:1000/aboutus2.html";
                 //   Console.WriteLine("Reques is : "+request_string);

                    Request r = new Request(request_string);

                    //  r.ParseRequest();

                    // TODO: Call HandleRequest Method that returns the response
                    Response response = HandleRequest(r);
                    // TODO: Send Response back to client
                    data = Encoding.ASCII.GetBytes(response.ResponseString);
                   // Console.WriteLine("Response send ");
                    //Console.WriteLine("Response IS:  "+ response.ResponseString);
                    Sub_sockets.Send(data);
                    // Sub_sockets.Send(response);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            Sub_sockets.Close();
        }

        Response HandleRequest(Request request)
        {

            string Response_content;
            try
            {
                //TODO: check for bad request 
                //  Console.WriteLine("Reques is"  );
             //   throw new Exception();

                if (!request.ParseRequest())
                {
                    Response_content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    Response response_badRequest = new Response(StatusCode.BadRequest, "text/html", Response_content, string.Empty,request);
                    return response_badRequest;
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.
               // string PH_Path = Configuration.RootPath + request.relativeURI;
             //   Console.WriteLine("out  " + PH_Path);
                //TODO: check for redirect  // 2 -> 
                string redirectionPath = GetRedirectionPagePathIFExist(request.relativeURI);
                //Console.WriteLine("Len  " + redirectionPath);
                if (redirectionPath.Length > 0)
                {
                //    PH_Path = Configuration.RootPath + "/" + redirectionPath;
                    //Console.WriteLine("IN IF  1");
                    //Console.WriteLine("IN  " +PH_Path);
                    Response_content = LoadDefaultPage(redirectionPath);
                    //Console.WriteLine("IN IF 2");
                    Response response_reDirect = new Response(StatusCode.Redirect, "text/html", Response_content, redirectionPath,request);
                    //Console.WriteLine("IN IF 3");
                    return response_reDirect;
                }

                //TODO: check file exists
                redirectionPath = LoadDefaultPage(request.relativeURI);
                ////Console.WriteLine("Len  " + redirectionPath);
                if (redirectionPath.Length == 0)
                {
                    Response_content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    Response response_not_found = new Response(StatusCode.NotFound, "text/html", Response_content, string.Empty,request);
                    return response_not_found;
                }

                //TODO: read the physical file
                //Console.WriteLine("IN  "+PH_Path);
                Response_content = LoadDefaultPage(request.relativeURI);

                // Create OK response
                Response ok = new Response(StatusCode.OK, "text/html", Response_content, string.Empty,request);

               // Console.WriteLine("Content:  "+Response_content);
                Console.WriteLine("OK");
                return ok;
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                Response_content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                Response response_Internal = new Response(StatusCode.InternalServerError, "text/html", Response_content, string.Empty,request);
                return response_Internal;
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath) //
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
          //  Console.WriteLine("relativePath  "  + relativePath ) ;
            string[] s = relativePath.Split('/');
            //foreach(string l in s)
            //{
            //    Console.WriteLine("S  " + l);
            //}
         
            if (Configuration.RedirectionRules.ContainsKey(s[1]))
            {
            //    Console.WriteLine("relativePath");
                return Configuration.RedirectionRules[s[1]];
            }
           // Console.WriteLine("Empty");
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            if(defaultPageName[0] == '/')
            {
                string[] s = defaultPageName.Split('/');
                defaultPageName = s[1];
            }
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            bool isExist = File.Exists(filePath);
            if (!isExist)
            {
                Logger.LogException(new Exception(defaultPageName + " not Exist"));
                return string.Empty;
            }
            // else read file and return its content
            return File.ReadAllText(filePath);
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                Configuration.RedirectionRules = new Dictionary<string, string>();
                string[] lines = filePath.Split('\n');
                foreach (string line in lines)
                {
                    string[] l = line.Split(',');
                //    Console.WriteLine(l[0]);
                 //   Console.WriteLine(l[1]);
                    Configuration. RedirectionRules.Add(l[0], l[1]);
                 //   Console.WriteLine("**********");

                }
                //Console.WriteLine(lines.Length);
           
                /*
                  foreach (KeyValuePair<string, string> kvp in Configuration.RedirectionRules)
                  {
                      //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                      Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                  }
                  */
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }

    }
}
