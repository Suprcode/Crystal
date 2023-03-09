using System.Net;
using System.Text;

namespace Server.Library.Utils
{
    internal abstract class HttpService
    {
        protected string Host;
        private HttpListener _listener;
        private bool _isActive = true;

        protected HttpService()
        {
        }

        public void Listen(object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            if (!HttpListener.IsSupported)
            {
                throw new InvalidOperationException(
                    "To use HttpListener the operating system must be Windows XP SP2 or Server 2003 or higher.");
            }
            string[] prefixes = { Host };

            _listener = new HttpListener();
            try
            {
                foreach (var s in prefixes)
                {
                    _listener.Prefixes.Add(s);
                }
                _listener.Start();
                MessageQueue.Instance.Enqueue("HttpService started.");
            }
            catch (Exception err)
            {
                MessageQueue.Instance.Enqueue("HttpService start failed! Error:" + err);
                return;
            }

            while (_isActive && !token.IsCancellationRequested)
            {
                try
                {
                    var context = _listener.GetContext();
                    var request = context.Request;
                    Console.WriteLine("{0} {1} HTTP/1.1", request.HttpMethod, request.RawUrl);
                    Console.WriteLine("User-Agent: {0}", request.UserAgent);
                    Console.WriteLine("Accept-Encoding: {0}", request.Headers["Accept-Encoding"]);
                    Console.WriteLine("Connection: {0}", request.KeepAlive ? "Keep-Alive" : "close");
                    Console.WriteLine("Host: {0}", request.UserHostName);
                    var response = context.Response;
                    if (context.Request.RemoteEndPoint != null)
                    {
                        var clientIp = context.Request.RemoteEndPoint.Address.ToString();

                        if (clientIp != Settings.HTTPTrustedIPAddress)
                        {
                            WriteResponse(response, "notrusted:" + clientIp);
                            continue;
                        }
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
                catch { }
            }
        }

        public void Stop()
        {
            _isActive = false;
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
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
                var output = response.OutputStream;
                var writer = new StreamWriter(output);
                writer.Write(responseString);
                writer.Close();
            }
        }
    }
}
