using System;
using System.Threading.Tasks;
using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using Marketplace.WebApi.Contracts.V1.UserProfile;
using Serilog;

namespace Marketplace.WebApi.Services
{
    public class UserProfileAppService : IApplicationService
    {
        private readonly CheckTextForProfanity _checkText;
        private readonly IAggregateStore _eventStore;
        private readonly ILogger _logger;

        public UserProfileAppService(IAggregateStore eventStore, CheckTextForProfanity checkText, ILogger logger)
        {
            _eventStore = eventStore;
            _checkText = checkText;
            _logger = logger;
        }

        public async Task HandleAsync(object command)
        {
            _logger.Debug("{@Command}", command);

            switch (command)
            {
                case RegisterUser cmd:
                    await HandleCreate(cmd);
                    break;

                case UpdateUserFullName cmd:
                    await HandleUpdate(cmd.UserId, userProfile => userProfile.UpdateFullName(FullName.FromString(cmd.FullName)));
                    break;

                case UpdateUserDisplayName cmd:
                    await HandleUpdate(cmd.UserId
                                       , async userProfile => userProfile.UpdateDisplayName(DisplayName.FromString(cmd.DisplayName, _checkText)));
                    break;

                case UpdateUserProfilePhoto cmd:
                    await HandleUpdate(cmd.UserId, userProfile => userProfile.UpdateProfilePhoto(cmd.PhotoUrl));
                    break;

                default:
                    throw new InvalidOperationException($"Command {command.GetType()} not known");
            }
        }

        private async Task HandleCreate(RegisterUser cmd)
        {
            if (await _eventStore.ExistsAsync<UserProfile, UserId>(UserId.FromGuid(cmd.UserId)))
            {
                throw new InvalidOperationException($"Entity with id {cmd.UserId} already exists");
            }

            var userProfile = new UserProfile(UserId.FromGuid(cmd.UserId)
                                              , FullName.FromString(cmd.FullName)
                                              , DisplayName.FromString(cmd.DisplayName, _checkText));
            await _eventStore.SaveAsync<UserProfile, UserId>(userProfile);
        }

        private async Task HandleUpdate(Guid id, Action<UserProfile> action) => await this.HandleUpdateAsync(_eventStore, UserId.FromGuid(id), action);
    }
}