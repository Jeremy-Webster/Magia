using System.Collections;
using UnityEngine;

namespace MageiaGame
{
    public class CameraFollow : MonoBehaviour
    {
        public GameObject player;

        [Header("Movement")]
        public float offsetY = 3.355f;
        public float offsetZ = -4.16f;
        public float minZ = -21.64f;
        public float maxZ = 26.33f;

        [Space(2f)]
        public float smooth = 4f;


        //////////////////////////////////////////


        private Vector3 cameraPos;

        private float shakeEndTime = 0f;
        private float shakeIntensity = 0f;
        private bool shaking = false;

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                          Initalization                           //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        private void Start()
        {
            Camera cam = GetComponent<Camera>();

            if (cam != null)
            {
                cam.backgroundColor = GameplayController.COLOR_4;
                cam.clearFlags = CameraClearFlags.SolidColor;
            }
        }

        //////////////////////////////////////////////////////////////////////
        //                                                                  //
        //                              Core                                //
        //                                                                  //
        //////////////////////////////////////////////////////////////////////

        void LateUpdate()
        {
            cameraPos.x = player.transform.position.x;
            cameraPos.y = offsetY;
            cameraPos.z = player.transform.position.z + offsetZ;

            if (cameraPos.z < minZ)
                cameraPos.z = minZ;
            if (cameraPos.z > maxZ)
                cameraPos.z = maxZ;

            transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * smooth);
        }

        public void ForceMove()
        {
            cameraPos.x = player.transform.position.x;
            cameraPos.y = offsetY;
            cameraPos.z = player.transform.position.z + offsetZ;

            transform.position = cameraPos;
        }

        public IEnumerator IScreenShake(float duration, float intensity)
        {
            float start = Time.realtimeSinceStartup;

            shakeEndTime = start + duration;
            shakeIntensity = intensity;

            if (shaking) yield break;

            shaking = true;

            while (Time.realtimeSinceStartup < shakeEndTime)
            {
                transform.localPosition = new Vector3(
                    transform.localPosition.x + Random.Range(-1f, 1f) * shakeIntensity, 
                    transform.localPosition.y + Random.Range(-1f, 1f) * shakeIntensity, 
                    transform.localPosition.z);

                yield return new WaitForSecondsRealtime(0.01f);
            }

            shaking = false;
        }

        public void ScreenShake(float duration, float intensity)
        {
            StartCoroutine(IScreenShake(duration, intensity));
        }
    }

}