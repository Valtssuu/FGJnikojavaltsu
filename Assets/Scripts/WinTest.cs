using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTest : MonoBehaviour
{
    List<GameObject> Pieces = new List<GameObject>();
    public GameObject winPanel;
    public GameObject timer;
    // Start is called before the first frame update
    void Start()
    {
        
        foreach(GameObject Piece in GameObject.FindGameObjectsWithTag("Piece"))
        {
            Pieces.Add(Piece);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckWin())
        {
            Debug.Log("Win!");
            timer.GetComponent<Timer>().finished = true;
            winPanel.SetActive(true);
        }
    }

    private bool CheckWin()
    {
        foreach (GameObject Piece in Pieces)
        {
            RedCube redCube = Piece.GetComponent<RedCube>();
            if (redCube == null)
            {
                if (Piece.GetComponent<YellowCube>().isSnapped == false)
                    return false;
            }
            else
            {
                if (Piece.GetComponent<RedCube>().isSnapped == false)
                {
                    return false;
                }
            }
               
        }
        return true;
    }
}
