using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcsimetManager : MonoBehaviour {
    public PlayerController Control;
    Quaternion rotation;

    void Awake()
    {
        rotation = transform.rotation;
    }
    void Update()
    {
        transform.rotation = rotation;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Water")
            Control.MakeAcsimet(true);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Water")
            Control.MakeAcsimet(true);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
            Control.MakeAcsimet(false);
    }
}
