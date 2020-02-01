using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    public enum CubeColor
    {
        RedCube,
        YellowCube,
        BlueCube
    }

    public CubeColor cubeColor;
    private string gridColor;
    private float startPosX = 0;
    private float startPosY = 0;
    private bool isBeingHeld = false;
    public bool isSnapped = false;
    private bool isOnTopOfSomething = false;
    // Boolean flag to check if object is colliding with ground object
    private bool borderCollision = false;
    private List<string> borderCollisions = new List<string>();

    public RaycastHit2D hit;

    void Start()
    {
        switch (cubeColor)
        {
            case CubeColor.RedCube:
                gridColor = "GridRed";
                break;
            case CubeColor.YellowCube:
                gridColor = "GridYellow";
                break;
            case CubeColor.BlueCube:
                gridColor = "GridBlue";
                break;            
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Reset object position if it falls offscreen
        if (this.gameObject.transform.position.y <= -30)
        {
            this.gameObject.transform.position = new Vector3(startPosX, startPosY, 0);
        }

        // casting ray down 
        hit = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.33f, 0), -transform.up, 0.1f, 1 << 8);

        //if ray hits something we change boolean "isOnTopOfSomething" to true and false if it doesn't hit anything
        if (hit.collider != null)
        {
            isOnTopOfSomething = true;
        }
        else
        {
            isOnTopOfSomething = false;
        }

        //when piece is held by mouse/touch

        if (isBeingHeld == true)
        {
            // New y-axis position when dragging an object
            float newPosX, newPosY;
            // Object's xy-axis components before checking curcor dragging
            float objectPosX = this.gameObject.transform.position.x;
            float objectPosY = this.gameObject.transform.position.y;
            // Booleans to check if dragging along either of the axes should be restricted
            bool restrictX = false;
            bool restrictY = false;

            //gravity is disabled and piece follows mouses/touches position
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            if (borderCollision == true)
            {
                // Iterate through all colliding objects and set axes to restrict
                foreach (string collObject in borderCollisions)
                {
                    switch (collObject)
                    {
                    case "LeftWall":
                    case "RightWall":
                        restrictX = true;
                        break;
                    case "Ground":
                        restrictY = true;
                        break;
                    }
                }
            }

            // Stop restricting either of the axes if object is dragged towards the centre
            if (restrictX && Mathf.Abs(mousePos.x) < Mathf.Abs(objectPosX))
            {
                Debug.Log("YEET");
                restrictX = false;
            }

            if (restrictY && (objectPosY < mousePos.y))
            {
                restrictY = false;
            }
            
            // If no axes are restricted the object is not colliding with screen borders
            if (!restrictX && !restrictX && !restrictY && borderCollision)
            {
                borderCollision = false;
            }

            // Set new x and y coordinates for the object depending on if axes are restricted or not
            if (restrictX)
            {
                newPosX = objectPosX;
            }
            else
            {
                newPosX = mousePos.x;
            }

            if (restrictY)
            {
                newPosY = objectPosY;
            }

            // Object is not in collision and can move freely with cursor
            else
            {
                newPosY = mousePos.y;
            }

            this.gameObject.transform.localPosition = new Vector3(newPosX, newPosY, 0);
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        //if piece has snapped to grid
        //rigidbody changes from dynamic to kinematic

        if (isSnapped == true)
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnMouseDown()
    {
        //when player holds mouse or touch isBeingHeld changes to true
        if (isSnapped == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isBeingHeld = true;
            }
        }
    }

    private void OnMouseUp()
    {
        //on realease isBeingHeld changes to false
        isBeingHeld = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Check if trigger is an object with screen border tag
        if (collision.gameObject.tag == "ScreenBorder")
        {
            borderCollision = true;

            borderCollisions.Add(collision.gameObject.name);
        }

        //when inside gridslots trigger piece snaps to it
        if (isBeingHeld == false)
        {
            if (collision.gameObject.tag == gridColor)
            {
                if (isOnTopOfSomething == true)
                {
                    this.transform.position = collision.gameObject.transform.position;
                    isSnapped = true;
                }
                else
                {
                    isSnapped = false;
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when exiting trigger snapping stops
        if (collision.gameObject.tag == gridColor)
        {
            isSnapped = false;
        }
    }
}
