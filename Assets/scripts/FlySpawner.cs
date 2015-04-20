using UnityEngine;
using System.Collections;

public class FlySpawner : MonoBehaviour {

    public GameObject fly;

    int fliesSpawnedTotal = 0;
    public int fliesPool;
    public int initSpawn;

	void Start () {
        fly.CreatePool(fliesPool);
        for (int i = 0; i < initSpawn; i++) {
            fly.Spawn(new Vector2(Random.Range(3, 16), Random.Range(-1, 2)));
            fliesSpawnedTotal++;
        }
	}
	
	void Update () {
        //int stationedLimit = (int)(fliesPool - initSpawn - (fliesSpawnedTotal / 10));
        int currentLimit = initSpawn + (int)transform.position.x / 20;

        if (fly.CountPooled() > 0 && fliesPool - fly.CountPooled() < currentLimit) {
            float position = transform.position.x + Random.Range(8, 20);
            fly.Spawn(new Vector2(position, Random.Range(-1, 2)));
            fliesSpawnedTotal++;
            if (fliesSpawnedTotal % 10 == 0) {
                //Debug.Log(fliesSpawnedTotal);
            }
        }
	}
}
