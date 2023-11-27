using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreStackTutorial : MonoBehaviour
{
    public Animator animator;
    private void OnEnable()
    {
        animator.Play("MoreStackControllerAnim");
    }
}
