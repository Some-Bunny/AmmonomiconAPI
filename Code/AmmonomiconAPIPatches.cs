using Alexandria.ItemAPI;
using FullInspector;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using static AmmonomiconPageRenderer;

namespace AmmonomiconAPI
{
    class AmmonomiconAPIPatches
    {

        [HarmonyPatch(typeof(EncounterDatabaseEntry), nameof(EncounterDatabaseEntry.GetSecondTapeDescriptor))]
        public class Patch_GetSecondTapeDescriptor_Class
        {
            [HarmonyPrefix]
            private static bool GetSecondTapeDescriptor(EncounterDatabaseEntry __instance, ref string __result)
            {
                //CustomActions.OnAmmonomiconPrecache?.Invoke(__instance, __instance.m_AmmonomiconInstance);
                if (StaticData.Stored2ndTapeTexts.ContainsKey(__instance.shootStyleInt))
                {
                    __result = StringTableManager.GetItemsString(StaticData.Stored2ndTapeTexts[__instance.shootStyleInt], -1);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(AmmonomiconController), nameof(AmmonomiconController.PrecacheAllData))]
        public class Patch_PrecacheAllData_Class
        {
            [HarmonyPostfix]
            private static void PrecacheAllData(AmmonomiconController __instance)
            {          
                CustomActions.OnAmmonomiconPrecache?.Invoke(__instance, __instance.m_AmmonomiconInstance);
            }
        }

        [HarmonyPatch(typeof(AmmonomiconController), nameof(AmmonomiconController.Awake))]
        public class Patch_Awake_Class
        {
            [HarmonyPrefix]
            private static bool Awake(AmmonomiconController __instance)
            {
                var ammo = Enum.GetValues(typeof(AmmonomiconPageRenderer.PageType)).Length;
                float c = -20 - ammo;
                int a = (int)ammo + 4;
                for (int i = 0; i < a; i++)
                {
                    __instance.m_offsetInUse.Add(false);
                    __instance.m_offsets.Add(new Vector3((float)(-200 + (c * i)), (float)(-200 + (c * i)), 0f));
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeDeathPageRight))]
        public class Patch_InitializeDeathPageRight_Class
        {
            [HarmonyPrefix]
            private static bool PrecacheAllData(AmmonomiconPageRenderer __instance)
            {
                AmmonomiconDeathPageController component = __instance.guiManager.GetComponent<AmmonomiconDeathPageController>();
                component.DoInitialize();
                dfScrollPanel component2 = component.transform.Find("Scroll Panel").Find("Footer").Find("ScrollItemsPanel").GetComponent<dfScrollPanel>();
                dfPanel component3 = component2.transform.Find("AllItemsPanel").GetComponent<dfPanel>();
                for (int i = 0; i < component3.transform.childCount; i++)
                {
                    UnityEngine.Object.Destroy(component3.transform.GetChild(i).gameObject);
                }
                List<tk2dBaseSprite> list = new List<tk2dBaseSprite>();
                for (int j = 0; j < GameManager.Instance.AllPlayers.Length; j++)
                {
                    PlayerController playerController = GameManager.Instance.AllPlayers[j];
                    for (int k = 0; k < playerController.inventory.AllGuns.Count; k++)
                    {
                        Gun gun = playerController.inventory.AllGuns[k];
                        tk2dClippedSprite tk2dClippedSprite = __instance.AddSpriteToPage<tk2dClippedSprite>(gun.GetSprite().Collection, gun.DefaultSpriteID);
                        SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite, Color.black, 0.1f, 0.01f);
                        tk2dClippedSprite.transform.parent = component3.transform;
                        tk2dClippedSprite.transform.position = component3.GetCenter();
                        list.Add(tk2dClippedSprite);
                    }
                    for (int l = 0; l < playerController.activeItems.Count; l++)
                    {
                        tk2dClippedSprite tk2dClippedSprite2 = __instance.AddSpriteToPage<tk2dClippedSprite>(playerController.activeItems[l].sprite.Collection, playerController.activeItems[l].sprite.spriteId);
                        SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite2, Color.black, 0.1f, 0.01f);
                        tk2dClippedSprite2.transform.parent = component3.transform;
                        tk2dClippedSprite2.transform.position = component3.GetCenter();
                        list.Add(tk2dClippedSprite2);
                    }
                    for (int m = 0; m < playerController.passiveItems.Count; m++)
                    {
                        tk2dClippedSprite tk2dClippedSprite3 = __instance.AddSpriteToPage<tk2dClippedSprite>(playerController.passiveItems[m].sprite.Collection, playerController.passiveItems[m].sprite.spriteId);
                        SpriteOutlineManager.AddScaledOutlineToSprite<tk2dClippedSprite>(tk2dClippedSprite3, Color.black, 0.1f, 0.01f);
                        tk2dClippedSprite3.transform.parent = component3.transform;
                        tk2dClippedSprite3.transform.position = component3.GetCenter();
                        list.Add(tk2dClippedSprite3);
                    }
                }
                CustomActions.OnDeathPageFinalizing?.Invoke(__instance, list);
                list = list.ttOrderBy((tk2dBaseSprite a) => a.GetBounds().size.y);
                List<tk2dBaseSprite> list2 = new List<tk2dBaseSprite>();
                __instance.BoxArrangeItems(component3, list, new Vector2(0f, 6f), new Vector2(6f, 3f), ref list2);
                __instance.StartCoroutine(__instance.HandleDeathItemsClipping(component3, list));
                return false;
            }
        }



