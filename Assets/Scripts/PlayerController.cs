using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float movementSpeed = 5.0f;
  public float mouseSensitivity = 1.0f;
  public float jumpForce = 5.0f;
  public bool isDriving = false;

  Camera cam;
  Rigidbody body;
  Vector2 lookAt;
  CarController ourCar;

  // Start is called before the first frame update
  void Start()
  {
    body = GetComponent<Rigidbody>();
    cam = Camera.main;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  bool IsTouchingGround()
  {
    return Physics.Raycast(body.position, Vector3.down, 0.95f);
  }

  void HandleMovement()
  {
    Vector3 velocity = Vector3.zero;

    float hori = Input.GetAxis("Horizontal");
    float vert = Input.GetAxis("Vertical");

    velocity = transform.forward * vert + transform.right * hori;
    velocity = Vector3.ClampMagnitude(velocity, 1.0f);
    velocity *= movementSpeed;
    velocity.y = body.velocity.y;


    if (IsTouchingGround())
    {

      if (Input.GetAxis("Jump") > 0)
      {
        velocity.y = jumpForce;
      }

      body.velocity = velocity;
    }
  }

  void HandleRotations()
  {
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

    lookAt.y += mouseX;
    lookAt.x += mouseY;

    lookAt.x = Mathf.Clamp(lookAt.x, -80, 80);
    lookAt.y = Mathf.Repeat(lookAt.y, 360);

    transform.rotation = Quaternion.Euler(0, lookAt.y, 0);
    cam.transform.rotation = Quaternion.Euler(lookAt.x, lookAt.y, 0);
  }

  void HandleActivation()
  {
    if (Input.GetMouseButtonDown(0))
    {
      if (!isDriving)
      {
        RaycastHit hitInfo;
        if (Physics.Raycast(body.position, transform.forward, out hitInfo, 2.0f))
        {
          //Debug.Log("Collide with " + hitInfo.collider.name);
          if (hitInfo.collider.name == "carBody")
          {
            //a bit ugly /o\
            ourCar = hitInfo.collider.transform.parent.GetComponent<CarController>();
            ourCar.startDrive();
            isDriving = true;
            
            body.isKinematic = true;
            body.detectCollisions = false;
          }
        }
      }
      else
      {
        isDriving = false;
        ourCar.stopDrive();
        //Need check is there any space?
        body.detectCollisions = true;
        body.isKinematic = false;
        body.position = ourCar.transform.position - (ourCar.transform.right*1.5f);
        ourCar = null;
        cam.transform.SetParent(this.transform);
        cam.transform.localPosition = new Vector3(0, 0.7f, 0);
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
     if (!isDriving)
    {
      HandleRotations();
      HandleMovement();
    }

    HandleActivation();
  }

  void FixedUpdate()
  {
   
  }
}
