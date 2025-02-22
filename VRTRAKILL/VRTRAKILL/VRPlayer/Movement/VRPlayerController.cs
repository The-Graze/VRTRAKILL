﻿using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Movement
{
    internal class VRPlayerController : MonoBehaviour
    {
        CapsuleCollider CC;
        private void Start()
        {
            CC = GetComponent<CapsuleCollider>();
            UpdateCenter();
        }
        private IEnumerator UpdateCenter()
        {
            // Updates ingame player center to match irl player position
            float DistanceFromFloor = Vector3.Dot(Vars.VRCameraContainer.transform.localPosition, Vector3.up);
            CC.center = Vars.VRCameraContainer.transform.localPosition - 0.5f * DistanceFromFloor * Vector3.up;

            yield return new WaitForSeconds(0.5f);

            UpdateCenter();
        }
    }
}