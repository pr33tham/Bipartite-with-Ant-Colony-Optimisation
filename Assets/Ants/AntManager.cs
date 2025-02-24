using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
public class AntManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
=======
=======
>>>>>>> Stashed changes
public class AntManager : MonoBehaviour {

    [SerializeField] GameObject antPrefab;
    [SerializeField] GameObject Parent;
    [SerializeField] GameObject cityGenerator;
    private int noOfAnts;
    private void Start() {
        noOfAnts = cityGenerator.GetComponent<CityGenerator>().noOfCities;
    }

    public void GenerateAntsOnClick() {
        for (int i = 0; i < noOfAnts; i++) {
            GameObject ant = Instantiate(antPrefab, cityGenerator.GetComponent<CityGenerator>().cityPoistions[i], Quaternion.identity);
            ant.transform.SetParent(Parent.transform);
        }
    }

    private void Update() {
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        
    }
}
