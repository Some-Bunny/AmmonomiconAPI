using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmmonomiconAPI
{
    public class SpriteContainer
    {
        /// <summary>
        /// A container class for sprites for your bookmark.
        /// </summary>
        public SpriteContainer() { }
        /// <summary>
        /// A container class for sprites for your bookmark. A custom dfAtlas can be used.
        /// </summary>
        public SpriteContainer(dfAtlas dfAtlasToUse) { customDFAtlas = dfAtlasToUse; }

        public dfAtlas customDFAtlas = null;

        /// <summary>
        /// The name of the sprite used when the mouse hovers over your bookmark when it is selected. If not using a custom Atlas, write the filepath to your sprite instead.
        /// </summary>
        public string SelectHoverFrame;
        /// <summary>
        /// The name of the sprite used when the mouse hovers over your bookmark. If not using a custom Atlas, write the filepath to your sprite instead.
        /// </summary>
        public string HoverFrame;
        /// <summary>
        /// The names of the sprites used when the bookmark is selected. If not using a custom Atlas, write the filepaths to your sprites instead.
        /// </summary>
        public string[] SelectFrames;
        /// <summary>
        /// The names of the sprites used when the bookmark appears. If not using a custom Atlas, write the filepaths to your sprites instead.
        /// </summary>
        public string[] AppearFrames;

        public string BackingIcon = string.Empty;
    }
}
