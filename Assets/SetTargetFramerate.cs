using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFramerate : MonoBehaviour {

    [SerializeField] int target;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = target;	
	}
}
