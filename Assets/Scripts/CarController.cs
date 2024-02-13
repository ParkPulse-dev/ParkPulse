using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

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

	public List<CommandLog> CommandsLog;
	bool not_written_yet;

	bool canMove = false;
	int numFrame = 8000;

	int forward = 1;
	int backwards = -1;
	float slowdown = 0.1f;
	float speedChange = 0.05f;
	float SteerChange = 0.01f;
	float minSteer = 0.0f;


	void Start()
	{

		CommandsLog = new List<CommandLog>();
		not_written_yet = false;
		StartCoroutine(AllowMovement());
	}
	IEnumerator AllowMovement()
	{
		yield return new WaitForSeconds(3f);
		canMove = true; // Allow movement after 3 seconds
	}

	void FixedUpdate()
	{
		if (!canMove) // If movement not allowed yet, return
			return;


		CommandLog commandLog = new()
		{
			pos = transform.position,
			rot_z = transform.rotation.z,
			FrameExecuted = Time.frameCount
		};

		CommandsLog.Add(commandLog);

		if ((Time.frameCount > numFrame) && not_written_yet)
		{
			string _jsonPath = "Assets/Replay_System/computer_car_path.json";
			var gameInputData = new GameInputData
			{
				CommandLog = CommandsLog
			};

			Debug.Log("*****************************************");
			Debug.Log("Writing list len=" + CommandsLog.Count);
			Debug.Log("*****************************************");
			var actionInfo = JsonUtility.ToJson(gameInputData);
			File.WriteAllText(_jsonPath, actionInfo);

			not_written_yet = false;
		}

		if (Input.GetKey(KeyCode.UpArrow))
			Accel(forward);                                                 //Accelerate in forward direction
		else if (Input.GetKey(KeyCode.DownArrow))
			Accel(backwards);                                                   //Accelerate in backward direction
		else if (Input.GetKey(KeyCode.Space))
		{
			if (AccelFwd)
				StopAccel(forward, Breaks);                                 //Breaks while in forward direction
			else if (AccelBwd)
				StopAccel(backwards, Breaks);                                   //Breaks while in backward direction
		}
		else
		{
			if (AccelFwd)
				StopAccel(forward, slowdown);                                   //Applies breaks slowly if no key is pressed while in forward direction
			else if (AccelBwd)
				StopAccel(backwards, slowdown);                                 //Applies breaks slowly if no key is pressed while in backward direction
		}
	}

	public void Accel(int Direction)
	{
		if (Direction == forward)
		{
			AccelFwd = true;
			if (Acceleration <= MaxSpeed)
			{
				Acceleration += speedChange;
			}
			if (Input.GetKey(KeyCode.LeftArrow))
				transform.Rotate(Vector3.forward * Steer);              //Steer left
			if (Input.GetKey(KeyCode.RightArrow))
				transform.Rotate(Vector3.back * Steer);             //steer right
		}
		else if (Direction == backwards)
		{
			AccelBwd = true;
			if ((backwards * MaxSpeed) <= Acceleration)
			{
				Acceleration -= speedChange;
			}
			if (Input.GetKey(KeyCode.LeftArrow))
				transform.Rotate(Vector3.back * Steer);             //Steer left (while in reverse direction)
			if (Input.GetKey(KeyCode.RightArrow))
				transform.Rotate(Vector3.forward * Steer);              //Steer left (while in reverse direction)
		}

		if (Steer <= MaxSteer)
			Steer += SteerChange;

		transform.Translate(Vector2.up * Acceleration * Time.deltaTime);
	}

	public void StopAccel(int Direction, float BreakingFactor)
	{
		if (Direction == forward)
		{
			if (Acceleration >= 0.0f)
			{
				Acceleration -= BreakingFactor;

				if (Input.GetKey(KeyCode.LeftArrow))
					transform.Rotate(Vector3.forward * Steer);
				if (Input.GetKey(KeyCode.RightArrow))
					transform.Rotate(Vector3.back * Steer);
			}
			else
				AccelFwd = false;
		}
		else if (Direction == backwards)
		{
			if (Acceleration <= minSteer)
			{
				Acceleration += BreakingFactor;

				if (Input.GetKey(KeyCode.LeftArrow))
					transform.Rotate(Vector3.back * Steer);
				if (Input.GetKey(KeyCode.RightArrow))
					transform.Rotate(Vector3.forward * Steer);
			}
			else
				AccelBwd = false;
		}

		if (Steer >= minSteer)
			Steer -= SteerChange;

		transform.Translate(Vector2.up * Acceleration * Time.deltaTime);
	}
}
