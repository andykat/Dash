using UnityEngine;
using System.Collections;

public class MC : MonoBehaviour {

	private float dt = 0.0f;

	//character move variables
	private float df = 0.0f;
	private float vf = 20.0f;

	private bool inAir = true;
	private bool onWall = false;
	private bool onWallJumpJump = false;
	private float wallMoveAngle = 0.0f;
	private float wallVy = 0.0f;
	private float wallVx = 0.0f;
	private float wallVz = 0.0f;
	private float wallSaveY = 0.0f;
	private float wallSaveX = 0.0f;
	private float wallSaveZ = 0.0f;
	private float wallTimeLimit = 1.0f;
	private float wallTime = 0.0f;
	private float wallUpVel = 3.0f;
	private float wallCameraAngleX = 0.0f;
	private float wallCameraAngleY = 0.0f;
	private float wallCameraSpeed = 220.0f;
	private float currentWallCameraAngleX = 0.0f;
	private float currentWallCameraAngleY = 0.0f;
	private float currentWallCameraAngleYVel = 0.0f;
	private float wallHitAngle = 0.0f;
	private float flipAngleSpeed = 200.0f;
	private float flipAngleCurX = 0.0f;
	private float flipAngleLimitX = 0.0f;
	private float flipAngleCurY = 0.0f;
	private float flipAngleLimitY = 0.0f;

	private float jumpForce = 1000.0f;


	private float yrot = 0.0f;
	private float rotv = 180.0f;
	private float dyrot = 0.0f;

	private float dyrotSave = 0.0f;
	private float dfSave = 0.0f;

	private float cx = 0.0f;
	private float cy = 0.0f;
	private float cz = 0.0f;

	private float cameraXRotation = 15.0f;

	private float cameraXRotationMax = 55.0f;
	private float cameraXRotationMin = 30.0f;
	private float cameraXRotationSpeedMin = 55.0f;
	private float cameraXRotationSpeedMax = 6.0f;


