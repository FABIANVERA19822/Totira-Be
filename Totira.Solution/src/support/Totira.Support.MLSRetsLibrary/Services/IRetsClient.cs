﻿using Microsoft.Extensions.Logging;
using CrestApps.RetsSdk.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrestApps.RetsSdk.Services
{
    public interface IRetsClient
    {
        Task Connect();
        Task Disconnect();

        Task<SearchResult> Search(SearchRequest request);
        Task<RetsSystem> GetSystemMetadata();
        Task<RetsResourceCollection> GetResourcesMetadata();
        Task<RetsResource> GetResourceMetadata(string resourceId);
        Task<RetsClassCollection> GetClassesMetadata(string resourceId);
        Task<RetsObjectCollection> GetObjectMetadata(string resourceId);
        Task<RetsFieldCollection> GetTableMetadata(string resourceId, string className);

        Task<RetsLookupTypeCollection> GetLookupValues(string resourceId, string lookupName);
        Task<IEnumerable<RetsLookupTypeCollection>> GetLookupValues(string resourceId);
        Task<IEnumerable<FileObject>> GetObjectAlpha(string resource, string type, PhotoIdAlpha id, bool useLocation = false);
        Task<IEnumerable<FileObject>> GetObjectAlpha(string resource, string type, IEnumerable<PhotoIdAlpha> ids, bool useLocation = false);
        Task<IEnumerable<FileObject>> GetObjectAlpha(string resource, string type, IEnumerable<PhotoIdAlpha> ids, int batchSize, bool useLocation = false);

        Task RoundTrip(Func<Task> action);
        Task<TResult> RoundTrip<TResult>(Func<Task<TResult>> action);
    }
}
