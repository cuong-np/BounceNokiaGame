using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float DeadZone = 0.1f;
    public float dampTime = 0.15f;
    public Transform target;
    public BoxCollider2D Room;

    Vector3 velocity = Vector3.zero;
    Vector3 oldTagerPos;
    float m_CamWidth;
    float m_CamHeight;

    void Start()
    {
        m_CamHeight = Camera.main.orthographicSize * 2f;
        m_CamWidth = m_CamHeight * Camera.main.aspect;
    }
    void LateUpdate()
    {
        if (target && target.GetComponent<PlayerController>().m_State != PlayerController.State.Die)
        {
            float posX = transform.position.x;
            float posY = transform.position.y;
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            Vector3 position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
          /*  if (Mathf.Abs(target.position.x - oldTagerPos.x) < DeadZone)
                position.x = posX;
            if (Mathf.Abs(target.position.y - oldTagerPos.y) < DeadZone)
                position.y = posY;
               */
            CheckRoom(ref position);
            transform.position = position;
            oldTagerPos = target.position;
        }

    }

    void CheckRoom(ref Vector3 position)
    {
        if (position.x + m_CamWidth / 2 > Room.bounds.max.x)
            position.x = Room.bounds.max.x - m_CamWidth / 2;
        if (position.x - m_CamWidth / 2 < Room.bounds.min.x)
            position.x = Room.bounds.min.x + m_CamWidth / 2;
        if (position.y + m_CamHeight / 2 > Room.bounds.max.y)
            position.y = Room.bounds.max.y - m_CamHeight / 2;
        if (position.y - m_CamHeight / 2 < Room.bounds.min.y)
            position.y = Room.bounds.min.y + m_CamHeight / 2;
    }
    public void ChangeRoom(BoxCollider2D room)
    {
        Room = room;
    }
}
