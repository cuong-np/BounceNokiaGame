using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

    public GameManager _GameManager;
    bool m_IsClose;
    float m_Time;
	void Start () {
        m_IsClose = true;
    }
	
    public void OpenGate()
    {
        m_IsClose = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
	void Update () {
        if (!m_IsClose)
        {
            UpdateColor();
            m_Time += Time.deltaTime;
            if (m_Time >= 1)
                m_Time = 0;
        }
	}
    void UpdateColor()
    {
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, m_Time);
    }

}
