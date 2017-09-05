using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorInstance : MonoBehaviour {

	public string[] animStateNames;
	public GameObject prefabTemplate;
	public float xInterval;
	public float yInterval;
	public int xCount;
	public int yCount;

	// Use this for initialization
	void Start () {
		CreateInstance ();
	}

	void CreateInstance()
	{
		for (int i = 0; i < animStateNames.Length; ++i) {
			if (animStateNames [i] == null) {
				Debug.LogError ("请确保animStateNames中不为空");
				return;
			}
		}
		
		for (int i = 0; i < xCount; ++i) {
			for (int j = 0; j < yCount; ++j) {
				GameObject childPrefab = GameObject.Instantiate(prefabTemplate);
				childPrefab.transform.SetParent (this.transform);
				childPrefab.transform.localScale = Vector3.one;
				childPrefab.transform.eulerAngles = Vector3.zero;
				float xPos = (i - xCount / 2) * xInterval;
				float zPos = (j - yCount / 2) * yInterval;
				childPrefab.transform.localPosition = new Vector3 (xPos, 0f, zPos);

				Animator animator = childPrefab.GetComponent<Animator> ();
				animator.Play(animStateNames[Random.Range(0, animStateNames.Length - 1)]);
			}
		}
	}
}
