using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AmmonomiconAPI.Code.Misc
{
    public class AmmonomiconPageKey : MonoBehaviour
    {
        public string UniqueKey = "Test";
        public AmmonomiconBookmarkController bookmarkController;
        public CustomAmmonomiconPageController ammonomiconPageTag;

        public AmmonomiconPageRenderer.PageType Left;
        public AmmonomiconPageRenderer.PageType Right;

        public dfButton dfButton;
        public InstancePlusManager instancePlusManager;

        public string DFGUI_BackingImage = string.Empty;
        public dfAtlas CustomAtlas = null;
    }
}
