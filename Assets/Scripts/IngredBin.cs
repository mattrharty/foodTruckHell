using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to Ingredient Bins used to instantiate ingredients when clicked on.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class IngredBin : MonoBehaviour
{

    /// <summary>
    /// The ingredient prefab to be instantiated on interaction.
    /// </summary>
    [Tooltip("The ingredient prefab to be instantiated on interaction.")]
    [SerializeField] private GameObject ingredPrefab;

    /// <summary>
    /// Returns an instantiation of the ingredient prefab.
    /// </summary>
    /// <returns>Instantiation of the <c>ingredPrefab</c></returns>
    public GameObject getIngred()
    {
        return Instantiate(ingredPrefab);
    }

}
