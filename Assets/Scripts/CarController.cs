using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Runtime;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections.Generic;

[Serializable]
public struct CommandLog
{
	public int FrameExecuted;
	public float rot_z;
	public Vector3 pos;
}

[Serializable]
public class GameInputData
{
	public List<CommandLog> CommandLog;
}


public class CarController : MonoBehaviour 
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
	Vector3 prev_pos;
	float prev_rot_z;
	public List<CommandLog> CommandsLog;
	bool not_written_yet;
	int prev_frame;
	bool canMove = false;
	

	void Start () 
	{
		
		prev_frame = 0;
		prev_pos = new Vector3(0,0,0);
		prev_rot_z = 0;
		CommandsLog = new List<CommandLog>();
		not_written_yet = false;
		StartCoroutine(AllowMovement());
	}
	IEnumerator AllowMovement()
    {
        yield return new WaitForSeconds(3f);
        canMove = true; // Allow movement after 3 seconds
    }

	// void Quit () 
	// {
	// 	//Debug.Log("Stopped " + CommandsLog.Count );
	// }
	void FixedUpdate () 
	{
		if (!canMove) // If movement not allowed yet, return
            return;
			//if (!(prev_pos == transform.position) || !(prev_rot_z == transform.rotation.z)) {
			//	Debug.Log(Time.frameCount + " " + transform.position + " " + CommandsLog.Count );
			
				CommandLog commandLog = new()
				{
					pos = transform.position,
					rot_z = transform.rotation.z,
					FrameExecuted = Time.frameCount
				};
         
		        CommandsLog.Add(commandLog);				
				prev_frame = Time.frameCount;
				prev_pos = transform.position;
				prev_rot_z = transform.rotation.z;
		//	}

			if( (Time.frameCount > 7000) && not_written_yet ) {
				string _jsonPath = "Assets/Replay_System/computer_car_path.json";
				var gameInputData = new GameInputData
					{
						CommandLog = CommandsLog
					};

				Debug.Log("***************");
				Debug.Log("Writing list len=" + CommandsLog.Count);
				Debug.Log("***************");
				var actionInfo = JsonUtility.ToJson(gameInputData); 
				File.WriteAllText(_jsonPath,actionInfo);

				not_written_yet = false;
			}

			if (Input.GetKey (KeyCode.UpArrow))
				Accel (1);													//Accelerate in forward direction
			else if (Input.GetKey (KeyCode.DownArrow))
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
				if (Input.GetKey (KeyCode.LeftArrow))
					transform.Rotate (Vector3.forward * Steer);				//Steer left
				if (Input.GetKey (KeyCode.RightArrow))
					transform.Rotate (Vector3.back * Steer);				//steer right
		} 
		else if (Direction == -1) 
		{
			AccelBwd = true;
			if ((-1 * MaxSpeed) <= Acceleration) 
			{
				Acceleration -= 0.05f;
			}
				if (Input.GetKey (KeyCode.LeftArrow))
					transform.Rotate (Vector3.back * Steer);				//Steer left (while in reverse direction)
				if (Input.GetKey (KeyCode.RightArrow))
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

					if (Input.GetKey (KeyCode.LeftArrow))
						transform.Rotate (Vector3.forward * Steer);
					if (Input.GetKey (KeyCode.RightArrow))
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

					if (Input.GetKey (KeyCode.LeftArrow))
						transform.Rotate (Vector3.back * Steer);
					if (Input.GetKey (KeyCode.RightArrow))
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