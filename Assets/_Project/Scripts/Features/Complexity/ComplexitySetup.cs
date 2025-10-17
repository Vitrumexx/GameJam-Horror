using System;
using _Project.Scripts.Features.Inventory;
using UnityEngine;

namespace _Project.Scripts.Features.Complexity
{
    public class ComplexitySetup : MonoBehaviour
    {
        [Header("Config")]
        public Complexities complexity = Complexities.Easy;
        public bool isComplexityFromStorage = true;

        [Header("Easy")] 
        public InventoryConfig easyInventoryConfig;
        
        [Header("Medium")] 
        public InventoryConfig mediumInventoryConfig;
        
        [Header("Hard")] 
        public InventoryConfig hardInventoryConfig;
        
        private Inventory.Inventory _inventory;

        private const string PlayerPrefsKey = "complexity";

        public enum Complexities
        {
            Easy = 0,
            Medium = 1,
            Hard = 2
        }
        
        public void Start()
        {
            _inventory = FindAnyObjectByType<Inventory.Inventory>();

            if (_inventory is null) return;
            
            if (isComplexityFromStorage && PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                complexity = (Complexities)PlayerPrefs.GetInt(PlayerPrefsKey);
            }
            
            SetupComplexity();
        }

        public static void SetComplexity(Complexities complexity)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey, (int)complexity);
        }

        private void SetupComplexity()
        {
            switch (complexity)
            {
                case Complexities.Easy:
                {
                    SetupEasy();
                    break;
                }
                case Complexities.Medium:
                {
                    SetupMedium();
                    break;
                }
                case Complexities.Hard:
                {
                    SetupHard();
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SetupEasy()
        {
            _inventory.SetupInventoryConfig(easyInventoryConfig);
        }

        private void SetupMedium()
        {
            _inventory.SetupInventoryConfig(mediumInventoryConfig);
        }

        private void SetupHard()
        {
            _inventory.SetupInventoryConfig(hardInventoryConfig);
        }
    }
}