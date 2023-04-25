using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerLocation : MonoBehaviour
{
    public GameObject playerT;
    public ARSessionOrigin ar;
    public int height;

    public CompassBehaviour cb;
    public float northRotation;
    public float compassOffset;

    // bool synced;
    // float syncDelay;

    Gyroscope gyro;
    Quaternion gyroCorrection;

    Quaternion gyroTrans;

    // Start is called before the first frame update
    void Start()
    {
        gyroCorrection = Quaternion.Euler(90f,0f,0f);
        height = 6;
        compassOffset = 0;//-4.17f;
        ar = GetComponent<ARSessionOrigin>();
        // synced = false;
        // syncDelay = 2;

        gyro = Input.gyro;
        gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(cb.startTracking)    
            northRotation = cb.northRotation;
        

        // Vector3 cameraLocation = playerT.transform.position + new Vector3(0, -height, 0);
        GyroModifyCamera();
        float gyroX = gyroTrans.eulerAngles.x;
        float gyroZ = gyroTrans.eulerAngles.z;
        //ar.MakeContentAppearAt(playerT.transform,  transform.position, Quaternion.Euler(gyroX, -northRotation + compassOffset,0));
        transform.position = playerT.transform.position;
        transform.rotation = Quaternion.Euler(gyroX, northRotation + compassOffset, gyroZ);
        //StartCoroutine(SyncCompass());
        // transform.position = playerT.transform.position + new Vector3(0,height,0);
        // transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, northRotation, 0), Time.deltaTime * .2f);
    }

    // IEnumerator SyncCompass()
    // {
    //     ar.MakeContentAppearAt(playerT.transform,  transform.position, Quaternion.Euler(0, -northRotation + compassOffset,0));
    //     synced = true;
    //     yield return new WaitForSeconds(syncDelay);
    //     synced = false;
    // }

    void GyroModifyCamera()
    {   
        Quaternion gyroQuaternion = GyroToUnity(Input.gyro.attitude);
        // rotate coordinate system 90 degrees. Correction Quaternion has to come first
        Quaternion calculatedRotation = gyroCorrection * gyroQuaternion;
        gyroTrans = calculatedRotation;
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}