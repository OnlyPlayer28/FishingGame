using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal static class Layer
    {
        /// <summary>/// Stays always on top, used for debug text/// </summary>
        public const float AlwaysOnTop = 0f;
        /// <summary>/// Used for UI that should be always on top/// </summary>
        public const float UI = .1f;
        /// <summary>/// Used for UI that should be on top of the game, but under e.g. the HUD -> menus/// </summary>
        public const float Overlay = .15f;
        /// <summary>/// Used for any overlay that need s to be rendered ontop of entities but under any UI/// </summary>
        public const float EntityOverlay = .3f;
        /// <summary>/// Used for any entities/ objects that need to be rendered on top of the game world/// </summary>
        public const float Entity = .4f;
        /// <summary>/// Used for rendering the game world -> the ground/// </summary>
        public const float Game = .6f;
        /// <summary>/// Used for mountains, paralaxed stuff/// </summary>
        public const float Backdrop = .7f;
    }
}
