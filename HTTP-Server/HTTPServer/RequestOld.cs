using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod  //request method goz2 mn l request line goz2 mn l request
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

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
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()  // ha2sem l request string elly gayaly 
        {
            //throw new NotImplementedException();
            
            //TODO: parse the receivedRequest using the \r\n delimeter   
            char[] c = { '\r', '\n' };
            string[] arr = requestString.Split(c);
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (arr.Length < 3)
            {
                return false;
            }
            else
            {
                // Parse Request line
                if (ParseRequestLine(arr) == false)
                    return false;
                else
                {
                    // Validate blank line exists
                    if (!ValidateBlankLine(arr[arr.Length - 2]))
                    {
                        return false;
                    }
                    // Load header lines into HeaderLines dictionary
                    else
                    {if(LoadHeaderLines(arr[1]))
                    
                        return true;

                        else 
                        return false;
                    
                    }
                }

            }
            
            

        }

        private bool ParseRequestLine(string []arr1)
        {
            //throw new NotImplementedException();
            string[] arr2 = arr1[0].Split(' ');
            method = (RequestMethod)Enum.Parse(typeof(RequestMethod), arr2[0]);
            relativeURI = arr2[1];
            if (arr2[2] == "HTTP/1.1")
            {
                arr2[2] = "HTTP11"; 
            }
            if (arr2[2] == "HTTP/1.0")
            {
                arr2[2] = "HTTP10";
            }
            if (arr2[2] == "HTTP/0.9")
            {
                arr2[2] = "HTTP09";
            }
            httpVersion = (HTTPVersion)Enum.Parse(typeof(HTTPVersion), arr2[2]);
            if (ValidateIsURI(relativeURI))
            {
                return true;
            }
            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines(string header)
        {
          //  throw new NotImplementedException();
            char[] c = { '\r', '\n' };
            string[] arr = header.Split(c);
            for (int i = 0; i < arr.Length-2; i++)
            {
                string[] s = arr[i].Split(':');
                headerLines.Add(s[0], s[1]);    //s[0] = host ,s[1]= header
   
            }
            return true;
        }

        private bool ValidateBlankLine(string blank)
        {
            //throw new NotImplementedException();
            if (blank == "")
            {
                return true;
            }
            else
                return false;
        }

    }
}
