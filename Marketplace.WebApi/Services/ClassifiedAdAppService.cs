using System;
using System.Threading.Tasks;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Marketplace.WebApi.Contracts.V1.ClassifiedAd;
using Serilog;

namespace Marketplace.WebApi.Services
{
    //public class ClassifiedAdAppService : IHandleCommand<CreateAdCommand>
    public class ClassifiedAdAppService : IApplicationService
    {
        private readonly ICurrencyLookup _currencyLookup;
        private readonly IAggregateStore _eventStore;
        private readonly ILogger _logger;

        private readonly IClassifiedAdRepository _repository;
        //private readonly IUnitOfWork _uow;

        //public ClassifiedAdAppService(IClassifiedAdRepository repository, ICurrencyLookup currencyLookup, IUnitOfWork uow, ILogger logger)
        public ClassifiedAdAppService(IAggregateStore eventStore, ICurrencyLookup currencyLookup, ILogger logger)
        {
            //_repository = repository;
            _eventStore = eventStore;
            _currencyLookup = currencyLookup;
            //_uow = uow;
            _logger = logger;
        }

        public async Task HandleAsync(object command)
        {
            _logger.Debug("{@Command}", command);

            switch (command)
            {
                case CreateAdCommand cmd:
                    await HandleCreateAsync(cmd);
                    break;
                case SetTitleCommand cmd:
                    await HandleUpdate(cmd.Id, c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title)));
                    break;
                case UpdateTextCommand cmd:
                    await HandleUpdate(cmd.Id, c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text)));
                    break;
                case UpdatePriceCommand cmd:
                    await HandleUpdate(cmd.Id, c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup)));
                    break;
                case RequestToPublishCommand cmd:
                    await HandleUpdate(cmd.Id, c => c.RequestToPublish());
                    break;
                default:
                    throw new InvalidOperationException($"Command {command.GetType()} not known");
            }
        }

        private async Task HandleCreateAsync(CreateAdCommand cmd)
        {
            //if (await _repository.ExistsAsync(ClassifiedAdId.FromGuid(cmd.Id)))
            //    throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
            //var classifiedAd = new ClassifiedAd(ClassifiedAdId.FromGuid(cmd.Id), UserId.FromGuid(cmd.OwnerId));
            //await _repository.AddAsync(classifiedAd);
            //await _uow.CommitAsync();
            if (await _eventStore.ExistsAsync<ClassifiedAd, ClassifiedAdId>(ClassifiedAdId.FromGuid(cmd.Id)))
            {
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
            }

            var classifiedAd = new ClassifiedAd(ClassifiedAdId.FromGuid(cmd.Id), UserId.FromGuid(cmd.OwnerId));
            await _eventStore.SaveAsync<ClassifiedAd, ClassifiedAdId>(classifiedAd);
        }

        private async Task HandleUpdate(Guid id, Action<ClassifiedAd> action)
        {
            //var classifiedAd = await _repository.LoadAsync(ClassifiedAdId.FromGuid(id));
            //if (classifiedAd == null) throw new InvalidOperationException($"Entity with id {id} does not exist");

            //action(classifiedAd);
            //await _uow.CommitAsync();
            await this.HandleUpdateAsync(_eventStore, ClassifiedAdId.FromGuid(id), action);
        }
    }
}