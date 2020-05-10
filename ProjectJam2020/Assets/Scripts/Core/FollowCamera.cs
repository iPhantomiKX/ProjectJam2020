using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        Vector3 OriginalRotation = new Vector3(0, 0, 0);

        void Awake()
        {
            target = FindObjectOfType<PlayerController>().transform;
        }

        private void Update() 
        {
            target = FindObjectOfType<PlayerController>().transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position;

            if(Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.up * 90 * Time.deltaTime);
            if(Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.up * -90 * Time.deltaTime);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }
}

