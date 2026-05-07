using IgniteSlideExpress.Core;
using IgniteSlideExpress.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace IgniteSlideExpress.UnitTests.UI;

public class StartupPresentationImporterTests
{
    [Fact]
    public async Task ImportsPdfFilesFromConfiguredFolder()
    {
        var originalCurrentDirectory = Environment.CurrentDirectory;
        var workingDirectory = Directory.CreateTempSubdirectory();
        var presentationsFolder = Directory.CreateDirectory(Path.Combine(workingDirectory.FullName, "imports"));
        await File.WriteAllTextAsync(Path.Combine(presentationsFolder.FullName, "Jane Doe-Second Talk.pdf"), "pdf");
        await File.WriteAllTextAsync(Path.Combine(presentationsFolder.FullName, "John Doe-First Talk.pdf"), "pdf");

        try
        {
            Environment.CurrentDirectory = workingDirectory.FullName;
            var addedTalks = new List<Talk>();
            var sessionRepository = new Mock<ISessionRepository>();
            sessionRepository.Setup(x => x.Load()).ReturnsAsync(new Session());
            sessionRepository
                .Setup(x => x.Add(It.IsAny<Talk>()))
                .Callback<Talk>(addedTalks.Add)
                .Returns(Task.CompletedTask);

            var pdf2Slides = new Mock<IPdf2Slides>();
            pdf2Slides
                .Setup(x => x.ProcessPdf(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(20);

            var importer = CreateImporter(
                presentationsFolder.FullName,
                pdf2Slides,
                sessionRepository);

            await importer.Import();

            Assert.Equal(["Second Talk", "First Talk"], addedTalks.Select(talk => talk.Title));
            Assert.Equal(["Jane Doe", "John Doe"], addedTalks.Select(talk => talk.Speaker));
            Assert.All(addedTalks, talk => Assert.Equal(20, talk.NumberOfSlides));
            pdf2Slides.Verify(x => x.ProcessPdf(It.IsAny<Guid>(), It.Is<string>(fileName => fileName.EndsWith(".pdf"))), Times.Exactly(2));
        }
        finally
        {
            Environment.CurrentDirectory = originalCurrentDirectory;
            workingDirectory.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task SkipsTalksAlreadyInSession()
    {
        var originalCurrentDirectory = Environment.CurrentDirectory;
        var workingDirectory = Directory.CreateTempSubdirectory();
        var presentationsFolder = Directory.CreateDirectory(Path.Combine(workingDirectory.FullName, "imports"));
        await File.WriteAllTextAsync(Path.Combine(presentationsFolder.FullName, "Existing Speaker-Existing Talk.pdf"), "pdf");

        try
        {
            Environment.CurrentDirectory = workingDirectory.FullName;
            var session = new Session();
            session.Add(new Talk(Guid.NewGuid(), "Existing Talk", "Existing Speaker", 20));

            var sessionRepository = new Mock<ISessionRepository>();
            sessionRepository.Setup(x => x.Load()).ReturnsAsync(session);

            var pdf2Slides = new Mock<IPdf2Slides>();
            var importer = CreateImporter(
                presentationsFolder.FullName,
                pdf2Slides,
                sessionRepository);

            await importer.Import();

            sessionRepository.Verify(x => x.Add(It.IsAny<Talk>()), Times.Never);
            pdf2Slides.Verify(x => x.ProcessPdf(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
        }
        finally
        {
            Environment.CurrentDirectory = originalCurrentDirectory;
            workingDirectory.Delete(recursive: true);
        }
    }

    [Fact]
    public async Task UsesFilenameAndUnavailableTitleWhenFilenameDoesNotMatchFormat()
    {
        var originalCurrentDirectory = Environment.CurrentDirectory;
        var workingDirectory = Directory.CreateTempSubdirectory();
        var presentationsFolder = Directory.CreateDirectory(Path.Combine(workingDirectory.FullName, "imports"));
        await File.WriteAllTextAsync(Path.Combine(presentationsFolder.FullName, "Malformed.pdf"), "pdf");

        try
        {
            Environment.CurrentDirectory = workingDirectory.FullName;
            Talk? addedTalk = null;
            var sessionRepository = new Mock<ISessionRepository>();
            sessionRepository.Setup(x => x.Load()).ReturnsAsync(new Session());
            sessionRepository
                .Setup(x => x.Add(It.IsAny<Talk>()))
                .Callback<Talk>(talk => addedTalk = talk)
                .Returns(Task.CompletedTask);

            var pdf2Slides = new Mock<IPdf2Slides>();
            pdf2Slides
                .Setup(x => x.ProcessPdf(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(20);

            var importer = CreateImporter(
                presentationsFolder.FullName,
                pdf2Slides,
                sessionRepository);

            await importer.Import();

            Assert.NotNull(addedTalk);
            Assert.Equal("Malformed", addedTalk.Speaker);
            Assert.Equal("not available", addedTalk.Title);
        }
        finally
        {
            Environment.CurrentDirectory = originalCurrentDirectory;
            workingDirectory.Delete(recursive: true);
        }
    }

    private static StartupPresentationImporter CreateImporter(
        string presentationsFolder,
        Mock<IPdf2Slides> pdf2Slides,
        Mock<ISessionRepository> sessionRepository)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["PresentationsFolder"] = presentationsFolder
            })
            .Build();

        return new StartupPresentationImporter(
            configuration,
            pdf2Slides.Object,
            sessionRepository.Object,
            Mock.Of<ILogger<StartupPresentationImporter>>());
    }
}
