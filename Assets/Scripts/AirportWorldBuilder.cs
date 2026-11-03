using UnityEngine;

public sealed class AirportWorldBuilder : MonoBehaviour
{
    [Header("Airport dimensions")]
    [SerializeField] private float runwayLength = 180f;
    [SerializeField] private float runwayWidth = 18f;

    private Transform worldRoot;

    public void Build()
    {
        if (worldRoot != null) Destroy(worldRoot.gameObject);

        worldRoot = new GameObject("Airport World").transform;
        worldRoot.SetParent(transform, false);

        Material grass = Mat("Grass", new Color(0.19f, 0.31f, 0.20f));
        Material concrete = Mat("Concrete", new Color(0.43f, 0.45f, 0.46f));
        Material asphalt = Mat("Runway Asphalt", new Color(0.12f, 0.13f, 0.14f));
        Material marking = Mat("Runway Markings", new Color(0.88f, 0.88f, 0.82f));
        Material glass = Mat("Terminal Glass", new Color(0.12f, 0.34f, 0.43f), 0.35f, 0.55f);
        Material white = Mat("Aircraft White", new Color(0.84f, 0.86f, 0.88f));
        Material blue = Mat("Aircraft Blue", new Color(0.08f, 0.22f, 0.48f));

        Cube("Ground", new Vector3(0, -0.6f, 0), new Vector3(320, 1, 260), grass);
        Cube("Runway", new Vector3(0, -0.05f, -78), new Vector3(runwayLength, 0.2f, runwayWidth), asphalt);
        Cube("Taxiway", new Vector3(0, 0.02f, -48), new Vector3(150, 0.16f, 8), concrete);
        Cube("Apron", new Vector3(-4, 0.04f, -20), new Vector3(115, 0.2f, 42), concrete);

        for (int i = -5; i <= 5; i++)
            Cube("Runway centre dash", new Vector3(i * 14f, 0.07f, -78), new Vector3(7, 0.025f, 0.35f), marking);

        Cube("Terminal lower", new Vector3(-7, 4, 8), new Vector3(82, 8, 15), glass);
        Cube("Terminal roof", new Vector3(-7, 8.3f, 8), new Vector3(84, 0.8f, 17), concrete);
        Cube("Terminal concourse", new Vector3(-7, 2.2f, -1), new Vector3(72, 3.8f, 3), concrete);

        for (int i = 0; i < 5; i++)
        {
            float x = -38 + i * 15f;
            Cube("Gate stand", new Vector3(x, 0.16f, -20), new Vector3(12, 0.04f, 23), marking);
            Cube("Jet bridge", new Vector3(x, 2.2f, -5.5f), new Vector3(2.2f, 3.5f, 12), concrete);
        }

        BuildAircraft(new Vector3(12, 0.4f, -23), white, blue);
        CreateCamera();
        CreateLight();
    }

    private void BuildAircraft(Vector3 position, Material fuselage, Material accent)
    {
        Transform plane = new GameObject("Demo Passenger Aircraft").transform;
        plane.SetParent(worldRoot, false);
        plane.position = position;

        Part(plane, PrimitiveType.Cylinder, "Fuselage", new Vector3(0, 1.5f, 0), new Vector3(1.25f, 7.2f, 1.25f), fuselage, new Vector3(90, 0, 0));
        Part(plane, PrimitiveType.Cube, "Main wing", new Vector3(0, 1.2f, -0.2f), new Vector3(22, 0.35f, 3.4f), accent, Vector3.zero);
        Part(plane, PrimitiveType.Cube, "Tail wing", new Vector3(0, 2.1f, 5.7f), new Vector3(8, 0.25f, 1.8f), accent, Vector3.zero);
        Part(plane, PrimitiveType.Cube, "Vertical stabilizer", new Vector3(0, 3.1f, 6.1f), new Vector3(0.3f, 3.2f, 2.4f), accent, Vector3.zero);

        for (int side = -1; side <= 1; side += 2)
        {
            Part(plane, PrimitiveType.Cylinder, "Engine nacelle", new Vector3(side * 5.4f, 0.2f, 0.2f), new Vector3(0.72f, 1.6f, 0.72f), accent, new Vector3(90, 0, 0));
        }
    }

    private void CreateCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) cam = new GameObject("Main Camera").AddComponent<Camera>();
        cam.transform.position = new Vector3(-72, 78, 92);
        cam.transform.rotation = Quaternion.Euler(38, 38, 0);
        cam.fieldOfView = 48;
        cam.tag = "MainCamera";
    }

    private void CreateLight()
    {
        if (FindFirstObjectByType<Light>() != null) return;
        GameObject sun = new GameObject("Airport Sun");
        Light light = sun.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        sun.transform.rotation = Quaternion.Euler(48, -32, 0);
    }

    private GameObject Cube(string name, Vector3 position, Vector3 scale, Material material)
        => Part(worldRoot, PrimitiveType.Cube, name, position, scale, material, Vector3.zero);

    private GameObject Part(Transform parent, PrimitiveType type, string name, Vector3 position, Vector3 scale, Material material, Vector3 rotation)
    {
        GameObject item = GameObject.CreatePrimitive(type);
        item.name = name;
        item.transform.SetParent(parent, false);
        item.transform.localPosition = position;
        item.transform.localScale = scale;
        item.transform.localEulerAngles = rotation;
        item.GetComponent<Renderer>().sharedMaterial = material;
        return item;
    }

    private Material Mat(string name, Color color, float metallic = 0f, float smoothness = 0.25f)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        Material material = new Material(shader) { name = name, color = color };
        if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
        if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
        return material;
    }
}
