using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BulletSystem))]
public class BulletSystemEditor : Editor
{
    // Spawner Patterns and Configurations
    private SerializedProperty _patternType;
    private SerializedProperty _emitterShape;
    private SerializedProperty _useEmitter;
    private SerializedProperty _spawnerName;

    // Collision Detection
    private SerializedProperty _useCollision;
    private SerializedProperty _collisionMask;
    private SerializedProperty _bulletsTag;
    private SerializedProperty _lifeLoss;

    // Bullet Configurations
    private SerializedProperty _bulletSpeed;
    private SerializedProperty _bulletLifeTime;
    private SerializedProperty _bulletSize;

    // Bullet Appearance
    private SerializedProperty _bulletSprite;
    private SerializedProperty _useColor;
    private SerializedProperty _bulletColor;

    // Spawner General settings
    private SerializedProperty _numberOfColumns;
    private SerializedProperty _fireRate;
    private SerializedProperty _spinSpeed;
    private SerializedProperty _followRefreshRate;
    private SerializedProperty _target;
    private SerializedProperty _useReversedSpiral;

    // Spawner Polygonal & Star Shape settings
    private SerializedProperty _numberOfSides;
    private SerializedProperty _dotsPerSide;

    // Pattern and Emitter names for the dropdown menu
    private string[] _patternNames = { "1. Target Shoot", "2. Circular", "3. Single Spiral", "4. Multi Spiral", "5. Polygonal", "6. Star Shape" };
    private string[] _emitterNames = { "1. Straight Line", "2. Box Shape", "3. Cone Shape" };

    private void OnEnable()
    {
        // Spawner Patterns and Configurations
        _patternType = serializedObject.FindProperty("_patternType");
        _emitterShape = serializedObject.FindProperty("_emitterShape");
        _useEmitter = serializedObject.FindProperty("_useEmitter");
        _spawnerName = serializedObject.FindProperty("_spawnerName");

        // Collision Detection
        _useCollision = serializedObject.FindProperty("_useCollision");
        _collisionMask = serializedObject.FindProperty("_collisionMask");
        _bulletsTag = serializedObject.FindProperty("_bulletsTag");
        _lifeLoss = serializedObject.FindProperty("_lifeLoss");

        // Bullet Configurations
        _bulletSpeed = serializedObject.FindProperty("_bulletSpeed");
        _bulletLifeTime = serializedObject.FindProperty("_bulletLifeTime");
        _bulletSize = serializedObject.FindProperty("_bulletSize");

        // Bullet Appearance
        _bulletSprite = serializedObject.FindProperty("_bulletSprite");
        _useColor = serializedObject.FindProperty("_useColor");
        _bulletColor = serializedObject.FindProperty("_bulletColor");

        // Spawner General settings
        _numberOfColumns = serializedObject.FindProperty("_numberOfColumns");
        _fireRate = serializedObject.FindProperty("_fireRate");
        _spinSpeed = serializedObject.FindProperty("_spinSpeed");
        _followRefreshRate = serializedObject.FindProperty("_followRefreshRate");
        _target = serializedObject.FindProperty("_target");
        _useReversedSpiral = serializedObject.FindProperty("_useReversedSpiral");

        // Spawner Polygonal & Star Shape settings
        _numberOfSides = serializedObject.FindProperty("_numberOfSides");
        _dotsPerSide = serializedObject.FindProperty("_dotsPerSide");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // BulletSystem script reference
        BulletSystem bulletSystem = (BulletSystem)target;

        // Pattern selector section
        DrawTitleLabel("Pattern Selector", "Choose between different patterns to customize.");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        _patternType.enumValueIndex = EditorGUILayout.Popup(_patternType.enumValueIndex, _patternNames);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_useEmitter, new GUIContent("Use Emitter:", "Change between different emitter shapes."));
        if (bulletSystem.UseEmitter)
        {
            _emitterShape.enumValueIndex = EditorGUILayout.Popup(_emitterShape.enumValueIndex, _emitterNames);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_spawnerName, new GUIContent("Spawner Name:", "Name of the spawner. Leave blank for deafult name"));
        GUILayout.EndHorizontal();

