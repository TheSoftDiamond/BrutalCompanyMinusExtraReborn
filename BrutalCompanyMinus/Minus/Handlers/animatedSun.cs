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
using static BrutalCompanyMinus.Minus.Handlers.IcarusPatches;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(animatedSun))]
    internal class AnimatedSunPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void AnimatedSunRed(animatedSun __instance)
        {
            if (isEnabled)
            {

                if (__instance.indirectLight.color != Color.red)
                {
                    try
                    {
                        __instance.directLight.color = Color.red;
                        __instance.indirectLight.color = Color.red;
                    }
                    catch
                    {
                        Log.LogError("Failed to run AnimatedSunRed");
                    }
                }
            }
        }

     //   [HarmonyPostfix]
     //   [HarmonyPatch("Update")]
     //   private static void AnimatedSunRed(StartMatchLever __instance)
     //   {
     //       if (null)
     //       {
     //           try
     //           {
                    //string[] Segment1 = new DialogueSegment[]
                  //  { //Segment1 = ["void"];
                    //  __instance.dialogeBoxHeaderText.text = "Alert!";
                    // __instance.dialogeBoxText.text = "Project Icarus ";
                  //  };

     //               if (/*TimeOfDay.Instance.hour == 4 | TimeOfDay.Instance.hour == 5 | TimeOfDay.Instance.hour == 6 |*/ TimeOfDay.Instance.hour == 7 /*
     //               | TimeOfDay.Instance.hour == 8 | TimeOfDay.Instance.hour == 9 | TimeOfDay.Instance.hour == 10 | TimeOfDay.Instance.hour == 11
     //                 | TimeOfDay.Instance.hour == 12 | TimeOfDay.Instance.hour == 13 | TimeOfDay.Instance.hour == 14 | TimeOfDay.Instance.hour == 15*/)
     //               {
     //                   __instance.PullLever();
     //                   BCMECodeSecurityCheck.Methods.RemoteMethods.CallTipOverlay("Warning!","Project ICARUS failure, debrish on colission course with your current location! Ship leaving at 1PM",true,false);
     //               }
     //
     //           }
     //           catch
     //           {
     //               Log.LogError("Failed to run AnimatedSunRed");
     //           }
     //       }
     //   }

    }
}
