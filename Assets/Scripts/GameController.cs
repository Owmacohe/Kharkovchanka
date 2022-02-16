using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float energy = 50;
    private float lastEnergy;

    public GameObject popup;
    private TMP_Text popupText;
    public float POIDistanceTrigger = 10;

    public Transform minimapCanvas;
    private Transform vehicleIcon;

    private GameObject vehicle, telescopeObject;
    private PlayerController playerController;
    [HideInInspector]
    public bool isTrackingStars;

    private TMP_Text energyText;

    private POI currentPOI;

    enum POITypes { StarTracking, MeteoriteDiscovery, IceCoreDrilling }
    class POI
    {
        public POI(Vector3 pos, bool isActive, POITypes POIType, GameObject icon)
        {
            this.pos = pos;
            this.isActive = isActive;
            this.POIType = POIType;
            this.icon = icon;
        }

        public Vector3 pos;
        public bool isActive;
        public POITypes POIType;
        public GameObject icon;
    }
    private List<POI> pointsOfInterest;

    private void Start()
    {
        popup.SetActive(false);
        popupText = popup.GetComponentsInChildren<TMP_Text>()[2];

        vehicleIcon = minimapCanvas.GetChild(0);

        vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        playerController = vehicle.GetComponentInParent<PlayerController>();
        telescopeObject = Resources.Load<GameObject>("Telescope");

        energyText = GetComponentInChildren<TMP_Text>();
    }

    private void FixedUpdate()
    {
        if (lastEnergy != energy)
        {
            energyText.text = "Energy: " + energy;
            lastEnergy = energy;
        }

        vehicleIcon.localPosition = new Vector3(vehicle.transform.position.x, vehicle.transform.position.z, 0) * 13.35f;

        if (!isTrackingStars)
        {
            bool foundPOI = false;

            foreach (POI i in pointsOfInterest)
            {
                if (Vector3.Distance(vehicle.transform.position, i.pos) <= POIDistanceTrigger)
                {
                    popup.SetActive(true);
                    foundPOI = true;
                    currentPOI = i;

                    switch (currentPOI.POIType)
                    {
                        case POITypes.StarTracking:
                            popupText.text = "to track star movements";
                            break;
                        case POITypes.MeteoriteDiscovery:
                            popupText.text = "to search for meteorites";
                            break;
                        case POITypes.IceCoreDrilling:
                            popupText.text = "to drill a deep ice core";
                            break;
                    }

                    break;
                }
            }

            if (!foundPOI && currentPOI != null)
            {
                popup.SetActive(false);
                currentPOI = null;
            }
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
            GameObject tempIcon = Instantiate(Resources.Load<GameObject>("Icon"), minimapCanvas);
            POI temp = new POI(POIList[i], false, (POITypes)Random.Range(0, 3), tempIcon);
            pointsOfInterest.Add(temp);

            tempIcon.transform.localPosition = new Vector3(temp.pos.x, temp.pos.z, 0) * 13.35f;
        }
    }

    public void triggerPOI()
    {
        if (currentPOI.POIType.Equals(POITypes.StarTracking) && !isTrackingStars)
        {
            startTrackingStars();
        }
    }

    private void startTrackingStars()
    {
        GameObject temp = Instantiate(telescopeObject, vehicle.transform);
        temp.transform.parent = null;

        isTrackingStars = true;

        energy -= 20;

        popup.SetActive(false);
        pointsOfInterest.Remove(currentPOI);
        Destroy(currentPOI.icon);
        currentPOI = null;
    }
}
