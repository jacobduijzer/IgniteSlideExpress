# Agent Instructions

## Project Overview

Ignite Slide Express is a .NET Blazor Server application for Ignite-style talks. Users create talks, upload PDF slide decks, and run presentations where slides auto-advance over a configured duration.
Presentating via the Ignite format is simple: you get 5 minutes, with a maximum of 20 slides. Each slide is displayed for 15 seconds (or less), and the presentation auto-advances through the deck. 
The timer starts when the presenter clicks "Start Presentation", and the presentation ends after 5 minutes.

The solution is organized as:

- `src/IgniteSlideExpress.Core`: domain models and services such as `Talk`, `Session`, timers, repositories, and PDF-to-slide conversion.
- `src/IgniteSlideExpress.UI`: Blazor Server UI, view models, file upload handling, layouts, static assets, and app startup.
- `tests/IgniteSlideExpress.UnitTests`: xUnit tests for Core behavior and UI view models.

## Technical Baseline

- Target framework: `.NET 10` (`net10.0`).
- Language version: C# 14.
- Nullable reference types and implicit usings are enabled.
- UI framework: Blazor Server with Razor pages/components.
- Tests: xUnit with Moq and coverlet collector.
- PDF conversion dependency: `PDFtoImage`.
- Docker publishes `IgniteSlideExpress.UI.dll` from the solution.

## Common Commands

Run these from the repository root:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/IgniteSlideExpress.UI/IgniteSlideExpress.UI.csproj
```

To build the container locally:

```bash
docker build -t igniteslideexpress .
```

To run the published image with a custom presentation duration:

```bash
docker run -it --rm -p 3000:8080 -e "DurationInMinutes=1" igniteslideexpress
```

## Coding Guidelines

- Prefer the existing project structure over adding new layers.
- Keep business rules in `IgniteSlideExpress.Core`; keep Blazor-specific behavior in `IgniteSlideExpress.UI`.
- Preserve nullable annotations and avoid introducing nullable warnings.
- Use constructor injection for app services and register dependencies in `src/IgniteSlideExpress.UI/Program.cs`.
- View models inherit from `BaseViewModel`; use `SetValue` for properties that should notify the UI.
- Keep tests close to the behavior being changed, under the matching `tests/IgniteSlideExpress.UnitTests/Core` or `tests/IgniteSlideExpress.UnitTests/UI` folder.
- Do not touch generated/static vendor assets under `wwwroot/css/bootstrap` or `wwwroot/css/open-iconic` unless the task explicitly requires it.
- Avoid broad refactors while making small feature or bug-fix changes.

## Testing Expectations

- Run `dotnet test` after code changes when the SDK is available.
- Add or update xUnit tests for changed domain behavior and view model state changes.
- For UI-only Razor changes, at minimum run `dotnet build`; add tests when behavior moves through a view model or service.
- If tests cannot be run, report the exact command attempted and why it failed.

## Repository Notes

- The sample PDF is stored at `asssets/test-presentation.pdf`; preserve the existing folder name unless a task explicitly includes renaming/migration.
- CI lives in `.github/workflows/dotnet.yml`. Check the workflow before changing build or test assumptions.
- The current source projects target `.NET 10`; keep local changes aligned with the project files.
- Existing user changes may be present. Inspect `git status --short` before edits and do not revert unrelated modifications.
