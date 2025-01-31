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
using static BrutalCompanyMinus.Minus.Events.JetpackFailure;
using System.IO;
using static BCMECodeSecurityCheck.Modules.CodeCheckingMain;
using UnityEngine.UIElements;
using TMPro;
using System.Linq.Expressions;
using System;
using UnityEngine.SceneManagement;


namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch(typeof(MenuManager))]
    internal class CodeCheckPatches
    {

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void PreventGameBoot(MenuManager __instance)
        {

            
            
            // Overwrite the door power and change the display text.
            if (SceneManager.GetSceneByName("MainMenu").isLoaded == true)
            {
                
                // Log.LogError("Menu");          GetComponent("HostButton")
                // Make sure the power is constantly maxed out.
                // __instance.DisplayMenuNotification("Secure core violated!\n Menu buttons disabled!", "What?!");
                __instance.menuNotificationText.text = "Welcome to\n Brutal Company Minus Extra!";
                __instance.menuNotificationButtonText.text = "[ Ok? ]";
              //  __instance.HostSettingsOptionsNormal.SetActive(false);
                __instance.versionNumberText.text = "v56 BCME";
             //   __instance.hostSettingsPanel.SetActive(false);
                //   var hostButton = __instance.menuButtons.GetComponent("HostButton");
                //   var hostText = hostButton.GetComponent("Button");
                //   hostText.name = "Debug";
                //   if (hostButton == null) 
                //   {
                //       throw new FileLoadException("hostButton is null");
                //   }
            //  var vrd = __instance.menuButtons.name = "Host";
             //   vrd.
            }

            
        }
        

    }
}
