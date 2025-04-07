using BDGCodingTask.Domain.Entities;

namespace BDGCodingTask.Services.Interfaces
{
    public interface IExchangeDataLoaderService
    {
        public List<Exchange> Exchanges { get; set; }
    }
}
