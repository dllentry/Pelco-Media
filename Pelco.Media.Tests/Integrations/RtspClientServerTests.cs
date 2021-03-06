﻿//
// Copyright (c) 2018 Pelco. All rights reserved.
//
// This file contains trade secrets of Pelco.  No part may be reproduced or
// transmitted in any form by any means or for any purpose without the express
// written permission of Pelco.
//
using Pelco.Media.RTSP;
using Pelco.Media.Tests.Integrations.Handlers;
using System.Threading;
using Xunit;

namespace Pelco.Media.Tests.Integrations
{
    public class RtspClientServerTests : IClassFixture<RtspCommunicationFixture>
    {
        private static readonly string URI_PATH = "/test";

        private RtspCommunicationFixture _fixture;

        public RtspClientServerTests(RtspCommunicationFixture fixture)
        {
            _fixture = fixture;
            _fixture.Initialize(URI_PATH, new TestRequestHandler());
        }

        [Fact]
        public void TestOptionsCall()
        {
            var request = RtspRequest.CreateBuilder()
                                     .Uri(_fixture.ServerUriEndpoint)
                                     .Method(RtspRequest.RtspMethod.OPTIONS)
                                     .Build();

            var response = _fixture.Client.Send(request);

            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("OPTIONS, DESCRIBE, GET_PARAMETER, SETUP, PLAY, TEARDOWN", response.Headers[RtspHeaders.Names.PUBLIC]);
            
            // Test IRtspInvoker method
            response = _fixture.Client.Request().Options();
            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("OPTIONS, DESCRIBE, GET_PARAMETER, SETUP, PLAY, TEARDOWN", response.Headers[RtspHeaders.Names.PUBLIC]);
        }
        
        [Fact]
        public void TestAyncOptionsCall()
        {
            var e = new ManualResetEvent(false);

            _fixture.Client.Request().OptionsAsync((response) =>
            {
                Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
                Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
                Assert.Equal("OPTIONS, DESCRIBE, GET_PARAMETER, SETUP, PLAY, TEARDOWN", response.Headers[RtspHeaders.Names.PUBLIC]);

                e.Set();
            });

            Assert.True(e.WaitOne(5000));
        }

        [Fact]
        public void TestGetParamaterCall()
        {
            var request = RtspRequest.CreateBuilder()
                                     .Uri(_fixture.ServerUriEndpoint)
                                     .Method(RtspRequest.RtspMethod.GET_PARAMETER)
                                     .AddHeader(RtspHeaders.Names.SESSION, "test-id")
                                     .Build();

            var response = _fixture.Client.Send(request);

            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("GET_PARAMATER", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);
            Assert.Equal("test-id", response.Headers[RtspHeaders.Names.SESSION]);

            // Test IRtspInvokder method
            response = _fixture.Client.Request().Session("test-id").GetParameter();
            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("GET_PARAMATER", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);
            Assert.Equal("test-id", response.Headers[RtspHeaders.Names.SESSION]);
        }

        [Fact]
        public void TestAyncGetParameterCall()
        {
            var e = new ManualResetEvent(false);

            _fixture.Client.Request().GetParameterAsync((response) =>
            {
                Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
                Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
                Assert.Equal("GET_PARAMATER", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

                e.Set();
            });

            Assert.True(e.WaitOne(5000));
        }

        [Fact]
        public void TestPlayCall()
        {
            var request = RtspRequest.CreateBuilder()
                                     .Uri(_fixture.ServerUriEndpoint)
                                     .Method(RtspRequest.RtspMethod.PLAY)
                                     .Build();

            var response = _fixture.Client.Send(request);

            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("PLAY", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

            // Test IRtspInvoker method.
            response = _fixture.Client.Request().Play();
            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("PLAY", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);
        }

        [Fact]
        public void TestAyncPlayCall()
        {
            var e = new ManualResetEvent(false);

            _fixture.Client.Request().PlayAsync((response) =>
            {
                Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
                Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
                Assert.Equal("PLAY", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

                e.Set();
            });

            Assert.True(e.WaitOne(5000));
        }

        [Fact]
        public void TestSetupCall()
        {
            var request = RtspRequest.CreateBuilder()
                                     .Uri(_fixture.ServerUriEndpoint)
                                     .Method(RtspRequest.RtspMethod.SETUP)
                                     .Build();

            var response = _fixture.Client.Send(request);

            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("SETUP", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

            // Test IRtspInvoker method.
            response = _fixture.Client.Request().SetUp();
            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("SETUP", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);
        }

        [Fact]
        public void TestAyncSetUpCall()
        {
            var e = new ManualResetEvent(false);

            _fixture.Client.Request().SetUpAsync((response) =>
            {
                Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
                Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
                Assert.Equal("SETUP", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

                e.Set();
            });

            Assert.True(e.WaitOne(5000));
        }

        [Fact]
        public void TestTeardownCall()
        {
            var request = RtspRequest.CreateBuilder()
                                     .Uri(_fixture.ServerUriEndpoint)
                                     .Method(RtspRequest.RtspMethod.TEARDOWN)
                                     .Build();

            var response = _fixture.Client.Send(request);

            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("TEARDOWN", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

            response = _fixture.Client.Request().TearDown();
            Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
            Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
            Assert.Equal("TEARDOWN", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);
        }

        [Fact]
        public void TestTearPlayCall()
        {
            var e = new ManualResetEvent(false);

            _fixture.Client.Request().TeardownAsync((response) =>
            {
                Assert.True(response.ResponseStatus.Is(RtspResponse.Status.Ok));
                Assert.Equal(_fixture.NextCseq().ToString(), response.Headers[RtspHeaders.Names.CSEQ]);
                Assert.Equal("TEARDOWN", response.Headers[TestRequestHandler.CALLED_METHOD_HEADER]);

                e.Set();
            });

            Assert.True(e.WaitOne(5000));
        }
    }
}
