using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NightController : MonoBehaviour
{
    /// <summary>
    /// The accumulated amount of time since the last wave spawned. Must be greater than or equal to 0.
    /// </summary>
    private float timeSinceWave;
    /// <summary>
    /// The multiplier that affects the hunger of the zombies during the night.
    /// </summary>
    private float hungerMult;
    /// <summary>
    /// The multiplier that affects the speed of the zombies during the night.
    /// </summary>
    private float speedMult;

    /// <summary>
    /// Tracks night progress
    /// 0 = Shutter to be opened
    /// 1 = Merchant
    /// 2 = Zombie attack
    /// 3 = Boss
    /// 4 = Shutter to be closed
    /// 5 = Shutter closed
    /// </summary>
    [System.NonSerialized]public int nightProg = 0;

    /// <summary>
    /// The current wave of zombies being spawned.
    /// </summary>
    private ZombieWave currentWave;

    [Header("HUD References")]
    /// <summary>
    /// The animator that controls the fade in and out of the screen.
    /// </summary>
    [Tooltip("The animator that controls the fade in and out of the screen.")]
    [SerializeField] private Animator fadeOut;
    /// <summary>
    /// The text that shows the current night.
    /// </summary>
    [Tooltip("The text that shows the current night.")]
    [SerializeField] private TMP_Text nightText;

    [Space][Header("Zombie-Related Variables")]
    /// <summary>
    /// The animator that controls the jumpscare upon player death.
    /// </summary>
    [Tooltip("The animator that controls the jumpscare upon player death.")]
    [SerializeField] private Animator jumpscare;
    /// <summary>
    /// The relative difficultly level of the zombies.
    /// </summary>
    [Tooltip("The relative difficultly level of the zombies.")]
    [SerializeField] private int zombAI;
    /// <summary>
    /// An array of Transform components depicting the possible spawn locations of the zombies.
    /// </summary>
    [Tooltip("An array of Transform components depicting the possible spawn locations of the zombies.")]
    [SerializeField] private Transform[] SpawnLoc;
    /// <summary>
    /// The prefab of the zombie to be spawned.
    /// </summary>
    [Tooltip("The prefab of the zombie to be spawned.")]
    [SerializeField] private GameObject zombiePrefab;

    [Space][Header("General References")]
    /// <summary>
    /// The text asset containing the <c>nightData</c> to be used for the current night.
    /// </summary>
    [Tooltip("The text asset containing the nightData to be used for the current night.")]
    [SerializeField] private TextAsset defaultNightData;
    /// <summary>
    /// A reference the Dialogue Controller.
    /// </summary>
    [Tooltip("A reference the Dialogue Controller.")]
    [SerializeField] private DialogueController dc;

    [Space][Header("Merchant References")]
    /// <summary>
    /// The animator that controls the merchant.
    /// </summary>
    [Tooltip("The animator that controls the merchant.")]
    [SerializeField] private Animator merchantAnim;
    /// <summary>
    /// The text asset containing the dialogue for the merchant.
    /// </summary>
    [Tooltip("The text asset containing the dialogue for the merchant.")]
    [SerializeField] private TextAsset merchDiag;

    /// <summary>
    /// Tracks whether the player is currently in dialogue.
    /// </summary>
    private bool inDiag;

    /// <summary>
    /// The <c>nightData</c> to be used for the night.
    /// </summary>
    private nightData night;

    /// <summary>
    /// An array of queues of zombies storing references to the zombies in each lane.
    /// </summary>
    private Queue<Zombie>[] zoms;

    /// <summary>
    /// The total amount of zombies to be spawned in the wave.
    /// </summary>
    private int waveTotal;
    /// <summary>
    /// The amount of zombies killed during the current wave.
    /// </summary>
    private int waveDead;

    /// <summary>
    /// The amount of time between zombies of the same wave spawning.
    /// </summary>
    private float nextZom = 1.0f;
    /// <summary>
    /// The last lane that a zombie was spawned in.
    /// </summary>
    private int lastLane = -1;

    // Start is called before the first frame update
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

    /// <summary>
    /// Sends the given food down the given lane to the front zombie in that lane.
    /// </summary>
    /// <param name="lane">The lane for which <c>food</c> is sent down.</param>
    /// <param name="food">The food to be sent down <c>lane</c>.</param>
    public void orderUp(int lane, GameObject food)
    {
        //Debug.Log("Launching at zombie in lane " + lane);
        StartCoroutine(eat(zoms[lane].Peek(), food, lane));
    }

    /// <summary>
    /// Checks whether there is a zombie present in a given lane.
    /// </summary>
    /// <param name="lane">The lane to be checked.</param>
    /// <returns><c>true</c> if there is a zombie present in the lane, otherwise <c>false</c></returns>
    public bool zombiePresent(int lane)
    {
        if(zoms[lane].Count < 1)
            return false;
        return true;
    }

    /// <summary>
    /// Removes the food from the counter and feeds it to the zombie. If the zombie's <c>hunger</c> is less than or equal to 0, than they are removed.
    /// </summary>
    /// <param name="z">The zombie to feed.</param>
    /// <param name="food">The food to be fed to <c>z</c></param>
    /// <param name="lane">The lane that <c>z</c> is in.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Increments <c>nightProg</c> by 1.
    /// </summary>
    public void incrementProg()
    {
        nightProg++;
        Debug.Log("Incrementing to progress " + nightProg);
    }

    /// <summary>
    /// Spawns a zombie of given type in the given lane.
    /// </summary>
    /// <param name="type">The type of zombie to spawn.</param>
    /// <param name="lane">The lane in which to spawn the zombie.</param>
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

    /// <summary>
    /// Summons the merchant and makes them leave once their dialogue is done.
    /// </summary>
    /// <returns></returns>
    private IEnumerator showMerchant()
    {
        merchantAnim.SetTrigger("enter");
        yield return StartCoroutine(dc.playDialogue(JsonUtility.FromJson<Dialogue>(merchDiag.text), true));
        merchantAnim.SetTrigger("leave");
        incrementProg();
        GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>().forceInteract();
        inDiag = false;
    }

    // Update is called once per frame
    public void Update()
    {      

        bool nextWaveReady = false;

        if(nightProg == 1 && !inDiag)
        {
            inDiag = true;
            StartCoroutine(showMerchant());
        }


        if(nightProg < 2)
            return;

        timeSinceWave += Time.deltaTime;

        if (!night.hasNextWave())
        {
            if(waveTotal > 0 && Mathf.RoundToInt(waveDead / (float)waveTotal) == 1)
                nightProg = 4;
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
                lastLane = lane;
                spawnZombie(currentWave.getNextZombie(), lane);
            }
        }

        if(nightProg == 5)
        {
            nightProg++;
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

    /// <summary>
    /// Reloads the scene after 2 seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator endNight()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    /// <summary>
    /// Loads the main menu after 0.75 seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator gameOver()
    {
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(0);
    } 

    /// <summary>
    /// Triggers the jumpscare and reloads the scene.
    /// </summary>
    public void die()
    {
        jumpscare.SetTrigger("die");
        StartCoroutine(gameOver());
    }   

    /// <summary>
    /// Sets the night's multipliers and night name according the given <c>nightData</c>.
    /// </summary>
    /// <param name="n">The <c>nightData</c> to pull <c>zombAI</c> and <c>name</c> from.</param>
    public void setNight(nightData n)
    {
        hungerMult = 0.75f + 0.25f * n.getZombAI();
        speedMult = 0.9f + 0.1f * n.getZombAI();;
        nightText.text = n.getName();
    }

}
