// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.Abstractions;

namespace ISL.ReIdentification.Core.Brokers.Notifications
{
    public class NotificationBroker : INotificationBroker
    {
        private readonly INotificationAbstractionProvider notificationAbstractionProvider;

        public NotificationBroker(INotificationAbstractionProvider notificationAbstractionProvider)
        {
            this.notificationAbstractionProvider = notificationAbstractionProvider;
        }

        /// <summary>
        /// Sends an email to the specified email address with the specified
        /// subject, body and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public async ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation) =>
                await this.notificationAbstractionProvider.SendEmailAsync(toEmail, subject, body, personalisation);

        /// <summary>
        /// Sends a SMS using the specified template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent SMS.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public async ValueTask<string> SendSmsAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation) =>
                await this.notificationAbstractionProvider.SendSmsAsync(templateId, personalisation);
    }
}
