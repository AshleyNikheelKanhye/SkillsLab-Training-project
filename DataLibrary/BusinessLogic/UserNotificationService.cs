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
        ILogger _logger;
        public UserNotificationService( IUserNotificationDAL userNotificaitonDal, ILogger logger)
        {
            this._userNotificaitonDal = userNotificaitonDal;
            this._logger = logger;
            
        }

        public async Task<IEnumerable<IUserNotification>> GetUserNotifications(int userID)
        {
            try
            {
                return await _userNotificaitonDal.GetUserNotifications(userID);
            }catch (Exception ex)
            {
                this._logger.LogError(ex);
                return null;
            }
        }

        public async Task InsertDummyNotification() //testing purposes
        {
            await _userNotificaitonDal.InsertDummyNotification();
        }
    }
}
