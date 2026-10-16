using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// The controller attached to a GameObject in scene to manage the playing of dialogue.
/// </summary>
public class DialogueController : MonoBehaviour
{
    
    [Header("Dialogue Bubble Canvas Elements")]
    /// <summary>
    /// The Image component for the dialogue bubble in scene.
    /// </summary>
    [Tooltip("The Image component for the dialogue bubble in scene.")]
    public Image dialogueBubble;

    /// <summary>
    /// The text asset where dialogue is displayed.
    /// </summary>
    [Tooltip("The text asset where dialogue is displayed.")]
    public TMP_Text text;

    /// <summary>
    /// The text asset showing the speaker's name.
    /// </summary>
    [Tooltip("The text asset showing the speaker's name.")]
    public TMP_Text speakerName;

    [Space]
    /// <summary>
    /// A Sprite Array containing sprites depicting the different variants of dialogue bubbles, with offset for which lane is being spoken from.
    /// 0-2 = Normal
    /// 3-5 = Yelling
    /// 6-8 = Thought
    /// 9-11 = Tired
    /// 12-14 = Afar
    /// </summary>
    [Tooltip("A Sprite Array containing sprites depicting the different variants of dialogue bubbles, with offset for which lane its being spoken from.\n0-2 = Normal\n3-5 = Yelling\n6-8 = Thought\n9-11 = Tired\n12-14 = Afar")]
    public Sprite[] bubbleTypes;

    /// <summary>
    /// Runs on the start of the scene to disable the dialogue bubble.
    /// </summary>
    public void Start()
    {
        dialogueBubble.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// A method that runs each line of dialogue, waiting for the player to click to continue.
    /// </summary>
    /// <param name="d">The dialogue to be played.</param>
    /// <param name="locked">Whether the player is required to interact with the dialogue to.</param>
    /// <returns></returns>
    public IEnumerator playDialogue(Dialogue d, bool locked)
    {
        dialogueBubble.transform.parent.gameObject.SetActive(true);
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
        dialogueBubble.transform.parent.gameObject.SetActive(false);
    }


}
