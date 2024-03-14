using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 10)]
    [SerializeField] float dustFormatationPeriod;

    private CarController carController;
    private PhotonView photonView;
    float counter;

    private void Start()
    {
        carController = CarController.GetInstance();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            photonView = transform.parent.parent.GetComponent<PhotonView>();
        }

    }
    private void Update()
    {
        if ((photonView != null && !photonView.IsMine) || carController == null)
        {
            return;
        }

        counter += Time.deltaTime;

        float speed = Mathf.Abs(carController.CurrentAcceleration);

        if (speed > occurAfterVelocity)
        {
            if (counter > dustFormatationPeriod)
            {
                movementParticle.Play();
                counter = 0;

            }
        }
    }
}
