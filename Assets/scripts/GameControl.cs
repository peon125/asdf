using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameControl : MonoBehaviour 
{
    public GameObject tilePrefab, borderPrefab, endingBoard;
	public Transform tilesArea;
    public Text equationText, counterText, findText;
    public Image timeBar;
    public float time;
    Color[] colors;
    List<GameObject> selectedTiles, allTiles;
    GameSet gameSet;
    string sign, equation;
    int[] posX, posY;
    int wantedValue, points;
    float _time, timeSinceSceneStarted;

	void Start() 
    {
		gameSet = GameObject.Find ("gameSet").GetComponent<GameSet> ();
        selectedTiles = new List<GameObject>();
        allTiles = new List<GameObject>();
        _time = time;
        points = 0;
        timeSinceSceneStarted = 0;
        sign = "";
        equation = "";

        SetColorsTable(out colors);
		SetValuesInArraysContainingCords();
		SpawnTiles();
        DrawnANumber();

        if (gameSet.operations == "add")
        {
            findText.text = "Find sum:";
            sign = " + ";
        }
        else if (gameSet.operations == "sub")
        {
            findText.text = "Find product:";
            sign = " - ";
        }

        if (!gameSet.timeIsRunningOut)
        {
            timeBar.gameObject.SetActive(false);
        }
	}

    void Update()
    {
        timeSinceSceneStarted += Time.deltaTime;

        if (gameSet.timeIsRunningOut)
        {
            timeBar.transform.localScale = new Vector3(_time / time, 1, 1);
            _time -= Time.deltaTime;
        }

        if ((_time <= 0 && gameSet.timeIsRunningOut) || allTiles.Count == 0)
        {
            EndGame();
        }
    }

	void SetValuesInArraysContainingCords()
	{
		posX = new int[6];
		posX[0] = -150;
		for (int i = 1; i < posX.Length; i++) 
		{
			posX[i] = posX[i - 1] + 60;
		}

		posY = new int[7];
		posY[0] = -180;
		for (int i = 1; i < posY.Length; i++) 
		{
			posY[i] = posY[i - 1] + 60;
		}
	}

	void SpawnTiles()
	{
		for (int x = 0; x < posX.Length; x++)
		{
			for (int y = 0; y < posY.Length; y++)
			{
                Vector2 pos = new Vector2(posX[x], posY[y]);
                int r = Random.Range(1, gameSet.maxValueOnTiles);
                SpawnASingleTile(pos, r, true);
			}
		}
	}

    void SpawnASingleTile(Vector2 pos, int value, bool addToTheList)
    {
        GameObject tile = Instantiate(tilePrefab, Vector2.zero, new Quaternion(0, 0, 0, 0), tilesArea) as GameObject;
        tile.GetComponent<RectTransform>().localPosition = pos;
        tile.transform.GetChild(0).GetComponent<Text>().text = value.ToString();
        tile.GetComponent<Tile>().value = value;
        tile.transform.GetChild(0).GetComponent<Text>().color = colors[value];

        if(addToTheList)
        {
            allTiles.Add(tile);
        }
    }

    public void TileTouched(GameObject tile)
    {
        if (!selectedTiles.Contains(tile))
        {
            selectedTiles.Add(tile);
            GameObject border = Instantiate(borderPrefab, tile.transform.position, new Quaternion(0,0,0,0), tile.transform);
            border.transform.localScale *= 0.5f;
        }
        else if (selectedTiles.Contains(tile))
        {
            selectedTiles.Remove(tile);
            Destroy(tile.transform.GetChild(1).gameObject);
        }

        if (allTiles.Count > 0)
        {
            CheckIfTilesGiveWantedValue();
        }
    }

    void DrawnANumber()
    {
        int minuend = 0;
        wantedValue = 0;

        int howManyTiles = 2;

        if(allTiles.Count == 3)
        {
            howManyTiles = 3;
        }

        Tile[] drawnTiles = new Tile[howManyTiles];
        Tile drawnTile = null;

        for (int i = 0; i < drawnTiles.Length; i++)
        {
            do
            {
                drawnTile = allTiles[Random.Range(0, allTiles.Count)].GetComponent<Tile>();
            } while(drawnTiles.Contains(drawnTile));

            drawnTiles[i] = drawnTile;
        }

        foreach (Tile tile in drawnTiles)
        {
            wantedValue += tile.value;
        }

        if (gameSet.operations == "sub")
        {
            minuend = Random.Range(gameSet.maxValueOnTiles, gameSet.maxValueOnTiles * 2 - 1);
            equation = (minuend - wantedValue) + " = " + minuend + " - ";
        }

        if (gameSet.operations == "add")
        {
            equation = wantedValue + " = ";
        }

        equationText.text = equation;
    }

    void CheckIfTilesGiveWantedValue()
    {
        int summary = 0;

        string text = "";

        foreach(GameObject tile in selectedTiles)
        {
            summary += tile.GetComponent<Tile>().value;
            text += tile.GetComponent<Tile>().value + sign;
        }

        equationText.text = equation + text;

        if(summary == wantedValue && selectedTiles.Count >= 2)
        {
            switch (gameSet.gameMode)
            {
                case 1:
                    SpawnAZeroTile();
                    break;
                case 2:
                    ChangeATile();
                    break;
            }

            GivePoints();
            ResetSelectedTiles();

            if (allTiles.Count > 0)
            {
                DrawnANumber();
            }
        }
    }

    void ResetSelectedTiles()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("border");
        foreach(GameObject go in gos)
        {
            allTiles.Remove(go.transform.parent.gameObject);
            Destroy(go.transform.parent.gameObject);
            Destroy(go);
        }

        selectedTiles.Clear();

        _time = time;
    }

    void SetColorsTable(out Color[] colors)
    {
        colors = new Color[gameSet.maxValueOnTiles];
        colors[0] = Color.white;

        float f = 360 / gameSet.maxValueOnTiles;
        f /= 60;
        float limit = 1f;
        int i = 1;

        for (int o = 0; i < colors.Length; o++)
        {
            if (f * o > limit)
            {
                break;
            }
            colors[i] = new Color(limit, f * o, 0f);
            i++;
        }

        for (int o = 0; i < colors.Length; o++)
        {
            if (f * o > limit)
            {
                break;
            }
            colors[i] = new Color(limit - f * o, limit, 0f);
            i++;
        }

        for (int o = 0; i < colors.Length; o++)
        {
            if (f * o > limit)
            {
                break;
            }
            colors[i] = new Color(0f, limit, f * o);
            i++;
        }

        for (int o = 0; i < colors.Length; o++)
        {
            if (f * o > limit)
            {
                break;
            }
            colors[i] = new Color(0f, limit - f * o, limit);
            i++;
        }

        for (int o = 0; i < colors.Length; o++)
        {
            if (f * o > limit)
            {
                break;
            }
            colors[i] = new Color(f * o, 0f, limit);
            i++;
        }

        for (int o = 0; i < colors.Length; o++)
        {
            if (f * o > limit)
            {
                break;
            }
            colors[i] = new Color(limit, 0f, limit - f * o);
            i++;
        }
    }

    void SpawnAZeroTile()
    {
        foreach (GameObject tile in selectedTiles)
        {
            Vector2 pos = tile.GetComponent<RectTransform>().localPosition;
            SpawnASingleTile(pos, 0, false);
        }
    }

    void ChangeATile()
    {
        foreach (GameObject tile in selectedTiles)
        {
            Vector2 pos = tile.GetComponent<RectTransform>().localPosition;
            int r = Random.Range(1, gameSet.maxValueOnTiles);
            SpawnASingleTile(pos, r, true);
        }
    }

    void GivePoints()
    {
        int howManyTilesWorthPoints = 0;

        foreach(GameObject tile in selectedTiles)
        {
            if (tile.GetComponent<Tile>().value != 0)
            {
                howManyTilesWorthPoints++;
            }
        }

        points += (int)Mathf.Pow(howManyTilesWorthPoints, 2);

        counterText.text = points.ToString();
    }

    void EndGame()
    {
        Time.timeScale = 0;
        endingBoard.SetActive(true);
    }

    public float GetTime()
    {
        return timeSinceSceneStarted;
    }
}