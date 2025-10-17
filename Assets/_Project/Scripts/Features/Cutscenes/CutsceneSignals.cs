using System.Collections.Generic;
using _Project.Scripts.Features.Inventory;
using _Project.Scripts.Features.Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSignals: MonoBehaviour
{
    public static CutsceneSignals Instance;

    public GameObject UI;
    public TextMeshProUGUI TMP;
    [SerializeField] private List<CutsceneStruct> cutscenes = new List<CutsceneStruct>();
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    public static Dictionary<string, GameObject> cutsceneDataBase = new Dictionary<string, GameObject>();

    public static GameObject activeCutscene;

    public GameObject playerPrefab;
    
    private void Awake()
    {
        Instance = this;
        InitializeCutsceneDataBase();

        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }
    }

    private void InitializeCutsceneDataBase()
    {
        cutsceneDataBase.Clear();

        for (int i = 0; i < cutscenes.Count; i++)
        {           
            cutsceneDataBase.Add(cutscenes[i].cutsceneKey, cutscenes[i].cutsceneObject);
        }
    }

    public void StartCutscene(string cutsceneKey)
    {
        if (!cutsceneDataBase.ContainsKey(cutsceneKey)) 
        {
            Debug.Log($"Scene \"{cutsceneKey}\" isn't in the cutsceneDataBase.");
            return;
        } 

        if (activeCutscene != null)
        {
            if (activeCutscene == cutsceneDataBase[cutsceneKey])
            {
                return;
            }
        }
        
        UI.gameObject.SetActive(false);
        activeCutscene = cutsceneDataBase[cutsceneKey];

        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }

        cutsceneDataBase[cutsceneKey].SetActive(true);
    }

    public void EndCutscene()
    {
        if (activeCutscene != null)
        {
            activeCutscene.SetActive(false);
            activeCutscene = null;
            TMP.text = null;
            playerPrefab.GetComponent<AudioSource>().enabled = true;
        }
        
        UI.gameObject.SetActive(true);
    }

    public void SpawnPlayer(int num)
    {
        playerPrefab.transform.position = spawnPoints[num].transform.position;
        playerPrefab.transform.parent = null;
        playerPrefab.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void SwitchScene()
    {
        if (activeCutscene != null)
        {
            activeCutscene.SetActive(false);
            activeCutscene = null;
            TMP.text = null;
            SceneManager.LoadScene(2);
        }
    }

    public void Victory()
    {
        if (activeCutscene != null)
        {
            activeCutscene.SetActive(false);
            activeCutscene = null;
            TMP.text = null;
            SceneManager.LoadScene(0);
        }
    }
}

[System.Serializable]
public struct CutsceneStruct
{
    public string cutsceneKey;
    public GameObject cutsceneObject;
}