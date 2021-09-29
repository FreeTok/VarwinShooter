using UnityEngine;
using UnityEngine.SceneManagement;

public class PredictionManager : MonoBehaviour{

    Scene currentScene;
    Scene predictionScene;

    PhysicsScene currentPhysicsScene;
    PhysicsScene predictionPhysicsScene;

    LineRenderer lineRenderer;
    GameObject dummy;

    public GameObject firePoint;
    public GameObject bulletPrefab;

    public int maxIterations;
    public float power;

    Vector3 currentPosition;
    Quaternion currentRotation;

    void Start(){
        Physics.autoSimulation = false;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene();

        lineRenderer = GetComponent<LineRenderer>();

        currentPosition = transform.position;
        currentRotation = transform.rotation;
        predict();
    }

    void FixedUpdate(){
        if (currentPhysicsScene.IsValid()){
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }

        if (currentRotation != transform.rotation || currentPosition != transform.position)
            predict();

        currentRotation = transform.rotation;
        currentPosition = transform.position;
    }

    public void predict(){
        if (currentPhysicsScene.IsValid() && predictionPhysicsScene.IsValid()){
            if(dummy == null){
                dummy = Instantiate(bulletPrefab);
                SceneManager.MoveGameObjectToScene(dummy, predictionScene);
            }

            dummy.transform.position = firePoint.transform.position;
            dummy.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = maxIterations;


            for (int i = 0; i < maxIterations; i++){
                predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
                lineRenderer.SetPosition(i, dummy.transform.position);
            }

            Destroy(dummy);
        }
    }
}
