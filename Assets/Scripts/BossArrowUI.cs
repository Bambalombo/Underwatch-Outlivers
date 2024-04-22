using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArrowUI : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab; // Prefab for the arrow UI
    [SerializeField] private Camera mainCamera; // The main camera
    [SerializeField] private List<GameObject> bosses; // List of all bosses
    [SerializeField] private Dictionary<GameObject, GameObject> bossToArrowMap = new Dictionary<GameObject, GameObject>();
    [SerializeField] private Dictionary<GameObject, RectTransform> bossToArrowRectMap = new Dictionary<GameObject, RectTransform>();
    private Transform _bossParent;
    [SerializeField] private float offset = 50f; // Offset from the edge of the screen
    private const float Tolerance = 0.01f; // Define your tolerance here
    
    


    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame(); // Ensures all bosses are spawned
        PopulateBossesList();
    }

    private void PopulateBossesList()
    {
        _bossParent = GameManager.GetBossParent();

        foreach (Transform bossChild in _bossParent)
        {
            bosses.Add(bossChild.gameObject);
            GameObject arrowInstance = Instantiate(arrowPrefab, transform);
            bossToArrowMap[bossChild.gameObject] = arrowInstance;

            bossToArrowRectMap[bossChild.gameObject] = arrowInstance.GetComponent<RectTransform>();
        }
    }

    private void FixedUpdate() // FixedUpdate to optimize performance
    {
        for (int i = bosses.Count - 1; i >= 0; i--) // I can't use foreach because we are removing elements
        {
            var boss = bosses[i];
            if (boss == null)
            {
                RemoveBoss(boss);
            }
            else
            {
                UpdateArrowForBoss(boss);
            }
        }
    }

    private void UpdateArrowForBoss(GameObject boss)
    {
        var arrow = bossToArrowMap[boss];
        var arrowRect = bossToArrowRectMap[boss];
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(boss.transform.position);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            arrow.SetActive(true);
            PositionArrowAtScreenEdge(arrowRect, viewportPosition, Tolerance);
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    private void PositionArrowAtScreenEdge(RectTransform arrowRect, Vector3 viewportPosition, double tolerance)
    {
        Vector3 screenPosition = new Vector3(viewportPosition.x * Screen.width, viewportPosition.y * Screen.height, 0);

        screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

        if (screenPosition.x == 0) screenPosition.x += offset;
        else if (Math.Abs(screenPosition.x - Screen.width) < tolerance) screenPosition.x -= offset;
        if (screenPosition.y == 0) screenPosition.y += offset;
        else if (Math.Abs(screenPosition.y - Screen.height) < tolerance) screenPosition.y -= offset;

        arrowRect.anchoredPosition = screenPosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
    }
    
    private void RemoveBoss(GameObject boss)
    {
        bosses.Remove(boss);
        SoundManager.PlaySound("BossDeath");
        var arrow = bossToArrowMap[boss];
        bossToArrowMap.Remove(boss);
        bossToArrowRectMap.Remove(boss);
        Destroy(arrow);
    }
}
