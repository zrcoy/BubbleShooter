using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardVisualization : MonoBehaviour
{
    public GameObject Space_Prefab;
    public GameObject Q_Prefab;
    public GameObject E_Prefab;
    public GameObject A_Prefab;
    public GameObject D_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        Q = Instantiate(Q_Prefab, new Vector3(-3.22f, -9.16f, -7.75f), Quaternion.identity);
        E = Instantiate(E_Prefab, new Vector3(-1.82f, -9.16f, -7.75f), Quaternion.identity);
        A = Instantiate(A_Prefab, new Vector3(-3.22f, -10.65f, -7.75f), Quaternion.identity);
        D = Instantiate(D_Prefab, new Vector3(-1.82f, -10.65f, -7.75f), Quaternion.identity);
        SpaceBar = Instantiate(Space_Prefab, new Vector3(-2.51f, -12.14f, -7.75f), Quaternion.identity);
        SpaceBar.transform.localScale = new Vector3(3.7842f, 1, 1);
    }

    public void KeyPress(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Q:
                {
                    Press_Release(Q, true);
                    break;
                }
            case KeyCode.E:
                {
                    Press_Release(E, true);
                    break;
                }
            case KeyCode.A:
                {
                    Press_Release(A, true);
                    break;
                }
            case KeyCode.D:
                {
                    Press_Release(D, true);
                    break;
                }
            case KeyCode.Space:
                {
                    Press_Release(SpaceBar, true);
                    break;
                }
        }
    }

    public void KeyRelease(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Q:
                {
                    Press_Release(Q, false);
                    break;
                }
            case KeyCode.E:
                {
                    Press_Release(E, false);
                    break;
                }
            case KeyCode.A:
                {
                    Press_Release(A, false);
                    break;
                }
            case KeyCode.D:
                {
                    Press_Release(D, false);
                    break;
                }
            case KeyCode.Space:
                {
                    Press_Release(SpaceBar, false);
                    break;
                }
        }
    }

    void Press_Release(GameObject letter, bool press)
    {
        float move = -0.1f;
        if (!press)
            move *= -1;
        letter.transform.position = letter.transform.position + new Vector3(0, move, 0);
    }



    // Update is called once per frame
    void Update()
    {

    }

    GameObject Q;
    GameObject E;
    GameObject A;
    GameObject D;
    GameObject SpaceBar;
}
