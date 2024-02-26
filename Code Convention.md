# Code Convention

## Classes

Names will always be in PascalCase.

``` C#
// Not allowed:
class carFactory {...}
class car_Factory {...}

// Is Allowed:
class CarFactory {...}

```

Accesors are, by default internal. This is due to seperation via namespaces. Public can be used, if the class and it's methods are used outside of the namespace.

## Fields

Fields are prefixed by an underscore, and are spelled in camelCase.

``` C#
private int _age;
private string _localAdress;
```

Protected fieds should be avoided, whereas public and internal fields should not exist at all.
Only in extreme circumstances should they be allowed, but their existence should be explained. In this case, they are spelled in pure camelCase.

``` C#
public int count;
```

Static fields may exist in a private context, but should not exist in a public one. Prefer handling the same use case by virtue of references.

## Serialized Fields

Serialisation using `[SerializedField]` must be done on the same line as the field name, for clarity. Additional Attributes, such as range, can be defined as `[SerializedField, Range(0,10)]`.

Additionally, they must always be private, and specify the m

## Local Variables

Local variables should be in camelCase.

``` C#
private float GetArea(float height, float width)
{
    return height * width;
}
```

## Constants

Cosntants should be written in PascalCase.

``` C#
public constant string EnemyName = "BulletBill";
```

## Properties

Properties will be accessed externally, and require clear naming. Use Pascal case for these.

``` C#
public float TimeLeft {get; private set;}
```

Additionally, if they return a field, they should be written as follows:

``` C#
public float TimeLeft => _timeLeft;
```

## Methods

All methods are written in PascalCase, disregarding the modifier. By default, methods must be private, unless required outside the class or assembly.

``` C#
public float GetEnemyByDifficulty(Difficulty difficulty) {...}
```

## Events

Events should be prefaced with the `On` prefix.

``` C#
public event Action<bool> onClick;
```

Methods can (but are not required to) use the `Handle` prefix, followed by the event name.

``` C#
// Optional:
public void HandleButtonClick(bool state) {...}
```

## Interfaces

Preface an interface name with `I`. This is a widespread practice within the C# community, and ignoring it may do more harm than good.

## Singletons

Should be avoided, but may be required in specific use cases.

## Enumeration

Should always have an accessor, and written as a Singular as well as Pascal Case.

``` C#
internal enum Colour
{
    Red,
    Green,
    Blue
}
```

## Structs

Are treated in the same way as classes, and thus must be written in PascalCase.

## Structure

Regions are strongly encouraged, but not required.

```C#
class Example : Monobehaviour
{
    // Nested Entities, such as enums structs and classes.

    // Events
    
    // Serialized Fields

    // Fields

    // Properties

    // Setup, such as Awake, Start, OnDestroy

    // Update, such as Update, FixedUpdate

    // Event Handlers

    // Public Methods

    // Internal Methods

    // Protected Methods

    // Private Methods

    // Tests
}
```

## Namespaces

Namespaces should use the following style:

``` C#
namespace OrganistationName.ProjectName.Folder...;
```

Where folder is the folder the code lives in. This also acts as an overal category, IE `Score`, `Player`, `Melee`.

Sub folders are encouraged to a degree, though the majority of code should not live under `Gameplay.Folder`. Instead, `UI` and `Audio` can live in the same folder as `Score`.

## Style

### Absolutely banned

Any use of `goto`. There is absolutely no reason to use this.

### Avoid

- Using `this` if you don't need to. Your IDE will usually understand if you are trying to access it's own property or method. If you use another copy of the same class in the class, feel free to use `this`.

- Any lines over 120 characters. At that point, wrap and align the function. There is no specific request when it comes to wrapping, but preference goes to aliging vertically.
  
``` C#
Instantiate(prefab,
            transform.position
            Quaternion.Identity,
            null);
```

### Encouraged

- When using a switch statement, add a default. This can always be used for a `NotImplementedException`, allwoing the next developer to know they need to add addtional cases.

- When using a switch statement, a [Switch Expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression) can be used if you are simply pattern matching.

- Comments should be written as sentences, starting with a capital letter. Proper punctuation however, is not required. Additionally, prefixes such as `TODO:` , `BUG:` or any other should be fixed and removed prior to opening a PR.

- Use single line `if` statements, if the code only contains the following: `return`, `break` or `throw`.

- Drop the second type specification if one already exists:

