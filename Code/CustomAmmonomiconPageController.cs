using HutongGames.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AmmonomiconAPI
{


    public class CustomAmmonomiconPageController
    {
        private List<EncounterDatabaseEntry> AdditionalEntries = new List<EncounterDatabaseEntry>();
        public void AddAdditionalEntry(EncounterDatabaseEntry encounterDatabaseEntry)
        {
            AdditionalEntries.Add(encounterDatabaseEntry);
        }


        /// <summary>
        /// The initialization of the CustomAmmonomiconPageController, used by AmmonomiconAPI to create a custom page.
        /// </summary>
        /// <param name="PageName">The name given to your custom page, which will be displayed at the top of the left page.</param> 
        /// <param name="Order">The slot in which your bookmark will be. Starting index is at 0.</param>
        /// <param name="Localized">Whether the name has translations it can use for different languages. Requires a proper Localization Key.</param>
        /// <param name="LocalizationKey">The Localization Key for your Page</param>
        public CustomAmmonomiconPageController(string PageName, int Order, bool Localized = false, string LocalizationKey = "")
        {
            Name = PageName;
            ZOrder = Order;
            StringLocalization = LocalizationKey;
            isLocalized = Localized;
        }
        public string Name = "Balls";
        public string StringLocalization = "Balls";
        public bool isLocalized = false;
        public int ZOrder;
        /// <summary>
        /// Is ran when the bookmark is instantiated and added to the bookmark list. Use for initialization.
        /// </summary>
        /// <param name="self">The object instance of the bookmark.</param>
        public virtual void OnFirstAdd(AmmonomiconBookmarkController self) {}

        /// <summary>
        /// Return a list of your EncounterDatabaseEntries to be processed and displayed.
        /// </summary>
        /// <param name="renderer">The page asking for it.</param>
        /// <returns></returns>
        public virtual List<EncounterDatabaseEntry> GetEntriesForPage(AmmonomiconPageRenderer renderer)
        {
            return new List<EncounterDatabaseEntry>();
        }


        /// <summary>
        /// Ran to display the page name at the top of your page.
        /// </summary>
        /// <param name="self">The Page Renderer that is being modified.</param>
        public virtual void InitializeName(AmmonomiconPageRenderer self)
        {
            dfScrollPanel component = self.transform.parent.Find("Scroll Panel").GetComponent<dfScrollPanel>();
            Transform transform = component.transform.Find("Header");
            dfLabel[] componentsInChildren = transform.GetComponentsInChildren<dfLabel>();
            foreach(var entry in componentsInChildren)
            {
                entry.IsLocalized = isLocalized;
                entry.localizationKey = StringLocalization;
                entry.text = Name;
            }
        }
        /// <summary>
        /// Ran to initialize the creation of your left page.
        /// </summary>
        /// <param name="self">The Page Renderer that is being modified.</param>
        public virtual void InitializeItemsPageLeft(AmmonomiconPageRenderer self)
        {
            List<EncounterDatabaseEntry> entries = GetEntriesForPage(self);
            Transform transform = self.guiManager.transform.Find("Scroll Panel").Find("Scroll Panel");
            dfPanel component = transform.Find("Guns Panel").GetComponent<dfPanel>();
            dfPanel component3 = component.transform.GetChild(0).GetComponent<dfPanel>();
            self.StartCoroutine(self.ConstructRectanglePageLayout(component3, entries, new Vector2(12f, 20f), new Vector2(20f, 20f), false, null));
            component3.Anchor = (dfAnchorStyle.Top | dfAnchorStyle.Bottom | dfAnchorStyle.CenterHorizontal);
            component.Height = component3.Height;
            component3.Height = component.Height;
        }
        /// <summary>
        /// Ran to initialize the creation of your right page.
        /// </summary>
        public virtual void InitializeItemsPageRight(AmmonomiconPageRenderer rightPage)
        {
            if (rightPage == null)
            {
                return;
            }
            dfScrollPanel component = rightPage.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();
            Transform transform = component.transform.Find("Header");
            if (transform)
            {
                dfLabel component2 = transform.Find("Label").GetComponent<dfLabel>();
                component2.Text = component2.ForceGetLocalizedValue("#AMMONOMICON_UNKNOWN");
                component2.PerformLayout();
                dfSprite component3 = transform.Find("Sprite").GetComponent<dfSprite>();
                if (component3)
                {
                    component3.FillDirection = dfFillDirection.Vertical;
                    component3.FillAmount = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? 0.8f : 1f);
                    component3.InvertFill = true;
                }
            }
            dfLabel component4 = component.transform.Find("Tape Line One").Find("Label").GetComponent<dfLabel>();
            component4.Text = component4.ForceGetLocalizedValue("#AMMONOMICON_QUESTIONS");
            component4.PerformLayout();
            dfSlicedSprite componentInChildren = component.transform.Find("Tape Line One").GetComponentInChildren<dfSlicedSprite>();
            componentInChildren.Width = component4.GetAutosizeWidth() / 4f + 12f;
            dfLabel component5 = component.transform.Find("Tape Line Two").Find("Label").GetComponent<dfLabel>();
            component5.Text = component4.ForceGetLocalizedValue("#AMMONOMICON_QUESTIONS");
            component5.PerformLayout();
            dfSlicedSprite componentInChildren2 = component.transform.Find("Tape Line Two").GetComponentInChildren<dfSlicedSprite>();
            componentInChildren2.Width = component5.GetAutosizeWidth() / 4f + 12f;
            dfPanel component6 = component.transform.Find("ThePhoto").Find("Photo").Find("tk2dSpriteHolder").GetComponent<dfPanel>();
            dfSprite component7 = component.transform.Find("ThePhoto").Find("Photo").Find("ItemShadow").GetComponent<dfSprite>();
            component7.IsVisible = false;
            tk2dSprite componentInChildren3 = component6.GetComponentInChildren<tk2dSprite>();
            dfTextureSprite componentInChildren4 = component.transform.Find("ThePhoto").GetComponentInChildren<dfTextureSprite>();
            if (componentInChildren4 != null)
            {
                componentInChildren4.IsVisible = false;
            }
            if (!(componentInChildren3 == null))
            {
                if (SpriteOutlineManager.HasOutline(componentInChildren3))
                {
                    SpriteOutlineManager.RemoveOutlineFromSprite(componentInChildren3, true);
                }
                componentInChildren3.renderer.enabled = false;
            }


            dfLabel component8 = component.transform.Find("Scroll Panel").Find("Panel").Find("Label").GetComponent<dfLabel>();
            rightPage.CheckLanguageFonts(component8);
            component8.Text = component8.ForceGetLocalizedValue("#AMMONOMICON_MYSTERIOUS");
            component8.transform.parent.GetComponent<dfPanel>().Height = component8.Height;
            component.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>().ScrollPosition = Vector2.zero;

        }
        /// <summary>
        /// Ran whenever the left page becomes active.
        /// </summary>
        public virtual void OnPageOpenedLeft(AmmonomiconPageRenderer rightPage)
        {

        }
        /// <summary>
        /// Ran whenever the right page becomes active.
        /// </summary>
        public virtual void OnPageOpenedRight(AmmonomiconPageRenderer rightPage)
        {
            SetPreExistingEncounteredObjectifPossible(rightPage);
        }

        public void SetPreExistingEncounteredObjectifPossible(AmmonomiconPageRenderer rightPage)
        {
            if (AmmonomiconController.Instance.ImpendingLeftPageRenderer != null)
            {
                for (int i = 0; i < AmmonomiconController.Instance.ImpendingLeftPageRenderer.m_pokedexEntries.Count; i++)
                {
                    AmmonomiconPokedexEntry ammonomiconPokedexEntry = AmmonomiconController.Instance.ImpendingLeftPageRenderer.m_pokedexEntries[i];
                    if (ammonomiconPokedexEntry.encounterState == AmmonomiconPokedexEntry.EncounterState.ENCOUNTERED)
                    {
                        rightPage.SetRightDataPageTexts(ammonomiconPokedexEntry.ChildSprite, ammonomiconPokedexEntry.linkedEncounterTrackable);
                        if (AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
                        {
                            AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget = ammonomiconPokedexEntry.GetComponent<dfControl>();
                        }
                        return;
                    }
                    if (ammonomiconPokedexEntry.encounterState == AmmonomiconPokedexEntry.EncounterState.KNOWN)
                    {
                        rightPage.SetPageDataUnknown(rightPage);
                        rightPage.SetRightDataPageName(ammonomiconPokedexEntry.ChildSprite, ammonomiconPokedexEntry.linkedEncounterTrackable);
                        if (AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget == null)
                        {
                            AmmonomiconController.Instance.ImpendingLeftPageRenderer.LastFocusTarget = ammonomiconPokedexEntry.GetComponent<dfControl>();
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Is ran every time the Ammonomicon is opened. If the value returns false, the bookmark will not be displayed and interactable. 
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldBeActive()
        {
            return true;
        }

        /// <summary>
        /// Ran whenever this bookmark becomes active.
        /// </summary>
        public virtual void OnActivate()
        {

        }
        /// <summary>
        /// Ran whenever this bookmark becomes deactivated by ShouldBeActive().
        /// </summary>
        public virtual void OnDeactivate()
        {

        }

        public void OnSetDataUnknown(AmmonomiconPageRenderer rightPage)
        {
            dfScrollPanel component = rightPage.guiManager.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>();

            Transform transform = component.transform.Find("Header");
            if (transform)
            {
                dfLabel component2 = transform.Find("Label").GetComponent<dfLabel>();
                component2.Text = component2.ForceGetLocalizedValue("#AMMONOMICON_UNKNOWN");
                component2.PerformLayout();
                dfSprite component3 = transform.Find("Sprite").GetComponent<dfSprite>();
                if (component3)
                {
                    component3.FillDirection = dfFillDirection.Vertical;
                    component3.FillAmount = ((GameManager.Options.CurrentLanguage != StringTableManager.GungeonSupportedLanguages.ENGLISH) ? 0.8f : 1f);
                    component3.InvertFill = true;
                }
            }
            dfLabel component4 = component.transform.Find("Tape Line One").Find("Label").GetComponent<dfLabel>();

            component4.Text = component4.ForceGetLocalizedValue("#AMMONOMICON_QUESTIONS");
            component4.PerformLayout();

            dfSlicedSprite componentInChildren = component.transform.Find("Tape Line One").GetComponentInChildren<dfSlicedSprite>();

            componentInChildren.Width = component4.GetAutosizeWidth() / 4f + 12f;
            dfLabel component5 = component.transform.Find("Tape Line Two").Find("Label").GetComponent<dfLabel>();
            component5.Text = component4.ForceGetLocalizedValue("#AMMONOMICON_QUESTIONS");
            component5.PerformLayout();

            dfSlicedSprite componentInChildren2 = component.transform.Find("Tape Line Two").GetComponentInChildren<dfSlicedSprite>();
            componentInChildren2.Width = component5.GetAutosizeWidth() / 4f + 12f;
            dfPanel component6 = component.transform.Find("ThePhoto").Find("Photo").Find("tk2dSpriteHolder").GetComponent<dfPanel>();
            dfSprite component7 = component.transform.Find("ThePhoto").Find("Photo").Find("ItemShadow").GetComponent<dfSprite>();
            component7.IsVisible = false;

            tk2dSprite componentInChildren3 = component6.GetComponentInChildren<tk2dSprite>();
            dfTextureSprite componentInChildren4 = component.transform.Find("ThePhoto").GetComponentInChildren<dfTextureSprite>();
            if (componentInChildren4 != null)
            {
                componentInChildren4.IsVisible = false;
            }
            if (!(componentInChildren3 == null))
            {
                if (SpriteOutlineManager.HasOutline(componentInChildren3))
                {
                    SpriteOutlineManager.RemoveOutlineFromSprite(componentInChildren3, true);
                }
                componentInChildren3.renderer.enabled = false;
            }
            dfLabel component8 = component.transform.Find("Scroll Panel").Find("Panel").Find("Label").GetComponent<dfLabel>();
            rightPage.CheckLanguageFonts(component8);
            component8.Text = component8.ForceGetLocalizedValue("#AMMONOMICON_MYSTERIOUS");
            component8.transform.parent.GetComponent<dfPanel>().Height = component8.Height;
            component.transform.Find("Scroll Panel").GetComponent<dfScrollPanel>().ScrollPosition = Vector2.zero;
        }
    }
}
