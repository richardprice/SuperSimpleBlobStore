using System;
using SuperSimpleBlobStore.Accounts.DataAccess.Common;

namespace SuperSimpleBlobStore.Accounts.DataAccess.UserAccounts
{
    public class TrackingEvent : IEntity
    {
        public int Id { get; set; }
        public Guid TrackingCode { get; set; }
        public Guid SessionId { get; set; }
        public Guid? AuthenticationId { get; set; }
        public string Referrer { get; set; }
        public string Campaign { get; set; }
        public string Group { get; set; }
        public string Provider { get; set; }
        public string Source { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public string QueryString { get; set; }
        public string UserAddress { get; set; }
        public DateTime When { get; set; }
    }
}
