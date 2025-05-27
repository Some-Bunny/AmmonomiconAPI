using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmmonomiconAPI
{
    public class CustomCallbacks
    {
        internal static Dictionary<string, CustomAmmonomiconPageController> AllCustomControllers = new Dictionary<string, CustomAmmonomiconPageController>();
        internal static Dictionary<string, List<EncounterDatabaseEntry>> CachedDatabases = new Dictionary<string, List<EncounterDatabaseEntry>>();

        public static void AddStoredPage(string Key, CustomAmmonomiconPageController customAmmonomiconPageController)
        {
            if (!AllCustomControllers.ContainsKey(Key))
            {
                AllCustomControllers.Add(Key, customAmmonomiconPageController);
            }
        }
        public static CustomAmmonomiconPageController GetStoredCustomPage(string Key)
        {
            if (AllCustomControllers.ContainsKey(Key))
            {
                return AllCustomControllers[Key];
            }
            return null;
        }
        public static void TryAddEntryToStoredPage(string Key, EncounterDatabaseEntry EntryToAdd)
        {
            if (AllCustomControllers.ContainsKey(Key))
            {
                AllCustomControllers[Key].AddAdditionalEntry(EntryToAdd);
            }
        }

        public static void AddEntryToStoredDatabase(string Key, EncounterDatabaseEntry EntryToAdd)
        {
            if (CachedDatabases.ContainsKey(Key))
            {
                CachedDatabases[Key].Add(EntryToAdd);
            }
            else
            {
                CachedDatabases.Add(Key, new List<EncounterDatabaseEntry>()
                {
                    EntryToAdd
                });
            }
        }
        public static void AddEntriesToStoredDatabase(string Key, List<EncounterDatabaseEntry> EntriesToAdd)
        {
            if (CachedDatabases.ContainsKey(Key))
            {
                CachedDatabases[Key].AddRange(EntriesToAdd);
            }
            else
            {
                var newList = new List<EncounterDatabaseEntry>()
                {

                };
                newList.AddRange(EntriesToAdd);
                CachedDatabases.Add(Key, newList);
            }
        }
    }
}
