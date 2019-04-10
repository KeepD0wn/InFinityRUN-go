using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour {

    public GameObject[] roadPrefs; //префабы дорог
    public GameObject[] roadBarriersPrefs; //префабы препятствий
    [SerializeField] GameObject startBlock; //стартовый блок
    [SerializeField] Transform player;
    List<GameObject> currentBlocks = new List<GameObject>(); //лист со всеми платформами

    int safeZone = 70;
    float blockZPos;
    int blocksCount = 7;
    float blockLenth = 0;
    

	// Use this for initialization
	void Start () {
        blockZPos = startBlock.transform.position.z;
        blockLenth = startBlock.GetComponent<BoxCollider>().bounds.size.z;

        currentBlocks.Add(startBlock);
        for(int i = 0; i<blocksCount;i++)
        {
            SpawnBlocks();
        }
       
    }
	
    	// Update is called once per frame
	void FixedUpdate () {
        CheckForSpawn();
	}

    void CheckForSpawn()
    {
        if (player.position.z-safeZone > (blockZPos - blocksCount * blockLenth))
        {
            SpawnBlocks();
            DestroyBlock();
        }
    }

    void DestroyBlock()
    {
        Destroy(currentBlocks[0]);
        currentBlocks.RemoveAt(0);
    }

    void SpawnBlocks()
    {
        GameObject newBlock = Instantiate(roadPrefs[Random.Range(0,roadPrefs.Length)],transform);
        blockZPos += blockLenth;
        newBlock.transform.position = new Vector3(0,0,blockZPos);
        currentBlocks.Add(newBlock);
    }
}
