using UnityEngine;
using TMPro;

/// <summary>
/// The controller attached to a GameObject in scene to manage the playing of dialogue.
/// </summary>
public class DialogueController : MonoBehaviour
{
    
    /// <summary>
    /// The Sprite Renderer for the dialogue bubble in scene.
    /// </summary>
    public SpriteRenderer dialogueBubble;

    /// <summary>
    /// The text asset where dialogue is displayed.
    /// </summary>
    public TMP_Text text;

    /// <summary>
    /// 0-2 = Normal
    /// 3-5 = Yelling
    /// 6-8 = Thought
    /// 9-11 = Tired
    /// </summary>
    public Sprite[] bubbleTypes;
    
    /// <summary>
    /// Displays the given dialogue on screen.
    /// </summary>
    /// <param name="d"></param> <summary>
    /// The dialogue to be played.
    /// </summary>
    /// <param name="d"></param>
    /// <param name="locked"></param> <summary>
    /// Whether the dialogue will lock the player's input to force them to click through the dialogue.
    /// </summary>
    /// <param name="locked"></param>
    public void showDialogue(Dialogue d, bool locked)
    {
        
    }


}
