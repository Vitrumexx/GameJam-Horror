using System.Collections.Generic;
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

    public GameObject playerPrefab;
    
    [Header("Hint")]
    public TextMeshProUGUI skipHintTmp;
    public KeyCode skipKey = KeyCode.Escape;
    public float timeToSkip = 2f;
    
    public static GameObject ActiveCutscene;
    private float _skipTimeElapsed = 0f;
    
    private void Awake()
    {
        Instance = this;
        InitializeCutsceneDataBase();

        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }
    }

    private void Update()
    {
        if (!Input.GetKeyDown(skipKey))
        {
            _skipTimeElapsed = 0f;
            return;
        }

        if (_skipTimeElapsed < timeToSkip)
        {
            _skipTimeElapsed += Time.deltaTime;
            return;
        }
        
        _skipTimeElapsed = 0f;
        EndCutscene();
    }

    private void ShowSkipHint()
    {
        skipHintTmp.gameObject.SetActive(true);
        skipHintTmp.text = $"To skip cutscene hold {skipKey}.";
    }

    private void HideSkipHint()
    {
        skipHintTmp.gameObject.SetActive(false);
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

        if (ActiveCutscene != null)
        {
            if (ActiveCutscene == cutsceneDataBase[cutsceneKey])
            {
                return;
            }
        }
        
        UI.gameObject.SetActive(false);
        ActiveCutscene = cutsceneDataBase[cutsceneKey];

        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }

        cutsceneDataBase[cutsceneKey].SetActive(true);
        ShowSkipHint();
    }

    public void EndCutscene()
    {
        if (ActiveCutscene != null)
        {
            ActiveCutscene.SetActive(false);
            ActiveCutscene = null;
            TMP.text = null;
            playerPrefab.GetComponent<AudioSource>().enabled = true;
        }
        
        UI.gameObject.SetActive(true);
        HideSkipHint();
    }

    public void SpawnPlayer(int num)
    {
        playerPrefab.transform.position = spawnPoints[num].transform.position;
        playerPrefab.transform.parent = null;
        playerPrefab.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void SwitchScene()
    {
        if (ActiveCutscene != null)
        {
            ActiveCutscene.SetActive(false);
            ActiveCutscene = null;
            TMP.text = null;
            SceneManager.LoadScene(2);
        }
    }

    public void Victory()
    {
        if (ActiveCutscene != null)
        {
            ActiveCutscene.SetActive(false);
            ActiveCutscene = null;
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