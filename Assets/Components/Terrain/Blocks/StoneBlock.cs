﻿using UnityEngine;

namespace Antymology.Terrain
{
    /// <summary>
    /// A solid stone block.
    /// </summary>
    public class StoneBlock : AbstractBlock
    {

        #region Fields

        /// <summary>
        /// Statically held tile map coordinate.
        /// </summary>
        private static Vector2 _tileMapCoordinate = new Vector2(3, 1);

        /// <summary>
        /// Statically held is visible.
        /// </summary>
        private static bool _isVisible = true;

        /// <summary>
        /// Get the Block type
        /// </summary>
        public override string BlockType 
        { 
            get => "StoneBlock"; 
        }

        #endregion

        #region Methods

        /// <summary>
        /// The tile at the 0, 1, position in the tilemap.
        /// </summary>
        public override Vector2 tileMapCoordinate()
        {
            return _tileMapCoordinate;
        }

        /// <summary>
        /// mulch is a visible block.
        /// </summary>
        public override bool isVisible()
        {
            return _isVisible;
        }

        #endregion

    }
}
