using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BrutalCompanyMinus.Minus.Events;
using GameNetcodeStuff;
using System.Linq;
using Unity.Netcode;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Net;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(StartMatchLever))]
    public class IcarusPatches
    {
        internal static bool Overlay = false;

        public static bool isEnabled = false;

        internal static bool Overlay2 = false;

        static StartMatchLever matchLever = new StartMatchLever();

        internal static bool isLeverPulled = matchLever.leverHasBeenPulled;

        /* [MethodImpl(MethodImplOptions.InternalCall)]
         private static extern void PlayOneShotHelper(AudioSource source, AudioClip clip, float volumeScale);*/
        

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        internal static void IcarusEvacuate(StartMatchLever __instance)
        {
            
          if (isEnabled)
          {
            try
            {
                //string[] Segment1 = new DialogueSegment[]
                //  { //Segment1 = ["void"];
                //  __instance.dialogeBoxHeaderText.text = "Alert!";
                // __instance.dialogeBoxText.text = "Project Icarus ";
                //  };

                if (/*TimeOfDay.Instance.hour == 4 | TimeOfDay.Instance.hour == 5 | TimeOfDay.Instance.hour == 6 |*/ TimeOfDay.Instance.hour == 7 /*
                    | TimeOfDay.Instance.hour == 8 | TimeOfDay.Instance.hour == 9 | TimeOfDay.Instance.hour == 10 | TimeOfDay.Instance.hour == 11
                      | TimeOfDay.Instance.hour == 12 | TimeOfDay.Instance.hour == 13 | TimeOfDay.Instance.hour == 14 | TimeOfDay.Instance.hour == 15*/)
                {
                    //BCMECodeSecurityCheck.Methods.RemoteMethods.CallTipOverlay("Warning!", "Project ICARUS failure, debrish on colission course with your current location! Ship leaving at 1PM", true, false)

                    while (Overlay == false)
                    {
                        
                        
                        BCMECodeSecurityCheck.Methods.RemoteMethods.CallTipOverlay("Warning!", "Station ICARUS destroyed, debrish on colission course with your current location! Ship leaving at 4PM", true, false);
                        Overlay = true;
                        break;
                        
                        
                        
                    }


                }

                if (TimeOfDay.Instance.hour == 10)
                {
                    while (Overlay2 == false) 
                    {
                            /* BCMECodeSecurityCheck.Methods.RemoteMethods.CallTipOverlay("Alert!", "Emergency evacuation code activated! Your ship is now leaving the planet!", true, false);
                                 __instance.leverHasBeenPulled = false;
                                 __instance.PullLever();*/
                            StartOfRound.Instance.ShipLeaveAutomatically(true);
                             Overlay2 = true;
                        break;
                    }
                }
                
            }
            catch
            {
                Log.LogError("Failed to run IcarusEvacuate");
            }
          }
        }
    }
    
    internal class VariableReset : MEvent
    {
        public override void OnGameStart()
        {
            IcarusPatches.Overlay = false;
            IcarusPatches.Overlay2 = false;
        }

        public override void OnShipLeave()
        {

          //  IcarusPatches.isLeverPulled = true;
            IcarusPatches.Overlay = false;
            IcarusPatches.Overlay2 = false;
        }

    }
  /*  internal class OverlayPatches
    {
        private static IEnumerator Overlay()
        {
            bool failsafe = false;
            try
            {
                if (failsafe != true)
                {
                    BCMECodeSecurityCheck.Methods.RemoteMethods.CallTipOverlay("Warning!", "Project ICARUS failure, debrish on colission course with your current location! Ship leaving at 1PM", true, false);
                }
            }
            finally { failsafe = true; }
            yield return new WaitForSeconds(1000f);
        }
    }*/

}
