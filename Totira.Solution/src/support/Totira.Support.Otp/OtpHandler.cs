using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.Otp.Configurations;
using Totira.Support.Otp.Domain;
using Totira.Support.Otp.DTO;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Support.Otp
{
    public class Otphandler : IOtpHandler
    {
        private readonly IRepository<TenantOtpProcess, Guid> _tenantOtpProcessRepository;
        private readonly IEncryptionHandler _encryptionHandler;
        private readonly ISecurityHandler _securityHandler;
        private readonly OtpOptions _otpOptions;

        public Otphandler(
            IRepository<TenantOtpProcess, Guid> tenantOtpProcessRepository,
            IEncryptionHandler encryptionHandler,
            ISecurityHandler securityHandler,
            IOptions<OtpOptions> otpOptions
            )
        {
            _tenantOtpProcessRepository = tenantOtpProcessRepository;
            _encryptionHandler = encryptionHandler;
            _securityHandler = securityHandler;
            _otpOptions = otpOptions.Value;
        }

        /// <summary>
        /// Method that is responsible for registering an OTP in the database
        /// </summary>
        /// <param name="otpId">Otp Id</param>
        /// <param name="entityId">entity Id</param>
        /// <param name="entityKey">entity Key</param>
        /// <param name="email">email</param>
        /// <param name="accessCode">Access Code</param>
        /// <param name="expiration">expiration time in minutes</param>
        /// <param name="scope">scope of the Otp code</param>
        /// <param name="encryptAccessCode">True if it has to be encrypted in access code and false if not.</param>
        /// <param name="oneOtp">True if each time an Otp is created with the same information, the previous ones are left invalid.</param>
        /// <returns></returns>
        public async Task SetOtpProcessAsync(
            Guid otpId,
            Guid entityId,
            Guid? entityKey,
            string email,
            string accessCode,
            double expiration,
            string scope,
            bool encryptAccessCode = true,
            bool oneOtp = false)
        {
            string encryptedAccessCode = encryptAccessCode ? _encryptionHandler.EncryptString(accessCode) : accessCode;
            if (oneOtp)
            {
                Expression<Func<TenantOtpProcess, bool>>? expression = null;
                if (entityKey != null)
                {
                    expression = (
                       r => r.EntityId == entityId && r.EntityKey == entityKey && r.Email == email && r.Scope == scope);
                }
                else
                {
                    expression = (
                       r => r.EntityId == entityId && r.Email == email && r.Scope == scope);
                }

                var actualOtp = await _tenantOtpProcessRepository.Get(expression);

                if (actualOtp != null)
                {
                    foreach (var otp in actualOtp)
                    {
                        otp.IsOtpValid = false;
                        await _tenantOtpProcessRepository.Update(otp);
                    }
                }
            }
            await _tenantOtpProcessRepository.Add(new TenantOtpProcess
            {
                Id = otpId,
                Email = email,
                Scope = scope,
                EntityId = entityId,
                EntityKey = entityKey,
                CreatedOn = DateTimeOffset.UtcNow,
                ExpirationDate = DateTimeOffset.UtcNow.AddMinutes(expiration),
                EncryptedAccessCode = encryptedAccessCode,
                AmountUse = 0,
                Retries = 0,
                IsOtpValid = true
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="otpId"></param>
        /// <returns></returns>
        public async Task<ValidateLinkOtpDto> ValidateLinkOtpAsync(Guid otpId)
        {
            var otpInfo = (await _tenantOtpProcessRepository.Get(s => s.Id == otpId)).FirstOrDefault();
            if (otpInfo != null)
            {
                // Otp is not Valid
                if (!otpInfo.IsOtpValid)
                {
                    return new ValidateLinkOtpDto(false, "Link is not valid.");
                }

                if (otpInfo.ExpirationDate < DateTimeOffset.UtcNow)
                {
                    otpInfo.IsOtpValid = false;
                    await _tenantOtpProcessRepository.Update(otpInfo);
                    return new ValidateLinkOtpDto(false, "Link has expired.");
                }
                var accessToken = await _securityHandler.GetAccessTokenAsync();
                return new ValidateLinkOtpDto(otpInfo.Id, otpInfo.Email, accessToken);
            }
            else
            {
                return new ValidateLinkOtpDto(false, "Invalid link. Please try again.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="otpId"></param>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public async Task<ValidateOtpDto> ValidateOtpAsync(Guid otpId, string accessCode)
        {
            var otpInfo = (await _tenantOtpProcessRepository.Get(s => s.Id == otpId)).FirstOrDefault();
            if (otpInfo != null)
            {
                // Otp is not Valid
                if (!otpInfo.IsOtpValid)
                {
                    return new ValidateOtpDto(false, "The accessCode is not valid");
                }
                // Validate Expiration Date
                if (otpInfo.ExpirationDate < DateTimeOffset.UtcNow)
                {
                    otpInfo.IsOtpValid = false;
                    await _tenantOtpProcessRepository.Update(otpInfo);
                    return new ValidateOtpDto(false, "The accessCode has expired");
                }

                // Valdate Retries
                if (otpInfo.Retries >= _otpOptions.Retries)
                {
                    otpInfo.IsOtpValid = false;
                    await _tenantOtpProcessRepository.Update(otpInfo);
                    return new ValidateOtpDto(false, "An exceeded number of login attempts.");
                }

                // Validate Amount Use
                if (otpInfo.AmountUse >= _otpOptions.AmountUse)
                {
                    otpInfo.IsOtpValid = false;
                    await _tenantOtpProcessRepository.Update(otpInfo);
                    return new ValidateOtpDto(false, "The number of login attempts has been exceeded.");
                }
                // Validate Access Code
                string decryptedAccessCode = _encryptionHandler.EncryptString(accessCode);
                if (otpInfo.EncryptedAccessCode == decryptedAccessCode)
                {
                    otpInfo.AmountUse++;
                    await _tenantOtpProcessRepository.Update(otpInfo);
                    return new ValidateOtpDto(otpInfo.EntityId, otpInfo.EntityKey, otpInfo.Scope);
                }
                else
                {
                    otpInfo.Retries++;
                    await _tenantOtpProcessRepository.Update(otpInfo);
                    return new ValidateOtpDto(false, "Invalid credentials. Please try again");
                }
            }
            else
            {
                return new ValidateOtpDto(false, "Invalid credentials. Please try again");
            }
        }

        public async Task UpdateOtpProcessAsync(Guid entityKey, string email, bool isActive, double expiration)
        {
            Expression<Func<TenantOtpProcess, bool>> expression = (o => o.EntityKey == entityKey && o.Email == email);

            var otpInfo = (await _tenantOtpProcessRepository.Get(expression)).FirstOrDefault();
            if (otpInfo != null)
            {
                otpInfo.ExpirationDate = isActive ? DateTimeOffset.UtcNow.AddMinutes(expiration) : DateTimeOffset.UtcNow.AddDays(-1);
                await _tenantOtpProcessRepository.Update(otpInfo);
            }
        }
    }
}