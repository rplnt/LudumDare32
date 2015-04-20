using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    int health;
    int energy;
    public int score;

    public Text healthText;
    public Text energyText;
    public Text scoreText;
    public Text distanceText;

    public GameObject intro;
    public GameObject final;


    void Start() {
        Invoke("hideIntro", 4f);
    }


    public void InitUI(float hp, float en, float pts) {
        updateHealth(hp);
        updateEnergy(en);
        updatePoints(pts);
    }


    public void updateHealth(float hp) {
        health = Mathf.CeilToInt(hp);
        healthText.text = health.ToString();
    }


    public void updateEnergy(float en) {
        energy = Mathf.CeilToInt(en);
        energyText.text = energy.ToString();
    }


    public void updatePoints(float pts) {
        score = Mathf.CeilToInt(pts);
        scoreText.text = score.ToString();
    }


    public void gameOver(float distance) {
        distanceText.text = "You've walked " + (int)(distance / 1.44f) + "m and collected " + score + " fairy corpses.";
        final.SetActive(true);
    }


    public void RestartLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }


    void hideIntro() {
        intro.SetActive(false);
    }

}
