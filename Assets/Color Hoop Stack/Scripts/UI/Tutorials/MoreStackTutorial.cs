using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MoreStackTutorial : MonoBehaviour
{
    public Animator animator;
    
    private void OnEnable()
    {
        animator.Play("MoreStackControllerAnim");
       
    }

    
}
