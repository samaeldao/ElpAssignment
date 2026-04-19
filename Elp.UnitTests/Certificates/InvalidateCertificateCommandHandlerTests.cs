using Elp.Application.Certificates.Commands.InvalidateCertificateCommand;
using Elp.Application.Common.Interfaces;
using Elp.Domain.Constants;
using Elp.Domain.Entities;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;

namespace Elp.UnitTests.Certificates;

public class InvalidateCertificateCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly InvalidateCertificateCommandHandler _handler;

    public InvalidateCertificateCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _handler = new InvalidateCertificateCommandHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_GivenValidId_ShouldInvalidateCertificateAndReturnTrue()
    {
        var certificateId = Guid.NewGuid();
        var certificate = DriverFitnessCertificate.Create(
            "ABC-123",
            Guid.NewGuid(),
            DateTime.UtcNow,
            "VYDANO",
            null);

        _mockContext.Setup(c => c.DriverFitnessCertificates)
                    .ReturnsDbSet(new List<DriverFitnessCertificate> { certificate });

        _mockContext.Setup(c => c.DriverFitnessCertificates.FindAsync(new object[] { certificateId }, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(certificate);

        var command = new InvalidateCertificateCommand(certificateId, null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        certificate.StatusCode.Should().Be(CertificateStatusCodes.Invalidated);

        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldReturnFalse()
    {
        var invalidId = Guid.NewGuid();

        _mockContext.Setup(c => c.DriverFitnessCertificates.FindAsync(new object[] { invalidId }, It.IsAny<CancellationToken>()))
                    .ReturnsAsync((DriverFitnessCertificate?)null);

        var command = new InvalidateCertificateCommand(invalidId, null);

        bool result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();

        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}