using UnityEngine;
using System;

namespace PirateGame.Core
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameState currentState = GameState.WorldMap;
        
        public GameState CurrentState => currentState;
        
        public event Action<GameState> OnStateEntered;
        public event Action<GameState> OnStateExited;
        
        public void ChangeState(GameState newState)
        {
            if (currentState == newState)
            {
                return;
            }
            
            OnStateExited?.Invoke(currentState);
            
            GameState previousState = currentState;
            currentState = newState;
            
            #if UNITY_EDITOR
            Debug.Log($"GameState changed from {previousState} to {currentState}");
            #endif
            
            OnStateEntered?.Invoke(currentState);
        }
    }
}
using UnityEngine;
using System;

namespace PirateGame.Core
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private GameState currentState = GameState.WorldMap;
        
        public GameState CurrentState => currentState;
        
        public event Action<GameState> OnStateEntered;
        public event Action<GameState> OnStateExited;
        
        public void ChangeState(GameState newState)
        {
            if (currentState == newState)
            {
                return;
            }
            
            OnStateExited?.Invoke(currentState);
            
            GameState previousState = currentState;
            currentState = newState;
            
            #if UNITY_EDITOR
            Debug.Log($"GameState changed from {previousState} to {currentState}");
            #endif
            
            OnStateEntered?.Invoke(currentState);
        }
    }
}
