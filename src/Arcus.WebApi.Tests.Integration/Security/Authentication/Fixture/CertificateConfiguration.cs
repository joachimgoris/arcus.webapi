﻿using System;
using System.Security.Cryptography.X509Certificates;
using Arcus.WebApi.Tests.Integration.Fixture;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Arcus.WebApi.Tests.Integration.Security.Authentication.Fixture
{
    /// <summary>
    /// Security configuration addition to set the TLS client certificate on every call made via the <see cref="TestApiServer"/>.
    /// </summary>
    internal class CertificateConfiguration : IStartupFilter
    {
        private readonly X509Certificate2 _clientCertificate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateConfiguration"/> class.
        /// </summary>
        /// <param name="clientCertificate">The client certificate.</param>
        /// <exception cref="ArgumentNullException">When the <paramref name="clientCertificate"/> is <c>null</c>.</exception>
        public CertificateConfiguration(X509Certificate2 clientCertificate)
        {
            _clientCertificate = clientCertificate ?? throw new ArgumentNullException(nameof(clientCertificate));
        }

        /// <inheritdoc />
        public  Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.Use((context, nxt) =>
                {
                    context.Connection.ClientCertificate = _clientCertificate;
                    return nxt();
                });
                next(builder);
            };
        }
    }
}
