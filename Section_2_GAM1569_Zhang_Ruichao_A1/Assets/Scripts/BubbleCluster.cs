using System;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCluster
{

    public BubbleCluster()
    {
        m_Bubbles = new List<DestroyableBubble>();
        m_bubbleGrid = GameObject.FindGameObjectWithTag("BubbleGrid").GetComponent<BubbleGrid>();
    }

    public void AddBubble(DestroyableBubble bubble)
    {
        m_Bubbles.Add(bubble);
    }



    public bool CheckClusterSeperated()
    {
        foreach (DestroyableBubble b in m_Bubbles)
        {
            if (!CheckNeibourForSeperatedCluster(b.m_BubbleIntVec2, b.m_Color))
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckNeibourForSeperatedCluster(IntVector2 origin, Color initColor)
    {
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x + 1, origin.y)))
        {
            if (m_bubbleGrid.m_Grid[origin.x + 1, origin.y].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x, origin.y + 1)))
        {
            if (m_bubbleGrid.m_Grid[origin.x, origin.y + 1].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x - 1, origin.y)))
        {
            if (m_bubbleGrid.m_Grid[origin.x - 1, origin.y].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x, origin.y - 1)))
        {
            if (m_bubbleGrid.m_Grid[origin.x, origin.y - 1].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x + 1, origin.y + 1)))
        {
            if (m_bubbleGrid.m_Grid[origin.x + 1, origin.y + 1].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x - 1, origin.y + 1)))
        {
            if (m_bubbleGrid.m_Grid[origin.x - 1, origin.y + 1].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x - 1, origin.y - 1)))
        {
            if (m_bubbleGrid.m_Grid[origin.x - 1, origin.y - 1].m_Color != initColor)
            {
                return false;
            }
        }
        if (m_bubbleGrid.AreCoordsValid(new IntVector2(origin.x + 1, origin.y - 1)))
        {
            if (m_bubbleGrid.m_Grid[origin.x + 1, origin.y - 1].m_Color != initColor)
            {
                return false;
            }
        }

        return true;
    }

    public void falldown()
    {
        foreach (DestroyableBubble bb in m_Bubbles)
        {
            bb.transform.position = new Vector3(bb.transform.position.x, bb.transform.position.y, 7.0f);// set it a it deeper so that is won't collide with shooting bubbles
            bb.transform.position += new Vector3(0, -5.0f * Time.deltaTime, 0);
        }
    }


    private BubbleGrid m_bubbleGrid;
    public List<DestroyableBubble> m_Bubbles;

    
}
