﻿/**
 * Copyright 2018 The Nakama Authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Nakama.Tests.Socket
{
    using System;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class WebSocketMatchmakerTest
    {
        private IClient _client;
        private ISocket _socket;

        // ReSharper disable RedundantArgumentDefaultValue
        [SetUp]
        public void SetUp()
        {
            _client = new Client("defaultkey", "127.0.0.1", 7350, false);
            _socket = _client.CreateWebSocket();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _socket.DisconnectAsync(false);
        }

        [Test]
        public async Task ShouldJoinMatchmaker()
        {
            var session = await _client.AuthenticateCustomAsync($"{Guid.NewGuid()}");
            await _socket.ConnectAsync(session);
            var matchmakerTicket = await _socket.AddMatchmakerAsync("*");

            Assert.NotNull(matchmakerTicket);
            Assert.IsNotEmpty(matchmakerTicket.Ticket);
        }

        [Test]
        public async Task ShouldJoinAndLeaveMatchmaker()
        {
            var session = await _client.AuthenticateCustomAsync($"{Guid.NewGuid()}");
            await _socket.ConnectAsync(session);
            var matchmakerTicket = await _socket.AddMatchmakerAsync("*");

            Assert.NotNull(matchmakerTicket);
            Assert.IsNotEmpty(matchmakerTicket.Ticket);
            Assert.DoesNotThrowAsync(() => _socket.RemoveMatchmakerAsync(matchmakerTicket));
        }
    }
}
