﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScen : MonoBehaviour {

	public void LoadScene (int Level)
	{
		SceneManager.LoadScene (Level);
	}
	public void Exit()
	{
		Application.Quit ();
	}
}
