using System.Collections;
using UnityEngine;

public class TeleportManager : MonoBehaviour
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
            yield return new WaitForSeconds(1f); // Adjust the interval as needed

            if (objectToFollow == null)
            {
                StartCoroutine(InstPlayer());
                ThreeSecTimer timer = ThreeSecTimer.GetInstance();
                timer.Begin(3);
                flag = false;
            }
        }
    }

    IEnumerator InstPlayer()
    {
        yield return new WaitForSeconds(1f);
        Debug.LogWarning("Should instatiate...");
        Instantiate(playerPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Start();
    }
}
