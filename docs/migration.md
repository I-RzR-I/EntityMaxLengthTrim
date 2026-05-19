# Migration Guide: `v2.*` to `v3.*`

Moving from `v2.*` to `v3.*` is simpler than it may first appear. In most projects, the application code itself can stay exactly as it is. The part that usually needs attention is the package identity: v2 was distributed as `EntityMaxLengthTrim`, while v3 is distributed as `RzR.Extensions.EntityLength`.

That distinction matters for package references, direct assembly references, and any setup that depends on the assembly name. It does not usually require a namespace rewrite, because the public namespaces already used by consumer code remain under `RzR.Extensions.EntityLength`.

## What You Need to Change

The first step is to replace the old NuGet package reference with the new one. If you are upgrading through Package Manager Console, the update looks like this:

```powershell
Uninstall-Package EntityMaxLengthTrim
Install-Package RzR.Extensions.EntityLength -Version 3.*
```

If you are updating an SDK-style project file, the package reference changes from the old package ID to the new one:

```xml
<!-- v2 -->
<PackageReference Include="EntityMaxLengthTrim" Version="2.*" />

<!-- v3 -->
<PackageReference Include="RzR.Extensions.EntityLength" Version="3.*" />
```

If your solution uses centralized package management, make the same package ID change there as well.

The second thing to check is whether your solution refers to the old assembly name directly. This usually only matters in projects that load assemblies by name, keep explicit DLL references, use binding redirects, or rely on reflection-based loading. In those cases, update `EntityMaxLengthTrim` to `RzR.Extensions.EntityLength`.

One smaller dependency detail is worth noting. In v2, `CodeSource` came in through this package. In v3, that transitive dependency is gone. Most projects will not notice any difference, but if your application uses `CodeSource` types directly, you should add your own explicit package reference instead of relying on the library to pull it in.

## What Does Not Change

The good news is that most existing call sites continue to work after the package update. The public namespaces remain under `RzR.Extensions.EntityLength`, and the familiar interceptor and fluent extension APIs still behave as expected by default. The current library targets also remain aligned with `net45`, `netstandard1.5`, `netstandard2.0`, and `netstandard2.1`.

That means code like the following should continue to compile without any rewrite beyond the package update:

```csharp
var entity1 = StringInterceptor.ApplyStringMaxAllowedLength(model);

var entity2 = StringInterceptor.ApplyStringMaxAllowedLength(model, true);

var entity3 = StringInterceptor.ApplyStringMaxAllowedLength(
    model,
    new List<string> { nameof(MyEntity.Name) },
    processOnlyAssigned: true);

var entity4 = model.ToSafeStoreStrings(
    new List<PropertyOption>
    {
        new PropertyOption { Name = nameof(MyEntity.Name), UseDots = true }
    },
    processOnlyAssigned: true);
```

## What v3 Adds

The most useful addition in v3 is finer control over trailing spaces when truncation uses a dots suffix. In earlier usage patterns, the default effect was to trim the retained substring before appending `...`. That remains the default in v3, so existing behavior stays stable unless you choose otherwise.

What v3 adds is the ability to preserve those trailing spaces when that detail matters to you. You can do that through the new `forceTrimEnd` overload parameter, or through the new option properties `TrimOption.ApplyForceTrimEnd` and `PropertyOption.ApplyForceTrimEnd`.

Here is the overload-based version:

```csharp
var result = StringInterceptor.ApplyStringMaxAllowedLength(
    model,
    useDotOnEnd: true,
    truncateType: StringTruncateType.AtTheEndOf,
    forceTrimEnd: false);
```

And here is the same behavior expressed through `TrimOption`:

```csharp
var result = StringInterceptor.ApplyStringMaxAllowedLength(
    model,
    new TrimOption
    {
        UseDots = true,
        TruncateType = StringTruncateType.AtTheEndOf,
        ApplyForceTrimEnd = false
    });
```

If you need that control on a property-by-property basis, you can use `PropertyOption` as well:

```csharp
var result = model.ToSafeStoreStrings(
    new List<PropertyOption>
    {
        new PropertyOption
        {
            Name = nameof(MyEntity.Name),
            UseDots = true,
            ApplyForceTrimEnd = false
        }
    });
```

In practical terms, this means a value can now keep the space before the suffix when you want it to. For example, with a max length of `6`, `ApplyForceTrimEnd = true` produces `ab...`, while `ApplyForceTrimEnd = false` produces `ab ...`.

v3 also behaves more predictably when the configured max length is extremely small and dots are enabled. Instead of producing an invalid result, it keeps the final value within the configured limit. For example, a max length of `1` becomes `.`, `2` becomes `..`, and `3` becomes `...`. If you have tests around very small limits, it is worth re-running them after the upgrade.

## Recommended Upgrade Path

In practice, the migration usually comes down to four checks: update the package reference, update any old assembly-name-based integrations, add `CodeSource` explicitly only if your code truly depends on it, and then rerun the tests that cover truncation with dots or whitespace-sensitive input.

If those tests pass, you can keep using the library exactly as before. The new whitespace-preservation controls are optional. You only need to opt in to `forceTrimEnd: false` or `ApplyForceTrimEnd = false` in places where preserving trailing spaces is actually part of the expected output.

## Bottom Line

For most projects, this is a light migration. The package name changes, the assembly name changes, but the day-to-day API usage stays familiar. Update the reference, verify any assembly-based integration points, and then decide whether you want the new trailing-space behavior. If not, the default v3 behavior stays close to what v2 callers already expect.
