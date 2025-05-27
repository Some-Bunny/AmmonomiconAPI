using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static AmmonomiconPageRenderer;
using UnityEngine;

namespace AmmonomiconAPI
{
    public class AmmonomiconAPIHooks
    {
        public static void Init()
        {

            try
            {
                // Mandatory Hooks
                var DelayedBuildPageHook = new Hook(
                    typeof(AmmonomiconPageRenderer).GetMethod("DelayedBuildPage", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(AmmonomiconAPIHooks).GetMethod("DelayedBuildPageHook", BindingFlags.Static | BindingFlags.NonPublic));
                
                var HandleOpenAmmonomiconDeath = new Hook(
                    typeof(AmmonomiconInstanceManager).GetMethod("HandleOpenAmmonomiconDeath", BindingFlags.Instance | BindingFlags.Public),
                    typeof(AmmonomiconAPIHooks).GetMethod("HandleOpenAmmonomiconDeathHook", BindingFlags.Static | BindingFlags.Public));
                /*
                var HandleOpenAmmonomicon = new Hook(
                     typeof(AmmonomiconInstanceManager).GetMethod("HandleOpenAmmonomicon", BindingFlags.Instance | BindingFlags.Public),
                     typeof(AmmonomiconAPIHooks).GetMethod("HandleOpenAmmonomiconHook", BindingFlags.Static | BindingFlags.Public));
                */

                /*
                var startHook = new Hook(
                    typeof(AmmonomiconInstanceManager).GetMethod("Open", BindingFlags.Instance | BindingFlags.Public),
                    typeof(AmmonomiconAPIHooks).GetMethod("OpenHook", BindingFlags.Static | BindingFlags.Public));
                */

                /*
                var ammonomiconInitializeHook = new Hook(
                    typeof(AmmonomiconPageRenderer).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.Public),
                    typeof(AmmonomiconAPIHooks).GetMethod("AmmonomiconInitializeHook", BindingFlags.Static | BindingFlags.Public));
                */
                /*
                var LoadPageUIAtPathHook = new Hook(
                    typeof(AmmonomiconController).GetMethod("LoadPageUIAtPath", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(AmmonomiconAPIHooks).GetMethod("LoadPageUIAtPathHook", BindingFlags.Static | BindingFlags.NonPublic));
                */
                /*
                var updateOnBecameActiveHook = new Hook(
                    typeof(AmmonomiconPageRenderer).GetMethod("UpdateOnBecameActive", BindingFlags.Instance | BindingFlags.Public),
                    typeof(AmmonomiconAPIHooks).GetMethod("UpdateOnBecameActiveHook", BindingFlags.Static | BindingFlags.Public));
                */


                /*
                var OpenDeath = new Hook(
                    typeof(AmmonomiconInstanceManager).GetMethod("OpenDeath", BindingFlags.Instance | BindingFlags.Public),
                    typeof(AmmonomiconAPIHooks).GetMethod("OpenDeathHook", BindingFlags.Static | BindingFlags.Public));
                */

                /*
                var checkLanguageFontsHook = new Hook(
                    typeof(AmmonomiconPageRenderer).GetMethod("CheckLanguageFonts", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(AmmonomiconAPIHooks).GetMethod("CheckLanguageFontsHook", BindingFlags.Static | BindingFlags.NonPublic));
                */


                /*
                var OpenInternal = new Hook(
                    typeof(AmmonomiconController).GetMethod("OpenInternal", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(AmmonomiconAPIHooks).GetMethod("OpenInternalHook", BindingFlags.Static | BindingFlags.Public));
                */

                /*
                var toggleHeaderImageHook = new Hook(
                    typeof(AmmonomiconPageRenderer).GetMethod("ToggleHeaderImage", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(AmmonomiconAPIHooks).GetMethod("ToggleHeaderImageHook", BindingFlags.Static | BindingFlags.Public));
                */
            }
            catch (Exception arg)
            {
                ETGModConsole.Log("oh no thats not good (AmmonomiconHooks broke)\n\n" + arg);

                //LostItemsMod.Log(string.Format("D:", ), "#eb1313");
            }
        }


        private static IEnumerator DelayedBuildPageHook(Func<AmmonomiconPageRenderer, IEnumerator> orig, AmmonomiconPageRenderer self)
        {
            //Debug.Log("DelayedBuildPageHook_");
            bool isInvoked = true;
            var invokers = CustomActions.OnPreAnyPageBuild?.GetInvocationList();
            if (invokers != null)
            {
                foreach (var entry in invokers)
                {
                    isInvoked = (bool)entry.DynamicInvoke(self);
                }
            }
            if (isInvoked == true)
            {
                if (self.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
                {
                    while (GameManager.Instance.IsSelectingCharacter)
                    {
                        yield return null;
                    }
                }
                //Debug.Log("DelayedBuildPageHook_: " + self.pageType);


                switch (self.pageType)
                {
                    case AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT:
                        self.InitializeEquipmentPageLeft();
                        break;
                    case AmmonomiconPageRenderer.PageType.EQUIPMENT_RIGHT:
                        self.InitializeEquipmentPageRight();
                        break;
                    case AmmonomiconPageRenderer.PageType.GUNS_LEFT:
                        self.InitializeGunsPageLeft();
                        break;
                    case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
                        self.SetPageDataUnknown(self);
                        break;
                    case AmmonomiconPageRenderer.PageType.ITEMS_LEFT:
                        self.InitializeItemsPageLeft();
                        break;
                    case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
                        self.SetPageDataUnknown(self);
                        break;
                    case AmmonomiconPageRenderer.PageType.ENEMIES_LEFT:
                        self.InitializeEnemiesPageLeft();
                        break;
                    case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
                        self.SetPageDataUnknown(self);
                        break;
                    case AmmonomiconPageRenderer.PageType.BOSSES_LEFT:
                        self.InitializeBossesPageLeft();
                        break;
                    case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
                        self.SetPageDataUnknown(self);
                        break;
                    case AmmonomiconPageRenderer.PageType.DEATH_LEFT:
                        self.InitializeDeathPageLeft();
                        break;
                    case AmmonomiconPageRenderer.PageType.DEATH_RIGHT:
                        self.InitializeDeathPageRight();
                        break;
                    default:

                        foreach (var entry in StaticData.customBookmarks)
                        {
                            //Debug.Log("DelayedBuildPageHook_: " + entry.Left + " | " + entry.Right);

                            if (self.pageType == entry.Left)
                            {
                                //self.
                                //entry.ammonomiconPageTag.InitializeName(self);
                                entry.ammonomiconPageTag.InitializeItemsPageLeft(self);
                                break;
                            }
                            if (self.pageType == entry.Right)
                            {
                                entry.ammonomiconPageTag.InitializeItemsPageRight(self);
                                break;
                            }
                        }
                        break;
                }
            }
            yield break;
        }
        
        public static IEnumerator HandleOpenAmmonomiconDeathHook(Func<AmmonomiconInstanceManager, IEnumerator> orig, AmmonomiconInstanceManager self)
        {
            float v = 0.50f / (float)(self.bookmarks.Count());
            foreach (var entry in self.bookmarks)
            {
                entry.TriggerAppearAnimation();
                if (entry.LeftPageType != PageType.DEATH_LEFT && entry.RightPageType != PageType.DEATH_RIGHT)
                {
                    entry.Disable();
                    yield return self.StartCoroutine(self.InvariantWait(v));
                }
            }
            yield return self.StartCoroutine(self.InvariantWait(0.025f));
            var elemt = self.bookmarks.Where(na => na.name == "Death").FirstOrDefault();
            elemt.IsCurrentPage = true;
            self.m_currentlySelectedBookmark = self.bookmarks.IndexOf(elemt);
            yield break;
        }
    }
}
