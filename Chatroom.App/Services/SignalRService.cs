﻿using Chatroom.App.Contracts;
using Chatroom.App.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatroom.App.Services
{
    public class SignalRService : Hub
    {
        private readonly IMessageProcessorService _messageProcessor;

        private readonly IBotService _botService;

        /// <summary>
        /// The messages cache
        /// </summary>
        private static readonly List<Message> Cache = new List<Message>();

        public SignalRService(IMessageProcessorService messageProcessor, IBotService botService)
        {
            _messageProcessor = messageProcessor;
            _botService = botService;
        }

        /// <summary>
        /// Initial method called after connected to the Hub
        /// </summary>
        public async Task<List<Message>> Connected()
        {
            return Cache;
        }

        public async Task SendMessage(string user, string message)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd, hh:mm:ss");
            var messageType = _messageProcessor.GetMessageType(message);

            if (messageType == MessageType.Command)
            {
                var command = _messageProcessor.GetStockFromCommand(message);
                var userId = Context.ConnectionId;
                await _botService.GetData(userId, command);
            }
            else
            {
                // This is a message for the audience
                // Add new message to the cache
                Cache.Add(new Message { Owner = user, TextMessage = message, Time = time });

                // Controls the messages cache not growing over 50 elements
                if (Cache.Count > 50)
                {
                    Cache.RemoveAt(0);
                }

                await Clients.All.SendAsync("ReceiveMessageFromServer", user, message, messageType.ToString(), time);
            }
        }
    }
}