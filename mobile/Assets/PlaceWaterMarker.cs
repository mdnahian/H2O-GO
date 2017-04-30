using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kudan.AR
{

    public class PlaceWaterMarker : MonoBehaviour
    {

        public KudanTracker _kudanTracker;

        public void placeMarker()
        {
            Vector3 position;
            Quaternion orientation;

            _kudanTracker.FloorPlaceGetPose(out position, out orientation);
            _kudanTracker.ArbiTrackStart(position, orientation);
        }
    }

}
