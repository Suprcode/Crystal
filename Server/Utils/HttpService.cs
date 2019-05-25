using System;
using System.IO;
using System.Net;
using System.Text;

namespace Server
{
    abstract class HttpService {
        protected string host;
        HttpListener listener;
        bool is_active = true;

        public HttpService() {
        }

        public void Listen() {
            if (!HttpListener.IsSupported) {
                throw new System.InvalidOperationException(
                    "To use HttpListener the operating system must be Windows XP SP2 or Server 2003 or higher.");
            }
            string[] prefixes = new string[] { host };

            listener = new HttpListener();
            try
            {
                foreach (string s in prefixes)
                {
                    listener.Prefixes.Add(s);
                }
                listener.Start();
                SMain.Enqueue("HttpService started.");
            }
            catch (Exception err)
            {
                SMain.Enqueue("HttpService start failed! Error:" + err);
                return;
            }
         

            while (is_active) {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    Console.WriteLine("{0} {1} HTTP/1.1", request.HttpMethod, request.RawUrl);
                    Console.WriteLine("User-Agent: {0}", request.UserAgent);
                    Console.WriteLine("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                    Console.WriteLine("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "close");
                    Console.WriteLine("Host: {0}", request.UserHostName);
                    HttpListenerResponse response = context.Response;
                    if (request.UserHostAddress != Settings.HTTPTrustedIPAddress)
                    {
                        WriteResponse(response, "");
                        continue;
                    }
                    if (request.HttpMethod == "GET")
                    {
                        OnGetRequest(request, response);
                    }
                    else
                    {
                        OnPostRequest(request, response);
                    }
                }
                catch{}
            }
        }

        public void Stop() {
            is_active = false;
            if (listener != null)
            {
                listener.Stop();
            }           
        }

        public abstract void OnGetRequest(HttpListenerRequest request, HttpListenerResponse response);
        public abstract void OnPostRequest(HttpListenerRequest request, HttpListenerResponse response);

        public void WriteResponse(HttpListenerResponse response, string responseString)
        {
            try
            {
                response.ContentLength64 = Encoding.UTF8.GetByteCount(responseString);
                response.ContentType = "text/html; charset=UTF-8";
            }
            finally
            {
                Stream output = response.OutputStream;
                StreamWriter writer = new StreamWriter(output);
                writer.Write(responseString);
                writer.Close();
            }
        }
    }
}
