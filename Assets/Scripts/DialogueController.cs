using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// The controller attached to a GameObject in scene to manage the playing of dialogue.
/// </summary>
public class DialogueController : MonoBehaviour
{
    
    /// <summary>
    /// The Image component for the dialogue bubble in scene.
    /// </summary>
    public Image dialogueBubble;

    /// <summary>
    /// The text asset where dialogue is displayed.
    /// </summary>
    public TMP_Text text;

    /// <summary>
    /// The text asset showing the speaker's name.
    /// </summary>
    public TMP_Text speakerName;

    /// <summary>
    /// 0-2 = Normal
    /// 3-5 = Yelling
    /// 6-8 = Thought
    /// 9-11 = Tired
    /// 12-14 = Afar
    /// </summary>
    public Sprite[] bubbleTypes;

    /// <summary>
    /// Keeps track of whether there is currently dialogue being run.
    /// </summary>
    public bool diagRunning;

    /// <summary>
    /// Runs on the start of the scene to disable the dialogue bubble.
    /// </summary>
    public void Start()
    {
        dialogueBubble.transform.parent.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Displays the given dialogue on screen.
    /// </summary>
    /// <param name="d">The dialogue to be played.</param>
    /// <param name="locked">Whether the dialogue will lock the player's input to force them to click through the dialogue.</param>
    public void showDialogue(Dialogue d, bool locked)
    {
        dialogueBubble.transform.parent.gameObject.SetActive(true);
        diagRunning = true;
        StartCoroutine(runDialogue(d, locked));
    }

    /// <summary>
    /// A method that runs each line of dialogue, waiting for the player to click to continue.
    /// </summary>
    /// <param name="d">The dialogue to be played.</param>
    /// <param name="locked">Whether the player is required to interact with the dialogue to.</param>
    /// <returns></returns>
    private IEnumerator runDialogue(Dialogue d, bool locked)
    {
        speakerName.text = d.speakerName;
        int lane = d.lane;
        if(locked){
            while (d.hasNext())
            {
                Line l = d.next();
                text.text = l.line;
                dialogueBubble.sprite = bubbleTypes[3 * l.bubbleSprite + lane];
                //Add emotion implementation;
                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }
        }
        diagRunning = false;
        dialogueBubble.transform.parent.gameObject.SetActive(false);
    }


}
