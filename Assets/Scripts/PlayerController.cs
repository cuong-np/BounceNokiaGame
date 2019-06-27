using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public enum State
    {
        Normal, Big, Die
    }
    const float COOLDOWN_JUMP = 0.1f;
    const float EXTRA_TIME = 20f;
    const float DEAD_ZONE = 0.1f;
    //
    public GameObject CheckPoint;
    public GameManager _GameManager;
    public GameObject Normal;
    public GameObject Big;
    public GameObject Die;
    public FindGrounded Find;
    public float MaxSpeed = 3.5f;
    public float PowerJump = 350f;
    public float ExtraPowerJump = 500f;
    public float AcsimetScale = 0.5f;
    public float WaterDrag = 0.25f;
    public Vector3 SpawnPoint;
    public State StartState = State.Normal;
    [HideInInspector]
    public State m_State;
    [HideInInspector]
    public float NumBounce;
    [HideInInspector]
    public float SavePowerJump;

    //
    Rigidbody2D m_R2D;
    CircleCollider2D m_Bound;
    float m_Move;
    float m_JumpTime;
    float m_SpeedTime;
    float m_ExtraSpeed;
    float m_ExtraPowerJump;
    float m_ExtraJumpTime;
    float m_PowerGravityTime;
    bool m_IsAcsimet;
    bool m_IsGravityPower;
    State m_OldState;
    void Start()
    {
        SpawnPoint = transform.position;
        m_JumpTime = 0;
        m_ExtraSpeed = 0;
        m_ExtraPowerJump = 0;
        SavePowerJump = 0;
        NumBounce = 0;
        m_SpeedTime = EXTRA_TIME;
        m_ExtraJumpTime = EXTRA_TIME;
        m_PowerGravityTime = EXTRA_TIME;
        Die.SetActive(false);
        m_State = State.Normal;
        m_OldState = m_State;
        m_R2D = GetComponent<Rigidbody2D>();
        m_Bound = GetComponent<CircleCollider2D>();
        m_IsAcsimet = false;
        if(StartState==State.Normal)
        {
            Big.SetActive(false);
            Normal.SetActive(true);
        }
        else
        {
            Big.SetActive(true);
            Normal.SetActive(false);
        }
        ChangeState(StartState);
        CheckPoint.transform.position = SpawnPoint;
        m_IsGravityPower = false;
    }

    void Update()
    {
        HandleInput();
        UpdateEffect();
        UpdateTime();
    }
    void FixedUpdate()
    {

        float maxSpeed = (m_R2D.drag != 0 ? 0.7f * (MaxSpeed+ m_ExtraSpeed) : (MaxSpeed + m_ExtraSpeed));
        m_R2D.velocity += new Vector2(m_Move * Time.fixedDeltaTime * maxSpeed*2, 0);
        if (Mathf.Abs(m_R2D.velocity.x) > maxSpeed)
            m_R2D.velocity = new Vector2(maxSpeed * m_Move, m_R2D.velocity.y);

    }
    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (m_Move != -1)
            {
                m_R2D.angularVelocity = 0;
                m_R2D.velocity = new Vector2(0, m_R2D.velocity.y);
            }
            m_Move = -1;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (m_Move != 1)
            {
                m_R2D.angularVelocity = 0;
                m_R2D.velocity = new Vector2(0, m_R2D.velocity.y);
            }
            m_Move = 1;
        }
        else
        {
            if (m_Move != 0)
            {
                m_R2D.angularVelocity = 0;
                m_R2D.velocity = new Vector2(0, m_R2D.velocity.y);
            }
            m_Move = 0;
        }
        if (Input.GetKey(KeyCode.Space) && Find.IsGround &&
           (m_JumpTime > COOLDOWN_JUMP && m_State != State.Die))
            Jump();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_IsGravityPower = false;
            if(!m_IsAcsimet)
                m_R2D.gravityScale = 1;
        }
    }
    void Jump()
    {
        if (m_PowerGravityTime <= EXTRA_TIME)
        {
            m_R2D.gravityScale = -1;
            m_IsGravityPower = true;
        }
        else
        {
            m_IsGravityPower = false;
            if (!Find.IsGroundBounce)
                SavePowerJump = 0;
            m_JumpTime = 0;
            m_R2D.velocity = new Vector2(m_R2D.velocity.x, 0);
            m_R2D.AddForce(Vector2.up * (PowerJump + m_ExtraPowerJump + SavePowerJump));
            if (Find.IsGroundBounce)
                SavePowerJump += PowerJump / 2;
            NumBounce = 0;
        }
    }
    void ChangeState(State _state)
    {
        m_OldState = m_State;
        m_State = _state;
        switch (m_State)
        {
            case State.Normal:
                Big.SetActive(false);
                Normal.SetActive(true);
                Die.SetActive(false);
                transform.localScale = new Vector3(1.4f, 1.4f, 1);
                break;
            case State.Big:
                Big.SetActive(true);
                Normal.SetActive(false);
                Die.SetActive(false);
                transform.localScale = new Vector3(1.4f * 4 / 3f, 1.4f * 4 / 3f, 1f);
                break;
            case State.Die:
                Big.SetActive(false);
                Normal.SetActive(false);
                Die.SetActive(true);
                transform.localScale = new Vector3(1, 1, 1);
                m_R2D.velocity = new Vector2();
                m_Bound.enabled = false;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            TouchItem(other);
            return;
        }
        if (other.gameObject.tag == "Enemy")
        {
            OnDie();
            return;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "CheckPoint":
                SpawnPoint = other.gameObject.transform.position;
                CheckPoint.transform.position = SpawnPoint;
                GameObject.Destroy(other.gameObject);
                break;
            case "Water":
                m_R2D.drag = WaterDrag;
                break;
            case "Life":
                GameManager.PlayerLife++;
                GameObject.Destroy(other.gameObject);
                _GameManager.UpdateLifeText();
                break;
            case "Ring":
                _GameManager.CurrentRing++;
                _GameManager.UpdateRingText();
                break;
            case "Gate":
                _GameManager.GotoNextLevel();
                break;
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Water")
            m_R2D.drag = 0;    
    }
    void TouchItem(Collision2D other)
    {
        switch (other.gameObject.name)
        {
            case "pumper":
                ChangeState(State.Big);
                break;
            case "deflater":
                ChangeState(State.Normal);
                break;
            case "power_speed":
                m_SpeedTime = 0;
                m_ExtraSpeed = MaxSpeed;
                break;
            case "power_jump":
                m_ExtraPowerJump = ExtraPowerJump;
                m_ExtraJumpTime = 0;
                break;
            case "power_gravity":
                m_PowerGravityTime = 0;
                break;
        }
    }

    void OnDie()
    {
        ChangeState(State.Die);
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3);
        GameManager.PlayerLife--;
        _GameManager.UpdateLifeText();
        ChangeState(m_OldState);
        transform.position = SpawnPoint;
        ReSet();    
        GetComponent<CameraFindRoom>().ListRoom.Clear();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void UpdateEffect()
    {
        if (m_SpeedTime > EXTRA_TIME)
            m_ExtraSpeed = 0;
        if (m_ExtraJumpTime > EXTRA_TIME)
            m_ExtraPowerJump = 0;
        if (m_PowerGravityTime > EXTRA_TIME && !m_IsAcsimet)
            m_R2D.gravityScale = 1;
        //
        float time = Mathf.Max(EXTRA_TIME - m_SpeedTime, EXTRA_TIME  - m_ExtraJumpTime, EXTRA_TIME  - m_PowerGravityTime, 0);
        _GameManager.UpdateSlider(time);
    }

    public void MakeAcsimet(bool isAcsimet)
    {
        if (isAcsimet && m_State == State.Big)
        {
            if(!m_IsGravityPower)
                 m_R2D.gravityScale = -AcsimetScale;
        }
        else
        {
            if (!m_IsGravityPower)
            {
                if (m_R2D.gravityScale != 1)
                    m_R2D.velocity = new Vector2(m_R2D.velocity.x, 0);
                m_R2D.gravityScale = 1;
            }
        }
        m_IsAcsimet = isAcsimet;
    }

    void UpdateTime()
    {
        m_SpeedTime += Time.deltaTime;
        m_JumpTime += Time.deltaTime;
        m_ExtraJumpTime += Time.deltaTime;
        m_PowerGravityTime += Time.deltaTime;
    }

    void ReSet()
    {
        m_SpeedTime = EXTRA_TIME;
        m_JumpTime = EXTRA_TIME;
        m_ExtraJumpTime = EXTRA_TIME;
        m_PowerGravityTime = EXTRA_TIME;
        m_R2D.drag = 0;
        m_Bound.enabled = true;
        m_R2D.velocity = new Vector2();
    }
}
