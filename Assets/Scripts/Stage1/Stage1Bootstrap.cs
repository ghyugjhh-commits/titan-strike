using UnityEngine;

public class Stage1Bootstrap : MonoBehaviour
{
    [SerializeField] private Material groundMaterial;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool autoInitialize = true;

    private void Start()
    {
        if (autoInitialize)
        {
            BuildScene();
        }
    }

    public void BuildScene()
    {
        CreateEnvironment();
        CreatePlayer();
        CreateCameraRig();
        CreateMobileControls();
    }

    private void CreateEnvironment()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10f, 1f, 10f);

        if (groundMaterial != null)
        {
            ground.GetComponent<MeshRenderer>().material = groundMaterial;
        }

        GameObject lightGO = new GameObject("Directional Light");
        Light light = lightGO.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        for (int i = 0; i < 10; i++)
        {
            GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crate.name = "Crate_" + i;
            crate.transform.position = new Vector3(Random.Range(-20f, 20f), 0.5f, Random.Range(-20f, 20f));
            crate.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }

    private void CreatePlayer()
    {
        GameObject playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerGO.name = "Player";
        playerGO.transform.position = new Vector3(0f, 1f, 0f);

        CharacterController controller = playerGO.AddComponent<CharacterController>();
        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center = new Vector3(0f, 0.9f, 0f);

        PlayerController player = playerGO.AddComponent<PlayerController>();
        player.SetCameraMode(CameraMode.ThirdPerson);

        PlayerHealth health = playerGO.AddComponent<PlayerHealth>();
        health.Heal(100f);

        WeaponController weapon = playerGO.AddComponent<WeaponController>();

        GameObject firstPersonAnchor = new GameObject("FirstPersonAnchor");
        firstPersonAnchor.transform.SetParent(playerGO.transform, false);
        firstPersonAnchor.transform.localPosition = new Vector3(0f, 1.2f, 0.1f);

        GameObject thirdPersonAnchor = new GameObject("ThirdPersonAnchor");
        thirdPersonAnchor.transform.SetParent(playerGO.transform, false);
        thirdPersonAnchor.transform.localPosition = new Vector3(0f, 1.65f, -4.5f);

        GameObject muzzle = new GameObject("Muzzle");
        muzzle.transform.SetParent(playerGO.transform, false);
        muzzle.transform.localPosition = new Vector3(0.25f, 1.35f, 0.7f);

        var muzzleFlash = muzzle.AddComponent<ParticleSystem>();
        var main = muzzleFlash.main;
        main.startColor = Color.yellow;
        main.startSize = 0.08f;
        main.loop = false;

        weapon.SetCamera(Camera.main);
    }

    private void CreateCameraRig()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraGO = new GameObject("Main Camera", typeof(Camera));
            cameraGO.tag = "MainCamera";
            mainCamera = cameraGO.GetComponent<Camera>();
        }

        mainCamera.clearFlags = CameraClearFlags.Skybox;
        mainCamera.nearClipPlane = 0.1f;
        mainCamera.fieldOfView = 72f;

        CameraController cameraController = mainCamera.gameObject.GetComponent<CameraController>();
        if (cameraController == null)
        {
            cameraController = mainCamera.gameObject.AddComponent<CameraController>();
        }

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            cameraController.SetTarget(player.transform);

            GameObject firstPersonAnchor = new GameObject("CameraFirstPersonAnchor");
            firstPersonAnchor.transform.SetParent(player.transform, false);
            firstPersonAnchor.transform.localPosition = new Vector3(0f, 1.2f, 0.1f);

            GameObject thirdPersonAnchor = new GameObject("CameraThirdPersonAnchor");
            thirdPersonAnchor.transform.SetParent(player.transform, false);
            thirdPersonAnchor.transform.localPosition = new Vector3(0f, 2.3f, -5.0f);

            cameraController.SetTarget(player.transform);
        }
    }

    private void CreateMobileControls()
    {
        GameObject rigGO = new GameObject("MobileControlRig");
        MobileControlRig rig = rigGO.AddComponent<MobileControlRig>();
        rig.CreateRuntimeUi();
    }
}
