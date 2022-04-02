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

  // Start is called before the first frame update
  void Start()
  {
    cam = Camera.main;
    cam.transform.SetParent(null);
    player = GameObject.Find("Player").GetComponent<PlayerController>();
    

  }

  // Update is called once per frame
  void Update()
  {
    Vector3 offset = new Vector3(0, 2, 0);
    cam.transform.position += ((transform.position + offset) - cam.transform.position) * 0.01f;
    cam.transform.LookAt(transform.position + (transform.forward * 4.0f), Vector3.up);

    if (player.isDriving) {
      if (Input.GetAxis("Fire1") > 0)
      {
          cam.transform.position = player.transform.position;
          player.isDriving = false;
      }
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
    float motor = Input.GetAxis("Vertical") * maxMotorTorque;
    float steering = Input.GetAxis("Horizontal") * maxSteeringAngle;

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
}

[System.Serializable]
public class AxleInfo
{
  public WheelCollider leftWheel;
  public WheelCollider rightWheel;
  public bool motor;
  public bool steering;

}
