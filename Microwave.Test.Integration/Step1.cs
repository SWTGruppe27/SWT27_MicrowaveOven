using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step1
    {
        private ICookController _sut;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _fakeTimer;
        private IOutput _fakeOutput;
        private IUserInterface _fakeUserInterface;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);
            _fakeTimer = Substitute.For<ITimer>();
            _fakeUserInterface = Substitute.For<IUserInterface>();
            _sut = new CookController(_fakeTimer, _display, _powerTube, _fakeUserInterface);
        }

        [Test]
        public void CookContoller_DisplayCorrectNumber_PowerCorrect()
        {
            _sut.StartCooking(50,60);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("50")));
        }

        [Test]
        public void CookContoller_DisplayCorrectNumber_TimeCorrect()
        {
            _fakeTimer.TimeRemaining.Returns(600);

            _sut.StartCooking(50, 0);

            _fakeTimer.TimerTick += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("10:00")));
        }

        [Test]
        public void CookContollerTimeExpired_DisplayPowerTubeTurnOf_Correct()
        {
            _sut.StartCooking(50, 0);

            _fakeTimer.Expired += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("powertube turned off")));
        }

        [Test]
        public void CookContollerStop_DisplayPowerTubeTurnOf_Correct()
        {
            _sut.StartCooking(50,50);
            _sut.Stop();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("powertube turned off")));
        }

        [Test]
        public void CookContoller_TimerStart_CalledCorrect()
        {
            _sut.StartCooking(50, 60);

            _fakeTimer.Received(1).Start(60);
        }

        [Test]
        public void CookContollerStop_TimerStop_CalledCorrect()
        {
            _sut.Stop();

            _fakeTimer.Received(1).Stop();
        }

        //UserInterface

    }
}