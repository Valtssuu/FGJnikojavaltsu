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

    private void Start()
    {
        StartCoroutine("GridOn");
    }
    void Update()
    {

        if (!finished)
        {
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

    private IEnumerator GridOn()
    {
        yield return new WaitForSeconds(1);
        gridParent.SetActive(true);

    }
}
