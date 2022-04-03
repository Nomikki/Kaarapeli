using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
  public float maxMotorTorque = 5;
  public float maxSteeringAngle = 25;
  public List<AxleInfo> axleInfos;
  PlayerController player;
  Camera cam;
  bool controlThis = false;

  // Start is called before the first frame update
  void Start()
  {
    cam = Camera.main;
    player = GameObject.Find("Player").GetComponent<PlayerController>();
  }

  // Update is called once per frame
  void Update()
  {
    if (controlThis)
    {
      Vector3 offset = new Vector3(0, 2, 0);
      cam.transform.position += ((transform.position + offset) - cam.transform.position) * 0.01f;
      cam.transform.LookAt(transform.position + (transform.forward * 4.0f), Vector3.up);
    }
  }

  public void ApplyLocalPosition(WheelCollider collider)
  {
    if (collider.transform.childCount == 0)
      return;

    Transform visualWheel = collider.transform.GetChild(0);

    Vector3 position;
    Quaternion rotation;
    collider.GetWorldPose(out position, out rotation);

    visualWheel.transform.position = position;
    visualWheel.transform.rotation = rotation;
  }

  void FixedUpdate()
  {
    float motor = 0;
    float steering = 0;

    if (controlThis)
    {
      motor = Input.GetAxis("Vertical") * maxMotorTorque;
      steering = Input.GetAxis("Horizontal") * maxSteeringAngle;
    }

    foreach (AxleInfo axleinfo in axleInfos)
    {
      if (axleinfo.steering)
      {
        axleinfo.leftWheel.steerAngle = steering;
        axleinfo.rightWheel.steerAngle = steering;
      }

      if (axleinfo.motor)
      {
        axleinfo.leftWheel.motorTorque = motor;
        axleinfo.rightWheel.motorTorque = motor;
      }

      ApplyLocalPosition(axleinfo.leftWheel);
      ApplyLocalPosition(axleinfo.rightWheel);
    }
  }

  public void startDrive()
  {
    controlThis = true;
    cam.transform.SetParent(null);
  }

  public void stopDrive()
  {
    controlThis = false;
  }
}

[System.Serializable]
public class AxleInfo
{
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor;
  public bool steering;

}
