using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBubble : DestroyableBubble
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
        m_InitPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(checkOutOfBounds())
        {
            Instantiate(m_bubbleGrid.EnergyExplosionPar, transform.position, Quaternion.identity);
            RespawnShootingBubble();
        }
    }

    

    //collision check
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DestroyableBubble>() != null && !collision.gameObject.GetComponent<DestroyableBubble>().m_isSeperated)
        {
            DestroyableBubble bubble = collision.gameObject.GetComponent<DestroyableBubble>();
            m_bubbleGrid.RecalculatedClusters(bubble.m_BubbleIntVec2, m_Color);
            m_bubbleGrid.MarkBubble();
            RespawnShootingBubble();
            
        }
    }

    void RespawnShootingBubble()
    {
        base.Start();
        transform.position = m_InitPos;
        GetComponentInParent<Turret>().setIsShoot(false);
        GetComponentInParent<Turret>().m_CanShoot = true;

    }

    public Vector3 m_InitPos;
}
