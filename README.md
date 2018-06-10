# Decode JWT

`.NET Core` [global tool][dotnet-global-tools] to decode a `JSON Web Token`.

| Package | Release | Pre-release |
| --- | --- | --- |
| `dotnet-decode-jwt` | [![NuGet][nuget-tool-badge]][nuget-tool-command] | [![MyGet][myget-tool-badge]][myget-tool-command] |

| CI | Status | Platform(s) | Framework(s) |
| --- | --- | --- | --- |
| [AppVeyor][app-veyor] | [![Build Status][app-veyor-shield]][app-veyor] | `Windows` | `netcoreapp2.1.0` |

## Invoke the global tool

```posh
dotnet tool install -g dotnet-decode-jwt
dotnet decode-jwt "SGVsbG8gV29ybGQh"
```

[nuget-tool-badge]: https://img.shields.io/nuget/v/dotnet-decode-jwt.svg?label=NuGet
[nuget-tool-command]: https://www.nuget.org/packages/dotnet-decode-jwt
[myget-tool-badge]: https://img.shields.io/myget/gabrielweyer-pre-release/v/dotnet-decode-jwt.svg?label=MyGet
[myget-tool-command]: https://www.myget.org/feed/gabrielweyer-pre-release/package/nuget/dotnet-decode-jwt
[app-veyor]: https://ci.appveyor.com/project/GabrielWeyer/dotnet-decode-jwt
[app-veyor-shield]: https://ci.appveyor.com/api/projects/status/github/gabrielweyer/dotnet-decode-jwt?branch=master&svg=true
[dotnet-global-tools]: https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools
