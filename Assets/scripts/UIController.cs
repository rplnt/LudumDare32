using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    int health;
    int energy;
    int score;

    public Text healthText;
    public Text energyText;
    public Text scoreText;

    public GameObject final;

	// Use this for initialization
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

    public void gameOver() {
        final.SetActive(true);
    }
}
