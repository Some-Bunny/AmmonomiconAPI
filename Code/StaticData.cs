using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AmmonomiconAPI.Code.Misc;

namespace AmmonomiconAPI
{
    public static class StaticData
    {
        public static void InitStaticData()
        {
            var atlas = StaticData.LoadAssetFromAnywhere<GameObject>("Ammonomicon Atlas").GetComponent<dfAtlas>();
            AmmonomiconAtlas = atlas;

            var ji = ((GameObject)BraveResources.Load("Ammonomicon Controller", ".prefab")).GetComponent<AmmonomiconController>();
            InstanceManagerCached = ji.AmmonomiconBasePrefab.GetComponent<AmmonomiconInstanceManager>();

            GameObject pageLeft = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(InstanceManagerCached.bookmarks[0].TargetNewPageLeft, ".prefab"));




            var pageLeftRenderer = pageLeft.GetComponentInChildren<AmmonomiconPageRenderer>();
            var panelMainLeft = pageLeftRenderer.transform.parent.GetComponent<dfGUIManager>().transform.Find("Scroll Panel");
            StaticData.HeaderObject = FakePrefab.Clone(panelMainLeft.Find("Header").gameObject).GetComponent<dfPanel>();
            StaticData.LeftPageFooter = FakePrefab.Clone(panelMainLeft.Find("Footer").gameObject).GetComponent<dfPanel>();
            var scroll2Left = panelMainLeft.Find("Scroll Panel");
            StaticData.ActiveItemsHeader = FakePrefab.Clone(scroll2Left.Find("Active Items Header").gameObject).GetComponent<dfPanel>();
            StaticData.GunsItemsHeader = FakePrefab.Clone(scroll2Left.Find("Guns Header").gameObject).GetComponent<dfPanel>();
            StaticData.ItemsHeader = FakePrefab.Clone(scroll2Left.Find("Passive Items Header").gameObject).GetComponent<dfPanel>();

            StaticData.ItemsPanel = FakePrefab.Clone(scroll2Left.Find("Passive Items Panel").gameObject).GetComponent<dfPanel>();



            GameObject pageRight = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load(InstanceManagerCached.bookmarks[0].TargetNewPageRight, ".prefab"));
            var pageRightRenderer = pageRight.GetComponentInChildren<AmmonomiconPageRenderer>();
            var panelMainRight = pageRightRenderer.transform.parent.GetComponent<dfGUIManager>().transform.Find("Scroll Panel");


            StaticData.HeaderObjectRightPage = FakePrefab.Clone(panelMainRight.Find("Header").gameObject).GetComponent<dfPanel>();
            StaticData.ThePhotoGraph = FakePrefab.Clone(panelMainRight.Find("ThePhoto").gameObject).GetComponent<dfPanel>();
            StaticData.RightPageDivider = FakePrefab.Clone(panelMainRight.Find("Divider").gameObject).GetComponent<dfPanel>();
            StaticData.RightPageFooter = FakePrefab.Clone(panelMainRight.Find("Footer").gameObject).GetComponent<dfPanel>();
            StaticData.RightPageScrollPanel = FakePrefab.Clone(panelMainRight.Find("Scroll Panel").gameObject).GetComponent<dfPanel>();

            StaticData.TapeLine = FakePrefab.Clone(panelMainRight.Find("Tape Line One").gameObject).GetComponent<dfPanel>();

