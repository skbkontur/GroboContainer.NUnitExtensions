# Changelog

## v1.0.35 - 2019.11.07
- Make exception thrown in test method visible to user when exception also occurs in TearDown method
  (PR [#4](https://github.com/skbkontur/GroboContainer.NUnitExtensions/pull/4)).

## v1.0.30 - 2019.08.21
- Await asynchronous wrapper methods (PR [#2](https://github.com/skbkontur/GroboContainer.NUnitExtensions/pull/2)).
- Add support of `[Injected]` for properties (PR [#3](https://github.com/skbkontur/GroboContainer.NUnitExtensions/pull/3)).

## v1.0.23 - 2019.08.09
- Add support of `Func<>` factories for `[Injected]` fields (PR [#1](https://github.com/skbkontur/GroboContainer.NUnitExtensions/pull/1)).

## v1.0 - 2018.11.21
- Support NUnit 3.x.
- Support .NET Standard 2.0.
- Switch to SDK-style project format and dotnet core build tooling.
- Use [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning) to automate generation of assembly 
  and nuget package versions.
