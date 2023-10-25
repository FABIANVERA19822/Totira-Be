using System;
using System.ComponentModel.DataAnnotations;
using Amazon.S3.Model;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Totira.Bussiness.PropertiesService.Domain;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Messages;
using Totira.Support.Application.Queries;
using Totira.Bussiness.UserService.Events;

using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands
{
	public class CreateTenantContactLandlordCommandHandler : IMessageHandler<CreateTenantContactLandlordCommand>
    {

        private readonly IRepository<TenantContactLandlord, Guid> _tenantContactLandlordRepository;
        private readonly IRepository<TenantContactInformation, Guid> _tenantContactInfoRepository;

        private readonly ILogger<CreateTenantContactLandlordCommandHandler> _logger;
        private readonly IContextFactory _contextFactory;
        private readonly IMessageService _messageService;
        public CreateTenantContactLandlordCommandHandler(
            IRepository<TenantContactLandlord, Guid> tenantContactLandlordRepository,
            ILogger<CreateTenantContactLandlordCommandHandler> logger,
            IContextFactory contextFactory,
            IMessageService messageService,
            IRepository<TenantContactInformation, Guid> tenantContactInfoRepository

            )
		{
            _tenantContactLandlordRepository = tenantContactLandlordRepository;
            _logger = logger;
            _contextFactory = contextFactory;
            _messageService = messageService;
            _tenantContactInfoRepository = tenantContactInfoRepository;

        }

        public async Task HandleAsync(IContext context, Either<Exception, CreateTenantContactLandlordCommand> command)
        {
            await command.MatchAsync(async cmd => {
                _logger.LogDebug("creating tenant contact landlord for tenant {TenantId}", cmd.TenantId);

                    var tenantContactlandlord = new TenantContactLandlord
                    {
                        Id = Guid.NewGuid(),
                        TenantId = cmd.TenantId,
                        PropertyId = cmd.PropertyId,
                        FirstName = cmd.FirstName,
                        LastName = cmd.LastName,
                        Email = cmd.Email,
                        PhoneNumber = new ContactInformationPhoneNumber(cmd.PhoneNumber.Number, cmd.PhoneNumber.CountryCode),
                        Message = cmd.Message,

                    };

                    await _tenantContactLandlordRepository.Add(tenantContactlandlord);

                if (cmd.TenantId != null)
                {

                    var tenantContactInfo = (await _tenantContactInfoRepository.Get(t=>t.TenantId == cmd.TenantId)).FirstOrDefault();
                    if (tenantContactInfo == null)
                    {
                        var tenantContact = new TenantContactInformation
                        {
                            Id = Guid.NewGuid(),
                            TenantId = cmd.TenantId.Value,
                            HousingStatus = string.Empty,
                            Country = string.Empty,
                            Province = string.Empty,
                            City = string.Empty,
                            ZipCode = string.Empty,
                            StreetAddress = string.Empty,
                            Unit = string.Empty,
                            Email = cmd.Email,
                            PhoneNumber = new ContactInformationPhoneNumber(cmd.PhoneNumber.Number, cmd.PhoneNumber.CountryCode),
                            CreatedOn = DateTimeOffset.Now,
                        };

                        await _tenantContactInfoRepository.Add(tenantContact);
                    }

                }
                var objectEvent = new TenantContactLandlordCreatedEvent(tenantContactlandlord.Id);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);

            }, async ex => {
                var objectEvent = new TenantContactLandlordCreatedEvent(Guid.Empty);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, objectEvent);
                throw ex; });
        }
    }
}