``` C#
// No:
private Gun _railGun = new Gun();

// Yes:
private Gun _railGun = new();
```

- Summaries and explanations for the method on public or internal functions. This makes it far easier for the next developer to work with your code. Please include:
    - Function Summary
    - Parameter description
    - Return values
    - Thrown exceptions

``` C#
        /// <summary>
        /// Reads the license plate from a texture, given an area and position.
        /// </summary>
        /// <param name="licensePlate">The camera feed.</param>
        /// <param name="area">The rough area of the license plate.</param>
        /// <param name="position">The position of the plate on the texture</param>
        /// <param name="licensePlateNumber">The output string</param>
        /// <returns>If the conversion was successful</returns>
        /// <exception cref="ArgumentNullException"
        /// <exception cref="ArgumentOutOfRangeException"
        public bool ReadLicensePlate(Texture licensePlate, Vector2 area, Vector2 position)
        {
            ///
            return true;
        }
```

## MonoBehaviour

- If you access a Unity Property (like [Transform](https://docs.unity3d.com/ScriptReference/Transform.html)), make a reference to the transform and use that. Unity has to perform some calls under the hood, which adds unnecessary overhead.

- Instantiate a new object by referencing it's script, if you need the script later in your code.
This is done by creating a copy of the object in your scene, and referencing that object as the prefab.

``` C#
[SerializeField] private ColliderController _colliderController;

private void SpawnEnemy()
{
    var enemy = Instantiate(_colliderController, transform, null);
    Enemy.DisableCollider(); // This works.
}
```

- Avoid using `GetComponent`. It's an expensive operation, serialize a reference instead. This reduces the wake-up time, and improves the speed of the code.

- Avoid using a `Monobehavior` instance GameObjects properties directly, such as `enabled` or `SetActive(bool)`. Instead, wrap them in a method and then call them. This makes debugging easier, and indicates more clearly that the GameObject is being manipulated.

- Try and only `Enable`/`Disable` the least impactful element. Start at component level, and then work up to GameObject, as this may impact other scripts. If you DO need to `enable` or `disable` multiple components, make a separate script to handle that.

## Asynchronous Programming

**Note: most suggestions are based on [UniTask](https://github.com/Cysharp/UniTask)**

- Favour `async` over `Coroutines`. There are several advantages:
  - It can be invoked from any method marked `async`, instead of only `Monobehaviour` classes.
  - It provides better error handling (`yield` cannot be declared in `try/catch/finally`).
  - It can return values when awaited, eliminating the need for callbacks.
  - It can be easily chained.
  - It does not require running on `Timing.PlayerLoop`, allowing other methods to take advantage of the timing.
  - It has cancellation support, allowing the use of `CancellationTokens`. This reduces risk, as opposed to Coroutines.

- Favour UniTask over C# vanilla Tasks. It has fewer GC allocations, and has some useful Unity   specific API calls.

- Methods marked `async` should always be awaited, unless:
      - They are event handlers. In that case, use `async avoid`.
      - They are unity event methods, such as `Awake` or `Start`. In that case, use `async avoid`.

- Avoid `UniTask.Forget`. Methods should mostly be awaited.

- Calls to `CancellationTokenSource.Cancel()` should always be followed by `CancellationTokenSource.Dispose()`.

- Append a `async` suffix to any `async` method for clarity.

- Asynchronous methods that should be awaited must support the use of a CancellationToken.
      - If private or protected, make sure that there is an `OnDestroy` method which cancels the token.
      - If public or internal, the method should accept a `CancellationToken` as a parameter.

- Top most Asynchronous methods should wrap the call in a `try/catch`, with a catch for **at least** `OperationCancelledException`, explaining what was cancelled and why. For example: "Connection attempt was cancelled because the operation was cancelled."

## Miscellaneous Changes

- Avoid `LayerMask.GetMask`. Instead, serialzie a `LayerMask`. This avoids potential mistakes and is slightly better for performance.

- Avoid using Unity events, due to them making debugging significantly harder, as they hide subscribers. Prefer vanilla C# events.

- Do not trust the scene hieracrhy, as this can change significantly over the course of gameplay. If you need to reference a parent or child class, serialzie a reference. Common names are `_root` or `_parent`.

- When raycasting, always consider the maxDistance. This improves performance significantly, as less calculations need to be made.
    - Never use `MathF.Infinity`.

- `FindObjectOfType` and any other general purpose searching methods should be avoided at all costs. If there truly is no other way, you should explain in comments why.
