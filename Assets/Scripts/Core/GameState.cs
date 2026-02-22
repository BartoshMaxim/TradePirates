using System;

namespace PirateGame.Core
{
    /// <summary>
    /// Represents the current high-level state of the game.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Player is navigating the world map
        /// </summary>
        WorldMap,
        
        /// <summary>
        /// Player is at a port, can trade
        /// </summary>
        Port,
        
        /// <summary>
        /// Player is in an encounter (combat, event, etc.)
        /// </summary>
        Encounter
    }
}
