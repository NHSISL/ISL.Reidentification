// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.ReIdentification.Core.Models.Brokers.Notifications
{
    public class NotificationConfigurations
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ApprovalRequestTemplateId { get; set; } = string.Empty;
        public string ApprovedRequestTemplateId { get; set; } = string.Empty;
        public string DeniedRequestTemplateId { get; set; } = string.Empty;
    }
}
