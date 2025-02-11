using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.SideChannels;
using UnityEngine.SceneManagement;

public class RobotManager : MonoBehaviour
{
    private static RobotManager instance;
    public static RobotManager Instance { get { return instance; } }
    public GameObject modularRobotPrefab;

    public GameObject climbingRackPrefab;

    [Tooltip("Makes sure the physics start after n fixed updates")]
    [SerializeField] int warmupFixedUpdates = 20;

    RobotConfigChannel rc;

    public void Awake() {
        if (instance != null && instance != this) {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            return;
        }
        else if (instance == null) {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        if (rc == null) {
            rc = new RobotConfigChannel();
            SideChannelManager.RegisterSideChannel(rc);
        }
        Unity.MLAgents.Academy.Instance.OnEnvironmentReset += ResetScene;
    }

    public void ResetScene() {
        SceneManager.LoadScene("SampleScene");
        Application.targetFrameRate = 30;
        StartCoroutine(SpawnRobot());
    }

    IEnumerator SpawnRobot() {
        // Wait for scene to load properly
        for (int i = 0; i < warmupFixedUpdates; i++)
            yield return new WaitForFixedUpdate();

        // Set seed
        Random.InitState(rc.seed);

        // Setup for climb mode
        if(rc.climb_mode) {
            Instantiate(climbingRackPrefab);
            GameObject.Find("Main Camera").transform.position = new Vector3(-20f, 30f, 0f);
            GameObject.Find("Main Camera").transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        // Create the robot
        GameObject modularRobotObject = Instantiate(modularRobotPrefab);
        ModularRobot modularRobot = modularRobotObject.GetComponent<ModularRobot>();
        modularRobot.CreateRobot(rc);
        rc.SendMessage();
    }

    public void OnDestroy(){
        if (Academy.IsInitialized && rc != null)
            SideChannelManager.UnregisterSideChannel(rc);
    }
}
