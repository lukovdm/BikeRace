using UnityEngine;
using System.Collections;

public class Ride : MonoBehaviour {
	public WheelJoint2D B_Wheel;
	public float speed;
	public float angularSpeed;
	private JointMotor2D motor;

	void Start(){
		motor.maxMotorTorque = B_Wheel.motor.maxMotorTorque;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!networkView.isMine)
						return;
		rigidbody2D.AddTorque (Input.GetAxis ("Horizontal") * -angularSpeed);
		motor.motorSpeed = Input.GetAxis ("Vertical") * speed;
		B_Wheel.motor = motor;
		B_Wheel.useMotor = motor.motorSpeed > 1;
	}
}
