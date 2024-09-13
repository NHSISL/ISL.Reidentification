using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public interface ILookupService
    {
        ValueTask<Lookup> AddLookupAsync(Lookup lookup);
        IQueryable<Lookup> RetrieveAllLookups();
        ValueTask<Lookup> RetrieveLookupByIdAsync(Guid lookupId);
    }
}