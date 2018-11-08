using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDisplayController : MonoBehaviour {

    [SerializeField] LineRenderer line;    

    public void AddPosition(Vector3 position) {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, position);
    }
	
	public void Clear() {
        line.positionCount = 0;
    }
}
