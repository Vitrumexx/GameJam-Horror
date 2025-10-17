using System.Collections.Generic;
using _Project.Scripts.Features.Inventory;
using _Project.Scripts.Features.Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSignals: MonoBehaviour
{
    // ������ Singleton ��� ���� ����� ����� ��������� � CutsceneManager ����� CutsceneManager.Instance.�����������������������������()
    public static CutsceneSignals Instance;

    public GameObject UI;

    public TextMeshProUGUI TMP;
    // ���� �� �������� �������, � ������� ���� Key � Value ������� � ���������� ����� ���������� � Dictionary "cutsceneDataBase"
    // ������ �� ��� ������ ��� ���� ��������� Dictionary �� ������������ � ����������
    [SerializeField] private List<CutsceneStruct> cutscenes = new List<CutsceneStruct>();
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    // ���� ������ ���������� ��� �������� ������� �� ����� ���������, � � ���������� ����� ���������� ���� �������� �� ������
    // ��� ��� ��� Dictionary ��������� � ��������� �� ����� ��������� � ���� �� ������ ������� ��� ��� - CutsceneManager.cutsceneDataBase["���� ������ ��������"]
    public static Dictionary<string, GameObject> cutsceneDataBase = new Dictionary<string, GameObject>();

    // ������ � ���� �������� ������� ������������� � ������� ������, ���� �� ����� �������� ������ �� ������������� - ��� ����� null
    public static GameObject activeCutscene;

    // ������ ������, ����� ����������� ���, ���� �����
    public GameObject playerPrefab;
    private void Awake()
    {
        // ������ Singleton
        Instance = this;

        // �������� ����� ������������� ���� ������ � ����������
        InitializeCutsceneDataBase();

        // ���������� �� ���� ��������� � ��������� �� (����� ��� ������� ���� �� ����������� ��������)
        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }
    }

    // ����� � ������� �� ��������� Dictionary cutsceneDataBase
    private void InitializeCutsceneDataBase()
    {
        // ����� ����������� �� ������ ������ ������� ���� ���� ������
        cutsceneDataBase.Clear();

        // ��������� cutsceneDataBase ������� � ���������� ������� �� ������ � ����� cutscenes
        for (int i = 0; i < cutscenes.Count; i++)
        {           
            cutsceneDataBase.Add(cutscenes[i].cutsceneKey, cutscenes[i].cutsceneObject);
        }
    }

    // ����� ��� ��������� �������� �� �����
    public void StartCutscene(string cutsceneKey)
    {
        // ���� cutsceneDataBase �� ������� �������� � cutsceneKey �� ��������� �� ���� � ������� � �� ��������� ���� ��������� �����
        if (!cutsceneDataBase.ContainsKey(cutsceneKey)) 
        {
            Debug.Log($"�������� c ������ \"{cutsceneKey}\" ���� � cutsceneDataBase");
            return;
        } 

        // ���� ������ ������������� �������� � �� �������� ������� � ���� ������ Ũ �� �� ������ �������� ���������� ������
        if (activeCutscene != null)
        {
            if (activeCutscene == cutsceneDataBase[cutsceneKey])
            {
                return;
            }
        }
        
        UI.gameObject.SetActive(false);

        // ����������� �������� ��������
        activeCutscene = cutsceneDataBase[cutsceneKey];

        // ��������� ��� ��������
        foreach (var cutscene in cutsceneDataBase)
        {
            cutscene.Value.SetActive(false);
        }

        // �������� �� �������� ������� ����� �������
        cutsceneDataBase[cutsceneKey].SetActive(true);
    }

    // ����� ������� ��������� ������� ��������
    public void EndCutscene()
    {
        if (activeCutscene != null)
        {
            activeCutscene.SetActive(false);
            activeCutscene = null;
            TMP.text = null;
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

// ��������� ������� ��� �����, ����� ����� ����������� ��� �������� � Key � Value � Dictionary cutsceneDataBase
[System.Serializable]
public struct CutsceneStruct
{
    public string cutsceneKey;
    public GameObject cutsceneObject;
}