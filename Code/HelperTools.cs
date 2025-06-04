using Alexandria.NPCAPI;
using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace AmmonomiconAPI
{
    public static class HelperTools
    {
        public static PlayerController LastPlayerOpenedUI()
        {
            PlayerController playerController = null;
            if (GameManager.Instance.IsSelectingCharacter)
            {
                if (Foyer.Instance.CurrentSelectedCharacterFlag)
                {
                    playerController = ((GameObject)BraveResources.Load(Foyer.Instance.CurrentSelectedCharacterFlag.CharacterPrefabPath, ".prefab")).GetComponent<PlayerController>();
                }
            }
            else
            {
                playerController = GameManager.Instance.PrimaryPlayer;
                for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
                {
                    if (GameManager.Instance.AllPlayers[i].PlayerIDX == GameManager.Instance.LastPausingPlayerID)
                    {
                        playerController = GameManager.Instance.AllPlayers[i];
                    }
                }
            }
            return playerController;
        }

        public static dfScrollPanel PageRendererScrollParentObject(this AmmonomiconPageRenderer ammonomiconPageRenderer)
        {
            int children = ammonomiconPageRenderer.transform.parent.childCount;
            for (int i = 0; i < children; i++)
            {
                var child = ammonomiconPageRenderer.transform.parent.GetChild(i).gameObject.GetComponent<dfScrollPanel>();
                if (child == null) { return child; }
            }
            return null;
        }

        public static EncounterDatabaseEntry CreateDummyEncounterDatabaseEntry(JournalEntry journalEntry, string EncounterGUID = null, bool autoEncounter = true)
        {
            EncounterGUID = EncounterGUID ?? Guid.NewGuid().ToString();
            EncounterDatabaseEntry encounterDatabaseEntry = new EncounterDatabaseEntry();
            encounterDatabaseEntry.journalData = journalEntry;
            encounterDatabaseEntry.path = string.Empty;
            encounterDatabaseEntry.myGuid = EncounterGUID;
            
            if (autoEncounter)
            {
                GameStatsManager.Instance.HandleEncounteredObjectRaw(EncounterGUID);
            }
            return encounterDatabaseEntry;
        }

        public static EncounterDatabaseEntry CreateDummyEncounterDatabaseEntry(string OverrideShootStyleString, JournalEntry journalEntry, string EncounterGUID = null, bool autoEncounter = true)
        {
            EncounterGUID = EncounterGUID ?? Guid.NewGuid().ToString();
            EncounterDatabaseEntry encounterDatabaseEntry = new EncounterDatabaseEntry();
            encounterDatabaseEntry.journalData = journalEntry;
            encounterDatabaseEntry.path = string.Empty;
            encounterDatabaseEntry.myGuid = EncounterGUID;
            if (OverrideShootStyleString != null)
            {
                encounterDatabaseEntry.shootStyleInt = OverrideShootStyleString.To2ndTapeDatabase();
            }
            if (autoEncounter)
            {
                GameStatsManager.Instance.HandleEncounteredObjectRaw(EncounterGUID);
            }
            return encounterDatabaseEntry;
        }
        public static EncounterDatabaseEntry CreateDummyEncounterDatabaseEntry(int? OverrideShootStyleValue, JournalEntry journalEntry,  string EncounterGUID = null, bool autoEncounter = true)
        {
            EncounterGUID = EncounterGUID ?? Guid.NewGuid().ToString();
            EncounterDatabaseEntry encounterDatabaseEntry = new EncounterDatabaseEntry();
            encounterDatabaseEntry.journalData = journalEntry;
            encounterDatabaseEntry.path = string.Empty;
            encounterDatabaseEntry.myGuid = EncounterGUID;
            if (OverrideShootStyleValue != null)
            {
                encounterDatabaseEntry.shootStyleInt = OverrideShootStyleValue.Value;
            }
            if (autoEncounter)
            {
                GameStatsManager.Instance.HandleEncounteredObjectRaw(EncounterGUID);
            }
            return encounterDatabaseEntry;
        }

        public static JournalEntry CreateJournalEntryData(string DisplayNameKey, string AmmonomiconSprite, string Tagline, string FullEntry, bool isEnemy = false, Texture2D PortraitTexture = null)
        {
            JournalEntry journalEntry = new JournalEntry();
            journalEntry.PrimaryDisplayName = DisplayNameKey.ToDatabase();
            journalEntry.NotificationPanelDescription = Tagline.ToDatabase();
            journalEntry.AmmonomiconFullEntry = FullEntry.ToDatabase();


            journalEntry.AmmonomiconSprite = AmmonomiconSprite;
            journalEntry.enemyPortraitSprite = PortraitTexture;
            journalEntry.IsEnemy = isEnemy;
            
            return journalEntry;
        }
        private static int _AutoStringCounter = 0;
        private static int _AutoStringCounter2ndTape = -1;
        private static string ToDatabase(this string s)
        {
            if (s.Length == 0 || s[0] == '#')
                return s;
            string key = $"#AMMONOMICON_AUTO_DB_STRING_{++_AutoStringCounter}";
            ETGMod.Databases.Strings.Items.AddComplex(key, s);
            return key;
        }

        public static int To2ndTapeDatabase(this string s)
        {
            string key = "";
            if (s.Length == 0)
            {
                return -1;
            }
            _AutoStringCounter2ndTape--;
            if (s[0] == '#')
            {
                key = s;
            }
            else
            {
                key = $"#AMMONOMICON_AUTO_DB_STRING_TAPE_{s}";
                ETGMod.Databases.Strings.Items.AddComplex(key, s);
            }
            StaticData.Stored2ndTapeTexts.Add(_AutoStringCounter2ndTape, key);
            return _AutoStringCounter2ndTape;
        }



        /// <summary>Convenience method for calling an internal / private static function with an ILCursor</summary>
        internal static void CallPrivate(this ILCursor cursor, Type t, string name)
        {
            cursor.Emit(OpCodes.Call, t.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }
    }
}
