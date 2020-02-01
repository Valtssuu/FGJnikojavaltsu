using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float targetTime = 60;
    public Text timer;
    public bool finished;

    [SerializeField] private GameObject gridParent = null;

    void Update()
    {
        if (!finished)
        {
            gridParent.SetActive(true);
            targetTime -= Time.deltaTime;

            if (targetTime <= 0)
            {
                timerEnded();
            }

            timer.text = targetTime.ToString("#.00");
        }
       

    }

    void timerEnded()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }

}
