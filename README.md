# HTTP_Server_Project
Http protocol is used between a client and a server. What a client sends is what a server receives, and vice-versa. Http was designed for the server to simply sit and wait for requests (possibly including data), and then respond (possibly including data).
Implement part of the HTTP protocol.
  1)Threaded (multiple clients)
  2)GET And Head Methods.
  3)Error handling
    a)Page Not found
    b)Bad Request
    c)Redirection
    d)Internal Server Error
it's Accept multiple clients by starting a thread for each accepted connectionand Keep on accepting requests from the remote client until the client closes the socket (sends a zero length message)and For each received request, the server must reply with a response.



