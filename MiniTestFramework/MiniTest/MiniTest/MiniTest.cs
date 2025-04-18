using System;

namespace MiniTest
{
    // 1. TestClassAttribute: Marks a class as a container for test methods.
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute
    {
    }

    // 2. TestMethodAttribute: Marks a method as a unit test to be executed.
    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodAttribute : Attribute
    {
    }

    // 3. BeforeEachAttribute: Defines a method to be executed before each test method.
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeEachAttribute : Attribute
    {
    }

    // 4. AfterEachAttribute: Defines a method to be executed after each test method.
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterEachAttribute : Attribute
    {
    }

    // 5. PriorityAttribute: Sets a priority (integer) for test prioritization.
    [AttributeUsage(AttributeTargets.Method)]
    public class PriorityAttribute : Attribute
    {
        public int Priority { get; }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }
    }

    // 6. DataRowAttribute: Enables parameterized testing by supplying data to test methods.
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DataRowAttribute : Attribute
    {
        public object[] Data { get; }
        public string? Description { get; set; }

        // Constructor for an object array
        public DataRowAttribute(object[] data)
        {
            Data = data;
        }

        // Constructor for a single object
        public DataRowAttribute(object data)
        {
            Data = new object[] { data }; // Wrap the single object in an array
        }
    }

    // 7. DescriptionAttribute: Allows inclusion of additional description for tests.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }

    // Custom exception for failed assertions
    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }

    public static class Assert
    {
        // 1. ThrowsException<TException>: Verifies that a specific exception type is thrown during an action.
        public static void ThrowsException<TException>(Action action, string message = "") where TException : Exception
        {
            try
            {
                action();
                throw new AssertionException($"Expected exception type:<{typeof(TException)}>. Actual exception type:<none>. {message}");
            }
            catch (TException)
            {
                // Exception was thrown as expected, no further action required
            }
            catch (Exception ex)
            {
                throw new AssertionException($"Expected exception type:<{typeof(TException)}>. Actual exception type:<{ex.GetType()}>. {message}");
            }
        }

        // 2. AreEqual<T>: Verifies equality between expected and actual values.
        public static void AreEqual<T>(T? expected, T? actual, string message = "")
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                throw new AssertionException($"Expected: {expected?.ToString() ?? "null"}. Actual: {actual?.ToString() ?? "null"}. {message}");
            }
        }

        // 3. AreNotEqual<T>: Ensures that expected and actual values are distinct.
        public static void AreNotEqual<T>(T? notExpected, T? actual, string message = "")
        {
            if (EqualityComparer<T>.Default.Equals(notExpected, actual))
            {
                throw new AssertionException($"Expected any value except: {notExpected?.ToString() ?? "null"}. Actual: {actual?.ToString() ?? "null"}. {message}");
            }
        }

        // 4. IsTrue: Confirms that a boolean condition is true.
        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                throw new AssertionException($"Condition is false. {message}");
            }
        }

        // 5. IsFalse: Confirms that a boolean condition is false.
        public static void IsFalse(bool condition, string message = "")
        {
            if (condition)
            {
                throw new AssertionException($"Condition is true. {message}");
            }
        }

        // 6. Fail: Explicitly fails a test with a custom error message.
        public static void Fail(string message = "")
        {
            throw new AssertionException($"Test failed. {message}");
        }
    }
}

