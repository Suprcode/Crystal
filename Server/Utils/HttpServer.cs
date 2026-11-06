using System.Net;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.Library.Utils
{
    class HttpServer : HttpService
    {
        readonly object _lifecycleLock = new();
        readonly CancellationTokenSource tokenSource = new();
        Thread _thread;
        bool _stopped;
        bool _tokenDisposed;
        System.Runtime.ExceptionServices.ExceptionDispatchInfo _failure;

        public HttpServer()
        {
            Host = Settings.HTTPIPAddress;
        }

        public void Start()
        {
            lock (_lifecycleLock)
            {
                if (_stopped || _thread != null)
                    throw new InvalidOperationException("The HTTP server can only be started once.");

                _thread = new Thread(() =>
                {
                    try
                    {
                        Listen(tokenSource.Token);
                    }
                    catch (Exception error)
                    {
                        lock (_lifecycleLock)
                            _failure ??= System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error);
                    }
                });
                try
                {
                    _thread.Start();
                }
                catch
                {
                    _thread = null;
                    throw;
                }
            }
        }

        public new void Stop()
        {
            Thread thread;
            lock (_lifecycleLock)
            {
                thread = _thread;
                if (thread == Thread.CurrentThread)
                    throw new InvalidOperationException("The HTTP listener thread cannot stop itself.");

                if (!_stopped)
                {
                    _stopped = true;
                    tokenSource.Cancel();
                }
                // Retry closing after a prior failure; do not join a still-blocked listener.
                base.Stop();
            }

            // Requests run on this thread, so joining it also drains in-flight handlers.
            thread?.Join();

            lock (_lifecycleLock)
            {
                if (!_tokenDisposed)
                {
                    tokenSource.Dispose();
                    _tokenDisposed = true;
                }
                var failure = _failure;
                _failure = null;
                failure?.Throw();
            }
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
                        WriteResponse(response, GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.GameName));
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
