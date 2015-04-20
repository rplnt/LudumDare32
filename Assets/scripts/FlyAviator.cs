using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyAviator : MonoBehaviour {

    public Sprite eyesBlue;
    public Sprite eyesRed;

    public Sprite flyRed;
    public Sprite flyYellow;

    Transform player;
    CharController cController;

    public bool agro;
    float deadTimer;

    bool idle = true;
    //bool avoid = false;
    public string type;

    float speed;
    public float health;
    float maxHealth;
    float viewRange;

    SpriteRenderer shadow;
    SpriteRenderer body;
    SpriteRenderer eyes;

    Vector2 target;

    Vector3 playerOffset = new Vector2(-0.7f, +1.8f);
    Vector3 flashlightOffset = new Vector2(-0.7f, 1.26f);


    void Awake() {
        body = transform.FindChild("Body").GetComponent<SpriteRenderer>();
        shadow = transform.FindChild("Shadow").GetComponent<SpriteRenderer>();
        eyes = transform.FindChild("Eyes").GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cController = player.GetComponent<CharController>();
    }


    void OnEnable() {
        if (Random.value > 0.55) {
            type = "red";
            body.sprite = flyRed;
        } else {
            type = "yellow";
            body.sprite = flyYellow;
        }

        shadow.color = new Color(1f, 1f, 1f, 1f);
        eyes.sprite = eyesRed;

        float difficulty = player.position.x / 35;
        speed = 0.7f + 0.1f * difficulty;
        viewRange = 2.5f + 0.1f * difficulty;
        maxHealth = Random.Range(0.4f, 0.7f + 0.1f * difficulty);
        health = maxHealth;
        idle = true;
        agro = true;

        target = new Vector3(player.position.x + Random.Range(8,20), Random.Range(-1, 2));
    }

	

	void Update () {
        if (cController.paused) return;
        Vector3 body = player.position + playerOffset;

        if (idle && Vector2.Distance(body, transform.position) < viewRange) {
            idle = false;
            speed *= 3;
        }

        float distance = Vector2.Distance(target, transform.position);
        if (distance < 0.05) {
            if (idle) {
                target = new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), Mathf.Max(-1, Mathf.Min(3, transform.position.y + Random.Range(-0.25f, 0.25f))));
            } else {
                target = body + new Vector3(0, Random.Range(-0.7f, 0.3f));
            }
        }

        if (agro) {
            // TODO: AI            
            // move to target
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        } else {
            // drop down
            transform.position = new Vector3(transform.position.x, Mathf.Max(-2.47f, transform.position.y - speed * Time.deltaTime));
            deadTimer -= Time.deltaTime;
            if (deadTimer < 0) {
                this.Recycle();
            }
        }

        if (agro && health < maxHealth) {
            changeHealth(Time.deltaTime / 4);
        }

	}


    void changeHealth(float gain) {
        health += gain;
        float opacity;
        if (health < 0) {
            agro = false;
            opacity = 0;
            deadTimer = 4.0f;
            eyes.sprite = eyesBlue;
        } else {
            opacity = 0.5f + (health / maxHealth);
        }

        shadow.color = new Color(1f, 1f, 1f, opacity);
    }


    void OnTriggerStay2D(Collider2D coll) {
        if (cController.paused) return;
        if (!agro) return;

        if (coll.gameObject.name == "LightCone") {
            float drain = ((1 / Vector2.Distance(player.position + flashlightOffset, transform.position)) * 3 * Time.deltaTime);
            changeHealth(-drain);

            //avoid = true;
        }
    }

}
