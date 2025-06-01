using System.Net;
using System.Text;
using System.Text.Json;
using Server.MirEnvir;
using S = ServerPackets;
using Server.MirDatabase;

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
        protected void WriteResponse(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentType = "text/plain; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        protected void WriteJsonResponse(HttpListenerResponse response, string json)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            response.ContentType = "application/json; charset=utf-8";
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
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

                    case "/status":
                        try
                        {
                            var isOnline = Envir.Main?.Running ?? false;
                            WriteResponse(response, isOnline ? "Online" : "Offline");
                        }
                        catch (Exception ex)
                        {
                            WriteResponse(response, "It hung :(");
                        }
                        break;

                    case "/usercount":
                        try
                        {
                            var count = Envir.Main?.PlayerCount ?? 0;
                            WriteResponse(response, count.ToString());
                        }
                        catch (Exception ex)
                        {
                            WriteResponse(response, "It hung :(");
                        }
                        break;

                    case "/listaccounts":
                        {
                            // Directly use Envir.Main.AccountList
                            var rawList = Envir.Main?.AccountList;
                            if (rawList == null)
                            {
                                Console.WriteLine("[HTTP] /listaccounts → AccountList is null");
                                WriteJsonResponse(response, "[]");
                                break;
                            }

                            Console.WriteLine($"[HTTP] /listaccounts → found {rawList.Count} account(s).");

                            var accountDtos = rawList.Select(account => new
                            {
                                account.Index,
                                account.AccountID,
                                account.UserName,
                                account.AdminAccount,
                                account.Banned,
                                account.BanReason,
                                account.ExpiryDate,
                                account.Gold,
                                account.Credit
                            }).ToList();

                            string json = JsonSerializer.Serialize(accountDtos, new JsonSerializerOptions { WriteIndented = true });
                            WriteJsonResponse(response, json);
                        }
                        break;

                    //case "/playersonline":
                    //    try
                    //    {
                    //        var playerNames = Envir.Main?.Players
                    //            .Where(p => p?.Info != null)
                    //            .Select(p => new
                    //            {
                    //                p.Info.Index,
                    //                p.Info.Name,
                    //                p.Info.Level,
                    //                Class = p.Info.Class.ToString(),
                    //                Gender = p.Info.Gender.ToString(),
                    //                Map = p.CurrentMap?.Info?.Title
                    //            });
                    //        Console.WriteLine("[HTTP] Online Players: " + string.Join(", ", playerNames));

                    //        var json = JsonSerializer.Serialize(playerNames);
                    //        WriteJsonResponse(response, json);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine("[HTTP] /playersonline error: " + ex);
                    //        WriteJsonResponse(response, "[]");
                    //    }
                    //    break;

                    case "/newaccount":
                        var id = request.QueryString["id"];
                        var psd = request.QueryString["psd"];
                        var email = request.QueryString["email"];
                        var name = request.QueryString["name"];
                        var question = request.QueryString["question"];
                        var answer = request.QueryString["answer"];
                        var ip = request.QueryString["ip"];
                        var p = new ClientPackets.NewAccount
                        {
                            AccountID = id,
                            Password = psd,
                            EMailAddress = email,
                            UserName = name,
                            SecretQuestion = question,
                            SecretAnswer = answer
                        };
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
