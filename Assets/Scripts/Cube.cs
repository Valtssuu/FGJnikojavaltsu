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
    public Bomb bomb;
    private string gridColor;
    private float startPosX = 0;
    private float startPosY = 0;
    private bool isBeingHeld = false;
    public bool isSnapped = false;
    private bool isOnTopOfSomething = false;
    // Boolean flag to check if object is colliding with ground object
    private bool groundCollision = false;
    public GameObject particleStar;
    public RaycastHit2D hit;

    void Start()
    {
        bomb = GameObject.FindGameObjectWithTag("Bomb").GetComponent<Bomb>();
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
        hit = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.53f, 0), -transform.up, 0.1f, 1 << 8);

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
            float newPosY;
            // Object's y-axis position before checking dragging
            float objectPosY = this.gameObject.transform.position.y;

            //gravity is disabled and piece follows mouses/touches position
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            // If object is in collision with ground and mouse y-position is 
            // high enough, reset ground collision state
            if (groundCollision == true && mousePos.y > objectPosY)
            {
                groundCollision = false;
            }

            // If ground collision flag is set, prevent y-axis movement
            if (groundCollision == true)
            {
                newPosY = objectPosY;
            }

            // Object is not in collision and can move freely with cursor
            else
            {
                newPosY = mousePos.y;
            }

            this.gameObject.transform.localPosition = new Vector3(mousePos.x, newPosY, 0);
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
            particleStar.SetActive(true);
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
        if (collision.gameObject.tag == "Ground")
        {
            groundCollision = true;
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

    

    public void Blow()
    {
        float distance = Vector2.Distance(bomb.transform.position, this.transform.position);
        Vector3 bombForce = bomb.kiloTons * (transform.position - bomb.transform.position) / (distance * distance);
        Debug.Log("Bombs away!");
        this.GetComponent<Rigidbody2D>().AddForce(bombForce, ForceMode2D.Impulse);
        
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
