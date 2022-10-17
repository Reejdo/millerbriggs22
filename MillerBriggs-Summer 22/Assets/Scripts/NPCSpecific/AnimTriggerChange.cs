using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTriggerChange : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator myAnim;
    public AnimationClip defaultAnim;
    public AnimationClip enterAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myAnim.Play(enterAnim.name); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            myAnim.Play(defaultAnim.name);
        }
    }
}
