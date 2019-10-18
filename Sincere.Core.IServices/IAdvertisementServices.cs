using Sincere.Core.IServices.Base;
using Sincere.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sincere.Core.IServices
{
    public interface IAdvertisementServices : IBaseServices<Advertisement>
    {
        Task<List<Advertisement>> ReadAllAd();
    }
}