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

            _cookController = new CookController(_fakeTimer, _display, _powerTube);
            _sut = new UserInterface(_fakePowerButton, _fakeTimerButton, _fakeStartCancelButton, _fakeDoor, _display, _light,
                _cookController);
        }

        [Test]
        public void 
        {

        }
    }
}
