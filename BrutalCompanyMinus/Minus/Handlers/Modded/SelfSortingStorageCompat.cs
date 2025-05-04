using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using SelfSortingStorage.Cupboard;
using SelfSortingStorage.Utils;

namespace BrutalCompanyMinus.Minus.Handlers.Modded
{
    internal class SelfSortingStorageCompat
    {
        public static int GetSelfSortingStorageScrapValue()
        {
            try
            {
                int sssCount = 0;

                return sssCount;
            }
            catch (Exception)
            {
                Console.WriteLine("SelfSortingStorage types not found.");
                int sssCount = 0;
                return sssCount;
            }
        }
    }
}

