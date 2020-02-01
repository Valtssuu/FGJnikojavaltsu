using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int kiloTons;
    public bool blow = false;

    public void Blow()
    {

        blow = true;

    }
}

