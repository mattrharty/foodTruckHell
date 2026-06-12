using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NightController : MonoBehaviour
{
    
    //[SerializeField] public static Material[] states;
    private float time;
    private int nightNum;
    private float hungerMult;
    private float speedMult;
    private float spawnCooldown;

    [SerializeField] private Animator fadeOut;
    [SerializeField] private TMP_Text nightText;

    [SerializeField] private Animator jumpscare;

    [SerializeField] private float nightLength;
    [SerializeField] private Transform[] SpawnLoc;
    [SerializeField] private GameObject zombiePrefab;

    private List<Zombie>[] zoms;
    private float spawnTime;

    private bool nightEnded = false;

    public void Start()
    {
        int num = 1;
        time = 0;
        spawnTime = 0;
        if(GameObject.FindGameObjectWithTag("global") != null)
            num = GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>().getNightNum();
        setNight(num);
        zoms = new List<Zombie>[] {new List<Zombie>(), new List<Zombie>(), new List<Zombie>()};
    }

    public void orderUp(int lane, GameObject food)
    {
        Debug.Log(lane);
        StartCoroutine(eat(zoms[lane][0], food));
    }

    public bool zombiePresent(int lane)
    {
        if(zoms[lane].Count < 1)
            return false;
        return true;
    }

    private IEnumerator eat(Zombie z, GameObject food)
    {
        yield return new WaitForSeconds(2);
        int foodVal = food.transform.parent.gameObject.GetComponent<CounterSpot>().calculateFoodValue();
        food.transform.parent.gameObject.GetComponent<CounterSpot>().clear();
        foreach(Ingred ing in food.transform.GetComponentsInChildren<Ingred>())
            Destroy(ing.gameObject);
        if (z.eatFood(foodVal))
        {
            fadeOut.SetTrigger("flicker");
            yield return new WaitForSeconds(1.25f);
            Destroy(z.gameObject);
        }
    }

    public void Update()
    {
        if(!nightEnded && Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
        time += Time.deltaTime;

        if(time >= nightLength && !nightEnded)
        {
            nightEnded = true;
            if (ColorUtility.TryParseHtmlString("#FFF6CB", out Color myColor))
            {
                fadeOut.gameObject.GetComponent<Image>().color = myColor;
            }
            
            fadeOut.SetTrigger("endNight");

            if(GameObject.FindGameObjectWithTag("global") != null)
                GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>().setNightNum(nightNum + 1);
            StartCoroutine(endNight());
        }
    }

    void FixedUpdate()
    {
        spawnTime += Time.fixedDeltaTime;
        if(spawnTime >= spawnCooldown){
            spawnTime = 0;
            int loc = Random.Range(0, 3);
            float speed = speedMult;
            int hunger = Mathf.RoundToInt(hungerMult * 100) - Random.Range(0, 15);
            GameObject newZombie = Instantiate(zombiePrefab, SpawnLoc[loc]);
            newZombie.transform.localPosition = new Vector3();
            newZombie.GetComponent<Animator>().speed = speed;
            newZombie.GetComponent<Zombie>().setHunger(hunger);
            newZombie.GetComponent<Zombie>().setControl(this);
            zoms[loc].Add(newZombie.GetComponent<Zombie>());
        }
    }


    private IEnumerator endNight()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    private IEnumerator gameOver()
    {
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(0);
    } 

    public void die()
    {
        jumpscare.SetTrigger("die");
        StartCoroutine(gameOver());
    }   

    public void setNight(int _nightNum)
    {
        nightNum = _nightNum;
        hungerMult = 0.75f + 0.25f * nightNum;
        speedMult = 0.9f + 0.1f * nightNum;
        spawnCooldown = 8 * Mathf.Atan(-0.8f * (nightNum - 1)) + 15;
        nightText.text = "Night " + nightNum;
    }

}
