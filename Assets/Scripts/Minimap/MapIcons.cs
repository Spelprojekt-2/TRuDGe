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
    [SerializeField] private Sprite[] playerIcons = new Sprite[4]; //Lars, Nina, Capow, Napoleon
    [SerializeField] private float iconScale = 1f;

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



            //float angle = racer.transform.eulerAngles.y;
            //iconMap[racer].localRotation = Quaternion.Euler(0, 0, -angle + 180); //Det m�ste vara -angle + 180 annars blir turningen inversed p� kartan
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
        rt.localScale = new Vector3(iconScale, iconScale, iconScale);

        Image img = go.transform.Find("PlayerIcon").GetComponent<Image>();
        if (img != null)
        {
            if (racer.racername != null)
            {
                switch (racer.racername)
                {
                    case "Lars-Göran":
                        img.sprite = playerIcons[0];
                        break;
                    case "The Brass Beast":
                        Debug.Log("Changed icon");
                        img.sprite = playerIcons[1];
                        break;
                    case "Capôw":
                        img.sprite = playerIcons[2];
                        break;
                    case "King Napoleon III":
                        img.sprite = playerIcons[3];
                        break;
                }
            }
        }
        iconMap.Add(racer, rt);
    }
}