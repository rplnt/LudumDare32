using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Cleaner : MonoBehaviour {

    float groundWidth = 8;
    float groundCount = 3;

    float starfieldWidth = 16;
    float starfieldCount = 2;

    System.Random rnd;

    public List<GameObject> foliage;
    public List<Sprite> clouds;

    Dictionary<GameObject, GameObject> foliageData;

    void Start() {
        foliageData = new Dictionary<GameObject, GameObject>();
        rnd = new System.Random();
    }

	void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            Transform ground = coll.gameObject.transform.parent.parent;
            ground.position = new Vector2(ground.position.x + (groundCount * groundWidth), ground.position.y);
            RecycleFoliage(ground.gameObject);
        } else if (coll.gameObject.name == "Starfield") {
            coll.transform.position = new Vector3(coll.transform.position.x + (starfieldCount * starfieldWidth), coll.transform.position.y);
        } else if (coll.gameObject.tag == "Cloud") {
            RecycleCloud(coll.gameObject);
        } else {
            Debug.LogWarning(coll.gameObject.name);
            Destroy(coll.gameObject);
        }
    }

    void RecycleFoliage(GameObject ground) {
        if (foliageData.ContainsKey(ground)) {
            Destroy(foliageData[ground]);
            foliageData.Remove(ground);
        }
        int i = rnd.Next(foliage.Count + 1);
        if (i < foliage.Count) {
            GameObject newFoliage = (GameObject)Instantiate(foliage[i], new Vector2(ground.transform.position.x + UnityEngine.Random.value * 4, -2.55f), Quaternion.identity);
            newFoliage.transform.SetParent(ground.transform);
            foliageData.Add(ground, newFoliage);
        }
    }

    void RecycleCloud(GameObject cloud) {
        SpriteRenderer renderer = cloud.GetComponent<SpriteRenderer>();
        if (renderer == null) {
            Debug.Log("This cloud sucks");
            return;
        }

        cloud.transform.position = new Vector3(cloud.transform.position.x + groundWidth * (groundCount - 1) + UnityEngine.Random.value * groundWidth, rnd.Next(1, 4), 0);
        renderer.sprite = clouds[rnd.Next(clouds.Count)];

    }

}
