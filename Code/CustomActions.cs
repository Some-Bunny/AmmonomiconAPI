using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;

namespace AmmonomiconAPI
{
    public class CustomActions
    {
        /// <summary>
        /// Runs whenever any ammonomicon page is opened. The returned bool shows whether the page was precached or not. (Ammonomicon pages are pre-cached whenver they are created/first opened.)
        /// </summary>
        public static Action<AmmonomiconPageRenderer, bool> OnAnyPageOpened;
        /// <summary>
        /// Ran whenever the Ammonomicon is starting to be opened. The first bool dictates whether it was opened because of a player death, and the second bool dictates whether it was opened because of a player victory.
        /// </summary>
        public static Action<AmmonomiconController, bool, bool, EncounterTrackable> OnAmmonomiconStartOpened;
        /// <summary>
        /// Runs whenever this page is refreshed (Pages are refreshed when opened, and recheck certain data when refreshed. Player Equipment and Death pages are fully rebuilt when refreshed, while the rest update all of their entries to see if they have been encountered.).
        /// </summary>
        public static Action<AmmonomiconPageRenderer> OnAnyPageRefreshData;

        /// <summary>
        /// Runs when the Ammonomicon initializes and does all of its precaching. Is only ever ran once, do your weird initialization stuff here.
        /// </summary>
        public static Action<AmmonomiconController, AmmonomiconInstanceManager> OnAmmonomiconPrecache;


        /// <summary>
        /// Runs after the players equipment page is rebuilt.
        /// </summary>
        public static Action<AmmonomiconPageRenderer> OnEquipmentPageRebuilt;
        /// <summary>
        /// Runs after the left page is rebuilt. Bool returns true whether the page is a victory page, else is a death page. 
        /// </summary>
        public static Action<AmmonomiconDeathPageController, bool> OnDeathPageRebuiltLeft;
        /// <summary>
        /// Runs after the right page is rebuilt. Bool returns true whether the page is a victory page, else is a death page. 
        /// </summary>
        public static Action<AmmonomiconDeathPageController, bool> OnDeathPageRebuiltRight;


        /// <summary>
        /// Runs before the Equipment Left Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconPageRenderer, bool> OnPreEquipmentPageBuild;
        /// <summary>
        /// Runs before the Item Left Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconPageRenderer, bool> OnPreItemPageBuild;
        /// <summary>
        /// Runs before the Gun Left Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconPageRenderer, bool> OnPreGunPageBuild;
        /// <summary>
        /// Runs before the Enemy Left Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconPageRenderer, bool> OnPreEnemyPageBuild;
        /// <summary>
        /// Runs before the Boss Left Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconPageRenderer, bool> OnPreBossPageBuild;


        /// <summary>
        /// Runs before the Death Left Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconDeathPageController, bool> OnPreDeathPageBuildLeft;
        /// <summary>
        /// Runs before the Death Right Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconDeathPageController, bool> OnPreDeathPageBuildRight;


        /// <summary>
        /// Runs before any Page is built. Bool dictates whether the original building code will be ran or not. 
        /// </summary>
        public static Func<AmmonomiconPageRenderer, bool> OnPreAnyPageBuild;
    }
}
