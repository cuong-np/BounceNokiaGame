using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour {
    public GameObject Nomal;
    public GameObject Catched;

	void Start () {
        Catched.SetActive(false);
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player" && Nomal.activeSelf)
        {
            Nomal.SetActive(false);
            Catched.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
