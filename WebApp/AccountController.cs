﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    // TODO 4: unauthorized users should receive 401 status code
    // Done.
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize] 
        [HttpGet]
        public ValueTask<Account> Get()
        {
            // TODO 3: Get user id from cookie
            // Done.
            string id = HttpContext.User?.FindFirst(ClaimTypes.Name).Value;
            return _accountService.LoadOrCreateAsync(id);
        }

        // TODO 5: Endpoint should works only for users with "Admin" Role
        // Done.
        [Authorize(Policy = "Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
            => _accountService.GetFromCache(id);

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            // Update account in cache, don't bother saving to DB, this is not an objective of this task.
            // Done.
            var account = await Get();
            account.Counter++;
            _accountService.UpdateAccount(account);
        }
    }
}