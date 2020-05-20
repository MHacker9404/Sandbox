namespace Marketplace.Domain.Shared
{
    public interface ICurrencyLookup
    {
        CurrencyDetails FindCurrency(string currency);
    }
}
