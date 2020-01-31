using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    // Start is called before the first frame update

    private float startPosX;
    private float startPosY;
    private bool isBeingHeld = false;
    private bool isSnapped = false;
    private bool isOnTopOfSomething = false;

    public RaycastHit2D hit;
    // Update is called once per frame
    void Update()
    {

        // casting ray down 
        hit = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.24f, 0), -transform.up, 0.1f, 1 << 8);
        Debug.DrawRay(this.transform.position + new Vector3(0, -0.24f, 0), -transform.up);

        //if ray hits something we change boolean "isOnTopOfSomething" to true and false if it doesn't hit anything
        if (hit.collider != null)
        {
            isOnTopOfSomething = true;
        }
        else
        {
            isOnTopOfSomething = false;
        }
        

        Debug.Log(isOnTopOfSomething);


        //when piece is held by mouse/touch
        if (isBeingHeld == true)
        {
            //gravity is disabled and piece follows mouses/touches position
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }

        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        //if piece is on top of something and it has snapped to grid
        //rigidbody changes from dynamic to kinematic

        if(isOnTopOfSomething == true)
        {
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
        
    }

    private void OnMouseDown()
    {
        //when player holds mouse or touch isBeingHeld changes to true
        if (Input.GetMouseButtonDown(0))
        {
            isBeingHeld = true;
        }
        
    }

    private void OnMouseUp()
    {
        //on realease isBeingHeld changes to false
        isBeingHeld = false;
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //when inside gridslots trigger piece snaps to it
        if(isBeingHeld == false)
        {
            if (collision.gameObject.tag == "GridSlot")
            {
                if (isOnTopOfSomething == true)
                {
                    this.transform.position = collision.gameObject.transform.position;
                    isSnapped = true;
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
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
        if (collision.gameObject.tag == "GridSlot")
        {
            isSnapped = false;
        }
    }
}
