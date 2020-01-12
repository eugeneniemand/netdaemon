﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using JoySoftware.HomeAssistant.Client;
using JoySoftware.HomeAssistant.NetDaemon.Common;
using Microsoft.Extensions.Logging;

namespace JoySoftware.HomeAssistant.NetDaemon.Daemon
{
    internal interface INetDaemonHost : INetDaemon
    {
        Task Run(string host, short port, bool ssl, string token, CancellationToken cancellationToken);

        Task Stop();
    }

    public class NetDaemonHost : INetDaemonHost
    {
        private readonly IHassClient _hassClient;

        private readonly List<string> _supportedDomainsForTurnOnOff = new List<string>
        {
            "light"
        };

        public NetDaemonHost(IHassClient hassClient, ILoggerFactory? loggerFactory = null)
        {
            loggerFactory ??= DefaultLoggerFactory;
            Logger = loggerFactory.CreateLogger<NetDaemonHost>();
            _hassClient = hassClient;
            Action = new FluentAction(this);
        }

        private static ILoggerFactory DefaultLoggerFactory => LoggerFactory.Create(builder =>
        {
            builder
                .ClearProviders()
//                .AddFilter("HassClient.HassClient", LogLevel.Debug)
                .AddConsole();
        });

        public Task ListenStateAsync(string pattern, Func<StateChangedEvent, Task> action)
        {
            throw new NotImplementedException();
        }

        public async Task TurnOnAsync(string entityId, params (string name, object val)[] attributeNameValuePair)
        {
            var domain = GetDomainFromEntity(entityId);

            // Use default domain "homeassistant" if supported is missing
            domain = GetDomainFromEntity(entityId);

            // Convert the value pairs to dynamic type
            var attributes = attributeNameValuePair.ToDynamic();
            // and add the entity id dynamically
            attributes.entity_id = entityId;

            await _hassClient.CallService(domain, "turn_on", attributes);
        }

        public async Task TurnOffAsync(string entityId, params (string name, object val)[] attributeNameValuePair)
        {
            // Get the domain if supported, else domain is homeassistant
            var domain = GetDomainFromEntity(entityId);

            // Use expando object as all other methods
            var attributes = attributeNameValuePair.ToDynamic();
            // and add the entity id dynamically
            attributes.entity_id = entityId;

            await _hassClient.CallService(domain, "turn_off", attributes);
        }

        public async Task ToggleAsync(string entityId, params (string name, object val)[] attributeNameValuePair)
        {
            // Get the domain if supported, else domain is homeassistant
            var domain = GetDomainFromEntity(entityId);

            // Use expando object as all other methods
            var attributes = attributeNameValuePair.ToDynamic();
            // and add the entity id dynamically
            attributes.entity_id = entityId;

            await _hassClient.CallService(domain, "toggle", attributes);
        }

        public EntityState? GetState(string entity)
        {
            return _hassClient.States.TryGetValue(entity, out var returnValue) ? returnValue.ToDaemonEvent() : null;
        }

        public IAction Action { get; }


        public ILogger Logger { get; }

        /// <summary>
        ///     Runs the Daemon
        /// </summary>
        /// <remarks>
        ///     Connects to Home Assistant and the task completes if canceled or if Home Assistant
        ///     can´t be connected or disconnects.
        /// </remarks>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        /// <param name="token"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Run(string host, short port, bool ssl, string token, CancellationToken cancellationToken)
        {
            if (_hassClient == null)
                throw new NullReferenceException("HassClient cant be null when running daemon, check constructor!");

            // We don´t need to provide cancellation to hass client it has it´s own connect timeout
            var connectResult = await _hassClient.ConnectAsync(host, port, ssl, token, true);

            if (!connectResult)
                return;

            await Task.Delay(500, cancellationToken);
        }

        public async Task Stop()
        {
            if (_hassClient == null)
                throw new NullReferenceException("HassClient cant be null when running daemon, check constructor!");

            await _hassClient.CloseAsync();
        }



        private string GetDomainFromEntity(string entity)
        {
            var entityParts = entity.Split('.');
            if (entityParts.Length != 2)
                throw new ApplicationException($"entity_id is mal formatted {entity}");

            return entityParts[0];
        }
    }
}