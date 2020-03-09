using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float m_rotation_speed = 0.6f;
    public float m_ShootingSpeed = 10.0f;
    public bool m_CanShoot = true;
    public float m_MovingSpeed_Horizontal = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_shootingBubble = transform.GetChild(2).GetComponent<ShootingBubble>();
        m_isShooting = false;
        m_KbVisual = GameObject.FindGameObjectWithTag("KeyBoardVisualization").GetComponent<KeyBoardVisualization>();
    }

    // Update is called once per frame
    void Update()
    {
        LockInRange();


        // key board input
        float rotation = Input.GetAxis("Horizontal") * m_rotation_speed;
        if(Input.GetKeyDown(KeyCode.A))
        {
            m_KbVisual.KeyPress(KeyCode.A);
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            m_KbVisual.KeyRelease(KeyCode.A);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            m_KbVisual.KeyPress(KeyCode.D);
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            m_KbVisual.KeyRelease(KeyCode.D);
        }
        
        if (transform.rotation.z <= 0.5f && transform.rotation.z >= -0.5f && rotation != 0 && !m_isShooting)
        {
            transform.Rotate(0, 0, -rotation);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_IsMoving = true;
            m_MovingSpeed_Horizontal = -Mathf.Abs(m_MovingSpeed_Horizontal);
            m_CanShoot = false;
            m_KbVisual.KeyPress(KeyCode.Q);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_IsMoving = true;
            m_MovingSpeed_Horizontal = Mathf.Abs(m_MovingSpeed_Horizontal);
            m_CanShoot = false;
            m_KbVisual.KeyPress(KeyCode.E);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            m_CanShoot = true;
            m_IsMoving = false;
            m_KbVisual.KeyRelease(KeyCode.Q);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            m_CanShoot = true;
            m_IsMoving = false;
            m_KbVisual.KeyRelease(KeyCode.E);
        }



        if (m_IsMoving && !m_isShooting)
        {
            transform.position = transform.position + new Vector3(m_MovingSpeed_Horizontal * Time.deltaTime, 0, 0);
        }


        // mouse input
        if (m_mousePosPre != Input.mousePosition && !m_isShooting)
        {
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = new Vector3(mousePosInWorld.x - transform.position.x, mousePosInWorld.y - transform.position.y, 0);
            dir.Normalize();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90.0f;
            if (angle <= 60.0f && angle >= -60.0f)
            {
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
        }

        // fire
        if (m_CanShoot)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_CanShoot = false;
                m_isShooting = true;
                m_shootingBubble.m_InitPos = m_shootingBubble.transform.position;
                m_ShootingDir = new Vector3(m_shootingBubble.transform.up.x, m_shootingBubble.transform.up.y, 0);
                m_KbVisual.KeyPress(KeyCode.Space);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_KbVisual.KeyRelease(KeyCode.Space);
        }
        if (m_isShooting && m_shootingBubble)
        {
            m_shootingBubble.transform.position += m_ShootingDir * m_ShootingSpeed * Time.deltaTime;
        }

        m_mousePosPre = Input.mousePosition;

    }

    void LockInRange()
    {
        if (transform.rotation.z > 0.5f)
        {
            Quaternion target = Quaternion.Euler(0, 0, 60.0f);
            transform.rotation = target;
        }
        else if (transform.rotation.z < (-0.5f))
        {
            Quaternion target = Quaternion.Euler(0, 0, -60.0f);
            transform.rotation = target;
        }

    }

    public void setIsShoot(bool b)
    {
        m_isShooting = b;
    }

    public bool getIsShoot()
    {
        return m_isShooting;
    }

    KeyBoardVisualization m_KbVisual;
    bool m_IsMoving = false;
    Vector3 m_mousePosPre;
    private ShootingBubble m_shootingBubble;
    private bool m_isShooting;
    private Vector3 m_ShootingDir;
}