        // Collision Detection section
        DrawTitleLabel("Collision Detection", "Enable or disable collision detection.");

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_useCollision, new GUIContent("Use Collision:", "Enable or disable collision detection."));
        if (bulletSystem.UseCollision)
        {
            EditorGUILayout.PropertyField(_collisionMask, GUIContent.none);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (bulletSystem.UseCollision)
        {
            EditorGUILayout.PropertyField(_bulletsTag, new GUIContent("Bullets Tag:", "The tag that the bullets will have."));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (bulletSystem.UseCollision)
        {
            EditorGUILayout.PropertyField(_lifeLoss, new GUIContent("Life Loss:", "Life loss when a collision is detected."));
        }
        GUILayout.EndHorizontal();

        // Bullet Configurations and Appearence section
        DrawTitleLabel("Bullet Settings");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(new GUIContent("Speed:", "The speed that the bullet will travel. Takes absolute value, set to 1 if recieves 0."));
        EditorGUILayout.PropertyField(_bulletSpeed, GUIContent.none);
        _bulletSpeed.floatValue = _bulletSpeed.floatValue == 0 ? 1 : Mathf.Abs(_bulletSpeed.floatValue);
        GUILayout.Label(new GUIContent("Life Time:", "The time in seconds that the bullet will exist. Takes absolute value, set to 1 if recieves 0."));
        EditorGUILayout.PropertyField(_bulletLifeTime, GUIContent.none);
        _bulletLifeTime.floatValue = _bulletLifeTime.floatValue == 0 ? 1 : Mathf.Abs(_bulletLifeTime.floatValue);
        GUILayout.Label(new GUIContent("Size:", "The size of the bullet. Takes absolute value, set to 1 if recieves 0."));
        _bulletSize.floatValue = _bulletSize.floatValue == 0 ? 1 : Mathf.Abs(_bulletSize.floatValue);
        EditorGUILayout.PropertyField(_bulletSize, GUIContent.none);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Sprite:", "The sprite that the bullet will use."));
        EditorGUILayout.PropertyField(_bulletSprite, GUIContent.none);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_useColor, new GUIContent("Use Color:", "Enable or disable color for the bullet. If false, default color is set to white."));
        if (bulletSystem.UseColor)
        {
            EditorGUILayout.PropertyField(_bulletColor, GUIContent.none);
        }
        GUILayout.EndHorizontal();

        // Pattern specific settings
        switch (bulletSystem.CurrentPattern)
        {
            case BulletSystem.PatternType.TargetShoot:
                DrawTitleLabel("Target Shoot Pattern");

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Fire Rate:", "The time in seconds each bullet is shot. Takes absolute value, set to 0.1 if recieves 0."));
                EditorGUILayout.PropertyField(_fireRate, GUIContent.none);
                _fireRate.floatValue = _fireRate.floatValue == 0 ? 0.1f : Mathf.Abs(_fireRate.floatValue);
                GUILayout.Label(new GUIContent("Follow Rate:", "The time in seconds the spawner will wait before following the target. Use 0 for no delay."));
                EditorGUILayout.PropertyField(_followRefreshRate, GUIContent.none);
                _followRefreshRate.floatValue = Mathf.Abs(_followRefreshRate.floatValue);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Target:", "The target that the bullets will follow. Won't start if it's null."));
                EditorGUILayout.PropertyField(_target, GUIContent.none);
                GUILayout.EndHorizontal();
                break;
            case BulletSystem.PatternType.Circular:
                DrawTitleLabel("Circular Pattern");

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Precition:", "The number of dots that the circle will have. Takes absolute value, set to 1 if recieves 0."));
                EditorGUILayout.PropertyField(_numberOfColumns, GUIContent.none);
                _numberOfColumns.intValue = _numberOfColumns.intValue == 0 ? 1 : Mathf.Abs(_numberOfColumns.intValue);
                GUILayout.Label(new GUIContent("Fire Rate:", "The time in seconds each bullet is shot. Takes absolute value, set to 0.1 if recieves 0."));
                EditorGUILayout.PropertyField(_fireRate, GUIContent.none);
                _fireRate.floatValue = _fireRate.floatValue == 0 ? 0.1f : Mathf.Abs(_fireRate.floatValue);
                GUILayout.EndHorizontal();
                break;

            case BulletSystem.PatternType.SingleSpiral:
                DrawTitleLabel("Spiral Pattern");

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Spin Speed:", "The speed that the spiral will rotate. Takes absolute value."));
                EditorGUILayout.PropertyField(_spinSpeed, GUIContent.none);
                _spinSpeed.floatValue = Mathf.Abs(_spinSpeed.floatValue);
                GUILayout.Label(new GUIContent("Fire Rate:", "The time in seconds each bullet is shot. Takes absolute value, set to 0.1 if recieves 0."));
                EditorGUILayout.PropertyField(_fireRate, GUIContent.none);
                _fireRate.floatValue = _fireRate.floatValue == 0 ? 0.1f : Mathf.Abs(_fireRate.floatValue);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_useReversedSpiral, new GUIContent("Use Reversed Spiral:", "Spawns a second spiral with inverse rotation."));
                GUILayout.EndHorizontal();
                break;

            case BulletSystem.PatternType.MultiSpiral:
                DrawTitleLabel("Polygonal Pattern");

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Spirals:", "The number of spirals that the pattern will have. Takes absolute value, set to 1 if recieves 0."));
                EditorGUILayout.PropertyField(_numberOfColumns, GUIContent.none);
                _numberOfColumns.intValue = _numberOfColumns.intValue == 0 ? 1 : Mathf.Abs(_numberOfColumns.intValue);
                GUILayout.Label(new GUIContent("Fire Rate:", "The time in seconds each bullet is shot. Takes absolute value, set to 0.1 if recieves 0."));
                EditorGUILayout.PropertyField(_fireRate, GUIContent.none);
                _fireRate.floatValue = _fireRate.floatValue == 0 ? 0.1f : Mathf.Abs(_fireRate.floatValue);
                GUILayout.Label(new GUIContent("Spin Speed:", "The speed that the spirals will rotate. Takes absolute value."));
                EditorGUILayout.PropertyField(_spinSpeed, GUIContent.none);
                _spinSpeed.floatValue = Mathf.Abs(_spinSpeed.floatValue);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_useReversedSpiral, new GUIContent("Use Reversed Spiral:", "Spawns a second spiral with inverse rotation."));
                GUILayout.EndHorizontal();
                break;

            case BulletSystem.PatternType.Polygonal:
                DrawTitleLabel("Polygonal Pattern");

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Sides:", "The number of sides that the polygon will have. Takes absolute value, cannot be less than 3"));
                EditorGUILayout.PropertyField(_numberOfSides, GUIContent.none);
                _numberOfSides.intValue = _numberOfSides.intValue < 3 ? 3 : Mathf.Abs(_numberOfSides.intValue);
                GUILayout.Label(new GUIContent("Precition:", "The number of dots that the polygon will have. Takes absolute value, set to 1 if recieves 0."));
                EditorGUILayout.PropertyField(_dotsPerSide, GUIContent.none);
                _dotsPerSide.intValue = _dotsPerSide.intValue == 0 ? 1 : Mathf.Abs(_dotsPerSide.intValue);
                GUILayout.Label(new GUIContent("Fire Rate:", "The time in seconds each bullet is shot. Takes absolute value, set to 0.1 if recieves 0."));
                EditorGUILayout.PropertyField(_fireRate, GUIContent.none);
                _fireRate.floatValue = _fireRate.floatValue == 0 ? 0.1f : Mathf.Abs(_fireRate.floatValue);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Spin Speed:", "The speed that the polygon will rotate. Takes absolute value."));
                EditorGUILayout.PropertyField(_spinSpeed, GUIContent.none);
                _spinSpeed.floatValue = Mathf.Abs(_spinSpeed.floatValue);
                GUILayout.EndHorizontal();
                break;

            case BulletSystem.PatternType.StarShaped:
                DrawTitleLabel("Star Shape Pattern");

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Sides:", "The number of sides that the star will have. Takes absolute value, cannot be less than 3"));
                EditorGUILayout.PropertyField(_numberOfSides, GUIContent.none);
                _numberOfSides.intValue = _numberOfSides.intValue < 3 ? 3 : Mathf.Abs(_numberOfSides.intValue);
                GUILayout.Label(new GUIContent("Precition:", "The number of dots that the polygon will have. Takes absolute value, set to 1 if recieves 0."));
                EditorGUILayout.PropertyField(_dotsPerSide, GUIContent.none);
                _dotsPerSide.intValue = _dotsPerSide.intValue == 0 ? 1 : Mathf.Abs(_dotsPerSide.intValue);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Fire Rate:", "The time in seconds each bullet is shot. Takes absolute value, set to 0.1 if recieves 0."));
                EditorGUILayout.PropertyField(_fireRate, GUIContent.none);
                _fireRate.floatValue = _fireRate.floatValue == 0 ? 0.1f : Mathf.Abs(_fireRate.floatValue);
                GUILayout.Label(new GUIContent("Spin Speed:", "The speed that the star will rotate. Takes absolute value."));
                EditorGUILayout.PropertyField(_spinSpeed, GUIContent.none);
                _spinSpeed.floatValue = Mathf.Abs(_spinSpeed.floatValue);
                GUILayout.EndHorizontal();
                break;
        }

        // Test section
        DrawTitleLabel("Test Section", "Test the current pattern.");

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Test Pattern"))
        {
            bulletSystem.StartSpawner();
        }
        if (GUILayout.Button("Stop Pattern"))
        {
            bulletSystem.StopSpawner();
        }
        if (GUILayout.Button("Pause/Resume"))
        {
            LocalTime.TimeScale = LocalTime.TimeScale == 0 ? 1 : 0;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draws a title label with the specified title, centered in the inspector.
    /// </summary>
    /// <param name="title"></param> The title of the label.
    private void DrawTitleLabel(string title)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField(title, new GUIStyle
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState { textColor = Color.white }
        });
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }

    /// <summary>
    /// Draws a title label with the specified title and tooltip, centered in the inspector.
    /// </summary>
    /// <param name="title"></param> The title of the label.
    /// <param name="tooltip"></param> The tooltip of the label.
    private void DrawTitleLabel(string title, string tooltip)
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField(new GUIContent(title, tooltip), new GUIStyle
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState { textColor = Color.white }
        });
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }
}