using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
    
public class FreeLookCamera : MonoBehaviour
{
    
    public CinemachineFreeLook cameraLook;
    
    private void FixedUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                cameraLook.m_YAxis.m_InputAxisName = "Mouse ScrollWheel";
                cameraLook.m_XAxis.m_InputAxisName = "Mouse X";
            }
            else
            {
                cameraLook.m_YAxis.m_InputAxisName = "Mouse ScrollWheel";
                cameraLook.m_XAxis.m_InputAxisName = "";
            }
        }
}

