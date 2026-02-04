using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MinimapIcons : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Minimap minimap;
    [SerializeField] private GameObject iconPrefab;

    [Header("---Icon Settings---")]
    [SerializeField] private float iconScale = 1f;
    [SerializeField] private Color[] playerColors = { Color.blue, Color.red, Color.green, Color.yellow };

    private Dictionary<RacerData, RectTransform> iconMap = new Dictionary<RacerData, RectTransform>();

    void Update()
    {
        var allRacers = FindObjectsByType<RacerData>(FindObjectsSortMode.None);

        foreach (var racer in allRacers)
        {
            if (!iconMap.ContainsKey(racer))
            {
                CreateIconForRacer(racer);
            }
            iconMap[racer].anchoredPosition = minimap.GetWorldToMinimap(racer.transform.position);



            float angle = racer.transform.eulerAngles.y;
            iconMap[racer].localRotation = Quaternion.Euler(0, 0, -angle + 180); //Det måste vara -angle + 180 annars blir turningen inversed på kartan
        }

        var keysToRemove = iconMap.Keys.Where(r => r == null).ToList();
        foreach (var key in keysToRemove)
        {
            Destroy(iconMap[key].gameObject);
            iconMap.Remove(key);
        }
    }

    void CreateIconForRacer(RacerData racer)
    {
        GameObject go = Instantiate(iconPrefab, transform);
        RectTransform rt = go.GetComponent<RectTransform>();

        rt.localScale = Vector3.one * iconScale;
        rt.localScale = new Vector3(iconScale/2, iconScale, iconScale);

        Image img = go.transform.Find("PlayerIcon").GetComponent<Image>();
        if (img != null)
        {
            img.color = playerColors[iconMap.Count % playerColors.Length];
        }

        iconMap.Add(racer, rt);
    }
}