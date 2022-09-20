using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    [SerializeField] private float idleAfterSec = 10;

    private GameObject currentPanel;
    private float idleTime = 0;
    private bool idle = false;
    private Vector3 lastMousePos = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKey && Vector3.Distance(Input.mousePosition, lastMousePos) <= 0.1f)
        {
            idleTime += Time.deltaTime;
        }
        else
        {
            idleTime = 0;
            if (idle)
            {
                currentPanel.SetActive(true);
                idle = false;
            }
        }

        lastMousePos = Input.mousePosition;

        if (idleTime >= idleAfterSec && !idle)
        {
            foreach (var panel in panels)
            {
                if (panel.activeSelf)
                {
                    currentPanel = panel;
                    currentPanel.SetActive(false);
                    idle = true;
                    break;
                }
            }
        }
    }
}
