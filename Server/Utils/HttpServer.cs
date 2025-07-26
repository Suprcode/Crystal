using System.Net;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.Library.Utils
{
    class HttpServer : HttpService
    {
        Thread _thread;
        CancellationTokenSource tokenSource = new();

        public HttpServer()
        {
            Host = Settings.HTTPIPAddress;
        }

        public void Start()
        {
            _thread = new Thread(Listen);
            _thread.Start(tokenSource.Token);
        }

        public new void Stop()
        {
            base.Stop();
            
            tokenSource.Cancel();
            Thread.Sleep(1000);
            tokenSource.Dispose();

        }


        public override void OnGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            var url = request.Url.PathAndQuery;
            if (url.Contains("?"))
            {
                url = url.Substring(0, url.IndexOf("?", StringComparison.Ordinal));
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
                        var id = request.QueryString["id"];
                        var psd = request.QueryString["psd"];
                        var email = request.QueryString["email"];
                        var name = request.QueryString["name"];
                        var question = request.QueryString["question"];
                        var answer = request.QueryString["answer"];
                        var ip = request.QueryString["ip"];
                        var p = new ClientPackets.NewAccount();
                        p.AccountID = id;
                        p.Password = psd;
                        p.EMailAddress = email;
                        p.UserName = name;
                        p.SecretQuestion = question;
                        p.SecretAnswer = answer;
                        var result = Envir.Main.HTTPNewAccount(p, ip);
                        WriteResponse(response, result.ToString());
                        break;                               
                    case "/addnamelist":
                        id = request.QueryString["id"];
                        var fileName = request.QueryString["fileName"];
                        AddNameList(id, fileName);
                        WriteResponse(response, "true");
                        break;              
                    case "/broadcast":
                        var msg = request.QueryString["msg"];
                        if (msg.Length < 5)
                        {
                            WriteResponse(response, "short");
                            return;
                        }
                        Envir.Main.Broadcast(new S.Chat
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

        void AddNameList(string playerName, string fileName)
        {
            fileName = Path.Combine(Settings.NameListPath, fileName);
            var sDirectory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(sDirectory ?? throw new InvalidOperationException());
            if (!File.Exists(fileName))
                File.Create(fileName).Close();

            var tempString = fileName;
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