            UnityEngine.Object.Destroy(pageLeft.gameObject);
            UnityEngine.Object.Destroy(pageRight.gameObject);
        }




        internal static AmmonomiconInstanceManager InstanceManagerCached;


        /// <summary>
        /// Copy of the Main Header in the Left Equipment Page. [1st Child | "Sprite" | dfSprite ] === [2nd Child | "Label 2" | dfLabel ] (Has multiple other label objects attached to it ["Label", "Label 2", "Label 3", "Label 4"])
        /// </summary>
        public static dfPanel HeaderObject;

        /// <summary>
        /// Copy of the Weapons Header in the Left Equipment Page. [1st Child | "Label" | dfLabel ] === [2nd Child | "Sliced Sprite" | dfSlicedSprite ] === [3rd Child | "Guns" | dfSlicedSprite ]
        /// </summary>
        public static dfPanel GunsItemsHeader;

        /// <summary>
        /// Copy of the Active Item Header in the Left Equipment Page. [1st Child | "Label" | dfLabel ] === [2nd Child | "Sprite" | dfSprite ] === [3rd Child | "Sliced Sprite" | dfSlicedSprite ]
        /// </summary>
        public static dfPanel ActiveItemsHeader;
        /// <summary>
        /// Copy of the Item Header in the Left Equipment Page. [1st Child | "Label" | dfLabel ] === [2nd Child | "Sprite" | dfSprite ] === [3rd Child | "Sliced Sprite" | dfSlicedSprite ]
        /// </summary>
        public static dfPanel ItemsHeader;

        /// <summary>
        /// Copy of the Item Header in the Left Equipment Page. [1st Child | "Label" | dfLabel ] === [2nd Child | "Sprite" | dfSprite ] === [3rd Child | "Sliced Sprite" | dfSlicedSprite ]
        /// </summary>
        public static dfPanel ItemsPanel;

        /// <summary>
        /// Copy of the Footer (Sprite at the bottom) in the Left Equipment Page. [1st Child | "Sprite" | dfSprite ] === [2nd Child | "Scrollbar" | dfScrollbar ]
        /// </summary>
        public static dfPanel LeftPageFooter;

        /// <summary>
        /// Copy of the Main Header in the Right Equipment Page. [1st Child | "Label" | dfLabel ] === [2nd Child | "Sprite" | dfSprite ]
        /// </summary>
        public static dfPanel HeaderObjectRightPage;
        /// <summary>
        /// Copy of the Photograph in the Right Equipment Page. [1st Child | "Photo" | dfSprite ](Has 2 children ["tk2dSpriteHolder" (Has 1 Child "ammonomicon tk2d sprite", which is a tk2dSprite. Also has 4 child tk2dSprite objects that serve as outlines.) | "ItemShadow" (dfSprite) ])  === [2nd Child | "Tape" | dfSprite ]
        /// </summary>
        public static dfPanel ThePhotoGraph;
        /// <summary>
        /// Copy of the Divider in the Right Equipment Page. [1st Child | "Sprite" | dfSprite ]
        /// </summary>
        public static dfPanel RightPageDivider;
        /// <summary>
        /// Copy of the Footer (Sprite at the bottom) in the Right Equipment Page. [1st Child | "Sprite" | dfSprite ] === [2nd Child | "Scrollbar" | dfScrollbar ]
        /// </summary>
        public static dfPanel RightPageFooter;
        /// <summary>
        /// Copy of the Scroll Panel (Used for Descriptions) in the Right Equipment Page. [1st Child | "Panel" | dfPanel ] (Has 1 Child ["Label" | dfLabel ])
        /// </summary>
        public static dfPanel RightPageScrollPanel;

        /// <summary>
        /// Copy of the Tape Panel in the Right Equipment Page. [1st Child | "Label" | dfLabel ] === [2nd Child | "Sliced Sprite" | "dfSlicedSprite" ]
        /// </summary>
        public static dfPanel TapeLine;

        public static dfAtlas AmmonomiconAtlas;
        public static List<AmmonomiconPageKey> customBookmarks = new List<AmmonomiconPageKey>();
        public static List<AmmonomiconPageRenderer.PageType> AllCustomEnums = new List<AmmonomiconPageRenderer.PageType>();





        public static T LoadAssetFromAnywhere<T>(string path) where T : UnityEngine.Object
        {
            T t = default(T);
            foreach (string path2 in BundlePrereqs)
            {
                try
                {
                    t = ResourceManager.LoadAssetBundle(path2).LoadAsset<T>(path);
                }
                catch
                {
                }
                bool flag = t != null;
                if (flag)
                {
                    break;
                }
            }
            return t;
        }

        // Token: 0x04000434 RID: 1076
        private static string[] BundlePrereqs = new string[]
        {
            "brave_resources_001",
            "dungeon_scene_001",
            "encounters_base_001",
            "enemies_base_001",
            "flows_base_001",
            "foyer_001",
            "foyer_002",
            "foyer_003",
            "shared_auto_001",
            "shared_auto_002",
            "shared_base_001",
            "dungeons/base_bullethell",
            "dungeons/base_castle",
            "dungeons/base_catacombs",
            "dungeons/base_cathedral",
            "dungeons/base_forge",
            "dungeons/base_foyer",
            "dungeons/base_gungeon",
            "dungeons/base_mines",
            "dungeons/base_nakatomi",
            "dungeons/base_resourcefulrat",
            "dungeons/base_sewer",
            "dungeons/base_tutorial",
            "dungeons/finalscenario_bullet",
            "dungeons/finalscenario_convict",
            "dungeons/finalscenario_coop",
            "dungeons/finalscenario_guide",
            "dungeons/finalscenario_pilot",
            "dungeons/finalscenario_robot",
            "dungeons/finalscenario_soldier"
        };

        //public static Dictionary<string, AmmonomiconPageRenderer> customPages = new Dictionary<string, AmmonomiconPageRenderer>();
        //public static Dictionary<AmmonomiconPageRenderer, AmmonomiconPageTag> customTags = new Dictionary<AmmonomiconPageRenderer, AmmonomiconPageTag>();


        internal static Dictionary<int, string> Stored2ndTapeTexts = new Dictionary<int, string>();
    }
}
