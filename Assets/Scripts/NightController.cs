using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NightController : MonoBehaviour
{
    private float timeSinceWave;
    private float hungerMult;
    private float speedMult;

    private ZombieWave currentWave;

    [SerializeField] private Animator fadeOut;
    [SerializeField] private TMP_Text nightText;

    [SerializeField] private Animator jumpscare;

    [SerializeField] private int zombAI;

    [SerializeField] private Transform[] SpawnLoc;
    [SerializeField] private GameObject zombiePrefab;

    [SerializeField] private TextAsset defaultNightData;

    private nightData night;

    private Queue<Zombie>[] zoms;

    private int waveTotal;
    private int waveDead;

    private float nextZom = 1.0f;
    private int lastLane = -1;

    private bool nightEnded = false;

    public void Start()
    {
        timeSinceWave = 0;
        if(GameObject.FindGameObjectWithTag("global") != null)
            night = GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>().getNightData();
        else
            night = JsonUtility.FromJson<nightData>(defaultNightData.text);
        setNight(night);
        zoms = new Queue<Zombie>[] {new Queue<Zombie>(), new Queue<Zombie>(), new Queue<Zombie>()};
    }

    public void orderUp(int lane, GameObject food)
    {
        //Debug.Log("Launching at zombie in lane " + lane);
        StartCoroutine(eat(zoms[lane].Peek(), food, lane));
    }

    public bool zombiePresent(int lane)
    {
        if(zoms[lane].Count < 1)
            return false;
        return true;
    }

    private IEnumerator eat(Zombie z, GameObject food, int lane)
    {
        int foodVal = food.GetComponent<CounterSpot>().calculateFoodValue();
        food.GetComponent<CounterSpot>().clear(z.transform);
        yield return new WaitForSeconds(2);
        if (z.eatFood(foodVal))
        {
            //fadeOut.SetTrigger("flicker");
            yield return new WaitForSeconds(1.25f);
            StartCoroutine(z.kill());
            waveDead++;
            zoms[lane].Dequeue();
        }
    }

    public void spawnZombie(string type, int lane)
    {
        //Spawns zombiiies
        //Debug.Log("Spawning zombie in lane " + (lane + 1));

        waveTotal++;

        float speed = speedMult;
        int hunger = Mathf.RoundToInt(hungerMult * 100) - Random.Range(0, 15);
        GameObject newZombie = Instantiate(zombiePrefab, SpawnLoc[lane]);
        newZombie.transform.localPosition = new Vector3();
        //newZombie.GetComponent<Animator>().speed = speed;
        newZombie.GetComponent<Zombie>().setHunger(hunger);
        newZombie.GetComponent<Zombie>().setControl(this);
        zoms[lane].Enqueue(newZombie.GetComponent<Zombie>());
    }

    public void Update()
    {
        timeSinceWave += Time.deltaTime;
        

        bool nextWaveReady = false;
        bool nightDone = false;

        if (!night.hasNextWave())
        {
            nightDone = waveTotal > 0 && Mathf.RoundToInt(waveDead / (float)waveTotal * 100) == 100;
        }   
        else if((currentWave == null && timeSinceWave >= night.getStartDelay()) || (currentWave != null && !currentWave.hasNextZombie()))
        {
            if(currentWave == null)
                nextWaveReady = true;
            currentWave = night.getNextWave();
        }

        if(currentWave != null){
            if(waveTotal > 0 && currentWave.isReadyToSpawn(timeSinceWave, waveDead / (float)waveTotal))
            {
                nextWaveReady = true;
                waveDead = 0;
                waveTotal = 0;
                timeSinceWave = 0;
            }

            if(timeSinceWave > nextZom && (nextWaveReady || !currentWave.isFirstZombie()))
            {
                nextZom = timeSinceWave + 1.0f;
                int lane = Random.Range(0, 3);
                while(lane == lastLane)
                    lane = Random.Range(0, 3);
                spawnZombie(currentWave.getNextZombie(), lane);
            }
        }

        if(nightDone && !nightEnded)
        {
            nightEnded = true;
            if (ColorUtility.TryParseHtmlString("#FFF6CB", out Color myColor))
            {
                fadeOut.gameObject.GetComponent<Image>().color = myColor;
            }
            
            fadeOut.SetTrigger("endNight");

            if(GameObject.FindGameObjectWithTag("global") != null)
                GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>().incrementNightNum();
            StartCoroutine(endNight());
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

    public void setNight(nightData n)
    {
        hungerMult = 0.75f + 0.25f * n.getZombAI();
        speedMult = 0.9f + 0.1f * n.getZombAI();;
        nightText.text = n.getName();
    }

}
