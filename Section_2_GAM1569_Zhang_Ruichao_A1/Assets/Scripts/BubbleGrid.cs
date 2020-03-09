using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGrid : MonoBehaviour
{
    public int GridDimX = 3;
    public int GridDimY = 3;
    public float GridSpacing = 1.2f;
    public GameObject BubblePrefab;
    public float LeftBoundary = -30.0f;
    public float RightBoundary = 30.0f;
    public float TopBoundary = 30.0f;
    public float BotBoundary = -30.0f;
    public float MarkInterval = 0.5f;
    public GameObject EnergyExplosionPar;

    // Start is called before the first frame update
    void Start()
    {
        m_Grid = new DestroyableBubble[GridDimX, GridDimY];

        m_VisitedCells = new bool[GridDimX, GridDimY];
        m_AddedBBForSeperatedCluster = new bool[GridDimX, GridDimY];

        GenerateBubbles();

        m_DestroyedBBClusters = new BubbleCluster();
        m_SameColorClusters = new List<BubbleCluster>();
        m_SeperatedClusters = new List<BubbleCluster>();

        m_Turret = GameObject.FindGameObjectWithTag("Turret").GetComponent<Turret>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSameColorCluster();
        CalculateSeperatedClusters();
        Falldown();
    }

   void Falldown()
    {
        if (m_SeperatedClusters.Count > 0)
        {
            foreach (BubbleCluster sepCluster in m_SeperatedClusters)
            {
                sepCluster.falldown();

            }
        }
    }
    

    public void CalculateSameColorCluster()
    {
        m_SameColorClusters.Clear();
        ClearVisitedCellsForSeperatedCluster();
        List<IntVector2> coordsToVisit = new List<IntVector2>();
        IntVector2 startCoords = new IntVector2(0, 0);

        while (FindNonAddedCoordForSeperated(out startCoords))
        {
            BubbleCluster bubbleCluster = new BubbleCluster();
            m_SameColorClusters.Add(bubbleCluster);
            coordsToVisit.Add(startCoords);
            Color InitColor = m_Grid[startCoords.x, startCoords.y].m_Color;

            while (coordsToVisit.Count > 0)
            {
                //Remove current cell from the list
                int indexToRemove = coordsToVisit.Count - 1;
                IntVector2 currentCoords = coordsToVisit[indexToRemove];
                coordsToVisit.RemoveAt(indexToRemove);

                //Skip iteration if current coordinate/cell is already visited
                if (m_AddedBBForSeperatedCluster[currentCoords.x, currentCoords.y] == true)
                {
                    continue;
                }



                //Retrieve ClicableCube object based on current coordinates
                DestroyableBubble currentBubble = m_Grid[currentCoords.x, currentCoords.y];



                //Skip iteration if current bubble is not the same color with the shooting bubble
                if (currentBubble.m_Color != InitColor)
                {
                    continue;
                }

                //Add current ClicableCube to the cluster
                bubbleCluster.AddBubble(currentBubble);
                //Set status of the current cell to visited
                m_AddedBBForSeperatedCluster[currentCoords.x, currentCoords.y] = true;


                //Search around the current cell for enabled neighbour cells. If found add a coordinate to the need to visit container.
                AddCoordsIfNeeded(currentCoords, new IntVector2(1, 0), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(-1, 0), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(0, 1), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(0, -1), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(1, 1), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(-1, 1), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(-1, -1), ref coordsToVisit);
                AddCoordsIfNeeded(currentCoords, new IntVector2(1, -1), ref coordsToVisit);
            }
        }


    }



    public void CalculateSeperatedClusters()
    {
        m_SeperatedClusters.Clear();
        foreach (BubbleCluster bCluster in m_SameColorClusters)
        {
            if (bCluster.CheckClusterSeperated())
            {
                foreach(DestroyableBubble b in bCluster.m_Bubbles)
                {
                    b.m_isSeperated = true;
                }
                m_SeperatedClusters.Add(bCluster);
            }
        }
    }

    bool FindNonVisitedCoord(out IntVector2 nonVisitedCoord)
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                if (m_Grid[x, y] && !m_VisitedCells[x, y])
                {
                    nonVisitedCoord = new IntVector2(x, y);

                    return true;
                }
            }
        }

        //No non-visited activated coords found.  Set the value to an invalid coordinate
        //and return false
        nonVisitedCoord = new IntVector2(-1, -1);

        return false;
    }

    bool FindNonAddedCoordForSeperated(out IntVector2 nonVisitedCoord)
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                if (m_Grid[x, y] && !m_AddedBBForSeperatedCluster[x, y])
                {
                    nonVisitedCoord = new IntVector2(x, y);

                    return true;
                }
            }
        }

        //No non-visited activated coords found.  Set the value to an invalid coordinate
        //and return false
        nonVisitedCoord = new IntVector2(-1, -1);

        return false;
    }

    public void RecalculatedClusters(IntVector2 startCoords, Color ShootingBubbleColor)
    {
        ClearVisitedCells();
        List<IntVector2> coordsToVisit = new List<IntVector2>();
        m_idx = 0;//index in bubble cluster, used for marking bubble

        //Add the same cell to the container which stores a coordinates/cells that needs to be visited
        coordsToVisit.Add(startCoords);

        //The logic below must be executed until we have no more coordinates to be visited
        while (coordsToVisit.Count > 0)
        {
            //Remove current cell from the list
            int indexToRemove = coordsToVisit.Count - 1;
            IntVector2 currentCoords = coordsToVisit[indexToRemove];
            coordsToVisit.RemoveAt(indexToRemove);

            //Skip iteration if current coordinate/cell is already visited
            if (m_VisitedCells[currentCoords.x, currentCoords.y] == true)
            {
                continue;
            }

            //Set status of the current cell to visited
            m_VisitedCells[currentCoords.x, currentCoords.y] = true;

            //Retrieve ClicableCube object based on current coordinates
            DestroyableBubble currentBubble = m_Grid[currentCoords.x, currentCoords.y];



            //Skip iteration if current bubble is not the same color with the shooting bubble
            if (currentBubble.m_Color != ShootingBubbleColor)
            {
                continue;
            }

            ////Add current ClicableCube to the cluster
            m_DestroyedBBClusters.AddBubble(currentBubble);



            ////Search around the current cell for enabled neighbour cells. If found add a coordinate to the need to visit container.
            AddCoordsIfNeeded(currentCoords, new IntVector2(1, 0), ref coordsToVisit);
            AddCoordsIfNeeded(currentCoords, new IntVector2(-1, 0), ref coordsToVisit);
            AddCoordsIfNeeded(currentCoords, new IntVector2(0, 1), ref coordsToVisit);
            AddCoordsIfNeeded(currentCoords, new IntVector2(0, -1), ref coordsToVisit);


        }

    }

    public void MarkBubble()
    {
        StartCoroutine("mark");
    }

    void StopMark()
    {
        StopCoroutine("mark");
    }

    IEnumerator mark()
    {
        
        yield return new WaitForSeconds(0.8f);
        if(m_idx<m_DestroyedBBClusters.m_Bubbles.Count)
        {
            m_DestroyedBBClusters.m_Bubbles[m_idx].GetComponent<Renderer>().material.color = Color.black;
            m_idx++;
            if(m_idx == m_DestroyedBBClusters.m_Bubbles.Count)
            {
                yield return new WaitForSeconds(1.0f);
                DestroyBubbleCluster();  
                StopMark();
            }
        }
        StartCoroutine("mark");
    }



    void AddCoordsIfNeeded(IntVector2 coords, IntVector2 checkDir, ref List<IntVector2> coordsToVisit)
    {
        IntVector2 nextCoords = coords + checkDir;

        if (AreCoordsValid(nextCoords))
        {
            coordsToVisit.Add(nextCoords);
        }
    }

    public bool AreCoordsValid(IntVector2 coords)
    {
        return coords.x >= 0 && coords.y >= 0 &&
            coords.x < m_Grid.GetLength(0) && coords.y < m_Grid.GetLength(1) && m_Grid[coords.x, coords.y];// must be in range as well as not be destroyed
    }



    void ClearVisitedCells()
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                m_VisitedCells[x, y] = false;
            }
        }
    }

    void ClearVisitedCellsForSeperatedCluster()
    {
        for (int x = 0; x < GridDimX; ++x)
        {
            for (int y = 0; y < GridDimY; ++y)
            {
                m_AddedBBForSeperatedCluster[x, y] = false;
            }
        }
    }




    private void GenerateBubbles()
    {
        for (int x = 0; x < GridDimX; x++)
        {
            for (int y = 0; y < GridDimY; y++)
            {
                Vector3 offset = new Vector3(x * GridSpacing, y * GridSpacing, 0.0f);

                GameObject bubbleObj = (GameObject)GameObject.Instantiate(BubblePrefab);

                bubbleObj.transform.position = offset + transform.position;

                bubbleObj.transform.parent = transform;

                bubbleObj.GetComponent<DestroyableBubble>().m_BubbleIntVec2 = new IntVector2(x, y);

                m_Grid[x, y] = bubbleObj.GetComponent<DestroyableBubble>();
            }
        }
    }

    public void DestroyBubbleCluster()
    {
        if (m_DestroyedBBClusters.m_Bubbles.Count > 0)
        {
            foreach (DestroyableBubble bb in m_DestroyedBBClusters.m_Bubbles)
            {
                Instantiate(EnergyExplosionPar, bb.transform.position, Quaternion.identity);



                Destroy(bb.gameObject);

            }
            m_DestroyedBBClusters.m_Bubbles.Clear();
        }
    }

    BubbleCluster m_BlankCluster;
    Turret m_Turret;
    int m_idx = 0;
    public DestroyableBubble[,] m_Grid;
    bool[,] m_VisitedCells;
    BubbleCluster m_DestroyedBBClusters;
    List<BubbleCluster> m_SameColorClusters;
    List<BubbleCluster> m_SeperatedClusters;
    bool[,] m_AddedBBForSeperatedCluster;
}
