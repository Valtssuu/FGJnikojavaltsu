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

        hit = Physics2D.Raycast(this.transform.position + new Vector3(0, -0.23f, 0), -transform.up, 0.5f);
        if (hit.collider != null)
        {
            isOnTopOfSomething = true;
        }
        else
        {
            isOnTopOfSomething = false;
        }

        if (isBeingHeld == true)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }
        if(isOnTopOfSomething == true)
        {
            if (isSnapped == true)
            {
                this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            else
            {
                this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
        
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            isBeingHeld = true;
        }
        
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "GridSlot")
        {
            if(isOnTopOfSomething == true)
            {
                this.transform.position = collision.gameObject.transform.position;
                isSnapped = true;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GridSlot")
        {
            isSnapped = false;
        }
    }
}
