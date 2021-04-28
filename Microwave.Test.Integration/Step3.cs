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
    public class Step3
    {
        private UserInterface _sut;
        private CookController _cookController;
        private Display _display;
        private Light _light;
        private PowerTube _powerTube;
        private ITimer _fakeTimer;
        private IOutput _fakeOutput;
        private IButton _fakePowerButton;
        private IButton _fakeTimerButton;
        private IButton _fakeStartCancelButton;
        private IDoor _fakeDoor;

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
            _fakeDoor = Substitute.For<IDoor>();
            _light = new Light(_fakeOutput);

            _cookController = new CookController(_fakeTimer, _display, _powerTube);
            _sut = new UserInterface(_fakePowerButton, _fakeTimerButton, _fakeStartCancelButton, _fakeDoor, _display, _light,
                _cookController);
            _cookController.UI = _sut;
        }

        //Display
        [Test]
        public void UserInterface_DisplayOnPowerPressed_ShowPower50()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 50 w")));
        }

        [Test]
        public void UserInterface_DisplayOnPowerPressed_ShowPower100()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 100 w")));
        }

        [Test]
        public void UserInterface_DisplayOnPowerPressed_ShowPowerResetTo50()
        {
            for (int i = 0; i < 15; i++)
            {
                _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            }

            _fakeOutput.Received(2).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 50 w")));
        }

        [Test]
        public void UserInterface_DisplayOnTimerPressed_ShowTime()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 01:00")));
        }

        [TestCase(1, 0, 1)]
        [TestCase(2, 0,2)]
        [TestCase(10,1,0)]
        [TestCase(60, 6, 0)]
        [TestCase(61, 6, 1)]
        [TestCase(100, 10, 0)]
        public void UserInterface_DisplayOnTimerPressed_ShowTimeDisplay(int numberPressed, int tenthMinNumber, int minNumber)
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            for (int i = 0; i < numberPressed; i++)
            {
                _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            }

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($": {tenthMinNumber}{minNumber}:00")));
        }

        [Test]
        public void UserInterface_DisplayOnStartCancelPressed_StartDisplayCleared()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s=>s.ToLower().Contains($"display cleared")));
        }

        [Test]
        public void UserInterface_DisplayOnStartCancelPressed_CancelDisplayCleared()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"display cleared")));
        }

        [Test]
        public void UserInterface_DisplaySetPower_DisplayCleared()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"display cleared")));
        }

        [Test]
        public void UserInterface_DisplaySetTimer_OpenDoorDisplayCleared()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"display cleared")));
        }

        [Test]
        public void UserInterface_DisplayCooking_OpenDoorDisplayCleared()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"display cleared")));
        }

        [Test]
        public void UserInterface_DisplayCookingIsDone_DisplayCleared()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeTimer.Expired += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"display cleared")));
        }

        //Light
        [Test]
        public void UserInterface_LightSetTimer_LightOn()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned on")));
        }

        [Test]
        public void UserInterface_LightCooking_LightOff()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned off")));
        }

        [Test]
        public void UserInterface_LightReadyStateDoorOpened_LightOn()
        {
            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned on")));
        }

        [Test]
        public void UserInterface_LightSetPowerDoorOpened_LightOn()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned on")));
        }

        [Test]
        public void UserInterface_LightSetTimerDoorOpened_LightOn()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned on")));
        }

        [Test]
        public void UserInterface_LightDoorOpenedAndClosed_LightOff()
        {
            _fakeDoor.Opened += Raise.EventWith(new EventArgs()); 
            _fakeDoor.Closed += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned off")));
        }

        [Test]
        public void UserInterface_LightCookingIsDone_LightOff()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeTimer.Expired += Raise.EventWith(new EventArgs());

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains($"light is turned off")));
        }

        //Cookcontroller
        [Test]
        public void UserInterface_CookingControlSetTimer_StartCooking()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeTimer.Received(1).Start(60);
        }

        [Test]
        public void UserInterface_CookingControlCooking_StopCooking()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeTimer.Received(1).Stop();
        }

        [Test]
        public void UserInterface_CookingControlCookingDoorOpened_StopCooking()
        {
            _fakePowerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeTimerButton.Pressed += Raise.EventWith(new EventArgs());
            _fakeStartCancelButton.Pressed += Raise.EventWith(new EventArgs());

            _fakeDoor.Opened += Raise.EventWith(new EventArgs());

            _fakeTimer.Received(1).Stop();
        }

    }
}
