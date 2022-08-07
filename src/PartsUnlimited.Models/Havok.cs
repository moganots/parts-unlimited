﻿using System;

namespace PartsUnlimited.Models
{
    public class Havok
    {
     
        public int Id { get; set; }
        public String Name { get; set; }
        public bool HavokEnabled { get; set; }
        public bool isScaledOut { get; set; }
        public string SubscriptionId { get; set; }
        public string resourceGroupName { get; set; }
        public string AppServiceName { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }


    }
}
