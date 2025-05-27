using BepInEx;
using Alexandria;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Collections;
using System.Reflection;
using AmmonomiconAPI.Code;

namespace AmmonomiconAPI
{
    [BepInDependency(Alexandria.Alexandria.GUID)] 
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class AmmonomiconAPIModule : BaseUnityPlugin
    {
        public const string GUID = "bobot_and_bunny.etg.ammonomiconapi";
        public const string NAME = "[[ AmmonomiconAPI ]]";
        public const string VERSION = "1.0.0";
        public const string TEXT_COLOR = "#FF0000";

        public void Start()
        {
            new Harmony(GUID).PatchAll();
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            StaticData.InitStaticData();
            AmmonomiconAPIHooks.Init();
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
            GameManager.Instance.StartCoroutine(FrameDelay());
        }

        public IEnumerator FrameDelay()
        {
            yield return null;
            yield break;
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}

/*
public static void OpenHook(Action<AmmonomiconInstanceManager> orig, AmmonomiconInstanceManager self)
{
    try
    {

        //Debug.Log("OpenHook_!");

        //Ammonomicon.BuildBookmark("Mods", "BotsMod/sprites/wip", "BotsMod/sprites/wip");





        bool dosetup = true;
        bool dosetup1 = true;
        //scroll.onChildControlInvalidatedLayout();
        //scroll.OnSizeChanged();
        //scroll.AutoArrange();

        /*
        foreach (var mark in self.bookmarks)
        {
            if (mark.name.Contains("Spells"))
            {

                //dosetup = false;
                break;
            }

        }
        */
        //dosetup = false;
        /*
        if (dosetup == true)
        {
            /*
            foreach (var entry in AmmonomiconPageInitialization.customBookmarks)
            {
                var dumbObj = UnityEngine.Object.Instantiate(entry);
                AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();
                dumbObj.transform.parent = self.bookmarks.Last().gameObject.transform.parent;
                dumbObj.transform.position = self.bookmarks.Last().gameObject.transform.position;
                dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);
            }
            */
        //}
        /*
        List<AmmonomiconBookmarkController> ammonomiconBookmarks = self.bookmarks.ToList();

        if (dosetup1 == false)
        {
            var dumbObj = FakePrefab.Clone(self.bookmarks[ammonomiconBookmarks.Count - 1].gameObject);


            AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();

            //Tools.Log("9");
            dumbObj.transform.parent = self.bookmarks[2].gameObject.transform.parent;
            dumbObj.transform.position = self.bookmarks[2].gameObject.transform.position;
            dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);

            tabController2.gameObject.name = "Spells";
            //1967693681992645534
            //tabController2.DeselectSelectedSpriteName = "bookmark_beyond_select_hover_001";
            //tabController2.SelectSpriteName = "bookmark_beyond_hover_001";

            FieldInfo _sprites = typeof(dfAnimationClip).GetField("sprites", BindingFlags.NonPublic | BindingFlags.Instance);

            var beyondClipObj = new GameObject("AmmonomiconBookmarkBeyondHover");
            FakePrefab.MarkAsFakePrefab(beyondClipObj);
            var beyondClip = beyondClipObj.AddComponent<dfAnimationClip>();
            beyondClip.Atlas = self.bookmarks[2].AppearClip.Atlas;

            _sprites.SetValue(beyondClip, new List<string> { "bookmark_beyond_001", "bookmark_beyond_002", "bookmark_beyond_003", "bookmark_beyond_004" });

            var beyondClipObj2 = new GameObject("AmmonomiconBookmarkBeyondSelectHover");
            FakePrefab.MarkAsFakePrefab(beyondClipObj2);
            var beyondClip2 = beyondClipObj.AddComponent<dfAnimationClip>();
            beyondClip2.Atlas = self.bookmarks[2].AppearClip.Atlas;

            _sprites.SetValue(beyondClip2, new List<string> { "bookmark_beyond_select_001", "bookmark_beyond_select_002", "bookmark_beyond_select_003" });

            tabController2.TargetNewPageLeft = "Global Prefabs/Ammonomicon Pages/Beyond Page Left";
            tabController2.TargetNewPageRight = "Global Prefabs/Ammonomicon Pages/Info Page Right";
            //tabController2.RightPageType = (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_RIGHT;
            //tabController2.RightPageType = AmmonomiconPageRenderer.PageType.ITEMS_RIGHT;
            //tabController2.LeftPageType = (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_LEFT;
            //tabController2.LeftPageType = AmmonomiconPageRenderer.PageType.ITEMS_LEFT;
            tabController2.AppearClip = beyondClip;
            tabController2.SelectClip = beyondClip2;
            //Tools.Log("9.5");

            /*FieldInfo m_sprite = typeof(AmmonomiconBookmarkController).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
            m_sprite.SetValue(tabController2, m_sprite.GetValue(self.bookmarks[2]) as dfButton);

            FieldInfo m_animator = typeof(AmmonomiconBookmarkController).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
            m_animator.SetValue(tabController2, m_animator.GetValue(self.bookmarks[2]) as dfSpriteAnimation);
            */
            //Tools.Log("10");
            /*
            if (dumbObj.GetComponent<dfButton>() == null)
            {
                ETGModConsole.Log("ERR");

                //Tools.Log("dfButton nulled :(");
            }

            dumbObj.GetComponent<dfButton>().BackgroundSprite = "bookmark_beyond_004";

            ammonomiconBookmarks.Insert(ammonomiconBookmarks.Count - 1, tabController2);

            //ammonomiconBookmarks.Add(deathBookmark);

            //Tools.Log("11");
            //self.bookmarks = ammonomiconBookmarks.ToArray();

            //Tools.Log("12");

            dumbObj.SetActive(true);
            //ItemAPI.FakePrefab.MarkAsFakePrefab(dumbObj);*/
            //self.bookmarks = ammonomiconBookmarks.ToArray();
            //Tools.Log("13");


/*

            foreach (Transform bookmark in tabController2.transform.parent)
            {
                //Tools.Log(bookmark.gameObject.name);
            }
        }

        self.m_currentlySelectedBookmark = 0;
        self.StartCoroutine(self.HandleOpenAmmonomicon());
        //orig(self);
        //Tools.Log("8");

    }

    catch (Exception e)
    {
        ETGModConsole.Log(e);
        //Tools.Log("Ammonomicon broken :(", "#eb1313");
        //Tools.Log(string.Format(e + ""), "#eb1313");
    }
}
*/