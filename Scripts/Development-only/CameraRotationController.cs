using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VROrdzhonikidzevskii.DevelopmentOnly
{
    public sealed class CameraRotationController : MonoBehaviour
    {
        private const string Input_Horizontal = "Horizontal";
        private const string Input_Vertical = "Vertical";
        [SerializeField]
        private float Sensitive = 1;
        private void Update()
        {
            float horizontal = Input.GetAxisRaw(Input_Horizontal)* Sensitive;
            float vertical=-Input.GetAxisRaw(Input_Vertical)* Sensitive;
            if (horizontal != 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+horizontal, 0);
            }
            if (vertical != 0)
            {
                if (vertical > 0)
                {
                    if (transform.eulerAngles.x + vertical >= 90&&
                        transform.eulerAngles.x+vertical<-90)
                        transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, 0);
                    else
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x+vertical, transform.eulerAngles.y, 0);
                }
                else
                {
                    if (transform.eulerAngles.x + vertical <= -90&&
                        transform.eulerAngles.x+vertical>90)
                        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 0);
                    else
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x+vertical, transform.eulerAngles.y, 0);
                }
            }
        }
    }
}
