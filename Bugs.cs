using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bugs : MonoBehaviour {
    public GameObject[,] BugsArray;
    public GameObject Bug;
    public int[,] CreateGeneration;
    int columns, rows;
    float timer1 = 0;

	// Use this for initialization
	void Start () {
        columns = 56;
        rows = 42;
        BugsArray = new GameObject[rows, columns];
        CreateGeneration = new int[rows, columns];
        BugsArrayFiller(rows, columns);
        MoveLayerBugs(rows, columns);
        //ThreeBugs(42, 54);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        timer1 += Time.deltaTime;
        if (timer1 >= 0.1f) {
            CheckDeathOrAlive(rows, columns);
            GenerationToAppear(rows, columns);
            timer1 = 0;
        }
    }


    void BugsArrayFiller(int m, int n) {
        int numbername = 0;
        float newpositionx = 0, newpositiony = 0;
        for (int i = 1; i < m - 1; ++i)
        {
            for (int j = 1; j < n - 1; ++j)
            {
                BugsArray[i, j] = Instantiate(Bug, new Vector3(-26.5f + newpositionx, 19.5f - newpositiony, 0), transform.rotation);
                BugsArray[i, j].name = BugsArray[i, j].name.Replace("(Clone)", "") + (numbername++);
                newpositionx += 1f;
            }
            newpositionx = 0f;
            newpositiony += 1f;
        }
    }

    void MoveLayerBugs(int m, int n) {
        float randomvalue = 0;
        for (int i = 1; i < m - 1; ++i)
        {
            for (int j = 1; j < n - 1; ++j)
            {
                randomvalue = Random.Range(0f, 5f);
                if (randomvalue < 1) {
                    BugsArray[i, j].GetComponent<SpriteRenderer>().sortingOrder = 2;
                }

            }
        }
    }

    void ThreeBugs(int m, int n) {
        for (int i = 1; i < m - 1; ++i)
        {
            for (int j = 1; j < n - 1; ++j)
            {
                BugsArray[i, j].GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
        BugsArray[5, 4].GetComponent<SpriteRenderer>().sortingOrder = 2;
        BugsArray[5, 5].GetComponent<SpriteRenderer>().sortingOrder = 2;
        BugsArray[5, 6].GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    void CheckDeathOrAlive(int m, int n)
    {
        int counterbugs = 0;
        for (int i = 1; i < m - 1; ++i)
        {
            for (int j = 1; j < n - 1; ++j)
            {
                if (BugsArray[i, j].GetComponent<SpriteRenderer>().sortingOrder == 0) //Detect if the there is a space
                {
                    CreateGeneration[i, j] = 0;
                    counterbugs = CheckIfThereIsABug(i, j, counterbugs);
                    if (counterbugs == 3) CreateGeneration[i,j] = 2; //nace un bicho
                    counterbugs = 0;
                    continue;
                }
                if (BugsArray[i, j].GetComponent<SpriteRenderer>().sortingOrder == 2) //Detect if there is an entity
                {
                    CreateGeneration[i, j] = 2;
                    counterbugs = CheckIfThereIsABug(i, j, counterbugs);
                    if (counterbugs < 2 || counterbugs > 3) CreateGeneration[i, j] = 0; //kill the entity
                    counterbugs = 0;
                    continue;
                }
            }
        }
        return;
    }

    void GenerationToAppear(int m, int n) {
        for (int i = 1; i < m - 1; ++i)
        {
            for (int j = 1; j < n - 1; ++j)
            {
                BugsArray[i, j].GetComponent<SpriteRenderer>().sortingOrder = CreateGeneration[i, j];
            }
        }
    }

    int CheckIfThereIsABug(int i, int j, int counterbugs) {
        //Esquina izquierda
        if (BugsArray[i - 1, j - 1] != null && BugsArray[i - 1, j - 1].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Arriba
        if (BugsArray[i - 1, j] != null && BugsArray[i - 1, j].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Esquina Derecha
        if (BugsArray[i - 1, j + 1] != null && BugsArray[i - 1, j + 1].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Izquierda
        if (BugsArray[i, j - 1] != null && BugsArray[i, j - 1].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Derecha
        if (BugsArray[i, j + 1] != null && BugsArray[i, j + 1].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Inferior Izquierda
        if (BugsArray[i + 1, j - 1] != null && BugsArray[i + 1, j - 1].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Abajo
        if (BugsArray[i + 1, j] != null && BugsArray[i + 1, j].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;
        //Inferior Derecha
        if (BugsArray[i + 1, j + 1] != null && BugsArray[i + 1, j + 1].GetComponent<SpriteRenderer>().sortingOrder == 2) ++counterbugs;

        return counterbugs;
    }
}
