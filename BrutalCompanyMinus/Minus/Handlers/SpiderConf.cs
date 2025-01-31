using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MonsterPlushies;
using CSync.Lib;

namespace BrutalCompanyMinus.Minus.Handlers
{
    [HarmonyPatch]
    internal class SpiderConf
    {
        int valueS = 100;
        public SyncedEntry<int> BunkSpawnWeight
        {
            get
            {
                return BunkSpawnWeight;
            }



            set => BunkSpawnWeight = valueS;
            //  BunkerSpiderConfig.bunkSpawnWeight = 100

                

        }
    }
}
