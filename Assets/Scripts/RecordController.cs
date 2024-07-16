// using UnityEngine;
// using System.Collections;


// public class RecordController : MonoBehaviour
// {
//     public float MaxSpeed = 5.0f;
//     [SerializeField] float MaxSteer = 1.3f;
//     [SerializeField] float Breaks = 0.15f;
//     [SerializeField] float slowDown = 0.1f;
//     public float speedChange = 0.05f;
//     [SerializeField] float steerChange = 0.01f;
//     [SerializeField] float minSteer = 0.4f;
//     float Acceleration = 0.0f;
//     float Steer = 0.0f;
//     bool AccelFwd, AccelBwd;
//     bool canMove = false;
//     public bool isFrozen = false;
//     public bool isChangeDire = false;
//     int forward = 1;
//     int backwards = -1;
//     private SpriteRenderer SpriteRenderer;
//     public Sprite AudiSprite;
//     public Sprite SnowSprite;
//     public Sprite ChengeMoveSprite;

//     private static RecordController instance;

//     private void Awake()
//     {
//         if (instance == null)
//             instance = this;
//         else
//             Debug.LogWarning("Another instance of CarController already exists.");
//     }

//     public static RecordController GetInstance()
//     {
//         return instance;
//     }

//     void Start()
//     {
//         StartCoroutine(AllowMovement());
//         SpriteRenderer = GetComponent<SpriteRenderer>();
//     }
//     IEnumerator AllowMovement()
//     {
//         yield return new WaitForSeconds(3f);
//         canMove = true; // Allow movement after 3 seconds
//     }

//     IEnumerator AllowMovement2()
//     {
//         yield return new WaitForSeconds(3f);
//         if (isChangeDire)
//             SpriteRenderer.sprite = ChengeMoveSprite;
//         else
//             SpriteRenderer.sprite = AudiSprite;
//         isFrozen = false;
//     }
//     public void DireCar(bool isReversed)
//     {
//         if (!isChangeDire)
//         {
//             if (Input.GetKey(KeyCode.A))
//                 transform.Rotate((isReversed ? Vector3.back : Vector3.forward) * Steer);
//             if (Input.GetKey(KeyCode.D))
//                 transform.Rotate((isReversed ? Vector3.forward : Vector3.back) * Steer);
//         }
//         else
//         {
//             if (Input.GetKey(KeyCode.D))
//                 transform.Rotate((isReversed ? Vector3.back : Vector3.forward) * Steer);
//             if (Input.GetKey(KeyCode.A))
//                 transform.Rotate((isReversed ? Vector3.forward : Vector3.back) * Steer);
//         }
//     }

//     void FixedUpdate()
//     {
//         if (!canMove) // If movement not allowed yet, return
//             return;

//         if (isFrozen)
//         {
//             StartCoroutine(AllowMovement2());
//             return;
//         }

//         if (Input.GetKey(KeyCode.W))
//         {
//             if (AccelBwd) StopAccel(backwards, Breaks);
//             else Accel(forward);
//         }
//         //Accelerate in backward direction
//         else if (Input.GetKey(KeyCode.S))
//         {
//             if (AccelFwd) StopAccel(forward, Breaks);
//             else Accel(backwards);
//         }
//         else
//         {
//             if (AccelFwd)
//                 StopAccel(forward, slowDown); //Applies breaks slowly if no key is pressed while in forward direction
//             else if (AccelBwd)
//                 StopAccel(backwards, slowDown); //Applies breaks slowly if no key is pressed while in backward direction
//         }
//     }

//     public void Accel(int Direction)
//     {
//         if (Direction == forward)
//         {
//             AccelFwd = true;
//             if (Acceleration <= MaxSpeed)
//             {
//                 Acceleration += speedChange;
//             }
//             DireCar(false);
//             /*  if (Input.GetKey(KeyCode.A))
//                   transform.Rotate(Vector3.forward * Steer);
//               if (Input.GetKey(KeyCode.D))
//                   transform.Rotate(Vector3.back * Steer);*/
//         }
//         else if (Direction == backwards)
//         {
//             AccelBwd = true;
//             if ((backwards * MaxSpeed) <= Acceleration)
//             {
//                 Acceleration -= speedChange;
//             }
//             /* if (Input.GetKey(KeyCode.A))
//                  transform.Rotate(Vector3.back * Steer);
//              if (Input.GetKey(KeyCode.D))
//                  transform.Rotate(Vector3.forward * Steer);*/
//             DireCar(true);
//         }

//         if (Steer <= MaxSteer)
//             Steer += steerChange;

//         transform.Translate(Acceleration * Time.deltaTime * Vector2.up);
//     }

//     public void StopAccel(int Direction, float BreakingFactor)
//     {
//         if (Direction == forward)
//         {
//             if (Acceleration >= 0.0f)
//             {
//                 Acceleration -= BreakingFactor;

//                 DireCar(false);
//             }
//             else
//                 AccelFwd = false;
//         }
//         else if (Direction == backwards)
//         {
//             if (Acceleration <= minSteer)
//             {
//                 Acceleration += BreakingFactor;

//                 DireCar(true);
//             }
//             else
//                 AccelBwd = false;
//         }

//         if (Steer >= minSteer)
//             Steer -= steerChange;

//         transform.Translate(Acceleration * Time.deltaTime * Vector2.up);
//     }
//     public float CurrentAcceleration
//     {
//         get { return Acceleration; }
//     }

// }
