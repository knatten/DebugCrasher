This is a reproduction for the JetBrains Rider issue discussed at https://rider-support.jetbrains.com/hc/en-us/community/posts/115000572890-Debugger-crashes-on-a-certain-solution

In short, this program runs just fine in Mono, but step debugging crashes in Rider.

**I consider this low priority, since it can only be triggered by having stupid, non-working, dead code in your solution.** Anyway, here's what happens:

### Steps to reproduce
- Open and build the solution in Rider
- `mono DebugCrasher/bin/Debug/DebugCrasher.exe` works fine
- Running the `DebugCrasher` project in the Rider debugger works fine
- Place a breakpoint in Library1.Class1.M()
- Running the `DebugCrasher` project in the Rider debugger now crashes with an unhandled loader error and a `SIGABRT`

### What's going on here
There are three projcts in the solution, making three assemblies:

- `DebugCrasher.csproj` -> `DebugCrasher.exe`
- `Library1.csproj` -> `Library1.dll`
- `Library2.csproj` -> `DebugCrasher.exe`

Note that two of the projects create *identically named assemblies*.

`DebugCrasher` calls into `Library1`, which instantiates the type `Class2`, which is defined in `Library2`. However, since the assembly produced by `Library2.csproj` is named `DebugCrasher.exe`, it gets overwritten by the `DebugCrasher.exe` produced by `DebugCrasher.csproj`. This means that when we run the program, the type definition for `Class2` doesn't exist anywhere.

"Luckily" for us though, this instantiation happens in a lambda function that's never called, so Mono lets us get away with it.

However, I suspect Rider attempts to load all the types referenced in a file when stepping into that file, or something like that, which totally makes sense for a debugger to do. This is why it crashes.

### Versions and details

```
Mono JIT compiler version 4.4.2 (mono-4.4.0-branch-c7sr1/f72fe45 Wed Jul 27 16:20:13 EDT 2016)
```

```
Rider 2017.1.1
Build #RD-171.4456.2813, built on August 21, 2017
Licensed to Rider Evaluator
Expiration date: September 16, 2017
JRE: 1.8.0_112-release-736-b22 x86_64
JVM: OpenJDK 64-Bit Server VM by JetBrains s.r.o
Mac OS X 10.12.6
```
