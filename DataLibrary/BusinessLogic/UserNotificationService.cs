﻿using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public class UserNotificationService : IUserNotificationService
    {
        
        IUserNotificationDAL _userNotificaitonDal;
        public UserNotificationService( IUserNotificationDAL userNotificaitonDal, ILogger logger)
        {
            this._userNotificaitonDal = userNotificaitonDal;
            
        }
        


        public async Task<IEnumerable<IUserNotification>> GetUserNotifications(int userID)
        {
            try
            {
                return await _userNotificaitonDal.GetUserNotifications(userID);
            }catch (Exception ex)
            {
                
                return null;
            }
        }

        public async Task InsertDummyNotification()
        {
            await _userNotificaitonDal.InsertDummyNotification();
        }

        
    }
}
