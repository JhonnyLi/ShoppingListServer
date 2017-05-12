using ShoppingWeb.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.Interface
{
    public interface IIdentityOperations
    {
        void SignInUser(SyncIdentityUser user);
        void SignOutUser(SyncIdentityUser user);

        List<SyncIdentityRole> GetAllRoles();
        SyncIdentityUser GetUser(string name);
        bool UserNameExists(string name);
        bool UserEmailExists(string email);
        SyncIdentityUser CreateUser(SyncIdentityUser user);
    }
}
