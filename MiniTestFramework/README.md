# MiniTest Framework

Lightweight C# unit test framework + runner. Built for a university course on C#

## Components
- **MiniTest Library**:  
  Attributes (`[TestClass]`, `[TestMethod]`, `[DataRow]`) + assertions (`Assert.AreEqual()`, `IsTrue()`, etc).
- **MiniTestRunner**:  
  Discovers/runs tests from DLLs.

## How To Run It
1. **Write Tests**:
```csharp
[TestClass]
public class MathTests {
    [TestMethod]
    [DataRow(new object[] { 4, 2 + 2 })]
    public void AddTest(int expected, int actual) {
        Assert.AreEqual(expected, actual);
    }
}
```

2. **Make Sure** to include reference to MiniTest in your tests project's `.csproj`.

3. **Run Tests**:
```bash
MiniTestRunner ./YourTests.dll
```

## Key Features
- Parameterized tests via `[DataRow]`
- Priority-based execution (`[Priority(1)]`)
- Setup/teardown methods (`[BeforeEach]`, `[AfterEach]`)
- Dynamic assembly loading/unloading







