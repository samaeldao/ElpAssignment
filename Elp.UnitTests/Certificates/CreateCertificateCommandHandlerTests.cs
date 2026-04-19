using Elp.Application.Certificates.Commands.CreateCertificate;
using Elp.Application.Common.Interfaces;
using Elp.Domain.Entities;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace Elp.UnitTests.Certificates;

public class CreateCertificateCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly CreateCertificateCommandHandler _handler;

    public CreateCertificateCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();

        _mockContext.Setup(c => c.DriverFitnessCertificates)
                    .ReturnsDbSet(new List<DriverFitnessCertificate>());

        _handler = new CreateCertificateCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddCertificateToDb_AndReturnNewId()
    {
        var command = new CreateCertificateCommand(
            "NEW-999-888",
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-1),
            "VYDANO"
        );

        var resultId = await _handler.Handle(command, CancellationToken.None);

        resultId.Should().NotBeEmpty();

        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}