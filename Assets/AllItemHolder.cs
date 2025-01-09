using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AllItemHolder : MonoBehaviour
{
    public List<DecorationData> DecorationData = new List<DecorationData>(); // Assign all your decorations in the Inspector

    public static AllItemHolder instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); // Ensure it persists across scenes
        Debug.Log("AllItemHolder instance set and persists across scenes.");

        // Additional debug log to check the state of instance and DecorationData
        if (instance != null)
        {
            Debug.Log("AllItemHolder instance is not null.");
            if (DecorationData == null)
            {
                Debug.LogError("DecorationData is null!");
            }
            else
            {
                Debug.Log($"DecorationData has {DecorationData.Count} items.");
            }
        }
        else
        {
            Debug.LogError("AllItemHolder instance is null.");
        }
    }

    public DecorationData GetDecorationByName(string name)
    {
        if (DecorationData != null)
        {
            // Log the available decoration names in the list for debugging
            string msg = "Available Decorations: " + string.Join(", ", DecorationData.Select(d => d.itemName));
            GameManager.Instance.lol.text = msg;
            return DecorationData.Find(d => d.itemName.Equals(name));
        }
        else
        {
            Debug.LogError("AllItemHolder instance or DecorationData is null!");
            return null;
        }
    }

    public static List<DecorationData> GetDecorationsByNames(List<string> names)
    {
        List<DecorationData> decorations = new List<DecorationData>();
        foreach (string name in names)
        {
            DecorationData decoration = AllItemHolder.instance.GetDecorationByName(name);
            if (decoration != null)
            {
                decorations.Add(decoration);
            }
            else
            {
                Debug.LogWarning($"Decoration with name {name} not found.");
            }
        }
        return decorations;
    }
}
