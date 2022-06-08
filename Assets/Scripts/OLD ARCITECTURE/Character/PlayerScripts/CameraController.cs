using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;  
    public float offsetX = -5;        //The offset of the camera to centrate the player in the X axis
    public float offsetZ = 0;         //The offset of the camera to centrate the player in the Z axis
    public float offsetY = 7;
    public float maximumDistance = 2; //The maximum distance permited to the camera to be far from the player, its used to make a smooth movement
    public float playerVelocity = 10; //The velocity of your player, used to determine que speed of the camera

    private float movementX;
    private float movementZ;
    private float movementY;

    // Update is called once per frame
    void Update()
    {
        movementX = ((player.transform.position.x + offsetX - this.transform.position.x)) / maximumDistance;
        movementZ = ((player.transform.position.z + offsetZ - this.transform.position.z)) / maximumDistance;
        movementY = ((player.transform.position.y + offsetY - this.transform.position.y)) / maximumDistance;
        transform.position += new Vector3((movementX * playerVelocity * Time.deltaTime), (movementY * playerVelocity * Time.deltaTime), (movementZ * playerVelocity * Time.deltaTime));

    }

}
