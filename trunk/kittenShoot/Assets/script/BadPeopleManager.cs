using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadPeopleManager : MonoBehaviour {

    public GameObject spawnBadHuman;
    public float SpawnRate = 5f;

    public Vector3[] spawnPoint;
    private GameObject human;

    bool spawnLock = false;

	// Use this for initialization


   void Start(){
       /* UpdateSpawning(); */

       InvokeRepeating("UpdateSpawning", 0f, SpawnRate);
       //UpdateSpawning();
   }
   
    void Update(){

/*          if(SpawnRate > 1  && !spawnLock){
            StartCoroutine(TwentySecond());
        }      */

 

    }

     IEnumerator TwentySecond()
    {
        spawnLock = true;
        InvokeRepeating("UpdateSpawning", SpawnRate, SpawnRate);
        yield return new WaitForSeconds(20);
        CancelInvoke();
        SpawnRate --;
        GameObject.Find("UIManager").SendMessage("SpawnRateCount", SpawnRate);
        spawnLock = false;
        if(SpawnRate ==  1){
            InvokeRepeating("UpdateSpawning", SpawnRate, SpawnRate);
            spawnLock = true;
        }
    }
	
	// Update is called once per frame
	void UpdateSpawning () {
        int temp;
        temp = Random.Range(0,4);
        human = (GameObject)Instantiate(spawnBadHuman, spawnPoint[temp], Quaternion.Euler(new Vector3(0, 0, 1)));
        human.GetComponent<HumanAI>().spawnTransform = spawnPoint[temp];
	}
}
