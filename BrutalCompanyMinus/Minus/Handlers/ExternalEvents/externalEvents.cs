using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;

namespace BrutalCompanyMinus.Minus.Handlers.ExternalEvents
{
    [HarmonyPatch]
    public class externalEvents
    {
        public static void RegisterExternalEvents(List<MEvent> externalEvents)
        {
            foreach (MEvent e in externalEvents)
            {
                e.Initalize();
                EventManager.ExternalEvents.Add(e);
            }
        }

        public static void RegisterExternalEvent(MEvent externalEvent)
        {
            externalEvent.Initalize();
            EventManager.ExternalEvents.Add(externalEvent);
        }

        public static void UnregisterAllExternalEvents()
        {
            foreach (MEvent e in EventManager.ExternalEvents)
            {
                EventManager.events.Remove(e);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PreInitSceneScript), nameof(PreInitSceneScript.Start))]
        public static void runExternalEventSetup()
        {
            //Unregister
            UnregisterAllExternalEvents();

            //Register
            if (EventManager.ExternalEvents.Count > 0) 
            {
                EventManager.events.AddRange(EventManager.ExternalEvents);
            }
        }
    }
}
