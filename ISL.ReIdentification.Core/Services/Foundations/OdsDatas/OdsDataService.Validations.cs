// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.OdsDatas
{
    public partial class OdsDataService : IOdsDataService
    {
        private async ValueTask ValidateOdsDataId(Guid odsDataId) =>
            Validate((Rule: await IsInvalidAsync(odsDataId), Parameter: nameof(OdsData.Id)));

        private static async ValueTask<dynamic> IsInvalidAsync(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is invalid"
        };

        private async static ValueTask ValidateStorageOdsData(OdsData maybeOdsData, Guid odsDataId)
        {
            if (maybeOdsData is null)
            {
                throw new NotFoundOdsDataException(odsDataId);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidOdsDataException =
                new InvalidOdsDataException(
                    message: "Invalid ODS data. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidOdsDataException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidOdsDataException.ThrowIfContainsErrors();
        }
    }
}
