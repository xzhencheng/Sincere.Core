﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sincere.Core.IServices;

namespace Sincere.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IAdvertisementServices _advertisementServices;
        public ValuesController(IAdvertisementServices advertisementServices)
        {
            _advertisementServices = advertisementServices;
        }

        // GET api/values
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var t = await _advertisementServices.ReadAllAd();

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
