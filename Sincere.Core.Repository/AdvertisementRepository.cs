using Sincere.Core.IRepository;
using Sincere.Core.Model.EFCore;
using Sincere.Core.Model.Models;
using Sincere.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sincere.Core.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IBaseContext mydbcontext) : base(mydbcontext)
        {
            
        }
    }
}
