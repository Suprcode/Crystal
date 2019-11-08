using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using S = ServerPackets;

namespace Server
{
    class HttpServer : HttpService
    {

        Thread thread;

        public HttpServer()
        {
            host = Settings.HTTPIPAddress;
        }

        public void Start()
        {
            thread = new Thread(new ThreadStart(Listen));
            thread.Start();
        }

        public new void Stop()
        {
            base.Stop();
            if (thread != null)
            {
                thread.Abort();
            }
        }


        public override void OnGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string url = request.Url.PathAndQuery;
            if (url.Contains("?"))
            {
                url = url.Substring(0, url.IndexOf("?"));
                url = url.ToLower();
            }
            try
            {
                switch (url)
                {
                    case "/":
                        WriteResponse(response, GameLanguage.GameName);
                        break;
                    case "/newaccount":
                        string id = request.QueryString["id"].ToString();
                        string psd = request.QueryString["psd"].ToString();
                        string email = request.QueryString["email"].ToString();
                        string name = request.QueryString["name"].ToString();
                        string question = request.QueryString["question"].ToString();
                        string answer = request.QueryString["answer"].ToString();
                        string ip = request.QueryString["ip"].ToString();
                        ClientPackets.NewAccount p = new ClientPackets.NewAccount();
                        p.AccountID = id;
                        p.Password = psd;
                        p.EMailAddress = email;
                        p.UserName = name;
                        p.SecretQuestion = question;
                        p.SecretAnswer = answer;
                        int result = SMain.Envir.HTTPNewAccount(p, ip);
                        WriteResponse(response, result.ToString());
                        break;                               
                    case "/addnamelist":
                        id = request.QueryString["id"].ToString();
                        string fileName = request.QueryString["fileName"].ToString();
                        addNameList(id, fileName);
                        WriteResponse(response, "true");
                        break;              
                    case "/broadcast":
                        string msg = request.QueryString["msg"];
                        if (msg.Length < 5)
                        {
                            WriteResponse(response, "short");
                            return;
                        }
                        SMain.Envir.Broadcast(new S.Chat
                        {
                            Message = msg.Trim(),
                            Type = ChatType.Shout2
                        });
                        WriteResponse(response, "true");
                        break;
                    default:
                        WriteResponse(response, "error");
                        break;
                }
            }
            catch (Exception error)
            {
                WriteResponse(response, "request error: " + error);
            }
        }      

        void addNameList(string playerName, string fileName)
        {
            fileName = Settings.NameListPath + fileName;
            string sDirectory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(sDirectory);
            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            string tempString = fileName;
            if (File.ReadAllLines(tempString).All(t => playerName != t))
            {
                using (var line = File.AppendText(tempString))
                {
                    line.WriteLine(playerName);
                }
            }
        }    

        public override void OnPostRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            Console.WriteLine("POST request: {0}", request.Url);
        }
    }

}
