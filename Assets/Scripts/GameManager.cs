using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    private GameObject objectToFollow;
    public GameObject playerPrefab;

    void Start()
    {
        objectToFollow = GameObject.FindWithTag("Car");
        StartCoroutine(CheckForObject());
    }

    IEnumerator CheckForObject()
    {
        bool flag = true;
        while (flag)
        {
            yield return new WaitForSeconds(0.2f);

            if (objectToFollow == null)
            {
                StartCoroutine(InstPlayer());
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    ThreeSecTimer timer = ThreeSecTimer.GetInstance();
                    timer.Begin(3);
                }

                flag = false;
            }
        }
    }

    IEnumerator InstPlayer()
    {
        yield return new WaitForSeconds(1f);
        Start();
    }

}

