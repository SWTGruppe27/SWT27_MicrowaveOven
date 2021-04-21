using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Tests
    {
        private ICookController _sut;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _fakeTimer;
        private IOutput _fakeOutput;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);
            _fakeTimer = Substitute.For<ITimer>();
            _sut = new CookController(_fakeTimer, _display, _powerTube);
        }

        [Test]
        public void CookContoller_CorrectNumber_PowerCorrect()
        {
            _sut.StartCooking(50,60);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("50")));
        }

        [Test]
        public void CookContoller_CorrectNumber_TimeInTimerCorrect()
        {
            _sut.StartCooking(50, 10000);

            _fakeTimer.TimerTick += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("10")));
        }

        [Test]
        public void CookContoller_TurnOF_Correct()
        {
            //_sut.StartCooking(50, 10000);

            _fakeTimer.Expired += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("powerTube turned off")));
        }


    }
}