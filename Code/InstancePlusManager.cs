using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;
using System.Reflection.Emit;
using static PrototypeRoomExit;
using static AmmonomiconPageRenderer;
using System.Collections;
using AmmonomiconAPI.Code.Misc;

namespace AmmonomiconAPI
{

    [HarmonyPatch(typeof(AmmonomiconInstanceManager), nameof(AmmonomiconInstanceManager.Open))]
    public class NopeOutVisibility
    {
       
        static bool Prefix(AmmonomiconInstanceManager __instance)
        {
            
            InstancePlusManager component = __instance.GetComponent<InstancePlusManager>();

            if (component == null) {
                Debug.Log("[AmmonomiconAPI] Adding new InstancePlusManager!");
                component = __instance.gameObject.AddComponent<InstancePlusManager>();
                component.InitInstance(__instance);
            }
        

            
            
            List<AmmonomiconBookmarkController> ammonomiconBookmarks = __instance.bookmarks.ToList();
            var scroll = __instance.bookmarks[0].transform.parent.GetComponent<dfScrollPanel>();
            var instancePlus = __instance.GetComponent<InstancePlusManager>();
            if (instancePlus != null)
            {
                //Debug.Log("[AmmonomiconAPI] InstancePlusManager found!");
                foreach (var bookmark in StaticData.customBookmarks)
                {
                    if (instancePlus.AttemptAddBookmark(bookmark.bookmarkController, bookmark.bookmarkController.GetComponent<AmmonomiconPageKey>()) == true)
                    {



                        var dumbObj = UnityEngine.Object.Instantiate(bookmark);
                        dumbObj.gameObject.SetActive(true);
                        AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();
                        bookmark.ammonomiconPageTag.OnFirstAdd(tabController2);
                        dumbObj.transform.parent = __instance.bookmarks[0].gameObject.transform.parent;
                        dumbObj.transform.position = __instance.bookmarks[0].gameObject.transform.position;
                        dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);
                        var button = dumbObj.GetComponent<dfButton>();
                        tabController2.m_sprite = button;

                        instancePlus.AllBookmarks.Add(tabController2);
                        instancePlus.AllKeys.Add(bookmark.UniqueKey, tabController2);

                        button.cachedManager = __instance.bookmarks[0].m_sprite.cachedManager;
                        button.size = new Vector2(86, 64);
                        button.startSize = new Vector2(86, 64); //new Vector2(__instance.bookmarks[0].m_sprite.startSize.x, __instance.bookmarks[2].m_sprite.startSize.y);
                        button.cachedPixelSize = __instance.bookmarks[0].m_sprite.cachedPixelSize;
                        button.ZOrder = bookmark.ammonomiconPageTag.ZOrder;
                        //button.OnLocalize();
                        button.OnSizeChanged();
                        AmmonomiconPageKey pageKey = dumbObj.GetComponent<AmmonomiconPageKey>();
                        pageKey.instancePlusManager = instancePlus;
                        //instancePlusManager
                        //self.bookmarks.Concat(ammonomiconBookmarks);
                        ammonomiconBookmarks.Insert(ammonomiconBookmarks.Count - 2, tabController2);

                        button.MouseEnter += instancePlus.OnMouseEnter;
                        button.MouseLeave += instancePlus.OnMouseLeave;



                        //dumbObj.Enable();
                        //ammonomiconBookmarks.Add(deathBookmark);

                        //Tools.Log("11");
                        //self.bookmarks = ammonomiconBookmarks.ToArray();

                        //Tools.Log("12");

                        //dumbObj.gameObject.SetActive(true);
                        //ItemAPI.FakePrefab.MarkAsFakePrefab(dumbObj);

                        //scroll.OnControlAdded(button);
                        //AmmonomiconController.Instance
                        //AmmonomiconPageRenderer ammonomiconPageRenderer = AmmonomiconController.Instance.LoadPageUIAtPath(__instance.bookmarks[2].TargetNewPageLeft, bookmark.Left, true, false);
                        //ammonomiconPageRenderer.pageType = bookmark.Left;


                        //AmmonomiconPageRenderer ammonomiconPageRenderer2 = AmmonomiconController.Instance.LoadPageUIAtPath(__instance.bookmarks[2].TargetNewPageRight, bookmark.Right, true, false);
                        //ammonomiconPageRenderer.pageType = bookmark.Right;

                        //ammonomiconPageRenderer.transform.parent.parent = __instance.transform.parent;
                        //ammonomiconPageRenderer2.transform.parent.parent = __instance.transform.parent;
                        //ammonomiconBookmarks.Insert(ammonomiconBookmarks.Count - 2, bookmark);
                    }
                }
                instancePlus.UpdateSizes();
                __instance.bookmarks = ammonomiconBookmarks.ToArray();

                __instance.m_currentlySelectedBookmark = 0;
                __instance.StartCoroutine(HandleOpenAmmonomiconHook(__instance));
            }
            else
            {
                Debug.Log("[AmmonomiconAPI] InstancePlusManager not FOUND! This should not happen!");
            }


