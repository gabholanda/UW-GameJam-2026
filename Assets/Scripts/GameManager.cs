using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<NPCData> targetsData;

    public List<GameObject> NPCs;

    public List<Sprite> masks;
    public GameObject NPCPrefab;

    public GameObject finalTarget;
    public NPCData finalTargetData;

    public GameObject panelObject;
    public GameObject rightText;
    public GameObject wrongText;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        PickFinalTargetData();
        SpawnNPCs();
        PickTarget();
    }

    public void PickFinalTargetData()
    {
        finalTargetData = targetsData[Random.Range(0, targetsData.Count)];
    }

    public void SpawnNPCs()
    {
        for (int i = 0; i < targetsData.Count; i++)
        {
            GameObject NPCObj = Instantiate(NPCPrefab, new Vector2(Random.Range(-25, 13), Random.Range(-7, 15)), Quaternion.identity);
            //NPCObj.GetComponent<Animator>().runtimeAnimatorController = targetsData[i].animController;
            NPCObj.GetComponent<NPCBehaviour>().Initialize(targetsData[Random.Range(0, targetsData.Count)], masks[Random.Range(0, masks.Count)]);
            NPCs.Add(NPCObj);
        }
    }

    public void PickTarget()
    {
        finalTarget = NPCs[Random.Range(0, NPCs.Count)];
        finalTarget.GetComponent<NPCBehaviour>().SetFakeClue(finalTargetData.fakeClues[Random.Range(0, finalTargetData.fakeClues.Count)]);
    }

    public void ExecuteTarget(GameObject pickedTarget)
    {
        panelObject.SetActive(true);
        if(pickedTarget == finalTarget)
        {
            rightText.SetActive(true);
        }
        else
        {
            wrongText.SetActive(true);
        }
    }

    public void OnTryAgain()
    {
        foreach(GameObject NPC in NPCs)
        {
            Destroy(NPC);
        }

        NPCs.Clear();


        PickFinalTargetData();
        SpawnNPCs();
        PickTarget();
    }
}
