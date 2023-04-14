using DeliveryAgreagatorApplication.Auth.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Auth.Common.Interfaces
{
    public interface IProfileService
    {
        public Task<ProfileDTO> GetProfile(Guid userId);
        public Task ChangeProfile(Guid userId, ProfileDTO model);
        public Task UpdatePassword(Guid userId, PasswordUpdateDTO model);
    }
}
