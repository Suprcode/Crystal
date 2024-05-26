using Server.Library.MirEnvir;
using Server.Library.MirObjects;
using Shared;

namespace Server.Library.Helpers {
    internal static class ChatSystem {
        private static Envir Envir => Envir.Main;

        public static void SystemMessage(string chatMessage, bool triggerBroadcastInfo = false) {
            if(String.IsNullOrEmpty(chatMessage)) {
                return;
            }

            foreach(PlayerObject pl in Envir.Players) {
                pl.ReceiveChat(chatMessage, ChatType.System);

                if(triggerBroadcastInfo) {
                    pl.BroadcastInfo();
                }
            }
        }
    }
}
