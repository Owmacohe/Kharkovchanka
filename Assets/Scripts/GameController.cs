using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float energy = 50;
    private float lastEnergy;

    private GameObject vehicle, telescopeObject;
    bool isTrackingStars;

    private TMP_Text energyText;

    enum POITypes { StarTracking, MeteoriteDiscovery, IceCoreDrilling }
    class POI
    {
        public POI(Vector3 pos, bool isActive, POITypes POIType)
        {
            this.pos = pos;
            this.isActive = isActive;
            this.POIType = POIType;
        }

        public Vector3 pos;
        public bool isActive;
        public POITypes POIType;
    }
    private List<POI> pointsOfInterest;

    private void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        telescopeObject = Resources.Load<GameObject>("Telescope");

        energyText = GetComponentInChildren<TMP_Text>();

        Invoke("startTrackingStars", 3);
    }

    private void FixedUpdate()
    {
        if (lastEnergy != energy)
        {
            energyText.text = "Energy: " + energy;
            lastEnergy = energy;
        }
    }

    public void addNewPOIs(List<Vector3> POIList)
    {
        if (pointsOfInterest == null)
        {
            pointsOfInterest = new List<POI>();
        }

        for (int i = 0; i < POIList.Count; i++)
        {
            POI temp = new POI(POIList[i], false, (POITypes)Random.Range(0, 3));
            pointsOfInterest.Add(temp);
        }
    }

    private void startTrackingStars()
    {
        GameObject temp = Instantiate(telescopeObject, vehicle.transform);
        temp.transform.parent = null;

        isTrackingStars = true;

        energy -= 20;
    }

    public void stopTrackingStars()
    {
        isTrackingStars = false;
    }
}
