using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float impactThreshold;
    public float impactField;
    public float force;
    public LayerMask lmToHit;
    public GameObject explosionPrefab;

    private List<Collider2D> collidersInContact = new List<Collider2D>(); // List to store colliders in contact

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D thisRb = GetComponent<Rigidbody2D>();

        // Get the CarController instance(should be attached to the this object)
        CarController carController = CarController.GetInstance();

        if (carController != null)
        {
            // Access properties or methods of the CarController instance
            float speed = Mathf.Abs(carController.CurrentAcceleration);
            float collisionForce = speed * thisRb.mass;
            impactThreshold = carController.MaxSpeed - 0.1f;
            
            if (collisionForce > impactThreshold && !collidersInContact.Contains(collision.collider))
            {
                GameObject obj = collision.gameObject;
                if (obj.CompareTag("enemies"))
                {
                    // Instantiate explosion prefab at the enemy's position
                    Instantiate(explosionPrefab, obj.transform.position, obj.transform.rotation);

                    // Destroy the enemy object
                    Destroy(obj.gameObject);
                }

                // Add the collider to the list of colliders in contact
                collidersInContact.Add(collision.collider);

                // StartCoroutine(ExplosionCoroutine());
                Explosion();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject resource = collision.gameObject;
        if (resource.CompareTag("frozen") || resource.CompareTag("Dire") || resource.CompareTag("small")) { return; }
        // Remove the collider from the list when the contact ends
        if (collidersInContact.Contains(collision.collider))
        {
            collidersInContact.Remove(collision.collider);
        }
    }

    void Explosion()
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
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, impactField);
    }
}