        [HarmonyPatch(typeof(AmmonomiconController), nameof(AmmonomiconController.LoadPageUIAtPath))]
        public class Patch_LoadPageUIAtPath_Class
        {
            [HarmonyPrefix]
            private static bool Patch_LoadPageUIAtPath(AmmonomiconController __instance, string path, AmmonomiconPageRenderer.PageType pageType, bool isPreCache, bool isVictory, ref AmmonomiconPageRenderer __result)
            {
                AmmonomiconPageRenderer ammonomiconPageRenderer;
                var inst = __instance.m_AmmonomiconInstance.GetComponent<InstancePlusManager>();
                bool isPrecached = false;
                if (__instance.m_extantPageMap.ContainsKey(pageType))
                {
                    foreach (var entry in __instance.m_extantPageMap)
                    {
                        if (StaticData.AllCustomEnums.Contains(entry.Key))
                        {
                            entry.Value.targetRenderer.enabled = false;
                        }
                    }
                    ammonomiconPageRenderer = __instance.m_extantPageMap[pageType];
                    if (pageType == AmmonomiconPageRenderer.PageType.DEATH_LEFT || pageType == AmmonomiconPageRenderer.PageType.DEATH_RIGHT)
                    {
                        AmmonomiconDeathPageController component = ammonomiconPageRenderer.transform.parent.GetComponent<AmmonomiconDeathPageController>();
                        component.isVictoryPage = isVictory;
                    }
                    ammonomiconPageRenderer.EnableRendering();
                    ammonomiconPageRenderer.DoRefreshData();
                }
                else
                {


                    GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(path, ".prefab"));
                    ammonomiconPageRenderer = gameObject.GetComponentInChildren<AmmonomiconPageRenderer>();
                    dfGUIManager component2 = __instance.m_AmmonomiconBase.GetComponent<dfGUIManager>();
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(__instance.m_LowerRenderTargetPrefab.gameObject);
                    gameObject2.transform.parent = component2.transform.Find("Core");
                    gameObject2.transform.localPosition = Vector3.zero;
                    gameObject2.layer = LayerMask.NameToLayer("SecondaryGUI");
                    if (inst && pageType != AmmonomiconPageRenderer.PageType.DEATH_LEFT && pageType != AmmonomiconPageRenderer.PageType.DEATH_RIGHT)
                    {
                        foreach (var entry in StaticData.customBookmarks)
                        {
                            //Debug.Log("LoadPageUIAtPathHook_"+ entry.Left + " | " + entry.Right);

                            if (pageType == entry.Left)
                            {
                                ammonomiconPageRenderer.pageType = pageType;
                                gameObject.gameObject.name = $"Page {pageType}";
                                gameObject2.gameObject.name = $"Page {entry.UniqueKey} Left";
                                ammonomiconPageRenderer.guiManager = component2;
                                ammonomiconPageRenderer.transform.parent.parent = __instance.m_AmmonomiconBase.transform.parent;
                                break;
                            }
                            if (pageType == entry.Right)
                            {
                                ammonomiconPageRenderer.pageType = pageType;
                                gameObject.gameObject.name = $"Page {pageType}";
                                gameObject2.gameObject.name = $"Page {entry.UniqueKey} Right";
                                ammonomiconPageRenderer.guiManager = component2;
                                ammonomiconPageRenderer.transform.parent.parent = __instance.m_AmmonomiconBase.transform.parent;

                                if (entry.DFGUI_BackingImage != string.Empty)
                                {
                                    ammonomiconPageRenderer.HeaderBGSprite.spriteName = entry.DFGUI_BackingImage;
                                    if (entry.CustomAtlas != null)
                                    {
                                        ammonomiconPageRenderer.HeaderBGSprite.atlas = entry.CustomAtlas;
                                    }
                                }
                                break;
                            }
                        }
                    }

                    MeshRenderer component3 = gameObject2.GetComponent<MeshRenderer>();
                    if (isVictory)
                    {
                        AmmonomiconDeathPageController component4 = ammonomiconPageRenderer.transform.parent.GetComponent<AmmonomiconDeathPageController>();
                        component4.isVictoryPage = true;
                    }


                    var ammo = Enum.GetValues(typeof(AmmonomiconPageRenderer.PageType)).Length;
                    int a = (int)ammo + 4;
                    float c = -20 - ammo;



                    for (int i = __instance.m_offsets.Count - 2; i < (int)a; ++i)
                    {

                        if (__instance.m_offsets.Count > i)
                        {
                            //__instance.m_offsetInUse[i] = false;
                            __instance.m_offsets[i] = new Vector3(-200 + +(c * i), -200 + (c * i), 0f);

                        }
                        else
                        {
                            __instance.m_offsetInUse.Add(false);
                            __instance.m_offsets.Add(new Vector3(-200 + (c * i), -200 + (c * i), 0f));
                        }
                    }

                    for (int i = 0; i < __instance.m_offsets.Count; i++)
                    {
                        if (__instance.m_offsetInUse[i] == false)
                        {
                            __instance.m_offsetInUse[i] = true;
                            gameObject.transform.position = __instance.m_offsets[i];
                            ammonomiconPageRenderer.offsetIndex = i;
                            break;
                        }
                    }
                    ammonomiconPageRenderer.Initialize(component3);
                    ammonomiconPageRenderer.EnableRendering();
                    __instance.m_extantPageMap.Add(pageType, ammonomiconPageRenderer);


                    if (isPreCache)
                    {
                        isPrecached = true;
                        ammonomiconPageRenderer.Disable(isPreCache);
                    }
                    else
                    {
                        ammonomiconPageRenderer.transform.parent.parent = __instance.m_AmmonomiconBase.transform.parent;
                    }
                }
                CustomActions.OnAnyPageOpened?.Invoke(ammonomiconPageRenderer, isPrecached);
                __result = ammonomiconPageRenderer;
                return false;
            }
        }

        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.UpdateOnBecameActive))]
        public class Patch_UpdateOnBecameActive_Class
        {
            [HarmonyPrefix]
            private static bool Patch_UpdateOnBecameActive(AmmonomiconPageRenderer __instance)
            {
                __instance.ForceUpdateLanguageFonts();
                if (AmmonomiconController.Instance.ImpendingLeftPageRenderer == null || AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
                {
                    switch (__instance.pageType)
                    {
                        case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
                            __instance.SetFirstVisibleTexts();
                            // typeof(AmmonomiconPageRenderer).GetMethod("SetFirstVisibleTexts", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, null);
                            break;
                        case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
                            __instance.SetFirstVisibleTexts();
                            break;
                        case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
                            __instance.SetFirstVisibleTexts();
                            break;
                        case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
                            __instance.SetFirstVisibleTexts();
                            break;
                        default:
                            //__instance.SetFirstVisibleTexts();
                            foreach (var entry in StaticData.customBookmarks)
                            {
                                //Debug.Log("Patch_UpdateOnBecameActive_: " + (__instance.pageType));

                                if (__instance.pageType == entry.Left)
                                {
                                    entry.ammonomiconPageTag.InitializeName(__instance);
                                    entry.ammonomiconPageTag.OnPageOpenedLeft(__instance);

                                    break;
                                }
                                if (__instance.pageType == entry.Right)
                                {
                                    entry.ammonomiconPageTag.OnPageOpenedRight(__instance);
                                    break;
                                }
                            }
                            break;

                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(AmmonomiconInstanceManager), nameof(AmmonomiconInstanceManager.OpenDeath))]
        public class Patch_OpenDeathHook_Class
        {
            [HarmonyPrefix]
            private static bool Patch_OpenDeathHook(AmmonomiconInstanceManager __instance)
            {
                var elemt = __instance.bookmarks.Where(na => na.name == "Death").FirstOrDefault();
                __instance.m_currentlySelectedBookmark = __instance.bookmarks.IndexOf(elemt);
                elemt.IsCurrentPage = true;
                __instance.StartCoroutine(__instance.HandleOpenAmmonomiconDeath());
                return false;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.CheckLanguageFonts))]
        public class Patch_CheckLanguageFonts_Class
        {
            [HarmonyPrefix]
            private static bool Patch_CheckLanguageFonts(AmmonomiconPageRenderer __instance, dfLabel mainText)
            {
                if (__instance.EnglishFont == null)
                {
                    __instance.EnglishFont = mainText.Font;
                    __instance.OtherLanguageFont = GameUIRoot.Instance.Manager.DefaultFont;
                }
                __instance.AdjustForChinese();
                if (__instance.m_cachedLanguage != GameManager.Options.CurrentLanguage)
                {
                    __instance.m_cachedLanguage = GameManager.Options.CurrentLanguage;
                    bool b = false;
                    foreach (var entry in StaticData.customBookmarks)
                    {
                        if (entry.Right == __instance.pageType)
                        {
                            __instance.SetPageDataUnknown(__instance);
                            b = true;

                        }
                    }
                    if (b == true)
                    {
                        switch (__instance.pageType)
                        {
                            case AmmonomiconPageRenderer.PageType.GUNS_RIGHT:
                                __instance.SetPageDataUnknown(__instance);
                                break;
                            case AmmonomiconPageRenderer.PageType.ITEMS_RIGHT:
                                __instance.SetPageDataUnknown(__instance);
                                break;
                            case AmmonomiconPageRenderer.PageType.ENEMIES_RIGHT:
                                __instance.SetPageDataUnknown(__instance);
                                break;
                            case AmmonomiconPageRenderer.PageType.BOSSES_RIGHT:
                                __instance.SetPageDataUnknown(__instance);
                                break;
                            default:
                                __instance.SetPageDataUnknown(__instance);
                                break;
                                //case (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_RIGHT:
                                //self.SetPageDataUnknown(self);
                                //typeof(AmmonomiconPageRenderer).GetMethod("SetPageDataUnknown", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(self, new object[] { self });
                                //break;
                        }
                    }

                }
                if (StringTableManager.CurrentLanguage == StringTableManager.GungeonSupportedLanguages.ENGLISH)
                {
                    if (mainText.Font != __instance.EnglishFont)
                    {
                        mainText.Atlas = __instance.guiManager.DefaultAtlas;
                        mainText.Font = __instance.EnglishFont;
                    }
                }
                else if (StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.JAPANESE && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.KOREAN && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.CHINESE && StringTableManager.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.RUSSIAN)
                {
                    if (mainText.Font != __instance.OtherLanguageFont)
                    {
                        mainText.Atlas = GameUIRoot.Instance.Manager.DefaultAtlas;
                        mainText.Font = __instance.OtherLanguageFont;
                    }
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconController), nameof(AmmonomiconController.OpenInternal))]
        public class Patch_OpenInternal_Class
        {
            [HarmonyILManipulator]
            private static void Patch_OpenInternalIL(ILContext il)
            {
                ILCursor cursor = new ILCursor(il);
                if (!cursor.TryGotoNext(MoveType.Before,
                    instr => instr.MatchLdfld<AmmonomiconInstanceManager>("bookmarks"),
                    instr => instr.MatchLdlen(),
                    instr => instr.MatchConvI4(),
                    instr => instr.MatchLdcI4(1),
                    instr => instr.MatchSub()
                    ))
                  return;

                cursor.RemoveRange(5);
                cursor.CallPrivate(typeof(Patch_OpenInternal_Class), nameof(GetActualDeathPageIndex));
            }

            private static int GetActualDeathPageIndex(AmmonomiconInstanceManager aim)
            {
                return aim.bookmarks.IndexOf(aim.bookmarks.Where(na => na.name == "Death").FirstOrDefault());
            }

            [HarmonyPostfix]
            private static void Patch_OpenInternal(AmmonomiconController __instance, bool isDeath, bool isVictory, EncounterTrackable targetTrackable)
            {
                CustomActions.OnAmmonomiconStartOpened?.DynamicInvoke(__instance, isDeath, isVictory, targetTrackable);
            }
        }
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.ToggleHeaderImage))]
        public class Patch_ToggleHeaderImage_Class
        {
            [HarmonyPrefix]
            private static bool ToggleHeaderImage(AmmonomiconPageRenderer __instance)
            {
                foreach (var entry in StaticData.customBookmarks)
                {
                    if (entry.Left == __instance.pageType)
                    {
                        if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH && __instance.HeaderBGSprite != null)
                        {
                            __instance.HeaderBGSprite.IsVisible = false;
                        }
                        else if (__instance.HeaderBGSprite != null)
                        {
                            __instance.HeaderBGSprite.IsVisible = true;
                        }
                        return false;
                    }
                }

                if (__instance.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT || __instance.pageType == AmmonomiconPageRenderer.PageType.GUNS_LEFT || __instance.pageType == AmmonomiconPageRenderer.PageType.ITEMS_LEFT || __instance.pageType == AmmonomiconPageRenderer.PageType.ENEMIES_LEFT || __instance.pageType == AmmonomiconPageRenderer.PageType.BOSSES_LEFT)
                {
                    if (GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH && __instance.HeaderBGSprite != null)
                    {
                        __instance.HeaderBGSprite.IsVisible = false;
                    }
                    else if (__instance.HeaderBGSprite != null)
                    {
                        __instance.HeaderBGSprite.IsVisible = true;
                    }
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconBookmarkController), nameof(AmmonomiconBookmarkController.TriggerAppearAnimation))]
        public class Patch_Enable_Class
        {
            [HarmonyPrefix]
            private static bool TriggerAppearAnimation(AmmonomiconBookmarkController __instance)
            {
                var inst = AmmonomiconController.Instance.m_AmmonomiconInstance.GetComponent<InstancePlusManager>();
                foreach (var entry in StaticData.customBookmarks)
                {
                    if (__instance.LeftPageType == entry.Left && __instance.RightPageType == entry.Right)
                    {
                        bool isActive = entry.ammonomiconPageTag.ShouldBeActive();
                        if (isActive)
                        {
                            return true;
                        }
                        else
                        {
                            __instance.Disable();
                            var scroller = __instance.transform.parent.GetComponent<dfScrollPanel>();
                            scroller.Localize();
                            return false;
                        }               
                    }
                }
                return true;
            }
        }


        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.DoRefreshData))]
        public class Patch_DoRefreshData_Class
        {
            [HarmonyPrefix]
            private static bool DoRefreshData(AmmonomiconPageRenderer __instance)
            {
                if (__instance.pageType == AmmonomiconPageRenderer.PageType.EQUIPMENT_LEFT)
                {
                    for (int i = 0; i < __instance.m_pokedexEntries.Count; i++)
                    {
                        UnityEngine.Object.Destroy(__instance.m_pokedexEntries[i].gameObject);
                    }
                    __instance.LastFocusTarget = null;
                    __instance.m_pokedexEntries.Clear();
                    __instance.InitializeEquipmentPageLeft();
                    CustomActions.OnAnyPageRefreshData?.Invoke(__instance);
                    if (__instance.m_pokedexEntries.Count > 0)
                    {
                        __instance.LastFocusTarget = __instance.m_pokedexEntries[0].GetComponent<dfButton>();
                    }
                    __instance.guiManager.UIScaleLegacyMode = true;
                    __instance.guiManager.UIScaleLegacyMode = false;
                }
                else if (__instance.pageType == AmmonomiconPageRenderer.PageType.DEATH_LEFT)
                {
                    __instance.InitializeDeathPageLeft();
                    CustomActions.OnAnyPageRefreshData?.Invoke(__instance);
                }
                else if (__instance.pageType == AmmonomiconPageRenderer.PageType.DEATH_RIGHT)
                {
                    __instance.InitializeDeathPageRight();
                    CustomActions.OnAnyPageRefreshData?.Invoke(__instance);
                }
                else
                {
                    for (int j = 0; j < __instance.m_pokedexEntries.Count; j++)
                    {
                        __instance.m_pokedexEntries[j].UpdateEncounterState();
                    }
                    CustomActions.OnAnyPageRefreshData?.Invoke(__instance);
                }
                return false;
            }
        }


        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.SetPageDataUnknown))]
        public class Patch_SetPageDataUnknown_Class
        {
            [HarmonyPrefix]
            private static bool SetPageDataUnknown(AmmonomiconPageRenderer __instance)
            {
                if (__instance == null)
                {
                    return false;
                }

                foreach (var entry in StaticData.customBookmarks)
                {
                    if (entry.Right == __instance.pageType)
                    {
                        entry.ammonomiconPageTag.OnSetDataUnknown(__instance);
                        return false;
                    }
                }
                return true;
            }
        }



        ///===================
        /// Func Patches
        ///====================
        #region Func Patches
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeEquipmentPageLeft))]
        public class PostPatch_InitializeEquipmentPageLeft_Class
        {
            [HarmonyPostfix]
            private static void InitializeEquipmentPageLeft(AmmonomiconPageRenderer __instance)
            {
                CustomActions.OnEquipmentPageRebuilt?.Invoke(__instance);
            }
        }

        [HarmonyPatch(typeof(AmmonomiconDeathPageController), nameof(AmmonomiconDeathPageController.InitializeLeftPage))]
        public class PostPatch_InitializeLeftPage_Class
        {
            [HarmonyPostfix]
            private static void InitializeLeftPage(AmmonomiconDeathPageController __instance)
            {
                CustomActions.OnDeathPageRebuiltLeft?.Invoke(__instance, __instance.isVictoryPage);
            }
        }
        [HarmonyPatch(typeof(AmmonomiconDeathPageController), nameof(AmmonomiconDeathPageController.InitializeRightPage))]
        public class PostPatch_InitializeRightPage_Class
        {
            [HarmonyPostfix]
            private static void InitializeRightPage(AmmonomiconDeathPageController __instance)
            {
                CustomActions.OnDeathPageRebuiltRight?.Invoke(__instance, __instance.isVictoryPage);
            }
        }


        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeEquipmentPageLeft))]
        public class PrePatch_InitializeEquipmentPageLeft_Class
        {
            [HarmonyPrefix]
            private static bool InitializeEquipmentPageLeft(AmmonomiconPageRenderer __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreEquipmentPageBuild?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeItemsPageLeft))]
        public class PrePatch_InitializeItemsPageLeft_Class
        {
            [HarmonyPrefix]
            private static bool InitializeItemsPageLeft(AmmonomiconPageRenderer __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreItemPageBuild?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeGunsPageLeft))]
        public class PrePatch_InitializeGunsPageLeft_Class
        {
            [HarmonyPrefix]
            private static bool InitializeGunsPageLeft(AmmonomiconPageRenderer __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreGunPageBuild?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeEnemiesPageLeft))]
        public class PrePatch_InitializeEnemiesPageLeft_Class
        {
            [HarmonyPrefix]
            private static bool InitializeEnemiesPageLeft(AmmonomiconPageRenderer __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreEnemyPageBuild?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconPageRenderer), nameof(AmmonomiconPageRenderer.InitializeBossesPageLeft))]
        public class PrePatch_InitializeBossesPageLeft_Class
        {
            [HarmonyPrefix]
            private static bool InitializeBossesPageLeft(AmmonomiconPageRenderer __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreBossPageBuild?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }

        [HarmonyPatch(typeof(AmmonomiconDeathPageController), nameof(AmmonomiconDeathPageController.InitializeLeftPage))]
        public class PrePatch_InitializeLeftPage_Class
        {
            [HarmonyPrefix]
            private static bool InitializeLeftPage(AmmonomiconDeathPageController __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreDeathPageBuildLeft?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }
        [HarmonyPatch(typeof(AmmonomiconDeathPageController), nameof(AmmonomiconDeathPageController.InitializeRightPage))]
        public class PrePatch_InitializeRightPage_Class
        {
            [HarmonyPrefix]
            private static bool InitializeRightPage(AmmonomiconDeathPageController __instance)
            {
                bool Invokes = true;
                var invokers = CustomActions.OnPreDeathPageBuildRight?.GetInvocationList();
                if (invokers != null)
                {
                    foreach (var entry in invokers)
                    {
                        Invokes = (bool)entry.DynamicInvoke(__instance);
                    }
                }
                return Invokes;
            }
        }
        #endregion
    }
}
