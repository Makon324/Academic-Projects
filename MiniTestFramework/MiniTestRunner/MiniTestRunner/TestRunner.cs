using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MiniTest;

namespace MiniTestRunner
{
    public class TestRunner
    {
        // Method for executing tests in a given assembly
        public void ExecuteTests(Assembly assembly)
        {
            Console.WriteLine($"Executing tests in assembly: {assembly.FullName}");
            var testClasses = DiscoverTestClasses(assembly);
            int totalTests = 0, passedTests = 0, failedTests = 0;

            // Looping through test classes
            foreach (var testClass in testClasses)
            {
                // Executing tests in current test class
                Console.Write($"Running tests from class: {testClass.Type}...");
                int classTests = 0, classPassed = 0, classFailed = 0;

                var instance = Activator.CreateInstance(testClass.Type);
                if (instance == null) // if instance could not be created (no parameterless constructor)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nWarning: Skipping - {testClass.Type.Name} - no parameterless constructor");
                    Console.ResetColor();
                    continue;
                }

                foreach (var testMethod in testClass.TestMethods.OrderBy(m => m.Priority).ThenBy(m => m.Method.Name))
                {
                    Console.Write("\n" + testMethod.Method.Name.PadRight(60));
                    foreach (var dataRow in testMethod.DataRows)
                    {
                        if (dataRow != null)
                        {
                            // Print the description of the data row
                            Console.Write($"\n - {dataRow.Description}".PadRight(61));
                        }
                        try
                        {
                            // Execute BeforeEach
                            testClass.BeforeEach?.Invoke(instance, null);

                            // Execute Test Method
                            if (dataRow == null)
                                testMethod.Method.Invoke(instance, null);
                            else if (!ValidateParameters(testMethod.Method, dataRow)) // skipping method if argumetns and data is mismathed
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($"\nWarning: Skipping - Datarow - arguments mismath");
                                Console.ResetColor();
                                continue;
                            }
                            else
                                testMethod.Method.Invoke(instance, new object[] { dataRow.Data[0] });

                            // Execute AfterEach
                            testClass.AfterEach?.Invoke(instance, null);

                            // If code came to this moment test is a success
                            Console.Write(": ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("PASSED");
                            Console.ResetColor();
                            classPassed++;
                            passedTests++;
                        }
                        catch (Exception ex) // If exception test failed
                        {
                            Console.Write(": ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("FAILED");
                            Console.Write("\n");
                            Console.Write(ex.InnerException?.Message ?? ex.Message);
                            Console.ResetColor();
                            classFailed++;
                            failedTests++;
                        }
                        classTests++;
                        totalTests++;
                    }

                    // Print test method description
                    if (!string.IsNullOrEmpty(testMethod.Description))
                    {
                        Console.Write("\n" + testMethod.Description);
                    }
                }

                // Summary of this test class
                Console.WriteLine($"\n******************************");
                Console.WriteLine($"* Test passed:".PadRight(18) + $"{classPassed} / {classTests}".PadRight(10) + "*");
                Console.WriteLine($"* Failed:".PadRight(18) + $"{ classFailed}".PadRight(10) + "*");
                Console.WriteLine($"******************************");
                Console.WriteLine("################################################################################");
            }

            // Summary of entire assembly
            Console.WriteLine($"Summary of running tests from {assembly.GetName().Name}:");
            Console.WriteLine($"******************************");
            Console.WriteLine($"* Test passed:".PadRight(18) + $"{passedTests} / {totalTests}".PadRight(10) + "*");
            Console.WriteLine($"* Failed:".PadRight(18) + $"{failedTests}".PadRight(10) + "*");
            Console.WriteLine($"******************************");
        }

        // Discovers all test classes within the provided assembly
        private IEnumerable<TestClassInfo> DiscoverTestClasses(Assembly assembly)
        {
            var testClasses = new List<TestClassInfo>();

            // Loop through all test classes in the assembly
            foreach (var type in assembly.GetTypes().Where(t => t.GetCustomAttribute<TestClassAttribute>() != null).ToList())
            {
                // Create a TestClassInfo object for each test class
                testClasses.Add(new TestClassInfo
                {
                    Type = type,
                    BeforeEach = BindMethod(type, typeof(BeforeEachAttribute)),
                    AfterEach = BindMethod(type, typeof(AfterEachAttribute)),
                    TestMethods = DiscoverTestMethods(type)
                });
            }

            return testClasses;
        }

        // Bind method to the specified attribute (BeforeEach, AfterEach)
        private MethodInfo? BindMethod(Type type, Type attributeType)
        {
            return type.GetMethods()
                .FirstOrDefault(m => m.GetCustomAttribute(attributeType) != null && m.GetParameters().Length == 0);
        }

        // Discover all test methods in a given test class
        private List<TestMethodInfo> DiscoverTestMethods(Type type)
        {
            var testMethods = new List<TestMethodInfo>();

            // Loop through all methods in the class
            foreach (var method in type.GetMethods())
            {
                // Check if method has TestMethod attribute
                var testMethodAttribute = method.GetCustomAttribute<TestMethodAttribute>();
                if (testMethodAttribute == null) continue;

                // Get Priority and Description
                var priority = method.GetCustomAttribute<PriorityAttribute>()?.Priority ?? 0;
                var description = method.GetCustomAttribute<MiniTest.DescriptionAttribute>()?.Description ?? "";

                // Get the DataRow attributes (no longer using .Data)
                var dataRows = method.GetCustomAttributes<DataRowAttribute>().ToList();

                // If no DataRow is present, add a null entry for default execution
                if (dataRows.Count == 0)
                    dataRows.Add(null);

                // Create a TestMethodsInfo object for each test method
                testMethods.Add(new TestMethodInfo
                {
                    Method = method,
                    Priority = priority,
                    Description = description,
                    DataRows = dataRows
                });
            }

            return testMethods;
        }

        // Validate if the DataRow matches the method parameters
        private bool ValidateParameters(MethodInfo? method, DataRowAttribute dataRow)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == 0) return dataRow.Data.Length == 0;

            // Check parameter count and type compatibility
            if (dataRow.Data.Length != parameters.Length) return false;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (dataRow.Data[i] != null && !parameters[i].ParameterType.IsAssignableFrom(dataRow.Data[i]?.GetType()))
                    return false;
            }

            return true;
        }
    }

    // Class to hold information about a test class
    public class TestClassInfo
    {
        public Type Type { get; set; }
        public MethodInfo? BeforeEach { get; set; }
        public MethodInfo? AfterEach { get; set; }
        public List<TestMethodInfo> TestMethods { get; set; }
    }

    // Class to hold information about a test method
    public class TestMethodInfo
    {
        public MethodInfo Method { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public List<DataRowAttribute> DataRows { get; set; }
    }
}

