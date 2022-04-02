using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float movementSpeed = 5.0f;
  public float mouseSensitivity = 1.0f;
  public float jumpForce = 5.0f;

  Camera cam;
  Rigidbody body;
  Vector2 lookAt;

  // Start is called before the first frame update
  void Start()
  {
    body = GetComponent<Rigidbody>();
    cam = Camera.main;
  }

  bool IsTouchingGround()
  {
    RaycastHit hitInfo;

    if (Physics.Raycast(body.position, Vector3.down, out hitInfo, 0.95f))
      return true;
    return false;
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


    if (Input.GetAxis("Jump") > 0)
    {
      if (IsTouchingGround())
        velocity.y = jumpForce;
    }

    body.velocity = velocity;
  }

  void HandleRotations()
  {
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

    lookAt.y += mouseX;
    lookAt.x += mouseY;

    transform.rotation = Quaternion.Euler(0, lookAt.y, 0);
    cam.transform.rotation = Quaternion.Euler(lookAt.x, lookAt.y, 0);
  }

  // Update is called once per frame
  void Update()
  {
    HandleRotations();
    HandleMovement();
  }
}
