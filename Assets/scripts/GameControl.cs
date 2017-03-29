using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameControl : MonoBehaviour 
{
    public GameObject tilePrefab, borderPrefab;
	public Transform tilesArea;
    public Text equation, counter;
    public Image timeBar;
    public float time;
    Color[] colors;
    List<GameObject> selectedTiles, allTiles;
    GameSet gameSet;
    int[] posX, posY;
    int wantedValue;
    float _time;

	void Start() 
    {
		gameSet = GameObject.Find ("gameSet").GetComponent<GameSet> ();
        selectedTiles = new List<GameObject>();
        allTiles = new List<GameObject>();
        _time = time;

        SetColorsTable(out colors);
		SetValuesInArraysContainingCords();
		SpawnTiles();
        DrawnANumber();
	}

    void FixedUpdate()
    {
        timeBar.transform.localScale = new Vector3(_time / time, 1, 1);
        _time -= Time.deltaTime;
        counter.text = _time.ToString();
        counter.text = counter.text.Substring(0, counter.text.IndexOf(".") + 2);

        if(_time <= 0)
        {
            _time = time;
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
                int r = Random.Range(1, gameSet.maxValueOnTiles);
				Vector2 pos = new Vector2(posX[x], posY[y]);
                GameObject tile = Instantiate(tilePrefab, Vector2.zero, new Quaternion(0, 0, 0, 0), tilesArea);
                tile.GetComponent<RectTransform>().localPosition = pos;
				tile.transform.GetChild(0).GetComponent<Text>().text = r.ToString();
				tile.GetComponent<Tile>().value = r;
                tile.transform.GetChild(0).GetComponent<Text>().color = colors[r];
                allTiles.Add(tile);
			}
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

        CheckIfTilesGiveWantedValue();
    }

    void DrawnANumber()
    {
        wantedValue = 0;

        int howManyTiles = 0;

        switch (allTiles.Count)
        {
            case 2:
                howManyTiles = 2;
                break;
            case 3:
                howManyTiles = 3;
                break;
            default:
                howManyTiles = Random.Range(2, 4);
                break;
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

        equation.text = wantedValue + " = ";
    }

    void CheckIfTilesGiveWantedValue()
    {
        int summary = 0;

        equation.text = wantedValue + " = ";

        foreach(GameObject tile in selectedTiles)
        {
            summary += tile.GetComponent<Tile>().value;
            equation.text += tile.GetComponent<Tile>().value + " + ";
        }

        if(summary == wantedValue)
        {
            ResetSelectedTiles();
            DrawnANumber();
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

        for (int i = 0; i < colors.Length; i++)
        {
            float r = Random.Range(0.4f, 1f);
            float g = Random.Range(0.4f, 1f);
            float b = Random.Range(0.4f, 1f);
            colors[i] = new Color(r, g, b);
        }
    }
}