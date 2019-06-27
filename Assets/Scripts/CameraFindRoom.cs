using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFindRoom : MonoBehaviour {
    //[HideInInspector]
    public List<GameObject> ListRoom;
	void Awake() {
        ListRoom = new List<GameObject>();

    }
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Room")
        {
            if (!ListRoom.Contains(other.gameObject))
                ListRoom.Add(other.gameObject);
            if (ListRoom.Count >= 1)
                Camera.main.gameObject.GetComponent<CameraFollow>().ChangeRoom(ListRoom[0].GetComponent<BoxCollider2D>());
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Room")
        {    
            if (!ListRoom.Contains(other.gameObject))
                ListRoom.Add(other.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Room")
        {
            if (ListRoom.Contains(other.gameObject))
                ListRoom.Remove(other.gameObject);
            if(ListRoom.Count>=1)
             Camera.main.gameObject.GetComponent<CameraFollow>().ChangeRoom(ListRoom[0].GetComponent<BoxCollider2D>());
        }
    }

}
