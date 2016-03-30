using UnityEngine;
using System.Collections;

public class MC : MonoBehaviour {

	private float dt = 0.0f;

	//character move variables
	private float df = 0.0f;
	private float vf = 20.0f;

	private bool inAir = true;
	private float jumpForce = 1000.0f;

	private float yrot = 0.0f;
	private float rotv = 180.0f;
	private float dyrot = 0.0f;

	private float cx = 0.0f;
	private float cy = 0.0f;
	private float cz = 0.0f;

	private float cameraXRotation = 15.0f;

	private float cameraXRotationMax = 45.0f;
	private float cameraXRotationMin = 25.0f;
	private float cameraXRotationSpeedMin = 55.0f;
	private float cameraXRotationSpeedMax = 4.0f;

	void Start () {

	}
	void OnCollisionEnter(Collision collision){
		cx = gameObject.transform.position.x;
		cy = gameObject.transform.position.y;
		cz = gameObject.transform.position.z;
		float ny = cy - 1.0f;
		foreach (ContactPoint contact in collision.contacts) {
			float coX = contact.point.x;
			float coY = contact.point.y;
			float coZ = contact.point.z;

			float d = dist (cx, ny, cz, coX, coY, coZ);
			//print (coX + " " + coY + " " + coZ + " " + cx + " " + cy + " " + cz + " " + d);
			if (d < 1.5f) {
				inAir = false;
				//cameraXRotation = 15.0f;

				//gameObject.GetComponentInChildren<Camera>().transform.rotation = Quaternion.Euler (15.0f, 0.0f, 0.0f);

			}
		}
	}


	// Update is called once per frame
	void Update () {
		

		dt = Time.deltaTime;
		cx = gameObject.transform.position.x;
		cy = gameObject.transform.position.y;
		cz = gameObject.transform.position.z;

		float cry = gameObject.transform.rotation.eulerAngles.y;
		float crz = gameObject.transform.rotation.eulerAngles.z;

		Camera.main.transform.position = new Vector3 (cx, cy + 9.0f, cz);
		Camera.main.transform.rotation = Quaternion.Euler (cameraXRotation, cry, crz);
		if (inAir) {
			cameraXRotation += dt * cameraXRotationSpeedMax;
			if (cameraXRotation > cameraXRotationMax) {
				cameraXRotation = cameraXRotationMax;
			}
		} else {
			cameraXRotation -= dt * cameraXRotationSpeedMin;
			if (cameraXRotation < cameraXRotationMin) {
				cameraXRotation = cameraXRotationMin;
			}
		}
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
			if (!inAir) {
				gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0.0f, jumpForce, 0.0f));
				inAir = true;

				//cameraXRotation = 80.0f;
			}
		}

		Vector3 curRot = gameObject.transform.eulerAngles;
		curRot.y = curRot.y + dyrot * dt;
		gameObject.transform.eulerAngles = curRot;


		gameObject.transform.position = new Vector3 (cx + Mathf.Sin (curRot.y * Mathf.Deg2Rad) * df * dt, cy, cz + Mathf.Cos (curRot.y * Mathf.Deg2Rad)* df * dt);
	}

	float dist(float a,float b,float c,float x,float y,float z){
		return Mathf.Sqrt ((a - x) * (a - x) + (b - y) * (b - y) + (c - z) * (c - z));
	}
}
