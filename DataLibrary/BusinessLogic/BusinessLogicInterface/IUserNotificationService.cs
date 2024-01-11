using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.BusinessLogicInterface
{
    public interface IUserNotificationService
    {
        Task<IEnumerable<IUserNotification>> GetUserNotifications(int userID);
    }
}
