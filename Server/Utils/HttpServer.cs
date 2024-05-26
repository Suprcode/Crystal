using System.Net;
using Server.Library.MirEnvir;
using Shared;

namespace Server.Library.Utils {
    internal class HttpServer : HttpService {
        private Thread _thread;
        private CancellationTokenSource tokenSource = new();

        public HttpServer() {
            Host = Settings.HTTPIPAddress;
        }

        public void Start() {
            _thread = new Thread(Listen);
            _thread.Start(tokenSource.Token);
        }

        public new void Stop() {
            base.Stop();

            tokenSource.Cancel();
            Thread.Sleep(1000);
            tokenSource.Dispose();
        }


        public override void OnGetRequest(HttpListenerRequest request, HttpListenerResponse response) {
            string url = request.Url.PathAndQuery;
            if(url.Contains("?")) {
                url = url.Substring(0, url.IndexOf("?", StringComparison.Ordinal));
                url = url.ToLower();
            }

            try {
                switch (url) {
                    case "/":
                        WriteResponse(response, GameLanguage.GameName);
                        break;
                    case "/newaccount":
                        string id = request.QueryString["id"];
                        string psd = request.QueryString["psd"];
                        string email = request.QueryString["email"];
                        string name = request.QueryString["name"];
                        string question = request.QueryString["question"];
                        string answer = request.QueryString["answer"];
                        string ip = request.QueryString["ip"];
                        ClientPacket.NewAccount p = new ClientPacket.NewAccount();
                        p.AccountID = id;
                        p.Password = psd;
                        p.EMailAddress = email;
                        p.UserName = name;
                        p.SecretQuestion = question;
                        p.SecretAnswer = answer;
                        int result = Envir.Main.HTTPNewAccount(p, ip);
                        WriteResponse(response, result.ToString());
                        break;
                    case "/addnamelist":
                        id = request.QueryString["id"];
                        string fileName = request.QueryString["fileName"];
                        AddNameList(id, fileName);
                        WriteResponse(response, "true");
                        break;
                    case "/broadcast":
                        string msg = request.QueryString["msg"];
                        if(msg.Length < 5) {
                            WriteResponse(response, "short");
                            return;
                        }

                        Envir.Main.Broadcast(new ServerPacket.Chat {
                            Message = msg.Trim(),
                            Type = ChatType.Shout2
                        });
                        WriteResponse(response, "true");
                        break;
                    default:
                        WriteResponse(response, "error");
                        break;
                }
            } catch(Exception error) {
                WriteResponse(response, "request error: " + error);
            }
        }

        private void AddNameList(string playerName, string fileName) {
            fileName = Path.Combine(Settings.NameListPath, fileName);
            string sDirectory = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(sDirectory ?? throw new InvalidOperationException());
            if(!File.Exists(fileName)) {
                File.Create(fileName).Close();
            }

            string tempString = fileName;
            if(File.ReadAllLines(tempString).All(t => playerName != t)) {
                using(StreamWriter line = File.AppendText(tempString)) {
                    line.WriteLine(playerName);
                }
            }
        }

        public override void OnPostRequest(HttpListenerRequest request, HttpListenerResponse response) {
            Console.WriteLine("POST request: {0}", request.Url);
        }
    }
}
