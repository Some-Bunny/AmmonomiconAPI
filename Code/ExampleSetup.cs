using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace AmmonomiconAPI.Code
{
    class ExampleSetup
    {
        static void ExampleSetupMyPage()
        {
            var spriteContainer = new SpriteContainer();
            spriteContainer.AppearFrames = new string[]
            {
                "ExampleFramePath_1_Appear",
                "ExampleFramePath_2_Appear",
                "ExampleFramePath_3_Appear",
                "ExampleFramePath_4_Appear",               
            };
            spriteContainer.SelectFrames = new string[]
            {
                "ExampleFramePath_1_Select",
                "ExampleFramePath_2_Select",
                "ExampleFramePath_3_Select",
            };
            spriteContainer.HoverFrame = "ExampleFramePath_1_Hover";
            spriteContainer.SelectHoverFrame = "ExampleFramePath_1_SelectHover";

            //spriteContainer.customDFAtlas = MyAtlas;  ///Only use this if you either have a custom atlas, or have already added your sprites to an atlas.
            //If you do use an atlas, instead of filepaths, you use the names of the sprites in the atlas instead
            UIBuilder.BuildBookmark("ExamplePrefix", "ExampleEnumNames", new ExampleCustomAmmonomiconPageController(), spriteContainer, Assembly.GetExecutingAssembly());
        }

        public class ExampleCustomAmmonomiconPageController : CustomAmmonomiconPageController
        {
            public ExampleCustomAmmonomiconPageController() : base("ExampleName", 3, false, "") { }

            public override List<EncounterDatabaseEntry> GetEntriesForPage(AmmonomiconPageRenderer renderer)
            {
                ///All sprites used by the Ammonomicon for entries MUST be in the Ammonomicon Collection.
                List<EncounterDatabaseEntry> entries = new List<EncounterDatabaseEntry>();
                entries.Add(
                    HelperTools.CreateDummyEncounterDatabaseEntry(
                        HelperTools.CreateJournalEntryData("ExampleEntryName", "YourAmmonomiconSprite", "Tagline", "OOOOOOOOOO thats a lot")));
                /// return a list of entries which will then be filled out and added to your page. You can make custom EncounterDatabaseEntry for whatever you desire using the methods in HelperTools.

                ///if you want to add custom text to the 2nd tape line of your entry, there are 2 methods
                ///
                /// First Method: will automatically generate a key without your input for your entry.
                entries.Add(
                    HelperTools.CreateDummyEncounterDatabaseEntry("Your Second Tape Line",
                        HelperTools.CreateJournalEntryData("ExampleEntryName", "YourAmmonomiconSprite", "Tagline", "text text text")));


                /// Second Method: Create your own key so you can re-use it for multiple entries without adding duplicate text to the database
                string my2ndKeyText = "hey this is a 2nd key";
                int My2ndTapeKey = my2ndKeyText.To2ndTapeDatabase(); 

                /// HelperTools.To2ndTapeDatabase(my2ndKeyText); also works

                entries.Add(
                    HelperTools.CreateDummyEncounterDatabaseEntry(My2ndTapeKey,
                        HelperTools.CreateJournalEntryData("ExampleEntryName", "YourAmmonomiconSprite", "Tagline", "text text text")));

                return entries;
            }
            public override bool ShouldBeActive()
            {
                ///If returns false, your bookmark wont be active when you open the Ammonomicon.
                return true;
            }
            ///Theres a lot more overridable actions here for extra customizability, but not really necessary unless you wanna do something advanced.
        }


        static void ExampleAction()
        {
            ///Runs when the equipment page is being built.
            AmmonomiconAPI.CustomActions.OnPreEquipmentPageBuild += BuildEquipmentPage;

        }

        static bool BuildEquipmentPage(AmmonomiconPageRenderer ammonomiconPageRenderer)
        {
            var c = ammonomiconPageRenderer.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
            if (c.Find("MyNewTab") == null)
            {
                var obj = AmmonomiconAPI.StaticData.HeaderObject;
                var newObject = UnityEngine.Object.Instantiate(AmmonomiconAPI.StaticData.ActiveItemsHeader, c);
                newObject.name = "MyNewTab";
                var label = newObject.GetComponentInChildren<dfLabel>();
                label.isLocalized = false;
                label.localizationKey = "";
                label.Text = "My Tab";
                var translator = label.gameObject.GetComponent<ConditionalTranslator>();
                translator.enabled = false;

                var lablePanel = newObject.GetComponent<dfPanel>();
                lablePanel.ZOrder = 9;

                var newObjectList = UnityEngine.Object.Instantiate(AmmonomiconAPI.StaticData.ItemsPanel, c);
                newObjectList.name = "MyNewPanel";
                newObjectList.ZOrder = 10;
            }

            List<EncounterDatabaseEntry> list2 = new List<EncounterDatabaseEntry>();
            if (list2.Count > 0)
            {
                var panel = c.Find("MyNewPanel").GetComponent<dfPanel>();
                dfPanel component3 = panel.transform.GetChild(0).GetComponent<dfPanel>();
                //Generates new boxes from the given transform.
                ammonomiconPageRenderer.StartCoroutine(ammonomiconPageRenderer.ConstructRectanglePageLayout(component3, list2, new Vector2(12f, 16f), new Vector2(8f, 8f), true, null));
                panel.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal);
                panel.Height = component3.Height;
                component3.Height = panel.Height;
            }
            //If returns false, prevents the rest of the original method from running
            return true;
        }

    }
}
