# infinite-pie-section
Simple but very useful tool for infinite rotation of rolling windows / pie sections in games. C#

![Pie demo](pie_demo.gif "Pie demo")

As you can see in this gif in the circle (this is from [Badabroom](https://totemastudio.itch.io/badabroom), a gamejam game friends and I made), we needed a way to be able to revolve around a circle infinitely but with finite values (0 to 360), and we also needed a way to tell that when the value goes from 359 to 1, it's actually just moving up by 2, and not -358.


Pie sections with infinite cycle support is very useful for game development. This class can be used in unity easily.

Run tests using `dotnet tests`

Project test structure created using this guide: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test

