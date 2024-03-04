using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;

    [Range(0, 10)]
    [SerializeField] float dustFormatationPeriod;

    private CarController carController;
    float counter;

    private void Start()
    {

        carController = CarController.GetInstance();
    }
    private void Update()
    {

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
