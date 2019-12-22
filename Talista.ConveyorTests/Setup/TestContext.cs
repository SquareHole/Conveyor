using System;
using Talista.Conveyor;

namespace Talista.ConveyorTests.Setup
{
    class TestContext : ConveyorContext
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();
        public int TestTimeout { get; set; } = 10;

        public string TestResult { get; set; }
    }
}