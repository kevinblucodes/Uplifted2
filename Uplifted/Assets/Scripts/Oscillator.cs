using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    //[SerializeField] [Range(0,1)] float movementFactor;
    float movementFactor;
    [SerializeField] float period = 2f;
    // Start is called before the first frame update
    void Start()
    {

        startingPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinewave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinewave + 1f) / 2f; //recalculated from 0 to 1 for simplicity

        //Debug.Log(rawSinewave);

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
