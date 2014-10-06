using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using VideoStreamer.Net.Configuration;
using VideoStreamer.Net.Storage;

namespace VideoStreamer.Net
{
    public class InterceptModule : IHttpModule
    {
        private List<IStorage> _storages;
        private VideoStreamingConfigSection _config;

        public void Init(HttpApplication context)
        {
            _config = (VideoStreamingConfigSection)ConfigurationManager.GetSection("VideoStreamer");
            _storages = new List<IStorage>();
            if (_config == null)
                throw new ConfigurationErrorsException("VideoStreamer.Net isn't configured properly");

            if (!_config.Storages.Any())
                throw new ConfigurationErrorsException("VideoStreamer.Net isn't configured properly. No storages defined. Please setup at least one storage in Web.config");

            foreach (StorageTypeElement storageConfig in _config.Storages)
            {
                var storage = (IStorage)Activator.CreateInstance(Type.GetType(storageConfig.Type));
                storage.Config = storageConfig;
                storage.ValidateConfig(storageConfig);

                _storages.Add(storage);
            }

            context.PostAuthorizeRequest -= ContextOnPostAuthorizeRequest;
            context.PostAuthorizeRequest += ContextOnPostAuthorizeRequest;
        }

        private void ContextOnPostAuthorizeRequest(object sender, EventArgs eventArgs)
        {
            HttpApplication httpApplication = sender as HttpApplication;
            if (httpApplication == null || httpApplication.Context == null)
                return;

            string rawUrl = httpApplication.Request.RawUrl.ToLower();
            var storage = _storages.FirstOrDefault(x => rawUrl.StartsWith(x.Config.Prefix));
            if (storage == null)
                return;


            var prefix = storage.Config.Prefix.Trim();
            if (!prefix.EndsWith("/"))
                prefix += "/";

            var fileName = httpApplication.Request.RawUrl.Substring(prefix.Length - 1);
            RequestHandler handler = new RequestHandler(storage);
            handler.Handle(httpApplication.Context, fileName);
        }

        public void Dispose()
        {
        }
    }
}
