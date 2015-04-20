using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {

    public float speed;
    public float offset;
    public float jump;
    bool grounded = false;
    bool jumping = false;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer self;

    bool over = false;

    bool flickering = false;

    float health;
    public float maxHealth;

    float energy;
    public float maxEnergy;
    float energyDrain = 2.3f;

    Transform flashlight;
    Transform cone;
    UIController ui;

    public Color damageColor;

    public GameObject starfield;

    float walkBackSlowdown = 0.5f;

    void OnCollisionEnter2D(Collision2D coll) {
        /* landed */
        if (coll.gameObject.tag == "Ground") {
            grounded = true;
        }

        if (coll.gameObject.tag == "Enemy") {
            FlyAviator fly = coll.gameObject.GetComponent<FlyAviator>();
            if (fly.agro) {
                health -= fly.health * 8;
                self.color = damageColor;
                Invoke("TurnBlack", 0.1f);
            } else {
                switch (fly.type) {
                    case "yellow":
                        energy = Mathf.Min(energy + 6.3f, maxEnergy);
                        break;
                    case "red":
                        health = Mathf.Min(health + 4.8f, maxHealth);
                        break;
                }
                ui.updatePoints(ui.score + 1);
            }
            coll.gameObject.Recycle();
        }
    }


	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        self = gameObject.GetComponent<SpriteRenderer>();

        flashlight = gameObject.transform.FindChild("Flashlight");
        cone = flashlight.transform.FindChild("LightCone");

        health = maxHealth;
        energy = maxEnergy;

        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        ui = canvas.GetComponent<UIController>();
        ui.InitUI(health, energy, 0);

        Cursor.visible = false;

        Invoke("TurnThatStupidLightOnWithoutParameters", 3);
	}


    void FixedUpdate() {
        if (over) return;

        /* move left/right */
        float input = 0.0f;
        input = Input.GetAxis("Horizontal");
        float move = input * speed;
        if (move < 0) {
            if (transform.position.x + offset < Camera.main.transform.position.x) {
                move = 0;
            } else {
                move *= walkBackSlowdown;
            }
        }

        rigid.velocity = new Vector2(move, rigid.velocity.y);
        anim.SetFloat("speed", move);

        /* Jump */
        if (jumping) {
            anim.SetTrigger("jumped");
            rigid.AddForce(new Vector2(0, jump));
            jumping = false;
            grounded = false;
        }

        /* rotate flashlight */
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle - 15, -45, 60);
        flashlight.rotation = Quaternion.Euler(0, 0, angle);
    }


	void Update () {
        if (over) {
            if (transform.localScale.y > 0) {
                transform.localScale = new Vector3(1, transform.localScale.y - 0.8f * Time.deltaTime, 1);
            }
            return;
        }

        /* Jump */
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (grounded) {
                jumping = true;
            }
        }

        if (cone.gameObject.activeSelf) {
            energy -= energyDrain * Time.deltaTime;
        }
        ui.updateEnergy(Mathf.Max(energy, 0));
        ui.updateHealth(Mathf.Max(health, 0));

        if (!flickering && energy > 0 && energy / maxEnergy < 0.12f) {
            flickering = true;
            StartCoroutine(FlickerLight(1.0f, 1f - (energy / maxEnergy)));
        }

        if (energy < 0) {
            cone.gameObject.SetActive(false);
        }

        if (health < 0) {
            over = true;
            anim.SetFloat("speed", 0);
            ui.gameOver(transform.position.x);
        }

	}


    IEnumerator FlickerLight(float duration, float intensity) {
        float elapsed = 0.0f;
        float flick = -1;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            if (flick < 0) {
                flick = Random.value/5;
                if (5*flick < intensity) {
                    cone.gameObject.SetActive(!cone.gameObject.activeSelf);
                }
            }

            flick -= Time.deltaTime;

            yield return null;
        }

        cone.gameObject.SetActive(energy > 0);
        flickering = false;
    }


    void TurnBlack() {
        self.color = new Color(0f, 0f, 0f);
    }


    void TurnThatStupidLightOnWithoutParameters() {
        StartCoroutine(FlickerLight(1, 0.5f));
    }

}
