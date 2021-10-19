using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentControl : MonoBehaviour
{
    [Header("Hover")]
    public bool hoverEnabled = false;
    public float hoverSpeed = 1f;
    public float hoverHeight = 4f;

    [Header("Spin")]
    public bool spinEnabled = false;
    public Quaternion spinSpeed = Quaternion.Euler(0f, 0.2f, 0f);

    [Header("Rotate Around")]
    public bool rotateAroundEnabled = false;
    public float rotateSpeed = 1f;
    public Transform piviot;

    [Header("Cloud")]
    public bool cloudEnabled = false;
    public float cloudSpeed = 10f;
    public Transform start;
    public Transform end;

    private float hoverStart;
    private float hoverGoal;
    private bool hoverUp = true;


    // Start is called before the first frame update
    void Start()
    {
        hoverStart = transform.position.y;
        hoverGoal = transform.position.y + (hoverHeight / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (hoverEnabled)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, hoverGoal, Time.deltaTime * hoverSpeed), transform.position.z);

            if (Mathf.Abs(hoverGoal - transform.position.y) < 0.2f)
            {
                hoverUp = !hoverUp;
                hoverGoal = hoverUp ? transform.position.y + (hoverHeight) : transform.position.y - (hoverHeight);
            }
        }

        if (spinEnabled)
        {
            transform.rotation *= spinSpeed;
        }
    }
}

// Cloud
// Go up and down (Crystal, Floating Island)
// Rotate Around Object