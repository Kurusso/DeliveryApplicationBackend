﻿using DeliveryAgreagatorApplication.Common.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryAgreagatorApplication.Notifications.BL.Services
{
    [Authorize]
    public class NotificationsHub : Hub 
    {
	}
}
