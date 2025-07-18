using HarmonyLib;
using System.Collections.Generic;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(EnemyAI))]
    internal class AntiBounty
    {
        public static List<int> enemyObjectIDs = new List<int>();

        private static int due = 0;

        [HarmonyPostfix]
        [HarmonyPatch(nameof(EnemyAI.KillEnemyOnOwnerClient))]
        static void PayOnkill(ref EnemyAI __instance)
        {
            // Keep track of the due amount on future kills where you have credits.
            if (due > 0 && Manager.currentTerminal.groupCredits > 0)
            {
                HUDManager.Instance.AddTextToChatOnServer("<color=#FF0000>Due payment.</color>");

                int owed = due;
                if(Manager.currentTerminal.groupCredits - due < 0)
                {
                    owed = Manager.currentTerminal.groupCredits;
                }
                Manager.PayCredits(-owed);
                due -= owed;

                HUDManager.Instance.AddTextToChatOnServer(string.Format("<color=#FF0000>New Due balance:</color> <color=#800000>{0}</color>", due));
            }

            if (!Events.AntiBounty.AntiBountyActive) return;
            foreach (int id in enemyObjectIDs)
            {
                if (__instance.gameObject.GetInstanceID() == id) return;
            }

            MEvent AntibountyEvent = Events.AntiBounty.Instance;
            
            int fineIncurred = UnityEngine.Random.Range(AntibountyEvent.Get(MEvent.ScaleType.MinValue), AntibountyEvent.Get(MEvent.ScaleType.MaxValue) + 1);

            //Lacking credits
            if (Manager.currentTerminal.groupCredits - fineIncurred < 0) {
                due += fineIncurred - Manager.currentTerminal.groupCredits;
                fineIncurred = Manager.currentTerminal.groupCredits;
                HUDManager.Instance.AddTextToChatOnServer(string.Format("<color=#FF0000>Since you lack credits to pay the full fine, you will be due </color><color=#800000>{0}</color><color=#FF0000> on the next kill.</color>", due));
            }
            Log.LogDebug("BCMER Fine " + -fineIncurred);
            Manager.PayCredits(-fineIncurred);

            enemyObjectIDs.Add(__instance.gameObject.GetInstanceID());

        }
    }
}