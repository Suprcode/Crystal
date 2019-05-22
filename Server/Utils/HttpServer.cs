using Server.MirEnvir;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

namespace Server
{
    class HttpServer : HttpService {
       
        Thread thread;

        public HttpServer() {
            host = Settings.HTTPIPAddress;          
        }

        public void Start() {
            thread = new Thread(new ThreadStart(Listen));
            thread.Start();
        }

        public new void Stop() {
            base.Stop();
            thread.Abort();
        }


        public override void OnGetRequest(HttpListenerRequest request, HttpListenerResponse response)
		{
            string url = request.Url.PathAndQuery;
            if (url.Contains("?"))
            {
                url = url.Substring(0,url.IndexOf("?"));
            }
            try
            {
                switch (url)
                {
                    case "/":
                        writeRresponse(response, GameLanguage.GameName);
                        break;
                    case "/login":
                        string id = request.QueryString["id"].ToString();
                        string psd = request.QueryString["psd"].ToString();
                        int result = SMain.Envir.HTTPLogin(id, psd);
                        writeRresponse(response, result.ToString());                        
                        break;
                    default:
                        writeRresponse(response, "error");
                        break;
                }
            }
            catch
            {
                writeRresponse(response, "error");
            }            
        }


        void writeRresponse(HttpListenerResponse response , string responseString) {          
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

        public override void OnPostRequest(HttpListenerRequest request, HttpListenerResponse response) {
            Console.WriteLine("POST request: {0}", request.Url);
        }
    }

}
