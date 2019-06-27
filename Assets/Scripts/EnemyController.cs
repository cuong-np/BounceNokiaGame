using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public Vector2 Velocity;
    Rigidbody2D m_R2d;
    int numCollision;
	// Use this for initialization
	void Start () {
        m_R2d = GetComponent<Rigidbody2D>();
        m_R2d.velocity = Velocity;
        numCollision = 0;
    }
	
	void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag=="Ring")
        {
            numCollision++;
            if (numCollision >= 2)
                return;
        }
        
        if(other.gameObject.tag!="Player")
            Velocity *= -1;
        m_R2d.velocity = Velocity;
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ring")
        {
            numCollision = 0;
        }
    }



}