            return false;
        }
        public static IEnumerator HandleOpenAmmonomiconHook(AmmonomiconInstanceManager self)
        {


            float waitPer = 0.3f / (float)(self.bookmarks.Length);



            dfGUIManager.SetFocus(null, true);
            
            self.bookmarks[self.m_currentlySelectedBookmark].IsCurrentPage = true;
            var inst = self.GetComponent<InstancePlusManager>();
            inst?.UpdateSizes();


            for (int currentBookmark = 0; currentBookmark < self.bookmarks.Length; currentBookmark++)
            {

                if (!AmmonomiconController.Instance.IsOpen)
                {
                    yield break;
                }
                bool isCustom = false;
                if (self.bookmarks[currentBookmark].LeftPageType != PageType.DEATH_LEFT && self.bookmarks[currentBookmark].LeftPageType != PageType.DEATH_RIGHT)
                {
                    inst?.UpdateSizes();

                    var currentBookmarkInst = self.bookmarks[currentBookmark];

                    foreach (var entry in StaticData.customBookmarks)
                    {
                        if (currentBookmarkInst.LeftPageType == entry.Left && currentBookmarkInst.RightPageType == entry.Right)
                        {
                            isCustom = true;
                            if (entry.ammonomiconPageTag.ShouldBeActive() == false)
                            {
                                currentBookmarkInst.Disable();
                                var scroller = currentBookmarkInst.transform.parent.GetComponent<dfScrollPanel>();
                                scroller.Localize();
                                entry.ammonomiconPageTag.OnDeactivate();
                            }
                            else
                            {
                                entry.ammonomiconPageTag.OnActivate();
                                currentBookmarkInst.TriggerAppearAnimation();
                            }
                        }
                    }
                    if (isCustom == false)
                    {
                        self.bookmarks[currentBookmark].TriggerAppearAnimation();
                    }
                    yield return self.StartCoroutine(self.InvariantWait(waitPer));
                }
            }
            self.bookmarks[self.m_currentlySelectedBookmark].IsCurrentPage = true;

            yield return null;
        }

    }



    public class InstancePlusManager : MonoBehaviour
    {
        public List<AmmonomiconBookmarkController> AllBookmarks;
        public Dictionary<string, AmmonomiconBookmarkController> AllKeys;
        public AmmonomiconInstanceManager ammonomiconInstanceManager;
        public void InitInstance(AmmonomiconInstanceManager inst)
        {
            ammonomiconInstanceManager = inst;
            AllBookmarks = new List<AmmonomiconBookmarkController>();
            AllBookmarks.AddRange(inst.bookmarks.ToList());
            AllKeys = new Dictionary<string, AmmonomiconBookmarkController>();
            foreach (var entry in AllBookmarks)
            {
                AllKeys.Add(entry.name, entry);
                entry.m_sprite.MouseEnter += this.OnMouseEnter;
                entry.m_sprite.MouseLeave += this.OnMouseLeave;
            }
        }

        public float PixelSize = 0.0017f;

        public bool AttemptAddBookmark(AmmonomiconBookmarkController ammonomiconBookmarkController, AmmonomiconPageKey key)
        {
            if (key != null)
            {
                if (!AllKeys.ContainsKey(key.UniqueKey))
                {
                    return true;
                }
            }
            else
            {
                if (!AllKeys.ContainsKey(ammonomiconBookmarkController.name))
                {
                    AllBookmarks.Add(ammonomiconBookmarkController);
                    AllKeys.Add(ammonomiconBookmarkController.name, ammonomiconBookmarkController);
                    return true;
                }
            }
            return false;
        }


        public void UpdateSizes()
        {
            float amountOfMarks = SizeCalc;
            //Debug.Log($"Size: {amountOfMarks} | {6f / Count}");
            var panel = AllBookmarks[0].transform.parent.GetComponent<dfScrollPanel>();
            panel.ScrollPadding.left = (int)(86f * (1f - amountOfMarks));
            float padding = 60f * (1f - amountOfMarks);

            for (int i = AllBookmarks.Count() - 1; i > -1; i--)
            {

                var entry = AllBookmarks[i];

                entry.m_sprite.maxSize = new Vector2(-1, -1);

                entry.m_sprite.size = new Vector2(86, 64) * amountOfMarks;
                entry.m_sprite.OnSizeChanged();

                //entry.m_sprite.cachedPixelSize = CACHED_PIXEL_SIZE;
            }
        }
        public float SizeCalc
        {
            get
            {
                float Count = 0;
                foreach (var entry in AllBookmarks)
                {
                    if (entry.LeftPageType != AmmonomiconPageRenderer.PageType.DEATH_LEFT) { Count++; continue; }
                    var s = entry.GetComponent<AmmonomiconPageKey>();
                    if (s)
                    {
                        if (s.ammonomiconPageTag.ShouldBeActive())
                        {
                            Count++;
                        }
                    }
                }
                return Mathf.Min((6f / Count)*0.96f, 1);
            }
        }

        public float SizeCalc_Size
        {
            get
            {
                float Count = 0;
                foreach (var entry in AllBookmarks)
                {
                    if (entry.LeftPageType != AmmonomiconPageRenderer.PageType.DEATH_LEFT) { Count++; continue; }
                    var s = entry.GetComponent<AmmonomiconPageKey>();
                    if (s)
                    {
                        if (s.ammonomiconPageTag.ShouldBeActive())
                        {
                            Count++;
                        }
                    }
                }
                return Mathf.Min((12f / Count) * 0.96f, 1);
            }
        }



        public Vector2 Size;
        public Dictionary<dfControl, Coroutine> keyValuePairs = new Dictionary<dfControl, Coroutine>();
        public void OnMouseEnter(dfControl control, dfMouseEventArgs mouseEvent)
        {
            float amountOfMarks = SizeCalc_Size;
            if (amountOfMarks >= 1) { return; }
            control.Size = SizeConst  * (1 / amountOfMarks * 0.8f);

            control.OnSizeChanged();
        }
        public Vector2 SizeConst = new Vector2(86, 64);
        public void OnMouseLeave(dfControl control, dfMouseEventArgs mouseEvent)
        {
            float amountOfMarks = SizeCalc_Size;
            if (amountOfMarks >= 1) { return; }
            control.Size = SizeConst * amountOfMarks;

            control.OnSizeChanged();
        }
    }
}
