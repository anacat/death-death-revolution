using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using UnityEngine.UI;
using Kinect = Windows.Kinect;

public class CoordinateMapperView : MonoBehaviour
{
    public GameObject handPrefab;
    public GameObject danceColliders;

    public CoordinateMapperManager coordinateMapperManager;

    private ComputeBuffer depthBuffer;
    private ComputeBuffer bodyIndexBuffer;

    DepthSpacePoint[] depthPoints;
    byte[] bodyIndexPoints;
    private RawImage _rawImage;

    public bool bodyFound;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };


    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();

        coordinateMapperManager = FindObjectOfType<CoordinateMapperManager>();
    }

    void Start()
    {
        ReleaseBuffers();

        if (coordinateMapperManager == null)
        {
            return;
        }

        Texture2D renderTexture = coordinateMapperManager.GetColorTexture();
        if (renderTexture != null)
        {
            _rawImage.material.SetTexture("_MainTex", renderTexture);
        }

        depthPoints = coordinateMapperManager.GetDepthCoordinates();
        if (depthPoints != null)
        {
            depthBuffer = new ComputeBuffer(depthPoints.Length, sizeof(float) * 2);
            _rawImage.material.SetBuffer("depthCoordinates", depthBuffer);
        }

        bodyIndexPoints = coordinateMapperManager.GetBodyIndexBuffer();
        if (bodyIndexPoints != null)
        {
            bodyIndexBuffer = new ComputeBuffer(bodyIndexPoints.Length, sizeof(float));
            _rawImage.material.SetBuffer("bodyIndexBuffer", bodyIndexBuffer);
        }
    }

    void Update()
    {
        //TODO: fix perf on this call.
        depthBuffer.SetData(depthPoints);

        // ComputeBuffers do not accept bytes, so we need to convert to float.
        float[] buffer = new float[512 * 424];
        for (int i = 0; i < bodyIndexPoints.Length; i++)
        {
            buffer[i] = (float)bodyIndexPoints[i];
        }
        bodyIndexBuffer.SetData(buffer);

        Kinect.Body[] data = coordinateMapperManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId], body.TrackingId);
            }
        }

        buffer = null;
    }

    private void ReleaseBuffers()
    {
        if (depthBuffer != null) depthBuffer.Release();
        depthBuffer = null;

        if (bodyIndexBuffer != null) bodyIndexBuffer.Release();
        bodyIndexBuffer = null;

        depthPoints = null;
        bodyIndexPoints = null;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject, ulong id)
    {
        Transform leftHand = bodyObject.transform.Find(JointType.HandLeft.ToString() + id);
        leftHand.localPosition = GetVector3FromJoint(body.Joints[JointType.HandLeft]);

        Transform rightHand = bodyObject.transform.Find(JointType.HandRight.ToString() + id);
        rightHand.localPosition = GetVector3FromJoint(body.Joints[JointType.HandRight]);

        if (danceColliders != null)
        {
            Transform dc = bodyObject.transform.Find("DanceColliders" + id);
            dc.localPosition = GetVector3FromJoint(body.Joints[JointType.SpineBase]);
        }
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        GameObject leftHand = Instantiate(handPrefab, body.transform);
        leftHand.name = JointType.HandLeft.ToString() + id;

        GameObject rightHand = Instantiate(handPrefab, body.transform);
        rightHand.name = JointType.HandRight.ToString() + id;

        if (danceColliders != null)
        {
            GameObject dc = Instantiate(danceColliders, body.transform);
            dc.name = "DanceColliders" + id;
        }

        bodyFound = true;

        return body;
    }

    void OnDisable()
    {
        ReleaseBuffers();
    }
}
