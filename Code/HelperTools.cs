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
        private static string ToDatabase(this string s)
        {
            if (s.Length == 0 || s[0] == '#')
                return s;
            string key = $"#AMMONOMICON_AUTO_DB_STRING_{++_AutoStringCounter}";
            ETGMod.Databases.Strings.Items.AddComplex(key, s);
            return key;
        }

        /// <summary>Convenience method for calling an internal / private static function with an ILCursor</summary>
        internal static void CallPrivate(this ILCursor cursor, Type t, string name)
        {
            cursor.Emit(OpCodes.Call, t.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }
    }
}
