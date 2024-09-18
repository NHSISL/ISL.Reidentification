﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq.Expressions;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Services.Foundations.ReIdentifications;
using LHDS.Core.Brokers.NECS;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationServiceTests
    {
        private readonly Mock<INECSBroker> necsBrokerMock = new Mock<INECSBroker>();
        private readonly Mock<ILoggingBroker> loggingBrokerMock = new Mock<ILoggingBroker>();
        private readonly IReIdentificationService reIdentificationService;

        public ReIdentificationServiceTests()
        {
            this.necsBrokerMock = new Mock<INECSBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.reIdentificationService = new ReIdentificationService(
                necsBroker: this.necsBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static IdentificationRequest CreateRandomIdentificationRequest() =>
            CreateIdentificationRequestFiller().Create();

        private static Filler<IdentificationRequest> CreateIdentificationRequestFiller()
        {
            var filler = new Filler<IdentificationRequest>();

            return filler;
        }
    }
}
