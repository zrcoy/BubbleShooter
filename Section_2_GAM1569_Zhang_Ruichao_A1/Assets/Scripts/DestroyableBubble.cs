using UnityEngine;
using System.Collections;

public class DestroyableBubble : MonoBehaviour
{
    public Color m_Color;
    public IntVector2 m_BubbleIntVec2;
    public bool m_isSeperated;
    public BubbleGrid m_bubbleGrid;
    
    public virtual void Start()
    {
        int c = UnityEngine.Random.Range(0, 4);
        switch (c)
        {
            case (0):
                m_Color = Color.red;
                break;
            case (1):
                m_Color = Color.green;
                break;
            case (2):
                m_Color = Color.blue;
                break;
            case (3):
                m_Color = Color.yellow;
                break;
        }
        GetComponent<Renderer>().material.color = m_Color;
        m_isSeperated = false;
        m_bubbleGrid = GameObject.FindGameObjectWithTag("BubbleGrid").GetComponent<BubbleGrid>();

    }

    public virtual bool checkOutOfBounds()
    {
        if (transform.position.x < m_bubbleGrid.LeftBoundary || transform.position.x > m_bubbleGrid.RightBoundary || transform.position.y > m_bubbleGrid.TopBoundary || transform.position.y < m_bubbleGrid.BotBoundary)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if (checkOutOfBounds())
        {
            Instantiate(m_bubbleGrid.EnergyExplosionPar, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

}
