using UnityEngine;
using Photon.Pun;

public class NetworkBomb : MonoBehaviourPun
{
    private float impactThreshold;
    public float impactField;
    public float force;
    public LayerMask lmToHit;
    public GameObject explosionPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a player car
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            // Ignore collision with player cars
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
        if (photonView.IsMine)
        {
            Rigidbody2D thisRb = GetComponent<Rigidbody2D>();
            CarController carController = GetComponent<CarController>();
            impactThreshold = carController.MaxSpeed - 0.1f;
            if (carController != null)
            {
                float speed = Mathf.Abs(carController.CurrentAcceleration);
                float collisionForce = speed * thisRb.mass;

                if (collisionForce > impactThreshold)
                {
                    if (collision.gameObject.CompareTag("enemies"))
                    {
                        photonView.RPC("ExplodeEnemy", RpcTarget.All, collision.gameObject.transform.position);
                    }

                    photonView.RPC("TriggerExplosion", RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    void ExplodeEnemy(Vector3 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }

    [PunRPC]
    void TriggerExplosion()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, impactField, lmToHit);
        foreach (Collider2D obj in objects)
        {
            if (obj.CompareTag("enemies"))
            {
                Vector2 dir = obj.transform.position - transform.position;
                obj.GetComponentInParent<Rigidbody2D>().AddForce(dir * force);
            }
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, impactField);
    }
}
