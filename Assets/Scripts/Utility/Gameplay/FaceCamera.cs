using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour
{

    /// <summary>
    /// By default, will take the main camera
    /// </summary>
    public Transform targetCamera = null;
    public bool ChooseInverseCamera = false;

    void Awake()
    {
        if (targetCamera == null)
        {
            if (!ChooseInverseCamera)
            {
                GameObject go = GameObject.FindGameObjectWithTag("MainCamera");
                if (go != null)
                    targetCamera = go.transform;
                else
                    Debug.LogError("Could not find main camera ! Make sure there is a valid camera for FaceCamera Script");
            }
        }
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(targetCamera.transform.position - transform.position);
    }
}