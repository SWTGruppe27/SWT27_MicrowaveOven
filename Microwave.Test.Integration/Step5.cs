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
    public class Step5
    {
        private UserInterface _userInterface;
        private CookController _cookController;
        private Display _display;
        private Light _light;
        private PowerTube _powerTube;
        private ITimer _fakeTimer;
        private IOutput _fakeOutput;
        private IButton _sutPowerButton;
        private IButton _sutTimerButton;
        private IButton _sutStartCancelButton;
        private IDoor _door;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _fakeTimer = Substitute.For<ITimer>();
            _sutPowerButton = new Button();
            _sutStartCancelButton = new Button();
            _sutTimerButton = new Button();
            _powerTube = new PowerTube(_fakeOutput);
            _display = new Display(_fakeOutput);
            _light = new Light(_fakeOutput);
            _door = new Door();

            _cookController = new CookController(_fakeTimer, _display, _powerTube);
            _userInterface = new UserInterface(_sutPowerButton, _sutTimerButton, _sutStartCancelButton, _door, _display, _light, _cookController);
            _cookController.UI = _userInterface;
        }

        [Test]
        public void Button_PowerPressed_DisplayShowPower50()
        {
            _sutPowerButton.Press();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 50 w")));
        }

        [Test]
        public void Button_TimePressed_DisplayShowTime0100()
        {
            _sutPowerButton.Press();
            _sutTimerButton.Press();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 01:00")));
        }

        [Test]
        public void Button_StartCancelPressed_DisplayShows50()
        {
            _sutPowerButton.Press();
            _sutTimerButton.Press();
            _sutStartCancelButton.Press();

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains("powertube works with 50")));
        }
    }
}
