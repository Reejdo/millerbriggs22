using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisappear : MonoBehaviour
{
    public DialogueObject myDialogueObject;
    [SerializeField] private float animTime;
    private Animator myAnim;
    [SerializeField] private string triggerName;


    // Start is called before the first frame update
    void Start()
    {
        myAnim= GetComponent<Animator>();
    }

    private void Update()
    {

    }

    public void DisappearAnim()
    {
        StartCoroutine(Disappear()); 
    }

    IEnumerator Disappear()
    {
        myAnim.SetTrigger(triggerName);

        yield return new WaitForSeconds(animTime);

        gameObject.SetActive(false); 
    }

}
