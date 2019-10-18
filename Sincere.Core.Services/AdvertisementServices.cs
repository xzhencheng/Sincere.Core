using Sincere.Core.IRepository;
using Sincere.Core.IRepository.Base;
using Sincere.Core.IServices;
using Sincere.Core.Model.Models;
using Sincere.Core.Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sincere.Core.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        IAdvertisementRepository _advertisementRepository;
        public AdvertisementServices(IBaseRepository<Advertisement> baseRepository) {
            base.BaseDal = baseRepository;
            _advertisementRepository = baseRepository as IAdvertisementRepository;
        }
        public Task<List<Advertisement>> ReadAllAd() {

           return BaseDal.GetList();
        }
    }
}
