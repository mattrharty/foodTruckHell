using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Zombie : MonoBehaviour
{

    private int hunger;
    private int speed;
    private NightController control;

    public void Start()
    {
        hunger = 100;
        GetComponent<NavMeshAgent>().SetDestination(new Vector3());
    }

    public void setHunger(int _hunger)
    {
        hunger = _hunger;
    }

    public bool eatFood(int foodVal)
    {
        hunger -= foodVal;
        StartCoroutine(eatAnim(foodVal));
        return hunger <= 0;
    }

    public IEnumerator eatAnim(int foodVal)
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("eat");
        GetComponent<NavMeshAgent>().speed = 0;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        GetComponent<NavMeshAgent>().speed = 1;
    }

    public void setControl(NightController _control)
    {
        control = _control;
    }

    public void Update()
    {
        if(transform.position.z < 6){
            control.die();
            Destroy(gameObject);
        }

        transform.GetChild(0).eulerAngles = new Vector3();
        if(transform.eulerAngles.x > 0)
            transform.GetChild(0).eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    }

    public IEnumerator kill()
    {
        Animator anim = GetComponent<Animator>();

        anim.SetTrigger("kill");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        Destroy(this);
    }

}
