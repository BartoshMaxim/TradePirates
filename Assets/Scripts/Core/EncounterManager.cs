using UnityEngine;
using PirateGame.Core;

namespace PirateGame.Core
{
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;
        
        void Start()
        {
            if (gameStateManager == null)
            {
                gameStateManager = FindObjectOfType<GameStateManager>();
            }
        }
        
        public void TriggerEncounter()
        {
            if (gameStateManager != null)
            {
                gameStateManager.ChangeState(GameState.Encounter);
            }
        }
    }
}
