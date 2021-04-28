using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Step4
    {
        private UserInterface _userInterface;
        private CookController _cookController;
        private Display _display;
        private Light _light;
        private PowerTube _powerTube;
        private ITimer _fakeTimer;
        private IOutput _fakeOutput;
        private IButton _fakePowerButton;
        private IButton _fakeTimerButton;
        private IButton _fakeStartCancelButton;
        private IDoor _sut;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _fakeTimer = Substitute.For<ITimer>();
            _fakePowerButton = Substitute.For<IButton>();
            _fakeTimerButton = Substitute.For<IButton>();
            _fakeStartCancelButton = Substitute.For<IButton>();
            _powerTube = new PowerTube(_fakeOutput);
            _display = new Display(_fakeOutput);
            _light = new Light(_fakeOutput);

            _sut = new Door();
            _cookController = new CookController(_fakeTimer, _display, _powerTube);
            _userInterface = new UserInterface(_fakePowerButton, _fakeTimerButton, _fakeStartCancelButton, _sut,
                _display, _light,
                _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void Door_DoorOpened_LightOn()
        {
            _sut.Open();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s=>s.ToLower().Contains("light is turned on")));
        }

        [Test]
        public void Door_DoorOpenedAndClosed_LightOff()
        {
            _sut.Open();
            _sut.Close();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("light is turned off")));
        }
    }
}
