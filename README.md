[![.NET](https://github.com/jacobduijzer/IgniteSlideExpress/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jacobduijzer/IgniteSlideExpress/actions/workflows/dotnet.yml)

# Ignite Slide Express

Ignite Slide Express is built for Ignite talks: 5-minute presentations with up to 20 slides. Slides auto-advance, so there is no time to pause. It is used for conferences like DevOpsDays.

With Ignite Slide Express, you can create talks, upload PDFs, and start presentations.

## Getting started

Run the project locally with the .NET CLI:

```bash
dotnet run --project src/IgniteSlideExpress.UI/IgniteSlideExpress.UI.csproj
```

To run locally with another presentation duration:

```bash
DurationInMinutes=1 dotnet run --project src/IgniteSlideExpress.UI/IgniteSlideExpress.UI.csproj
```

To run locally and import PDF presentations at startup, point `PresentationsFolder` at a folder containing files named `speaker-title.pdf`:

```bash
PresentationsFolder="$(pwd)/presentations" dotnet run --project src/IgniteSlideExpress.UI/IgniteSlideExpress.UI.csproj
```

You can combine settings:

```bash
DurationInMinutes=1 PresentationsFolder="$(pwd)/presentations" dotnet run --project src/IgniteSlideExpress.UI/IgniteSlideExpress.UI.csproj
```

Run the project as a docker container:

```bash
docker run -it --rm -p 3000:8080 jacobduijzer/igniteslideexpress:latest
```

If you want to use another duration for the presentation, you can use an environment variable when running the Docker image. For example, the next command will run with a duration of 1 minute:
```bash
docker run -it --rm -p 3000:8080 -e "DurationInMinutes=1" jacobduijzer/igniteslideexpress:latest
```

To import a folder of PDF presentations at startup, mount the folder into the container and set `PresentationsFolder`:

```bash
docker run -it --rm \
  -p 3000:8080 \
  -e "PresentationsFolder=/presentations" \
  -v "$(pwd)/presentations:/presentations:ro" \
  jacobduijzer/igniteslideexpress:latest
```

Each `*.pdf` in the folder is converted into slides and added as a talk. Use the filename format `speaker-title.pdf`; for example, `Jane Doe-Continuous Delivery.pdf` imports speaker `Jane Doe` with title `Continuous Delivery`. The importer splits on the first hyphen, so titles may contain additional hyphens. If a filename does not match the format, the filename without extension is used as the speaker and the title is set to `not available`.

You can also use Docker Compose:

```bash
docker compose up --build
```

The included `docker-compose.yml` mounts `./presentations` into the container and imports PDFs from there.

Open a browser and go to [http://localhost:3000](http://localhost:3000) to open the application.

## Sources

* [Ignite (Wikipedia)](https://en.wikipedia.org/wiki/Ignite_(event))
* [Blazor File Upload](https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-10.0)
