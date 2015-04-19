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

    bool over = false;

    float health;
    public float maxHealth;

    float energy;
    public float maxEnergy;
    float energyDrain = 3.2f;

    Transform flashlight;
    UIController ui;

    public GameObject starfield;

    float walkBackSlowdown = 0.5f;

    void OnCollisionEnter2D(Collision2D coll) {
        /* landed */
        if (coll.gameObject.tag == "Ground") {
            grounded = true;
        }
    }

	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        flashlight = gameObject.transform.FindChild("Flashlight");

        health = maxHealth;
        energy = maxEnergy;

        GameObject canvas = GameObject.FindGameObjectWithTag("UI");
        ui = canvas.GetComponent<UIController>();
        ui.InitUI(health, energy, 0);

        //Cursor.visible = false;
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
        //if (move > 0 && Camera.main.transform.position.x <= transform.position.x) {
        //    Debug.Log(move / 1024);
        //    starfield.transform.position += new Vector3(move/64, 0, 0);
        //}
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
        if (over) return;

        /* Jump */
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (grounded) {
                jumping = true;
            }
        }

        energy -= energyDrain * Time.deltaTime;
        ui.updateEnergy(energy);

        if (energy < 0 || health < 0) {
            over = true;
            ui.gameOver();
        }
	}
}
