using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmmonomiconAPI.Code.Misc
{
    internal class OldCode
    {

        /*
        
                    //spriteContainer.IdleFrame
                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_001.png"), "bookmark_beyond_001");
                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_002.png"), "bookmark_beyond_002");
                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_003.png"), "bookmark_beyond_003");
                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_004.png"), "bookmark_beyond_004");

                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_hover_001.png"), "bookmark_beyond_hover_001");

                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_001.png"), "bookmark_beyond_select_001");
                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_002.png"), "bookmark_beyond_select_002");
                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_003.png"), "bookmark_beyond_select_003");

                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_hover_001.png"), "bookmark_beyond_select_hover_001");

                    //atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/Item_Picture_Beyond_001.png"), "Item_Picture_Beyond_001");
        */


        /*
                ETGModConsole.Log(1);
                /*
                UIRootPrefab = Tools.LoadAssetFromAnywhere<GameObject>("UI Root").GetComponent<GameUIRoot>();
                var atlas = Tools.LoadAssetFromAnywhere<GameObject>("Ammonomicon Atlas").GetComponent<dfAtlas>(); //AmmonomiconController.Instance.Ammonomicon.bookmarks[2].AppearClip.Atlas;
                //CollectionDumper.DumpdfAtlas(atlas);


                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_001.png"), "bookmark_beyond_001");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_002.png"), "bookmark_beyond_002");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_003.png"), "bookmark_beyond_003");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_004.png"), "bookmark_beyond_004");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_hover_001.png"), "bookmark_beyond_hover_001");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_001.png"), "bookmark_beyond_select_001");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_002.png"), "bookmark_beyond_select_002");
                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_003.png"), "bookmark_beyond_select_003");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/bookmark_beyond_select_hover_001.png"), "bookmark_beyond_select_hover_001");

                atlas.AddNewItemToAtlas(Tools.GetTextureFromResource("BotsMod/sprites/Ammonomicon/Item_Picture_Beyond_001.png"), "Item_Picture_Beyond_001");
                */
        /*
        StringHandler.AddDFStringDefinition("#AMMONOMICON_TEST", "TEST_PAGE");

        var page = FakePrefab.Clone(BraveResources.Load<GameObject>("Global Prefabs/Ammonomicon Pages/Guns Page Left", ".prefab"));
        if (page == null)
        {
            //Tools.Log("Clone is returning a null object", "#eb1313");
            return;
        }

        page.name = "Test Page Left";
        ETGModConsole.Log(2);



        var renderer = page.GetComponentInChildren<AmmonomiconPageRenderer>();
        ETGModConsole.Log(21);

        //Tools.Log("h", "#eb1313");
        foreach (var child in page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").gameObject.GetComponents<Component>())
        {
            //Tools.Log(child.ToString());
        }
        ETGModConsole.Log(22);
        */
        /*
        page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
        //Tools.Log("1", "#eb1313");
        ETGModConsole.Log(23);

        page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
        //Tools.Log("2", "#eb1313");
        ETGModConsole.Log(24);

        page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label 2").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
        //Tools.Log("3", "#eb1313");
        ETGModConsole.Log(25);

        page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label 3").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
        //Tools.Log("4", "#eb1313");
        ETGModConsole.Log(26);

        page.transform.Find("Scroll Panel").Find("Header").Find("Label 4").Find("Label 4").gameObject.GetComponent<dfLabel>().Text = "#AMMONOMICON_BEYOND";
        */
        /*
        ETGModConsole.Log(27);

        foreach (Transform child in page.transform.Find("Scroll Panel").Find("Scroll Panel").Find("Guns Panel").GetChild(0))
        {
            //UnityEngine.Object.Destroy(child.gameObject);
        }
        ETGModConsole.Log(28);

        renderer.pageType = (AmmonomiconPageRenderer.PageType)CustomPageType.MODS_LEFT;
        ETGModConsole.Log(29);

        //Tools.Log("a", "#eb1313");


        customPages.Add("Global Prefabs/Ammonomicon Pages/" + page.name, renderer);
        ETGModConsole.Log(3);
        */
    }
}
