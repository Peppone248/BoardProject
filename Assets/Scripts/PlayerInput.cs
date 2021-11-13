using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float m_h;
    private float m_v;
    private bool m_inputEnabled = false;
    public float H
    {
        get { return m_h; }
    }
    public float V
    {
        get { return m_v; }
    }
    public bool InputEnabled
    {
        get { return m_inputEnabled; }
        set { m_inputEnabled = value; }
    }

   /* public float GetH()
    {
        return h;
    }
    public void SetH(float myH)
    {
        h = myH;
    }
    public float GetV()
    {
        return v;
    }
    public void SetV(float myV)
    {
        v = myV;
    }
    public bool GetInputEnabled()
    {
        return inputEnabled;
    }
    public void SetInputEnabled(bool myValue) 
    {
        inputEnabled = myValue;
    } */

    public void GetKeyInput()
    {
        if (m_inputEnabled) {
            m_h = Input.GetAxisRaw("Horizontal");
            m_v = Input.GetAxisRaw("Vertical");
        }
        
    }

}
