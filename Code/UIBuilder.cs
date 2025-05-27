using Alexandria.CharacterAPI;
using Alexandria.ItemAPI;
using AmmonomiconAPI.Code.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Alexandria;
using static dfAtlas;

namespace AmmonomiconAPI
{
    public class UIBuilder
    {

        public static string GetItemNamesFromIdList(string baseString, string prefix, List<int> list)
        {
            if (list?.Count > 0)
            {
                prefix += list.Count > 1 ? "s: " : ": ";

                baseString += prefix;
                for (int i = 0; i < list.Count; i++)
                {
                    baseString += PickupObjectDatabase.GetById(list[i]).EncounterNameOrDisplayName + (i == list.Count - 1 ? "." : ", ");
                }
                baseString += "\n";
            }
            return baseString;
        }
        /// <summary>
        /// Creates a bookmark that can be accessed in the Ammonomicon.
        /// </summary>
        /// <param name="ModPrefix">The prefix of your mod.</param>
        /// <param name="EnumName">The name of the Enum that will be used by the bookmark (NO SPACES).</param>
        /// <param name="CustomTagData">AmmonomiconPageTag is a custom class that has multiple methods that can be overwritten to modify/customize your ammonomicon pages. Necessary to do for custom entries.</param>
        /// <param name="spriteContainer">A class that handles the sprites used by your bookmark.</param>
        /// <param name="assemblyToUse">The assembly to use.</param>
        public static void BuildBookmark(string ModPrefix, string EnumName, CustomAmmonomiconPageController CustomTagData, SpriteContainer spriteContainer, Assembly assemblyToUse = null)
        {
            assemblyToUse = assemblyToUse ?? Assembly.GetExecutingAssembly();
            var PageLeft = ETGModCompatibility.ExtendEnum<AmmonomiconPageRenderer.PageType>(AmmonomiconAPIModule.GUID, $"{ModPrefix}_{EnumName}_(PageLeft)");
            var PageRight = ETGModCompatibility.ExtendEnum<AmmonomiconPageRenderer.PageType>(AmmonomiconAPIModule.GUID, $"{ModPrefix}_{EnumName}_(PageRight)");

            StaticData.AllCustomEnums.Add(PageLeft);
            StaticData.AllCustomEnums.Add(PageRight);

            //var ji = ((GameObject)BraveResources.Load("Ammonomicon Controller", ".prefab")).GetComponent<AmmonomiconController>();

            //var Manager = ji.AmmonomiconBasePrefab.GetComponent<AmmonomiconInstanceManager>();


            var baseBookmark = StaticData.InstanceManagerCached.bookmarks[2];


            //AmmonomiconBookmarkController();//ji.bookmarks[2];

            var dumbObj = FakePrefab.Clone(baseBookmark.gameObject);
            dumbObj.SetActive(false);
            
            if (dumbObj.transform.parent) { UnityEngine.Object.Destroy(dumbObj.transform.parent.gameObject); }
            dumbObj.transform.parent = null;

            //UnityEngine.Object.Destroy(obj.gameObject);
            //UnityEngine.Object.Destroy(Manager.gameObject);



            AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();

            tabController2.LeftPageType = PageLeft;
            tabController2.RightPageType = PageRight;//(AmmonomiconPageRenderer.PageType)CustomPageType.MODS_RIGHT;
            // (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_LEFT;

            var key = dumbObj.AddComponent<AmmonomiconPageKey>();
            key.UniqueKey = EnumName;
            key.bookmarkController = tabController2;
            
            key.Left = PageLeft;
            key.Right = PageRight;
            key.ammonomiconPageTag = CustomTagData ?? new CustomAmmonomiconPageController("[ERR]: NULL AmmonomiconPageTag", 1);


            var button = dumbObj.GetComponent<dfButton>();
            key.dfButton = button;
            tabController2.m_sprite = button;

            //tabController2.m_sprite.autoSize = true;
            //tabController2.m_sprite.AutoSize = true;
            //tabController2.m_sprite.cachedPixelSize = InstancePlusManager.CACHED_PIXEL_SIZE;// ammonomiconInstanceManager.bookmarks[2].m_sprite.cachedPixelSize;


            var dfAnimator = dumbObj.GetComponent<dfSpriteAnimation>();
            tabController2.m_animator = dfAnimator;

            

            tabController2.gameObject.name = $"{ModPrefix}_{EnumName}";
            if (spriteContainer != null)
            {
                if (spriteContainer.customDFAtlas == null)
                {
                    var atlas = StaticData.AmmonomiconAtlas;

                    var itemInfo = new List<string>();
                    var item_Hover = atlas.AddNewItemToAtlas(Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource(spriteContainer.HoverFrame, assemblyToUse), spriteContainer.HoverFrame);
                    var item_SelectHover = atlas.AddNewItemToAtlas(Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource(spriteContainer.SelectHoverFrame, assemblyToUse), spriteContainer.SelectHoverFrame);

                    tabController2.SelectSpriteName = item_Hover.name;
                    tabController2.DeselectSelectedSpriteName = item_SelectHover.name;



                    var animationController = new GameObject("AmmonomiconBookmarkAnimationController");
                    animationController.transform.parent = dumbObj.transform;


                    ///===== Hover Clip =====
                    var HoverClip = animationController.AddComponent<dfAnimationClip>();
                    HoverClip.Atlas = baseBookmark.AppearClip.Atlas;                
                    foreach (var entry in spriteContainer.AppearFrames)
                    {
                        itemInfo.Add(atlas.AddNewItemToAtlas(Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource(entry, assemblyToUse), entry).name);
                    }
                    button.backgroundSprite = itemInfo.Last();

                    HoverClip.sprites = itemInfo;
                    ///=====

                    ///===== Select Hover Clip =====
                    var itemInfo2 = new List<string>();
                    var SelectHoverClip = animationController.AddComponent<dfAnimationClip>();
                    SelectHoverClip.Atlas = baseBookmark.AppearClip.Atlas;
                    foreach (var entry in spriteContainer.SelectFrames)
                    {
                        itemInfo2.Add(atlas.AddNewItemToAtlas(Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource(entry, assemblyToUse), entry).name);
                    }
                    SelectHoverClip.sprites = itemInfo2;
                    ///=====

                    tabController2.AppearClip = HoverClip;
                    tabController2.SelectClip = SelectHoverClip;


                    if (spriteContainer.BackingIcon != string.Empty)
                    {
                         key.DFGUI_BackingImage = atlas.AddNewItemToAtlas(Alexandria.ItemAPI.ResourceExtractor.GetTextureFromResource(spriteContainer.BackingIcon, assemblyToUse), spriteContainer.BackingIcon).name;
                    }


                }
                else
                {
                    button.Atlas = spriteContainer.customDFAtlas;

                    var animationController = new GameObject("AmmonomiconBookmarkAnimationController");
                    animationController.transform.parent = dumbObj.transform;

                    ///===== Appear =====
                    var AppearClip = animationController.AddComponent<dfAnimationClip>();
                    AppearClip.Atlas = spriteContainer.customDFAtlas;
                    AppearClip.sprites = spriteContainer.AppearFrames.ToList();
                    tabController2.AppearClip = AppearClip;
                    ///=====



                    ///===== Select Hover =====
                    var SelectHoverClip = animationController.AddComponent<dfAnimationClip>();
                    SelectHoverClip.Atlas = spriteContainer.customDFAtlas;
                    SelectHoverClip.sprites = spriteContainer.SelectFrames.ToList();
                    tabController2.SelectClip = SelectHoverClip;
                    ///=====

 

                    button.backgroundSprite = spriteContainer.AppearFrames.Last();
                    tabController2.DeselectSelectedSpriteName = spriteContainer.SelectHoverFrame;
                    tabController2.SelectSpriteName = spriteContainer.HoverFrame;

                    if (spriteContainer.BackingIcon != string.Empty)
                    {
                        key.DFGUI_BackingImage = spriteContainer.BackingIcon;
                        key.CustomAtlas = spriteContainer.customDFAtlas;
                    }
                }
            }

            //1967693681992645534
            //tabController2.DeselectSelectedSpriteName = "bookmark_beyond_select_hover_001";
            //tabController2.SelectSpriteName = "bookmark_beyond_hover_001";

            //FieldInfo _sprites = typeof(dfAnimationClip).GetField("sprites", BindingFlags.NonPublic | BindingFlags.Instance);



            //var beyondClipObj2 = new GameObject("AmmonomiconBookmarkBeyondSelectHover");
            //FakePrefab.MarkAsFakePrefab(beyondClipObj2);
            //var beyondClip2 = beyondClipObj.AddComponent<dfAnimationClip>();

            //beyondClip2.Atlas = self.bookmarks[2].AppearClip.Atlas;

            //_sprites.SetValue(beyondClip2, new List<string> { "bookmark_beyond_select_001", "bookmark_beyond_select_002", "bookmark_beyond_select_003" });

            //tabController2.TargetNewPageLeft = "Global Prefabs/Ammonomicon Pages/Beyond Page Left";
            //tabController2.TargetNewPageRight = "Global Prefabs/Ammonomicon Pages/Info Page Right";

            //tabController2.RightPageType = AmmonomiconPageRenderer.PageType.ITEMS_RIGHT;
            //tabController2.LeftPageType = AmmonomiconPageRenderer.PageType.ITEMS_LEFT;
            //tabController2.AppearClip = beyondClip;
            //tabController2.SelectClip = beyondClip2;
            //Tools.Log("9.5");

            /*FieldInfo m_sprite = typeof(AmmonomiconBookmarkController).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
            m_sprite.SetValue(tabController2, m_sprite.GetValue(self.bookmarks[2]) as dfButton);

            FieldInfo m_animator = typeof(AmmonomiconBookmarkController).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
            m_animator.SetValue(tabController2, m_animator.GetValue(self.bookmarks[2]) as dfSpriteAnimation);
            */
            //Tools.Log("10");




            StaticData.customBookmarks.Add(key);




            //ammonomiconBookmarks.Insert(ammonomiconBookmarks.Count - 1, tabController2);

            //ammonomiconBookmarks.Add(deathBookmark);

            //Tools.Log("11");
            //self.bookmarks = ammonomiconBookmarks.ToArray();

            //Tools.Log("12");



            //ItemAPI.FakePrefab.MarkAsFakePrefab(dumbObj);*/
            //self.bookmarks = ammonomiconBookmarks.ToArray();
            //Tools.Log("13");







            /*
            ETGModConsole.Log(5);

            var obj = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Ammonomicon Controller", ".prefab"));
            ETGModConsole.Log(51);

            var ji = obj.GetComponent<AmmonomiconController>();
            ETGModConsole.Log(511);


            var Manager = UnityEngine.Object.Instantiate(ji.AmmonomiconBasePrefab).GetComponent<AmmonomiconInstanceManager>();


            var baseBookmark = Manager.bookmarks[2];


            //AmmonomiconBookmarkController();//ji.bookmarks[2];
            ETGModConsole.Log(512);

            //var selectSprite = UIRootPrefab.Manager.DefaultAtlas.AddNewItemToAtlas(Tools.GetTextureFromResource(selectSpritePath + ".png"));
            //var deselectSelectedSprite = UIRootPrefab.Manager.DefaultAtlas.AddNewItemToAtlas(Tools.GetTextureFromResource(deselectSelectedSpritePath + ".png"));

            var dumbObj = FakePrefab.Clone(baseBookmark.gameObject);
            ETGModConsole.Log(52);

            //var myatlas = StaticSpriteDefinitions.PlanetsideUIAtlas;
            ETGModConsole.Log(53);


            AmmonomiconBookmarkController tabController2 = dumbObj.GetComponent<AmmonomiconBookmarkController>();
            ETGModConsole.Log(54);

            //Tools.Log("9");
            //dumbObj.transform.parent = baseBookmark.gameObject.transform.parent;
            //dumbObj.transform.position = baseBookmark.gameObject.transform.position;
            //dumbObj.transform.localPosition = new Vector3(0, -1.2f, 0);
            ETGModConsole.Log(6);

            tabController2.gameObject.name = name;

            //tabController2.SelectSpriteName = "bookmark_psog_hover_001";//selectSprite.name;//1967693681992645534
            //tabController2.DeselectSelectedSpriteName = "bookmark_psog_004";//deselectSelectedSprite.name;

            tabController2.TargetNewPageLeft = "Global Prefabs/Ammonomicon Pages/Equipment Page Left";
            tabController2.TargetNewPageRight = "Global Prefabs/Ammonomicon Pages/Equipment Page Right";
            tabController2.RightPageType = (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_RIGHT;
            tabController2.LeftPageType = (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_LEFT;

            tabController2.AppearClip = baseBookmark.AppearClip;
            tabController2.SelectClip = baseBookmark.SelectClip;
            //Tools.Log("9.5");
            FieldInfo m_sprite = typeof(AmmonomiconBookmarkController).GetField("m_sprite", BindingFlags.NonPublic | BindingFlags.Instance);
            //var thing = m_sprite.GetValue(baseBookmark) as dfButton;
            //m_sprite.SetValue(tabController2, thing);

            FieldInfo m_animator = typeof(AmmonomiconBookmarkController).GetField("m_animator", BindingFlags.NonPublic | BindingFlags.Instance);
            //var thing2 = m_animator.GetValue(baseBookmark) as dfSpriteAnimation;
            //m_animator.SetValue(tabController2, thing2);
            ETGModConsole.Log(7);

            dumbObj.SetActive(true);
            customBookmarks.Add(tabController2);
            */
        }

        

    }
}
