using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevTreks.Models;

namespace DevTreks.Extensions
{
    /// <summary>
    ///Purpose:		The ExtensionService class is adapted from the Service class in DevTreks.
    ///Author:		www.devtreks.org
    ///Date:		2011, September
    ///References:	www.devtreks.org/helptreks/linkedviews/help/linkedview/HelpFile/148
    ///</summary>
    public class ExtensionService
    {
        public ExtensionService() 
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.Amount = 0;
            this.Status = string.Empty;
            this.StatusLastChangedDate = DateTime.Now;
            this.AuthorizationLevel = string.Empty;
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now;
            this.LastChangedDate = DateTime.Now;
            this.IsOwner = false;
            this.AccountId = 0;
            this.ServiceId = 0;
            //base table (service being subscribed to or owned)
            this.Label = string.Empty;
            this.ServiceName = string.Empty;
            this.Description = string.Empty;
            this.Price = 0;
            this.Unit = string.Empty;
            this.ServiceGroupId = 100;
            this.NetworkId = 0;
            this.OwningClubId = 0;
            this.SubscribedClubs = null;
            this.NetworkCategories = null;
            this.MiscDocPath = string.Empty;
        }
        public ExtensionService(ExtensionService serviceCopy)
        {
            this.Id = serviceCopy.Id;
            this.Name = serviceCopy.Name;
            this.Amount = serviceCopy.Amount;
            this.Status = serviceCopy.Status.ToString();
            this.StatusLastChangedDate = serviceCopy.StatusLastChangedDate;
            this.AuthorizationLevel = serviceCopy.AuthorizationLevel.ToString();
            this.StartDate = serviceCopy.StartDate;
            this.EndDate = serviceCopy.EndDate;
            this.LastChangedDate = serviceCopy.LastChangedDate;
            this.IsOwner = serviceCopy.IsOwner;
            this.AccountId = serviceCopy.AccountId;
            this.ServiceId = serviceCopy.ServiceId;
            //base table (service being subscribed to or owned)
            this.Label = serviceCopy.Label;
            this.ServiceName = serviceCopy.ServiceName;
            this.Description = serviceCopy.Description;
            this.Price = serviceCopy.Price;
            this.Unit = serviceCopy.Unit;
            this.ServiceGroupId = serviceCopy.ServiceGroupId;
            this.NetworkId = serviceCopy.NetworkId;
            this.OwningClubId = serviceCopy.OwningClubId;
            this.MiscDocPath = string.Empty;
        }
        public ExtensionService(AccountToService serviceCopy)
        {
            this.Id = serviceCopy.PKId;
            this.Name = serviceCopy.Name;
            this.Amount = serviceCopy.Amount1;
            this.Status = serviceCopy.Status.ToString();
            this.StatusLastChangedDate = serviceCopy.StatusDate;
            this.AuthorizationLevel = serviceCopy.AuthorizationLevel.ToString();
            this.StartDate = serviceCopy.StartDate;
            this.EndDate = serviceCopy.EndDate;
            this.LastChangedDate = serviceCopy.LastChangedDate;
            this.IsOwner = serviceCopy.IsOwner;
            this.AccountId = serviceCopy.AccountId;
            this.ServiceId = serviceCopy.ServiceId;
            //base table (service being subscribed to or owned)
            this.Label = serviceCopy.Service.ServiceNum;
            this.ServiceName = serviceCopy.Service.ServiceName;
            this.Description = serviceCopy.Service.ServiceDesc;
            this.Price = serviceCopy.Service.ServicePrice1;
            this.Unit = serviceCopy.Service.ServiceUnit1;
            this.ServiceGroupId = serviceCopy.Service.ServiceClassId;
            this.NetworkId = serviceCopy.Service.NetworkId;
            this.OwningClubId = serviceCopy.OwningClubId;
            this.MiscDocPath = string.Empty;
        }
        public static AccountToService GetService(ExtensionService serviceCopy)
        {
            AccountToService newService = new AccountToService();
            newService.PKId = serviceCopy.Id;
            newService.Name = serviceCopy.Name;
            newService.Amount1 = serviceCopy.Amount;
            newService.Status 
                = Data.AppHelpers.Agreement.SERVICE_STATUS_TYPES.current.ToString();
            newService.StatusDate = serviceCopy.StatusLastChangedDate;
            newService.AuthorizationLevel 
                = (int) AccountHelper.AUTHORIZATION_LEVELS.none;
            newService.StartDate = serviceCopy.StartDate;
            newService.EndDate = serviceCopy.EndDate;
            newService.LastChangedDate = serviceCopy.LastChangedDate;
            newService.IsOwner = serviceCopy.IsOwner;
            newService.AccountId = serviceCopy.AccountId;
            newService.ServiceId = serviceCopy.ServiceId;
            //base table (service being subscribed to or owned)
            newService.Service.ServiceNum = serviceCopy.Label;
            newService.Service.ServiceName = serviceCopy.ServiceName;
            newService.Service.ServiceDesc = serviceCopy.Description;
            newService.Service.ServicePrice1 = serviceCopy.Price;
            newService.Service.ServiceUnit1 = serviceCopy.Unit;
            newService.Service.ServiceClassId = serviceCopy.ServiceGroupId;
            newService.Service.NetworkId = serviceCopy.NetworkId;
            newService.OwningClubId = serviceCopy.OwningClubId;
            //newService.Service.MiscDocPath = string.Empty;
            return newService;
        }
        //join table
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
        public DateTime StatusLastChangedDate { get; set; }
        public string AuthorizationLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public bool IsOwner { get; set; }
        public int AccountId { get; set; }
        public int ServiceId { get; set; }
        public string Label { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public int NetworkId { get; set; }
        public int ServiceGroupId { get; set; }
        public bool IsSelected { get; set; }
        //owning club accountid
        public int OwningClubId { get; set; }
        //clubs that have subscriptions to this service
        public IList<ExtensionClub> SubscribedClubs { get; set; }
        //categories used to classify the data associated with this service
        //(filtered by network and subapptype i.e. crops and inputs)
        public IList<ExtensionContentURI> NetworkCategories { get; set; }
        //stylesheets display network categories using a stateful xml doc
        public string MiscDocPath { get; set; }
    }
}
