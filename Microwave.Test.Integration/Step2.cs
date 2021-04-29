using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    public class Step2
    {
        private ICookController _sut;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _fakeOutput;

        [SetUp]
        public void Setup()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _display = new Display(_fakeOutput);
            _powerTube = new PowerTube(_fakeOutput);
            _timer = new Timer();

            _sut = new CookController(_timer, _display, _powerTube);
        }

        [Test]
        public void CookController_TimerSet_Correct()
        {
            _sut.StartCooking(50,60);

            Assert.That(_timer.TimeRemaining,Is.EqualTo(60));
        }

        [Test]
        public void CookController_DisplayShowTime_Correct1sec()
        {
            _sut.StartCooking(50, 60);

            Thread.Sleep(1100);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 00:59")));
        }

        [Test]
        public void CookController_DisplayShowTime_Correct2Sec()
        {
            _sut.StartCooking(50, 60);

            Thread.Sleep(2100);

            _fakeOutput.Received(1).OutputLine(Arg.Is<string>(s => s.ToLower().Contains(": 00:58")));
        }
    }
}
