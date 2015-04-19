using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    Transform player;
    public float offset;

    public GameObject starfield;
    public GameObject clouds;

	void Start () {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        player = go.transform;
	}
	
	void Update () {
        if (player.position.x + offset > transform.position.x) {
            float newPos = player.position.x + offset;
            starfield.transform.position = new Vector3(starfield.transform.position.x + (newPos - transform.position.x) - (newPos - transform.position.x) / 6, starfield.transform.position.y);
            clouds.transform.position = new Vector3(clouds.transform.position.x + (newPos - transform.position.x) - (newPos - transform.position.x) / 3, clouds.transform.position.y);
            transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }
	}
}
