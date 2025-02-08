using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code;
using Code.Wave;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Zenject;

public class ARPlayerPlacement : MonoBehaviour
{
    [SerializeField] private GameObject arObjectToSpawn;
    [SerializeField] private GameObject placementIndicator;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    [SerializeField] private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid;

    private WaveSpawner _waveSpawner;
    private GameState _gameState;

    [Inject]
    private void Construct(WaveSpawner waveSpawner, GameState gameState)
    {
        _waveSpawner = waveSpawner;
        _gameState = gameState;
    }

    private void Update()
    {
        if (spawnedObject == null && placementPoseIsValid)
            ARPlaceObject();

        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }


    private void UpdatePlacementIndicator()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
            placementIndicator.SetActive(false);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;

        if (placementPoseIsValid)
            PlacementPose = hits[0].pose;
    }

    private async void ARPlaceObject()
    {
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
        _gameState.ChangeState(GameStates.Game);
        await _waveSpawner.StartNextWave();
    }
}
