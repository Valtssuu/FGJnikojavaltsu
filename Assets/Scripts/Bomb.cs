using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int kiloTons;
    public bool blow = false;

    public void Blow()
    {
        float yPos = this.gameObject.transform.position.y;
        // Set Bomb x-position with random float between -1 and 1
        this.gameObject.transform.position = new Vector3(Random.Range(-1f, 1f), yPos, 0);
        blow = true;

    }
}

