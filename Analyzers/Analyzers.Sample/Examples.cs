// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace Analyzers.Sample;

// If you don't see warnings, build the Analyzers Project.

public class Examples
{
    public class MyCompanyClass // Try to apply quick fix using the IDE.
    {
    }

    public void ToStars()
    {
        var spaceship = new Spaceship();
        spaceship.SetSpeed(300000000); // Invalid value, it should be highlighted.
        spaceship.SetSpeed(42);
    }

    public IDictionary<int, int> GetData()// Invalid value, it should be highlighted.
    {
        return new Dictionary<int, int>();
    }
    
    public void DoSomething(bool a, bool b) // Invalid method, it should be highlighted.
    {
    }
}