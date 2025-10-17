using UnityEngine;

namespace _Project.Scripts.Features.Controls
{
    public class CursorStateChanger : MonoBehaviour
    {
        public GameStates startState = GameStates.Menu;
        
        public enum GameStates
        {
            Menu = 0,
            Game = 1
        }
        
        public void Start()
        {
            SetCursorState(startState);
        }

        public void SetCursorState(GameStates state)
        {
            switch (state)
            {
                default:
                case GameStates.Menu:
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
                }
                case GameStates.Game:
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                }
            }
        }
    }
}