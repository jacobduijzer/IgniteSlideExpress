[![.NET](https://github.com/jacobduijzer/IgniteSlideExpress/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jacobduijzer/IgniteSlideExpress/actions/workflows/dotnet.yml)

# Ignite Slide Express

Ignite Slide Express is intended to be used for Ignite talks: talks where you have a maximum of 20 slides, and 5 minutes. The slides will auto-advance, no time to stop. It is used for conferences like the DevOpsDays. 

With Ignite Slide Express, you can create talks, upload PDF's and start the presentations. 

## Getting started

Run the project as a docker container:

```bash
docker run -it --rm -p 3000:8080 jacobduijzer/igniteslideexpress:latest
```

If you want to use another duration for the presentation, you can use an environmental variable when running the docker image. For example, the next command will run with a duration of 1 minute:
```bash
docker run -it --rm -p 3000:8080 jacobduijzer/igniteslideexpress:latest -e "DurationInMinutes=1"
```

Open a browser and go to [http://localhost:3000](http://localhost:3000) to open the application.

## Sources

* [Ignite (Wikipedia)](https://en.wikipedia.org/wiki/Ignite_(event))
* [Blazor File Upload](https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-8.0)
