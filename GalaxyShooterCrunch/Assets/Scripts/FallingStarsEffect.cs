using UnityEngine;

public class FallingStarsEffect : MonoBehaviour
{
    private ParticleSystem particles;
    public float minSpeed = 8f;
    public float maxSpeed = 15f;
    public float spawnRate = 5f;
    
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        if (particles == null)
        {
            particles = gameObject.AddComponent<ParticleSystem>();
        }
        
        ConfigureParticles();
    }
    
    void ConfigureParticles()
    {
        var main = particles.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(minSpeed, maxSpeed);
        main.startLifetime = 2f;
        main.startSize = 0.1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.gravityModifier = 0.1f; // Slight curve
        
        var emission = particles.emission;
        emission.rateOverTime = spawnRate;
        
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(20f, 0f, 0f);
        shape.position = new Vector3(0f, 6f, 0f); // Top of screen
        
        var colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(Color.white, 0.0f), 
                new GradientColorKey(Color.cyan, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        colorOverLifetime.color = gradient;
    }
    
    void Update()
    {
        // Increase stars during gameplay
        if (Time.timeSinceLevelLoad > 30f)
        {
            var emission = particles.emission;
            emission.rateOverTime = 10f;
        }
    }
}