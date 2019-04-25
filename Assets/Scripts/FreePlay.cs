using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using UnityEngine.SceneManagement;
public class FreePlay : MonoBehaviour 
{
    
    public BodySourceManager mBodySourceManager;
    public GameObject spaceship;
    
    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<Kinect.JointType> _joints = new List<Kinect.JointType>
    {
        //Kinect.JointType.HandLeft,
        Kinect.JointType.HandRight,
    };

    
    void Update () 
    {
        #region Get Kinect data
        Kinect.Body[] data = mBodySourceManager.GetData();

        if (data == null)
        {
            return;
        }
        
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
                continue;

            if (body.IsTracked)
                trackedIds.Add(body.TrackingId);
        }

        #endregion

        #region Delete Kinect bodies
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach (ulong trackingID in knownIds)
        {
            if (!trackedIds.Contains(trackingID))
            {
                //set spaceship parnet to null so it doesn't get destroyed
                spaceship.transform.parent = null;
                //Destroy body object
                Destroy(mBodies[trackingID]);

                //Remove from list
                mBodies.Remove(trackingID);
            }
        }
        #endregion

        #region Create Kinect bodies
        foreach (var body in data)
        {
            //if no body, skip
            if(body == null)
                continue;

            if (body.IsTracked)
            {
                //IF body isn't tracked, create body
                if(!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);

                // Update positions
                UpdateBodyObject(body, mBodies[body.TrackingId]);

            }
        }
        #endregion
        //Test
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            spaceship.transform.position += move * (float)10.0 * Time.deltaTime;
        //endTest
         if (Input.GetMouseButtonDown(0))
        {   
            //currentSong.tempo = 
            Debug.Log("Pressed primary button.");
            Vector2 rayPos = new Vector2(spaceship.transform.position.x, spaceship.transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);
            if (hit)
            {
                Debug.Log(hit);
                if (hit.transform.gameObject.name.Contains("Home"))
                {
                    SceneManager.LoadScene("MainMenu");
                } else if (hit.transform.gameObject.name.Contains("Key"))
                {
                  hit.transform.gameObject.GetComponent<AudioSource>().Play();
                    
                } 
            }
        }
       
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        //Create body parent
        GameObject body = new GameObject("Body:" + id);

        //Create joints
        foreach (Kinect.JointType joint in _joints)
        {
            //Create object
            spaceship.name = joint.ToString();

            //Parent to body
            spaceship.transform.parent = body.transform;

        }
        return body;
    }

    private void UpdateBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        //Update joints
        foreach (Kinect.JointType _joint in _joints)
        {
            //Get new target position
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition.z=-2;

            //Get joint, set new position
            Transform jointObject = bodyObject.transform.Find(_joint.ToString());
            
            
            /* Debug.Log("new hand movement");
            Debug.Log(jointObject.transform.position);
            Debug.Log(Vector3.Scale(jointObject.transform.position, new Vector3(2,2,1)));
            Vector3 newp = Vector3.Scale(jointObject.transform.position, new Vector3(2,2,1));
            */
            jointObject.transform.position = Vector3.Lerp(jointObject.transform.position, targetPosition*2, 3*Time.deltaTime);
        }
    }
    
    private Vector3 GetVector3FromJoint (Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

}
