using GPOS.Core.Enums;
using GPOS.Infrastructure.UOW;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnappMarketWorker
{
    public class ChangeOrderStatusService : BackgroundService
    {
        private readonly ILogger<ChangeOrderStatusService> _logger;
        private IUnitOfWork? _unitOfWork = null;
        private readonly IServiceProvider _serviceProvider;

        public ChangeOrderStatusService(ILogger<ChangeOrderStatusService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var marketPlaceTypes =await _unitOfWork.MarketPlaceRepository.GetActiveMarketPlaceTypes();
              
                foreach (var marketPlacetype in marketPlaceTypes)
                {
                    switch (marketPlacetype.Id)
                    {
                        case (int)MarketPlaceTypeEnum.SnappMarket:
                            var marketPlaceOrders = await _unitOfWork.MarketPlaceRepository.GetAllActiveMarketPlaceOrders(marketPlacetype.Id);
                            var aa = await _unitOfWork.SnappMarketRepository.ChangeStatusAsync();
                            break;
                        case (int)MarketPlaceTypeEnum.TopMarket:
                            break;
                        case (int)MarketPlaceTypeEnum.DigiKalaJet:
                            break;
                        case (int)MarketPlaceTypeEnum.SabzAfzar:
                            break;
                        case (int)MarketPlaceTypeEnum.SnappFood:
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }
}
