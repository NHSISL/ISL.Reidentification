// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.ReIdentification.Core.Brokers.Notifications
{
    internal interface INotificationBroker
    {
        /// <summary>
        /// Sends an email to the specified email address with the specified
        /// subject, body and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation);
    }
}
