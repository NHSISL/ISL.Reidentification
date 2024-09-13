using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public interface ILookupService
    {
        ValueTask<Lookup> AddLookupAsync(Lookup lookup);
    }
}