using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTargetRotator : MonoBehaviour
{
    //////////////////////////////Config
    public Vector3 rot;

    //////////////////////////////State
    //////////////////////////////Cached Component References
    public Transform rotationTesting;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this.transform.LookAt(rotationTesting.position, Vector3.forward);
        // this.transform.LookAt(rotationTesting.position, Vector3.right);
        // this.transform.right = rotationTesting.position;

        // this.transform.Rotate(rot *Time.deltaTime);
    }



    public IEnumerator RotateChunk()
    {   
        float rotationAmount = 45f * (Random.Range(1,5));
        float targetRotation = this.transform.rotation.eulerAngles.z + rotationAmount > 360 ? this.transform.rotation.eulerAngles.z + rotationAmount -360 : this.transform.rotation.eulerAngles.z + rotationAmount;
        print("target rotation: " + targetRotation);
        while (Mathf.Abs(this.transform.rotation.eulerAngles.z - targetRotation) > 2)
        {
            this.transform.Rotate(rot *Time.deltaTime);
            print(this.transform.rotation.eulerAngles.z);
            yield return null;
        }
        // this.transform.rotation.eulerAngles = new Vector3(0,0,targetRotation);
    }


    [ContextMenu("RotateChunk()")]
    private void RotateChunkFromEditor()
    {
        StartCoroutine(RotateChunk());
    }

}
