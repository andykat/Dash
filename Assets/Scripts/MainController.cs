using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
	public GameObject mc;

	private float dt = 0.0f;

	//character move variables
	private float df = 0.0f;
	private float vf = 15.0f;

	private float jumpForce = 950.0f;

	private float yrot = 0.0f;
	private float rotv = 180.0f;
	private float dyrot = 0.0f;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		dt = Time.deltaTime;

		//inputs
		if(Input.GetKeyDown("left")){
			dyrot = -rotv;
		}
		else if(Input.GetKeyDown("right")){
			dyrot = rotv;
		}

		if (Input.GetKeyUp ("left")) {
			if (dyrot == -rotv) {
				dyrot = 0.0f;
			}
		} else if (Input.GetKeyUp ("right")) {
			if (dyrot == rotv) {
				dyrot = 0.0f;
			}
		}

		if (Input.GetKeyDown ("up")) {
			df = vf;
		}
		if (Input.GetKeyUp ("up")) {
			if(df == vf)
				df = 0.0f;
		}

		if (Input.GetKeyDown ("down")) {
			df = -vf;
		}
		if (Input.GetKeyUp ("down")) {
			if(df == -vf)
				df = 0.0f;
		}

		if (Input.GetKeyDown ("space")) {
			mc.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, jumpForce, 0.0f));
		}

		Vector3 curRot = mc.transform.eulerAngles;
		curRot.y = curRot.y + dyrot * dt;
		mc.transform.eulerAngles = curRot;

		float cx = mc.transform.position.x;
		float cy = mc.transform.position.y;
		float cz = mc.transform.position.z;
		mc.transform.position = new Vector3 (cx + Mathf.Sin (curRot.y * Mathf.Deg2Rad) * df * dt, cy, cz + Mathf.Cos (curRot.y * Mathf.Deg2Rad)* df * dt);
	}
}
