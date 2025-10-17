using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Features.UI
{
    public class UITaskArea : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Image icon;
        public Sprite solvedTask;
        public Sprite unsolvedTask;
        public bool hideOnSolve = false;
        
        [SerializeField] private bool isSolved = false;

        public bool IsSolved => isSolved; 
        
        private void Start()
        {
            SetIsSolved(isSolved);
        }
        
        private void SetIcon(Sprite sprite = null)
        {
            if (sprite is null)
            {
                icon.gameObject.SetActive(false);
                return;
            }
            
            icon.sprite = sprite;
            icon.gameObject.SetActive(true);
        }

        public void SetIsSolved(bool isTaskSolved)
        {
            if (isTaskSolved) SolveTask();
            else UnsolveTask();
        }
        
        public void SolveTask()
        {
            isSolved = true;
            
            SetIcon(solvedTask);

            if (hideOnSolve)
            {
                gameObject.SetActive(false);
            }
        }

        public void UnsolveTask()
        {
            isSolved = false;
            
            SetIcon(unsolvedTask);
            
            gameObject.SetActive(true);
        }
    }
}