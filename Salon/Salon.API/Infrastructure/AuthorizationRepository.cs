using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Salon.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Salon.API.Infrastructure
{
    public class AuthorizationRepository : IDisposable
    {
        /*
        UserManager -> UserStore -> DataContext
        */
        private UserManager<SalonUser> _userManager;
        private SalonDataContext _salonDataContext;

        public AuthorizationRepository()
        {
            _salonDataContext = new SalonDataContext();
            var userStore = new UserStore<SalonUser>(_salonDataContext);
            _userManager = new UserManager<SalonUser>(userStore);
        }
        public async Task<IdentityResult> RegisterUser(RegistrationModel model)
        {
            var user = new SalonUser
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                SalonName = model.SalonName,
                Name = model.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<SalonUser> FindUser(string username, string password)
        {
            return await _userManager.FindAsync(username, password);
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}