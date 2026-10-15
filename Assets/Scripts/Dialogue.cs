using System;
using System.Linq;

/// <summary>
/// A class storing information about a dialogue sequence, intended to be stored in JSON files.
/// </summary>
public class Dialogue
{
    
    /// <summary>
    /// The lane in which the dialogue will be pointing to.
    /// 0 = Left
    /// 1 = Middle
    /// 2 = Right
    /// </summary>
    public int lane;
    /// <summary>
    /// The name of who is speaking the dialogue
    /// </summary>
    public string speakerName;
    /// <summary>
    /// An array of lines to be displayed one by one.
    /// </summary>
    public Line[] lines;

    /// <summary>
    /// The index of the next line to be retrieved.
    /// </summary>
    [NonSerialized]
    private int index;

    /// <summary>
    /// Returns whether the dialogue has another line.
    /// </summary>
    /// <returns>True if there is another line, otherwise false.</returns>
    public bool hasNext()
    {
        return index < lines.Count();
    }

    /// <summary>
    /// Returns the next line of dialogue and increments the index.
    /// </summary>
    /// <returns>The next line of dialogue</returns>
    public Line next()
    {
        index++;
        return lines[index - 1];
    }

    /// <summary>
    /// Returns the next line of dialogue.
    /// </summary>
    /// <returns>The next line of dialogue</returns>
    public Line peek()
    {
        return lines[index];
    }

}

[Serializable]
/// <summary>
/// A class containing a piece of speech to be shown at once
/// </summary>
public class Line
{
    /// <summary>
    /// The emotion of the line, correlating to the sprite of the speaker.
    /// </summary>
    public string emotion;

    /// <summary>
    /// The sprite of the bubble to be used:
    /// 0 = Normal
    /// 1 = Yelling
    /// 2 = Thought
    /// 3 = Tired
    /// 4 = Afar
    /// </summary>
    public int bubbleSprite;

    /// <summary>
    /// The actual string containing the line of speech.
    /// </summary>
    public string line;
}
