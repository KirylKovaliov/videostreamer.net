using System;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using VideoStreamer.Net.Configuration;
using VideoStreamer.Net.Storage;

namespace VideoStreamer.Net
{
    public class InterceptModule : IHttpModule
    {
        private IStorage _storage;
        private HttpApplication _context;
        private VideoStreamingConfigSection _config;

        public void Init(HttpApplication context)
        {
            _context = context;
            _config = (VideoStreamingConfigSection)ConfigurationManager.GetSection("VideoStreamer");
            if (_config == null)
                throw new ConfigurationErrorsException("VideoStreamer.Net isn't configured properly");

            if (string.IsNullOrEmpty(_config.Prefix))
                throw new ConfigurationErrorsException("VideoStreamer.Net configuration error. Prefix is undefined");

            if (_config.Storage == null)
                throw new ConfigurationErrorsException("VideoStreamer.Net Storage type isn't defined");

            if (string.IsNullOrEmpty(_config.Storage.Type))
                throw new ConfigurationErrorsException("VideoStreamer.Net Storage type isn't defined");

            var storageType = Type.GetType(_config.Storage.Type, false, true);
            if (storageType == null)
                throw new ConfigurationErrorsException(string.Format("VideoStreamer.Net configuration error. Unable to resolve storage type '{0}'", _config.Storage.Type));

            _storage = Activator.CreateInstance(storageType) as IStorage;
            if (_storage == null)
                throw new ConfigurationErrorsException(string.Format("VideoStreamer.Net configuration error. Type '{0}' doesn't implement '{1}'", _config.Storage.Type, typeof(IStorage).FullName));

            _storage.SetupConfig(_config.Storage);
            _storage.ValidateConfig(_config.Storage);

            context.PostAuthorizeRequest -= ContextOnPostAuthorizeRequest;
            context.PostAuthorizeRequest += ContextOnPostAuthorizeRequest;
        }

        private void ContextOnPostAuthorizeRequest(object sender, EventArgs eventArgs)
        {
            HttpApplication httpApplication = sender as HttpApplication;
            if (httpApplication == null || httpApplication.Context == null)
                return;

            var prefix = _config.Prefix.ToLower().Trim();
            if (!httpApplication.Request.RawUrl.ToLower().StartsWith(prefix))
                return;


            if (!prefix.EndsWith("/"))
                prefix += "/";

            var fileName = httpApplication.Request.RawUrl.Substring(prefix.Length - 1);
            RequestHandler handler = new RequestHandler(_storage);
            handler.Handle(httpApplication.Context, fileName);
        }

        public void Dispose()
        {
            _context.PostAuthorizeRequest -= ContextOnPostAuthorizeRequest;
        }
    }
}
