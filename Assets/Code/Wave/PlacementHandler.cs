using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Code.Wave
{
    public class PlacementHandler : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager _arRaycastManager;
        [SerializeField] private ARPlaneManager _arPlaneManager;
        
        private List<ARRaycastHit> _hits = new();
        private Pose _placementPose;
        private bool _placementPoseIsValid;

        public Vector3 GetSpawnPosition()
        {
            UpdatePlacementPose();
            return _placementPoseIsValid ? _placementPose.position : Vector3.zero;
        }

        private void UpdatePlacementPose()
        {
            if (_arPlaneManager.trackables.count == 0)
            {
                _placementPoseIsValid = false;
                return;
            }

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            _arRaycastManager.Raycast(screenCenter, _hits, TrackableType.PlaneEstimated);

            _placementPoseIsValid = _hits.Count > 0;
            if (_placementPoseIsValid)
                _placementPose = _hits[0].pose;
        }
    }   
}