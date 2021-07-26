using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    float speed = 0;
    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        // Calculate Swipe Length
        if (Input.GetMouseButtonDown(0))
        {
            this.startPos = Input.mousePosition; // Position when click
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Position when click off
            Vector2 endPos = Input.mousePosition;
            float swipeLength = endPos.x - startPos.x;

            // Change length of swipe to Start speed
            this.speed = swipeLength / 500.0f;

            // Play Audio Clip
            GetComponent<AudioSource>().Play();
        }

        transform.Translate(this.speed, 0, 0); // Move Car by speed
        this.speed *= 0.98f; // Deceleration
    }
}
