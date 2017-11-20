
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pomodoro.ViewModel;

namespace Pomodoro.UnitTest {
    [TestClass]
    public class PomodoroVmTests {
        [TestMethod]
        public void ValidDurationIsNotOverridden() {
#pragma warning disable IDE0017 // Simplify object initialization
            var vm = new PomodoroVM();
#pragma warning restore IDE0017 // Simplify object initialization
            vm.Duration = "22";
            Assert.AreEqual("22", vm.Duration);
        }

        [TestMethod]
        public void NonValidDurationIsOverriddenByDefaultDuration() {
#pragma warning disable IDE0017 // Simplify object initialization
            var vm = new PomodoroVM();
#pragma warning restore IDE0017 // Simplify object initialization
            vm.Duration = "22a";
            Assert.AreEqual(vm.DefaultDuration, vm.Duration);
        }

        [TestMethod]
        public void ValidIntervalIsNotOverridden() {

#pragma warning disable IDE0017 // Simplify object initialization
            var vm = new PomodoroVM();
#pragma warning restore IDE0017 // Simplify object initialization
            vm.Duration = "5";
            vm.Interval = "4";
           
            Assert.AreEqual("4", vm.Interval);
        }

        [TestMethod]
        public void NonValidIntervalIsOverriddenByDefaultInterval() {
#pragma warning disable IDE0017 // Simplify object initialization
            var vm = new PomodoroVM();
#pragma warning restore IDE0017 // Simplify object initialization
            vm.Interval = "46";
            Assert.AreEqual(vm.DefaultInterval, vm.Interval);
        }
    }
}
