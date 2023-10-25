using Moq;
using Totira.Support.Otp;
using Totira.Support.Otp.DTO;

namespace Test.PropertiesService.RepoMocks
{
    public static class MockOtp
    {
        public static Mock<IOtpHandler> GetIOtpHandlerMock()
        {
            var serviceMock = new Mock<IOtpHandler>();

            var validateLinkOtpDTO = new ValidateLinkOtpDto
            {
                AccessToken = "Bearer dsfsdfsdfsdfgdfgdfsgdfg654fdg4df5g546fd6sg5d6sfgdfsgdfs",
                Email = "xxxxxxxxx@email.com",
                ErrorMessage = string.Empty,
                IsSuccess = true,
                OtpId = Guid.NewGuid(),
            };

            serviceMock.Setup(r => r.ValidateLinkOtpAsync(It.IsAny<Guid>()))
            .ReturnsAsync(validateLinkOtpDTO);

            var validateOtpDTO = new ValidateOtpDto
            {
                EntityId = Guid.NewGuid(),
                EntityKey = Guid.NewGuid(),
                ErrorMessage = string.Empty,
                IsSuccess = true,
                Scope = "scope",
            };

            serviceMock.Setup(r => r.ValidateOtpAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(validateOtpDTO);

            serviceMock.Setup(
                r => r.SetOtpProcessAsync(It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()));

            serviceMock.Setup(r => r.UpdateOtpProcessAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<double>()));

            return serviceMock;
        }
    }
}