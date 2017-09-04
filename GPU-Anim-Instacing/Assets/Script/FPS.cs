using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {

	int mFrames = 0;
	int mFPS = 0;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("UpdateFPS", 0f, 1f);		
	}
	
	// Update is called once per frame
	void Update () {
		++mFrames;
	}

	void UpdateFPS()
	{
		mFPS = mFrames;
		mFrames = 0;
	}

	void OnGUI(){
		GUILayout.Label("fps:" + mFPS);
	}
}
