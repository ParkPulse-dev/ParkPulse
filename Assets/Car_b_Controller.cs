using UnityEngine;
using System.Collections;

public class Car_b_Controller : MonoBehaviour 
{


	public float MaxSpeed = 7.0f;
	public float MaxSteer = 2.0f;
	public float Breaks = 0.2f;

	[SerializeField]
	float Acceleration = 0.0f;
	float Steer = 0.0f;

	bool AccelFwd, AccelBwd;
	bool TouchAccel,TouchBack,TouchBreaks;
	bool SteerLeft, SteerRight;

	void Start () 
	{
	
	}

	void FixedUpdate () 
	{
			if (Input.GetKey (KeyCode.W))
				Accel (1);													//Accelerate in forward direction
			else if (Input.GetKey (KeyCode.S))
				Accel (-1);													//Accelerate in backward direction
			else if (Input.GetKey (KeyCode.Space)) 
			{
				if (AccelFwd)
					StopAccel (1, Breaks);									//Breaks while in forward direction
				else if (AccelBwd)
					StopAccel (-1, Breaks);									//Breaks while in backward direction
			} 
			else 
			{
				if (AccelFwd)
					StopAccel (1, 0.1f);									//Applies breaks slowly if no key is pressed while in forward direction
				else if (AccelBwd)
					StopAccel (-1, 0.1f);									//Applies breaks slowly if no key is pressed while in backward direction
			}
	}

	public void Accel(int Direction)
	{
		if (Direction == 1) 
		{
			AccelFwd = true;
			if (Acceleration <= MaxSpeed) 
			{
				Acceleration += 0.05f;
			}
				if (Input.GetKey (KeyCode.A))
					transform.Rotate (Vector3.forward * Steer);				//Steer left
				if (Input.GetKey (KeyCode.D))
					transform.Rotate (Vector3.back * Steer);				//steer right
		} 
		else if (Direction == -1) 
		{
			AccelBwd = true;
			if ((-1 * MaxSpeed) <= Acceleration) 
			{
				Acceleration -= 0.05f;
			}
				if (Input.GetKey (KeyCode.A))
					transform.Rotate (Vector3.back * Steer);				//Steer left (while in reverse direction)
				if (Input.GetKey (KeyCode.D))
					transform.Rotate (Vector3.forward * Steer);				//Steer left (while in reverse direction)
		}
			
		if (Steer <= MaxSteer)
			Steer += 0.01f;

		transform.Translate (Vector2.up * Acceleration * Time.deltaTime);
	}

	public void StopAccel(int Direction, float BreakingFactor)
	{
		if (Direction == 1) 
		{
			if (Acceleration >= 0.0f) 
			{
				Acceleration -= BreakingFactor;

					if (Input.GetKey (KeyCode.A))
						transform.Rotate (Vector3.forward * Steer);
					if (Input.GetKey (KeyCode.D))
						transform.Rotate (Vector3.back * Steer);
			}
			else
				AccelFwd = false;
		} 
		else if (Direction == -1) 
		{
			if(Acceleration <= 0.0f)
			{
				Acceleration += BreakingFactor;

					if (Input.GetKey (KeyCode.A))
						transform.Rotate (Vector3.back * Steer);
					if (Input.GetKey (KeyCode.D))
						transform.Rotate (Vector3.forward * Steer);
			}
			else
				AccelBwd = false;
		}

		if (Steer >= 0.0f)
			Steer -= 0.01f;

		transform.Translate (Vector2.up * Acceleration * Time.deltaTime);
	}
}
