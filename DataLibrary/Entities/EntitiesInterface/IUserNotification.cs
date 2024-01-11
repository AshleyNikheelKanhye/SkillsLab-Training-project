using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entities.EntitiesInterface
{
    public interface IUserNotification
    {
        int NotificationID { get; set; }
        int UserID { get; set; }
        string Title { get; set; }
        string MessageBody {  get; set; }
        DateTime MessageDate { get; set; }


    }
}
