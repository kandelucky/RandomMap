using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour
{
    public LevelGeneration levelGeneration;
    public int rows;
    public int cols;
    public Text days;

    public Transform content;

    public GameObject[] mapPathImages;
    public GameObject tile;
    bool startMap;
    private void Awake()
    {
        startMap = true;
    }

    private void OnEnable()
    {
        StartGenerateBigMap();
    }
    public void StartGenerateBigMap()
    {
        levelGeneration.buttonIsOn = false;
        rows = levelGeneration.miniMapArray.GetLength(0);
        cols = levelGeneration.miniMapArray.GetLength(1);
        content.GetComponent<GridLayoutGroup>().constraintCount = cols;
        days.text = "DAYS - " + levelGeneration.days.ToString();
        if (levelGeneration.newMiniMap) StartCoroutine(GenerateMiniMap());
        else EditMiniMap();
    }
    IEnumerator GenerateMiniMap()
    {
        if (!startMap)
        {
            yield return StartCoroutine(DeleteAll());
        }
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(10, 0, 0), Quaternion.identity);
                newTile.transform.SetParent(content.transform);
                newTile.transform.localScale = Vector3.one;
                if (levelGeneration.miniMapArray[row, col] == 0)
                {
                    newTile.GetComponent<Image>().sprite = mapPathImages[7].GetComponent<Image>().sprite;
                    newTile.GetComponent<Image>().color = mapPathImages[7].GetComponent<Image>().color;
                }
                else
                {
                    newTile.GetComponent<Image>().sprite = mapPathImages[levelGeneration.miniMapArray[row, col] - 1].GetComponent<Image>().sprite;
                    newTile.GetComponent<Image>().color = mapPathImages[levelGeneration.miniMapArray[row, col] - 1].GetComponent<Image>().color;
                }
            }
        }
        levelGeneration.newMiniMap = startMap = false;
    }
    
    private void EditMiniMap()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject newTile = content.GetChild(col+row*cols).gameObject;
                if (levelGeneration.miniMapArray[row, col] == 0)
                {
                    newTile.GetComponent<Image>().sprite = mapPathImages[7].GetComponent<Image>().sprite;
                    newTile.GetComponent<Image>().color = mapPathImages[7].GetComponent<Image>().color;
                }
                else
                {
                    newTile.GetComponent<Image>().sprite = mapPathImages[levelGeneration.miniMapArray[row, col] - 1].GetComponent<Image>().sprite;
                    newTile.GetComponent<Image>().color = mapPathImages[levelGeneration.miniMapArray[row, col] - 1].GetComponent<Image>().color;
                }
            }
        }
    }
    private IEnumerator DeleteAll()
    {
        for (int i = 0; i < content.childCount; i++)
        {
           Destroy(content.GetChild(i).gameObject);

        }
        yield return null;
    }
}
