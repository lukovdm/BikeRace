using UnityEngine;
using System.Collections;

public class Ride : MonoBehaviour {
	public WheelJoint2D B_Wheel;
	public float acceleration;
	public float angularSpeed;
	public float topspeed;
	public Transform centerOfMass;
	public bool grounded;
	private JointMotor2D motor;
	private bool brake = false;

	void Start(){
		motor.maxMotorTorque = B_Wheel.motor.maxMotorTorque;
		rigidbody2D.centerOfMass = centerOfMass.position;
	}
	
	void FixedUpdate () {
		//if (!networkView.isMine)
		//				return;
		if (!grounded) {
			motor.motorSpeed = 0;
			return;
		}

		rigidbody2D.AddTorque (Input.GetAxis ("Horizontal") * -angularSpeed);
		if (Input.GetAxis ("Vertical") > 0) {
			motor.motorSpeed += Input.GetAxis ("Vertical") * acceleration;
			if(motor.motorSpeed > topspeed)
				motor.motorSpeed = topspeed;
		} else
			motor.motorSpeed = 0;

		if (Input.GetAxis ("Vertical") < 0) {
			brake = true;
			motor.motorSpeed = 0;
		} else
			brake = false;

		B_Wheel.motor = motor;
		B_Wheel.useMotor = motor.motorSpeed != 0 || brake;
	}

	void OnTriggerEnter2D(Collider2D other) {
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0;
		motor.motorSpeed = 0;
	}
}
