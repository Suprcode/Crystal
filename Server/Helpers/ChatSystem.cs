using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Helpers
{
    internal static class ChatSystem
    {

         static Envir Envir
        {
            get { return Envir.Main; }
        }

        public static void SystemMessage(string chatMessage, bool triggerBroadcastInfo = false)
        {
            if (String.IsNullOrEmpty(chatMessage))
            {
                return;
            }

            foreach (var pl in Envir.Players)
            {
                pl.ReceiveChat(chatMessage, ChatType.System);

                if (triggerBroadcastInfo) 
                {
                    pl.BroadcastInfo();
                }
            }

        }
    }
}
