﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBehavior : MonoBehaviour
{

	public enum DayState { Day, Night };        //Enum of the day state


	private GameObject sun;     //Holds the sun directional light game object
	private Light sunOptions;   //Holds reference to the light component of the sun game obect

	[SerializeField]
	private float lengthOfDay;  //The amount of time it will take for the sun to cross the sky in real minutes
	private float lengthOfDaySeconds;   //The amount of time day lasts converted to seconds
	[SerializeField]
	private float lengthOfNight;  //  The amount of time the sun will be gone before it comes back in real minutes
	private float lengthOfNightSeconds;   //The amount of time night lasts converted to seconds

	private float dayTimer;     //Timer to keep track how far through the day it is
	private float nightTimer;     //Timer to keep track how far through the night it is

	private float duskDawnLength;	//Variable to hold value of one 24th of the day length to be used as dusk and dawn


	[SerializeField]
	private TerrainData mapData;    //Variable to hold refrence to map data

	private Vector3 centerOfMap;    //Variable to save the center position of the map

	private DayState currentDayState;   //Enum to save wether it is currently day or night

	private Material daySkybox;        //Holds reference to the skybox displayed material during the day
	[SerializeField]
	private Material nightSkybox;        //Holds reference to the skybox displayed material during the night

	// Start is called before the first frame update
	void Start()
	{
		sun = GameObject.Find("Sun");
		sunOptions = sun.GetComponent<Light>();
		sun.SetActive(true);
		centerOfMap = mapData.bounds.center;
		CalcSecondsOfDay();
		CalcSecondsOfNight();
		sun.transform.position = new Vector3(0, centerOfMap.y, 0);
		daySkybox = RenderSettings.skybox;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{
		if (currentDayState == DayState.Day)
		{
			dayTimer -= Time.deltaTime;

			sun.transform.RotateAround(centerOfMap, new Vector3(0, 0, -1), Time.deltaTime * (180 / lengthOfDaySeconds));
			//Keeping light pointed directly at center of map
			sun.transform.forward = centerOfMap - sun.transform.position;
			if (dayTimer <= 0)
			{

				currentDayState = DayState.Night;
				sunOptions.intensity = 0;
				sunOptions.shadowStrength = 0f;
				RenderSettings.skybox = nightSkybox;
				CalcSecondsOfNight();
			}
			//Dusk behavior
			else if (dayTimer - duskDawnLength <= 0)
			{
				sunOptions.intensity = (dayTimer / duskDawnLength);
			}
			//Dawn behaviordis
			else if (dayTimer > lengthOfDaySeconds - duskDawnLength)
            {
				sunOptions.intensity = (lengthOfDaySeconds - dayTimer) / duskDawnLength;
			}
		}
		else if (currentDayState == DayState.Night)
		{
			nightTimer -= Time.deltaTime;
			if (nightTimer <= 0)
			{
				sun.transform.position = new Vector3(0, centerOfMap.y, 0);
				currentDayState = DayState.Day;
				sunOptions.shadowStrength = .25f;
				RenderSettings.skybox = daySkybox;
				CalcSecondsOfDay();
			}
		}
	}

	/// <summary>
	/// Helper function to convert the length of day from  mintues to seconds
	/// </summary>
	private void CalcSecondsOfDay()
	{
		lengthOfDaySeconds = lengthOfDay * 60f;
		duskDawnLength = lengthOfDaySeconds / 24;
		dayTimer = lengthOfDaySeconds;
	}

	/// <summary>
	/// Helper function to convert the length of night from  mintues to seconds
	/// </summary>
	private void CalcSecondsOfNight()
	{
		lengthOfNightSeconds = lengthOfNight * 60f;
		nightTimer = lengthOfNightSeconds;
	}

	/// <summary>
	/// Property to return the current day state
	/// </summary>
	public DayState CurrentDayState
	{
		get { return currentDayState; }
	}


}