using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    #region Variables
    // Spawner Patterns and Configurations
    [SerializeField] private PatternType _patternType;
    [SerializeField] private EmitterShape _emitterShape;
    [SerializeField] private bool _useEmitter;
    [SerializeField] private string _spawnerName;

    // Collision Detection
    [SerializeField] private bool _useCollision;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField, Range(0.0f, 1f)] private float _lifeLoss;

    // Bullet Configurations
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private float _bulletSize;

    // Bullet Appearance
    [SerializeField] private Sprite _bulletSprite;
    [SerializeField] private bool _useColor;
    [SerializeField] private Color _bulletColor;

    // Bullet Material Path
    private const string _materialPath = "Sprites/Default";

    // Spawner General Settings
    [SerializeField] private int _numberOfColumns;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _spinSpeed;
    [SerializeField] private float _followRefreshRate;
    [SerializeField] private Transform _target;
    [SerializeField] private bool _useReversedSpiral;
    private float _angle;

    // Spawner Polygon & Star Shaped Settings
    [SerializeField] private int _numberOfSides;
    [SerializeField] private int _dotsPerSide;
    private float _starRatio = 0.8f;

    // Particle System
    private ParticleSystem _particleSystem;
    
    // Enums Definitions
    /// <summary>
    /// Contains the different patterns that the spawner can use.
    /// </summary>
    public enum PatternType
    {
        TargetShoot, Circular, SingleSpiral,
        MultiSpiral, Polygonal, StarShaped,
    }

    /// <summary>
    /// Contains the different shapes that the emitter can use.
    /// </summary>
    public enum EmitterShape
    {
        Straight, Box, Cone
    }
    #endregion

    #region Properties
    // Spawner Patterns and Configurations
    /// <summary>
    /// Changes the current pattern of the spawner.
    /// </summary>
    /// <remarks>
    /// 0: Target Shoot, 1: Circular, 2: Single Spiral, 3: Multi Spiral, 4: Polygonal, 5: Star Shaped
    /// </remarks>
    public PatternType CurrentPattern
    {
        get => _patternType;
        set => _patternType = value;
    }

    /// <summary>
    /// Determines if the spawner will use a custom emitter shape.
    /// </summary>
    public bool UseEmitter
    {
        get => _useEmitter;
        set => _useEmitter = value;
    }

    /// <summary>
    /// Change the current emitter shape of the spawner.
    /// </summary>
    /// <remarks>
    /// 0: Straight, 1: Box, 2: Cone
    /// </remarks>
    public EmitterShape CurrentEmitter
    {
        get => _emitterShape;
        set => _emitterShape = value;
    }

    /// <summary>
    /// The name of the spawner object in the hierarchy.
    /// </summary>
    public string SpawnerName
    {
        get => _spawnerName;
        set => _spawnerName = value;
    }

    // Collision Detection
    /// <summary>
    /// Determines if the spawner will use collision detection.
    /// </summary>
    public bool UseCollision
    {
        get => _useCollision;
        set => _useCollision = value;
    }

    /// <summary>
    /// Selects the layers that the spawner will collide with.
    /// </summary>
    public LayerMask CollisionMask
    {
        get => _collisionMask;
        set => _collisionMask = value;
    }

    /// <summary>
    /// The amount of life that the bullet will lose when colliding with an object. Must be between 0 and 1.
    /// </summary>
    public float LifeLoss
    {
        get => _lifeLoss;
        set => _lifeLoss = Mathf.Clamp01(value);
    }

    // Bullet Configurations
    /// <summary>
    /// The speed that the bullet will travel. Cannot be 0.
    /// </summary>
    public float BulletSpeed
    {
        get => _bulletSpeed;
        set => _bulletSpeed = Mathf.Max(0.1f, value);
    }

    /// <summary>
    /// The time in seconds that the bullet will exist. Cannot be 0.
    /// </summary>
    public float BulletLifeTime
    {
        get => _bulletLifeTime;
        set => _bulletLifeTime = Mathf.Max(0.1f, value);
    }

    /// <summary>
    /// The size of the bullet. Cannot be 0.
    /// </summary>
    public float BulletSize
    {
        get => _bulletSize;
        set => _bulletSize = Mathf.Max(0.1f, value);
    }

    // Bullet Appearance
    /// <summary>
    /// The sprite that the bullet will use. Cannot be null.
    /// </summary>
    public Sprite BulletSprite
    {
        get => _bulletSprite;
        set => _bulletSprite = value;
    }

    /// <summary>
    /// Determines if the bullet will have a color over the sprite.
    /// </summary>
    public bool UseColor
    {
        get => _useColor;
        set => _useColor = value;
    }

    /// <summary>
    /// The color with which the sprite will be tinted.
    /// </summary>
    public Color BulletColor
    {
        get => _bulletColor;
        set => _bulletColor = value;
    }

    // Spawner General Settings
    /// <summary>
    /// The number of columns the spawner will have. Cannot be 0. Some patterns will override this value.
    /// </summary>
    public int NumberOfColumns
    {
        get => _numberOfColumns;
        set => _numberOfColumns = Mathf.Max(1, value);
    }

    /// <summary>
    /// The rate at which the spawner will shoot bullets. Cannot be 0.
    /// </summary>
    public float FireRate
    {
        get => _fireRate;
        set => _fireRate = Mathf.Max(0.1f, value);
    }

    /// <summary>
    /// The speed at which the spawner will rotate. Can be negative and 0.
    /// </summary>
    public float SpinSpeed
    {
        get => _spinSpeed;
        set => _spinSpeed = value;
    }

    /// <summary>
    /// The time in seconds the spawner will wait before following the target. Use 0 for no delay.
    /// </summary>
    public float FollowRefreshRate
    {
        get => _followRefreshRate;
        set => _followRefreshRate = Mathf.Abs(value);
    }

    /// <summary>
    /// The target that the bullets will follow. Cannot be null if the pattern is Target Shoot.
    /// </summary>
    public Transform Target
    {
        get => _target;
        set => _target = value;
    }

    /// <summary>
    /// Determines if the spawner will use a reversed spiral pattern.
    /// </summary>
    public bool UseReversedSpiral
    {
        get => _useReversedSpiral;
        set => _useReversedSpiral = value;
    }

    // Spawner Polygon & Star Shaped Settings
    /// <summary>
    /// Amount of sides that the polygon will have. Cannot be less than 3.
    /// </summary>
    public int NumberOfSides
    {
        get => _numberOfSides;
        set => _numberOfSides = Mathf.Max(3, value);
    }

    /// <summary>
    /// The number of dots that the will be in each side. Cannot be less than 1.
    /// </summary>
    public int DotsPerSide
    {
        get => _dotsPerSide;
        set => _dotsPerSide = Mathf.Max(1, value);
    }
    #endregion

    private void Update()
    {
        if (LocalTime.TimeScale == 0)
        {
            PauseSpawner(true);
        }

        if (LocalTime.TimeScale == 1)
        {
            PauseSpawner(false);
        }
    }

    #region Spawner Control
    /// <summary>
    /// Initializes the spawner with the current pattern and settings.
    /// </summary>
    public void StartSpawner()
    {
        if (!_useEmitter)
        {
            _emitterShape = EmitterShape.Straight;
        }

        switch (_patternType)
        {
            case PatternType.TargetShoot:
                InitTargetShoot();
                break;
            case PatternType.Circular:
                InitCircular();
                break;
            case PatternType.SingleSpiral:
                InitSingleSpiral();
                break;
            case PatternType.MultiSpiral:
                InitMultiSpiral();
                break;
            case PatternType.Polygonal:
                InitPolygonal();
                break;
            case PatternType.StarShaped:
                InitStarShaped();
                break;
        }
    }

    /// <summary>
    /// Stops the spawner and destroys the spawner objects after every bullet disappears.
    /// </summary>
    public void StopSpawner()
    {
        // Check if there are spawners to stop
        if (transform.childCount == 0)
        {
            Debug.LogWarning("There are no spawners to stop");
        }

        // Change the name of the spawners to stop them
        foreach (Transform spawner in transform)
        {
            spawner.name = "Stopped";
        }

        CancelInvoke("Shoot");
        StopAllCoroutines();
        StartCoroutine(DestroySpawners());
    }

    /// <summary>
    /// Stops the spawner from shooting and freezes the bullets in place.
    /// </summary>
    /// <param name="isPaused"></param> Determines if the spawner will be paused or resumed.
    public void PauseSpawner(bool isPaused)
    {
        // Change the speed of the bullets based on the pause state.
        float bulletSpeed = isPaused ? 0 : _bulletSpeed;

        // Go through all the spawners and change the speed of the bullets.
        foreach (Transform spawner in transform)
        {
            foreach (Transform child in spawner)
            {
                _particleSystem = child.GetComponent<ParticleSystem>();
                var mainModule = _particleSystem.main;
                mainModule.startSpeed = bulletSpeed;
                if (isPaused)
                {
                    // Pause the particle system if true.
                    _particleSystem.Pause();
                }
                else
                {
                    // Play the particle system if false.
                    _particleSystem.Play();
                }
            }
        }
    }

    private float[] CalculatePolygonalSpeeds()
    {
        // Set the default values of speed and angle
        List<float> speeds = new List<float>();
        float originalSpeed = _bulletSpeed;
        float originalAngle = (360 / _numberOfSides);
        float alphaAngle = (180 - originalAngle) / 2;
        float fixedAngle = originalAngle / (_dotsPerSide + 1);

        // Divide the polygon into triangles and calculate the speed of each bullet based on the dots per side
        for (int i = 0; i < _dotsPerSide; i++)
        {
            speeds.Add(originalSpeed);

            float betaAngle = 180 - (alphaAngle + fixedAngle);
            float distance = originalSpeed * Mathf.Sin(fixedAngle * Mathf.Deg2Rad) / Mathf.Sin(betaAngle * Mathf.Deg2Rad);

            originalSpeed = Mathf.Sqrt(Mathf.Pow(originalSpeed, 2) + Mathf.Pow(distance, 2) - 2 * originalSpeed * distance * Mathf.Cos(alphaAngle * Mathf.Deg2Rad));

            float newAngle = 180 - betaAngle;
            alphaAngle = newAngle;
        }

        speeds.Add(originalSpeed);

        // Repeat the speeds for all the sides
        List<float> allSpeeds = new List<float>();
        for (int i = 0; i < _numberOfSides; i++)
        {
            allSpeeds.AddRange(speeds);
        }

        return allSpeeds.ToArray();
    }
    #endregion

    #region Patterns Initialization
    private void InitTargetShoot()
    {
        _numberOfColumns = 1; // Target Shoot only shoots from one direction

        // Default name and target check
        if (_spawnerName == string.Empty) _spawnerName = "TargetShoot";
        if (_target == null)
        {
            Debug.Log("Target is not assigned");
            return;
        }

        // Instantiate the spawner and start following the target
        var spawner = InstantiateSpawner(_spawnerName, false);
        StartCoroutine(FollowTarget(spawner));
    }

    private void InitCircular()
    {
        _spinSpeed = 0; // Circular pattern does not spin

        // Default name
        if (_spawnerName == string.Empty) _spawnerName = "Circular";

        // Instantiate the spawner
        InstantiateSpawner(_spawnerName, false);
    }

    private void InitSingleSpiral()
    {
        _numberOfColumns = 1; // Single Spiral only shoots one column

        // Default name
        if (_spawnerName == string.Empty) _spawnerName = "SingleSpiral";

        if (_useReversedSpiral)
        {
            // Instantiate one spawner for each spiral
            var originalSpiral = InstantiateSpawner(_spawnerName, true);
            StartCoroutine(SpinSpawner(originalSpiral, 1));

            var reversedSpiral = InstantiateSpawner(_spawnerName, false);
            StartCoroutine(SpinSpawner(reversedSpiral, -1));
        }
        else
        {
            // Instantiate only one spawner
            var spawner = InstantiateSpawner(_spawnerName, false);
            StartCoroutine(SpinSpawner(spawner, 1));
        }
    }

    private void InitMultiSpiral()
    {
        // Default name
        if (_spawnerName == string.Empty) _spawnerName = "MultiSpiral";

        if (_useReversedSpiral)
        {
            // Instantiate one spawner for each spiral
            var originalSpiral = InstantiateSpawner(_spawnerName, true);
            StartCoroutine(SpinSpawner(originalSpiral, 1));

            var reversedSpiral = InstantiateSpawner(_spawnerName, false);
            StartCoroutine(SpinSpawner(reversedSpiral, -1));
        }
        else
        {
            // Instantiate only one spawner
            var spawner = InstantiateSpawner(_spawnerName, false);
            StartCoroutine(SpinSpawner(spawner, 1));
        }
    }

    private void InitPolygonal()
    {
        // Default name
        if (_spawnerName == string.Empty) _spawnerName = "Polygonal";

        // Calculate the speeds of the bullets
        float[] speeds = CalculatePolygonalSpeeds();

        // Instantiate the spawner based on the number of sides and dots inside
        _numberOfColumns = _numberOfSides * (_dotsPerSide + 1);
        var spawner = InstantiateSpawner(_spawnerName, false);

        // Start the coroutine to keep the polygonal shape
        StartCoroutine(KeepPolygonal(spawner, speeds));

        // Start the coroutine to spin the spawner
        StartCoroutine(SpinSpawner(spawner, 1));
    }

    private void InitStarShaped()
    {
        // Default name
        if (_spawnerName == string.Empty) _spawnerName = "StarShaped";

        // Obtain the velocity of the bullets of each column for a polygonal-like shape
        float[] speeds = CalculatePolygonalSpeeds();

        // Change the speeds to create the star shape
        // Speed is changed cyclically. Ej: If star has 5 tips with 3 dots between each tip, at a speed of 1, the first tip will have a speed of 1, the first dot a speed of 0.5, the second dot a speed of 0.25, the third dot a speed of 0.5, and the second tip a speed of 1.
        int dotsPerSide = _dotsPerSide + 1;
        _numberOfColumns = _numberOfSides * dotsPerSide;
        for (int i = 0; i <= _numberOfSides - 1; i++)
        {
            int tipIndex = i * dotsPerSide;
            speeds[tipIndex] = _bulletSpeed;
            if (dotsPerSide % 2 == 0)
            {
                for (int j = 1; j <= dotsPerSide / 2; j++)
                {
                    speeds[tipIndex + j] = _bulletSpeed * Mathf.Pow(_starRatio, j);
                    speeds[tipIndex + dotsPerSide - j] = _bulletSpeed * Mathf.Pow(_starRatio, j);
                }
            }
            else
            {
                for (int j = 1; j <= dotsPerSide / 2; j++)
                {
                    speeds[tipIndex + j] = _bulletSpeed * Mathf.Pow(_starRatio, j);
                    speeds[tipIndex + dotsPerSide - j] = _bulletSpeed * Mathf.Pow(_starRatio, j);
                }
                speeds[tipIndex + dotsPerSide / 2] = _bulletSpeed * Mathf.Pow(_starRatio, dotsPerSide / 2);
            }
        }

        // Create the spawner based on the sides and between dots
        var spawner = InstantiateSpawner(_spawnerName, false);

        // Change the velocity of the bullets
        StartCoroutine(KeepPolygonal(spawner, speeds));

        // Spin the spawner
        StartCoroutine(SpinSpawner(spawner, 1));
    }
    #endregion

    #region Spawner Initialization
    private Transform InstantiateSpawner(string name, bool isReversed)
    {
        _angle = 360f / _numberOfColumns; // Divide the circle into equal parts

        // Create a spawner object to hold the particle system
        string spawnerName = isReversed ? "R_" + name : "N_" + name; // 'R' for reversed, 'N' for normal
        var spawner = new GameObject(spawnerName);
        spawner.transform.position = transform.position;
        spawner.transform.rotation = transform.rotation;
        spawner.transform.SetParent(transform);

        for (int i = 0; i < _numberOfColumns; i++)
        {
            Material particleMaterial = new Material(Shader.Find(_materialPath));

            // Initialize the child object that will hold the particle system
            var container = new GameObject($"{i}_ParticleSystem_{_angle * i}º"); // Syntax: ObjectNumber_ParticleSystem_Angleº
            container.transform.position = spawner.transform.position;
            container.transform.Rotate(_angle * i, 90f, 0f);
            container.transform.SetParent(spawner.transform);

            // Create the paprticle system
            _particleSystem = container.AddComponent<ParticleSystem>();
            var mainModule = _particleSystem.main;
            mainModule.startSpeed = _bulletSpeed;
            mainModule.maxParticles = int.MaxValue;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

            // Adjust the renderer for the material and particle rotation
            var renderer = container.GetComponent<ParticleSystemRenderer>();
            renderer.material = particleMaterial;
            renderer.renderMode = ParticleSystemRenderMode.Stretch;

            // Disable the emission
            var emission = _particleSystem.emission;
            emission.enabled = false;

            // Set the shape of the emitter
            var shape = _particleSystem.shape;
            shape.enabled = true;
            switch (_emitterShape)
            {
                case EmitterShape.Straight:
                    shape.shapeType = ParticleSystemShapeType.Sprite;
                    break;
                case EmitterShape.Box:
                    shape.shapeType = ParticleSystemShapeType.Box;
                    break;
                case EmitterShape.Cone:
                    shape.shapeType = ParticleSystemShapeType.Cone;
                    break;
            }
            shape.sprite = null;
            shape.alignToDirection = true;

            // Add the sprite texture (sprite must be aligned to the left)
            var texture = _particleSystem.textureSheetAnimation;
            texture.enabled = true;
            texture.mode = ParticleSystemAnimationMode.Sprites;
            texture.AddSprite(_bulletSprite);

            // Set the collision
            if (_useCollision)
            {
                var collision = _particleSystem.collision;
                collision.enabled = true;
                collision.type = ParticleSystemCollisionType.World;
                collision.mode = ParticleSystemCollisionMode.Collision2D;
                collision.collidesWith = _collisionMask;
                collision.lifetimeLoss = _lifeLoss;
                collision.sendCollisionMessages = true;
            }
        }

        // Start the shooting
        InvokeRepeating("Shoot", 0f, _fireRate);

        // Return the spawner object
        return spawner.transform;
    }

    private void Shoot()
    {
        if (LocalTime.TimeScale == 0) return;

        // Travel through all the spawners
        foreach (Transform spawner in transform)
        {
            if (spawner.name != "Stopped")
            {
                // Travel through all the particle systems of the spawner
                foreach (Transform child in spawner)
                {
                    _particleSystem = child.GetComponent<ParticleSystem>();
                    if (_particleSystem != null)
                    {
                        var emitParams = new ParticleSystem.EmitParams()
                        {
                            startColor = _useColor ? _bulletColor : Color.white,
                            startSize = _bulletSize,
                            startLifetime = _bulletLifeTime
                        };
                        _particleSystem.Emit(emitParams, 1);
                    }
                }
            }
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator FollowTarget(Transform spawner)
    {
        while (true)
        {
            if (_target != null)
            {
                Vector3 direction = _target.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spawner.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            yield return new WaitForSeconds(_followRefreshRate);
        }
    }

    private IEnumerator SpinSpawner(Transform spawner, int direction)
    {
        while (spawner != null)
        {
            spawner.Rotate(0, 0, _spinSpeed * direction * Time.deltaTime * LocalTime.TimeScale);
            yield return null;
        }
    }

    private IEnumerator DestroySpawners()
    {
        while (LocalTime.TimeScale == 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(_bulletLifeTime);
        foreach (Transform spawner in transform)
        {
            if (spawner.name == "Stopped")
            {
                Destroy(spawner.gameObject);
            }
        }
    }

    private IEnumerator KeepPolygonal(Transform spawner, float[] speeds)
    {
        while (true)
        {
            for (int i = 0; i < _numberOfColumns; i++)
            {
                Transform child = spawner.GetChild(i);
                var mainModule = child.GetComponent<ParticleSystem>().main;
                mainModule.startSpeed = speeds[i];
            }
            yield return null;
        }
    }
    #endregion
}