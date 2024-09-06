// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Reidentification.Core.Brokers.Storages.Blob
{
    public class BlobStorageBroker : IBlobStorageBroker
    {
        public async ValueTask InsertFileAsync(Stream input, string fileName, string container) =>
            throw new NotImplementedException();

        public async ValueTask SelectByFileNameAsync(Stream output, string fileName, string container) =>
            throw new NotImplementedException();

        public async ValueTask DeleteFileAsync(string fileName, string container) =>
            throw new NotImplementedException();

        public async ValueTask<string> GetDownloadLinkAsync(
                string fileName,
                string container,
                DateTimeOffset expiresOn) =>
            throw new NotImplementedException();
    }
}