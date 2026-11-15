using System.Reflection;

namespace Cisco.Api.Test;

/// <summary>
/// Sets properties by name and checks that they read back the same value.
/// </summary>
internal static class PropertyAssert
{
	public static void SetAndRead<T>(params (string Name, object? Value)[] values) where T : new()
	{
		var instance = new T();
		foreach (var (name, value) in values)
		{
			Property(typeof(T), name).SetValue(instance, value);
		}

		AssertValues(instance, values);
	}

	public static void AssertValues(object instance, params (string Name, object? Value)[] values)
	{
		foreach (var (name, expected) in values)
		{
			var actual = Property(instance.GetType(), name).GetValue(instance);
			(ReferenceEquals(actual, expected) || Equals(actual, expected))
				.Should().BeTrue("property {0} should read back the value that was set", name);
		}
	}

	private static PropertyInfo Property(Type type, string name)
		=> type.GetProperty(name) ?? throw new InvalidOperationException($"{type.Name} has no property {name}");
}
