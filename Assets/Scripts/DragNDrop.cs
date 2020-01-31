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
    // Update is called once per frame
    void Update()
    {
        if(isBeingHeld == true)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.rotation = Quaternion.identity;

            this.gameObject.transform.localPosition = new Vector3(mousePos.x, mousePos.y, 0);
        }

        if(isSnapped == true)
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
        else
        {
            this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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
            this.transform.position = collision.gameObject.transform.position;
            isSnapped = true;
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