	void Start () {

	}
	void OnCollisionEnter(Collision collision){
		cx = gameObject.transform.position.x;
		cy = gameObject.transform.position.y;
		cz = gameObject.transform.position.z;

		float crx = gameObject.transform.rotation.eulerAngles.x;
		float cry = gameObject.transform.rotation.eulerAngles.y;
		float crz = gameObject.transform.rotation.eulerAngles.z;

		float ny = cy - 1.0f;
		foreach (ContactPoint contact in collision.contacts) {
			float coX = contact.point.x;
			float coY = contact.point.y;
			float coZ = contact.point.z;

			float d = dist (cx, ny, cz, coX, coY, coZ);

			float ax = angleX(cx, cy, cz, coX, coY, coZ);
			float ay = angleY(cx, cy, cz, coX, coY, coZ);
			float az = angleZ(cx, cy, cz, coX, coY, coZ);

			print (ax +" "+ ay+" "+ az+ " " + cry);
			//print (coX + " " + coY + " " + coZ + " " + cx + " " + cy + " " + cz + " " + d);

			//land on the ground
			if (ax < -45 && ax > -135 && az < -45 && az > -135) {
				inAir = false;
				dyrot = dyrotSave;
				df = dfSave;
				onWallJumpJump = false;
				onWall = false;
				//cameraXRotation = 15.0f;

				//gameObject.GetComponentInChildren<Camera>().transform.rotation = Quaternion.Euler (15.0f, 0.0f, 0.0f);

			} else {
				if (inAir) {
					df = 0.0f;
				}
			}



			//wall jump
			if ((Mathf.Abs (ax) < 45.0f || Mathf.Abs (Mathf.Abs (ax) - 180.0f) < 45.0f)
			    && (Mathf.Abs (az) < 45.0f || Mathf.Abs (Mathf.Abs (az) - 180.0f) < 45.0f) && inAir && !onWall) {
				//top of colliding object
				//print("ayy");
				float tTop = contact.otherCollider.transform.position.y + contact.otherCollider.transform.localScale.y / 2.0f;
				//must be not close to the top
				if (Mathf.Abs (tTop - coY) > 3.0f) {
					
					//wall jump
					onWall = true;
					float tforce = 700.0f;
					wallTime = wallTimeLimit;
					print ("firstanglex:" + Camera.main.transform.rotation.eulerAngles.x);
					currentWallCameraAngleX = Camera.main.transform.rotation.eulerAngles.x;
					currentWallCameraAngleY = Camera.main.transform.rotation.eulerAngles.y;
					wallHitAngle = cry;

					if (dyrotSave == -rotv) {
						//left
						float forceAngle = (ay - 120.0f) * Mathf.Deg2Rad;
						gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (Mathf.Sin (forceAngle) * tforce, 0.0f, Mathf.Cos (forceAngle)));
						wallMoveAngle = forceAngle * Mathf.Rad2Deg;

						wallCameraAngleY = wallMoveAngle;
						currentWallCameraAngleYVel = wallCameraSpeed;
						wallVy = 0.0f;
						wallSaveY = gameObject.transform.position.y;
						wallCameraAngleX = 10.0f;

						Vector3 curRot = gameObject.transform.eulerAngles;
						curRot.y = wallMoveAngle;
						gameObject.transform.eulerAngles = curRot;

					} else if (dyrotSave == rotv) {
						//right
						float forceAngle = (ay) * Mathf.Deg2Rad;
						gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (Mathf.Sin (forceAngle) * tforce, 0.0f, Mathf.Cos (forceAngle)));
						wallMoveAngle = forceAngle * Mathf.Rad2Deg;
		
						wallCameraAngleY = wallMoveAngle;
						currentWallCameraAngleYVel = -wallCameraSpeed;
						wallVy = 0.0f;
						wallSaveY = gameObject.transform.position.y;
						wallCameraAngleX = 10.0f;

						Vector3 curRot = gameObject.transform.eulerAngles;
						curRot.y = wallMoveAngle;
						gameObject.transform.eulerAngles = curRot;
					} else {
						//move straight up
						print ("set!");
						wallVy = wallUpVel;
						wallMoveAngle = gameObject.transform.eulerAngles.y;
						wallCameraAngleY = gameObject.transform.eulerAngles.y;
						float oppositeAngle = (ay + 180.0f) * Mathf.Deg2Rad;
						wallSaveX = Mathf.Sin (oppositeAngle) * 1.0f + cx;
						wallSaveZ = Mathf.Cos (oppositeAngle) * 1.0f + cz;
						wallCameraAngleX = -45.0f;
						gameObject.GetComponent<Rigidbody> ().velocity = new Vector3 (0.0f, 0.0f, 0.0f);

					}
					gameObject.GetComponent<Rigidbody> ().useGravity = false;
				}
			}

		}
	}


	// Update is called once per frame
	void Update () {
		

		dt = Time.deltaTime;
		wallTime -= dt;
		cx = gameObject.transform.position.x;
		cy = gameObject.transform.position.y;
		cz = gameObject.transform.position.z;

		float cry = gameObject.transform.rotation.eulerAngles.y;
		float crz = gameObject.transform.rotation.eulerAngles.z;


		//inputs

		if(Input.GetKeyDown("left")){
			if (inAir) {
				dyrotSave = -rotv;
			} else {
				dyrot = -rotv;
			}
		}
		else if(Input.GetKeyDown("right")){
			if (inAir) {
				dyrotSave = rotv;
			} else {
				dyrot = rotv;
			}
		}

		if (Input.GetKeyUp ("left")) {
			if (inAir) {
				if (dyrotSave == -rotv) {
					dyrotSave = 0.0f;
				}
			} else {
				if (dyrot == -rotv) {
					dyrot = 0.0f;
				}
			}
		} else if (Input.GetKeyUp ("right")) {
			if (inAir) {
				if (dyrotSave == rotv) {
					dyrotSave = 0.0f;
				}
			} else {
				if (dyrot == rotv) {
					dyrot = 0.0f;
				}
			}
		}

		if (Input.GetKeyDown ("up")) {
			if (inAir) {
				dfSave = vf;
			} else {
				df = vf;
			}
		}
		if (Input.GetKeyUp ("up")) {
			if (inAir) {
					dfSave = 0.0f;
			} else {
					df = 0.0f;
			}

		}

		if (Input.GetKeyDown ("down")) {
			if (inAir) {
				dfSave = -vf;
			} else {
				df = -vf;
			}
		}
		if (Input.GetKeyUp ("down")) {
			if (inAir) {
				if (dfSave == -vf)
					dfSave = 0.0f;
			} else {
				if (df == -vf)
					df = 0.0f;
			}
		}

		if (Input.GetKeyDown ("space")) {
			if (!inAir) {
				gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0.0f, jumpForce, 0.0f));
				inAir = true;
				dyrot = 0.0f;
				dyrotSave = 0.0f;
				dfSave = df;
				//cameraXRotation = 80.0f;
			}
			if (onWall) {
				if (wallVy != 0.0f) {
					onWallJumpJump = true;
					onWall = false;
					gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 
						(Mathf.Sin((180.0f+wallHitAngle)*Mathf.Deg2Rad)*jumpForce/2.0f, jumpForce/1.2f,
							Mathf.Cos((180.0f+wallHitAngle)*Mathf.Deg2Rad)*jumpForce/2.0f));
					flipAngleCurX = -45.0f;
					flipAngleLimitX = 30.0f;
					flipAngleCurY = wallHitAngle;
					flipAngleLimitY = wallHitAngle + 180.0f;
					gameObject.transform.rotation = Quaternion.Euler (0.0f, wallHitAngle + 180.0f, 0.0f);
					gameObject.GetComponent<Rigidbody> ().useGravity = true;
				}
			}
		}
		if (!onWall) {
			if (!onWallJumpJump) {
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

				Vector3 curRot = gameObject.transform.eulerAngles;
				curRot.y = curRot.y + dyrot * dt;
				gameObject.transform.eulerAngles = curRot;


				gameObject.transform.position = new Vector3 (cx + Mathf.Sin (curRot.y * Mathf.Deg2Rad) * df * dt, cy, cz + Mathf.Cos (curRot.y * Mathf.Deg2Rad) * df * dt);

				//check if left off the ground
				if (!inAir) {
					if (gameObject.GetComponent<Rigidbody> ().velocity.y < -0.5f) {
						inAir = true;
						dyrot = 0.0f;
						dyrotSave = 0.0f;
						dfSave = df;
					}
				}
			} else {
				//////////////////////////////////
				//////////////////////////////////
				//wall jumpjumpp
				//////////////////////////////////
				//////////////////////////////////
				Camera.main.transform.position = new Vector3 (cx, cy + 9.0f, cz);

				flipAngleCurX += flipAngleSpeed*dt/2.8f;
				if (flipAngleCurX > flipAngleLimitX) {
					flipAngleCurX = flipAngleLimitX;
				}

				flipAngleCurY += flipAngleSpeed*dt;

				if (flipAngleCurY > flipAngleLimitY) {
					flipAngleCurY = flipAngleLimitY;
				}

				Camera.main.transform.rotation = Quaternion.Euler (flipAngleCurX, flipAngleCurY, 0.0f);
			}

		} else {
			//on the wall
			Camera.main.transform.position = new Vector3 (cx, cy + 9.0f, cz);

			currentWallCameraAngleX -= wallCameraSpeed * dt;
			if (currentWallCameraAngleX < wallCameraAngleX) {
				currentWallCameraAngleX = wallCameraAngleX;
			}

			//rotate y direction camera
			if (wallVy == 0.0f) {
				currentWallCameraAngleY -= currentWallCameraAngleYVel*dt/2.0f;
				if (currentWallCameraAngleYVel > 0.0f) {
					if (currentWallCameraAngleY < wallCameraAngleY) {
						currentWallCameraAngleY = wallCameraAngleY;
					}
				} else {
					if (currentWallCameraAngleY > wallCameraAngleY) {
						currentWallCameraAngleY = wallCameraAngleY;
					}
				}
			}

			Camera.main.transform.rotation = Quaternion.Euler (currentWallCameraAngleX, currentWallCameraAngleY, crz);
			//align camera
			//Vector3 curRot = gameObject.transform.eulerAngles;
			//curRot.y = wallMoveAngle;


			//if going straight up wall
			if (wallVy != 0.0f) {
				print ("up!");
				gameObject.transform.position = new Vector3 (cx, cy + wallVy * dt, cz);
				//curRot.x = wallCameraAngleX;
			} else {
				//going along the wall
				gameObject.transform.position = new Vector3(cx, wallSaveY, cz);
			}
				

			if (wallTime < 0.0f) {
				onWall = false;
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
				inAir = true;
				cameraXRotation = currentWallCameraAngleX;
				//curRot.x = cameraXRotationMax;
			}

			//gameObject.transform.eulerAngles = curRot;
		}
		//Constant camera
		//Camera.main.transform.position = new Vector3(0.0f,10.0f,0.0f);
		//Camera.main.transform.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
	}

	float dist(float a,float b,float c,float x,float y,float z){
		return Mathf.Sqrt ((a - x) * (a - x) + (b - y) * (b - y) + (c - z) * (c - z));
	}
	float angleX(float a, float b, float c, float x, float y, float z){
		return Mathf.Atan2 (y-b, z - c) * Mathf.Rad2Deg;
	}
	float angleY(float a, float b, float c, float x, float y, float z){
		return Mathf.Atan2 (z - c, x - a) * Mathf.Rad2Deg;
	}
	float angleZ(float a, float b, float c, float x, float y, float z){
		return Mathf.Atan2 (y-b, x - a) * Mathf.Rad2Deg;
	}
}
