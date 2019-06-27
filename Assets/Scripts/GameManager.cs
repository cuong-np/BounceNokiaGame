using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static int MaxLevel = 7;
    public static int PlayerLife = 3;
    public static int Level = 1;
    public Text LifeText;
    public Text RingText;
    public Slider TimeSlider;
    public int NumRing;
    public GateController _GateController;
    [HideInInspector]
    public int CurrentRing;

	// Use this for initialization


	void Start () {
        CurrentRing = 0;
        UpdateLifeText();
        UpdateRingText();
    }
	
    public void UpdateLifeText()
    {
        LifeText.text = "X" + PlayerLife.ToString();
    }

    public void UpdateRingText()
    {
        if (NumRing - CurrentRing <= 0)
            _GateController.OpenGate();
        RingText.text = "X" + (NumRing - CurrentRing).ToString();
    }

    public void UpdateSlider(float values)
    {
        if (values <= 0)
            TimeSlider.gameObject.SetActive(false);
        else
        {
            TimeSlider.gameObject.SetActive(true);
            TimeSlider.value = values;
        }
    }

    public void GotoNextLevel()
    {
        Level++;
        SceneManager.LoadScene(Level);
    }
}
